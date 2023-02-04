using UnityEngine;

public static class Extensions
{
    public static Vector3 to3D(this Vector2 vec)
    {
        return new Vector3(vec.x, 0, vec.y);
    }

    public static Vector2 to2D(this Vector3 vec)
    {
        return new Vector2(vec.x, vec.z);
    }
}