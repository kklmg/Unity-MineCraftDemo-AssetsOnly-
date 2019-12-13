using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using MyNoise.Perlin;
using UnityEditor;

namespace Assets.Scripts.Noise
{
    class NoiseTester : MonoBehaviour
    {
        public GameObject targetprefab;

        private INoiseMaker m_NoiseMaker;

        public int Magnitude;

        [Range(0.01f, 0.5f)]
        public float interval = 0.5f;

        public int scale_x = 10;
        public int scale_z = 10;

        private void Awake()
        {
            m_NoiseMaker = new PerlinNoiseMaker();
        }

        private void Start()
        {
            test_1D();
            //test_2D();
        }

        void test_1D()
        {
            for (float x = 0.0f; x < 1.0f; x += interval)
            {
                //float y = Mathf.PerlinNoise(x, 1.0f);
                float y = m_NoiseMaker.GetNoise_2D(new Vector2(x, 1.0f));
                Instantiate(targetprefab, new Vector3(x*scale_x, y * Magnitude, 1.0f),Quaternion.Euler(0,180,0));
            }

        }
        void test_2D()
        {
            for (float x = 0.0f; x < 10.0f; x += 0.1f)
            {
                for (float z = 0.0f; z < 10.0f; z += 0.1f)
                {
                    //float y = Mathf.PerlinNoise(x, z);
                    float y = m_NoiseMaker.GetNoise_2D(new Vector2(x* scale_x, z));
                    Instantiate(targetprefab, new Vector3(x * Magnitude, y * Magnitude, z * Magnitude), Quaternion.identity);
                }

            }

        }


    }
}
