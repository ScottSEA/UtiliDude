using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public static class CollectionUtilities
{
    public static bool IsEmpty<T>(this ICollection<T> collection) => collection.Count == 0;
    public static bool IsEmpty(this ICollection collection) => collection.Count == 0;
    public static bool IsNullOrEmpty<T>(this ICollection<T> collection) => collection is null || collection.Count == 0;
    public static bool IsNullOrEmpty(this ICollection collection) => collection is null || collection.Count == 0;
}
