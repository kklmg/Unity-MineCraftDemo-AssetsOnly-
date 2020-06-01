using UnityEngine;

using Assets.Scripts.NGlobal.ServiceLocator;
using Assets.Scripts.NGlobal.Singleton;
using Assets.Scripts.NGameSystem;
using Assets.Scripts.NCharacter;

namespace Assets.Scripts.NInput
{
    class PlayerController : MonoBehaviour
    {
        private float m_fHor; //Horizontol Input
        private float m_fVer; //Vertical Input
        private float m_fYaw; //Yaw Input
        private bool m_bJump;


        private Vector3 m_Translation;
        private IController m_Controller;
        private ChaBevData m_BevData;   //PLayer Behavior Board

        private float m_Sensitivity = 1.0f;
        public float RotateSensitivity
        {
            set
            {
                m_Sensitivity = value;
            }
        }

        private void Start()
        {
            m_Controller = Locator<IController>.GetService();
            m_BevData = MonoSingleton<GameSystem>.Instance.PlayerMngIns.BevData;
        }

        private void Update()
        {
            //get input
            m_fHor = m_Controller.Horizontal();
            m_fVer = m_Controller.Vertical();
            m_fYaw = m_Controller.Rotate_Yaw();
           
            //check if translation valuies are valid
            if (!Mathf.Approximately(0.0f, m_fHor)
                || !Mathf.Approximately(0.0f, m_fVer))
            {
                //compute translation per frame
                m_Translation.x = m_fHor * Time.fixedDeltaTime;
                m_Translation.z = m_fVer * Time.fixedDeltaTime;

                //compute direction
                m_Translation = m_BevData.Character.transform.rotation * m_Translation;

                //write value to bev Board
                m_BevData.Request_Translation.SetRequest(m_Translation); ;
            }

            //check if Yaw value is valid
            if (!Mathf.Approximately(0.0f, m_fYaw))
            {
                //write value to bev Board
                m_BevData.Request_Yaw.SetRequest(m_fYaw * m_Sensitivity);
            }


            m_bJump = m_Controller.Jump();

            //jump 
            if(m_bJump)
                m_BevData.Request_Jump.SetRequest(true);
        }   
    }
}

