using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Assets.Scripts.InputHandler;

namespace Assets.Scripts.WorldComponent
{
    [RequireComponent(typeof(Player))]
    public class PlayerController : MonoBehaviour
    {
        public Transform m_Camera;
        public Player m_refPlayer;
        public World m_refWorld;

        [SerializeField]
        private float m_Horizontol;
        [SerializeField]
        private float m_Vertical;
        [SerializeField]
        private float m_Mouse_Horizontol;
        [SerializeField]
        private float m_Mouse_Vertical;
        [SerializeField]
        private Vector3 m_Velocity;
       

        private float m_VerticalMomentum = 0;

        //jump 
        [SerializeField]
        private bool m_isGrounded;
        [SerializeField]
        private float m_JumpForce = 5f;
        [SerializeField]
        private bool m_JumpRequest = false;


        private void Awake()
        {
            m_refPlayer = GetComponent<Player>();
            m_refWorld = m_refPlayer.refWorld;           
        }

        private void Update()
        {
            //receive inputs;
            m_Horizontol = Input.GetAxis(INPUT_STR.HORIZONTAL);
            m_Vertical = Input.GetAxis(INPUT_STR.VERTICAL);
            m_Mouse_Horizontol = Input.GetAxis(INPUT_STR.MOUSE_HORIZONTAL);
            m_Mouse_Vertical = Input.GetAxis(INPUT_STR.MOUSE_VERTICAL);

            if (m_isGrounded && Input.GetButtonDown(INPUT_STR.JUMP))
                m_JumpRequest = true;
        }

        /// <summary>
        /// 
        /// </summary>
        private void FixedUpdate()
        {
            if (m_JumpRequest)
            {
                m_VerticalMomentum = m_JumpForce;
                m_isGrounded = false;
                m_JumpRequest = false;
            }

            //Compute velocity
            m_Velocity = ((transform.forward * m_Vertical) + (transform.right * m_Horizontol) )
                * Time.fixedDeltaTime * m_refPlayer.WalkSpeed;

            //Apply vertical momentum 
            //m_Velocity += Vector3.up * m_VerticalMomentum * Time.fixedDeltaTime;



            if ((m_Velocity.z > 0 && Front()) || (m_Velocity.z < 0 && Back()))
                m_Velocity.z = 0;
            if ((m_Velocity.x > 0 && Right()) || (m_Velocity.x < 0 && Left()))
                m_Velocity.x = 0;


            if (!Down())
            {
                m_Velocity.y = -1;
                //m_isGrounded = true;
            }
             
            //Vector3 rotation = new Vector3(0, transform.eulerAngles.y, 0);
            //dir = Quaternion.Euler(rotation) * dir;
            //dir.Normalize();

            ////compute rotation
            //transform.Rotate(Vector3.up * Mouse_Horizontol);
            //m_Camera.Rotate(Vector3.right * -Mouse_Vertical);

            ////update result
            //transform.transform.position += dir * velocity * Time.deltaTime;

            transform.Rotate(Vector3.up * m_Mouse_Horizontol);
            m_Camera.Rotate(Vector3.right * -m_Mouse_Vertical);
            transform.Translate(m_Velocity, Space.World);
        }

        public bool Front()
        {
            Block target = m_refWorld.GetBlock(transform.position + Vector3.forward);
            if (target == null) return false;
            return target.IsSolid(eDirection.backward);
        }
        public bool Back()
        {
            Block target = m_refWorld.GetBlock(transform.position + Vector3.back);
            if (target == null) return false;
            return target.IsSolid(eDirection.forward);
        }
        public bool Left()
        {
            Block target = m_refWorld.GetBlock(transform.position + Vector3.left);
            if (target == null) return false;
            return target.IsSolid(eDirection.right);
        }
        public bool Right()
        {
            Block target = m_refWorld.GetBlock(transform.position + Vector3.right);
            if (target == null) return false;
            return target.IsSolid(eDirection.left);
        }
        public bool Up()
        {
            Block target = m_refWorld.GetBlock(transform.position + Vector3.up);
            if (target == null) return false;
            return target.IsSolid(eDirection.down);
        }
        public bool Down()
        {
            Block target = m_refWorld.GetBlock(transform.position + Vector3.down);
            if (target == null) return false;
            return target.IsSolid(eDirection.up);
        }
    }
}
