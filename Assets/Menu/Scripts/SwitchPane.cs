using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchPane : MonoBehaviour
{
    public List<GameObject> pages;
    public bool addChildren = true;

    private void OnEnable()
    {
        if (addChildren)
        {
            int count = gameObject.transform.childCount;
            for (int i = 0; i < count; i++)
            {
                pages.Add(gameObject.transform.GetChild(i).gameObject);
            }
        }
        CloseAll();
        Open(0);
    }

    public void OpenOne(string name)
    {
        int openCounter = 0;

        foreach (GameObject elem in pages)
        {
            if (elem.name == name)
            {
                elem.SetActive(true);
                openCounter++;
            }
            else
            {
                elem.SetActive(false);
            }
        }

        if (openCounter == 0)
        {
            Debug.Log("No canvas has been displayed", gameObject);
        }
        if (openCounter >= 2)
        {
            Debug.Log("Multiple canvas with the same name exists, displayed: " + openCounter, gameObject);
        }
    }

    public void OpenOne(int index)
    {
        if (index >= pages.Count || index < 0)
        {
            Debug.Log("Index out of bounds while opening canvas; index = " + index + ", array size =" + pages.Count, gameObject);
        }
        else
        {
            for (int i=0; i<pages.Count; i++)
            {
                if(i == index)
                {
                    pages[i].SetActive(true);
                }
                else
                {
                    pages[i].SetActive(false);
                }
            }
        }
    }
    public void OpenOne(GameObject elem)
    {
        int index = pages.IndexOf(elem);
        if (index >= 0)
        {
            OpenOne(index);
        }
        else
        {
            Debug.Log("GameObject couldn't have been found while swithing Panels", gameObject);
        }
    }

    public void Open(string name)
    {
        int openCounter = 0;

        foreach (GameObject elem in pages)
        {
            if(elem.name == name)
            {
                elem.SetActive(true);
                openCounter++;
            }
        }

        if(openCounter == 0)
        {
            Debug.Log("No canvas has been displayed", gameObject);
        }
        if (openCounter >= 2)
        {
            Debug.Log("Multiple canvas with the same name exists, displayed: "+openCounter , gameObject);
        }
    }

    public void Open(int index)
    {
        if(index >= pages.Count)
        {
            Debug.Log("Index out of bounds while opening canvas; index = " + index + ", array size = " + pages.Count, gameObject);
        }
        else
        {
            pages[index].SetActive(true);
        }
    }

    public void Open(GameObject elem)
    {
        int index = pages.IndexOf(elem);
        if (index >= 0)
        {
            Open(index);
        }
        else
        {
            Debug.Log("GameObject couldn't have been found while swithing Panels", gameObject);
        }
    }

    public void CloseAll()
    {
        foreach (GameObject elem in pages)
        {
            elem.SetActive(false);
        }
    }
}
