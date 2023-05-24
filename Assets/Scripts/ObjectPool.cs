using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool Instance;

    public GameObject prefab;
    public Transform parent;
    public int poolSize = 8;
    public bool expandable = true;

    private List<GameObject> objectPool;

    private void Awake()
    {
        Instance = this;

        CreatePool();
    }

    private void CreatePool()
    {
        objectPool = new List<GameObject>();

        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(prefab);
            obj.transform.localScale = Vector3.one;
            obj.transform.SetParent(parent);
            obj.SetActive(false);
            objectPool.Add(obj);
        }
    }

    public GameObject GetObjectFromPool()
    {
        for (int i = 0; i < objectPool.Count; i++)
        {
            if (!objectPool[i].activeInHierarchy)
            {
                objectPool[i].SetActive(true);
                objectPool[i].transform.localScale = Vector3.one;
                return objectPool[i];
            }
        }

        if (expandable)
        {
            GameObject obj = Instantiate(prefab);
            obj.transform.localScale = Vector3.one;
            obj.transform.SetParent(parent);
            obj.SetActive(true);
            objectPool.Add(obj);
            return obj;
        }

        return null;
    }

    public void ReturnObjectToPool(GameObject obj)
    {
        obj.transform.SetParent(parent);
        obj.SetActive(false);
    }
}
