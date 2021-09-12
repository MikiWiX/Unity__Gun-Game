using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ScoreManager : MonoBehaviour
{
    public int score;

    public TMPro.TMP_Text textMesh;
    // Start is called before the first frame update

    public string[] types;
    public int[] killScores;

    public static ScoreManager Instance { get; private set; }

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

    public static int getScore(string type)
    {
        int value = 0;
        if(Instance != null)
        {
            int index = LevelTools.IndexOf<string>(ref Instance.types, type);
            if (index > -1 && index < Instance.killScores.Length)
            {
                value = Instance.killScores[index];
            }
        }
        return value;
    }

    private void Update()
    {
        textMesh.text = score.ToString();
    }

    public static void Increase(int value)
    {
        Instance.score += value;
    }
    public static void Increase(string type)
    {
        Instance.score += getScore(type);
    }

    public static void Decrease(int value)
    {
        Instance.score -= value;
    }
    public static void Decrease(string type)
    {
        Instance.score -= getScore(type);
    }

    public static void Set(int value)
    {
        Instance.score = value;
    }


}
