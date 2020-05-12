using UnityEngine;

using Assets.Scripts.NBehaviorTree;
using Assets.Scripts.NInput;
using Assets.Scripts.NEvent;
using Assets.Scripts.NWorld;
using Assets.Scripts.NGlobal.ServiceLocator;
using Assets.Scripts.NGlobal.WorldSearcher;

namespace Assets.Scripts.NCharacter
{
    class Control_Cha_Move : BevConditionBase
    {
        private float m_fHor;
        private float m_fVer;
        private ChaBevData m_ThisData;      
        private IController m_Controller;

        protected override void VEnter(BevData workData)
        {
            m_ThisData = workData as ChaBevData;
            m_Controller = Locator<IController>.GetService();
        }

        public override bool Check(BevData workData)
        {
            //get input
            m_fHor = m_Controller.Horizontal();
            m_fVer = m_Controller.Vertical();

            //not valid input
            if (Mathf.Approximately(0.0f, m_fHor)
                && Mathf.Approximately(0.0f, m_fVer)) return false;

            //calculate movement
            m_ThisData.Translation.x = m_fHor * Time.fixedDeltaTime
                * (m_ThisData.isWalking ? m_ThisData.Character.WalkSpeed : m_ThisData.Character.RunSpeed);

            m_ThisData.Translation.z = m_fVer * Time.fixedDeltaTime
            * (m_ThisData.isWalking ? m_ThisData.Character.WalkSpeed : m_ThisData.Character.RunSpeed);

            //compute direction
            m_ThisData.Translation = m_ThisData.Character.transform.rotation * m_ThisData.Translation;

            return true;
        }
    }
    class Control_Cha_RotateY : BevConditionBase
    {
        private float Cache_Rotate;
        public override bool Check(BevData workData)
        {
            ChaBevData thisData = workData as ChaBevData;
            IController control = Locator<IController>.GetService();

            //get input
            Cache_Rotate = control.Rotate_Y();

            //not valid input
            if (Mathf.Approximately(0.0f, Cache_Rotate)) return false;

            thisData.Rotation.y = Cache_Rotate;

            return true;
        }
    }

    class Control_Camera_UpDown : BevConditionBase
    {
        private float Cache_Rotate;
        public override bool Check(BevData workData)
        {
            ChaBevData thisData = workData as ChaBevData;
            IController control = Locator<IController>.GetService();

            //get input
            Cache_Rotate = control.Rotate_X();

            //not valid input
            if (Mathf.Approximately(0.0f, Cache_Rotate)) return false;

            //save to blackboard
            thisData.SetValue(KEY_CONTROL.CAMERA_UD, Cache_Rotate);

            return true;
        }
    }

    public class Cha_ApplyMove : BevLeaf
    {
        E_Cha_MoveRequest m_EMoveRequest = new E_Cha_MoveRequest(Vector3.zero);
        protected override eRunningState Tick(BevData workData)
        {
            ChaBevData thisData = workData as ChaBevData;

            //set move request event
            m_EMoveRequest.Translation = thisData.Translation;

            //notify orther component
            thisData.Communicator.PublishEvent(m_EMoveRequest);
           
            m_enRunningState = eRunningState.Suceed;
            return m_enRunningState;
        }
    }

    public class Player_move
    {


    }

    public class Cha_Rotate : BevLeaf
    {
        protected override eRunningState Tick(BevData workData)
        {
            ChaBevData thisData = workData as ChaBevData;

            thisData.Character.transform.Rotate(Vector3.up * thisData.Rotation.y);

            m_enRunningState = eRunningState.Suceed;
            return m_enRunningState;
        }
    }

    public class CameraUpDown : BevLeaf
    {
        float Camera_UD;
        protected override eRunningState Tick(BevData workData)
        {
            ChaBevData thisData = workData as ChaBevData;

            thisData.GetValue(KEY_CONTROL.CAMERA_UD, out Camera_UD);
            thisData.Character.TCamera.Rotate(Vector3.right *-Camera_UD);

            m_enRunningState = eRunningState.Suceed;
            return m_enRunningState;
        }
    }

    public class Cha_Jump : BevConditionBase
    { 
        public override bool Check(BevData workData)
        {
            ChaBevData thisData = workData as ChaBevData;
            IController control = Locator<IController>.GetService();

            //get input
            bool trigger = control.Jump();
            if (!trigger) return false;

            //notify subscribed component
            thisData.Communicator.PublishEvent(new E_Cha_Jump(thisData.Character.JumpForce));
            //Debug.Log("jump triggerd");
            return true;
        }
    }

    public class Cha_NotGrounded : BevConditionBase
    {
        private IWorld m_refWorld;
        //private E_Cha_TryMove m_ECha_TryMove = new E_Cha_TryMove();
        protected override void VEnter(BevData workData)
        {
            m_refWorld = Locator<IWorld>.GetService();
        }
        public override bool Check(BevData workData)
        {
            ChaBevData thisData = workData as ChaBevData;
            IController control = Locator<IController>.GetService();

            
            IBlock adj = GWorldSearcher.GetBlock(thisData.Character.transform.position + Vector3.down,m_refWorld);
            return !(adj != null && adj.IsObstacle);
        }
    }
}
