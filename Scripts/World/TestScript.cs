using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Assets.Scripts.SMesh;

namespace Assets.Scripts.NWorld
{
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    [RequireComponent(typeof(MeshCollider))]
    class TestScript: MonoBehaviour
    {
        //public QuadMesh[] quads;
        public MeshFilter m_MeshFilter;
        public MeshData m_MeshData;
        private void Start()
        {
            m_MeshFilter = gameObject.GetComponent<MeshFilter>();
            m_MeshData.ToMeshFilter(m_MeshFilter);
        }

    }
}
