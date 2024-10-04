using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace System
{
   public static class StringUtilities
   {
      public static bool IsNullOrEmpty(this string s) => string.IsNullOrEmpty(s);
      public static bool IsNullOrWhiteSpace(this string value)
      {
         if (value == null) return true;

         for (int i = 0; i < value.Length; i++)
         {
            if (!Char.IsWhiteSpace(value[i])) return false;
         }

         return true;
      }

      public static bool Contains(this string s, string value, StringComparison comparison) => s.IndexOf(value, comparison) >= 0;
   }
}
