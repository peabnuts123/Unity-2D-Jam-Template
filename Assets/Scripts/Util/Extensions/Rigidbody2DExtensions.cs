using System;
using UnityEngine;

public static class Rigidbody2DExtensions
{
    public static Collider2D[] GetAllColliders(this Rigidbody2D self)
    {
        Collider2D[] colliders = new Collider2D[self.attachedColliderCount];
        self.GetAttachedColliders(colliders);
        return colliders;
    }

    public static void IgnoreCollision(this Rigidbody2D self, Collider2D collider, bool ignore)
    {
        Collider2D[] colliders = self.GetAllColliders();

        for (int colliderIndex = 0; colliderIndex < colliders.Length; colliderIndex++)
        {
            Physics2D.IgnoreCollision(colliders[colliderIndex], collider, ignore);
        }
    }

    public static void IgnoreCollision(this Rigidbody2D self, Rigidbody2D other, bool ignore)
    {
        Collider2D[] colliders = self.GetAllColliders();
        Collider2D[] otherColliders = other.GetAllColliders();

        for (int colliderIndex = 0; colliderIndex < colliders.Length; colliderIndex++)
        {
            for (int otherColliderIndex = 0; otherColliderIndex < otherColliders.Length; otherColliderIndex++)
            {
                Physics2D.IgnoreCollision(colliders[colliderIndex], otherColliders[otherColliderIndex], ignore);
            }
        }
    }
}