/*
 * Created by Hugo Uchoas Borges <hugouchoas@outlook.com>
 */

namespace util
{
    public class Util
    {
        public static bool IsEditor()
        {
#if UNITY_EDITOR
            return true;
#endif
            return false;
        }

        public static bool IsWebGL()
        {
#if UNITY_WEBGL
            return true;
#endif
            return false;
        }

        public static bool IsWindows()
        {
#if UNITY_WINDOWS
            return true;
#endif
            return false;
        }
    }
}
