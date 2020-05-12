using System.IO;

using UnityEngine;
using UnityEditor;

using Assets.Scripts.NWorld;
using Assets.Scripts.NData;

namespace Assets.Scripts.NGameSystem
{
    [RequireComponent(typeof(WorldMng))]
    class SaveMng : MonoBehaviour
    {
        public GameSave FileTest;
        public string FileTest_Path;

        [SerializeField]
        private string m_SavePath;

        private SaveHelper_World m_SaveHelper;


        public string SavePath { get { return Application.dataPath + m_SavePath; } }
        //Save File
        public GameSave LoadedFile { private set; get; }
        public string LoadedFilePath { private set; get; }

        public void Update()
        {
            //if (LoadedFile != null)
            //{
            //    LoadedFile.LastPlayed.Assign(System.DateTime.Now);
            //}
        }

        public void InitSaveSystem()
        {
            if (LoadedFile == null)
            {
                CreateSaveFile("TempFile", "TempWorld");
            }

            m_SaveHelper = new SaveHelper_World(LoadedFile.WorldModfication);
        }

        public string[] GetAllSavePaths()
        {
            if (!Directory.Exists(SavePath)) return null;

            DirectoryInfo directoryInfo  = new DirectoryInfo(SavePath);

            FileInfo[] files = directoryInfo.GetFiles("*.txt");

            if (files.Length == 0) return null;

            string [] res = new string[files.Length];

            int i = 0;
            foreach (var file in files)
            {
                res[i++] = file.FullName;
            }
            return res;
        }

        public void CreateSaveFile(string WorldName, string WorldSeed)
        {
            //Create Directory
            if (!Directory.Exists(SavePath))
            {
                Directory.CreateDirectory(SavePath);
            }


            //Create instance of Save Data
            LoadedFile = new GameSave();
            LoadedFile.Init(WorldName, WorldSeed);

            string tempPath = SavePath +"/"+ WorldName + ".txt";

            //Avoid Duplicate file
            int i = 1;
            while (File.Exists(tempPath))
            {
                tempPath = SavePath + "/" + WorldName + '(' + i + ')' + ".txt";
                ++i;
            }

            LoadedFilePath = tempPath;

            string strJson = JsonUtility.ToJson(LoadedFile);
            File.WriteAllText(tempPath, strJson);
        }

        public void LoadSaveFile(GameSave save, string path)
        {
            LoadedFile = save;
            LoadedFilePath = path;
        }

        public void SaveBlock(BlockLocation Location, byte blockId)
        {
            m_SaveHelper.Modify(Location, blockId,GetComponent<WorldMng>().WorldService);
        }
        public void LoadBlock(ChunkInWorld chunkInWorld, Chunk chunk)
        {
            m_SaveHelper.ApplyModification(chunkInWorld, chunk, GetComponent<WorldMng>().WorldService);
        }

        public void SavePlayerLocation(Transform PlayerTransform)
        {
            LoadedFile.PlayerPos = PlayerTransform.localPosition;
            LoadedFile.playerRot = PlayerTransform.localRotation;
        }

        public void SaveCurProgress()
        {
            if (LoadedFile != null)
            {
                Debug.Log(LoadedFilePath);
                LoadedFile.LastPlayed.Assign(System.DateTime.Now);
                File.WriteAllText(LoadedFilePath,JsonUtility.ToJson(LoadedFile));
            }
        }

        public void OnApplicationQuit()
        {
            SaveCurProgress();
            Debug.Log("quited");
        }
    }
}



