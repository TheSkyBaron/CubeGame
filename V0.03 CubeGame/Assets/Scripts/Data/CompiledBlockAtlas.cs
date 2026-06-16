using Game.Renderer;

namespace Game.Data
{
    public static class CompiledBlockAtlas
    {
        public static Block[] BlockAtlas {  get; private set; }
        private static int PredefinedBlockCount = 2;
        public static void Compile()
        {
            BlockAtlas = new Block[PredefinedBlockCount + 256];
            PredefinedBlocks();
        }

        private static void PredefinedBlocks()
        {
            BlockAtlas[0] = new Block(0, new BlockRenderData(0,new UnityEngine.Vector3[0,0],new int[0,0],new Unity.Mathematics.int3[0]));
            BlockAtlas[1] = new Block(1,PredefinedVertexData.CubeRenderData);
        }
    }
}
