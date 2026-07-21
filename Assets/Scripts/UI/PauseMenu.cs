using Game.Player;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.UI
{
    public class PauseMenu : MonoBehaviour
    {
        private GameObject Player;
        private void Start()
        {
            Player = GameObject.FindGameObjectWithTag("Player");
        }
        public void QuittoDesktop()
        {
         Application.Quit();
        }

        public void ReturntoMainMenu()
        {
            SceneManager.LoadScene("MainMenu");
        }

        public void ReturntoGame()
        {
            Player.GetComponent<PlayerWorkbenchController>().UnFreezePlayer();
        }
    }
}


