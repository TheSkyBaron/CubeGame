using Game.Grid;
using UnityEngine;

namespace Game.Utils
{
    public class PhotoShoot : MonoBehaviour
    {
        [SerializeField] private int TargetID = 1;
        private void Start()
        {
            GridBlock TestBlock = new() { ID = TargetID,Rotation = Byte3.Zero};
            gameObject.GetComponent<MeshFilter>().sharedMesh = GameUtils.SingleBlockRenderer(TestBlock);
        }
    }
}

