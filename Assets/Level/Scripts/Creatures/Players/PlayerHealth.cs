using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int hp = 1;
    
    public Collider2D collider;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.gameObject.tag == "ENEMY")
        {
            EnemyCollision();
        }
    }
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "ENEMY")
        {
            EnemyCollision();
        }
    }

    public void EnemyCollision()
    {
        hp -= 1;
        if (hp <= 0)
        {
            hp = 0;
            PlayerManager.PlayerKilled(gameObject);
            LevelManager.gameOver();
        }
    }
}
