using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectPooler : MonoBehaviour
{

    private GameObject pooledObject;
    private int poolAmount = 0;
    private bool allowGrow = false;

    private List<GameObject> pooledObjects;
    public List<GameObject> PooledObjects
    {
        get
        {
            return pooledObjects;
        }
    }

    public void initialize(GameObject PooledObject, int PoolAmount, bool AllowGrow)
    {
        pooledObject = PooledObject;
        poolAmount = PoolAmount;
        allowGrow = AllowGrow;

        pooledObjects = new List<GameObject>();

        for (int i = 0; i < poolAmount; i++)
        {
            GameObject obj = Instantiate(pooledObject);
            obj.SetActive(false);
            pooledObjects.Add(obj);
        }
    }

    public GameObject getObject()
    {
        for (int i = 0; i < pooledObjects.Count; i++)
        {
            if (!pooledObjects[i].activeInHierarchy)
            {
                return pooledObjects[i];
            }
        }

        if (allowGrow)
        {
            GameObject obj = Instantiate(pooledObject);
            pooledObjects.Add(obj);
            return obj;
        }

        return null;
    }

}
