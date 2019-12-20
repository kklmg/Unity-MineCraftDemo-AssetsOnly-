using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.InputHandler
{
    public static class INPUT_STR
    {
        public const string HORIZONTAL = "Horizontal";
        public const string VERTICAL = "Vertical";
        public const string ATTACK = "Attack";
        public const string JUMP = "Jump";
        public const string SWITCH = "Switch";
        public const string RUN = "Run";
    }

    public interface IMoveHandler
    {


    }
    public class InputHandler : MonoBehaviour
    {
        public Transform Camera;
        public float Horizontol;
        public float Vertical;
        public float Mouse_Horizontol;
        public float Mouse_Vertical;
        public int velocity = 1;
        public Vector3 dir;
        private void Update()
        {          
            Horizontol = Input.GetAxis("Horizontal");
            Vertical = Input.GetAxis("Vertical");
            Mouse_Horizontol = Input.GetAxis("Mouse X");
            Mouse_Vertical = Input.GetAxis("Mouse Y");
           
            dir = new Vector3(Horizontol, 0, Vertical);
            dir.Normalize();
            Vector3 rotation = new Vector3(0, transform.eulerAngles.y, 0);
            dir = Quaternion.Euler(rotation) * dir;
            //Camera.Rotate(Vector3.up * Mouse_Horizontol);
            //Camera.Rotate(Vector3.right * -Mouse_Vertical);

            transform.Rotate(Vector3.up * Mouse_Horizontol);
            //transform.Rotate(Vector3.right * -Mouse_Vertical);
            transform.transform.position += dir * velocity * Time.deltaTime;
        }
    }
}
