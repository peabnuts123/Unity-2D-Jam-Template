using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class Drawing
{
    public static readonly Color DEFAULT_COLOR = Color.black;

    public enum Mode
    {
        Set,
        Add,
        Subtract,
        Multiply,
        Max,
        Min,
    }

    // Static state (lol)
    private static bool isDrawing = false;
    // Set is the default mode
    private static Mode currentMode = Mode.Set;
    private static Color32[] currentPixelsOriginal;
    private static Texture2D currentTexture;
    private static Color currentColor;
    private static HashSet<Vector2Int> currentPixelsToDraw;

    public static void SetMode(Mode newMode)
    {
        currentMode = newMode;
    }

    public static void BeginDrawing(Texture2D currentTexture, Color currentColor)
    {
        isDrawing = true;
        Drawing.currentTexture = currentTexture;
        Drawing.currentColor = currentColor;
        currentPixelsOriginal = currentTexture.GetPixels32();
        currentPixelsToDraw = new HashSet<Vector2Int>();
    }

    public static void EndDrawing()
    {
        isDrawing = false;
        // Sanity
        currentTexture = null;
        currentColor = default(Color);
        currentPixelsOriginal = null;
        currentPixelsToDraw = null;
    }

    /// <summary>
    /// Fill a circle of color at position `center`.
    /// </summary>
    /// <param name="center">Pixel coordinate for the center of the circle</param>
    /// <param name="radius">Radius of circle to fill in pixels</param>
    /// <param name="color">Color to fill circle, optional</param>
    public static void FillCircle(Vector2Int center, int radius)
    {
        // Perform processing
        HashSet<Vector2Int> pixelsToPaint = FillCircleRaw(center, radius);

        // Register draw operation
        currentPixelsToDraw.UnionWith(pixelsToPaint);

        // Apply the changes
        UpdateCurrentTexture();
    }

    private static HashSet<Vector2Int> FillCircleRaw(Vector2Int center, int radius)
    {
        HashSet<Vector2Int> pixelsToPaint = new HashSet<Vector2Int>();

        int radiusSquared = radius * radius;
        Vector2Int ruler = new Vector2Int();

        RectInt circleSpan = new RectInt(center.x - radius, center.y - radius, radius * 2, radius * 2);
        for (int x = circleSpan.xMin; x < circleSpan.xMax; x++)
        {
            for (int y = circleSpan.yMin; y < circleSpan.yMax; y++)
            {
                ruler.x = Mathf.Abs(x - center.x);
                ruler.y = Mathf.Abs(y - center.y);

                if (ruler.sqrMagnitude < radiusSquared && currentTexture.Contains(x, y))
                {
                    pixelsToPaint.Add(new Vector2Int(x, y));
                }
            }
        }

        return pixelsToPaint;
    }

    /// <summary>
    ///     Fill a line of radius `radius` from `pointA` to `pointB`
    /// </summary>
    /// <param name="pointA">Starting point of line</param>
    /// <param name="pointB">End point of line</param>
    /// <param name="radius">Radius of line</param>
    /// <param name="color">Color to draw line, optional</param>
    public static void FillLine(Vector2Int pointA, Vector2Int pointB, int radius)
    {
        // Perform processing
        HashSet<Vector2Int> pixelsToPaint = FillLineRaw(pointA, pointB, radius);

        // Register draw operation
        currentPixelsToDraw.UnionWith(pixelsToPaint);

        // Apply the changes
        UpdateCurrentTexture();
    }

    private static HashSet<Vector2Int> FillLineRaw(Vector2Int pointA, Vector2Int pointB, int radius)
    {
        HashSet<Vector2Int> pixelsToPaint = new HashSet<Vector2Int>();

        // @TODO this is not the greatest implementation
        // Draw a bunch of circles between the two points
        //  resolution = step (in pixels) between each circle
        int resolution = 2;

        Vector2 delta = pointB - pointA;
        float distanceBetweenPoints = delta.magnitude;
        // numIterations must be at least 2
        int numIterations = Mathf.Max(2, Mathf.RoundToInt(distanceBetweenPoints / resolution));

        float linearProgress = 0;
        Vector2Int currentPoint;
        for (int i = 0; i < numIterations; i++)
        {
            // linearProgress goes from 0-1
            linearProgress = i / (float)(numIterations - 1);

            // Offset from point A a vector to vector `linearProgress` of the way to point B
            currentPoint = Vector2Int.RoundToInt(pointA + (delta * linearProgress));

            // Draw a circle here (raw, do not apply changes)
            pixelsToPaint.UnionWith(FillCircleRaw(currentPoint, radius));
        }

        return pixelsToPaint;
    }

    // @TODO Alright I think I have figured this out
    /* I think the best way to achieve this is with a buffered mask
        Bounds Mask + Regular Mask + Buffer Mask
        Buffer mask is created when you BeginDrawing. Fill operations mutate the mask
        When you end drawing the mask is applied

        Then this function can be implemented in shader lol
     */
    private static void UpdateCurrentTexture()
    {
        // Reset texture to its state when first began drawing
        currentTexture.SetPixels32(currentPixelsOriginal);

        // Draw every pixel to the texture
        //  massive switch-case to see what color operation to perform
        Color pixelColor = Color.white;
        foreach (Vector2Int point in currentPixelsToDraw)
        {
            // Operate on `pixelColor`
            OperateOnPixelRaw(currentTexture, point, currentColor, currentMode, ref pixelColor);

            currentTexture.SetPixel(point.x, point.y, pixelColor);
        }

        currentTexture.Apply();
    }

    public static Color TestPixelOperation(Texture2D texture, Vector2Int point, Color color, Mode operation)
    {
        // Value does not matter, create instance
        Color refPixel = Color.white;

        // Mutate `refPixel`
        OperateOnPixelRaw(texture, point, color, operation, ref refPixel);

        return refPixel;
    }

    private static void OperateOnPixelRaw(Texture2D texture, Vector2Int point, Color color, Mode operation, ref Color refPixel)
    {
        if (operation == Mode.Set)
        {
            // Assign directly, no need to read from texture
            refPixel = color;
        }
        else
        {
            // Read pixel from image
            refPixel = texture.GetPixel(point.x, point.y);
        }

        switch (operation)
        {
            case Mode.Set:
                /* No-Op */
                break;
            case Mode.Add:
                refPixel.r += color.r;
                refPixel.g += color.g;
                refPixel.b += color.b;
                refPixel.a += color.a;
                break;
            case Mode.Subtract:
                refPixel.r -= color.r;
                refPixel.g -= color.g;
                refPixel.b -= color.b;
                refPixel.a -= color.a;
                break;
            case Mode.Multiply:
                refPixel.r *= color.r;
                refPixel.g *= color.g;
                refPixel.b *= color.b;
                refPixel.a *= color.a;
                break;
            case Mode.Max:
                refPixel.r = Mathf.Max(refPixel.r, color.r);
                refPixel.g = Mathf.Max(refPixel.r, color.g);
                refPixel.b = Mathf.Max(refPixel.r, color.b);
                refPixel.a = Mathf.Max(refPixel.r, color.a);
                break;
            case Mode.Min:
                refPixel.r = Mathf.Min(refPixel.r, color.r);
                refPixel.g = Mathf.Min(refPixel.r, color.g);
                refPixel.b = Mathf.Min(refPixel.r, color.b);
                refPixel.a = Mathf.Min(refPixel.r, color.a);
                break;
            default:
                throw new NotImplementedException("Drawing mode '" + operation + "' is not yet implemented.");
        }
    }

    public static bool IsDrawing
    {
        get
        {
            return isDrawing;
        }
    }
}