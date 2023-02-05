using System;
using UnityEngine;

public static class Extensions
{
    public static Plane plane = new Plane(Vector3.up, 0);

    public static Vector3 to3D(this Vector2 vec)
    {
        return new Vector3(vec.x, 0, vec.y);
    }

    public static Vector2 to2D(this Vector3 vec)
    {
        return new Vector2(vec.x, vec.z);
    }

    //Assumes that the ground is a plane at height 0. 
    //FIXME: Needs to be extended to raycast against ground model. Too tired now though.
    public static Vector3 GetGroundPoint(Vector2 mousePos)
    {
        float distance;
        if (Camera.main == null)
        {
            return Vector3.zero;
        }
        Ray ray = Camera.main.ScreenPointToRay(mousePos);
        if (plane.Raycast(ray, out distance))
        {
            return ray.GetPoint(distance);
        }
        Debug.LogError("GroundPoint Raycast didnt hit ground plane, error.");
        return Vector3.zero;
    }

    public static Transform FindRecursiveByName(this Transform self, string exactName) => self.FindRecursive(child => child.name == exactName);
    public static Transform FindRecursiveByTag(this Transform self, string tag) => self.FindRecursive(child => child.tag == tag);

    public static Transform FindRecursive(this Transform self, Func<Transform, bool> selector)
    {
        // if (selector(self))
        // {
        //     return self;
        // }

        foreach (Transform child in self)
        {
            if (selector(child))
            {
                return child;
            }

            var finding = child.FindRecursive(selector);

            if (finding != null)
            {
                return finding;
            }
        }

        return null;
    }

    public static Transform GetRootParent(this Transform self)
    {
        if (self.parent == null)
        {
            return self;
        }

        return GetRootParent(self.parent);
    }

    public static bool IsSameOrChildOf(this Transform self, Transform parent)
    {
        if (parent == self)
        {
            return true;
        }
        if (self.parent == null)
        {
            return false;
        }

        return IsSameOrChildOf(self.parent, parent);
    }
}