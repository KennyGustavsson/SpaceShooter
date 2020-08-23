using System.Collections.Generic;
using UnityEngine;

namespace SS
{
    public class ObjectPool : MonoBehaviour
    {
        public static ObjectPool ObjPool;

        [System.Serializable]
        public struct StructPool
        {
            public GameObject objectToPool;
            public int amountToPool;
            public int ID;
        }

        public StructPool[] structPools;
        public Dictionary<int, Queue<GameObject>> poolDictionary;

        void Awake()
        {
            ObjPool = this;
            poolDictionary = new Dictionary<int, Queue<GameObject>>();

            for (int i = 0; i < structPools.Length; i++)
            {
                Queue<GameObject> objectPool = new Queue<GameObject>();
                for (int j = 0; j < structPools[i].amountToPool; j++)
                {
                    GameObject obj = (GameObject)Instantiate(structPools[i].objectToPool);
                    obj.SetActive(false);
                    obj.transform.SetParent(transform);
                    objectPool.Enqueue(obj);
                }
                poolDictionary.Add(structPools[i].ID, objectPool);
            }
        }

        public GameObject GetPooledObject(int ID)
        {
            if (!poolDictionary.ContainsKey(ID))
            {
                return null;
            }

            GameObject obj = poolDictionary[ID].Dequeue();
            poolDictionary[ID].Enqueue(obj);

            obj.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);

            return obj;
        }
    }
}