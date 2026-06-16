using Game.Data;
using Game.Grid;
using Game.Mathematics;
using System;
using Unity.Mathematics;
using UnityEngine;

namespace Game.Utils
{
    public static class GameUtils
    {
        private static int[] Reversefaceorder = new int[] {2,3,0,1,5,4};
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
            int[] TempFaceCheckSteps = new int[Target._FaceCheckSteps.Length];
            for(int i = 0;i < Target._FaceCheckSteps.Length;i++)
            {
                TempFaceCheckSteps[i] = Target._FaceCheckSteps[i];
            }

            return new BlockRenderData(Target.FaceCount, TempBlockVectors, TempBlockTriangles, TempBlockFaceCheckVectors,TempFaceCheckSteps, Target._FaceBitmask);
        }

        public static bool BlockValididityChecker(int3 Position, int3 GridSize)
        {
            if (Position.x < 0 || Position.y < 0 || Position.z < 0) return false;
            if (Position.x >= GridSize.x || Position.y >= GridSize.y || Position.z >= GridSize.z) return false;
            return true;
        }
        public static bool BlockFaceRenderCheck(int3 AdjentBlockPosition, int3 Gridsize, GridBlock[] BuildableArray, int AdjectBlockIndex, int CurrentFace, BlockRenderData CurrentBlock)
        {
            if (CurrentBlock.FaceCheckVectors[CurrentFace].Equals(new int3(-1, -1, -1))) return true;
            if (!BlockValididityChecker(AdjentBlockPosition, Gridsize)) return true; // returns true if adjent block is not valid
            if (BuildableArray[AdjectBlockIndex].ID == 0) return true; // returns true if adject block is air

            byte CurrentFaceByte = CurrentBlock._FaceBitmask;
            byte AdjentFaceByte = RotateBytemask(CompiledBlockAtlas.BlockAtlas[BuildableArray[AdjectBlockIndex].ID]._FaceBitmask, BuildableArray[AdjectBlockIndex].Rotation);
            switch (CurrentFace)
            {
                case 0: // Front Face
                    CurrentFaceByte &= 0b11110000;
                    AdjentFaceByte &= 0b00001111;
                    AdjentFaceByte = (byte)(AdjentFaceByte << 4);
                    if ((CurrentFaceByte & AdjentFaceByte) == CurrentFaceByte) return false;
                    else return true;
                case 1: // Top Face
                    CurrentFaceByte &= 0b00110011;
                    AdjentFaceByte &= 0b11001100;
                    AdjentFaceByte = (byte)(AdjentFaceByte >> 2);
                    if ((CurrentFaceByte & AdjentFaceByte) == CurrentFaceByte) return false;
                    else return true;
                case 2: // Back Face
                    CurrentFaceByte &= 0b00001111;
                    AdjentFaceByte &= 0b11110000;
                    AdjentFaceByte = (byte)(AdjentFaceByte >> 4);
                    if ((CurrentFaceByte & AdjentFaceByte) == CurrentFaceByte) return false;
                    else return true;
                case 3: // Bottom Face
                    CurrentFaceByte &= 0b11001100;
                    AdjentFaceByte &= 0b00110011;
                    AdjentFaceByte = (byte)(AdjentFaceByte << 2);
                    if ((CurrentFaceByte & AdjentFaceByte) == CurrentFaceByte) return false;
                    else return true;
                case 4: // Left Face
                    CurrentFaceByte &= 0b10101010;
                    AdjentFaceByte &= 0b01010101;
                    AdjentFaceByte = (byte)(AdjentFaceByte << 1);
                    if ((CurrentFaceByte & AdjentFaceByte) == CurrentFaceByte) return false;
                    else return true;
                case 5: // Right Face
                    CurrentFaceByte &= 0b01010101;
                    AdjentFaceByte &= 0b10101010;
                    AdjentFaceByte = (byte)(AdjentFaceByte >> 1);
                    if ((CurrentFaceByte & AdjentFaceByte) == CurrentFaceByte) return false;
                    else return true;
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

        public static BlockRenderData RotateBlock(BlockRenderData TargetBlock, GridBlock TargetData)
        {
            BlockRenderData DeepCopiedData = DeepCopyBlockData(TargetBlock);
            DeepCopiedData = RotateVertexData(DeepCopiedData, TargetData);
            DeepCopiedData = RotateAdjentBlockOffset(DeepCopiedData, TargetData);
            DeepCopiedData._FaceBitmask = RotateBytemask(TargetBlock._FaceBitmask, TargetData.Rotation);
            DeepCopiedData._FaceCheckSteps = FaceCheckStepRotation(DeepCopiedData._FaceCheckSteps,TargetData);
            return DeepCopiedData;
        }

        private static byte RotateBytemask(byte Mask, Byte3 Rotation)
        {

            // Normalise
            Byte3 NormalisedRotation = new()
            {
                x = (byte)(((Rotation.x % 4) + 4) % 4),
                y = (byte)(((Rotation.y % 4) + 4) % 4),
                z = (byte)(((Rotation.z % 4) + 4) % 4)
            };
            // Copy Mask
            byte Result = Mask;

            // Rotate Mask
            if (NormalisedRotation.x != 0) Result = CompiledBlockAtlas.CalculatedRotationLookupTable[0, NormalisedRotation.x, Result];
            if (NormalisedRotation.y != 0) Result = CompiledBlockAtlas.CalculatedRotationLookupTable[1, NormalisedRotation.y, Result];
            if (NormalisedRotation.z != 0) Result = CompiledBlockAtlas.CalculatedRotationLookupTable[2, NormalisedRotation.z, Result];

            return Result;
        }

        private static BlockRenderData RotateVertexData(BlockRenderData InputData, GridBlock BlockData)
        {
            for (int FaceIndex = 0; FaceIndex < InputData.FaceCount; FaceIndex++)
                for (int VertIndex = 0; VertIndex < InputData.BlockVerticies.GetLength(1); VertIndex++)
                {
                    if (BlockData.Rotation.x != 0) InputData.BlockVerticies[FaceIndex, VertIndex] = RotateVert(InputData.BlockVerticies[FaceIndex, VertIndex], 0, BlockData.Rotation.x);
                    if (BlockData.Rotation.y != 0) InputData.BlockVerticies[FaceIndex, VertIndex] = RotateVert(InputData.BlockVerticies[FaceIndex, VertIndex], 1, BlockData.Rotation.y);
                    if (BlockData.Rotation.z != 0) InputData.BlockVerticies[FaceIndex, VertIndex] = RotateVert(InputData.BlockVerticies[FaceIndex, VertIndex], 2, BlockData.Rotation.z);
                }
            return InputData;
        }

        private static Vector3 RotateVert(Vector3 InputVert, int Axis, int Steps)
        {
            Steps = ((Steps % 4) + 4) % 4;
            if (Steps == 0) return InputVert; // Rotation is 0 or 360 so no calculation needed.

            // Center Vert
            Vector3 CenteredVert = new()
            {
                x = InputVert.x - 0.5f,
                y = InputVert.y - 0.5f,
                z = InputVert.z - 0.5f
            };

            return CommonMathematics.CalculateRotationforVector3(CenteredVert, Axis, Steps) + new Vector3(0.5f, 0.5f, 0.5f); //Returning noncentered calculated vert
        }

        private static BlockRenderData RotateAdjentBlockOffset(BlockRenderData InputData, GridBlock BlockData)
        {
            for (int VertIndex = 0; VertIndex < InputData.FaceCheckVectors.Length; VertIndex++)
            {
                if (BlockData.Rotation.x != 0) InputData.FaceCheckVectors[VertIndex] = CommonMathematics.CalculateRotationforVector3(InputData.FaceCheckVectors[VertIndex], 0, BlockData.Rotation.x);
                if (BlockData.Rotation.y != 0) InputData.FaceCheckVectors[VertIndex] = CommonMathematics.CalculateRotationforVector3(InputData.FaceCheckVectors[VertIndex], 1, BlockData.Rotation.y);
                if (BlockData.Rotation.z != 0) InputData.FaceCheckVectors[VertIndex] = CommonMathematics.CalculateRotationforVector3(InputData.FaceCheckVectors[VertIndex], 2, BlockData.Rotation.z);
            }
            return InputData;
        }

        public static int[] FaceCheckStepRotation(int[] InputArray, GridBlock BlockData)
        {
            if (BlockData.Rotation.x != 0)
            {
                for(int i = 0; i < BlockData.Rotation.x; i++)
                {
                    int FrontFace = InputArray[0];
                    InputArray[0] = InputArray[3];
                    InputArray[3] = InputArray[2];
                    InputArray[2] = InputArray[1];
                    InputArray[1] = FrontFace;
                }
            }
            if (BlockData.Rotation.y != 0)
            {
                for (int i = 0; i < BlockData.Rotation.y; i++)
                {
                    int FrontFace = InputArray[0];
                    InputArray[0] = InputArray[4];
                    InputArray[4] = InputArray[2];
                    InputArray[2] = InputArray[5];
                    InputArray[5] = FrontFace;
                }
            }
            if (BlockData.Rotation.z != 0)
            {
                for (int i = 0; i < BlockData.Rotation.z; i++)
                {
                    int TopFace = InputArray[1];
                    InputArray[1] = InputArray[4];
                    InputArray[4] = InputArray[3];
                    InputArray[3] = InputArray[5];
                    InputArray[5] = TopFace;
                }
            }
            return InputArray;
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

        public static Byte3 operator +(Byte3 Input1, Byte3 Input2)
        {
            return new Byte3((byte)(Input1.x + Input2.x), (byte)(Input1.y + Input2.y), (byte)(Input1.z + Input2.z));
        }
        public static Byte3 operator -(Byte3 Input1, Byte3 Input2)
        {
            return new Byte3((byte)(Input1.x - Input2.x), (byte)(Input1.y - Input2.y), (byte)(Input1.z - Input2.z));
        }
    }
}