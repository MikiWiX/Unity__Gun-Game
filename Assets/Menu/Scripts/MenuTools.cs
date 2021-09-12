using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MenuTools
{
    public static void ChangeTab(GameObject switchPane, string targetName)
    {
        SwitchPane sp = switchPane.GetComponent<SwitchPane>();
        if (sp != null)
        {
            sp.OpenOne(targetName);
        }
    }
    public static void ChangeTab(GameObject switchPane, GameObject target)
    {
        SwitchPane sp = switchPane.GetComponent<SwitchPane>();
        if (sp != null)
        {
            sp.OpenOne(target);
        }
    }
    public static void ChangeTab(GameObject switchPane, int targetIndex)
    {
        SwitchPane sp = switchPane.GetComponent<SwitchPane>();
        if (sp != null)
        {
            sp.OpenOne(targetIndex);
        }
    }
    public static void ChangeTab(string switchPaneName, string targetName)
    {
        GameObject switchPane = GameObject.Find(switchPaneName);
        if (switchPane != null)
        {
            ChangeTab(switchPane, targetName);
        }
    }
    public static void ChangeTab(string switchPaneName, GameObject target)
    {
        GameObject switchPane = GameObject.Find(switchPaneName);
        if (switchPane != null)
        {
            ChangeTab(switchPane, target);
        }
    }
    public static void ChangeTab(string switchPaneName, int targetIndex)
    {
        GameObject switchPane = GameObject.Find(switchPaneName);
        if (switchPane != null)
        {
            ChangeTab(switchPane, targetIndex);
        }
    }
}
