using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class VectorUtility
{
    public static Vector2 LerpInt(Vector2 a, Vector2 b, float t)
    {
        var vector = Vector2.Lerp(a, b, t);
        vector.x = (int)vector.x;
        vector.y = (int)vector.y;

        return vector;
    }

    public static Vector2Int ParseVector2Int (string serialized)
    {
        serialized = serialized.Replace("(", "").Replace(")", "");
        var split = serialized.Split(',');

        if (split.Length < 2)
        {
            Debug.LogError($"Wrong format string {serialized} was passed");
            return Vector2Int.zero;
        }

        return new Vector2Int(int.Parse(split[0]), int.Parse(split[1]));
    }
}
