using Unity.Mathematics;

namespace Game.Mathematics
{
    public static class CommonMathematics
    {
        public static int PositionToArray (int3 Position,int3 GridSize)
        {
            int CalculatedPosition = (Position.y * GridSize.z * GridSize.x ) + (Position.z * GridSize.x) + Position.x;
            
            return CalculatedPosition;
        }

        public static int3 ArrayToPosition (int ArrayIndex,int3 Gridsize)
        {
            int3 CalculatedPosition;

            CalculatedPosition.y = ArrayIndex / (Gridsize.z * Gridsize.x);
            ArrayIndex -= CalculatedPosition.y * Gridsize.z * Gridsize.x;
            CalculatedPosition.z = ArrayIndex / Gridsize.y;
            CalculatedPosition.x = ArrayIndex % Gridsize.y;
            return CalculatedPosition;
        }

        public static bool BlockValididityChecker(int3 Position, int3 GridSize)
        {
            if (Position.x < 0 || Position.y < 0 || Position.z < 0) return false;
            if (Position.x >= GridSize.x || Position.y >= GridSize.y || Position.z >= GridSize.z) return false;
            return true;
        }
    }
}
