using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public GameObject pokeballPrefab;
    public int poolSize = 5;

    public List<GameObject> pool;

    void Start()
    {
        pool = new List<GameObject>();
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(pokeballPrefab);
            obj.SetActive(false);
            pool.Add(obj);
        }
    }

    public GameObject GetPooledObject()
    {
        foreach (GameObject obj in pool)
        {
            if (!obj.activeInHierarchy)
            {
                obj.SetActive(true);
                return obj;
            }
        }

        // Si no hay objetos disponibles, creamos uno nuevo
        GameObject newObj = Instantiate(pokeballPrefab);
        newObj.SetActive(true);
        pool.Add(newObj);
        return newObj;
    }

    public void ReturnObjectToPool(GameObject obj)
    {
        // Reiniciar la posición y la física
        obj.transform.position = Vector3.zero;
        obj.transform.rotation = Quaternion.identity; // Restablecer la rotación

        // Reiniciar la física del objeto si tiene un Rigidbody
        Rigidbody rb = obj.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        // Desactivar el objeto
        obj.SetActive(false);


    }
}

