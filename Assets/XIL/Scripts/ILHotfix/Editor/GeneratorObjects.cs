﻿#if USE_HOT
namespace wxb
{
    using UnityEditor;
    using UnityEngine;
    using System.Reflection;
    using System.Collections.Generic;
    using System.Linq;

    public static class GeneratorObjects
    {
        public static void Gen(HashSet<int> ps)
        {
            for (int i = 1; i <= 10; ++i)
                ps.Remove(i);

            int[] v = ps.ToArray();
            System.Array.Sort(v, (x, y) => { return x.CompareTo(y); });
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            string marco, suffix;
            AutoRegILType.GetPlatform(out marco, out suffix);
            string filepath = string.Format("Assets/XIL/Auto/GenObjects_{0}.cs", suffix);

            sb.AppendLine(string.Format("#if {0}", marco));
            sb.Append(@"namespace IL
{
    public partial struct Objects
    {"
);
            for (int i = 0; i < v.Length; ++i)
            {
                sb.AppendLine();
                Gen(sb, v[i]);
            }

            sb.Append(@"
    }
}
");
            sb.Append("#endif");
            System.IO.File.WriteAllText(filepath, sb.ToString());
        }

        [UnityEditor.MenuItem("Assets/Objects")]
        public static void Gen()
        {
            Gen(new HashSet<int>(new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20 }));
        }

        static void Gen(System.Text.StringBuilder sb, int length)
        {
            sb.Append("        public Objects(object p0");
            for (int j = 1; j < length; ++j)
                sb.Append(", object p" + j);
            sb.AppendLine(")");

            sb.AppendLine("        {");

            sb.AppendFormat("            objs = ObjectArrayPools.New({0});", length);
            sb.AppendLine();

            for (int i = 0; i < length; ++i)
            {
                sb.AppendFormat("            objs[{0}] = p{0};", i);
                sb.AppendLine();
            }

            sb.AppendLine("        }");
        }
    }
}
#endif