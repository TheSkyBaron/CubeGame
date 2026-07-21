using Game.Mathematics;
using Game.Renderer;
using Game.UI;
using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Game.Data
{
    public static class CompiledBlockAtlas
    {
        public static BlockRenderData[] BlockAtlas { get; private set; }
        public static Sprite BlockImageAtlas { get; private set; }
        public static byte[,,] CalculatedRotationLookupTable = new byte[3, 4, 256];
        public static readonly int PredefinedBlockCount = 3;
        public static int LoadingBarPercentage { get; private set; } = 10;
        public static void Compile()
        {
            // Block Generation
            BlockAtlas = new BlockRenderData[PredefinedBlockCount + 256];
            PredefinedBlocks();

            // Table Generation
            CalculateLookupTable();

            // SpriteAtlasLoad
            LoadSprites();

            // Inventory System
            InventoryPlanner.CalculateInventoryGroup();
        }

        private static Task PredefinedBlocks()
        {

            BlockAtlas[0] = new BlockRenderData(Blocktypes.Air, new Vector3[0, 0], new int[0, 0], new Unity.Mathematics.int3[0], 0b00000000);
            BlockAtlas[1] = PredefinedVertexData.CubeRenderData;
            BlockAtlas[2] = PredefinedVertexData.WedgeRenderData;

            return Task.CompletedTask;
        }

        private static Task CalculateLookupTable()
        {
            for (int Axis = 0; Axis < CalculatedRotationLookupTable.GetLength(0); Axis++)
                for (int Step = 0; Step < CalculatedRotationLookupTable.GetLength(1); Step++)
                    for (int Mask = 0; Mask < CalculatedRotationLookupTable.GetLength(2); Mask++)
                        CalculatedRotationLookupTable[Axis, Step, Mask] = CommonMathematics.CalculateBitmask(Axis, Step, Mask);
            return Task.CompletedTask;
        }

        private static Task LoadSprites()
        {
            for (int i = 0; i < BlockAtlas.Length; i++)
            {

            }
            return Task.CompletedTask;
        }
        public async static Task<string> AsyncCompile()
        {
            string Status = "Compiler completed operation. Errors:\n";

            // Block Generation
            BlockAtlas = new BlockRenderData[PredefinedBlockCount + 256];
            try
            {
                await PredefinedBlocks();
            }
            catch
            {
                Status += "Predefined blocks loading failed!\n";
            }
            LoadingBarPercentage = 25;

            // Table Generation
            try
            {
                await CalculateLookupTable();
            }
            catch
            {
                Status += "Calculated Lookup Table calculation failed!\n";
            }
            LoadingBarPercentage = 50;
            // SpriteAtlasLoad
            try
            {
                await LoadSprites();
            }
            catch
            {
                Status += "Sprite loading failed!\n";
            }
            LoadingBarPercentage = 75;
            // Inventory System
            try
            {
                await InventoryPlanner.CalculateInventoryGroup();
            }
            catch
            {
                Status += "Block sorting for inventory failed!\n";
            }
            LoadingBarPercentage = 100;
            return Status;
        }
    }
}