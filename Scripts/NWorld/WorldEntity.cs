using System.Collections.Generic;
using UnityEngine;

using Assets.Scripts.NData;
using Assets.Scripts.NCache;
using Assets.Scripts.NGameSystem;
using Assets.Scripts.NGlobal.Singleton;

namespace Assets.Scripts.NWorld
{
    public class WorldEntity : MonoBehaviour
    {
        //Field
        //---------------------------------------------------------------------------
        public GameObject Prefab_Chunk;

        private IWorld m_refWorld;

        [SerializeField]
        private Dictionary<Vector2Int, Chunk> m_Chunks;

        //Unity Function
        //---------------------------------------------------------------------------
        private void Awake()
        {
            //int view = MonoSingleton<GameSystem>.Instance.PlayerMngIns.PlayerView;

            //m_refWorld = GetComponentInParent<IWorld>();
            //m_Chunks = new LRUCache<Vector2Int, Chunk>(view*view);
            m_Chunks = new Dictionary<Vector2Int, Chunk>();
        }


        //public Function
        //---------------------------------------------------------------------------  
        public Chunk GetChunk(ChunkInWorld _chunkinworld)
        {
            if (m_Chunks.TryGetValue(_chunkinworld.Value, out Chunk TempChunk))
            {
                return TempChunk;
            }
            else return null;
        }

        public bool TryGetChunk(ChunkInWorld _chunkinworld, out Chunk chunk)
        {
            return m_Chunks.TryGetValue(_chunkinworld.Value, out chunk);
        }

        public bool Contains(ChunkInWorld _chunkinworld)
        {
            return m_Chunks.ContainsKey(_chunkinworld.Value);
        }

        public void Remove(ChunkInWorld _chunkinworld)
        {
            m_Chunks.Remove(_chunkinworld.Value);
        }

        public void AddChunk(ChunkInWorld inWorld, Chunk chunk)
        {
            m_Chunks.Add(inWorld.Value,chunk);
        }

        public void GetNotInArea(ref AreaRect area,List<ChunkInWorld> Receive)
        {
            foreach (var data in m_Chunks)
            {
                if (!area.IsInvolvePoint(data.Key))
                {
                    Receive.Add(new ChunkInWorld(data.Key,m_refWorld));
                }
            }
        }
    }
}



//using System.Collections.Generic;
//using UnityEngine;

//using Assets.Scripts.NData;

//namespace Assets.Scripts.NWorld
//{
//    public class WorldEntity : MonoBehaviour
//    {
//        //Field
//        //---------------------------------------------------------------------------
//        public GameObject Prefab_Chunk;

//        private IWorld m_refWorld;

//        [SerializeField]
//        private Dictionary<Vector2Int, Chunk> m_Chunks;

//        //Unity Function
//        //---------------------------------------------------------------------------
//        private void Awake()
//        {
//            m_refWorld = GetComponentInParent<IWorld>();
//            m_Chunks = new Dictionary<Vector2Int, Chunk>();
//        }


//        //public Function
//        //---------------------------------------------------------------------------  
//        public Chunk GetChunk(ChunkInWorld _chunkinworld)
//        {
//            if (m_Chunks.TryGetValue(_chunkinworld.Value, out Chunk TempChunk))
//            {
//                return TempChunk;
//            }
//            else return null;
//        }

//        public bool TryGetChunk(ChunkInWorld _chunkinworld, out Chunk chunk)
//        {
//            return m_Chunks.TryGetValue(_chunkinworld.Value, out chunk);
//        }

//        public bool Contains(ChunkInWorld _chunkinworld)
//        {
//            return m_Chunks.ContainsKey(_chunkinworld.Value);
//        }

//        public void Remove(ChunkInWorld _chunkinworld)
//        {
//            m_Chunks.Remove(_chunkinworld.Value);
//        }

//        public void AddChunk(ChunkInWorld inWorld, Chunk chunk)
//        {
//            m_Chunks.Add(inWorld.Value,chunk);
//        }

//        public void GetNotInArea(ref AreaRect area,List<ChunkInWorld> Receive)
//        {
//            foreach (var data in m_Chunks)
//            {
//                if (!area.IsInvolvePoint(data.Key))
//                {
//                    Receive.Add(new ChunkInWorld(data.Key,m_refWorld));
//                }
//            }
//        }
//    }
//}
