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
            for (int i = 0; i < Target.FaceTypes.Length; i++)
            {
                TempFaceCulling[i] = Target.FaceTypes[i];
            }

            return new BlockRenderData(Target.FaceCount, TempBlockVectors, TempBlockTriangles, TempBlockFaceCheckVectors, TempFaceCulling);
        }

        public static bool BlockValididityChecker(int3 Position, int3 GridSize)
        {
            if (Position.x < 0 || Position.y < 0 || Position.z < 0) return false;
            if (Position.x >= GridSize.x || Position.y >= GridSize.y || Position.z >= GridSize.z) return false;
            return true;
        }
        public static bool BlockFaceRenderCheck(int3 AdjentBlockPosition, int3 Gridsize, GridBlock[] BuildableArray, int AdjectBlockIndex, BlockRenderData CurrentBlock, int CurrentFace, int CurrentBlockIndex)
        {
            if (!BlockValididityChecker(AdjentBlockPosition, Gridsize)) return true; // returns true if adjent block is not valid

            if (BuildableArray[AdjectBlockIndex].ID == 0) return true; // returns true if adject block is air

            FaceCullingTypes[] FaceCheck = CurrentBlock.FaceTypes; // Sets face check the current face
            BlockRenderData AdjectRotatedFaceData = CalculateBlock(BuildableArray[AdjectBlockIndex]);

            if (FaceCheck[CurrentFace].Equals(FaceCullingTypes.Full) && AdjectRotatedFaceData.FaceTypes[AdjectRotatedFaceData.ReverseFaceCheckSquence[CurrentFace]].Equals(FaceCullingTypes.Full)) return false;

            if (FaceCheck[CurrentFace].Equals(FaceCullingTypes.None) || AdjectRotatedFaceData.FaceTypes[AdjectRotatedFaceData.ReverseFaceCheckSquence[CurrentFace]].Equals(FaceCullingTypes.None)) return true; // returns true if current face will be rendered regardless

            if (!FaceCheck[CurrentFace].Equals(FaceCullingTypes.Full) || !AdjectRotatedFaceData.FaceTypes[AdjectRotatedFaceData.ReverseFaceCheckSquence[CurrentFace]].Equals(FaceCullingTypes.Full))
            {
                if (FaceCheck[CurrentFace].Equals(FaceCullingTypes.Partial) && AdjectRotatedFaceData.FaceTypes[AdjectRotatedFaceData.ReverseFaceCheckSquence[CurrentFace]].Equals(FaceCullingTypes.Full)) return false;
                if (FaceCheck[CurrentFace].Equals(FaceCullingTypes.Full) && AdjectRotatedFaceData.FaceTypes[AdjectRotatedFaceData.ReverseFaceCheckSquence[CurrentFace]].Equals(FaceCullingTypes.Partial)) return true;
                if (BuildableArray[CurrentBlockIndex].Rotation.Equals(BuildableArray[AdjectBlockIndex].Rotation) &&
                    BuildableArray[AdjectBlockIndex].ID == BuildableArray[CurrentBlockIndex].ID) return false;
            }
            return false; // Cull face return false
        }

        public static Mesh SingleBlockRenderer(GridBlock Block)
        {
            if (Block.ID == 0) return null;
            BlockRenderData NewData = DeepCopyBlockData(CompiledBlockAtlas.BlockAtlas[Block.ID]);
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

        public static BlockRenderData CalculateBlock(GridBlock CurrentGridBlock)
        {
            BlockRenderData CalculatedBlock = DeepCopyBlockData(CompiledBlockAtlas.BlockAtlas[CurrentGridBlock.ID]);
            if (CurrentGridBlock.Rotation.Equals(Byte3.Zero) || CurrentGridBlock.ID == 1 || CurrentGridBlock.ID == 0) return CalculatedBlock;
            CalculatedBlock = BlockRotate(CurrentGridBlock.Rotation, CalculatedBlock, Vector3.one);
            CalculatedBlock = RotateFaceChecks(CalculatedBlock, CurrentGridBlock);
            CalculatedBlock = RotateFaceType(CalculatedBlock, CurrentGridBlock);
            CalculatedBlock = FaceSequenceRotate(CalculatedBlock, CurrentGridBlock);
            return CalculatedBlock;
        }

        private static BlockRenderData BlockRotate(Byte3 Rotation, BlockRenderData Original, Vector3 Scale)
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
                        for (int i = 0; i < Rotation.x; i++)
                        {
                            float OldY = CenteredVertex.y;

                            CenteredVertex.y = -CenteredVertex.z;
                            CenteredVertex.z = OldY;
                        }
                    }

                    if (Rotation.y != 0)
                    {
                        for (int i = 0; i < Rotation.y; i++)
                        {
                            float OldX = CenteredVertex.x;

                            CenteredVertex.x = CenteredVertex.z;
                            CenteredVertex.z = -OldX;
                        }
                    }

                    if (Rotation.z != 0)
                    {
                        for (int i = 0; i < Rotation.z; i++)
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

        private static BlockRenderData RotateFaceChecks(BlockRenderData Input, GridBlock BlockData)
        {
            for (int i = 0; i < Input.FaceCheckVectors.Length; i++)
            {
                if (BlockData.Rotation.x != 0)
                {
                    for (int j = 0; j < BlockData.Rotation.x; j++)
                    {
                        int OldY = Input.FaceCheckVectors[i].y;
                        Input.FaceCheckVectors[i].y = -Input.FaceCheckVectors[i].z;
                        Input.FaceCheckVectors[i].z = OldY;
                    }
                }

                if (BlockData.Rotation.y != 0)
                {
                    for (int j = 0; j < BlockData.Rotation.y; j++)
                    {
                        int OldX = Input.FaceCheckVectors[i].x;
                        Input.FaceCheckVectors[i].x = Input.FaceCheckVectors[i].z;
                        Input.FaceCheckVectors[i].z = -OldX;
                    }
                }

                if (BlockData.Rotation.z != 0)
                {
                    for (int j = 0; j < BlockData.Rotation.z; j++)
                    {
                        int OldX = Input.FaceCheckVectors[i].x;
                        Input.FaceCheckVectors[i].x = -Input.FaceCheckVectors[i].y;
                        Input.FaceCheckVectors[i].y = OldX;
                    }
                }
            }
            return Input;
        }

        private static BlockRenderData RotateFaceType(BlockRenderData Input, GridBlock Blockdata)
        {
            if (Blockdata.Rotation.x != 0)
            {
                for (int j = 0; j < Blockdata.Rotation.x; j++)
                {
                    FaceCullingTypes FrontFace = Input.FaceTypes[0];
                    Input.FaceTypes[0] = Input.FaceTypes[3];
                    Input.FaceTypes[3] = Input.FaceTypes[2];
                    Input.FaceTypes[2] = Input.FaceTypes[1];
                    Input.FaceTypes[1] = FrontFace;
                }
            }
            if (Blockdata.Rotation.y != 0)
            {
                for (int j = 0; j < Blockdata.Rotation.y; j++)
                {
                    FaceCullingTypes FrontFace = Input.FaceTypes[0];
                    Input.FaceTypes[0] = Input.FaceTypes[5];
                    Input.FaceTypes[5] = Input.FaceTypes[2];
                    Input.FaceTypes[2] = Input.FaceTypes[4];
                    Input.FaceTypes[4] = FrontFace;

                }
            }
            if (Blockdata.Rotation.z != 0)
            {
                for (int j = 0; j < Blockdata.Rotation.z; j++)
                {
                    FaceCullingTypes TopFace = Input.FaceTypes[1];
                    Input.FaceTypes[1] = Input.FaceTypes[5];
                    Input.FaceTypes[5] = Input.FaceTypes[3];
                    Input.FaceTypes[3] = Input.FaceTypes[4];
                    Input.FaceTypes[4] = TopFace;
                }
            }
            return Input;
        }

        private static FaceCullingTypes[] RotateFaceType(FaceCullingTypes[] Input, GridBlock Blockdata)
        {
            FaceCullingTypes[] FaceTypes = new FaceCullingTypes[Input.Length];
            for (int i = 0; i < Input.Length; i++)
            {
                FaceTypes[i] = Input[i];
            }
            if (Blockdata.Rotation.Equals(Byte3.Zero)) return FaceTypes;
            if (Blockdata.Rotation.x != 0)
            {
                for (int j = 0; j < Blockdata.Rotation.x; j++)
                {
                    FaceCullingTypes FrontFace = FaceTypes[0];
                    FaceTypes[0] = FaceTypes[3];
                    FaceTypes[3] = FaceTypes[2];
                    FaceTypes[2] = FaceTypes[1];
                    FaceTypes[1] = FrontFace;
                }
            }
            if (Blockdata.Rotation.y != 0)
            {
                for (int j = 0; j < Blockdata.Rotation.y; j++)
                {
                    FaceCullingTypes FrontFace = FaceTypes[0];
                    FaceTypes[0] = FaceTypes[5];
                    FaceTypes[5] = FaceTypes[2];
                    FaceTypes[2] = FaceTypes[4];
                    FaceTypes[4] = FrontFace;

                }
            }
            if (Blockdata.Rotation.z != 0)
            {
                for (int j = 0; j < Blockdata.Rotation.z; j++)
                {
                    FaceCullingTypes TopFace = FaceTypes[1];
                    FaceTypes[1] = FaceTypes[5];
                    FaceTypes[5] = FaceTypes[3];
                    FaceTypes[3] = FaceTypes[4];
                    FaceTypes[4] = TopFace;
                }
            }
            return FaceTypes;
        }

        private static BlockRenderData FaceSequenceRotate(BlockRenderData Input, GridBlock BlockData)
        {
            if (BlockData.Rotation.x != 0)
            {
                for (int i = 0; i < BlockData.Rotation.x; i++)
                {
                    int Frontface = Input.FaceCheckSquence[0];
                    Input.FaceCheckSquence[0] = Input.FaceCheckSquence[3];
                    Input.FaceCheckSquence[3] = Input.FaceCheckSquence[2];
                    Input.FaceCheckSquence[2] = Input.FaceCheckSquence[1];
                    Input.FaceCheckSquence[1] = Frontface;

                    int Backface = Input.ReverseFaceCheckSquence[0];
                    Input.ReverseFaceCheckSquence[0] = Input.ReverseFaceCheckSquence[3];
                    Input.ReverseFaceCheckSquence[3] = Input.ReverseFaceCheckSquence[2];
                    Input.ReverseFaceCheckSquence[2] = Input.ReverseFaceCheckSquence[1];
                    Input.ReverseFaceCheckSquence[1] = Backface;
                }
            }
            if (BlockData.Rotation.y != 0)
            {
                for (int i = 0; i < BlockData.Rotation.y; i++)
                {
                    int Frontface = Input.FaceCheckSquence[0];
                    Input.FaceCheckSquence[0] = Input.FaceCheckSquence[5];
                    Input.FaceCheckSquence[5] = Input.FaceCheckSquence[2];
                    Input.FaceCheckSquence[2] = Input.FaceCheckSquence[4];
                    Input.FaceCheckSquence[4] = Frontface;

                    int Backface = Input.ReverseFaceCheckSquence[0];
                    Input.ReverseFaceCheckSquence[0] = Input.ReverseFaceCheckSquence[5];
                    Input.ReverseFaceCheckSquence[5] = Input.ReverseFaceCheckSquence[2];
                    Input.ReverseFaceCheckSquence[2] = Input.ReverseFaceCheckSquence[4];
                    Input.ReverseFaceCheckSquence[4] = Backface;
                }
            }
            if (BlockData.Rotation.z != 0)
            {
                for (int i = 0; i < BlockData.Rotation.z; i++)
                {
                    int Topface = Input.FaceCheckSquence[1];
                    Input.FaceCheckSquence[1] = Input.FaceCheckSquence[5];
                    Input.FaceCheckSquence[5] = Input.FaceCheckSquence[3];
                    Input.FaceCheckSquence[3] = Input.FaceCheckSquence[4];
                    Input.FaceCheckSquence[4] = Topface;

                    int BottomFace = Input.ReverseFaceCheckSquence[1];
                    Input.ReverseFaceCheckSquence[1] = Input.ReverseFaceCheckSquence[5];
                    Input.ReverseFaceCheckSquence[5] = Input.ReverseFaceCheckSquence[3];
                    Input.ReverseFaceCheckSquence[3] = Input.ReverseFaceCheckSquence[4];
                    Input.ReverseFaceCheckSquence[4] = BottomFace;
                }
            }
            return Input;
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