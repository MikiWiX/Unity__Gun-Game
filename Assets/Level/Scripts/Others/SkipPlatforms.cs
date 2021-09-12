using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class SkipPlatforms : MonoBehaviour
{
    public float minGroundNormalY;

    Vector2 preVelocity = Vector2.zero;
    List<Collider2D> ignoredCollisions = new List<Collider2D>();

    private Collider2D collider;
    private Rigidbody2D rigidbody;
    ContactFilter2D contactFilter;

    private void OnEnable()
    {
        collider = (Collider2D)gameObject.GetComponent(typeof(Collider2D));
        rigidbody = ((Rigidbody2D)gameObject.GetComponent(typeof(Rigidbody2D)));

        contactFilter.useTriggers = true; // dont use trigger collider
        contactFilter.SetLayerMask(Physics2D.GetLayerCollisionMask(gameObject.layer)); //get and set this object layer to collider
        contactFilter.useLayerMask = true; // filter only collisions from loaded layer
    }

    List<Collider2D> tmpResult = new List<Collider2D>();

    private void FixedUpdate()
    {       
        preVelocity = rigidbody.velocity;

        int listSize = ignoredCollisions.Count;
        // continue ignoring collisions until they end

        int colliderCount = collider.OverlapCollider(contactFilter, tmpResult);

        for (int i = 0; i < listSize; i++)
        {
            // if element is null or is not colliding not touching : remove it and stop ignoring collisions
            int indx = tmpResult.IndexOf(ignoredCollisions[i]);
            if(ignoredCollisions[i]==null || indx < 0 && !collider.IsTouching(ignoredCollisions[i])) 
            {
                if(ignoredCollisions[i] != null)
                {
                    Physics2D.IgnoreCollision(collider, ignoredCollisions[i], false);
                }
                ignoredCollisions.RemoveAt(i);
                i--;
                listSize--;
            }
        }
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // if the object tag is Platform and
        // if the collider is not yet ignored
        if (collision.gameObject.CompareTag("Platform") && !ignoredCollisions.Contains(collision.collider)) 
        {
            // get average normal of all collision points between 2 colliders
            Vector2 averageNormal = getAverageNormal(collision);

            // if incoming collider has high averageNormal (hit below this object)
            // ...so it is not walking
            if (averageNormal.y < minGroundNormalY)
            {
                // ignore collision and add to list
                Physics2D.IgnoreCollision(collider, collision.collider, true);
                rigidbody.velocity = preVelocity;
                ignoredCollisions.Add(collision.collider);
            }
        }
    }

    public void DropDown()
    {
        // for all collisions
        int colliderCount = collider.OverlapCollider(contactFilter, tmpResult);
        for (int i=0; i<colliderCount; i++)
        {
            //if it is a platform and it is not yet ignored
            if(tmpResult[i].gameObject.CompareTag("Platform") && !ignoredCollisions.Contains(tmpResult[i]))
            {
                // ignore collisions from it
                Physics2D.IgnoreCollision(collider, tmpResult[i], true);
                ignoredCollisions.Add(tmpResult[i]);
            }
        }
    }

    private Vector2 localVec;
    private Vector2 getAverageNormal(Collision2D collision)
    {
        localVec.Set(0, 0.01f);
        for (int i = 0; i < collision.contactCount; i++)
        {
            localVec += collision.GetContact(i).normal;
        }
        return localVec.normalized;
    }
}
