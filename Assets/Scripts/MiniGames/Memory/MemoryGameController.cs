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

        private void Awake()
        {
            runtimeData = FindObjectOfType<RuntimeData>();
        }

        public AsyncState RunGame(MemoryGameModel gameModel)
        {
            this.gameModel = gameModel;

            InitSprites();
            InitCardsLayout();

            StartCoroutine(SetCardsIEnumerator());

            // game login entry point
            return Planner.Chain()
                    .AddAwait(AwaitFunc)
                ;
        }

        private void AwaitFunc(AsyncStateInfo state)
        {
            // todo: game complete condition;
            state.IsComplete = false;
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
            pos -= new Vector3(gameModel.RowsSpace * ((float)rows / 2), 0,  gameModel.ColumnsSpace * ((float)collumns / 2));

            var cardsTable = runtimeData.CardsLayoutPositions;

            for (int i = 0; cardsTable.Count < cardsCount; i++)
            {
                for (int j = 0; j < collumns && cardsTable.Count < cardsCount; j++)
                {
                    cardsTable.Add(pos +
                        new Vector3(gameModel.RowsSpace * i, 0, gameModel.ColumnsSpace * j));
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

            yield return new WaitForSeconds(0.4f);

            // Вставить карту
            SetCard(cardPosition, sprite);

            yield return new WaitForSeconds(0.5f);
        }

        private IEnumerator SetFX(GameObject prefab, Vector3 position)
        {
            // Вставить эффект
            var effect = Instantiate(prefab, position, Quaternion.identity).GetComponent<ParticleSystem>();

            yield return new WaitForSeconds(0.4f);

            effect.Stop();

            yield return new WaitForSeconds(3);

            Destroy(effect.gameObject);
        }

        private IEnumerator SetCardsIEnumerator()
        {
            for (int i = 0; i< runtimeData.CycleSettings.CardsCount; i++)
            {
                yield return SetNextCardIEnumerator();
            }
        }

        private void SetCard(Vector3 cardPosition, Sprite sprite)
        {
            Quaternion rotation = runtimeData.CycleSettings.FaceUpOnStart ?
                Quaternion.LookRotation(Vector3.up, Vector3.left)
                : Quaternion.LookRotation(Vector3.down, Vector3.left);

            var card = Instantiate(gameModel.CardPrefab, cardPosition, rotation, cardsLayoutCenter).GetComponent<Card>();

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
