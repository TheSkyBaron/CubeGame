using Game.Engine;
using Game.Grid;
using Game.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.UI
{
    public class MainMenuSelector : MonoBehaviour
    {
        [SerializeField] private GameObject CreditsTextGameObject;
        [SerializeField] private GameObject DecorObject;

        private float CurrentRotation;
        private float StartCreditObjectPos;
        private bool isCreditsActive = false;
        private void Start()
        {
            GridBlock Block = new() { ID = Settings.DecorBlockID,Rotation = Byte3.Zero};
           DecorObject.GetComponent<MeshFilter>().sharedMesh = GameUtils.SingleBlockRenderer(Block);
            CreditsTextGameObject.transform.parent.gameObject.SetActive(false);
        }

        private void FixedUpdate()
        {
            if(DecorObject.activeSelf)
            {
                CurrentRotation = (CurrentRotation + Settings.MenuDecorObjectSpinSpeed) % 360;
                DecorObject.transform.eulerAngles = new Vector3(0, CurrentRotation, 0);
            }
            if (isCreditsActive)
            {
                CreditsTextGameObject.transform.position = CreditsTextGameObject.transform.position + Vector3.up;
                if (CreditsTextGameObject.transform.position.y > CreditsTextGameObject.GetComponent<RectTransform>().rect.height + Settings.CreditsTextScrollOffset) CloseCredits();
            }
        }

        public void Designer()
        {
            SceneManager.LoadScene("WorkbenchEditor");
        }
        public void PhotoShoot()
        {
            SceneManager.LoadScene("PhotoShoot");
        }

        public void SettingsMenu()
        {
            Debug.Log("TBD");
        }

        public void Credits()
        {
            Debug.Log("Made by Sky");
            DecorObject.SetActive(false);
            StartCreditObjectPos = CreditsTextGameObject.transform.position.y;
            isCreditsActive = true;
        }

        public void CloseCredits()
        {
            DecorObject.SetActive(true);
            isCreditsActive=false;
            CreditsTextGameObject.transform.position = new Vector3(CreditsTextGameObject.transform.position.x, StartCreditObjectPos, CreditsTextGameObject.transform.position.z);
        }

        public void Exit()
        {
            Application.Quit();
        }
    }
}

