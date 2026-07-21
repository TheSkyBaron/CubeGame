using Game.Data;
using System.Collections.Generic;

namespace Game.UI
{
    public static class InventoryPlanner
    {
        public static List<int>[] CatagorizedBlocks = new List<int>[]
        {
         new (),
         new (),
         new (),
         new (),
         new (),
         new (),
         new ()
        };
        public static void CalculateInventoryGroup()
        {
            for (int index = 1; index < CompiledBlockAtlas.BlockAtlas.Length; index++)
            {
                if (CompiledBlockAtlas.BlockAtlas[index] == null) continue;
                switch (CompiledBlockAtlas.BlockAtlas[index].BlockType)
                {
                    case Blocktypes.Block:
                        CatagorizedBlocks[0].Add(index);
                        break;
                    case Blocktypes.Land:
                        CatagorizedBlocks[1].Add(index);
                        break;
                    case Blocktypes.Air:
                        CatagorizedBlocks[2].Add(index);
                        break;
                    case Blocktypes.Sea:
                        CatagorizedBlocks[3].Add(index);
                        break;
                    case Blocktypes.Weapons:
                        CatagorizedBlocks[4].Add(index);
                        break;
                    case Blocktypes.Decorations:
                        CatagorizedBlocks[5].Add(index);
                        break;
                    case Blocktypes.Computers:
                        CatagorizedBlocks[6].Add(index);
                        break;
                }
            }
        }
    }
}
