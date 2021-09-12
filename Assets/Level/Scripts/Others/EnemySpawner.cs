using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Characters;

public class EnemySpawner : MonoBehaviour
{
    public GameObject defaultSpawn;

    public float sizeX;
    public float sizeY;

    public bool REFRESH = false;

    public bool autoSpawn = false;
    public float spawnInterval = 1;
    public float initialTime = 0;

    private float timeSinceLastSpawn;
    private bool working = true;

    private void Start()
    {
        timeSinceLastSpawn = initialTime;
    }

    private void OnValidate()
    {
        REFRESH = false;
        sizeToScale();
    }

    private void scaleToSize()
    {
        sizeX = sizeX * transform.localScale.x;
        sizeY = sizeY * transform.localScale.y;
        transform.localScale = Vector2.one;
    }

    private void sizeToScale()
    {
        transform.localScale *= new Vector2(sizeX, sizeY);
        sizeX = 1;
        sizeY = 1;
    }

    float randX, randY;
    protected virtual void FixedUpdate()
    {
        if (timeSinceLastSpawn >= spawnInterval)
        {
            timeSinceLastSpawn = spawnInterval;
            if (autoSpawn)
            {
                timeSinceLastSpawn = 0;
                SpawnEnemy(getRandomLocation(), getDefaultRotation());
            }
        }
        timeSinceLastSpawn += Time.fixedDeltaTime;

    }

    public T Spawn<T>(T enemy, bool ignoreCooldown = true) where T : Object
    {
        return Spawn(enemy, getRandomLocation(), getDefaultRotation(), ignoreCooldown);
    }
    public GameObject Spawn(bool ignoreCooldown = true)
    {
        return Spawn(getRandomLocation(), getDefaultRotation(), ignoreCooldown);
    }
    public GameObject Spawn(Vector2 spawnPos, Quaternion rotation, bool ignoreCooldown = true)
    {
        return Spawn<GameObject>(defaultSpawn, spawnPos, rotation, ignoreCooldown);
    }
    public T Spawn<T>(T enemy, Vector2 spawnPos, Quaternion rotation, bool ignoreCooldown = true) where T : Object
    {
        if (ignoreCooldown || timeSinceLastSpawn >= spawnInterval)
        {
            timeSinceLastSpawn = 0;
            return SpawnEnemy(enemy, spawnPos, rotation);
        }
        return null;
    }

    private GameObject SpawnEnemy(Vector2 spawnPos, Quaternion rotation)
    {
        return SpawnEnemy(defaultSpawn, spawnPos, rotation);
    }
    private T SpawnEnemy<T>(T enemy, Vector2 spawnPos, Quaternion rotation) where T : Object
    {
        return Instantiate(enemy, spawnPos, rotation);
    }


    private Vector2 getRandomLocation()
    {
        randX = Random.Range(-sizeX, sizeX);
        randY = Random.Range(-sizeY, sizeY);
        return transform.TransformPoint(new Vector3(randX, randY, 0));
    }

    private Quaternion getDefaultRotation()
    {
        return Quaternion.identity;
    }

    void OnDrawGizmos()
    {
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawWireCube(Vector3.zero, new Vector3(sizeX, sizeY, 0));
    }

    private void OnBecameVisible()
    {
        working = false;
    }
    private void OnBecameInvisible()
    {
        working = true;
    }

    public bool getWorkingState()
    {
        return working;
    }
} 
