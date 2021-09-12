using System;
using System.Collections.Generic;
using UnityEngine;
using Characters;

public class SpawnManager : MonoBehaviour
{
    public GameObject spawnersParent;

    public bool spawnOnScreen = false;
    public float spawnRange = 100;

    public float spawnInterval = 3;
    public float timeSinceLastSpawn = 0;

    public float intervalChangeFactor = 1;

    List<EnemySpawner> spawners = new List<EnemySpawner>();

    public static SpawnManager Instance { get; private set; }
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

    private void OnEnable()
    {
        spawners = LevelTools.getChildrenWithComponent<EnemySpawner>(Instance.spawnersParent);
    }

    private void FixedUpdate()
    {
        if (timeSinceLastSpawn >= spawnInterval)
        {
            spawnInterval *= intervalChangeFactor;
            timeSinceLastSpawn = 0;
            SpawnEnemy();
        }
        timeSinceLastSpawn += Time.fixedDeltaTime;  
    }

    private void getSpawnerArray(out EnemySpawner[] spawnerArray, bool ignoreVisible, Vector2 position, float range)
    {
        spawnerArray = new EnemySpawner[spawners.Count];
        int count = 0;
        foreach(EnemySpawner obj in spawners){
            if ((!ignoreVisible || obj.getWorkingState()) && // if it is working (not visible) or not ignore visible
                ((Vector2)obj.transform.position - position).magnitude < range) // and it is in range from given position
            {
                spawnerArray[count++] = obj;
            }
        }
        Array.Resize<EnemySpawner>(ref spawnerArray, count);
    }

    public void SpawnEnemy()
    {
        SpawnEnemy(spawnOnScreen, spawnRange);
    }

    public void SpawnEnemy(bool onScreen, float range)
    {
        EnemySpawner[] array;
        if (onScreen)
        {
            getSpawnerArray(out array, false, CameraManager.getWorldSpaceAtScreenCenter(), range);
        } 
        else
        {
            getSpawnerArray(out array, true, CameraManager.getWorldSpaceAtScreenCenter(), range);
        }
        if(array.Length >= 1)
        {
            int spawnIndex = UnityEngine.Random.Range(0, array.Length - 1);
            GameObject enemy = array[spawnIndex].Spawn();
            if(enemy != null)
            {
                Enemy enemyProps = enemy.GetComponent<Enemy>();
                if (enemyProps != null)
                {
                    enemyProps.enemyMotion.setSpeedScale(computeEnemySpeed());
                    enemyProps.enemyHP.HP = computeEnemyHP();
                }
            }
        }
    }

    private float computeEnemySpeed()
    {
        // 1 + 1/10 of the (time-20)
        // so at the beginning enemies are 0.8 speed
        // after 20 sec they are at speed 1
        // speed increasing linearly
        return 1 + ((LevelManager.getLevelTime() - 20) * 0.01f);
    }

    private int computeEnemyHP()
    {
        // same as with speed, just that the value is rounded and the modifier is greater
        return (int)(3 + ((LevelManager.getLevelTime() - 10) * 0.2f));
    }
}
