using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.UI
{
    public class PauseMenu : MonoBehaviour
    {
        public void QuittoDesktop()
        {
         Application.Quit();
        }

        public void ReturntoMainMenu()
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
}


