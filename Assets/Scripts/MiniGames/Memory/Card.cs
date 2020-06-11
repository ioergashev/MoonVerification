using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MiniGames.Memory
{
    public class Card : MonoBehaviour
    {
        [HideInInspector]
        public SpriteRenderer SpriteRenderer;

        public float MaxSpriteWidth = 0.4f;
        public float MaxSpriteHeght = 0.7f;

        private Animator animator;

        public float AspectRatio
        {
            get
            {
                if (MaxSpriteHeght == 0)
                    MaxSpriteHeght = 1;

                return MaxSpriteWidth / MaxSpriteHeght;
            }
        }

        private void Awake()
        {
            SpriteRenderer = GetComponentInChildren<SpriteRenderer>();
            animator = GetComponentInChildren<Animator>();
        }

        public void SetSprite(Sprite sprite)
        {
            SpriteRenderer.sprite = sprite;

            float spriteAspectRatio = sprite.rect.width / sprite.rect.height;

            Vector2 cardSize = new Vector2();

            // if the sprite is more extended in width than the card
            if (spriteAspectRatio > AspectRatio)
            {
                cardSize = new Vector2(MaxSpriteWidth, MaxSpriteWidth / spriteAspectRatio);
            }
            else
            {
                cardSize = new Vector2(MaxSpriteHeght * spriteAspectRatio, MaxSpriteHeght);
            }

            SpriteRenderer.size = cardSize;
        }

        public void FaceUp()
        {
            animator.SetTrigger("face_up_trigger");
        }

        public void FaceDown()
        {
            animator.SetTrigger("face_down_trigger");
        }
    }
}