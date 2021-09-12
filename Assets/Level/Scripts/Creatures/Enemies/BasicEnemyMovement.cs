using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Characters
{
    [RequireComponent(typeof(Enemy))]
    public class BasicEnemyMovement : CreatureMotion, EnemyMotion
    {
        Enemy enemy;

        public void setEnemy(Enemy enemy)
        {
            this.enemy = enemy;
        }

        new void OnEnable()
        {
            base.OnEnable();
        }

        private float cooldown = 0;
        void Update()
        {
            cooldown = cooldown < 0 ? 0 : cooldown - Time.deltaTime;
            
            float size = transform.localScale.x;

            // offset between player and this enemy
            Vector3 distance = (Vector3)PlayerManager.getPlayerPosition() - transform.position;
            // if enemy is far anough and cannot move
            // and there is no cooldown
            if (distance.magnitude > size
                && body.velocity.magnitude < 0.25f
                && cooldown <= 0)
            {

                // distance.normalized.y : -1 to 1 
                // add 1 : 0 to 2
                float jumpProb = distance.normalized.y + 1;
                //randomly jump or drop down based on jumpProb
                switch (Random.Range(0.0f, 2.0f) < jumpProb)
                {
                    case true:
                        base.Jump();
                        cooldown = 0.3f;
                        break;
                    case false:
                        base.DropDown();
                        cooldown = 0.3f;
                        break;
                }
            }
        }

        new private void FixedUpdate()
        {
            float size = transform.localScale.x;

            if(PlayerManager.getPlayerPosition().x - size > transform.position.x)
            {
                updateMovementDirection(1);
            } else if (PlayerManager.getPlayerPosition().x + size < transform.position.x)
            {
                updateMovementDirection(-1);
            } else
            {
                updateMovementDirection(0);
            }

            base.FixedUpdate();

        }

        public Vector2 getSize()
        {
            return boxCollider2d.size;
        }

        
    }

}
