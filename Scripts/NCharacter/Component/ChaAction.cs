using UnityEngine;
using Assets.Scripts.NGlobal.ServiceLocator;
using Assets.Scripts.NGlobal.WorldSearcher;
using Assets.Scripts.NWorld;
using Assets.Scripts.NEvent;

namespace Assets.Scripts.NCharacter
{
    [RequireComponent(typeof(Communicator))]
    class ChaAction : MonoBehaviour
    {
        Communicator m_Communicator;

        private void Awake()
        {
            m_Communicator = GetComponent<Communicator>();
        }

        private void Start()
        {
            m_Communicator.SubsribeEvent(E_Cha_MoveRequest_XZ.ID, HandleMovement_XZ,9);
            m_Communicator.SubsribeEvent(E_Cha_YawRequest.ID, HandleYaw, 9);
            m_Communicator.SubsribeEvent(E_Cha_RotateRequest.ID, HandleRotation,9);
            m_Communicator.SubsribeEvent(E_Cha_MoveRequest_Y.ID, HandleMovement_Y, 9);
        }

        bool HandleMovement_XZ(IEvent _event)
        {
            Vector3 trans = (_event as E_Cha_MoveRequest_XZ).Translation;
            transform.position += trans;

            m_Communicator.PublishEvent(new E_Cha_Moved_XZ());
            
            return true;
        }

        bool HandleMovement_Y(IEvent _event)
        {
            float Y = (_event as E_Cha_MoveRequest_Y).Speed;
            transform.position =
                new Vector3(transform.position.x, transform.position.y + Y, transform.position.z);

            return true;
        }

        bool HandleYaw(IEvent _event)
        {
            float Y = (_event as E_Cha_YawRequest).Value;
            transform.Rotate(Vector3.up,Y);
            return true;
        }

        bool HandleRotation(IEvent _event)
        {
            Vector3 rotation = (_event as E_Cha_RotateRequest).Rotation;
            transform.Rotate(rotation);
            return true;
        }
    }
}
