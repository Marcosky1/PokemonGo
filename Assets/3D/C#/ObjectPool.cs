using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public GameObject objectPrefab;
    public int poolSize = 10;

    private List<GameObject> pool = new List<GameObject>();

    void Start()
    {
        // Inicializar el pool con la cantidad deseada de objetos
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(objectPrefab);
            obj.SetActive(false);
            pool.Add(obj);
        }
    }

    // Obtener un objeto del pool
    public GameObject GetPooledObject()
    {
        foreach (var obj in pool)
        {
            if (!obj.activeInHierarchy)
            {
                obj.SetActive(true);
                return obj;
            }
        }

        // Si no hay objetos disponibles, instanciar uno nuevo
        GameObject newObj = Instantiate(objectPrefab);
        pool.Add(newObj);
        return newObj;
    }

    // Retornar el objeto al pool
    public void ReturnObjectToPool(GameObject obj)
    {
        obj.SetActive(false);
    }
}

