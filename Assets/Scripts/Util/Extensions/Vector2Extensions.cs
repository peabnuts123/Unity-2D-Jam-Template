using System;
using UnityEngine;

public static class Vector2Extensions
{
    public static Vector2 ProjectOnto(this Vector2 a, Vector2 b)
    {
        return (Vector2.Dot(a, b) / b.sqrMagnitude) * b;
    }

    public static Vector2 Orthogonal(this Vector2 self)
    {
        return new Vector2(-self.y, self.x);
    }

    public static Vector2 Clone(this Vector2 self)
    {
        return new Vector2(self.x, self.y);
    }

    public static Vector2 Add(this Vector2 self, float x, float y)
    {
        return new Vector2(self.x + x, self.y + y);
    }

    public static Vector2 Multiply(this Vector2 self, Vector2 other)
    {
        return new Vector2(self.x * other.x, self.y * other.y);
    }

    public static Vector2 WithX(this Vector2 self, float x)
    {
        return new Vector2(x, self.y);
    }

    public static Vector2 WithY(this Vector2 self, float y)
    {
        return new Vector2(self.x, y);
    }

    public static Vector4 Concat(this Vector2 self, Vector2 other)
    {
        return new Vector4(self.x, self.y, other.x, other.y);
    }

    public static float Dot(this Vector2 self, Vector2 other)
    {
        return self.x * other.x + self.y * other.y;
    }

    public static float AsScalarOf(this Vector2 self, Vector2 other)
    {
        float scalar;
        if (other.x != 0)
        {
            scalar = self.x / other.x;
        }
        else if (other.y != 0)
        {
            scalar = self.y / other.y;
        }
        else
        {
            // other is {0,0}, so…
            return 0;
        }

        Vector2 scaledOther = other * scalar;

        float tolerance = 1e-5F;
        if (scaledOther.x - self.x < tolerance && scaledOther.y - self.y < tolerance)
        {
            return scalar;
        }
        else
        {
            throw new InvalidOperationException("Vector parameter is not a scalar of target Vector");
        }
    }
}