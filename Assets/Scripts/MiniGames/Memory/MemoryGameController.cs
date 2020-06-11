using Moon.Asyncs;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Zenject;
using System;
using System.Collections;

namespace MiniGames.Memory
{
    public class MemoryGameController : MonoBehaviour
    {
        private MemoryGameModel gameModel;

        private RuntimeData runtimeData;

        [Inject(Id = "cards_layout_center")]
        private Transform cardsLayoutCenter;

        [Inject(Id = "game_camera")]
        private Camera camera;

        private void Awake()
        {
            runtimeData = FindObjectOfType<RuntimeData>();
        }

        public AsyncState RunGame(MemoryGameModel gameModel)
        {
            this.gameModel = gameModel;

            // game login entry point
            return Planner.Chain()
                    .AddAction(SetCarts)
                    .AddAwait(AwaitAllCardsSet)
                    .AddAction(StartFindAllPairs)
                    .AddAwait(AwaitFindAllPairs)
                    .AddAwait(AwaitGameComplite)
                ;
        }

        private void AwaitFindAllPairs(AsyncStateInfo state)
        {
            state.IsComplete = runtimeData.CardsSet.Count != 0;
        }

        private void StartFindAllPairs()
        {
            if(runtimeData.CardsSet.Count == 0)
            {
                return;
            }

            Planner.Chain()
                    .AddFunc(TryFindPair)
                     .AddTimeout(0.5f)
                    .AddAction(StartFindAllPairs);
                    ;
        }

        private AsyncState TryFindPair()
        {
            return Planner.Chain()
                    .AddAwait(AwaitCardClick)
                    .AddAwait(AwaitCardClick)
                    .AddTimeout(1)
                    .AddAction(CheckPair)
                ;
        }



        private void SetCarts()
        {
            InitSprites();
            InitCardsLayout();

            StartCoroutine(SetCardsIEnumerator());
        }


        private void AwaitGameComplite(AsyncStateInfo state)
        {
            // todo: game complete condition;
            state.IsComplete = false;
        }

        private void AwaitAllCardsSet(AsyncStateInfo state)
        {
            state.IsComplete = runtimeData.AllCardsSet;
        }

