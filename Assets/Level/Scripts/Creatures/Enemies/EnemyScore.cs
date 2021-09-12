using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Enemy))]
public class EnemyScore : MonoBehaviour, EnemyComponent
{
    public Enemy enemy;

    public void setEnemy(Enemy enemy)
    {
        this.enemy = enemy;
    }

    public string type;
    public bool updateScoreByType;
    public int score;

    private void OnEnable()
    {
        updateScore();
    }
    private void OnValidate()
    {
        updateScore();
    }

    private void updateScore()
    {
        if (updateScoreByType && type.Length > 0)
        {
            score = ScoreManager.getScore(type);
        }
    }
}
