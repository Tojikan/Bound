using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace jasonNamespace
{
    [Serializable]
    public class Jason
    {
        public int leve;
        public float whatever;
        public string whatever2;
        public int[] array;
        public jasonNamespace.Freddy NestedFreddy;


        public Jason(int a, float b, string c, int[] d, jasonNamespace.Freddy e = null)
        {
            leve = a;
            whatever = b;
            whatever2 = c;
            array = d;
            NestedFreddy = e;
        }
    }

    public class Freddy
    {
        public string stringA;
        public string stringB;

        public Freddy(string a, string b)
        {
            stringA = a;
            stringB = b;
        }


    }
}
