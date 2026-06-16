using Unity.Mathematics;
using UnityEngine;

namespace Game.Data
{
    public enum FaceCullingTypes {Full,Partial,None};
    public class BlockRenderData
    {
        public readonly int FaceCount;

        public readonly Vector3[,] BlockVerticies;
        public readonly int[,] BlockTriangles;

        public readonly int3[] FaceCheckVectors;

        public readonly FaceCullingTypes[] FaceTypes;

        public BlockRenderData (int _FaceCount, Vector3[,] _BlockVerticies, int[,] _BlockTriangles, int3[] _FaceCheckVectors, FaceCullingTypes[] faceTypes)
        {
            FaceCount = _FaceCount;
            BlockVerticies = _BlockVerticies;
            BlockTriangles = _BlockTriangles;
            FaceCheckVectors = _FaceCheckVectors;
            FaceTypes = faceTypes;
        }

        // Small but important note -1 vectors3 (-1,-1,-1) etc will be ignored so make sure left front corner is (0,0,0)! Also triangles are included (-1).
    }
}