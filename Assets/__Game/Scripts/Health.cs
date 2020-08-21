using UnityEngine;
using UnityEngine.UI;

namespace SS
{
    public class Health : MonoBehaviour
    {
        public int health = 100;
        private Text _healthDisplay;

        private void Awake()
        {
            if(gameObject.layer == 8)
            {
                _healthDisplay = GameObject.Find("HealthDisplay").GetComponent<Text>();
                _healthDisplay.text = $"Health {health}";
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            Debug.Log(collision.relativeVelocity.magnitude);
            Debug.Log((int)collision.relativeVelocity.magnitude);
            if (collision.relativeVelocity.magnitude > 2) HealthChange(-(int)collision.relativeVelocity.magnitude);
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

                this.gameObject.SetActive(false);

                health = 0;
                if(gameObject.layer == 8)
                {
                    Debug.Log("GameOver");
                    GameManager.Instance.RestartGame();
                }
            }
            else if (health > 100) health = 100;

            if(_healthDisplay != null) _healthDisplay.text = $"Health {health}";
        }
    }
}
