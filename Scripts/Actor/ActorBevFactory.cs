using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

using Assets.Scripts.BehaviorTree;
using Assets.Scripts.Pattern;
using Assets.Scripts.InputHandler;


namespace Assets.Scripts.ActorSpace
{
    public class ActorBevFactory : Singleton<ActorBevFactory>
    {
        public BevNodeBase InputHandler()
        {
            BevRepeator repeat = new BevRepeator();
            BevSequence root = new BevSequence();
            BevConditionBase inputAxis = InputBevFactory.Instance.HasInput_Axis(); 
            ComputeVelocity comVelocity = new ComputeVelocity();
            ApplyChanged applyChange = new ApplyChanged();
                        
            root.AddChild(inputAxis);
            root.AddChild(comVelocity);
            root.AddChild(applyChange);

            repeat.setChild(root);

            return repeat;
        }
        public BevNodeBase MouseHandler()
        {         
            BevRepeator repeat = new BevRepeator();
            BevParallel parall = new BevParallel();
            BevSequence seqHor = new BevSequence();
            BevSequence seqVer = new BevSequence();
            ConInputAxis mouseHor = InputBevFactory.Instance.InputAxis(INPUT_STR.MOUSE_HORIZONTAL);
            ConInputAxis mouseVer = InputBevFactory.Instance.InputAxis(INPUT_STR.MOUSE_VERTICAL);
            PlayerRotate rotate = new PlayerRotate();
            CameraUpDown camera = new CameraUpDown();

            seqHor.AddChild(mouseHor);
            seqHor.AddChild(rotate);

            seqVer.AddChild(mouseVer);
            seqVer.AddChild(camera);

            parall.AddChild(seqVer);
            parall.AddChild(seqHor);

            repeat.setChild(parall);

            return repeat;
        }
        public BevConditionBase isJumping()
        {
            ConButtonDown jump = new ConButtonDown(INPUT_STR.JUMP);

            jump.InputEvent = (BevData workData) =>
            {
                ActorBevData thisData = workData as ActorBevData;
                thisData.isJumping = true;
            };
            return jump;
        }
        public BevNodeBase PLayerBev()
        {
            BevParallel parall = new BevParallel();

            BevRepeator repeat = new BevRepeator();
            BevNodeBase keyinput = InputHandler();
            BevNodeBase mouseinput = MouseHandler();

            repeat.setChild(parall);
            parall.AddChild(keyinput);
            parall.AddChild(mouseinput);

            return repeat;
        }

    }
}

