using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace SS
{
    public class Health : MonoBehaviour
    {
        public int health = 100;
        public float collisionThreshold = 5f;

        private Text _healthDisplay;
        private SpriteRenderer _spriteRend;
        private bool _initialized = false;

        private void Awake()
        {
            health = 100;

            if(gameObject.layer == 8)
            {
                _healthDisplay = GameObject.Find("HealthDisplay").GetComponent<Text>();
                _healthDisplay.text = $"Health {health}";
            }

            if(GetComponent<SpriteRenderer>() != null) _spriteRend = GetComponent<SpriteRenderer>();
            else if(GetComponentInChildren<SpriteRenderer>() != null) _spriteRend = GetComponentInChildren<SpriteRenderer>();

            _initialized = true;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.layer == 10 && gameObject.layer == 8) return;
            if (collision.relativeVelocity.magnitude > collisionThreshold) HealthChange(-(int)collision.relativeVelocity.magnitude);
        }

        public void HealthChange(int value)
        {
            if (!_initialized) return;

            health += value;

            // Check if Dead
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
                if (gameObject.layer == 8)
                {
                    Debug.Log("GameOver");
                    GameManager.Instance.RestartGame();
                }
                else 
                {
                    GameManager.Instance.AddScore(10);
                    SpawningManager.instance.Respawn(gameObject);
                    _spriteRend.color = Color.white;
                    health = 100;
                }
            }

            // Check if health is over 100
            else if (health > 100) health = 100;

            // Check if taking damage or healing
            else if (value < 0)
            {
                StartCoroutine(ColorEffect(Color.red));
                SoundManager.Instance.PlayAudioAtLocation(2, transform.position);
            }
            else StartCoroutine(ColorEffect(Color.green));


            if (_healthDisplay != null) _healthDisplay.text = $"Health {health}";
        }

        IEnumerator ColorEffect(Color color)
        {
            _spriteRend.color = color;
            yield return new WaitForSeconds(0.1f);
            _spriteRend.color = Color.white;
        }
    }
}