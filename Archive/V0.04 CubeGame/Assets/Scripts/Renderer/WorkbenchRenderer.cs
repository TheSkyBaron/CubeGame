using Game.Data;
using Game.Engine;
using Game.Grid;
using Game.Mathematics;
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
                // Loops Throught each face
                for (int FaceIndex = 0; FaceIndex < CompiledBlockAtlas.BlockAtlas[BuildableGrid[i].ID].FaceCount; FaceIndex++)
                {
                    int VertexStartIndex = VertsCount;
                    // Check if Face Needs Rendering
                    int3 AdjacentBlock = CalculatedBlockPosition + CompiledBlockAtlas.BlockAtlas[BuildableGrid[i].ID].FaceCheckVectors[FaceIndex];
                    int AdjacentBlockIndex = CommonMathematics.PositionToArray(AdjacentBlock, Gridsize);
                    if (CommonMathematics.BlockValididityChecker(AdjacentBlock, Gridsize) && BlockChecker(BuildableGrid, AdjacentBlockIndex)) continue;

                    // Face looking at surface so we will render

                    for (int VertexIndex = 0; VertexIndex < CompiledBlockAtlas.BlockAtlas[BuildableGrid[i].ID].BlockVerticies.GetLength(1); VertexIndex++)
                    {
                        BlockVerticies[VertsCount + VertexIndex] = CompiledBlockAtlas.BlockAtlas[BuildableGrid[i].ID].BlockVerticies[FaceIndex, VertexIndex] + new Vector3(CalculatedBlockPosition.x, CalculatedBlockPosition.y, CalculatedBlockPosition.z);
                    }

                    for (int TriangleIndex = 0; TriangleIndex < CompiledBlockAtlas.BlockAtlas[BuildableGrid[i].ID].BlockTriangles.GetLength(1); TriangleIndex++)
                    {
                        BlockTriangles[TrianglesCount + TriangleIndex] = CompiledBlockAtlas.BlockAtlas[BuildableGrid[i].ID].BlockTriangles[FaceIndex, TriangleIndex] + VertexStartIndex;
                    }
                    VertsCount += CompiledBlockAtlas.BlockAtlas[BuildableGrid[i].ID].BlockVerticies.GetLength(1);
                    TrianglesCount += CompiledBlockAtlas.BlockAtlas[BuildableGrid[i].ID].BlockTriangles.GetLength(1);
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

        private BlockRenderData BlockRotate(Vector3 Rotation, BlockRenderData Original, Vector3 Scale)
        {
            BlockRenderData RotatedBlock = Original;

            Vector3 ObjectCenter = new(0.5f * Scale.x, 0.5f * Scale.y, 0.5f * Scale.z);

            for (int FaceIndex = 0; FaceIndex < Original.BlockVerticies.GetLength(0); FaceIndex++)
            {
                for (int VertexIndex = 0; VertexIndex < Original.BlockVerticies.GetLength(1); VertexIndex++)
                {
                    Vector3 CenteredVertex = Original.BlockVerticies[FaceIndex, VertexIndex] - ObjectCenter;

                    if (Rotation.x != 0)
                    {
                        for (int i = 0; i < (Rotation.x / 90); i++)
                        {
                            float OldY = CenteredVertex.y;

                            CenteredVertex.y = -CenteredVertex.z;
                            CenteredVertex.z = OldY;
                        }
                    }

                    if (Rotation.y != 0)
                    {
                        for (int i = 0; i < (Rotation.y / 90); i++)
                        {
                            float OldX = CenteredVertex.x;

                            CenteredVertex.x = CenteredVertex.z;
                            CenteredVertex.z = -OldX;
                        }
                    }

                    if (Rotation.z != 0)
                    {
                        for (int i = 0; i < (Rotation.z / 90); i++)
                        {
                            float OldX = CenteredVertex.x;

                            CenteredVertex.x = -CenteredVertex.y;
                            CenteredVertex.y = OldX;
                        }
                    }

                    CenteredVertex += ObjectCenter;

                    CenteredVertex.x = MathF.Round(CenteredVertex.x);
                    CenteredVertex.y = MathF.Round(CenteredVertex.y);
                    CenteredVertex.z = MathF.Round(CenteredVertex.z);

                    RotatedBlock.BlockVerticies[FaceIndex, VertexIndex] = CenteredVertex;
                }
            }
            return RotatedBlock;
        }

        private BlockRenderData CalculateBlock(GridBlock CurrentGridBlock)
        {
            if (CurrentGridBlock.Rotation == Vector3.zero) return CompiledBlockAtlas.BlockAtlas[CurrentGridBlock.ID];
            BlockRenderData CalculatedBlock = BlockRotate(CurrentGridBlock.Rotation, CompiledBlockAtlas.BlockAtlas[CurrentGridBlock.ID], Vector3.one);
            CalculatedBlock = RotateFaceChecks(CalculatedBlock,CurrentGridBlock);
            return CalculatedBlock;
        }

        private BlockRenderData RotateFaceChecks(BlockRenderData Input, GridBlock BlockData)
        {
            for (int i = 0; i < Input.FaceCheckVectors.Length; i++)
            {
                if (BlockData.Rotation.x != 0)
                {
                    for(int j = 0; j < (BlockData.Rotation.x/90);j++)
                    {
                        int OldY = Input.FaceCheckVectors[i].y;
                        Input.FaceCheckVectors[i].y = -Input.FaceCheckVectors[i].z;
                        Input.FaceCheckVectors[i].z = OldY;
                    }

                }

                if (BlockData.Rotation.y != 0)
                {
                    for(int j = 0; j < (BlockData.Rotation.y/90);j++)
                    {
                        int OldX = Input.FaceCheckVectors[i].x;
                        Input.FaceCheckVectors[i].x = Input.FaceCheckVectors[i].z;
                        Input.FaceCheckVectors[i].z = -OldX;
                    }
                }

                if(BlockData.Rotation.z != 0)
                {
                    for(int j = 0; j < (BlockData.Rotation.z/90);j++)
                    {
                        int OldX = Input.FaceCheckVectors[i].x;
                        Input.FaceCheckVectors[i].x = -Input.FaceCheckVectors[i].y;
                        Input.FaceCheckVectors[i].y = OldX;
                    }
                }
            }
            return Input;
        }

    }


    public interface IRenderer
    {
        public Mesh Calculate(GridBlock[] BuildableGrid, int3 Gridsize);
    }
}