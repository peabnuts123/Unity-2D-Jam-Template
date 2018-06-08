using System;
using System.Collections.Generic;
using UnityEngine;

public static class Utilities
{
    public static float Sigmoid(float x)
    {
        return 1 / (1 + Mathf.Exp(-5 * x));
    }

    public static Vector2 GetClosestPointOnPath(Vector2 target, IList<Vector2> path)
    {
        // Validate
        if (path.Count < 2)
        {
            throw new InvalidOperationException("Cannot get closest point on path; path must contain at least 2 vertices");
        }

        Vector2? closestPoint = null;
        float closestDistanceSqr = float.PositiveInfinity;

        for (int i = 1; i < path.Count; i++)
        {
            // Line is a vector between the current point and previous points
            Vector2 line = path[i] - path[i - 1];
            // Construct a vector from the previous point to the target
            Vector2 targetLine = target - path[i - 1];
            // Project targetLine onto line
            Vector2 projectedLineRaw = targetLine.ProjectOnto(line);

            // Clamp projection to within the current line
            float clampedScalar = Mathf.Clamp01(projectedLineRaw.AsScalarOf(line));
            Vector2 projectedLine = clampedScalar * line;
            
            // Figure out whereabouts on line our projection ends (world space)
            Vector2 orthogonalPoint = path[i - 1] + projectedLine;
            // Construct a vector from that point to our target
            Vector2 orthogonalLine = target - orthogonalPoint;

            // Evaluate
            float orthogonalLineSqrMagnitude = orthogonalLine.sqrMagnitude;
            if (orthogonalLineSqrMagnitude < closestDistanceSqr)
            {
                closestPoint = orthogonalPoint;
                closestDistanceSqr = orthogonalLineSqrMagnitude;
            }
        }

        return closestPoint.Value;
    }

    public static Vector2 GetClosestTangentOnPath(Vector2 target, IList<Vector2> path)
    {
        // Validate
        if (path.Count < 2)
        {
            throw new InvalidOperationException("Cannot get closest point on path; path must contain at least 2 vertices");
        }

        Vector2? closestTangent = null;
        float closestDistanceSqr = float.PositiveInfinity;

        for (int i = 1; i < path.Count; i++)
        {
            // Line is a vector between the current point and previous points
            Vector2 line = path[i] - path[i - 1];
            // Construct a vector from the previous point to the target
            Vector2 targetLine = target - path[i - 1];
            // Project targetLine onto line
            Vector2 projectedLineRaw = targetLine.ProjectOnto(line);

            // Clamp projection to within the current line
            float clampedScalar = Mathf.Clamp01(projectedLineRaw.AsScalarOf(line));
            Vector2 projectedLine = clampedScalar * line;
            
            // Figure out whereabouts on line our projection ends (world space)
            Vector2 orthogonalPoint = path[i - 1] + projectedLine;
            // Construct a vector from that point to our target
            Vector2 orthogonalLine = target - orthogonalPoint;

            // Evaluate
            float orthogonalLineSqrMagnitude = orthogonalLine.sqrMagnitude;
            if (orthogonalLineSqrMagnitude < closestDistanceSqr)
            {
                // Invert because this path collection is backwards
                closestTangent = line.normalized * -1;
                closestDistanceSqr = orthogonalLineSqrMagnitude;
            }
        }

        // @BUG it is still possible for this to be null under some
        //  circumstances.
        return closestTangent.Value;
    }
}