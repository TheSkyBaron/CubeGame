using UnityEngine;

namespace Game.Utils
{
    public class PhotoShoot : MonoBehaviour
    {
        [SerializeField] private int TargetID = 1;
        private void Start()
        {
            gameObject.GetComponent<MeshFilter>().sharedMesh = GameUtils.SingleBlockRenderer(TargetID);
        }
    }
}

