using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;

public static class GenericUtility
{

    public static Boolean ConcatBool(Boolean[] compares)
    {
        Boolean output = false;

        for (int i = 0; i < compares.Length; i++)
        {
            if (compares[i]) return true;
        }

        return output;
    }

    public static float ConcatAxis(float[] axis, float threashold)
    {
        float output = 0.0f;
        int counter = 0;

        for (int i = 0; i < axis.Length; i++)
        {
            if (Math.Abs(axis[i]) > threashold)
            {
                output += Mathf.Clamp(axis[i], -1.0f, 1.0f);
                counter++;
            }
        }
        counter = counter == 0 ? 1 : counter;

        //Debug.Log(string.Format("Output: {0} Raw :{1} Counter:{2}", output / counter, output, counter));
        return output / counter;
    }

    public static int LoopInput(int value, int length)
    {
        while (value < 0) value += length;
        while (value >= length) value -= length;

        return value;
    }

    public static Enum[] GetEnums (Type type)
    {
        var names = Enum.GetNames(type);
        int length = names.Length;

        var enums = new Enum[length];
        for (int i = 0; i < length; i++)
        {
            enums[i] = (Enum) Enum.Parse(type, names[i]);
        }

        return enums;

    }

    public static Enum GetRandom(this Enum[] array)
    {
        return array[UnityEngine.Random.Range(0, array.Length)];
    }

    public static object[] ExpandArray(object[] input, object insert)
    {
        List<object> list = new List<object>(input);
        list.Add(insert);
        return list.ToArray();
    }

    public static int BiggestOfTwo(int a, int b)
    {
        int result = 0;

        if (a > b) result = a;
        else result = b;

        return result;
    }

    public static bool Compare(float compared, float comparison, int way)
    {
        if (way < 0) return compared < comparison;

        if (way > 0) return compared > comparison;

        else return false;

    }

    public static string SpaceAtCapitals(string input)
    {
        return Regex.Replace(input, "([a-z])([A-Z])", "$1 $2");
    }
    public static string TrimFinalPunctuation(string input)
    {
        if (string.IsNullOrEmpty(input)) return input;

        var finalChar = input[input.Length - 1];
        if(finalChar == '.' || finalChar == ',' || finalChar == '!' || finalChar == ';' || finalChar == '?')
        {
            input = input.Remove(input.Length - 1, 1);
        }

        return input;
    }
    public static string FormatTime(float input, bool includeMiliseconds = false)
    {
        var t = input;
        int secondsRaw = (int)t;
        var miliseconds = t - secondsRaw;
        int seconds = secondsRaw % 60;
        int minutesRaw = secondsRaw / 60;
        int minutes = minutesRaw % 60;
        int hours = minutesRaw / 60;

        var output = string.Format("{0}:{1}:{2}",
            hours.ToString("00"),
            minutes.ToString("00"),
            seconds.ToString("00"));

        if (includeMiliseconds) output += $".{miliseconds.ToString("0.00").Substring(2, 2)}";
        return output;
    }
    public static string[] SplitAtFirst(this string input, char character)
    {
        for (int i = 0; i < input.Length; i++)
        {
            if (input[i] == character)
            {
                return new string[]
                {
                    input.Substring(0, i),
                    input.Substring(i + 1)
                };
            }
        }
        return new string[] { input };
    }

    public static Vector2Int Normalized(this Vector2Int v)
    {
        return new Vector2Int(Mathf.Clamp(v.x, -1, 1), Mathf.Clamp(v.y, -1, 1));
    }
    /// <summary>
    /// Cross normalizes the direction, bringing to the closest non-diagonal value 
    /// </summary>
    /// <param name="v"></param>
    /// <returns></returns>
    public static Vector2Int CrossNormalized(this Vector2Int v)
    {
        if (Mathf.Abs(v.x) > Mathf.Abs(v.y))
        {
            v = new Vector2Int(v.x, 0);
        }
        else
        {
            v = new Vector2Int(0, v.y);
        }
        return v.Normalized();
    }
    public static int Magnitude(this Vector2Int v)
    {
        return Mathf.Abs(v.x) + Mathf.Abs(v.y);
    }
    /// <summary>
    /// Returns the largest component in the vector
    /// </summary>
    /// <param name="v"></param>
    /// <returns></returns>
    public static int Max(this Vector2Int v)
    {
        return Mathf.Abs(v.x) >= Mathf.Abs(v.y) ? v.x : v.y;
    }
    /// <summary>
    /// Returns the largest component in the vector, in its absolute value
    /// </summary>
    /// <param name="v"></param>
    /// <returns></returns>
    public static int MaxAbs(this Vector2Int v)
    {
        return Mathf.Abs(v.x) >= Mathf.Abs(v.y) ? Mathf.Abs(v.x) : Mathf.Abs(v.y);
    }
    public static Vector2Int InvertY(this Vector2Int v)
    {
        return new Vector2Int(v.x, -v.y);
    }
    public static int Distance(this Vector2Int v, Vector2Int vector)
    {
        return Mathf.Abs(v.x - vector.x) + Mathf.Abs(v.y - vector.y);
    }
    public static Vector2Int Multiply(this Vector2Int v, int value)
    {
        return new Vector2Int(v.x * value, v.y * value);
    }

    /// <summary>
    /// Returns the largest value
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public static int Max(int a, int b)
    {
        return a > b ? a : b;
    }

    public static Dictionary<string, T> LoadAsDictionary<T>(string path) where T : UnityEngine.Object
    {
        var objs = Resources.LoadAll<T>(path);
        var dict = new Dictionary<string, T>();
        for (int i = 0; i < objs.Length; i++)
        {
            dict.Add(objs[i].name, objs[i]);
        }
        return dict;
    }
    public static T GetFromDictionary<T>(this Dictionary<string, T> dict, string key) where T : UnityEngine.Object
    {
        return !string.IsNullOrEmpty(key) && dict != null && dict.ContainsKey(key) ? dict[key] : null;
    }
}
