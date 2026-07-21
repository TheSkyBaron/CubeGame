using Game.Engine;
using System.IO;
using UnityEngine;

namespace Game.Data
{
    public class VehicleLoad : MonoBehaviour
    {
        [SerializeField] GameObject PrefabObject;
        private string[] Folders;

        private void Start()
        {
           transform.parent.gameObject.SetActive(false);
        }
        public void LoadVehicle()
        {
            Folders = Directory.GetDirectories(Settings.SaveFoldersPath);
            for(int i = 0; i < transform.childCount;i++) Destroy(transform.GetChild(i).gameObject);
            foreach (string Dir in Folders)
            {
                GameObject SaveSlot = Instantiate(PrefabObject,Vector3.zero,Quaternion.identity,transform);
                SaveSlot.GetComponent<LoadObject>().SetVehicle(Path.GetFileName(Dir),Dir);
            }
        }
    }
}