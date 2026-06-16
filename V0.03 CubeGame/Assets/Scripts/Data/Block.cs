using Unity.Mathematics;
using UnityEngine;

namespace Game.Data
{
    public class Block
    {
        public readonly int ID;
        public readonly BlockRenderData RenderData;

        public Block(int _ID, BlockRenderData _RenderData)
        {
            ID = _ID;
            RenderData = _RenderData;
        }
    }

    public class BlockRenderData
    {
        public readonly int FaceCount;

        public readonly Vector3[,] BlockVerticies;
        public readonly int[,] BlockTriangles;

        public readonly int3[] FaceCheckVectors;

        public BlockRenderData (int _FaceCount, Vector3[,] _BlockVerticies, int[,] _BlockTriangles, int3[] _FaceCheckVectors)
        {
            FaceCount = _FaceCount;
            BlockVerticies = _BlockVerticies;
            BlockTriangles = _BlockTriangles;
            FaceCheckVectors = _FaceCheckVectors;
        }
    }
}
