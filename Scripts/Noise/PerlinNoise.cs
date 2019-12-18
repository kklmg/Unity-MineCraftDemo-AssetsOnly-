using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace MyNoise.Perlin
{
    //delegate fade function
    public delegate float del_FadeFunction(float t);

    //defined fade functions
    public static class FadeFunction
    {
        public static float Perlin_v0(float t)
        {
            return t * t * (3 - 2 * t);  // 3t^2 − 2t^3
        }
        public static float Perlin_v1(float t)
        {
            return t * t * t * (t * (t * 6 - 15) + 10);       // 6t^5 - 15t^4 + 10t^3
        }
    }

    //delegate of hash funcionn
    public delegate int del_HashFunction(int key);

    //defined hash functions
    public static class HashFunction
    {
        public static int MyHash(int key)
        {
            const uint BIT_NOISE1 = 0x53567A4D;
            const uint BIT_NOISE2 = 0x46E71DA4;
            const uint BIT_NOISE3 = 0x83f12d39;

            uint mangled = (uint)key;
            mangled *= BIT_NOISE1;
            mangled ^= (mangled >> 8);
            mangled += BIT_NOISE2;
            mangled ^= (mangled << 8);
            mangled *= BIT_NOISE3;
            mangled ^= (mangled >> 8);

            return (int)mangled;
        }
    }


    public interface INoiseMaker
    {
        float GetNoise_2D(Vector2 point);

        float GetNoise_3D(Vector3 point);
    }
    /// <summary>
    /// class perlin noise maker
    /// </summary>
    public class PerlinNoiseMaker : INoiseMaker
    {
        //Fileld
        const float NOISE_OFFSET = 0.001f;

        //Delegate----------------------------------------------------------------
        private del_FadeFunction m_delFadeFunction;       //Fade Function
        private del_HashFunction m_delHashFunction;       //Hash Function

        //Property----------------------------------------------------------------
        public del_FadeFunction FadeFun {set { m_delFadeFunction = value; } }
        public del_HashFunction HashFun { set { m_delHashFunction = value; } }


        //Constructor----------------------------------------------------------------
        public PerlinNoiseMaker()
        {
            //set default fade function
            m_delFadeFunction = new del_FadeFunction(FadeFunction.Perlin_v1);
            //set defatul hash function
            m_delHashFunction = new del_HashFunction(HashFunction.MyHash);
        }


        //Public Function---------------------------------------------------------------
        /// <summary>
        /// get a perlin noise 
        /// </summary>
        /// <param name="point">2 dimensiol input</param>
        /// <returns>outout range(0,1)</returns>
        public float GetNoise_2D(Vector2 point)
        {
            //for avoid integer input
            point.x += NOISE_OFFSET;
            point.y += NOISE_OFFSET;

            //Debug.Log("offset " + NOISE_OFFSET);

            //Debug.Log("point x" + point.x);
            //Debug.Log("point y" + point.y);
            //Debug.Log("point " + point);
            //       _______________
            //      |               |
            //      |  point        |
            //      |    *          |
            //      |               |
            //      |_______________|
            //    p_int

            //keep interger
            Vector2 p_int = new Vector2((int)point.x, (int)point.y);
            //Debug.Log("point_int " + p_int);

            //offset between  p_int and point: (0.0f,0.0f) ~ (1.0f,1.0f)
            Vector2 offset = point - p_int;

            //Debug.Log("offset :"+ offset.x);
            //Debug.Log("offset :" + offset.y);
            //vector positon to Lattice  surrounded the point
            Vector2[] verticies = new Vector2[4]
            {
                new Vector2(p_int.x, p_int.y + 1),      //left_top
                new Vector2(p_int.x + 1, p_int.y + 1),  //right_top
                new Vector2(p_int.x, p_int.y),          //left_bottom
                new Vector2(p_int.x + 1, p_int.y),      //right_bottom
            };

            //Debug.Log("verticies0 :" + verticies[0]);
            //Debug.Log("verticies1 :" + verticies[1]);
            //Debug.Log("verticies2 :" + verticies[2]);
            //Debug.Log("verticies3 :" + verticies[3]);

            // weight : product of random gradient and vector(position -> lattice)
            float weight_LT = Grad_2D(Hash_2D(verticies[0]), offset - new Vector2(0.0f, 1.0f));
            float weight_RT = Grad_2D(Hash_2D(verticies[1]), offset - new Vector2(1.0f, 1.0f));
            float weight_LB = Grad_2D(Hash_2D(verticies[2]), offset /*- new Vector2(0.0f, 0.0f)*/);
            float weight_RB = Grad_2D(Hash_2D(verticies[3]), offset - new Vector2(1.0f, 0.0f));

            //Debug.Log("wieght LT" + weight_LT.ToString("F16"));
            //Debug.Log("wieght RT" + weight_RT.ToString("F16"));
            //Debug.Log("wieght LB" + weight_LB.ToString("F16"));
            //Debug.Log("wieght RB" + weight_RB.ToString("F16"));


            //learp
            float lerpAbove = FadeLerp(weight_LT, weight_RT, offset.x);
            float lerpBottom = FadeLerp(weight_LB, weight_RB, offset.x);

            return FadeLerp(lerpBottom, lerpAbove, offset.y);
       
        }

        public float GetNoise_2D_abs(Vector2 point)
        {
            return (GetNoise_2D(point) + 1) / 2;
        }

        public float GetNoise_2D(Vector2 point, float frequency, float amplitude)
        {
            return GetNoise_2D(new Vector2(point.x * frequency, point.y * frequency)) * amplitude;
        }

        public float GetNoise_2D_abs(Vector2 point, float frequency, float amplitude)
        {
            return ((GetNoise_2D(new Vector2(point.x * frequency, point.y * frequency))+1)/2) * amplitude;
        }

        public float GetOctaveNoise_2D(Vector2 point,float frequency,float amplitude,int octaves=8)
        {
            float res = 0;
            float maxValue = 0;  // Used for normalizing result to 0.0 - 1.0


            for (int i = 0; i < octaves; i++)
            {
                res += GetNoise_2D(point,frequency,amplitude);

                maxValue += amplitude;//

                amplitude /= 2;
                frequency *= 2;
            }

            return res / maxValue;
        }

        public float GetNoise_3D(Vector3 point) { return 0; }//XXXXXXXXXXXXXXXXXXXXXX


        //Private Function----------------------------------------------------------------
        private int Hash_2D(Vector2 vertex)
        {
            return m_delHashFunction((int)vertex.x)/2 + m_delHashFunction((int)vertex.y)/2;
        }

        private int Hash_3D(Vector3 vertex)
        {
            return m_delHashFunction((int)vertex.x) / 3 
                +  m_delHashFunction((int)vertex.y) / 3
                +  m_delHashFunction((int)vertex.z) / 3;
        }

        //get weight (product of random gradient and the vector)
        private float Grad_2D(int hash, Vector2 p)
        {   
            switch (hash % 4)      //get fandom gradient
            {
                case 0x0: return  p.x + p.y;     //gradient(1,1)   * p => p.x *  1 + p.y *  1 
                case 0x1: return -p.x + p.y;    //gradient(-1,1)  * p => p.x * -1 + p.y *  1
                case 0x2: return  p.x - p.y;     //gradient(1,-1)  * p => p.x *  1 + p.y * -1
                case 0x3: return -p.x - p.y;    //gradient(-1,-1) * p => p.x * -1 + p.y * -1
                default: return 0;
            }
        }

        private float Grad_3D(int hash, Vector3 p)
        {
            switch (hash & 0xF)    //get fandom gradient          
            {
                case 0x0: return  p.x + p.y;
                case 0x1: return -p.x + p.y;
                case 0x2: return  p.x - p.y;
                case 0x3: return -p.x - p.y;
                case 0x4: return  p.x + p.z;
                case 0x5: return -p.x + p.z;
                case 0x6: return  p.x - p.z;
                case 0x7: return -p.x - p.z;
                case 0x8: return  p.y + p.z;
                case 0x9: return -p.y + p.z;
                case 0xA: return  p.y - p.z;
                case 0xB: return -p.y - p.z;
                case 0xC: return  p.y + p.x;
                case 0xD: return -p.y + p.z;
                case 0xE: return  p.y - p.x;
                case 0xF: return -p.y - p.z;
                default: return 0; 
            }        
        }

        public float FadeLerp(float left, float right, float t)
        {
            //case: no fade function setted, use liner lerp 
            if (m_delFadeFunction == null)
                return Mathf.Lerp(left, right, t);

            return Mathf.Lerp(left, right, m_delFadeFunction(t));
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
