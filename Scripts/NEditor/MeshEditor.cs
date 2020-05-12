using UnityEngine;
using UnityEditor;

using Assets.Scripts.NMesh;

namespace Assets.Scripts.NEditor
{
    [CustomEditor(typeof(DynamicMeshScObj))]
    class TileEditor : Editor
    {
        private DynamicMeshScObj MeshInfo;

        public void OnEnable()
        {
            MeshInfo = target as DynamicMeshScObj;
        }

        public override void OnInspectorGUI()
        {
            serializedObject.ApplyModifiedProperties();

            DynamicMesh EditingMesh = MeshInfo.Mesh;

            if (GUILayout.Button("Make Front Mesh", GUILayout.Width(200)))
            {
                EditingMesh.Clear();
                EditingMesh.AddQuad(new Vector3(1, 0, 1), new Vector3(1, 1, 1), new Vector3(0, 1, 1), new Vector3(0, 0, 1));
            }

            if (GUILayout.Button("Make Back Mesh", GUILayout.Width(200)))
            {
                EditingMesh.Clear();
                EditingMesh.AddQuad(new Vector3(0, 0, 0), new Vector3(0, 1, 0), new Vector3(1, 1, 0), new Vector3(1, 0, 0));
            }

            if (GUILayout.Button("Make Up Mesh", GUILayout.Width(200)))
            {
                EditingMesh.Clear();
                EditingMesh.AddQuad(new Vector3(0, 1, 0), new Vector3(0, 1, 1), new Vector3(1, 1, 1), new Vector3(1, 1, 0));
            }
            if (GUILayout.Button("Make Down Mesh", GUILayout.Width(200)))
            {
                EditingMesh.Clear();
                EditingMesh.AddQuad(new Vector3(0, 0, 1), new Vector3(0, 0, 0), new Vector3(1, 0, 0), new Vector3(1, 0, 1));
            }
            if (GUILayout.Button("Make Left Mesh", GUILayout.Width(200)))
            {
                EditingMesh.Clear();
                EditingMesh.AddQuad(new Vector3(0, 0, 1), new Vector3(0, 1, 1), new Vector3(0, 1, 0), new Vector3(0, 0, 0));
            }
            if (GUILayout.Button("Make Right Mesh", GUILayout.Width(200)))
            {
                EditingMesh.Clear();
                EditingMesh.AddQuad(new Vector3(1, 0, 0), new Vector3(1, 1, 0), new Vector3(1, 1, 1), new Vector3(1, 0, 1));
            }
            if (GUILayout.Button("Make Cross Mesh", GUILayout.Width(200)))
            {
                EditingMesh.Clear();
                //EditingMesh.AddQuad(new Vector3(1, 0, 0), new Vector3(1, 1, 0), new Vector3(1, 1, 1), new Vector3(1, 0, 1));
            }
            if (GUILayout.Button("Make Double Slice Mesh", GUILayout.Width(200)))
            {
               EditingMesh.Clear();
               //EditingMesh.AddQuad(new Vector3(1, 0, 0), new Vector3(1, 1, 0), new Vector3(1, 1, 1), new Vector3(1, 0, 1));
            }

            GUILayout.Space(20);

            base.OnInspectorGUI();
            EditorUtility.SetDirty(MeshInfo);
        }
        private void OnDisable()
        {
            serializedObject.ApplyModifiedProperties();
            EditorUtility.SetDirty(MeshInfo);
        }
    }
}
