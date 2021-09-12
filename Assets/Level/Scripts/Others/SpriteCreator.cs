using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Required when Using UI elements.

[RequireComponent(typeof(RectTransform))]
[RequireComponent(typeof(SpriteRenderer))]
public class SpriteCreator : MonoBehaviour
{
    public enum Pivot
    {
        AXIS_START,
        AXIS_END,
        NONE
    }

    public bool REFRESH = false;

    // 0 = further default (copy first Sprite resolution)
    public int pixelPerUnit = 0;

    // target texture dimension; 0 = default (calc from pixelPerUnit)
    public int textureWidth = 0;
    public int textureHeigth = 0;

    public TextureWrapMode wrapMode;
    public FilterMode filterMode;

    public Sprite[] inputSprites;
    public Pivot[] inputPivotX;
    public Pivot[] inputPivotY;

    SpriteRenderer rend; //renderer component
    Texture2D tex; //generated texture
    Vector2 size; // size of rectangle
    Vector3 scale;

    private void OnValidate()
    {
        REFRESH = false;
        // get renderer
        rend = GetComponent<SpriteRenderer>();
        // add scale to size
        size = ((RectTransform)transform).rect.size * transform.localScale;
        ((RectTransform)transform).sizeDelta = size;
        transform.localScale = Vector3.one;

        int texW=0, texH=0; // target texture dimensions
        int ppu = CalculatePixelPerUnit();

        //calculate target texture dimension
        texW = (int)(size.x * ppu);
        texH = (int)(size.y * ppu);
        Debug.Log(texW);
        Debug.Log(texH);
        //create blank textutre and its color array (canvas)
        tex = new Texture2D(texW, texH);
        Color[] canvas = new Color[tex.height * tex.width];

        //init canvas
        for (int i=0; i<canvas.Length; i++)
        {
            canvas[i] = new Color(0, 0, 0, 0);
        }

        // FOREACH GIVEN INPUT SPRITE
        for (int i=0; i<inputSprites.Length; i++)
        {
            Sprite sprite = inputSprites[i];

            // sprite position in source texture
            int x = (int)sprite.rect.x;
            int y = (int)sprite.rect.y;
            int w = (int)sprite.rect.width;
            int h = (int)sprite.rect.height;
            // read
            Debug.Log(x + " " + y + " " + w + " " + h);
            Color[] inputCanvas = sprite.texture.GetPixels(x, y, w, h, 0);

            // painting area in target texture
            int outMinX, outMaxX, outMinY, outMaxY;
            // pixel starting index at input image
            int inStartX, inStartY;

            Pivot localPivot = inputPivotX.Length <= i ? Pivot.NONE : inputPivotX[i];
            getImageBordersOnAxis(out outMinX, out outMaxX, out inStartX, texW, w, localPivot);
            localPivot = inputPivotY.Length <= i ? Pivot.NONE : inputPivotY[i];
            getImageBordersOnAxis(out outMinY, out outMaxY, out inStartY, texH, h, localPivot);
            Debug.Log(outMinY + " " + outMaxY + " " + inStartY);
            // actually layer the images
            int inY = inStartY; // get input sprite start/current Y coordinaate
            for (int lineIndex = outMinY; lineIndex < outMaxY; lineIndex++) // foreach line to paint
            {
                // ** paint line **
                // REPEAT ABOVE/BELOW
                int inX = inStartX; // get input sprite start/current Y coordinaate
                for (int columnIndex = outMinX; columnIndex < outMaxX; columnIndex++) // foreach line to paint
                {
                    // ** paint pixel **
                    int canvasIndex = (lineIndex * texW) + columnIndex;
                    int inputCanvasIndex = (inY * w) + inX;
                    canvas[canvasIndex] = NormalBlend(canvas[canvasIndex], inputCanvas[inputCanvasIndex]);     

                    inX++;
                    inX = inX >= w ? 0 : inX; // increment/zero input sprite Y coordinate
                }
                // END OF REPEAT
                inY++;
                inY = inY >= h ? 0 : inY; // increment/zero input sprite Y coordinate
            }
            // end of texture processing loop for single texture
        }
        // end for all textures processing
        tex.SetPixels(canvas);
        tex.wrapMode = wrapMode;
        tex.filterMode = filterMode;
        tex.Apply();

        Sprite finalSprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), Vector2.one * 0.5f, ppu);
        finalSprite.name = "Internal";
        rend.sprite = finalSprite;

        ResizeCollider();
    }

    private void getImageBordersOnAxis(
        out int targetMin,
        out int targetMax,
        out int sourceBegin,
        int targetSize,
        int sourceSize,
        Pivot pivot)
    {

        switch (pivot)
        {
            case Pivot.AXIS_START:
                targetMin = 0;
                targetMax = sourceSize <= targetSize ? sourceSize : targetSize;
                sourceBegin = 0;
                break;
            case Pivot.AXIS_END:
                targetMin = targetSize > sourceSize ? (targetSize - sourceSize) : 0;
                targetMax = targetSize;
                sourceBegin = targetSize < sourceSize ? (sourceSize - targetSize) : 0;
                break;
            default: //no snapping texture
                targetMin = 0;
                targetMax = targetSize;
                sourceBegin = 0;
                break;
        }
        return;
    }

    private int CalculatePixelPerUnit()
    {
        int ppu = 0;

        if (pixelPerUnit != 0)
        {
            ppu = pixelPerUnit;
        }
        else if (textureWidth != 0 && textureHeigth != 0)
        {
            int ppuw = (int)(textureWidth / size.x);
            int ppuh = (int)(textureHeigth / size.y);
            ppu = Mathf.Max(ppuw, ppuh);
        }
        else if (inputSprites.Length > 0 && inputSprites[0] != null)
        {
            int ppuw = (int)(inputSprites[0].textureRect.width / size.x);
            int ppuh = (int)(inputSprites[0].textureRect.height / size.y);
            ppu = Mathf.Max(ppuw, ppuh);
        }
        else
        {
            ppu = 100;
        }

        return ppu;
    }

    private void CalculateTextureDimensions(out int texW, out int texH)
    {
        if (textureWidth != 0 && textureHeigth != 0)
        {
            texW = textureWidth;
            texH = textureHeigth;
        }
        else if (pixelPerUnit != 0)
        {
            texW = (int)(size.x * pixelPerUnit);
            texH = (int)(size.y * pixelPerUnit);
        }
        else if (inputSprites.Length > 0 && inputSprites[0] != null)
        {
            texW = (int)inputSprites[0].textureRect.width;
            texH = (int)inputSprites[0].textureRect.height;
        }
        else
        {
            texW = 1;
            texH = 1;
        }
    }

    private Color NormalBlend(Color baseCol, Color coverCol)
    {
        return (baseCol * baseCol.a * (1 - coverCol.a)) + (coverCol * coverCol.a);
    }

    BoxCollider2D collider;

    private void ResizeCollider()
    {
        collider = GetComponent<BoxCollider2D>();
        if(collider != null)
        {
            collider.size = size;
        }
    }
}
