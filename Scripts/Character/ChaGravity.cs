using UnityEngine;
using Assets.Scripts.EventManager;
using Assets.Scripts.WorldComponent;
using Assets.Scripts.Pattern;

namespace Assets.Scripts.CharacterSpace
{
    [RequireComponent(typeof(Character))]
    [RequireComponent(typeof(ComponentCommunicator))]
    class ChaGravity : MonoBehaviour 
    {
        [SerializeField]
        private float m_Gravity = 9.8f;
        private float m_DropSpeed = 0;

        private Character m_refCha;
        private ComponentCommunicator m_Communicator;
        private World m_refWorld;
        private Block m_Ground;


        public float Gravity { get { return m_Gravity; } }

        private void Awake()
        {
            m_refCha = GetComponent<Character>();     
        }
        private void Start()
        {
            m_refWorld = Locator<World>.GetService();
            m_Communicator = GetComponent<ComponentCommunicator>();
            m_Communicator.SubsribeEvent(E_Cha_Jump.ID, TriggerJump);
            m_Communicator.SubsribeEvent(E_Cha_Moved.ID, CheckGround);
        }

        private bool TriggerJump(IEvent _event)
        {
            E_Cha_Jump EJump = _event as E_Cha_Jump;
            m_DropSpeed = EJump.Force;
            Debug.Log("jump!"+EJump.Force);

            return true;
        }
        private bool CheckGround(IEvent _event)
        {
            E_Cha_Moved EMove = _event as E_Cha_Moved;
            m_Ground = m_refWorld.GetBlock(transform.position + Vector3.down);

            return true;
        }


        private void Update()
        {
            Block adj = m_refWorld.GetBlock(transform.position + Vector3.down);

            if (adj == null || !adj.IsSolid(eDirection.up) || m_DropSpeed>0.0f)
            {
                m_DropSpeed -= m_Gravity * Time.deltaTime;
                transform.Translate(new Vector3(0, m_DropSpeed, 0));
            }
            else m_DropSpeed = 0;
           
        }
    }
}
