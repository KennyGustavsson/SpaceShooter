using UnityEngine;

namespace SS
{
    public class RandomSprite : MonoBehaviour
    {
        public Sprite[] sprites;

        private SpriteRenderer _spriteRend;

        private void Awake()
        {
            _spriteRend = GetComponent<SpriteRenderer>();

            int i = Random.Range(0, sprites.Length);
            _spriteRend.sprite = sprites[i];
        }
    }
}
