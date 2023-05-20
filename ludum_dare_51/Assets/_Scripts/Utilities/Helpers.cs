using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A static class for general helpful methods
/// </summary>
public static class Helpers
{
    // STATIC CAMERA

    /// <summary>
    /// Destroy all child objects of this transform (Unintentionally evil sounding).
    /// Use it like so:
    /// <code>
    /// transform.DestroyChildren();
    /// </code>
    /// </summary>
    public static void DestroyChildren(this Transform t)
    {
        foreach (Transform child in t) Object.Destroy(child.gameObject);
    }

    public static bool AllTransformsExist(List<Transform> transforms)
    {
        foreach (Transform t in transforms)
        {
            if (t == null)
                return false;
        }

        return true;
    }
}