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
        public int IndexMenuScene;
        public int IndexPlayScene;

        public EventMng EventMngIns { private set; get; }
        public InputMng InputMngIns { private set; get; }
        public WorldMng WorldMngIns { private set; get; }
        public PlayerMng PlayerMngIns { private set; get; }
        public SaveMng SaveMngIns { private set; get; }

        public Picker GetPicker()
        {
            return InputMngIns.BlockPicker;
        }

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);

            EventMngIns = GetComponent<EventMng>();
            InputMngIns = GetComponent<InputMng>();
            WorldMngIns = GetComponent<WorldMng>();
            PlayerMngIns = GetComponent<PlayerMng>();
            SaveMngIns = GetComponent<SaveMng>();
        }

        public void RunMenuScene()
        {
            SceneManager.LoadScene("GameMenu");
            //SceneManager.LoadScene(IndexMenuScene);
        }

        public void RunPlayScene()
        {         
            SceneManager.LoadScene("GamePlay");
            //SceneManager.LoadScene(IndexPlayScene);
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

            SaveMngIns.InitSaveSystem();
            EventMngIns.InitEventService();

            WorldMngIns.InitWorldService();
            WorldMngIns.SpawnWorld();

            PlayerMngIns.SpawnPlayer();

            InputMngIns.InitInputService();
            InputMngIns.InitController();
            InputMngIns.InitInteraction();
        }
    }
}
