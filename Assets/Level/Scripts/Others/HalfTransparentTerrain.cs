using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class HalfTransparentTerrain : MonoBehaviour
{
    public float maxGroundNormalY = 0.6f;
    List<Collider2D> ignoredCollisions = new List<Collider2D>();

    Collider2D collider;

    private void Start()
    {
        collider = (Collider2D)gameObject.GetComponent(typeof(Collider2D));
    }

    void FixedUpdate()
    {
        int listSize = ignoredCollisions.Count;
        // continue ignoring collisions until they end
        for (int i=0; i < listSize; i++)
        {
            // TODO check if list element is not null
            if (! collider.IsTouching(ignoredCollisions[i]))
            {
                Physics2D.IgnoreCollision(collider, ignoredCollisions[i], false);
                ignoredCollisions.RemoveAt(i);
                listSize--;
                Debug.Log("Unignored");
            }
            
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(((Rigidbody2D)collision.transform.gameObject.GetComponent(typeof(Rigidbody2D))).velocity);
        // if average cllision is below minimum, ignore all collisions 

        // get average normal of all collision points between 2 colliders
        Vector2 averageNormal = getAverageNormal(collision);

        // if incoming collider has high averageNormal (hit below this object)
        // so it is not walking
        if (averageNormal.y >= maxGroundNormalY) 
        {
            // ignore collision and add to list
            Debug.Log("Ignored");
            Physics2D.IgnoreCollision(collider, collision.collider, true);
            ignoredCollisions.Add(collision.collider);
        }
    }

    private Vector2 localVec = new Vector2();
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
