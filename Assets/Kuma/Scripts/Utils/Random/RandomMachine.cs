/*
    EnumHelper.cs
    Author: Kuma
    Created-Date: 2020-03-15
    
    -----Description-----
    枚舉的幫助類.
*/





using System;

namespace Kuma.Utils {

    public static class RandomMachine {

        public static T GetEnumRandom<T> () where T : Enum {
            Array values = Enum.GetValues (typeof (T));
            T value = (T)values.GetValue (UnityEngine.Random.Range (0, values.Length));
            return value;
        }

    }

}