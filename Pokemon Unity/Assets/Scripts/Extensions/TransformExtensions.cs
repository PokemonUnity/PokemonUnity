using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TransformExtensions
{
    /// <summary>Recursively searches all child Transforms for a component of Type T. Depth-first search</summary>
    /// <typeparam name="T">A MonoBehaviour</typeparam>
    /// <param name="root">The transform to start at</param>
    /// <returns>The first component of type T. Returns null if none found</returns>
    public static T FindFirst<T>(this Transform root, bool onlyActiveObjects = true, SearchType searchType = SearchType.DepthFirst) where T : MonoBehaviour {
        if (searchType == SearchType.DepthFirst) { 
            if (root.TryGetComponent(out T behaviour)) {
                return behaviour;
            } else {
                foreach (Transform transform in root) {
                    if (onlyActiveObjects && !transform.gameObject.activeSelf) continue;
                    behaviour = transform.FindFirst<T>();
                    if (behaviour is not null) return behaviour;
                }
            }
            return null;
        } else if (searchType == SearchType.BreadthFirst) {
            // Create a queue to hold the nodes at the current depth level
            Queue<Transform> queue = new Queue<Transform>();

            // Enqueue the root node
            queue.Enqueue(root);

            // Continue while there are nodes in the queue
            while (queue.Count > 0) {
                // Dequeue the next node
                Transform node = queue.Dequeue();

                // If the node value matches the search criteria, return it
                if (node.TryGetComponent(out T component)) {
                    return component;
                }

                // Enqueue the child nodes
                foreach (Transform childNode in node) {
                    queue.Enqueue(childNode);
                }
            }

            // Return the default value if the search fails
            return default(T);
        }
        return default(T);
    }

    /// <summary>Tries to find a Transform with the given name, and returns it in the parameter transform</summary>
    /// <param name="name">The name of the transform to find</param>
    /// <param name="transform">The found Transform, or null</param>
    /// <returns>True if the Transform was found</returns>
    public static bool TryFind(this Transform instance, string name, out Transform transform) {
        transform = instance.Find(name);
        return transform != null;
    }

    public static bool TryFind<T>(this Transform instance, string name, out T component) {
        Transform transform = instance.Find(name);
        if (transform != null) component = transform.GetComponent<T>();
        else component = default(T);
        return transform != null;
    }

    /// <summary>Find the first index of a child that has component of type T</summary>
    /// <returns>-1 if no valid child found</returns>
    public static int IndexOf<T>(this Transform root) {
        for (int i = 0; i < root.childCount; i++) {
            if (root.GetChild(i).TryGetComponent(out T component)) {
                return i;
            }
        }
        return -1;
    }

    public enum SearchType {
        DepthFirst,
        BreadthFirst
    }

    //public static void SetParent(this Transform transform, Transform parent, bool keepTransformValues) {
    //    if (!keepTransformValues) {
    //        transform.SetParent(parent);
    //        return;
    //    }
    //    Vector3 localPosition = transform.localPosition;
    //    Quaternion localRotation = transform.localRotation;
    //    Vector3 localScale = transform.localScale;
    //    transform.SetParent(parent);
    //    transform.localPosition = localPosition;
    //    transform.localRotation = localRotation;
    //    transform.localScale = localScale;
    //}
}
