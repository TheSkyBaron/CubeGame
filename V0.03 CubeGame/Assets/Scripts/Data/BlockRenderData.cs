using Game.Data;
using Unity.Mathematics;
using UnityEngine;

namespace Game.Renderer
{
    public static class PredefinedVertexData
    {
        public static readonly BlockRenderData CubeRenderData = new(
          6,
          new Vector3[,]
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
          },
             new int[,]
          {
           {0,2,1,0,3,2}, // Front
           {0,2,1,0,3,2}, // Top
           {0,2,1,0,3,2}, // Back
           {0,2,1,0,3,2}, // Bottom
           {0,2,1,0,3,2}, // Left
           {0,2,1,0,3,2} // Right
          },
           new int3[]
          {
            new(0,0,-1),
            new(0,1,0),
            new(0,0,1),
            new(0,-1,0),
            new(-1,0,0),
            new(1,0,0)
          }
          );
    }
}
