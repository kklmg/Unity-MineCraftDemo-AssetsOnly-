using Assets.Scripts.BehaviorTree;
using Assets.Scripts.InputHandler;
using UnityEngine;

namespace Assets.Scripts.ActorSpace
{
    public class ComputeVelocity : BevLeaf
    {
        //Cache hot Data for improve perfomance
        private Vector3 Cache_Velocity;
        private float Cache_Horizontal = 0;
        private float Cache_Vertical = 0;
        private Actor Cache_Actor;

        protected override eRunningState Tick(BevData workData)
        {
            ActorBevData thisData = workData as ActorBevData;

            //read black board data
            thisData.GetValue<float>(INPUT_STR.HORIZONTAL, out Cache_Horizontal);
            thisData.GetValue<float>(INPUT_STR.VERTICAL, out Cache_Vertical);

            Cache_Actor = thisData.actor;

            //Debug.Log(Cache_Horizontal);
           // Debug.Log(Cache_Vertical);

            //computevelocity
            Cache_Velocity =
                ((Cache_Actor.transform.forward * Cache_Vertical)
                + (Cache_Actor.transform.right * Cache_Horizontal)) * Time.fixedDeltaTime
                * (thisData.isWalking ? Cache_Actor.WalkSpeed : Cache_Actor.RunSpeed);//walk or run

            thisData.SetValue(ActorBevData.KEY_VELOCITY, Cache_Velocity);

            //Debug.Log(Cache_Velocity);
            m_enRunningState = eRunningState.Suceed;
            return m_enRunningState;
        }
    }
    public class ApplyChanged : BevLeaf
    {
        Vector3 Cache_Velocity; 
        protected override eRunningState Tick(BevData workData)
        {
            ActorBevData thisData = workData as ActorBevData;

            thisData.GetValue(ActorBevData.KEY_VELOCITY,out Cache_Velocity);
            thisData.actor.transform.Translate(Cache_Velocity);        

            m_enRunningState = eRunningState.Suceed;
            return m_enRunningState;
        }

    }
    public class PlayerRotate : BevLeaf
    {
        float Mouse_Horizontol;
        protected override eRunningState Tick(BevData workData)
        {
            ActorBevData thisData = workData as ActorBevData;

            thisData.GetValue(INPUT_STR.MOUSE_HORIZONTAL, out Mouse_Horizontol);
            thisData.actor.transform.Rotate(Vector3.up * Mouse_Horizontol);

            m_enRunningState = eRunningState.Suceed;
            return m_enRunningState;
        }
    }
    public class CameraUpDown : BevLeaf
    {
        float Mouse_Vertical;
        protected override eRunningState Tick(BevData workData)
        {
            ActorBevData thisData = workData as ActorBevData;

            thisData.GetValue(INPUT_STR.MOUSE_VERTICAL, out Mouse_Vertical);
            thisData.actor.Camera.transform.Rotate(Vector3.right * -Mouse_Vertical);

            m_enRunningState = eRunningState.Suceed;
            return m_enRunningState;
        }
    }
}
