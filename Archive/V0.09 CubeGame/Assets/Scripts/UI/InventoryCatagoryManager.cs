using UnityEngine;

namespace Game.UI
{
    public class InventoryCatagoryManager : MonoBehaviour
    {
        [SerializeField] private GameObject InventoryContent;
        [SerializeField] private GameObject Prefab;

        public void ChangeSelectedBlockType(int TypeID)
        {
            ChangeInventoryContent(TypeID);
        }

        private void ChangeInventoryContent(int TypeID)
        {
            for (int i = 0; i < InventoryContent.transform.childCount; i++)
            {
                Destroy(InventoryContent.transform.GetChild(i).gameObject);
            }
            foreach (int Block in InventoryPlanner.CatagorizedBlocks[TypeID])
            {
                GameObject Temp = Instantiate(Prefab,InventoryContent.transform);
                Temp.GetComponent<InventorySlot>().ChangeBlock(Block);
            }
        }
    }
}

