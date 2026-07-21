using Unity.Mathematics;
using UnityEngine;

namespace Game.Data
{
    public enum Blocktypes {Block,Land,Air,Sea,Weapons,Decorations,Computers};
    public class BlockRenderData
    {
        public int FaceCount; // Total face Count of the object
        public Blocktypes BlockType; // Type of the block.
        public Vector3[,] BlockVerticies2DArray; // [Face,Verts assigned to the face]
        public int[,] BlockTriangles2DArray; // [Face,Triangles assigned to the face]

        public int3[] FaceCheckVectors; // [what voxels we need to check (this voxel is the origin)]

        public byte _FaceBitmask; // Bitmask used for culling calculations

        public int[] _FaceCheckSteps; // Engine Face Check squence. This changes with block rotation

        public BlockRenderData (int _FaceCount,Blocktypes Type, Vector3[,] _BlockVerticies, int[,] _BlockTriangles, int3[] _FaceCheckVectors, int[] FaceCheckSteps,byte FaceBitMask)
        {
            FaceCount = _FaceCount;
            BlockType = Type;
            BlockVerticies2DArray = _BlockVerticies;
            BlockTriangles2DArray = _BlockTriangles;
            FaceCheckVectors = _FaceCheckVectors;
            _FaceBitmask = FaceBitMask; //(0,0,0),(1,0,0),(0,1,0),(1,1,0),(0,0,1),(1,0,1),(0,1,1),(1,1,1) These are squence of the verts for bitmask bits
            _FaceCheckSteps = FaceCheckSteps;
        }

        // Small but important note -1 vectors3 (-1,-1,-1) etc will be ignored so make sure left front corner is (0,0,0)! Also triangles with -1 are excluded in rendering.
    }
}