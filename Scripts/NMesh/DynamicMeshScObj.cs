using System;
using UnityEngine;

namespace Assets.Scripts.NMesh
{
    [System.Serializable]
    [CreateAssetMenu(menuName = "DynamicMesh")]
    public class DynamicMeshScObj : ScriptableObject
    {
        [SerializeField]
        private DynamicMesh m_Mesh;

        public DynamicMesh Mesh { get { return m_Mesh; } }

        public DynamicMesh GetClonedMesh(int x, int y, int z)
        {  
            DynamicMesh TempMesh = m_Mesh.Clone();

            //Compute translated postion
            TempMesh.Translate(new Vector3(x, y, z));

            return TempMesh;
        }
    }
}
