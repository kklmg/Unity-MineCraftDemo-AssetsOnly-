using UnityEngine;

using Assets.Scripts.NData;
using Assets.Scripts.NGlobal.Singleton;
using Assets.Scripts.NGlobal.ServiceLocator;
using Assets.Scripts.NGlobal.WorldSearcher;
using Assets.Scripts.NWorld;
using Assets.Scripts.NEvent;

namespace Assets.Scripts.NGameSystem
{
    class PlayerMng : MonoBehaviour
    {
        public GameObject m_PrefabPlayer;

        [SerializeField]
        [Range(0, 5)]
        private uint m_PlayerView = 2;

        private GameObject m_playerIns; //player instance
        public uint PlayerView { get { return m_PlayerView; } }

        public void SpawnPlayer()
        {
            GameSave SaveFile = MonoSingleton<GameSystem>.Instance.SaveMngIns.LoadedFile;
            IWorld world = Locator<IWorld>.GetService();

            if (SaveFile.HasPlayerSpawned == false)
            {
                SaveFile.PlayerPos.y =
                GWorldSearcher.GetGroundHeight(SaveFile.PlayerPos, world) + 3;
                SaveFile.HasPlayerSpawned = true;
            }

            //make a player Instance
            m_playerIns = Instantiate(m_PrefabPlayer,
                MonoSingleton<WorldMng>.Instance.WorldIns.transform);

            //set player's postion
            m_playerIns.transform.localPosition = SaveFile.PlayerPos;
            m_playerIns.transform.localRotation = SaveFile.playerRot;

            //publish event
            Locator<IEventPublisher>.GetService().
               Publish(new E_Player_Spawned(SaveFile.PlayerPos, m_playerIns.GetComponent<Character>()));
        }
    }
}
