using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Weapons;

[RequireComponent(typeof(Enemy))]
public class EnemyHP : MonoBehaviour, EnemyComponent
{
    public Enemy enemy;

    public void setEnemy(Enemy enemy)
    {
        this.enemy = enemy;
    }

    public float MaxHP = 10;
    public float HP = 10;

    private SpriteRenderer renderer;

    private void OnEnable()
    {
        renderer = GetComponent<SpriteRenderer>();
    }
    private void UpdateState()
    {
        if(renderer != null)
        {
            renderer.color = Color.Lerp(Color.white, Color.red, 1 - (HP / MaxHP));
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.gameObject.tag == "BULLET")
        {
            Bullet bullet = collision.collider.gameObject.GetComponent<Bullet>();
            if (bullet != null)
            {
                Decrease(bullet.baseDamage);
            }
        }
    }

    public void Decrease(float value)
    {
        HP -= value;
        if(HP <= 0) { onDie(); }
        UpdateState();
    }

    public void Increase(float value)
    {
        HP += value;
        if(HP > MaxHP) { HP = MaxHP; }
        UpdateState();
    }

    private void onDie()
    {
        ScoreManager.Increase(enemy.enemyScore.score);
        Destroy(gameObject);
    }
}
