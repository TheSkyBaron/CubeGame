using Game.Player;
using UnityEngine;

namespace Game.UI
{
    public class InventorySlot : MonoBehaviour
    {
        private int BlockID;
        private GameObject PlayerObject;

        private void Start()
        {
            PlayerObject = GameObject.FindGameObjectWithTag("Player");
        }

        public void ChangeBlock(int NewID)
        {
         BlockID = NewID;
        }

        public void ChangePlayerBlock()
        {
            PlayerObject.GetComponent<PlayerWorkbenchController>().PlayerBlockChange(BlockID);
        }
    }
}


