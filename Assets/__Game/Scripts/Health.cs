using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace SS
{
    public class Health : MonoBehaviour
    {
        public int health = 100;

        private Text _healthDisplay;
        private SpriteRenderer _spriteRend;

        private void Awake()
        {
            if(gameObject.layer == 8)
            {
                _healthDisplay = GameObject.Find("HealthDisplay").GetComponent<Text>();
                _healthDisplay.text = $"Health {health}";
            }

            if(GetComponent<SpriteRenderer>() != null) _spriteRend = GetComponent<SpriteRenderer>();
            else if(GetComponentInChildren<SpriteRenderer>() != null) _spriteRend = GetComponentInChildren<SpriteRenderer>();
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.relativeVelocity.magnitude > 5) HealthChange(-(int)collision.relativeVelocity.magnitude);
        }

        public void HealthChange(int value)
        {
            health += value;
            if (health <= 0)
            {
                var obj = ObjectPool.ObjPool.GetPooledObject(20);
                obj.SetActive(false);
                obj.transform.position = transform.position + transform.up;
                obj.transform.rotation = transform.rotation;
                obj.SetActive(true);

                SoundManager.Instance.PlayAudioAtLocation(3, transform.position);

                this.gameObject.SetActive(false);

                health = 0;
                if(gameObject.layer == 8)
                {
                    Debug.Log("GameOver");
                    GameManager.Instance.RestartGame();
                }
            }
            else if (health > 100) health = 100;
            else if (value < 0)
            {
                StartCoroutine(ColorEffect());
                SoundManager.Instance.PlayAudioAtLocation(2, transform.position);
            }


            if(_healthDisplay != null) _healthDisplay.text = $"Health {health}";
        }

        IEnumerator ColorEffect()
        {
            _spriteRend.color = Color.red;
            yield return new WaitForSeconds(0.1f);
            _spriteRend.color = Color.white;
        }
    }
}
