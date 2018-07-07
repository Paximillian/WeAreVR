using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LinqExtensions
{
    public static void DoForEach<T>(this IEnumerable<T> collection, Action<T> operation)
    {
        foreach (T obj in collection)
        {
            operation(obj);
        }
    }
}