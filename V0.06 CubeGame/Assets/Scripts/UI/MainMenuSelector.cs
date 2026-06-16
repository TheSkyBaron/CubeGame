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
           DecorObject.GetComponent<MeshFilter>().sharedMesh = GameUtils.SingleBlockRenderer(2);
        }

        private void FixedUpdate()
        {
            CurrentRotation = (CurrentRotation + SpinSpeed) % 360;
            DecorObject.transform.eulerAngles = new Vector3(0, CurrentRotation, 0);
        }

        public void Designer()
        {
            SceneManager.LoadScene("Workbench Editor");
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

