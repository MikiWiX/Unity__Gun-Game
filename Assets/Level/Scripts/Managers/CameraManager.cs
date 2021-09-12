using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{

    public GameObject cameraParent;
    public Camera cam;

    public static CameraManager Instance { get; private set; }
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

    public static void setCameraFollowTarget(GameObject target)
    {
        List<CinemachineVirtualCamera> cameras = LevelTools.getChildrenWithComponent<CinemachineVirtualCamera>(Instance.cameraParent);
        foreach(CinemachineVirtualCamera cam in cameras)
        {
            cam.Follow = target.transform;
        }
    }

    public static Vector2 getWorldSpaceAtScreenCenter()
    {
        int screenCenterX = Instance.cam.pixelWidth / 2;
        int screenCenterY = Instance.cam.pixelHeight / 2;
        return Instance.cam.ScreenToWorldPoint(new Vector3(screenCenterX, screenCenterY, 0));
    }

}
