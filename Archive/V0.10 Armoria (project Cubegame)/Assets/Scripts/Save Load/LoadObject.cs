using Game.Engine;
using Game.Grid;
using Game.Player;
using Game.Utils;
using System.Data.Common;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Data
{
    public class LoadObject : MonoBehaviour
    {
        [SerializeField] private TMP_Text ObjectText;
        [SerializeField] private Image ObjectImage;

        private string VehicleLocation;
        private string VehicleFileName;
        private GameObject Workbench;
        private GameObject Player;

        private void Start()
        {
            Workbench = GameObject.FindGameObjectWithTag("Workbench");
            Player = GameObject.FindGameObjectWithTag("Player");
        }

        public void LoadVehicle()
        {
            VehicleSaveFile SaveFile;
            using (FileStream Stream = new(VehicleLocation + "/" + VehicleFileName + Settings.SaveFileType, FileMode.Open))
            using (StreamReader Reader = new(Stream))
                SaveFile = (VehicleSaveFile)JsonUtility.FromJson(Reader.ReadToEnd(), typeof(VehicleSaveFile));
            Workbench.GetComponent<Workbench>().GridSystem.LoadGrid(SaveFileRecover(SaveFile));
            transform.parent.parent.gameObject.SetActive(false);
            Workbench.GetComponent<Workbench>().RenderWorkbench();
            Player.GetComponent<PlayerWorkbenchController>().UnFreezePlayer();
        }

        public void SetVehicle(string VehicleName, string VehicleLoadLocation)
        {
            VehicleLocation = VehicleLoadLocation;
            ObjectText.text = VehicleName;
            VehicleFileName = VehicleName;
            if (File.Exists(VehicleLoadLocation + "/" + Settings.CoverImage)) ObjectImage.sprite = GameUtils.LoadImage(VehicleLoadLocation + "/" + Settings.CoverImage);
            else ObjectImage.enabled = false;
        }

        private GridBlock[] SaveFileRecover(VehicleSaveFile file)
        {
            GridBlock[] VehicleGrid = new GridBlock[Workbench.GetComponent<Workbench>().GridSystem.BuildableGrid.Length];

            for (int i = 0; i < file.BlockArrayMap.Length; i++)
            {
                VehicleGrid[file.BlockArrayMap[i]] = file.VehicleBlocks[i];
            }

            return VehicleGrid;
        }
    }
}