using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEditor;
using Assets.Scripts.SMesh;


namespace Assets.Scripts.World
{
    [CustomEditor(typeof(MeshData))]
    class MeshEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            MeshData meshinfo = target as MeshData;

            if (GUILayout.Button("forward", GUILayout.Width(200)))
            {
                //meshinfo.Reset();
                meshinfo.AddQuad(new Vector3(0, 0, 0), new Vector3(0, 1, 0), new Vector3(1, 1, 0), new Vector3(1, 0, 0));
            }
            if (GUILayout.Button("backward", GUILayout.Width(200)))
            {
                //meshinfo.Reset();
                meshinfo.AddQuad(new Vector3(1, 0, 1), new Vector3(1, 1, 1), new Vector3(0, 1, 1), new Vector3(0, 0, 1));
            }
            if (GUILayout.Button("up", GUILayout.Width(200)))
            {
                //meshinfo.Reset();
                meshinfo.AddQuad(new Vector3(0, 1, 0), new Vector3(0, 1, 1), new Vector3(1, 1, 1), new Vector3(1, 1, 0));
            }
            if (GUILayout.Button("down", GUILayout.Width(200)))
            {
                //meshinfo.Reset();
                meshinfo.AddQuad(new Vector3(0, 0, 1), new Vector3(0, 0, 0), new Vector3(1, 0, 0), new Vector3(1, 0, 1));
            }
            if (GUILayout.Button("left", GUILayout.Width(200)))
            {
                //meshinfo.Reset();
                meshinfo.AddQuad(new Vector3(0, 0, 1), new Vector3(0, 1, 1), new Vector3(0, 1, 0), new Vector3(0, 0, 0));
            }
            if (GUILayout.Button("right", GUILayout.Width(200)))
            {
                //meshinfo.Reset();
                meshinfo.AddQuad(new Vector3(1, 0, 0), new Vector3(1, 1, 0), new Vector3(1, 1, 1), new Vector3(1, 0, 1));
            }

            GUILayout.Space(20);

            base.OnInspectorGUI();
        }


    }
}
