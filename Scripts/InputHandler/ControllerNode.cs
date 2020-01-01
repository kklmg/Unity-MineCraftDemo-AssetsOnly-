﻿using UnityEngine;
using Assets.Scripts.BehaviorTree;
using Assets.Scripts.Pattern;

namespace Assets.Scripts.InputHandler
{
    class Control_Horizontal : BevConditionBase
    {
        private float Cache_Hor;
        public override bool Check(BevData workData)
        {
            Cache_Hor = ServiceLocator<IController>.GetService().Horizontal();

            if (Mathf.Approximately(Cache_Hor, 0)) return false;
            else
            {
                workData.SetValue(KEY_CONTROL.HORIZONTAL, Cache_Hor);
                return true;
            }
        }
    }
    class Control_Vertical : BevConditionBase
    {
        private float Cache_Ver;
        public override bool Check(BevData workData)
        {
            Cache_Ver = ServiceLocator<IController>.GetService().Vertical();

            if (Mathf.Approximately(Cache_Ver, 0)) return false;
            else
            {
                workData.SetValue(KEY_CONTROL.VERTICAL, Cache_Ver);
                return true;
            }
        }
    }
    class Control_Rotate_X : BevConditionBase
    {
        private float cache;
        public override bool Check(BevData workData)
        {
            cache = ServiceLocator<IController>.GetService().Rotate_X();

            if (Mathf.Approximately(cache, 0)) return false;
            else
            {
                workData.SetValue(KEY_CONTROL.HORIZONTAL, cache);
                return true;
            }
        }
    }
    class Control_Rotate_Y : BevConditionBase
    {
        private float cache;
        public override bool Check(BevData workData)
        {
            cache = ServiceLocator<IController>.GetService().Rotate_Y();

            if (Mathf.Approximately(cache, 0)) return false;
            else
            {
                workData.SetValue(KEY_CONTROL.HORIZONTAL, cache);
                return true;
            }
        }
    }
    class Control_Rotate_Z : BevConditionBase
    {
        private float cache;
        public override bool Check(BevData workData)
        {
            cache = ServiceLocator<IController>.GetService().Rotate_Z();

            if (Mathf.Approximately(cache, 0)) return false;
            else
            {
                workData.SetValue(KEY_CONTROL.HORIZONTAL, cache);
                return true;
            }
        }
    }
    class Control_Jump : BevConditionBase
    {
        public override bool Check(BevData workData)
        {
            return ServiceLocator<IController>.GetService().Jump();
        }
    }
    class Control_Fire : BevConditionBase
    {
        public override bool Check(BevData workData)
        {
            return ServiceLocator<IController>.GetService().Fire();
        }
    }
    class Control_Sprint : BevConditionBase
    {
        public override bool Check(BevData workData)
        {
            return ServiceLocator<IController>.GetService().Sprint();
        }
    }
}