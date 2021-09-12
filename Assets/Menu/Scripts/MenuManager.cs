using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public static GameObject miscellaneous;

    public static MenuManager Instance { get; private set; }

    void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There is more than one instance!", gameObject);
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void ChangeTab(GameObject switchPane, string targetName)
    {
        SwitchPane sp = switchPane.GetComponent<SwitchPane>();
        if (sp != null)
        {
            sp.OpenOne(targetName);
        }
    }
    public void ChangeTab(GameObject switchPane, GameObject target)
    {
        SwitchPane sp = switchPane.GetComponent<SwitchPane>();
        if (sp != null)
        {
            sp.OpenOne(target);
        }
    }
    public void ChangeTab(GameObject switchPane, int targetIndex)
    {
        SwitchPane sp = switchPane.GetComponent<SwitchPane>();
        if (sp != null)
        {
            sp.OpenOne(targetIndex);
        }
    }
    public void ChangeTab(string switchPaneName, string targetName)
    {
        GameObject switchPane = GameObject.Find(switchPaneName);
        if (switchPane != null)
        {
            ChangeTab(switchPane, targetName);
        }
    }
    public void ChangeTab(string switchPaneName, GameObject target)
    {
        GameObject switchPane = GameObject.Find(switchPaneName);
        if (switchPane != null)
        {
            ChangeTab(switchPane, target);
        }
    }
    public void ChangeTab(string switchPaneName, int targetIndex)
    {
        GameObject switchPane = GameObject.Find(switchPaneName);
        if (switchPane != null)
        {
            ChangeTab(switchPane, targetIndex);
        }
    }

}
