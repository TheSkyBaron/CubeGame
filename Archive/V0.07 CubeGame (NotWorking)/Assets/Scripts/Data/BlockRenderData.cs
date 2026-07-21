using Game.Data;
using Unity.Mathematics;
using UnityEngine;

namespace Game.Renderer
{
    public static class PredefinedVertexData
    {
        public static BlockRenderData CubeRenderData = new(
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
             new(1,1,0),
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
          },
           new FaceCullingTypes[]
           {
            FaceCullingTypes.Full,
            FaceCullingTypes.Full,
            FaceCullingTypes.Full,
            FaceCullingTypes.Full,
            FaceCullingTypes.Full,
            FaceCullingTypes.Full
           }
          );
        public static BlockRenderData WedgeRenderData = new(
            6,
            new Vector3[,]
            {
                // Front
             {
                    new(0,0,0),
                    new(1,0,0),
                    new(1,1,1),
                    new(0,1,1),
                },
             // Top
             {
                    -Vector3.one,
                    -Vector3.one,
                    -Vector3.one,
                    -Vector3.one,
                },
             // Back
             {
                     new(0,0,1),
             new(0,1,1),
             new(1,1,1),
             new(1,0,1)
                },
             // Bottom
             {
             new(0,0,1),
             new(1,0,1),
             new(1,0,0),
             new(0,0,0)
                },
             // Left
             {
             new(0,0,1),
             new(0,0,0),
             new(0,1,1),
             -Vector3.one,
                },
             // Right
             {
             new(1,0,0),
             new(1,0,1),
             new(1,1,1),
             -Vector3.one,
                }
            }, new int[,]
            {
            {0,2,1,0,3,2},
            {-1,-1,-1,-1,-1,-1},
            {0,2,1,0,3,2},
            {0,2,1,0,3,2},
            {0,2,1,-1,-1,-1},
            {1,0,2,-1,-1,-1}
            }, new int3[]
            {
              new(0,0,-1),
             new(0,1,0),
             new(0,0,1),
             new(0,-1,0),
             new(-1,0,0),
             new(1,0,0)
            },
            new FaceCullingTypes[]
            {
             FaceCullingTypes.None,
             FaceCullingTypes.None,
             FaceCullingTypes.Full,
             FaceCullingTypes.Full,
             FaceCullingTypes.Partial,
             FaceCullingTypes.Partial
            }
            );
    }
}