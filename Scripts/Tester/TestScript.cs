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
        { }

        private void Update()
        {

            
        }

    }
}
