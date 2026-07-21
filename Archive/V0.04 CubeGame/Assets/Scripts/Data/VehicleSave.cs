using Game.Grid;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

namespace Game.Data
{
    public class VehicleSave : MonoBehaviour
    {
        [SerializeField] private GameObject SavePanel;
        [SerializeField] private TMP_Text VehicleName;
        [SerializeField] private GameObject OverwriteConfirmUI;

        private Workbench ActiveWorkbench;
        private bool isOverwriteConfirmed = false;
        private GameObject PlayerObject;

        private readonly string SaveFiles = "Vehicles";
        private readonly string SaveDataType = ".Vehicle";

        private void Start()
        {
            SavePanel.SetActive(false);
            OverwriteConfirmUI.SetActive(false);
            ActiveWorkbench = GameObject.FindGameObjectWithTag("Workbench").GetComponent<Workbench>();
            PlayerObject = GameObject.FindGameObjectWithTag("Player");
        }

        public void SaveVehicle()
        {
            string FolderPath = Application.persistentDataPath + "/" + SaveFiles;
            string FilePath = FolderPath + "/" + VehicleName.text + SaveDataType;
            if (!Directory.Exists(FolderPath)) Directory.CreateDirectory(FolderPath);
            if (File.Exists(FilePath) && !isOverwriteConfirmed)
            {
                OverwriteConfirmUI.SetActive(true);
                return;
            }

            using FileStream FStream = new(FilePath, FileMode.Create);
            using StreamWriter Writer = new(FStream, System.Text.Encoding.UTF8);
            Writer.Write(JsonUtility.ToJson(SaveOptimizer(ActiveWorkbench.GridSystem.BuildableGrid), true));
            SavePanel.SetActive(false);
        }

        public void OverwriteConfirm()
        {
            isOverwriteConfirmed = true;
            OverwriteConfirmUI.SetActive(false);
            SaveVehicle();
        }

        private VehicleSaveFile SaveOptimizer(GridBlock[] InputArray)
        {
            List<int> ArrayLocation = new();
            List<GridBlock> OptimizedSave = new();
            for (int i = 0; i < InputArray.Length; i++)
            {
                if (InputArray[i].ID == 0) continue;
                ArrayLocation.Add(i);
                OptimizedSave.Add(InputArray[i]);
            }
            VehicleSaveFile SaveFile = new()
            {
                BlockArrayMap = ArrayLocation.ToArray(),
                VehicleBlocks = OptimizedSave.ToArray()
            };
            return SaveFile;
        }
    }

    public class VehicleSaveFile
    {
        public int[] BlockArrayMap;
        public GridBlock[] VehicleBlocks;
    }
}
