using Assets.Scripts.InputHandler;
using Assets.Scripts.Action;
using Assets.Scripts.Pattern;
using UnityEngine;

namespace Assets.Scripts.BehaviorTree
{
    class BevFactory : Singleton<BevFactory>
    {
        public BevNodeBase MakePlayerInputBehavior(Transform trans)
        {           
            BevSequence root = new BevSequence();
            ConButtonDown jump = new ConButtonDown(INPUT_STR.JUMP);

            goup up = new goup(trans);

            BevRepeator repeat = new BevRepeator();

            repeat.setChild(root);

            root.AddChild(jump);
            root.AddChild(up);

            return repeat;
        }


    }
}
