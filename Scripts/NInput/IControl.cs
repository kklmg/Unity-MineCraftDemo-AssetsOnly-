using UnityEngine;
using Assets.Scripts.NBehaviorTree;

namespace Assets.Scripts.NInput
{
    public interface IController
    {
        float Horizontal();
        float Vertical();
        
        bool Fire();
        bool Jump();
        bool Sprint();
        bool Back();

        float Rotate_X();
        float Rotate_Y();
        float Rotate_Z();

        bool CursorDown();
        bool CursorUp();
        bool CursorPress();
        Vector3 CursorPosition();

        bool HasCursorMoved();
    }
}
