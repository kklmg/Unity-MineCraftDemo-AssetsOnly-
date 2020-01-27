using UnityEngine;
using Assets.Scripts.NWorld;
using Assets.Scripts.NGlobal.ServiceLocator;
using Assets.Scripts.NEvent;

namespace Assets.Scripts.NCharacter
{
    [RequireComponent(typeof(Character))]
    [RequireComponent(typeof(Communicator))]

    class ChaPosition : MonoBehaviour
    {
        //Field
        //-------------------------------------------------------------------------------      
        private IWorld m_refWorld;
        private Communicator m_Communicator;

        private BlockLocation m_CurLoc;
        private Vector2Int m_PreChunkLoc;
        private Vector3Int m_PreSecLoc;

        private E_Player_LeaveChunk m_Cache_EChunkChange;

        private void Awake()
        {               
            m_Communicator = GetComponent<Communicator>(); 
        }
        private void Start()
        {
            m_refWorld = Locator<IWorld>.GetService();

            //Compute Current Location
            m_CurLoc = new BlockLocation(transform.position, m_refWorld);
            m_PreChunkLoc = m_CurLoc.ChunkInWorld.Value;
            m_PreSecLoc = m_CurLoc.SecInWorld.Value;

            //make event cache
            m_Cache_EChunkChange 
                = new E_Player_LeaveChunk(Vector2Int.zero,GetComponent<Character>().ViewDistance,Vector2Int.zero);
        }

        private void FixedUpdate()
        {
            //Save Location Data in Previous Frame 
            m_PreChunkLoc = m_CurLoc.ChunkInWorld.Value;
            m_PreSecLoc = m_CurLoc.SecInWorld.Value;

            //Update Location 
            m_CurLoc.Update(transform.position, m_refWorld);

            if (m_PreChunkLoc != m_CurLoc.ChunkInWorld.Value)
            {
                //Set Event
                m_Cache_EChunkChange.ChunkInWorld = m_CurLoc.ChunkInWorld.Value;
                m_Cache_EChunkChange.Offset = m_CurLoc.ChunkInWorld.Value - m_PreChunkLoc;

                //Publish Event: player's Chunk location has Changed
                Locator<IEventPublisher>.GetService().Publish(m_Cache_EChunkChange);

                Debug.Log("Chunk Location Changed");

                if (m_PreSecLoc != m_CurLoc.SecInWorld.Value)
                {
                    Debug.Log("Seciton Changed");
                }
            }
        }

        void OnGUI()
        {
            GUI.Label(new Rect(0 + 10, 0 + 10, 160, 20), "Location: " + m_CurLoc.BlkInWorld.Value.ToString());
            GUI.Label(new Rect(0 + 10, 0 + 30, 160, 20), "Sec Location: " + m_CurLoc.SecInWorld.Value.ToString());
            GUI.Label(new Rect(0 + 10, 0 + 50, 160, 20), "Blk In Section: " + m_CurLoc.BlkInSec.Value.ToString());
        }
    }

}
