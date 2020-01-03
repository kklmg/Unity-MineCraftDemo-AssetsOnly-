using Assets.Scripts.BehaviorTree;
using Assets.Scripts.InputHandler;
using Assets.Scripts.EventManager;
using Assets.Scripts.Pattern;

using UnityEngine;

namespace Assets.Scripts.CharacterSpace
{
    class Control_Cha_Move : BevConditionBase
    {
        private float m_fHor;
        private float m_fVer;
        private E_Cha_TryMove m_ECha_TryMove = new E_Cha_TryMove();

        public override bool Check(BevData workData)
        {
            ChaBevData thisData = workData as ChaBevData;
            IController control = Locator<IController>.GetService();

            //get input
            m_fHor = control.Horizontal();
            m_fVer = control.Vertical();

            //not valid input
            if (Mathf.Approximately(0.0f, m_fHor)
                && Mathf.Approximately(0.0f, m_fVer)) return false;

            //calculate movement
            thisData.Movement.x = m_fHor * Time.deltaTime
                * (thisData.isWalking ? thisData.Character.WalkSpeed : thisData.Character.RunSpeed);
            thisData.Movement.z = m_fVer * Time.deltaTime
            * (thisData.isWalking ? thisData.Character.WalkSpeed : thisData.Character.RunSpeed);

            //publish this event
            m_ECha_TryMove.Trans = thisData.Character.transform;
            m_ECha_TryMove.Movement = thisData.Movement;
            Locator<IEventPublisher>.GetService().PublishAndHandle(m_ECha_TryMove);

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

            thisData.Rotation_Y = Cache_Rotate;

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
            Cache_Rotate = control.Rotate_Z();

            //not valid input
            if (Mathf.Approximately(0.0f, Cache_Rotate)) return false;

            //save to blackboard
            thisData.SetValue(KEY_CONTROL.CAMERA_UD, Cache_Rotate);

            return true;
        }


    }
    public class Cha_Move : BevLeaf
    {
        Vector3 Cache_Velocity;
        E_Cha_Moved m_ECha_move = new E_Cha_Moved();
        protected override eRunningState Tick(BevData workData)
        {
            ChaBevData thisData = workData as ChaBevData;
            thisData.Character.transform.Translate(thisData.Movement);

            //notify orther system
            m_ECha_move.Cha = thisData.Character;
            Locator<IEventPublisher>.GetService().Publish(m_ECha_move);    

            m_enRunningState = eRunningState.Suceed;
            return m_enRunningState;
        }
    }
    public class Cha_Rotate : BevLeaf
    {
        protected override eRunningState Tick(BevData workData)
        {
            ChaBevData thisData = workData as ChaBevData;

            thisData.Character.transform.Rotate(Vector3.up * thisData.Rotation_Y);

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
            thisData.Character.Camera.transform.Rotate(Vector3.right *-Camera_UD);

            m_enRunningState = eRunningState.Suceed;
            return m_enRunningState;
        }
    }
}
