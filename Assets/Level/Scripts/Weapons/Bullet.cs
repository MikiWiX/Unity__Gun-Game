using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Weapons
{
    public class Bullet : MonoBehaviour
    {
        public float baseDamage = 1;

        public float timeLeft = 1;
        public float speed = 1;

        Vector3 velocity;

        ContactPoint2D[] contacts = new ContactPoint2D[16];
        Collider2D collider2d;
        ContactFilter2D contactFilter;

        private void OnEnable()
        {
            collider2d = GetComponent<Collider2D>();
        }

        void Start()
        {
            contactFilter.useTriggers = false; // dont use trigger collider
            contactFilter.SetLayerMask(Physics2D.GetLayerCollisionMask(gameObject.layer)); //get and set this object layer to collider
            contactFilter.useLayerMask = true; // filter only collisions from loaded layer

            Vector3 dir = transform.rotation * Vector3.right;
            Vector2 dir2D = new Vector2(dir.x, dir.y);
            velocity = dir2D * speed;
        }

        void FixedUpdate()
        {
            transform.position = transform.position + (velocity * Time.fixedDeltaTime);

            timeLeft -= Time.fixedDeltaTime;
            if (timeLeft <= 0)
            {
                Destroy(gameObject);
            }
        }

        void OnCollisionEnter2D(Collision2D collision)
        {
            Destroy(gameObject);
        }
    }

}
