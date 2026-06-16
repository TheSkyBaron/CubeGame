using Game.Grid;
using Game.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.UI
{
    public class MainMenuSelector : MonoBehaviour
    {
        [SerializeField] private GameObject DecorObject;
        [SerializeField] private float SpinSpeed = 0.1f;
        private float CurrentRotation;
        private void Start()
        {
            GridBlock Block = new() { ID = 2,Rotation = Byte3.Zero};
           DecorObject.GetComponent<MeshFilter>().sharedMesh = GameUtils.SingleBlockRenderer(Block);
        }

        private void FixedUpdate()
        {
            CurrentRotation = (CurrentRotation + SpinSpeed) % 360;
            DecorObject.transform.eulerAngles = new Vector3(0, CurrentRotation, 0);
        }

        public void Designer()
        {
            SceneManager.LoadScene("WorkbenchEditor");
        }
        public void PhotoShoot()
        {
            SceneManager.LoadScene("PhotoShoot");
        }

        public void Settings()
        {
            Debug.Log("TBD");
        }

        public void Credits()
        {
            Debug.Log("Made by Sky");
        }

        public void Exit()
        {
            Application.Quit();
        }
    }
}

