using System.Collections.Generic;
using UnityEngine;
using System;

namespace MiniGames.Memory
{
    [Serializable]
    public class GameCycleSettings
    {
        public List<Sprite> Sprites = new List<Sprite>();

        public float ShowCardDuration = 2f;

        public int SingleCardsCount = 0;

        public bool FaceUpOnStart = false;

        [SerializeField]
        private int pairsCount = 1;

        public int PairsCount
        {
            get
            {
                return Mathf.Min(Sprites.Count, pairsCount);
            }
        }

        public int CardsCount
        {
            get
            {
                return 2 * PairsCount + SingleCardsCount;
            }
        }

        public int DifferentSpritesCount
        {
            get
            {
                return PairsCount + SingleCardsCount;
            }
        }
    }

    [CreateAssetMenu(menuName = "MiniGames/Memory/MemoryGameModel")]
    public class MemoryGameModel: ScriptableObject
    {
        public List<GameCycleSettings> Cycles = new List<GameCycleSettings>();

        public GameObject CardPrefab;
        public GameObject CardEnterFxPrefab;
        public GameObject CardExitFxPrefab;

        public float ColumnsSpace = 0.1f;
        public float RowsSpace = 0.05f;
        public int LayoutAspectRatio = 2;
    }
}