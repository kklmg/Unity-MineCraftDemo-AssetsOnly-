//using UnityEngine;
//using UnityEditor;

//using Assets.Scripts.NWorld;

//namespace Assets.Scripts.NEditor
//{
//    [CustomEditor(typeof(BlockPalette))]
//    class BlockPaletteEditor : Editor
//    {
//        BlockPalette m_PaletteInfo;

//        public void OnEnable()
//        {
//            m_PaletteInfo = target as BlockPalette;
//        }
//        public override void OnInspectorGUI()
//        {
            

//            if (GUILayout.Button("setRow", GUILayout.Width(200)))
//            {
//                m_TextureInfo.SetMaxRow(m_nRow);
//            }
//            if (GUILayout.Button("setColumn", GUILayout.Width(200)))
//            {
//                m_TextureInfo.SetMaxColumn(m_nColumn);
//            }

//            GUILayout.Space(20);

//            base.OnInspectorGUI();
//            serializedObject.ApplyModifiedProperties();
//        }
//        private void OnDisable()
//        {
//            EditorUtility.SetDirty(target);
//        }

//    }
//}
