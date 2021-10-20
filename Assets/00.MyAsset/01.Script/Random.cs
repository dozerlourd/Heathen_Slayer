using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HCH
{
    

    public class Random
    {
        public static float RandomToDateTime()
        {
            // 무조건 보완해야함
            int milliSeconds = System.DateTime.Now.Millisecond;
            Debug.Log("CurrTime: " + milliSeconds);
            return milliSeconds;
        }
    }

    public class Well512
    {
        static uint[] state = new uint[16];
        static uint index = 0;

        static Well512()
        {
            System.Random random = new System.Random((int)System.DateTime.Now.Ticks);

            for (int i = 0; i < 16; i++)
            {
                state[i] = (uint)random.Next();
            }
        }

        internal static uint Next(int minValue, int maxValue)
        {
            return (uint)((Next() % (maxValue - minValue)) + minValue);
        }

        public static uint Next(uint maxValue)
        {
            return Next() % maxValue;
        }

        public static uint Next()
        {
            uint a, b, c, d;

            a = state[index];
            c = state[(index + 13) & 15];
            b = a ^ c ^ (a << 16) ^ (c << 15);
            c = state[(index + 9) & 15];
            c ^= (c >> 11);
            a = state[index] = b ^ c;
            d = a ^ ((a << 5) & 0xda442d24U);
            index = (index + 15) & 15;
            a = state[index];
            state[index] = a ^ b ^ d ^ (a << 2) ^ (b << 18) ^ (c << 28);

            return state[index];
        }
    }
}