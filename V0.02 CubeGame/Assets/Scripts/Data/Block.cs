using Unity.Mathematics;
using UnityEngine;

namespace Game.Data
{
    public class Block
    {
        public int ID { get; private set; }
        public BlockRenderData RenderData { get; private set; }
    }

    public class BlockRenderData
    {
        public int FaceCount;

        public Vector3[,] BlockVerticies;
        public int[,] BlockTriangles;

        public int3[] FaceCheckVectors;
    }
}
