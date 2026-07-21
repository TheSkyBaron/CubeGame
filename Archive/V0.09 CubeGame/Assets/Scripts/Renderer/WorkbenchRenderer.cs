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
                    if (!GameUtils.BlockFaceRenderCheck(AdjacentBlock, Gridsize, BuildableGrid, AdjacentBlockIndex,FaceIndex, CalcuatedData)) continue;
                    
                    // Face looking at surface so we will render
                    for (int VertexIndex = 0; VertexIndex < BlockVertexFaceLenght; VertexIndex++)
                    {
                        if (CalcuatedData.BlockVerticies2DArray[CalculatedFaceIndex, VertexIndex] == -Vector3.one) continue;
                        BlockVerticies[VertsCount + VertexIndex] = CalcuatedData.BlockVerticies2DArray[CalculatedFaceIndex, VertexIndex] + new Vector3(CalculatedBlockPosition.x, CalculatedBlockPosition.y, CalculatedBlockPosition.z);
                    }

                    for (int TriangleIndex = 0; TriangleIndex < BlockTrianglesFaceLenght; TriangleIndex++)
                    {
                        if (CalcuatedData.BlockTriangles2DArray[CalculatedFaceIndex, TriangleIndex] == -1) continue;
                        BlockTriangles[TrianglesCount + TriangleIndex] = CalcuatedData.BlockTriangles2DArray[CalculatedFaceIndex, TriangleIndex] + VertexStartIndex;
                    }
                    VertsCount += BlockVertexFaceLenght;
                    TrianglesCount += BlockTrianglesFaceLenght;
                }

            }
            Array.Resize(ref BlockVerticies, VertsCount);
            Array.Resize(ref BlockTriangles, TrianglesCount);
            return MeshGenerator(BlockVerticies, BlockTriangles);
        }

        private bool BlockChecker(GridBlock[] BlockArray, int Index)
        {
            if (BlockArray[Index].ID <= 0) return false;
            else return true;
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
        public Mesh Calculate(GridBlock[] BuildableGrid, int3 Gridsize);
    }
}