using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

[RequireComponent(typeof(SpriteShapeController))]

public class BindSpriteShape : MonoBehaviour
{
    public SpriteShapeController boundPrefab;
    public bool REFRESH;

    SpriteShapeController spriteShapeController;

    private void OnValidate()
    {
        REFRESH = false;
        spriteShapeController = gameObject.GetComponent<SpriteShapeController>();

        int pointCount = spriteShapeController.spline.GetPointCount();
        int destinationCount = boundPrefab.spline.GetPointCount();

        // for each point: 
        // copy point and its tangents to target spline,
        // remove extra points or add missing ones
        for (int i=0; i<pointCount; i++)
        { 
            Vector3 position = spriteShapeController.spline.GetPosition(i);
            Vector3 tangentR = spriteShapeController.spline.GetRightTangent(i);
            Vector3 tangentL = spriteShapeController.spline.GetLeftTangent(i);
            ShapeTangentMode tangentMode = spriteShapeController.spline.GetTangentMode(i);
            float height = spriteShapeController.spline.GetHeight(i);
            bool corner = spriteShapeController.spline.GetCorner(i);

            // if our point exists in target / if we have not less points than target
            if (i < destinationCount)
            {
                // SET point and tangets
                boundPrefab.spline.SetPosition(i, position);
                boundPrefab.spline.SetRightTangent(i, tangentR);
                boundPrefab.spline.SetLeftTangent(i, tangentL);
                boundPrefab.spline.SetTangentMode(i, tangentMode);
                boundPrefab.spline.SetHeight(i, height);
                boundPrefab.spline.SetCorner(i, corner);
            }
        }
        // remove excess points - between index pointCount and destinationCount
        for(int i = pointCount; i<destinationCount; i++)
        {
            boundPrefab.spline.RemovePointAt(i);
        }
    }
}
