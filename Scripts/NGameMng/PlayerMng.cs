using System;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.NGlobal.Singleton;
using Assets.Scripts.NGlobal.ServiceLocator;
using Assets.Scripts.NEvent;

namespace Assets.Scripts.NGameMng
{
    class PlayerMng : MonoSingleton<PlayerMng>
    {
        public Vector3 m_SpawnPosition;

        public GameObject m_PrefabPlayer;
        private GameObject m_playerIns; //player instance


        private void Start()
        {
            SpawnPlayer();

            Locator<IEventPublisher>.GetService().
                Publish(new E_Player_Spawned(m_SpawnPosition, m_playerIns.GetComponent<Character>()));
        }

        void SpawnPlayer()
        {
            //make a player Instance
            m_playerIns = Instantiate(m_PrefabPlayer,
                MonoSingleton<WorldMng>.Instance.WorldIns.transform);

            //set player's postion
            m_playerIns.transform.localPosition = m_SpawnPosition;
        }
    }
}
