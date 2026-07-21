using Game.Data;
using Game.Engine;
using Game.Grid;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Utils
{
    public class PhotoShoot : MonoBehaviour
    {
        GridBlock TestBlock = new()
        {
         ID = 1,
         Rotation = Byte3.Zero,
        };
        private int TargetID = 1;
        [SerializeField] private TMP_Text IDText;
        private void Start()
        {
            ReRenderObject();
        }

        public void ChangeBlock()
        {

            int.TryParse(IDText.text[..^1],out TargetID);
            if (TargetID == 0 || TargetID < 0 || TargetID > CompiledBlockAtlas.PredefinedBlockCount-1) TargetID = 1;
            ReRenderObject();
        }

        private void ReRenderObject()
        {
            TestBlock.ID = TargetID;
            gameObject.GetComponent<MeshFilter>().sharedMesh = GameUtils.SingleBlockRenderer(TestBlock);
        }

        public void RotateClockwise90()
        {
            float Rotation = (transform.eulerAngles.y + 90) % 360;
            transform.eulerAngles = new Vector3(0, Rotation, 0);
        }
        public void RotateCounterClockwise90()
        {
            float Rotation = (transform.eulerAngles.y - 90) % 360;
            transform.eulerAngles = new Vector3(0, Rotation, 0);
        }

        public void TakePhoto()
        {
            StartCoroutine(ScreenShot());
        }

        private IEnumerator ScreenShot()
        {
            Debug.Log("Saving Texture...");
            yield return new WaitForEndOfFrame();
            var Texture = new Texture2D(Settings.PhotoshootImageWidth, Settings.PhotoshootImageHeight,TextureFormat.RGBA64, false);
            Rect TextureData = new(Settings.PhotoshootStartPixelPosition.x, Settings.PhotoshootStartPixelPosition.y, Settings.PhotoshootImageWidth, Settings.PhotoshootImageHeight);

            Texture.ReadPixels(TextureData, 0, 0, false);
            Texture.Apply();

            var TextureBytes = Texture.EncodeToPNG();
            Destroy(Texture);
            System.Random Randomizer = new();
            System.IO.File.WriteAllBytes(Application.dataPath + $"/Textures/BlockImage{Randomizer.Next(1000,100000)}.png", TextureBytes);
            Debug.Log("Photo saved at: " + Application.dataPath + "/Textures/BlockImage.png");
        }

        public void BacktotheMenu()
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
}

