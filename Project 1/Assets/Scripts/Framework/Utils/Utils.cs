﻿using UnityEngine;

namespace Framework
{
    public static class Utils
    {
        public static T PickRandom<T>(T[] array)
        {
            return array[Random.Range(0, array.Length)];
        }
    }
}