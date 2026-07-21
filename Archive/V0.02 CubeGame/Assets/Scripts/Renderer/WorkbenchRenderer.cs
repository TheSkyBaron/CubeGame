using Game.Engine;
using Game.Mathematics;
using System;
using Unity.Mathematics;
using UnityEngine;

namespace Game.Renderer
{
    public class WorkbenchRenderer : IRenderer
    {
        public Mesh Calculate(int[] BuildableGrid, int3 Gridsize)
        {
            Vector3[] BlockVerticies = new Vector3[Settings.MaxVerticies];
            int[] BlockTriangles = new int[Settings.MaxTriangles];
            int VertsCount = 0;
            int TrianglesCount = 0;
            for (int i = 0; i < BuildableGrid.Length; i++)
            {
                if (!BlockChecker(BuildableGrid, i)) continue; // Don't render non valid blocks.
                int3 CalculatedBlockPosition = CommonMathematics.ArrayToPosition(i, Gridsize);
                // Loops Throught each face
                for (int FaceIndex = 0; FaceIndex < CubeVertexData.FaceCount; FaceIndex++)
                {
                    int VertexStartIndex = VertsCount;
                    // Check if Face Needs Rendering
                    int3 AdjacentBlock = CalculatedBlockPosition + CubeVertexData.FaceCheck[FaceIndex];
                    int AdjacentBlockIndex = CommonMathematics.PositionToArray(AdjacentBlock, Gridsize);
                    if (BlockValididityChecker(AdjacentBlock, Gridsize) && BlockChecker(BuildableGrid, AdjacentBlockIndex)) continue;

                    // Face looking at surface so we will render

                    for (int VertexIndex = 0; VertexIndex < CubeVertexData.CubeVerts.GetLength(1); VertexIndex++)
                    {
                        BlockVerticies[VertsCount + VertexIndex] = CubeVertexData.CubeVerts[FaceIndex, VertexIndex]+ new Vector3(CalculatedBlockPosition.x, CalculatedBlockPosition.y, CalculatedBlockPosition.z);
                    }

                    for (int TriangleIndex = 0; TriangleIndex < CubeVertexData.CubeTriangles.GetLength(1); TriangleIndex++)
                    {
                        BlockTriangles[TrianglesCount + TriangleIndex] = CubeVertexData.CubeTriangles[FaceIndex, TriangleIndex] + VertexStartIndex;
                    }
                    VertsCount += CubeVertexData.CubeVerts.GetLength(1);
                    TrianglesCount += CubeVertexData.CubeTriangles.GetLength(1);
                }

            }
            Array.Resize(ref BlockVerticies, VertsCount);
            Array.Resize(ref BlockTriangles, TrianglesCount);
            return MeshGenerator(BlockVerticies, BlockTriangles);
        }

        private bool BlockChecker(int[] BlockArray, int Index)
        {
            if (BlockArray[Index] <= 0) return false;
            else return true;
        }

        private bool BlockValididityChecker(int3 Position, int3 GridSize)
        {
            if (Position.x < 0 || Position.y < 0 || Position.z < 0) return false;
            if (Position.x >= GridSize.x || Position.y >= GridSize.y || Position.z >= GridSize.z) return false;
            return true;
        }
        private Mesh MeshGenerator(Vector3[] Verts, int[] Triangles)
        {
            Mesh GeneratedMesh = new()
            {
                indexFormat = UnityEngine.Rendering.IndexFormat.UInt32,
                vertices = Verts,
                triangles = Triangles
            };

            GeneratedMesh.RecalculateNormals();
            GeneratedMesh.name = "Generated Mesh";
            return GeneratedMesh;
        }
    }

    public interface IRenderer
    {
        public Mesh Calculate(int[] BuildableGrid, int3 Gridsize);
    }
}