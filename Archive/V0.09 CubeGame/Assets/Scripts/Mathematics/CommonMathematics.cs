using Game.Utils;
using System;
using Unity.Mathematics;
using UnityEngine;

namespace Game.Mathematics
{
    public static class CommonMathematics
    {
        public static int PositionToArray(int3 Position, int3 GridSize)
        {
            int CalculatedPosition = (Position.y * GridSize.z * GridSize.x) + (Position.z * GridSize.x) + Position.x;

            return CalculatedPosition;
        }

        public static int3 ArrayToPosition(int ArrayIndex, int3 Gridsize)
        {
            int3 CalculatedPosition;

            CalculatedPosition.y = ArrayIndex / (Gridsize.z * Gridsize.x);
            ArrayIndex -= CalculatedPosition.y * Gridsize.z * Gridsize.x;
            CalculatedPosition.z = ArrayIndex / Gridsize.y;
            CalculatedPosition.x = ArrayIndex % Gridsize.y;
            return CalculatedPosition;
        }

        public static Mesh Byte3toRotatedMesh(Byte3 Input, Mesh TargetMesh)
        {
            Mesh Temp = TargetMesh;
            for (int i = 0; i < TargetMesh.vertices.Length; i++)
            {
                Temp.vertices[i] = CalculateRotationforVector3(Temp.vertices[i], 0, Input.x);
                Temp.vertices[i] = CalculateRotationforVector3(Temp.vertices[i], 1, Input.y);
                Temp.vertices[i] = CalculateRotationforVector3(Temp.vertices[i], 2, Input.z);
            }
            return Temp;
        }

        public static Vector3 CalculateRotationforVector3(Vector3 InputVector, int Axis, int Steps)
        {
            if (Steps == 0) return InputVector;
            Vector3 CalculatedVert = new()
            {
                x = InputVector.x,
                y = InputVector.y,
                z = InputVector.z
            };

            // Clockwise Rotate
            if (Axis == 0) // X axis
            {
                CalculatedVert.x = InputVector.x;
                if (Steps == 1)
                {
                    CalculatedVert.y = -InputVector.z;
                    CalculatedVert.z = InputVector.y;
                }
                if (Steps == 2)
                {

                    CalculatedVert.y = -InputVector.y;
                    CalculatedVert.z = -InputVector.z;
                }
                if (Steps == 3)
                {
                    CalculatedVert.y = InputVector.z;
                    CalculatedVert.z = -InputVector.y;
                }
            }
            if (Axis == 1) // Y axis
            {
                CalculatedVert.y = InputVector.y;
                if (Steps == 1)
                {
                    CalculatedVert.x = InputVector.z;
                    CalculatedVert.z = -InputVector.x;
                }
                if (Steps == 2)
                {
                    CalculatedVert.x = -InputVector.x;
                    CalculatedVert.z = -InputVector.z;
                }
                if (Steps == 3)
                {
                    CalculatedVert.x = -InputVector.z;
                    CalculatedVert.z = InputVector.x;
                }
            }
            if (Axis == 2) // Z Axis
            {
                CalculatedVert.z = InputVector.z;
                if (Steps == 1)
                {
                    CalculatedVert.x = InputVector.y;
                    CalculatedVert.y = -InputVector.x;

                }
                if (Steps == 2)
                {
                    CalculatedVert.x = -InputVector.x;
                    CalculatedVert.y = -InputVector.y;
                }
                if (Steps == 3)
                {
                    CalculatedVert.x = -InputVector.y;
                    CalculatedVert.y = InputVector.x;
                }
            }
            return CalculatedVert;
        }

        public static int3 CalculateRotationforVector3(int3 InputVector, int Axis, int Steps)
        {
            if (Steps == 0) return InputVector;
            int3 CalculatedVert = new()
            {
                x = InputVector.x,
                y = InputVector.y,
                z = InputVector.z
            };

            // Clockwise Rotate
            if (Axis == 0) // X axis
            {
                CalculatedVert.x = InputVector.x;
                if (Steps == 1)
                {
                    CalculatedVert.y = -InputVector.z;
                    CalculatedVert.z = InputVector.y;
                }
                if (Steps == 2)
                {

                    CalculatedVert.y = -InputVector.y;
                    CalculatedVert.z = -InputVector.z;
                }
                if (Steps == 3)
                {
                    CalculatedVert.y = InputVector.z;
                    CalculatedVert.z = -InputVector.y;
                }
            }
            if (Axis == 1) // Y axis
            {
                CalculatedVert.y = InputVector.y;
                if (Steps == 1)
                {
                    CalculatedVert.x = InputVector.z;
                    CalculatedVert.z = -InputVector.x;
                }
                if (Steps == 2)
                {
                    CalculatedVert.x = -InputVector.x;
                    CalculatedVert.z = -InputVector.z;
                }
                if (Steps == 3)
                {
                    CalculatedVert.x = -InputVector.z;
                    CalculatedVert.z = InputVector.x;
                }
            }
            if (Axis == 2) // Z Axis
            {
                CalculatedVert.z = InputVector.z;
                if (Steps == 1)
                {
                    CalculatedVert.x = InputVector.y;
                    CalculatedVert.y = -InputVector.x;

                }
                if (Steps == 2)
                {
                    CalculatedVert.x = -InputVector.x;
                    CalculatedVert.y = -InputVector.y;
                }
                if (Steps == 3)
                {
                    CalculatedVert.x = -InputVector.y;
                    CalculatedVert.y = InputVector.x;
                }
            }
            return CalculatedVert;
        }

        public static byte CalculateBitmask(int Axis, int Step, int Mask)
        {
            byte Result = 0b00000000;
            float Epsilon = 0.01f;

            for (int index = 0; index < 8; index++) // Index == bits in byte (8)
            {
                if ((Mask & (1 << index)) != 0)
                {
                    // Decoding Bit index
                    int X = index & 1;
                    int Y = (index & 2) >> 1;
                    int Z = (index & 4) >> 2;

                    // Centering Coordinates
                    float CenteredX = X - 0.5f;
                    float CenteredY = Y - 0.5f;
                    float CenteredZ = Z - 0.5f;

                    for (int StepIndex = 0; StepIndex < Step; StepIndex++)
                    {
                        float Temp;
                        if (Axis == 0) // X Axis
                        {
                            Temp = CenteredY;
                            CenteredY = -CenteredZ;
                            CenteredZ = Temp;
                        }
                        if (Axis == 1) // Y Axis
                        {
                            Temp = CenteredX;
                            CenteredX = CenteredZ;
                            CenteredZ = -Temp;
                        }
                        if (Axis == 2) // Z Axis
                        {
                            Temp = CenteredX;
                            CenteredX = CenteredY;
                            CenteredY = -Temp;
                        }
                    }

                    // Returning values to int
                    X = (int)MathF.Round(CenteredX + 0.5f + (CenteredX > 0 ? Epsilon : -Epsilon));
                    Y = (int)MathF.Round(CenteredY + 0.5f + (CenteredY > 0 ? Epsilon : -Epsilon));
                    Z = (int)MathF.Round(CenteredZ + 0.5f + (CenteredZ > 0 ? Epsilon : -Epsilon));

                    int CalculatedByte = X + (Y * 2) + (Z * 4);
                    Result |= (byte)(1 << CalculatedByte);
                }
            }
            return Result;
        }
    }
}