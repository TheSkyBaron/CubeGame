using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Data
{
    public class CompilerManager : MonoBehaviour
    {
        [SerializeField] private GameObject LoadingBarMask;
        void Start()
        {
            AsyncLoader();
            StartCoroutine(Loadingbar());
        }

        private async void AsyncLoader()
        {
            float Timer = Time.realtimeSinceStartup;
            string Result = await CompiledBlockAtlas.AsyncCompile();
            await SceneManager.LoadSceneAsync("MainMenu");
            Timer = Time.realtimeSinceStartup - Timer;
            Debug.Log($"Loader completed in {Timer} Seconds! Status Report:\n" + Result);
        }
        private IEnumerator Loadingbar()
        {
            int LastPercentage = 0;
            int LoopBreaker = 0;
            while (CompiledBlockAtlas.LoadingBarPercentage < 100 && LoopBreaker < 100)
            {
                if (LastPercentage == CompiledBlockAtlas.LoadingBarPercentage) yield return null;
                else
                {
                    for (float i = LastPercentage; i < CompiledBlockAtlas.LoadingBarPercentage;i += 0.1f)
                    {
                        float CalculatedPercentage = i / 100f;
                        LoadingBarMask.GetComponent<RectTransform>().localScale = new Vector3(CalculatedPercentage, 1, 1);
                        yield return null;
                    }
                    LastPercentage = CompiledBlockAtlas.LoadingBarPercentage;
                    LoopBreaker++;
                }
            }
            if (LoopBreaker == 100) Debug.LogError("Loop breaker at compiler manager is tripped!");
            LoadingBarMask.GetComponent<RectTransform>().localScale = Vector3.one;
        }
    }
}

