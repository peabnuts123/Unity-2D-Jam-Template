using System;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public static class DrawDebug
{
    static readonly float defaultIconSize = 0.005F;
    static readonly Color defaultIconColor = Color.magenta;


    public static void Lines(params Vector4[] lines)
    {
        Lines(null, lines);
    }

    public static void Lines(Color? lineColor, params Vector4[] lines)
    {
        // No params
        if (lines == null)
        {
            return;
        }

        for (int i = 0; i < lines.Length; i++)
        {
            Vector4 line = lines[i];
            if (lineColor.HasValue)
            {
                Debug.DrawLine(new Vector3(line.x, line.y), new Vector3(line.z, line.w), lineColor.Value);
            }
            else
            {
                Debug.DrawLine(new Vector3(line.x, line.y), new Vector3(line.z, line.w), Color.HSVToRGB(((i * 1) % 360) / 360F, 1, 1));
            }
        }
    }

    public static void Points(params Vector2[] points)
    {
        Points(defaultIconSize, defaultIconColor, points);
    }

    public static void Points(Color iconColor, params Vector2[] points)
    {
        Points(defaultIconSize, iconColor, points);
    }

    public static void Points(float iconSize, params Vector2[] points)
    {
        Points(iconSize, Color.magenta, points);
    }

    public static void Points(float iconSize, Color iconColor, params Vector2[] points)
    {
        // No params
        if (points == null)
        {
            return;
        }

        Vector2 bottomLeft = new Vector2(-iconSize, -iconSize);
        Vector2 bottomRight = new Vector2(iconSize, -iconSize);
        Vector2 topLeft = new Vector2(-iconSize, iconSize);
        Vector2 topRight = new Vector2(iconSize, iconSize);

        foreach (Vector2 point in points)
        {
            Debug.DrawLine(point + bottomLeft, point + topRight, iconColor);
            Debug.DrawLine(point + topLeft, point + bottomRight, iconColor);
        }
    }

    public static void Label(String text, Vector2 position)
    {
#if UNITY_EDITOR
        Color oldColor = GUI.color;
        GUI.color = Color.red;
        Handles.Label(position, text);
        GUI.color = oldColor;
#endif
    }
}