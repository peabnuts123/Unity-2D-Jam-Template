using UnityEngine;
using Zenject;

public static class PointService
{
    public static Vector2 RandomPoint(float minX = 0, float minY = 0, float maxX = 1, float maxY = 1)
    {
        return new Vector2(Random.Range(minX, maxX), Random.Range(minY, maxY));
    }

    public static Vector2 RandomPointInRadius(Vector2 center, float radius)
    {
        return center + Random.insideUnitCircle.normalized * radius;
    }

    public static Vector2 RandomValidPoint(float minX, float minY, float maxX, float maxY, LayerMask mask)
    {
        bool isValid = false;
        Vector2? point = null;
        while (!isValid)
        {
            // Generate random point
            point = PointService.RandomPoint(minX, minY, maxX, maxY);

            // Cast point into scene against layers
            Collider2D collider = Physics2D.OverlapPoint(point.Value, mask);

            // Test validity of point
            isValid = collider == null && point.HasValue;
        }

        return point.Value;
    }

    public static Vector2 RandomValidPointInRadius(Vector2 center, float radius, LayerMask mask)
    {
        bool isValid = false;
        Vector2? point = null;
        while (!isValid)
        {
            // Generate random point
            point = PointService.RandomPointInRadius(center, radius);

            // Cast point into scene against layers
            Collider2D collider = Physics2D.OverlapPoint(point.Value, mask);

            // Test validity of point
            isValid = collider == null && point.HasValue;
        }

        return point.Value;
    }

    public static Vector2 DropPointToGround(Vector2 point, LayerMask ground, float distanceFromGround = 0.02F)
    {
        RaycastHit2D hitInfo = Physics2D.Raycast(point, Vector2.down, float.PositiveInfinity, ground);
        return hitInfo.point + Vector2.up * distanceFromGround;
    }
}