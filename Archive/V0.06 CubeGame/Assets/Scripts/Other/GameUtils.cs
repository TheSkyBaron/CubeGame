using Game.Data;
using Game.Grid;
using System;
using Unity.Mathematics;
using UnityEngine;

namespace Game.Utils
{
    public static class GameUtils
    {
        private static Mesh GeneratedMesh;

        public readonly static int[] ReverseCubeMap = new int[]
        {
         2,3,0,1,5,4
        };
        public static BlockRenderData DeepCopyBlockData(BlockRenderData Target)
        {
            Vector3[,] TempBlockVectors = new Vector3[Target.BlockVerticies.GetLength(0), Target.BlockVerticies.GetLength(1)];
            for (int i = 0; i < Target.BlockVerticies.GetLength(0); i++)
                for (int j = 0; j < Target.BlockVerticies.GetLength(1); j++)
                    TempBlockVectors[i, j] = Target.BlockVerticies[i, j];

            int[,] TempBlockTriangles = new int[Target.BlockTriangles.GetLength(0), Target.BlockTriangles.GetLength(1)];
            for (int i = 0; i < Target.BlockTriangles.GetLength(0); i++)
                for (int j = 0; j < Target.BlockTriangles.GetLength(1); j++)
                    TempBlockTriangles[i, j] = Target.BlockTriangles[i, j];

            int3[] TempBlockFaceCheckVectors = new int3[Target.FaceCheckVectors.Length];
            for (int i = 0; i < Target.FaceCheckVectors.Length; i++)
                TempBlockFaceCheckVectors[i] = Target.FaceCheckVectors[i];
            FaceCullingTypes[] TempFaceCulling = new FaceCullingTypes[Target.FaceTypes.Length];
            for(int i = 0; i < Target.FaceTypes.Length;i++)
            {
                TempFaceCulling[i] = Target.FaceTypes[i];
            }

            return new BlockRenderData(Target.FaceCount, TempBlockVectors, TempBlockTriangles, TempBlockFaceCheckVectors,TempFaceCulling);
        }

        public static bool BlockValididityChecker(int3 Position, int3 GridSize)
        {
            if (Position.x < 0 || Position.y < 0 || Position.z < 0) return false;
            if (Position.x >= GridSize.x || Position.y >= GridSize.y || Position.z >= GridSize.z) return false;
            return true;
        }
        public static bool BlockFaceRenderCheck(int3 AdjentBlockPosition , int3 Gridsize, GridBlock[] BuildableArray, int AdjectBlockIndex,BlockRenderData CurrentBlock,int CurrentFace)
        {
            if (CurrentBlock.FaceCheckVectors[CurrentFace].Equals(new int3(-1, -1, -1))) return true;
            if (!BlockValididityChecker(AdjentBlockPosition, Gridsize)) return true;
            if (BuildableArray[AdjectBlockIndex].ID == 0) return true;
            FaceCullingTypes FaceCheck = CompiledBlockAtlas.BlockAtlas[BuildableArray[AdjectBlockIndex].ID].FaceTypes[ReverseCubeMap[CurrentFace]];
            if (!FaceCheck.Equals(CurrentBlock.FaceTypes[CurrentFace]) && !FaceCheck.Equals(FaceCullingTypes.Full)) return true;
            return false;
        }

        public static Mesh SingleBlockRenderer(int BlockID)
        {
            if (BlockID == 0) return null;
            BlockRenderData NewData = DeepCopyBlockData(CompiledBlockAtlas.BlockAtlas[BlockID]);
            if (NewData.FaceCount == 0) return null;
            Vector3[] Verticies = new Vector3[NewData.BlockVerticies.Length];
            int[] Triangles = new int[NewData.BlockTriangles.Length];
            int VerticiesCount = 0;
            int TrianglesCount = 0;
            for (int FaceIndex = 0; FaceIndex < NewData.FaceCount; FaceIndex++)
            {
                int VertStartIndex = VerticiesCount;
                for (int VertIndex = 0; VertIndex < NewData.BlockVerticies.GetLength(1); VertIndex++)
                {
                    if (NewData.BlockVerticies[FaceIndex, VertIndex] == null) continue;
                    Verticies[VerticiesCount + VertIndex] = NewData.BlockVerticies[FaceIndex, VertIndex] - new Vector3(0.5f, 0.5f, 0.5f);
                }

                for (int TriIndex = 0; TriIndex < NewData.BlockTriangles.GetLength(1); TriIndex++)
                {
                    if (NewData.BlockTriangles[FaceIndex, TriIndex] == -1) continue;
                    Triangles[TrianglesCount + TriIndex] = NewData.BlockTriangles[FaceIndex, TriIndex] + VertStartIndex;
                }
                VerticiesCount += NewData.BlockVerticies.GetLength(1);
                TrianglesCount += NewData.BlockTriangles.GetLength(1);
            }
            if (GeneratedMesh != null)
            {
                GeneratedMesh.Clear();
                GeneratedMesh.vertices = Verticies;
                GeneratedMesh.triangles = Triangles;
            }
            else
            {
                GeneratedMesh = new()
                {
                    vertices = Verticies,
                    triangles = Triangles
                };
            }
            GeneratedMesh.RecalculateNormals();
            GeneratedMesh.name = "Generated Mesh";
            return GeneratedMesh;
        }
    }

    [Serializable]
    public struct Byte3
    {
        public byte x;
        public byte y;
        public byte z;

        public Byte3(byte X, byte Y, byte Z)
        {
            x = X;
            y = Y;
            z = Z;
        }

        public static readonly Byte3 Zero = new(0, 0, 0);

        public readonly Vector3 ToVector3()
        {
            return new Vector3(x, y, z);
        }

        public readonly bool Equals(Byte3 other)
        {
            if (x == other.x && y == other.y && z == other.z) return true;
            return false;
        }
    }
}