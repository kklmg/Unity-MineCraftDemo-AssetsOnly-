using UnityEngine;

using Assets.Scripts.NMesh;
using Assets.Scripts.NData;

namespace Assets.Scripts.NWorld
{
    [System.Serializable]
    [CreateAssetMenu(menuName = "block")]
    public class Block : ScriptableObject, IBlock
    {
        //Field------------------------------------------
        [SerializeField]
        private Sprite m_Icon;

        [SerializeField]
        private bool m_IsOpaque = true;

        [SerializeField]
        private bool m_IsObstacle = true;

        [SerializeField]
        private Tile m_UpTile;
        [SerializeField]
        private Tile m_DownTile;
        [SerializeField]
        private Tile m_LeftTile;
        [SerializeField]
        private Tile m_RightTile;
        [SerializeField]
        private Tile m_FrontTile;
        [SerializeField]
        private Tile m_BackTile;

        public Sprite Icon { get { return m_Icon; } }

        public bool IsSpecial { get { return false; } }
        public bool IsOpaque { get { return m_IsOpaque; } }
        public bool IsObstacle { get { return m_IsObstacle; } }

        public bool IsLeftMeshExist { get { return m_LeftTile != null; } }
        public bool IsRigthMeshExist { get { return m_RightTile != null; } }
        public bool IsUpMeshExist { get { return m_UpTile != null; } }
        public bool IsDownMeshExist { get { return m_DownTile != null; } }
        public bool IsFrontMeshExist { get { return m_FrontTile != null; } }
        public bool IsBackMeshExist { get { return m_BackTile != null; } }

       

        public DynamicMesh GetLeftMesh(TextureSheet tex = null, int x = 0, int y = 0, int z = 0)
        {
            if (m_LeftTile == null) return null;
            return m_LeftTile.GetClonedMesh(x, y, z, tex);
        }

        public DynamicMesh GetRightMesh(TextureSheet tex = null, int x = 0, int y = 0, int z = 0)
        {
            if (m_RightTile == null) return null;
            return m_RightTile.GetClonedMesh(x, y, z, tex);
        }

        public DynamicMesh GetUpMesh(TextureSheet tex = null, int x = 0, int y = 0, int z = 0)
        {
            if (m_UpTile == null) return null;
            return m_UpTile.GetClonedMesh(x, y, z, tex);
        }

        public DynamicMesh GetDownMesh(TextureSheet tex = null, int x = 0, int y = 0, int z = 0)
        {
            if(m_DownTile == null) return null;
            return m_DownTile.GetClonedMesh(x, y, z, tex);
        }

        public DynamicMesh GetFrontMesh(TextureSheet tex = null, int x = 0, int y = 0, int z = 0)
        {
            if (m_FrontTile == null) return null;
            return m_FrontTile.GetClonedMesh(x, y, z, tex);
        }

        public DynamicMesh GetBackMesh(TextureSheet tex = null, int x = 0, int y = 0, int z = 0)
        {
            if (m_BackTile == null) return null;
            return m_BackTile.GetClonedMesh(x, y, z, tex);
        }

        public DynamicMesh GetAllMesh(TextureSheet tex, int x, int y, int z)
        {
            DynamicMesh TempMesh = new DynamicMesh();

            TempMesh.Add(GetUpMesh(tex, x, y, z));
            TempMesh.Add(GetDownMesh(tex, x, y, z));
            TempMesh.Add(GetLeftMesh(tex, x, y, z));
            TempMesh.Add(GetRightMesh(tex, x, y, z));
            TempMesh.Add(GetFrontMesh(tex, x, y, z));
            TempMesh.Add(GetBackMesh(tex, x, y, z));

            return TempMesh;
        }
    }



}
