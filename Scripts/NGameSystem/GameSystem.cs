using System.IO;

using UnityEngine;
using UnityEngine.SceneManagement;

using Assets.Scripts.NTouch;
using Assets.Scripts.NGlobal.Singleton;

namespace Assets.Scripts.NGameSystem
{
    [RequireComponent(typeof(EventMng))]
    [RequireComponent(typeof(InputMng))]
    [RequireComponent(typeof(WorldMng))]
    [RequireComponent(typeof(PlayerMng))]
    class GameSystem : MonoSingleton<GameSystem>
    {
        [SerializeField]
        private string m_strMenuScene = "GameMenu";
        [SerializeField]
        private string m_strPlayScene = "GamePlay";
        [SerializeField]
        private string m_strSettingFile = "/Setting.txt";
        [SerializeField]
        private GameSetting m_GameSetting = null;

        //public GameSetting GameSettingIns { private set; get; }
        public EventMng EventMngIns { private set; get; }
        public InputMng InputMngIns { private set; get; }
        public WorldMng WorldMngIns { private set; get; }
        public PlayerMng PlayerMngIns { private set; get; }
        public SaveMng SaveMngIns { private set; get; }
        public GameSetting GameSettingIns { get { return m_GameSetting; } }

        public string SettingPath { get { return Application.dataPath + m_strSettingFile; } }

        public Picker GetPicker()
        {
            return InputMngIns.PickerIns;
        }

        //Unity Function
        //--------------------------------------------------

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
     
            EventMngIns = GetComponent<EventMng>();
            InputMngIns = GetComponent<InputMng>();
            WorldMngIns = GetComponent<WorldMng>();
            PlayerMngIns = GetComponent<PlayerMng>();
            SaveMngIns = GetComponent<SaveMng>();

            LoadSettingFile();
        }

        //Game Setting Relatived Function
        //--------------------------------------------------

        private void LoadSettingFile()
        {
            //Case: There is a setting file
            if (File.Exists(SettingPath))
            {
                string StrJson = File.ReadAllText(SettingPath);
                m_GameSetting = JsonUtility.FromJson<GameSetting>(StrJson);
            }
            else
            {
                m_GameSetting = new GameSetting();

                string StrJson = JsonUtility.ToJson(m_GameSetting);

                File.WriteAllText(SettingPath, StrJson);
            }
        }

        public GameSetting ResetSettingToDefault()
        {
            m_GameSetting = new GameSetting();

            string StrJson = JsonUtility.ToJson(m_GameSetting);

            File.WriteAllText(SettingPath, StrJson);

            return m_GameSetting;
        }

        public void SaveSettings()
        {
            if (m_GameSetting != null)
            {
                string StrJson = JsonUtility.ToJson(m_GameSetting);
                File.WriteAllText(SettingPath, StrJson);
            }
        }


        public void ApplySettings(bool SaveToFile = true)
        {
            EventMngIns.ApplySettings(m_GameSetting);
            InputMngIns.ApplySettings(m_GameSetting);
            WorldMngIns.ApplySettings(m_GameSetting);
            PlayerMngIns.ApplySettings(m_GameSetting);
            SaveMngIns.ApplySettings(m_GameSetting);

            //save settings to a file
            if (SaveToFile)
            {
                string StrJson = JsonUtility.ToJson(m_GameSetting);
                File.WriteAllText(SettingPath, StrJson);
            }
        }

        //Scene Relatived Function
        //--------------------------------------------------

        public void RunMenuScene()
        {
            SceneManager.LoadScene(m_strMenuScene);
        }

        public void RunPlayScene()
        {         
            SceneManager.LoadScene(m_strPlayScene);
        }

        public void InitMenuScene()
        {
            EventMngIns.enabled = false;
            InputMngIns.enabled = false;
            WorldMngIns.enabled = false;
            PlayerMngIns.enabled = false;
            SaveMngIns.enabled = false;
        }

        public void InitPlayScene()
        {
            EventMngIns.enabled = true;
            InputMngIns.enabled = true;
            WorldMngIns.enabled = true;
            PlayerMngIns.enabled = true;
            SaveMngIns.enabled = true;


            ApplySettings(false);
            

            SaveMngIns.InitSaveSystem();
            EventMngIns.InitEventService();

            WorldMngIns.InitWorldService();
            WorldMngIns.SpawnWorld();

            PlayerMngIns.SpawnPlayer();

            InputMngIns.InitInputService();
            InputMngIns.InitController();
            InputMngIns.InitUISet();
        }
    }
}
