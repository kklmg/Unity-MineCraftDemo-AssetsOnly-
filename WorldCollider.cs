using UnityEngine;
using Assets.Scripts;
using Assets.Scripts.WorldComponent;

namespace Assets
{
    [RequireComponent(typeof(Player))]
    [RequireComponent(typeof(PositionData))]
    class WorldCollider : MonoBehaviour
    {
        [SerializeField]
        float m_Gravity = 1.5f;
        [SerializeField]
        int m_Height;

        private Player m_refPlayer;
        private World m_refWorld;
        private PositionData m_refPosData;
        //private MyObserver<Vector3Int> m_obsPlayerSlot;
        

        void Awake()
        {
            m_refPlayer = GetComponent<Player>();
            m_refWorld = m_refPlayer.refWorld;
            m_refPosData = GetComponent<PositionData>();
            //m_refPlayer.SubjectWorldSlot.Subscribe(m_obsPlayerSlot);
        }

    }
}
