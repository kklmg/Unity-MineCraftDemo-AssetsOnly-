using UnityEngine;
using Assets.Scripts.NEvent;
using Assets.Scripts.NEvent.Impl;

using Assets.Scripts.NWorld;
using Assets.Scripts.NServiceLocator;

namespace Assets.Scripts.NCharacter
{
    [RequireComponent(typeof(Character))]
    [RequireComponent(typeof(Communicator))]
    class ChaGravity : MonoBehaviour 
    {
        [SerializeField]
        private float m_Gravity = 9.8f;
        [SerializeField]
        private float m_DropSpeed = 0;
        [SerializeField]
        private float m_Ground;

        private Character m_refCha;
        private Communicator m_Communicator;
        private IWorld m_refWorld;
        

        public float Gravity { get { return m_Gravity; } }
       
        private bool TriggerJump(IEvent _event)
        {
            E_Cha_Jump EJump = _event as E_Cha_Jump;
            m_DropSpeed = EJump.Force;

            return true;
        }
        private bool CheckGround(IEvent _event)
        {
            E_Cha_Moved EMove = _event as E_Cha_Moved;         
            m_Ground = GWorldSearcher.GetGroundHeight(transform.position,m_refWorld);
            return true;
        }
        
        private void Awake()
        {
            m_refCha = GetComponent<Character>();
            m_Communicator = GetComponent<Communicator>();
        }
        private void Start()
        {
            m_refWorld = Locator<IWorld>.GetService();          
            m_Communicator.SubsribeEvent(E_Cha_Jump.ID, TriggerJump);
            m_Communicator.SubsribeEvent(E_Cha_Moved.ID, CheckGround);

            m_Ground = GWorldSearcher.GetGroundHeight(transform.position,m_refWorld);
        }
        private void Update()
        {
            //Block bottom = m_refWorld.GetBlock(transform.position + Vector3.down);

            //if (bottom == null || !bottom.IsSolid(eDirection.up) || m_DropSpeed>0.0f)
            if (transform.position.y- m_refCha.BodyHeight > m_Ground || m_DropSpeed > 0.0f)
            {
                m_DropSpeed -= m_Gravity * Time.deltaTime;
                transform.Translate(new Vector3(0, m_DropSpeed, 0));
                if (transform.position.y < m_Ground)
                {
                    transform.position = new Vector3(
                        transform.position.x, m_Ground + m_refCha.BodyHeight, transform.position.z);
                };
            }
            else m_DropSpeed = 0;       
        }
    }
}
