using Game.Data;
using Game.Engine;
using Game.Grid;
using Game.Mathematics;
using Game.Utils;
using System;
using Unity.Mathematics;
using UnityEngine;

namespace Game.Renderer
{
    public class WorkbenchRenderer : IRenderer
    {
        public Mesh Calculate(GridBlock[] BuildableGrid, int3 Gridsize)
        {
            Vector3[] BlockVerticies = new Vector3[Settings.MaxVerticies];
            int[] BlockTriangles = new int[Settings.MaxTriangles];
            Vector2[] BlockUV = new Vector2[Settings.MaxVerticies];
            int VertsCount = 0;
            int TrianglesCount = 0;
            for (int i = 0; i < BuildableGrid.Length; i++)
            {
                if (!BlockChecker(BuildableGrid, i)) continue; // Don't render non valid blocks.
                int3 CalculatedBlockPosition = CommonMathematics.ArrayToPosition(i, Gridsize);
                BlockRenderData CalcuatedData = GameUtils.RotateBlock(CompiledBlockAtlas.BlockAtlas[BuildableGrid[i].ID], BuildableGrid[i]);
                int BlockVertexFaceLenght = CalcuatedData.BlockVerticies2DArray.GetLength(1);
                int BlockTrianglesFaceLenght = CalcuatedData.BlockTriangles2DArray.GetLength(1);
                // Loops Throught each face
                for (int FaceIndex = 0; FaceIndex < CompiledBlockAtlas.BlockAtlas[BuildableGrid[i].ID].FaceCount; FaceIndex++)
                {
                    int CalculatedFaceIndex = CalcuatedData._FaceCheckSteps[FaceIndex];
                    int VertexStartIndex = VertsCount;

                    // Check if Face Needs Rendering
                    int3 AdjacentBlock = CalculatedBlockPosition + CalcuatedData.FaceCheckVectors[CalculatedFaceIndex];
                    int AdjacentBlockIndex = CommonMathematics.PositionToArray(AdjacentBlock, Gridsize);
                    if (!GameUtils.BlockFaceRenderCheck(AdjacentBlock, Gridsize, BuildableGrid, AdjacentBlockIndex, FaceIndex, CalcuatedData)) continue;

                    // Face looking at surface so we will render
                    for (int VertexIndex = 0; VertexIndex < BlockVertexFaceLenght; VertexIndex++)
                    {
                        if (CalcuatedData.BlockVerticies2DArray[CalculatedFaceIndex, VertexIndex] == Settings.NullVector3) continue;
                        BlockVerticies[VertsCount + VertexIndex] = CalcuatedData.BlockVerticies2DArray[CalculatedFaceIndex, VertexIndex] + new Vector3(CalculatedBlockPosition.x, CalculatedBlockPosition.y, CalculatedBlockPosition.z);
                    }

                    for (int TriangleIndex = 0; TriangleIndex < BlockTrianglesFaceLenght; TriangleIndex++)
                    {
                        if (CalcuatedData.BlockTriangles2DArray[CalculatedFaceIndex, TriangleIndex] == Settings.NullInt) continue;
                        BlockTriangles[TrianglesCount + TriangleIndex] = CalcuatedData.BlockTriangles2DArray[CalculatedFaceIndex, TriangleIndex] + VertexStartIndex;
                    }
                    for(int UVIndex = 0; UVIndex < BlockVertexFaceLenght;UVIndex++)
                    {
                        if (CalcuatedData.BlockVerticies2DArray[CalculatedFaceIndex,UVIndex] == Settings.NullVector3) continue;
                        BlockUV[VertsCount + UVIndex] = CommonMathematics.CalculateUV(CalcuatedData.BlockVerticies2DArray[CalculatedFaceIndex,UVIndex],FaceIndex);
                    }

                    VertsCount += BlockVertexFaceLenght;
                    TrianglesCount += BlockTrianglesFaceLenght;
                }

            }
            Array.Resize(ref BlockVerticies, VertsCount);
            Array.Resize(ref BlockTriangles, TrianglesCount);
            Array.Resize(ref BlockUV, VertsCount);
            return MeshGenerator(BlockVerticies, BlockTriangles,BlockUV);
        }

        private bool BlockChecker(GridBlock[] BlockArray, int Index)
        {
            if (BlockArray[Index].ID <= 0) return false;
            else return true;
        }


        private Mesh MeshGenerator(Vector3[] Verts, int[] Triangles, Vector2[] UV)
        {
            Mesh GeneratedMesh = new()
            {
                indexFormat = UnityEngine.Rendering.IndexFormat.UInt32,
                vertices = Verts,
                triangles = Triangles,
                uv = UV
            };

            GeneratedMesh.RecalculateNormals();
            GeneratedMesh.name = "Generated Mesh";
            return GeneratedMesh;
        }
    }


    public interface IRenderer
    {
        public Mesh Calculate(GridBlock[] BuildableGrid, int3 Gridsize);
    }
}