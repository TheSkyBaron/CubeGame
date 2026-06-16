using UnityEngine;

namespace Game.Renderer
{
    public static class CubeVertexData
    {
        public readonly static int FaceCount = 6;
        // Vert Table
        public readonly static Vector3[,] CubeVerts = new Vector3[,]
        {
          // Front Face (0-3)
          {
          new(0,0,0),
          new(1,0,0),
          new(1,1,0),
          new(0,1,0),
          },
          // Top Face (4-7)
          {
          new(0,1,0),
          new(1,1,0),
          new(1,1,1),
          new(0,1,1),
          },
          // Back Face (8-11)
          {
           new(0,0,1),
           new(0,1,1),
           new(1,1,1),
           new(1,0,1)
            },
          // Bottom Face (12-15)
          {
           new(0,0,1),
           new(1,0,1),
           new(1,0,0),
           new(0,0,0)
            },
          // Left Face (16-19)
          {
           new(0,0,1),
           new(0,0,0),
           new(0,1,0),
           new(0,1,1)
            },
          // Right Face (20-23)
          {
           new(1,0,0),
           new(1,0,1),
           new(1,1,1),
           new(1,1,0)
            }
        };
        // Triangle Table
        public readonly static int[,] CubeTriangles = new int[,]
        {
         {0,2,1,0,3,2}, // Front
         {4,6,5,4,7,6}, // Top
         {8,10,9,8,11,10}, // Back
         {12,14,13,12,15,14}, // Bottom
         {16,18,17,16,19,18}, // Left
         {20,22,21,20,23,22} // Right
        };
    }
}
