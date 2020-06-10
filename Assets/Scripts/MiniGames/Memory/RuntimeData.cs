using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MiniGames.Memory
{
    public class RuntimeData : MonoBehaviour
    {
        [HideInInspector]
        public List<Sprite> SpritesToSet = new List<Sprite>();

        [HideInInspector]
        public List<Sprite> SpritesPairs = new List<Sprite>();

        [HideInInspector]
        public List<Sprite> SpritesSingle = new List<Sprite>();

        [HideInInspector]
        public List<Card> CardsSet = new List<Card>();

        [HideInInspector]
        public int CycleIndex = 0;

        public GameCycleSettings CycleSettings;

        [HideInInspector]
        public List<Vector3> CardsLayoutPositions = new List<Vector3>();

        [HideInInspector]
        public Vector3 LastRelativeCardPosition;
    }
}