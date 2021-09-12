using UnityEngine;
using System;

public class SingletonObjectOfName : MonoBehaviour
{
    public string[] componentNames;
    void Awake()
    {
        foreach (string name in componentNames)
        {
            if (GameObject.Find(name) != null)
            {
                Destroy(gameObject);
            }
        }
    }
}
