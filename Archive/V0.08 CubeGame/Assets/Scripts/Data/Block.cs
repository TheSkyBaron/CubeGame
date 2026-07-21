using Unity.Mathematics;
using UnityEngine;

namespace Game.Data
{
    public class BlockRenderData
    {
        public int FaceCount; // Total face Count of the object

        public Vector3[,] BlockVerticies; // [Face,Verts assigned to the face]
        public int[,] BlockTriangles; // [Face,Triangles assigned to the face]

        public int3[] FaceCheckVectors; // [what voxels we need to check (this voxel is the origin)]

        public byte _FaceBitmask; // Bitmask used for culling calculations

        public int[] _FaceCheckSteps;

        public BlockRenderData (int _FaceCount, Vector3[,] _BlockVerticies, int[,] _BlockTriangles, int3[] _FaceCheckVectors, int[] FaceCheckSteps,byte FaceBitMask)
        {
            FaceCount = _FaceCount;
            BlockVerticies = _BlockVerticies;
            BlockTriangles = _BlockTriangles;
            FaceCheckVectors = _FaceCheckVectors;
            _FaceBitmask = FaceBitMask; //(0,0,0),(1,0,0),(0,1,0),(1,1,0),(0,0,1),(1,0,1),(0,1,1),(1,1,1) These are squence of the verts for bitmask bits
            _FaceCheckSteps = FaceCheckSteps;
        }

        // Small but important note -1 vectors3 (-1,-1,-1) etc will be ignored so make sure left front corner is (0,0,0)! Also triangles with -1 are excluded in rendering.
        // Well, Lets fix this (attemp 5) -Sky
    }
}