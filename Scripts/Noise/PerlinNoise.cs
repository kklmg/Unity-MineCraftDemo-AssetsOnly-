using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace MyNoise.Perlin
{


    //    class PerlinNoise_2D
    //{
    //    float[][] m_arrNoisies;



    //}




    /// <summary>
    /// delefate fade function
    /// </summary>
    /// <param name="t"></param>
    /// <returns></returns>
    public delegate float del_Fade(float t);

    /// <summary>
    ///  delegate of hash funcionn
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public delegate int del_Hash(int key);

    /// <summary>
    /// class perlin noise maker
    /// </summary>
    class PerlinNoiseMaker
    {
        private static readonly uint NOISE1 = 0x12345678;
        private static readonly uint NOISE2 = 0xaf9d5efa;

        private static readonly Vector2[] m_arrGradients = new Vector2[]
            { new Vector2(1, 0), new Vector2(0, 1), new Vector2(0, 1), new Vector2(0, 0) };

        private del_Fade m_delFade;




        /// <summary>
        /// base constuctor
        /// </summary>
        public PerlinNoiseMaker()
        {
            m_delFade = new del_Fade(Perlinfade_v1);
        }

        private int __GetHash(int key)
        {
            uint res = (uint)key;
            res *= NOISE1;
            res *= NOISE2;
            return (int)res;
        }
        private int __GetHash_2d(Vector2 vertex)
        {
            return __GetHash((int)vertex.x) + __GetHash((int)vertex.y);
        }

        //get inner product of random gradient and the vector 
        float Grad(Vector2 vertex, Vector2 p)
        {
            switch (__GetHash_2d(vertex) & 0x3)
            {
                case 0x0: return p.x + p.y;     //gradient(1,1) * p => p.x + p.y
                case 0x1: return -p.x + p.y;    //gradient(-1,1) ................
                case 0x2: return p.x - p.y;     //gradient(1,-1) ................
                case 0x3: return -p.x - p.y;    //gradient(-1,-1) ...............
                default: return 0;
            }
        }

        // Fade function as defined by Ken Perlin.
        // version 0
        public static float Perlinfade_v0(float t)
        {
            return t * t * (3 - 2 * t);  // 3t^2 − 2t^3
        }
        public static float Perlinfade_v1(float t)
        {
            return t * t * t * (t * (t * 6 - 15) + 10);       // 6t^5 - 15t^4 + 10t^3
        }

        public void setFadeFun(del_Fade funFade)
        {
            m_delFade = funFade;
        }

        /// <summary>
        /// //defined fade function
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        #region FadeFunction
        public float FadeLerp(float left, float right, float t)
        {
            if (m_delFade == null)
                return Mathf.Lerp(left, right, t);

            return Mathf.Lerp(left, right, m_delFade(t));
        }

        #endregion

        public float MakeNoise(Vector2 point)
        {
            //
            Vector2 p_int = new Vector2((int)point.x, (int)point.y);

            //Lattice verticies surrounded the point
            Vector2[] verticies = new Vector2[4]
            {
                new Vector2(p_int.x, p_int.y),          //left_bottom
                new Vector2(p_int.x + 1, p_int.y),      //right_bottom
                new Vector2(p_int.x, p_int.y + 1),      //left_top
                new Vector2(p_int.x + 1, p_int.y + 1),  //right_top
            };

            Vector2 p_float = point - p_int;

            float lerpAbove = FadeLerp(Grad(verticies[0], p_float), Grad(verticies[1], p_float - new Vector2(1.0f, 0.0f)), p_float.x);
            float lerpBottom = FadeLerp(Grad(verticies[0], p_float), Grad(verticies[1], p_float - new Vector2(1.0f, 0.0f)), p_float.x);

            return FadeLerp(lerpAbove, lerpBottom, p_float.y);
        }


    }
}





//public double OctavePerlin(double x, double y, double z, int octaves, double persistence)
//{
//    double total = 0;
//    double frequency = 1;
//    double amplitude = 1;
//    double maxValue = 0;  // Used for normalizing result to 0.0 - 1.0
//    for (int i = 0; i < octaves; i++)
//    {
//        total += perlin(x * frequency, y * frequency, z * frequency) * amplitude;

//        maxValue += amplitude;

//        amplitude *= persistence;
//        frequency *= 2;
//    }

//    return total / maxValue;
//}
