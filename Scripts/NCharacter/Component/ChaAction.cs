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
            m_Communicator.SubsribeEvent(E_Cha_MoveRequest.ID, HandleMovement);
            m_Communicator.SubsribeEvent(E_Cha_RotateRequest.ID, HandleRotation);
        }

        bool HandleMovement(IEvent _event)
        {
            Vector3 trans = (_event as E_Cha_MoveRequest).Translation;
            transform.position += trans;
            
            //transform.Translate(trans);
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
