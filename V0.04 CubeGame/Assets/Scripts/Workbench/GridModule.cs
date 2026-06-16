using Game.Mathematics;
using System;
using Unity.Mathematics;
using UnityEngine;

namespace Game.Grid
{
    public class GridModule
    {
        public int3 Gridsize { get; private set; }
        public GridBlock[] BuildableGrid { get; private set; }

        public GridModule(int3 Size, int BlockPer1Meter)
        {
            Gridsize = Size * BlockPer1Meter;
            BuildableGrid = new GridBlock[Size.x * Size.y * Size.z * BlockPer1Meter * BlockPer1Meter * BlockPer1Meter];
        }

        public void SetBlock(int3 Position, int ID, Vector3 Rotation)
        {
            if (!CommonMathematics.BlockValididityChecker(Position, Gridsize)) return;
            int ArrayIndex = CommonMathematics.PositionToArray(Position, Gridsize);
            BuildableGrid[ArrayIndex].ID = ID;
            if(Rotation != Vector3.zero) BuildableGrid[ArrayIndex].Rotation = Rotation;
        }

        public void MassSetBlock(int3 Size, int3 Position, int ID,Vector3 Rotation)
        {
            for (int X = 0; X < Size.x; X++)
            {
                for (int Y = 0; Y < Size.y; Y++)
                {
                    for (int Z = 0; Z < Size.z; Z++)
                    {
                        int ArrayIndex = CommonMathematics.PositionToArray(new int3(X + Position.x, Y + Position.y, Z + Position.z), Gridsize);
                        BuildableGrid[ArrayIndex].ID = ID;
                        if (Rotation != Vector3.zero) BuildableGrid[ArrayIndex].Rotation = Rotation;
                    }
                }
            }
        }

        public void ResetGrid()
        {
            Array.Clear(BuildableGrid,0,BuildableGrid.Length);
        }
    }

    [Serializable]
    public struct GridBlock
    {
        public int ID;
        public Vector3 Rotation;
    }
}
