using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    float levelTime; // level time in seconds
    
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

    private void FixedUpdate()
    {
        levelTime += Time.fixedDeltaTime;
    }

    public static float getLevelTime()
    {
        return Instance.levelTime;
    }

    public static void gameOver()
    {
        gameStop();
        PopUpManager.showFinalPanel();
    }

    public static void gameStop()
    {
        Time.timeScale = 0;
    }
    public static void gameStart()
    {
        Time.timeScale = 1;
    }

}
