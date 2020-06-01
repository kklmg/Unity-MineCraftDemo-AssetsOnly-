using Assets.Scripts.NBehaviorTree;
using Assets.Scripts.NGlobal.Singleton;

namespace Assets.Scripts.NCharacter
{
    public class ChaBevFactory : Singleton<ChaBevFactory>
    {
        public BevNodeBase ChaMoving_Control()
        {         
            BevSequence seq = new BevSequence();

           // Control_Cha_Move control = new Control_Cha_Move();
            ChaBev_HandleMovement move = new ChaBev_HandleMovement();

            //seq.AddChild(control);
            seq.AddChild(move);

            BevRepeator repeat = new BevRepeator(seq);

            return repeat;
        }
        public BevNodeBase ChaRotation_Control()
        { 
            BevSequence seq = new BevSequence();
            
            ChaBev_HandleYaw Yaw = new ChaBev_HandleYaw();

            seq.AddChild(Yaw);

            BevRepeator repeat = new BevRepeator(seq);

            return repeat;
        }
      
        public BevNodeBase ChaAct_Base()
        {
            BevParallel parall = new BevParallel();
           
            parall.AddChild(new BevRepeator(new ChaBev_HandleJump()));
            parall.AddChild(this.ChaMoving_Control());
            parall.AddChild(this.ChaRotation_Control());
            //parall.AddChild(this.Camera_Control());

            return parall;
        }
    }
}

