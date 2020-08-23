using System.Collections;
using UnityEngine;

namespace SS
{
    public class SpawningManager : MonoBehaviour
    {
        public static SpawningManager instance;

        public float maxDinstanceFromPlayer = 250;

        [Header("Spawning")]
        public float minDistance = 70f;
        public float maxDistance = 140f;

        [Header("Enemies")]
        public int amountToSpawn;
        public GameObject enemyPrefab;

        [Header("Health Pickups")]
        public int amountHealthToSpawn;
        public GameObject healthPickupPrefab;

        private Transform _player;
        private GameObject[] _enemies;
        private GameObject[] _healthPickups;
        private bool _checkingDistance = true;

        private void Awake()
        {
            if (instance == null) instance = this;
            else Destroy(gameObject);

            _player = GameObject.Find("Player").transform;
        }

        private void Start()
        {
            StartCoroutine(InitialSpawner());
        }

        IEnumerator InitialSpawner()
        {
            yield return new WaitForSeconds(3);

            _enemies = new GameObject[amountToSpawn];
            for (int i = 0; i < amountToSpawn; i++)
            {
                GameObject obj = Instantiate(enemyPrefab);
                obj.SetActive(false);
                float distance = Random.Range(minDistance, maxDistance);
                float angle = Random.Range(-Mathf.PI, Mathf.PI);

                Vector3 spawnPos = _player.position;
                spawnPos += new Vector3(Mathf.Cos(angle) * distance, Mathf.Sin(angle) * distance);
                obj.transform.position = spawnPos;
                obj.SetActive(true);

                var effect = ObjectPool.ObjPool.GetPooledObject(5);
                effect.SetActive(false);
                effect.transform.position = spawnPos;
                effect.SetActive(true);

                _enemies[i] = obj;
                obj.transform.SetParent(transform);

                yield return new WaitForSeconds(Random.Range(0f, 0.5f));
            }

            _healthPickups = new GameObject[amountHealthToSpawn];
            for(int j = 0; j < amountHealthToSpawn; j++)
            {
                GameObject obj = Instantiate(healthPickupPrefab);
                obj.SetActive(false);
                obj.transform.SetParent(transform);
                float distance = Random.Range(minDistance, maxDistance);
                float angle = Random.Range(-Mathf.PI, Mathf.PI);

                _healthPickups[j] = obj;

                Vector3 spawnPos = _player.position;
                spawnPos += new Vector3(Mathf.Cos(angle) * distance, Mathf.Sin(angle) * distance);
                obj.transform.position = spawnPos;
                obj.SetActive(true);

                var effect = ObjectPool.ObjPool.GetPooledObject(5);
                effect.SetActive(false);
                effect.transform.position = spawnPos;
                effect.SetActive(true);
            }

            _checkingDistance = false;
        }

        private void Update()
        {
            if(!_checkingDistance) StartCoroutine(DistanceChecker());
        }

        IEnumerator DistanceChecker()
        {
            _checkingDistance = true;

            for (int i = 0; i < _enemies.Length; i++)
            {
                if (Vector3.Distance(_player.position, _enemies[i].transform.position) > maxDinstanceFromPlayer)
                {
                    Respawn(_enemies[i]);
                }
                yield return new WaitForSeconds(0.5f);
            }

            for (int i = 0; i < _healthPickups.Length; i++)
            {
                if (Vector3.Distance(_player.position, _healthPickups[i].transform.position) > maxDinstanceFromPlayer)
                {
                    Respawn(_healthPickups[i]);
                }
                yield return new WaitForSeconds(0.5f);
            }

            _checkingDistance = false;
        }

        public void Respawn(GameObject obj)
        {
            obj.SetActive(false);
            float distance = Random.Range(minDistance, maxDistance);
            float angle = Random.Range(-Mathf.PI, Mathf.PI);

            Vector3 spawnPos = _player.position;
            spawnPos += new Vector3(Mathf.Cos(angle) * distance, Mathf.Sin(angle) * distance);
            obj.transform.position = spawnPos;
            obj.SetActive(true);

            var effect = ObjectPool.ObjPool.GetPooledObject(5);
            effect.SetActive(false);
            effect.transform.position = spawnPos;
            effect.SetActive(true);
        }
    }
}

