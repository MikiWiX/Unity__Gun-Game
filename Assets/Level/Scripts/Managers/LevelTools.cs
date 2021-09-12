using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Characters;
using System;

public class LevelTools : MonoBehaviour
{
    public static LevelTools Instance { get; private set; }
    
    void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There is more than one instance of LevelTools!");
            return;
        }

        Instance = this;
    }

    public static T SpawnWithScript<T>(GameObject objct, Vector2 spawnPos, Quaternion rotation) where T : MonoBehaviour
    {
        GameObject localObj = Instantiate(objct, spawnPos, rotation);
        T comp = localObj.AddComponent<T>();
        return comp;
    }
    public static Component SpawnWithScript(GameObject objct, Vector2 spawnPos, Quaternion rotation, Type t)
    {
        GameObject localObj = Instantiate(objct, spawnPos, rotation);
        Component comp = localObj.AddComponent(t);
        return comp;
    }

    public static Component[] SpawnWithScripts(GameObject objct, Vector2 spawnPos, Quaternion rotation, Type[] t)
    {
        GameObject localObj = Instantiate(objct, spawnPos, rotation);
        Component[] comp = new Component[t.Length];
        for (int i=0; i<t.Length; i++)
        {
            comp[i] = localObj.AddComponent(t[i]);
        }
        return comp;
    }

    public static T SpawnChildWithScript<T>(GameObject objct, GameObject parent, Vector2 spawnPos, Quaternion rotation) where T: MonoBehaviour
    {
        GameObject localObj = Instantiate(objct, spawnPos, rotation);
        T comp = localObj.AddComponent<T>();
        localObj.transform.SetParent(parent.transform, false);
        return comp;
    }

    public static bool CollidingTagIs(Collision2D collision, string tag)
    {
        if (collision.collider.gameObject.CompareTag(tag)) //collider.tag == string tag
        {
            return true;
        }
        return false;
    }

    public static List<T> getChildrenWithComponent<T>(GameObject parent)
    {
        List<T> children = new List<T>();
        int childCount = parent.transform.childCount;
        for (int i = 0; i < childCount; i++)
        {
            T tmp = parent.transform.GetChild(i).gameObject.GetComponent<T>();
            if (tmp != null)
            {
                children.Add(tmp);
            }
        }
        return children;
    }

    // Copying an component using reflection API and adding to another GameObject
    // credit: @shaffe and @turbanov at unity forums
    //https://answers.unity.com/questions/458207/copy-a-component-at-runtime.html
    public static T CopyComponent<T>(T original, GameObject destination) where T : Component
     {
         System.Type type = original.GetType();
         var dst = destination.GetComponent(type) as T;
         if (!dst) dst = destination.AddComponent(type) as T;
         var fields = type.GetFields();
         foreach (var field in fields)
         {
             if (field.IsStatic) continue;
             field.SetValue(dst, field.GetValue(original));
         }
         var props = type.GetProperties();
         foreach (var prop in props)
         {
             if (!prop.CanWrite || !prop.CanWrite || prop.Name == "name") continue;
             prop.SetValue(dst, prop.GetValue(original, null), null);
         }
         return dst as T;
    }

    public static int IndexOf<T>(ref T[] array, T target)
    {
        for (int i=0; i<array.Length; i++)
        {
            if(array[i].Equals(target))
            {
                return i;
            }
        }
        return -1;
    }
}