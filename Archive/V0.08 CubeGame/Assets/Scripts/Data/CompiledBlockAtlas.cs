using Game.Mathematics;
using Game.Renderer;
using System;

namespace Game.Data
{
    public static class CompiledBlockAtlas
    {
        public static BlockRenderData[] BlockAtlas { get; private set; }
        public static byte[,,] CalculatedRotationLookupTable = new byte[3, 4, 256];
        private static readonly int PredefinedBlockCount = 2;
        public static void Compile()
        {
            BlockAtlas = new BlockRenderData[PredefinedBlockCount + 256];
            PredefinedBlocks();
            CalculateLookupTable();
        }

        private static void PredefinedBlocks()
        {
            BlockAtlas[0] = new BlockRenderData(0, new UnityEngine.Vector3[0, 0], new int[0, 0], new Unity.Mathematics.int3[0],new int[0], 0b00000000);
            BlockAtlas[1] = PredefinedVertexData.CubeRenderData;
            BlockAtlas[2] = PredefinedVertexData.WedgeRenderData;
        }

        private static void CalculateLookupTable()
        {
            for (int Axis = 0; Axis < CalculatedRotationLookupTable.GetLength(0); Axis++)
                for (int Step = 0; Step < CalculatedRotationLookupTable.GetLength(1); Step++)
                    for (int Mask = 0; Mask < CalculatedRotationLookupTable.GetLength(2); Mask++)
                        CalculatedRotationLookupTable[Axis, Step, Mask] = CommonMathematics.CalculateBitmask(Axis, Step, Mask);
        }

        
    }
}