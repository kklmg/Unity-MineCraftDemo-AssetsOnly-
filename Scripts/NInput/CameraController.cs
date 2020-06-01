using System;

using UnityEngine;

using Assets.Scripts.NGameSystem;
using Assets.Scripts.NCharacter;
using Assets.Scripts.NGlobal.Singleton;
using Assets.Scripts.NGlobal.ServiceLocator;

namespace Assets.Scripts.NInput
{
    class CameraController : MonoBehaviour
    {
        public GameObject Prefab_Camera;
        
        [SerializeField]
        private ChaPlayer m_FollowedPlayer;
        private IController m_Controller;

        private float m_Sensitivity = 1.0f;
        public float RotateSensitivity
        {
            set
            {
                m_Sensitivity = value;
            }
        }

        [SerializeField]
        private float m_Yaw;
        [SerializeField]
        private float m_Pitch;

        public Camera CameraIns { private set; get; }

        public void FollowObj(Transform trans)
        {
            CameraIns.transform.parent = trans;
            CameraIns.transform.localPosition = Vector3.zero;
            CameraIns.transform.localRotation= Quaternion.identity;
            CameraIns.transform.localScale = Vector3.one;
        }

        private void Awake()
        {
            CameraIns = Instantiate(Prefab_Camera).GetComponent<Camera>();
        }


        private void Start()
        {
            m_FollowedPlayer =
                MonoSingleton<GameSystem>.Instance.PlayerMngIns.PlayerScript;
            m_Controller = Locator<IController>.GetService();

            FollowObj(m_FollowedPlayer.TransformEye);
        }



        private void Update()
        {
            m_Yaw = m_Controller.Rotate_Yaw();
            m_Pitch = m_Controller.Rotate_Pitch();
            
           // m_FollowedPlayer.TransEye.set

            if (Mathf.Approximately(0.0f, m_Pitch)) return;
            CameraIns.transform.Rotate(Vector3.right * -m_Pitch * m_Sensitivity);
        }
    }
}
