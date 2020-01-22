using UnityEngine;
using Assets.Scripts.SMesh;
using Assets.Scripts.Pattern;

namespace Assets.Scripts.NWorld
{

    public interface ISay
    {
        void say();
    }

    public class SayHello : ISay
    {
        public void say()
        {
            Debug.Log("Hello world");
        }
    }

    public class SayHi : ISay
    {
        public void say()
        {
            Debug.Log("Hi world");
        }
    }

    public static class GTest
    {
        static int m_RunCount = 5;
        public static int RunCount { set { m_RunCount = value; } get { return --m_RunCount; } }
        public static bool isExecuted { get; set; }
        static int m_Count;
        static public void DoCount(int c)
        {
            m_Count += c;
        }
        static public int GetCount()
        {
            return m_Count;
        }
    }

    class TestScript: MonoBehaviour
    {
        public ISay sayimpl;

        //public QuadMesh[] quads;
        public MeshFilter m_MeshFilter;
        public MeshData m_MeshData;

        private void Awake()
        {
        }
        private void Start()
        {
        }

        private void Update()
        {           
        }

    }
}
