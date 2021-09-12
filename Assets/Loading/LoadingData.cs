using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingData : MonoBehaviour
{
    public enum LoadComponentType
    {
        MAIN_MENU,
        LEVEL
    }

    private static LoadComponentType toLoadType = LoadComponentType.MAIN_MENU;
    public static LoadComponentType ToLoadType
    {
        get { return toLoadType; }
        set { toLoadType = value; }
    }

    private static int index = 0;
    public static int Index
    {
        get { return index; }
        set { index = value; }
    }

    private static string sceneName = "MainMenu";
    public static string SceneName
    {
        get { return sceneName; }
        set { sceneName = value; }
    }

}
