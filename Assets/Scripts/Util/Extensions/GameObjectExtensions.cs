using System;
using System.Linq;
using System.Reflection;
using UnityEngine;

public static class GameObjectExtensions
{
    // @NOTE: I'm like, really sorry about all this.
    // public static T CloneComponent<T>(this GameObject self, Component sourceComponent, params string[] propertyBlacklist) where T : Component
    // {
    //     // Types for GameObject and dynamic type for `component`
    //     Type gameObjectType = typeof(GameObject);
    //     Type componentType = sourceComponent.GetType();

    //     // Get reflection method info for `GameObject.AddComponent`
    //     MethodInfo addComponentMethodInfo = gameObjectType.GetMethod("AddComponent", new Type[] { });
    //     // Curry methodInfo to use `componentType` as generic parameter
    //     addComponentMethodInfo = addComponentMethodInfo.MakeGenericMethod(componentType);

    //     // Invoke our addComponent method to create clonedComponent
    //     Component clonedComponent = (Component)(addComponentMethodInfo.Invoke(self, null));

    //     // Loop through properties and copy them from source to cloned component
    //     foreach (PropertyInfo propertyInfo in componentType.GetProperties())
    //     {
    //         bool isBlacklisted = propertyBlacklist.Any((string blacklistItem) => propertyInfo.Name.Equals(blacklistItem, StringComparison.InvariantCultureIgnoreCase));
    //         if (!isBlacklisted && propertyInfo.CanWrite && propertyInfo.CanRead)
    //         {
    //             propertyInfo.SetValue(clonedComponent, propertyInfo.GetValue(sourceComponent));
    //         }
    //     }

    //     // Cast to T because we're going to be doing that anyway
    //     //  If you pass the wrong T you'll get exceptions
    //     return (T)clonedComponent;
    // }
}