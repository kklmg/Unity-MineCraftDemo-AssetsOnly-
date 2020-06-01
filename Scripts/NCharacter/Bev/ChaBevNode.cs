using UnityEngine;

using Assets.Scripts.NBehaviorTree;
using Assets.Scripts.NInput;
using Assets.Scripts.NEvent;
using Assets.Scripts.NWorld;
using Assets.Scripts.NGlobal.ServiceLocator;
using Assets.Scripts.NGlobal.WorldSearcher;

namespace Assets.Scripts.NCharacter
{
    //class Control_Cha_RotateYaw : BevConditionBase
    //{
    //    private float Cache_Rotate;
    //    public override bool Check(BevData workData)
    //    {
    //        ChaBevData thisData = workData as ChaBevData;
    //        IController control = Locator<IController>.GetService();

    //        //get input
    //        Cache_Rotate = control.Rotate_Yaw();

    //        //not valid input
    //        if (Mathf.Approximately(0.0f, Cache_Rotate)) return false;

    //        thisData.Rotation.y = Cache_Rotate;

    //        return true;
    //    }
    //}

    //class Control_Camera_UpDown : BevConditionBase
    //{
    //    private float Cache_Rotate;
    //    public override bool Check(BevData workData)
    //    {
    //        ChaBevData thisData = workData as ChaBevData;
    //        IController control = Locator<IController>.GetService();

    //        //get input
    //        Cache_Rotate = control.Rotate_Yaw();

    //        //not valid input
    //        if (Mathf.Approximately(0.0f, Cache_Rotate)) return false;

    //        //save to blackboard
    //        thisData.SetValue(KEY_CONTROL.CAMERA_UD, Cache_Rotate);

    //        return true;
    //    }
    //}

    public class ChaBev_HandleMovement : BevLeaf
    {
        protected override eRunningState Tick(BevData workData)
        {
            ChaBevData thisData = workData as ChaBevData;

            if (thisData.Request_Translation.TryHandle(out Vector3 reqTrans))
            {
                reqTrans = thisData.isWalking ?
                  reqTrans * thisData.Character.WalkSpeed
                : reqTrans * thisData.Character.RunSpeed;
              
                if (reqTrans.x > 1.0f)
                {
                    reqTrans.x = 1.0f;
                }
                if (reqTrans.y > 1.0f)
                {
                    reqTrans.y = 1.0f;
                }
                if (reqTrans.z > 1.0f)
                {
                    reqTrans.z = 1.0f;
                }

                E_Cha_MoveRequest_XZ req = new E_Cha_MoveRequest_XZ(reqTrans);

                //publih this event to character's components
                thisData.NotifyOtherComponents(new E_Cha_MoveRequest_XZ(reqTrans));

                m_enRunningState = eRunningState.Suceed;
                return m_enRunningState;
            }

            else
            {
                m_enRunningState = eRunningState.Failed;
                return m_enRunningState;
            }
        }
    }


    public class ChaBev_HandleYaw : BevLeaf
    {
        protected override eRunningState Tick(BevData workData)
        {
            ChaBevData thisData = workData as ChaBevData;
        
            if (thisData.Request_Yaw.TryHandle(out float reqYaw))
            {
                //publih this event to character's components
                thisData.NotifyOtherComponents(new E_Cha_YawRequest(reqYaw));

                m_enRunningState = eRunningState.Suceed;
                return m_enRunningState;
            }
            else
            {
                m_enRunningState = eRunningState.Failed;
                return m_enRunningState;
            }
        }
    }

    public class ChaBev_HandleJump : BevConditionBase
    {
        public override bool Check(BevData workData)
        {
            ChaBevData thisData = workData as ChaBevData;

            //Check Jump Request 
            if (thisData.Request_Jump.TryHandle(out bool isJump) && !thisData.isInAir)
            {
                thisData.isInAir = true;
                
                thisData.NotifyOtherComponents(new E_Cha_StartJump(thisData.Character.JumpForce));
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    //public class Cha_NotGrounded : BevConditionBase
    //{
    //    private IWorld m_refWorld;
    //    //private E_Cha_TryMove m_ECha_TryMove = new E_Cha_TryMove();
    //    protected override void VEnter(BevData workData)
    //    {
    //        m_refWorld = Locator<IWorld>.GetService();
    //    }
    //    public override bool Check(BevData workData)
    //    {
    //        ChaBevData thisData = workData as ChaBevData;
    //        IController control = Locator<IController>.GetService();

            
    //        IBlock adj = GWorldSearcher.GetBlock(thisData.Character.transform.position + Vector3.down,m_refWorld);
    //        return !(adj != null && adj.IsObstacle);
    //    }
    //}
}
