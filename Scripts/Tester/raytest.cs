using UnityEngine;
using Assets.Scripts.NWorld;
using Assets.Scripts.NPattern;

namespace Assets.Scripts.Tester
{
    class raytest : MonoBehaviour
    {
        void Update()
        {

            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
           
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    Debug.DrawLine(ray.origin, hit.point);
                    Debug.Log(hit.point);
                }
            }
        }
    }
   

}
