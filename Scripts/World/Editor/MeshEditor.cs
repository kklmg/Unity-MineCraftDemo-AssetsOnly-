﻿using System;
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
        private MeshData meshinfo;

        public void OnEnable()
        {
            meshinfo = target as MeshData;
        }

        public override void OnInspectorGUI()
        {
            
            serializedObject.ApplyModifiedProperties();
           
            if (GUILayout.Button("Face to -Z", GUILayout.Width(200)))
            {
                //meshinfo.Reset();
                meshinfo.AddQuad(new Vector3(0, 0, 0), new Vector3(0, 1, 0), new Vector3(1, 1, 0), new Vector3(1, 0, 0));
            }
            if (GUILayout.Button("Face to +Z", GUILayout.Width(200)))
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
            EditorUtility.SetDirty(meshinfo);
        }
        private void OnDisable()
        {
            serializedObject.ApplyModifiedProperties();
            EditorUtility.SetDirty(meshinfo);
            Debug.Log("diabled");
        }
    }
}