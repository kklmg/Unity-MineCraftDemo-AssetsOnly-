﻿using UnityEngine;
using Assets.Scripts.NWorld;
using Assets.Scripts.NEvent;
using Assets.Scripts.NEvent.Impl;
using Assets.Scripts.NServiceLocator;

namespace Assets.Scripts.NUser
{
    class Cursor : MonoBehaviour
    {
        public GameObject go_placeFrame;
        public GameObject go_PickFrame;

        [SerializeField]
        private int m_HandLength;

        //Cache
        //----------------------------------
        private IWorld m_refWorld;
        private Ray m_Ray;
        private BlockLocation m_Picked;
        private BlockLocation m_Placed;
        private Vector3 m_Checking;
        private Collider m_Check_bound;

        //testing
        public  Vector3 m_offset;

        private void Awake()
        {
            m_refWorld = Locator<IWorld>.GetService();

            m_Check_bound = go_PickFrame.GetComponent<BoxCollider>();
            Debug.Log(m_Check_bound);
            //Debug.LogAssertion(m_Check_bound != null);
        }
        private void Start()
        {
            
        }
        private void FixedUpdate()
        {
            ChangeBlock();
        }

        private void ChangeBlock() 
        {
            if (PickBlock())
            {
                //click
                if (Input.GetMouseButtonDown(0))
                {
                    //!!!!!!!!!!!!!!!!!!!!!!!!!
                    Locator<IEventPublisher>.GetService().Publish(new E_Block_Change(ref m_Picked,0));
                }
            }
        } 

        void PlaceBlock()
        {
            if (PickBlockSide())
            {
                //click
                if (Input.GetMouseButtonDown(0))
                {
                    m_Placed.CurBlockID = 1;
                    Debug.Log("pick suceed!");
                }
            }
        }

        bool PickBlock()
        {
            int distance = m_HandLength;

            //cast ray from mouse position
            m_Ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            m_Checking = m_Ray.origin;

            //test
            do
            {
                m_Checking += m_Ray.direction;

                m_Picked.SetLocation(m_Checking, m_refWorld);

                if (m_Picked.IsValid())
                {
                    go_PickFrame.SetActive(true);
                    go_PickFrame.transform.position = m_Picked.Bound.center;
                    return true;
                }
            }
            while (--distance > 0);

            go_PickFrame.SetActive(false);
            return false;
        }

        bool PickBlockSide()
        {
            //Case 
            if (PickBlock())
            {
                if (CheckHitSide(m_Ray))
                {
                    go_placeFrame.SetActive(true);
                    go_placeFrame.transform.position = m_Placed.Bound.center;
                    m_Placed.SetLocation(m_Checking + m_offset,m_refWorld);    
                    return true;
                }
            }

            go_placeFrame.SetActive(false);
            return false;
        }

        bool CheckHitSide(Ray _ray)
        {
            m_offset = default(Vector3);
            RaycastHit rayhit;
            if (m_Check_bound.Raycast(_ray, out rayhit, m_HandLength))
            {
                //Debug.Log("hit offset: " + m_offset);
                m_offset = rayhit.normal;
                return true;
            }
            return false;
        }
    }
}
