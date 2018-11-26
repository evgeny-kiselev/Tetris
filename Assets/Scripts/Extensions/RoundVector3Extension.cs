using UnityEngine;

namespace Tetris
{
    static class RoundVector3Extension
    {
        public static Vector3 Round(this Vector3 vector)
        {
            vector.x = Mathf.Round(vector.x);
            vector.y = Mathf.Round(vector.y);
            vector.z = Mathf.Round(vector.z);

            return vector;
        }
    }
}
