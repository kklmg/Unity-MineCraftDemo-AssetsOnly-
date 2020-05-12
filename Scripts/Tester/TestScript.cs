using System.Collections.Generic;
using System.Collections;
using System.IO;
using System.Threading;

using UnityEngine;

using Assets.Scripts.NData;
using Assets.Scripts.NMesh;
using Assets.Scripts.NWorld;
using System;

namespace Assets.Scripts.Tester
{
    [System.Serializable]
    public struct TestST
    {
        public int HP;
        public int MP;
        public int STR;
        public int DEF;
        private int AGL;

        public List<int> weapon;
    }




    class TestScript : MonoBehaviour
    {
        //    public DynamicMeshScObj scobj;

        //    private void OnGUI()
        //    {

        //        //GUI.Label(new Rect(0 + 20, 0 + 350, 160, 20), "scobj: " + (scobj != null));
        //        //GUI.Label(new Rect(0 + 20, 0 + 275, 160, 20), "scobj data: " + (scobj.Data!=null));
        //        //GUI.Label(new Rect(0 + 20, 0 + 300, 160, 20), "vertex: " + scobj.Data._Vertices.Count);
        //        //GUI.Label(new Rect(0 + 20, 0 + 325, 160, 20), "indicies: " + scobj.Data._Indicies.Count);
        //        //// GUI.Label(new Rect(0 + 20, 0 + 350, 160, 20), "tex count: " + tile[);



        //    }


        //}


    }
}







