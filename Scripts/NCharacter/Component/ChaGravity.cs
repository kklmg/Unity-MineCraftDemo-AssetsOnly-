using UnityEngine;

using Assets.Scripts.NGlobal.ServiceLocator;
using Assets.Scripts.NWorld;
using Assets.Scripts.NEvent;
using Assets.Scripts.NData;

namespace Assets.Scripts.NCharacter.Component
{
    [RequireComponent(typeof(Character))]
    [RequireComponent(typeof(Communicator))]
    class ChaGravity : MonoBehaviour
    {
        public float m_Gravity = 9.8f;
        public bool m_IsGround = false;

        public float m_YAxisSpeed = 0.0f;

        private float m_BodyHeight_Half;

        private Communicator m_Communicator;

        public IWorld m_World;

        private void Awake()
        {
            m_BodyHeight_Half = GetComponent<Character>().BodyHeight / 2;
            m_Communicator = GetComponent<Communicator>();
        }

        private void OnEnable()
        {
            //Handle jump start
            m_Communicator.SubsribeEvent(E_Cha_StartJump.ID, HandleJump, 2);

            //Handle Touch Ground
            m_Communicator.SubsribeEvent(E_Cha_TouchGround.ID, HandleTouchGround, 2);

            //Handle Touch Upside
            m_Communicator.SubsribeEvent(E_Cha_TouchUpsideBlock.ID, HandleTouchUpside, 2);

            //Check Ground
            m_Communicator.SubsribeEvent(E_Cha_Moved_XZ.ID, CheckGround, 0);

            //Global Event
            Locator<IEventHelper>.GetService().Subscribe(E_Block_Recover.ID, CheckGround, 0);
            Locator<IEventHelper>.GetService().Subscribe(E_Block_Modify.ID, CheckGround, 0);
        }

        private void OnDisable()
        {
            //Handle jump start
            m_Communicator.UnSubscribe(E_Cha_StartJump.ID, HandleJump);

            //Handle Touch Ground
            m_Communicator.UnSubscribe(E_Cha_TouchGround.ID, HandleTouchGround);

            //Handle Touch Upside
            m_Communicator.UnSubscribe(E_Cha_TouchUpsideBlock.ID, HandleTouchUpside);

            //Check Ground
            m_Communicator.UnSubscribe(E_Cha_Moved_XZ.ID, CheckGround);

            //Global Event
            Locator<IEventHelper>.GetService().UnSubscribe(E_Block_Recover.ID, CheckGround);
            Locator<IEventHelper>.GetService().UnSubscribe(E_Block_Modify.ID, CheckGround);

            CheckGround(null);
        }

        private void Start()
        {
            m_World = Locator<IWorld>.GetService();
        }

        private void Update()
        {
            if (m_IsGround == false)
            {
                m_YAxisSpeed -= m_Gravity * Time.deltaTime;
                if (m_YAxisSpeed > 1.0f) m_YAxisSpeed = 0.9999f;
                if (m_YAxisSpeed < -1.0f) m_YAxisSpeed = -0.9999f;
                m_Communicator.PublishEvent(new E_Cha_MoveRequest_Y(m_YAxisSpeed)); 
            }
        }

        private bool HandleJump(IEvent _event)
        {
            E_Cha_StartJump EJump = _event as E_Cha_StartJump;
            m_IsGround = false;
            m_YAxisSpeed = EJump.Force;

            return false;
        }

        private bool HandleTouchGround(IEvent _event)
        {
            m_IsGround = true;
            m_YAxisSpeed = 0.0f;

            return true;
        }

        private bool HandleTouchUpside(IEvent _event)
        {           
            m_YAxisSpeed = 0.0f;
            return true;
        }

        //Check Ground
        private bool CheckGround(IEvent _event)
        {
            Vector3 intersection = Vector3.zero;

            BlockLocation Loc =
                new BlockLocation(transform.position + new Vector3(0, -0.9999f, 0), m_World);

            //there is a obstacle block downside
            if (!Loc.IsBlockExists() || !Loc.CurBlockRef.IsObstacle)
            {
                m_IsGround = false;
            }
            return true;
        }
    }
}
