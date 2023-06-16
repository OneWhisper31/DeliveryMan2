using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class Physics2DExtension
{
    public static bool InFieldOfView(this Vector3 start, Vector3 end, float viewRadius, float viewAngle, LayerMask wallLayer)
    {
        Vector3 dir = end - start;
        if (dir.magnitude > viewRadius) return false;
        if (!InLineOfSight(end, start, wallLayer)) return false;
        return Vector3.Angle(end, dir) <= viewAngle / 2;
    }

    public static bool InLineOfSight(this Vector3 start, Vector3 end, LayerMask wallLayer)
    {
        //origen, direccion, distancia maxima, layer mask
        Vector3 dir = end - start;
        return !Physics.Raycast(start, dir, wallLayer);
    }
    public static bool InLineOfSight(this Vector3 start, Vector3 end)
    {
        //origen, direccion, distancia maxima, layer mask
        Vector3 dir = end - start;
        return !Physics.Raycast(start, dir);
    }
}
