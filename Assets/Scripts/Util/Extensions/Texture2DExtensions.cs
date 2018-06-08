using UnityEngine;

public static class Texture2DExtensions
{
    /// <summary>
    ///     Fill an entire texture with a color
    /// </summary>
    /// <param name="color">The color to set all pixels to</param>
    public static void Fill(this Texture2D self, Color color)
    {
        // Do the actual processing 
        FillRaw(self, color);

        // Apply the changes
        self.Apply();
    }

    private static void FillRaw(Texture2D self, Color color)
    {
        for (int x = 0; x < self.width; x++)
        {
            for (int y = 0; y < self.height; y++)
            {
                self.SetPixel(x, y, color);
            }
        }
    }

    /// <summary>
    /// Get the width/height of a texture as a Vector2Int
    /// </summary>
    /// <returns>Vector2Int representing the dimensions</returns>
    public static Vector2Int GetDimensions(this Texture2D self)
    {
        return new Vector2Int(self.width, self.height);
    }

    /// <summary>
    ///     Test to see if texture dimensions contain point `point`
    /// </summary>
    /// <param name="point">Vector2 to test</param>
    /// <returns>True if `point` is within the dimensions of this texture</returns>
    public static bool Contains(this Texture2D self, Vector2 point)
    {
        return point.x >= 0 && point.x < self.width &&
            point.y >= 0 && point.y < self.height;
    }

    /// <summary>
    ///     Tet to see if texture dimensions contain point (`x`, `y`)
    /// </summary>
    /// <param name="x">X coordinate of point to test</param>
    /// <param name="y">Y coordinate of point to test</param>
    /// <returns>True if (`x`, `y`) is within the dimensions of this texture</returns>
    public static bool Contains(this Texture2D self, float x, float y)
    {
        return x >= 0 && x < self.width &&
            y >= 0 && y < self.height;
    }
}