        private void AwaitCardClick(AsyncStateInfo state)
        {
            state.IsComplete = false;

            if (runtimeData.AllCardsSet && Input.GetMouseButtonDown(0))
            {
                RaycastHit hit;
                Ray ray = camera.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit))
                {
                    var card = hit.transform.GetComponent<Card>();

                    if(card != null && !runtimeData.UpFacedCards.Contains(card))
                    {
                        card.FaceUp();
                        runtimeData.UpFacedCards.Add(card);
                        state.IsComplete = true;
                    }
                }
            }
        }

        private void CheckPair()
        {
            bool pairFound = runtimeData.UpFacedCards.Count == 2
                && runtimeData.UpFacedCards[0].SpriteRenderer.sprite
                == runtimeData.UpFacedCards[1].SpriteRenderer.sprite;

            if (pairFound)
            {
                StartCoroutine(RemoveCardIEnumerator(runtimeData.UpFacedCards[0]));
                StartCoroutine(RemoveCardIEnumerator(runtimeData.UpFacedCards[1]));
            }
            else
            {
                runtimeData.UpFacedCards[0].FaceDown();
                runtimeData.UpFacedCards[1].FaceDown();
            }

            runtimeData.UpFacedCards.Clear();
        }

        /// <summary>
        /// Проинициализировать разметку карт 
        /// направление Z соответсвует направлению от первого до последнего столбца
        /// направление X соответсвует направлению от первого до последнего строки
        /// </summary>
        private void InitCardsLayout()
        {
            int cardsCount = runtimeData.CycleSettings.CardsCount;

            // Начало отсчета
            Vector3 pos = cardsLayoutCenter.position;

            int collumns = Mathf.FloorToInt(Mathf.Sqrt(cardsCount * gameModel.LayoutAspectRatio));

            int rows = cardsCount / collumns;

            // Левый верхний угол
            pos -= cardsLayoutCenter.forward * gameModel.ColumnsSpace * ((float)collumns / 2);
            pos -= cardsLayoutCenter.right * gameModel.RowsSpace * ((float)rows / 2);

            var cardsTable = runtimeData.CardsLayoutPositions;

            for (int i = 0; cardsTable.Count < cardsCount; i++)
            {
                for (int j = 0; j < collumns && cardsTable.Count < cardsCount; j++)
                {
                    cardsTable.Add(pos 
                        + cardsLayoutCenter.right * gameModel.RowsSpace * i 
                        + cardsLayoutCenter.forward * gameModel.ColumnsSpace * j);
                }
            }
        }

        private List<Sprite> GetRandomRange(List<Sprite> source, int count)  
        {
            var spritesBuf = new List<Sprite>();

            spritesBuf.AddRange(source);

            var sprites = new List<Sprite>();

            for (int i = 0; i < count && spritesBuf.Count > 0; i++)
            {
                Sprite sprite = spritesBuf[UnityEngine.Random.Range(0, spritesBuf.Count - 1)];
                spritesBuf.Remove(sprite);

                sprites.Add(sprite);
            }

            return sprites;
        }

        private void InitSprites()
        {
            runtimeData.SpritesToSet = new List<Sprite>();

            var differentSprites = GetRandomRange(runtimeData.CycleSettings.Sprites, runtimeData.CycleSettings.DifferentSpritesCount);

            var pairsSprites = GetRandomRange(differentSprites, runtimeData.CycleSettings.PairsCount);
            runtimeData.SpritesToSet.AddRange(pairsSprites);
            runtimeData.SpritesToSet.AddRange(pairsSprites);

            var singleSprites = differentSprites.Where(s => !pairsSprites.Contains(s));
            runtimeData.SpritesToSet.AddRange(singleSprites);
        }

        private IEnumerator SetNextCardIEnumerator()
        {
            // Определить позицию для карты
            Vector3 cardPosition = new Vector3();

            try
            {
                cardPosition = GetNextCardLayoutPosition();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                yield break;
            }

            // Определить спрайт для карты
            var sprite = GetNextRandomSptire();

            if (sprite == null)
            {
                yield break;
            }

            Vector3 effectPos = cardPosition + Vector3.up * 0.1f;

            StartCoroutine(SetFX(gameModel.CardEnterFxPrefab, effectPos));

            yield return new WaitForSeconds(0.33f);

            // Вставить карту
            SetCard(cardPosition, sprite);

            yield return new WaitForSeconds(0.25f);
        }

        private IEnumerator RemoveCardIEnumerator(Card card)
        {
            Vector3 effectPos = card.transform.position + Vector3.up * 0.1f;

            StartCoroutine(SetFX(gameModel.CardEnterFxPrefab, effectPos));

            yield return new WaitForSeconds(0.33f);

            Destroy(card.gameObject);

            yield return new WaitForSeconds(0.25f);
        }

        private IEnumerator SetFX(GameObject prefab, Vector3 position)
        {
            // Вставить эффект
            var effect = Instantiate(prefab, position, Quaternion.identity).GetComponent<ParticleSystem>();

            yield return new WaitForSeconds(2);

            Destroy(effect.gameObject);
        }

        private IEnumerator SetCardsIEnumerator()
        {
            for (int i = 0; i< runtimeData.CycleSettings.CardsCount; i++)
            {
                yield return SetNextCardIEnumerator();
            }

            runtimeData.AllCardsSet = true;
        }

        private void SetCard(Vector3 cardPosition, Sprite sprite)
        {
            Quaternion rotation = runtimeData.CycleSettings.FaceUpOnStart ?
                Quaternion.LookRotation(Vector3.up, -cardsLayoutCenter.right)
                : Quaternion.LookRotation(Vector3.down, -cardsLayoutCenter.right);

            GameObject parent = new GameObject("card_parent");
            parent.transform.position = cardPosition;
            parent.transform.rotation = rotation;
            parent.transform.SetParent(cardsLayoutCenter);

            var card = Instantiate(gameModel.CardPrefab, cardPosition, rotation, parent.transform).GetComponent<Card>();

            card.SetSprite(sprite);

            runtimeData.CardsSet.Add(card);
        }
    
        private Vector3 GetNextCardLayoutPosition()
        {
            if(runtimeData.CardsLayoutPositions.Count < runtimeData.CardsSet.Count + 1)
            {
                throw new Exception("Отсутсвуют свободные позиции для установки карты");
            }

            return runtimeData.CardsLayoutPositions[runtimeData.CardsSet.Count];
        }

        private Sprite GetNextRandomSptire()
        {
            // Выбрать спрайты, которые использованы менее двух раз
            var sprites = runtimeData.SpritesToSet;

            if (sprites.Count == 0)
            {
                return null;
            }

            Sprite sprite = sprites[UnityEngine.Random.Range(0, sprites.Count - 1)];
            sprites.Remove(sprite);

            return sprite;
        }
    }
}
