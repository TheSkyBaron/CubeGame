using Game.Utils;
using Unity.Mathematics;
using UnityEngine;

namespace Game.Mathematics
{
    public static class CommonMathematics
    {
        public static int PositionToArray(int3 Position, int3 GridSize)
        {
            int CalculatedPosition = (Position.y * GridSize.z * GridSize.x) + (Position.z * GridSize.x) + Position.x;

            return CalculatedPosition;
        }

        public static int3 ArrayToPosition(int ArrayIndex, int3 Gridsize)
        {
            int3 CalculatedPosition;

            CalculatedPosition.y = ArrayIndex / (Gridsize.z * Gridsize.x);
            ArrayIndex -= CalculatedPosition.y * Gridsize.z * Gridsize.x;
            CalculatedPosition.z = ArrayIndex / Gridsize.y;
            CalculatedPosition.x = ArrayIndex % Gridsize.y;
            return CalculatedPosition;
        }

        public static Vector3 Byte3toAngle(Byte3 Input, float AnglePerStep)
        {
            return new Vector3((AnglePerStep * Input.x) % 360, (AnglePerStep * Input.y) % 360, (AnglePerStep * Input.z) % 360);
        }
    }
}