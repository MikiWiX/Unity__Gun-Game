using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface EnemyComponent
{
    public void setEnemy(Enemy enemy);
}

[RequireComponent(typeof(EnemyMotion))]
[RequireComponent(typeof(EnemyHP))]
[RequireComponent(typeof(EnemyScore))]
public class Enemy : MonoBehaviour
{
    public EnemyMotion enemyMotion;
    public EnemyHP enemyHP;
    public EnemyScore enemyScore;

    void OnValidate()
    {
        updateComponents();
    }
    private void OnEnable()
    {
        updateComponents();
    }
    private void updateComponents()
    {
        enemyMotion = GetComponent<EnemyMotion>();
        enemyMotion.setEnemy(this);
        enemyHP = GetComponent<EnemyHP>();
        enemyHP.setEnemy(this);
        enemyScore = GetComponent<EnemyScore>();
        enemyScore.setEnemy(this);
    }
}
