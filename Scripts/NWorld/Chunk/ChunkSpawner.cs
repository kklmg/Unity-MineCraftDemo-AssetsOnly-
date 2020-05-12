using System.Collections;
using System.Threading;

using UnityEngine;

using Assets.Scripts.NData;
using Assets.Scripts.NGameSystem;
using Assets.Scripts.NGlobal.Singleton;

namespace Assets.Scripts.NWorld
{
    [RequireComponent(typeof(WorldEntity))]
    public class ChunkSpawner : MonoBehaviour
    {
        private WorldEntity m_WorldEntity;
        private SaveMng m_SaveMng;

        //Unity Function
        //---------------------------------------------------------------------------
        private void Awake()
        {           
            m_WorldEntity = GetComponent<WorldEntity>();
            m_SaveMng = MonoSingleton<GameSystem>.Instance.SaveMngIns;
        }

        //public Function
        //---------------------------------------------------------------------------    
        public Chunk Spawn(ChunkInWorld _chunkinworld)
        {
            //Case: The Chunk has not  spawned
            if (!m_WorldEntity.TryGetChunk(_chunkinworld, out Chunk TempChunk))
            {
                //make a chunk instance
                TempChunk =
                    Instantiate(m_WorldEntity.Prefab_Chunk, transform).GetComponent<Chunk>();

                //Init Chunk
                TempChunk.SetLocation(_chunkinworld);
                TempChunk.GenerateBlocks();

                //Load data From save file
                m_SaveMng.LoadBlock(_chunkinworld,TempChunk);

                //Build Mesh
                TempChunk.BuildMesh();
    
                //TempChunk.BuildMeshInBackground();
                //TempChunk.RunBuildMeshCoro();

                //Add Chunk
                m_WorldEntity.AddChunk(_chunkinworld, TempChunk);
            }

            return TempChunk;
        }

        //Event Handler
        //---------------------------------------------------------------------------   
        //when the player has spawned spawn chunks neaerby the player
        public bool SpawnAt(Vector3 center,int extend,IWorld world)
        {
            ChunkInWorld SpawnCenter = new ChunkInWorld(center, world);

            //figure out visiable area 
            AreaRect ViewArea = new AreaRect(SpawnCenter.Value, extend);

            for (int i = ViewArea.left; i <= ViewArea.right; ++i)
            {
                for (int j = ViewArea.bottom; j <= ViewArea.top; ++j)
                {
                    Spawn(new ChunkInWorld(new Vector2Int(i, j), world));               
                }
            }
            return true;
        }

        static public void Thread_CreateChunkMesh(object chunk)
        {
            Chunk temp = (Chunk)chunk;
            temp.BuildMesh();
        }
    }
}


//using System.Collections;
//using System.Threading;

//using UnityEngine;

//using Assets.Scripts.NData;
//using Assets.Scripts.NGameSystem;
//using Assets.Scripts.NGlobal.Singleton;

//namespace Assets.Scripts.NWorld
//{
//    [RequireComponent(typeof(WorldEntity))]
//    public class ChunkSpawner : MonoBehaviour
//    {
//        private WorldEntity m_WorldEntity;
//        private SaveMng m_SaveMng;

//        //Unity Function
//        //---------------------------------------------------------------------------
//        private void Awake()
//        {
//            m_WorldEntity = GetComponent<WorldEntity>();
//            m_SaveMng = MonoSingleton<GameSystem>.Instance.SaveMngIns;
//        }

//        //public Function
//        //---------------------------------------------------------------------------    
//        public Chunk Spawn(ChunkInWorld _chunkinworld)
//        {
//            //Case: The Chunk has not  spawned
//            if (!m_WorldEntity.TryGetChunk(_chunkinworld, out Chunk TempChunk))
//            {
//                //make a chunk instance
//                TempChunk =
//                    Instantiate(m_WorldEntity.Prefab_Chunk, transform).GetComponent<Chunk>();

//                //Init Chunk
//                TempChunk.SetLocation(_chunkinworld);
//                TempChunk.GenerateBlocks();

//                //Load data From save file
//                m_SaveMng.LoadBlock(_chunkinworld, TempChunk);

//                GTimer.RestartTimer();

//                GTimer.ShowElapsedTimeAndRestart("ready ");
//                //Build Mesh
//                ThreadPool.QueueUserWorkItem(Thread_CreateChunkMesh, TempChunk);

//                GTimer.ShowElapsedTimeAndRestart("queue thread pool");
//                int workerThreads;
//                int portThreads;

//                ThreadPool.GetAvailableThreads(out workerThreads, out portThreads);
//                Debug.Log("available workerThreads" + workerThreads);
//                Debug.Log("available Maximum completion port threads" + portThreads);

//                //TempChunk.BuildMeshInBackground();
//                //TempChunk.RunBuildMeshCoro();

//                //Add Chunk
//                m_WorldEntity.AddChunk(_chunkinworld, TempChunk);
//            }

//            return TempChunk;
//        }

//        //Event Handler
//        //---------------------------------------------------------------------------   
//        //when the player has spawned spawn chunks neaerby the player
//        public bool SpawnAt(Vector3 center, int extend, IWorld world)
//        {
//            ChunkInWorld SpawnCenter = new ChunkInWorld(center, world);

//            //figure out visiable area 
//            AreaRect ViewArea = new AreaRect(SpawnCenter.Value, extend);

//            for (int i = ViewArea.left; i <= ViewArea.right; ++i)
//            {
//                for (int j = ViewArea.bottom; j <= ViewArea.top; ++j)
//                {
//                    Spawn(new ChunkInWorld(new Vector2Int(i, j), world));
//                }
//            }
//            return true;
//        }

//        static public void Thread_CreateChunkMesh(object chunk)
//        {
//            Chunk temp = (Chunk)chunk;
//            temp.BuildMeshInstantly();
//        }
//    }
//}
