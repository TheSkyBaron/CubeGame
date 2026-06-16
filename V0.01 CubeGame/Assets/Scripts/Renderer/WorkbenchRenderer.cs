using Game.Engine;
using Game.Mathematics;
using Unity.Mathematics;
using UnityEngine;

namespace Game.Renderer
{
    public class WorkbenchRenderer : IRenderer
    {
        public Mesh Calculate(int[] BuildableGrid,int3 Gridsize)
        {
            Vector3[] BlockVerticies = new Vector3[Settings.MaxVerticies];
            int[] BlockTriangles = new int[Settings.MaxTriangles];
            int VertsCount = 0;
            int TrianglesCount = 0;
            for (int i = 0; i < BuildableGrid.Length; i++)
            {
                int VertexStartIndex = VertsCount;
                if (BuildableGrid[i] <= 0) continue; // Don't render non valid blocks.
                int3 CalculatedBlockPosition = CommonMathematics.ArrayToPosition(i, Gridsize);
                // Loops Throught each face
                for (int FaceIndex = 0; FaceIndex < CubeVertexData.FaceCount; FaceIndex++)
                {
                    for (int VertexIndex = 0; VertexIndex < CubeVertexData.CubeVerts.GetLength(1); VertexIndex++)
                    {
                        
                        BlockVerticies[VertsCount + VertexIndex] = CubeVertexData.CubeVerts[FaceIndex, VertexIndex] + new Vector3(CalculatedBlockPosition.x,CalculatedBlockPosition.y,CalculatedBlockPosition.z);
                        
                    }

                    for (int TriangleIndex = 0; TriangleIndex < CubeVertexData.CubeTriangles.GetLength(1); TriangleIndex++)
                    {
                        BlockTriangles[TrianglesCount + TriangleIndex] = CubeVertexData.CubeTriangles[FaceIndex, TriangleIndex] + VertexStartIndex;
                        
                    }
                    VertsCount+= CubeVertexData.CubeVerts.GetLength(1);
                    TrianglesCount+= CubeVertexData.CubeTriangles.GetLength(1);
                }

            }
            Vector3[] OptimizedBlockVerticies = new Vector3[VertsCount];
            int[] OptimizedBlockTriangles = new int[TrianglesCount];
            for (int i = 0; i < OptimizedBlockVerticies.Length; i++) OptimizedBlockVerticies[i] = BlockVerticies[i];
            for (int i = 0; i < OptimizedBlockTriangles.Length; i++) OptimizedBlockTriangles[i] = BlockTriangles[i];
            return MeshGenerator(OptimizedBlockVerticies, OptimizedBlockTriangles);
        }

        private bool FaceChecker()
        {
            return false;
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
        public Mesh Calculate(int[] BuildableGrid,int3 Gridsize);
    }
}