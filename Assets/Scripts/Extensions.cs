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
        Ray ray = Camera.main.ScreenPointToRay(mousePos);
        if (plane.Raycast(ray, out distance))
        {
            return ray.GetPoint(distance);
        }
        Debug.LogError("GroundPoint Raycast didnt hit ground plane, error.");
        return Vector3.zero;
    }
}