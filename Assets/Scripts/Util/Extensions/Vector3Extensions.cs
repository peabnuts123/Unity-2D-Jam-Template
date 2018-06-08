using UnityEngine;

public static class Vector3Extensions
{
    public static Vector3 WithX(this Vector3 self, float newX)
    {
        return new Vector3(newX, self.y, self.z);
    }

    public static Vector3 WithY(this Vector3 self, float newY)
    {
        return new Vector3(self.x, newY, self.z);
    }
    public static Vector3 WithZ(this Vector3 self, float newZ)
    {
        return new Vector3(self.x, self.y, newZ);
    }
}