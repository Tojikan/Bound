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

        public jasonNamespace.Jason NestedJason;


        public Jason(int a, float b, string c, int[] d, jasonNamespace.Jason e = null)
        {
            leve = a;
            whatever = b;
            whatever2 = c;
            array = d;
            NestedJason = e;
        }
    }

    public class Freddy
    {
        public 
    }
}
