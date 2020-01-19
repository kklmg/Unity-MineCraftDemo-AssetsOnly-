using Assets.Scripts.BehaviorTree;
using Assets.Scripts.Pattern;
using Assets.Scripts.InputHandler;


namespace Assets.Scripts.NCharacter
{
    public class ChaBevFactory : Singleton<ChaBevFactory>
    {
        public BevNodeBase ChaMoving_Control()
        {         
            BevSequence seq = new BevSequence();

            Control_Cha_Move control = new Control_Cha_Move();
            Cha_Move move = new Cha_Move();

            seq.AddChild(control);
            seq.AddChild(move);

            BevRepeator repeat = new BevRepeator(seq);

            return repeat;
        }
        public BevNodeBase ChaRotate_Control()
        {
            
            BevSequence seq = new BevSequence();

            Control_Cha_RotateY control = new Control_Cha_RotateY();
            Cha_Rotate rotate = new Cha_Rotate();

            seq.AddChild(control);
            seq.AddChild(rotate);

            BevRepeator repeat = new BevRepeator(seq);

            return repeat;
        }
        public BevNodeBase Camera_Control()
        {
            
            BevSequence seq = new BevSequence();

            Control_Camera_UpDown control = new Control_Camera_UpDown();
            CameraUpDown cam = new CameraUpDown();

            seq.AddChild(control);
            seq.AddChild(cam);

            BevRepeator repeat = new BevRepeator(seq);

            return repeat;
        }
        public BevNodeBase ChaControl_Base()
        {
            BevParallel parall = new BevParallel();
           
            parall.AddChild(new BevRepeator(new Cha_Jump()));
            parall.AddChild(this.ChaMoving_Control());
            parall.AddChild(this.ChaRotate_Control());
            parall.AddChild(this.Camera_Control());

            return parall;
        }
    }
}

