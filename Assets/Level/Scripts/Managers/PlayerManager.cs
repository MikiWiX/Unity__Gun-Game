using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{

    public GameObject player;

    private Vector2 lastPlayerPosition = Vector2.zero;

    public static PlayerManager Instance { get; private set; }

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

    private void FixedUpdate()
    {
        if (player != null)
        {
            lastPlayerPosition = player.transform.position;
        }
    }

    public static void setPlayer(GameObject player)
    {
        Instance.player = player;
        CameraManager.setCameraFollowTarget(player);
    }

    public static Vector2 getPlayerPosition()
    {
        return Instance.lastPlayerPosition;
    }

    public static void PlayerKilled(GameObject player)
    {
        Destroy(player);
    }
    public static GameObject GetPlayer()
    {
        return Instance.player;
    }
}
