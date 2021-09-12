using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUpManager : MonoBehaviour
{
    public PopUp endGamePanel;

    public static PopUpManager Instance { get; private set; }

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

    public static void showFinalPanel()
    {
        Instance.endGamePanel.open();
    }
}
