using System;
using UnityEngine;
using UnityEditor;

using Assets.Scripts.NMesh;

namespace Assets.Scripts.NWorld
{
    [Serializable]
    struct Tile
    {
        public MeshDataScriptable Mesh;
        public bool IsSolid;
        public int TexID;
    }

}
