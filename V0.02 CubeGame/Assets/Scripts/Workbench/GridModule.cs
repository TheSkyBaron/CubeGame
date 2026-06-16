using Game.Mathematics;
using Unity.Mathematics;

namespace Game.Grid
{
    public class GridModule
    {
        public int3 Gridsize { get; private set; }
        public int[] BuildableGrid { get; private set; }

        public GridModule(int3 Size, int BlockPer1Meter)
        {
            Gridsize = Size * BlockPer1Meter;
            BuildableGrid = new int[Size.x * Size.y * Size.z * BlockPer1Meter * BlockPer1Meter * BlockPer1Meter];
        }

        public void SetBlock(int3 Position, int ID)
        {
            int ArrayIndex = CommonMathematics.PositionToArray(Position, Gridsize);
            BuildableGrid[ArrayIndex] = ID;
        }

        public void MassSetBlock(int3 Size, int3 Position, int ID)
        {
            for (int X = 0; X < Size.x; X++)
            {
                for (int Y = 0; Y < Size.y; Y++)
                {
                    for (int Z = 0; Z < Size.z; Z++)
                    {
                        int ArrayIndex = CommonMathematics.PositionToArray(new int3(X + Position.x, Y + Position.y, Z + Position.z), Gridsize);
                        BuildableGrid[ArrayIndex] = ID;
                    }
                }
            }
        }
    }
}
