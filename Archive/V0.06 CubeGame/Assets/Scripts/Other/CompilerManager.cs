using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Data
{
    public class CompilerManager : MonoBehaviour
    {
        void Start()
        {
            CompiledBlockAtlas.Compile();
            SceneManager.LoadSceneAsync("MainMenu");
        }
    }
}

