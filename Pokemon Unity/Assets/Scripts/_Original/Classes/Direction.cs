using UnityEngine;

namespace Classes
{
    public static class Direction
    {
        public const int up = 0;
        public const int right = 1;
        public const int down = 2;
        public const int left = 3;

        public static Vector3 Vectorize(int direction)
        {
            switch (direction)
            {
                case up:
                    return Vector3.forward;
                case right:
                    return Vector3.right;
                case down:
                    return Vector3.back;
                case left:
                    return Vector3.left;
                default:
                    return Vector3.zero;
            }
        }
    }
}
