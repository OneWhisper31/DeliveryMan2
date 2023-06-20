using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class Physics2DExtension
{
    public static bool InFieldOfView(this Vector2 start, Vector2 end, float viewRadius, float viewAngle, LayerMask wallLayer)
    {
        Vector2 dir = end - start;
        if (dir.magnitude > viewRadius) return false;
        if (!InLineOfSight(end, start, wallLayer)) return false;
        return Vector3.Angle(end, dir) <= viewAngle / 2;
    }

    public static bool InLineOfSight(this Vector2 start, Vector2 end, LayerMask wallLayer)
    {
        //origen, direccion, distancia maxima, layer mask
        Vector2 dir = end - start;
        return !Physics2D.Raycast(start, dir,dir.magnitude, wallLayer);
    }
    public static bool InLineOfSight(this Vector2 start, Vector2 end)
    {
        //origen, direccion, distancia maxima, layer mask
        Vector3 dir = end - start;
        return !Physics.Raycast(start, dir,dir.magnitude);
    }
}
