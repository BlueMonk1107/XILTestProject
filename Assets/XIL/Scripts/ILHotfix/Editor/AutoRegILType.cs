﻿using Consts;

namespace wxb
{
    using System.Reflection;
    using System.Collections.Generic;
//#if UNITY_EDITOR
//    public class EditorClass : System.Attribute
//    {

//    }
//    public class EditorField : System.Attribute
//    {

//    }

//#endif
    static partial class AutoRegILType
    {
        static Dictionary<System.Type, bool> HotTypes = new Dictionary<System.Type, bool>();

        // 是否热更当中的类型
        static bool IsHotType(System.Type type)
        {
            bool has = false;
            if (HotTypes.TryGetValue(type, out has))
                return has;

            if (type.Assembly.ManifestModule.Name == "DyncDll.dll")
            {
                HotTypes.Add(type, true);
                return true;
            }

            if (type.IsGenericType)
            {
                var types = type.GetGenericArguments();
                for (int i = 0; i < types.Length; ++i)
                {
                    bool v = IsHotType(types[i]);
                    if (v == true)
                    {
                        HotTypes.Add(type, true);
                        return true;
                    }
                }
            }

            string ns = type.FullName;
            if (string.IsNullOrEmpty(ns))
            {
                HotTypes.Add(type, false);
                return false;
            }
            if (ns.Contains("hot."))
            {
                HotTypes.Add(type, true);
                return true;
            }

            if (IsDelegate(type))
            {
                var method = type.GetMethod("Invoke");
                if (IsHotMethod(method))
                {
                    HotTypes.Add(type, true);
                    return true;
                }
            }

            HotTypes.Add(type, false);
            return false;
        }

        static Dictionary<System.Type, bool> EditorChecks = new Dictionary<System.Type, bool>();

        static bool IsEditorType(System.Type type)
        {
            bool v = false;
            if (EditorChecks.TryGetValue(type, out v))
                return v;

            v = IsEditorTypeImp(type);
            EditorChecks.Add(type, v);
            return v;
        }

        static bool IsEditorTypeImp(System.Type type)
        {
            if (type.IsNested)
            {
                if (IsEditorType(type.ReflectedType))
                    return true;
            }

            if (type.IsGenericType)
            {
                if (type.ContainsGenericParameters)
                    return true;

                foreach (var ator in type.GetGenericArguments())
                {
                    if (IsEditorType(ator))
                        return true;
                }
            }

            string ns = type.Namespace;
            if (!string.IsNullOrEmpty(ns) && (ns.StartsWith("GUIEditor.") || ns.StartsWith("UnityEditor.") || ns.StartsWith("MikuArtTech.EditorTools.")))
                return true;

            if (type.FullName.Contains("GUIEditor") || type.FullName.Contains("MikuArtTech.EditorTools."))
                return true;

            if (type.FullName.StartsWith("System.Runtime.Remoting"))
                return true;

            if (type.FullName == "System.CrossAppDomainDelegate")
                return true;
            if (type.FullName == "System.AppDomainInitializer")
                return true;

            if (type.GetCustomAttributes(typeof(EditorClass), true).Length != 0)
                return true;

            if (type.GetCustomAttributes(typeof(System.ObsoleteAttribute), true).Length != 0)
                return true;

            return false;
        }

        static bool IsTemplate(System.Type type)
        {
            return type.IsGenericType;
        }

        // 是否委托类型
        static bool IsDelegate(System.Type type)
        {
            var bt = type.BaseType;
            if (bt == typeof(System.MulticastDelegate) || bt == typeof(System.Delegate))
                return true;

            return false;
        }

        static bool IsPublic(System.Type type)
        {
            if (type.IsNested)
            {
                if (!type.IsNestedPublic)
                    return false;

                if (IsPublic(type.DeclaringType))
                    return true;

                return false;
            }

            return type.IsPublic;
        }

        static HashSet<System.Type> NeedExports = new HashSet<System.Type>(new System.Type[] 
        {

        });

        static HashSet<System.Type> NoExports = new HashSet<System.Type>(new System.Type[] 
        {
            typeof(UnityEngine.CanvasRenderer.OnRequestRebuild),
        });

        // 热更是否可能用到的类型
        static bool IsHotUsed(System.Type type)
        {
            if (type == typeof(void))
                return true;

            if (!IsPublic(type))
                return false;

            if (NeedExports.Contains(type))
                return true;

            if (NoExports.Contains(type))
                return false;

            string fullName = type.FullName;
            if (/*fullName.StartsWith("Config.") ||*/
                fullName.StartsWith("ParamList.") ||
                fullName.StartsWith("ParamList+") ||
                fullName.StartsWith("XLua.") ||
                fullName.StartsWith("ILRuntime.") ||
                fullName.StartsWith("Mono.") ||
                fullName.Contains("SharpNav.") ||
                fullName.Contains("FMOD") ||
                fullName.Contains("ParticlePlayground.") ||
                fullName.Contains("ICSharpCode.") ||
                fullName.Contains("xys.gm") ||
                fullName.StartsWith("Msdk") ||
                fullName.StartsWith("Candlelight") ||
                fullName.Contains("UnityEngine.WSA.") ||
                fullName.Contains("UnityEngineInternal.") ||
                fullName.Contains("UnityEngine.Material") ||
                //fullName.Contains("UnityEngine.Texture") ||
                //fullName.Contains("UnityEngine.Texture2D") ||
                fullName.Contains("UnityEngine.Shader") ||
                fullName.Contains("PackTool.SceneLoad") ||
                fullName.Contains("System.AsyncCallback") ||
                fullName.Contains("ModelPart") ||
                fullName.Contains("xys.IModleFactory") ||
                fullName.Contains("UnityEngine.AndroidJavaRunnable.") ||
                fullName.Contains("UnityEngine.VR.") ||
                fullName.Contains("UnityEngine.Audio") ||
                fullName.Contains("UnityEngine.SceneManagement") ||
                //fullName.Contains("UnityEngine.Video.") ||
                //fullName.Contains("UnityEngine.ParticleSystem") ||
                fullName.Contains("UnityEngine.Windows.") ||
                fullName.Contains("UnityEngine.Android") ||
                fullName.Contains("UnityEngine.Networking") ||
                fullName.Contains("Eyesblack") ||
                fullName.Contains("TinyXML") ||
                //fullName.Contains("UnityEngine.Gradient") ||
                //fullName.Contains("UnityEngine.AnimationCurve") ||
                fullName.Contains("Xft.") ||
                fullName.Contains("UI.Point") ||
                fullName.Contains("MeshSimplify") ||
                fullName.Contains("AutomaticLOD") ||
                fullName.Contains("EditorExtensions") ||
                //fullName.Contains("UnityEngine.CanvasRenderer") ||
                fullName.Contains("AnimateTiledTexture") ||
                //fullName.Contains("UnityEngine.Canvas") ||
                //fullName.Contains("UnityEngine.Font") ||
                fullName.Contains("UnityEngine.AI") ||
                //fullName.Contains("UnityEngine.RectTransform") ||
                fullName.Contains("UnityEngine.Apple.") ||
                fullName.Contains("UnityEngine.Display") ||
                fullName.Contains("PackTool.DTATask") ||
                fullName.Contains("behaviac") ||
                fullName.Contains("PackTool.DTATask.") ||
                //fullName.Contains("UnityEngine.Application") ||
                fullName.Contains("GUITextShow") ||
                fullName.Contains("Pathfinding") ||
                fullName.Contains("WellFired.") ||
                //fullName.Contains("UnityEngine.Camera") ||
                fullName.Contains("UltimateGameTools") ||
                //fullName.Contains("XTools.Utility") ||
                fullName.Contains("UnityEngine.CullingGroup") ||
                fullName.Contains("UltimateGameTools.") ||
                fullName.Contains("UnityEngine.GUI") ||
                fullName.Contains("System.Reflection") ||
                //fullName.Contains("System.Type") ||
                fullName.Contains("UnityEngine.iOS") ||
                fullName.Contains("UnityEngine.SocialPlatforms") ||
                //fullName.Contains("WXB.Draw") ||
                //fullName.Contains("WXB.RenderCache") ||
                //fullName.Contains("WXB.Line") ||
                //fullName.Contains("UnityEngine.AnimationClip") ||
                //fullName.Contains("UnityEngine.Animator") ||
                //fullName.Contains("UnityEngine.UICharInfo") ||
                //fullName.Contains("UnityEngine.UILineInfo") ||
                //fullName.Contains("UnityEngine.UIVertex") ||
                //fullName.Contains("UnityEngine.UI.RectMask2D") ||
                //fullName.Contains("UnityEngine.BoneWeight") ||
                //fullName.Contains("UnityEngine.Rendering") ||
                //fullName.Contains("EffectDelayPlayItem") ||
                //fullName.Contains("RoleSkinUnitData") ||
                //fullName.Contains("RoleFaceBaseSet") ||
                //fullName.Contains("RoleShapePartUnit") ||
                //fullName.Contains("RoleShapeSubPart") ||
                //fullName.Contains("RoleShapePart") ||
                //fullName.Contains("RoleShapeUnitData") ||
                //fullName.Contains("RoleSkinTexStyle") ||
                //fullName.Contains("RoleSkinColorStyle") ||
                //fullName.Contains("RoleSkinUnit") ||
                //fullName.Contains("RoleDisguiseItem") ||
                //fullName.Contains("RoleDisguiseType") ||
                //fullName.Contains("xys.ClothHandle") ||
                //fullName.Contains("xys.ClothConfig") ||
                //fullName.Contains("xys.HairItem") ||
                //fullName.Contains("xys.ClothItem") ||
                //fullName.Contains("xys.WeaponItem") ||
                //fullName.Contains("xys.RideItem") ||
                fullName.Contains("Tentacle3D") ||
                fullName.Contains("RoleDisguiseCareer") ||
                //fullName.Contains("UnityEngine.Rect") ||
                //fullName.Contains("UnityEngine.Renderer") ||
                fullName.Contains("Combine") ||
                //fullName.Contains("UnityEngine.LineRenderer") ||
                fullName.Contains("BendingSegment") ||
                fullName.Contains("PackTool.MagicThreadParamUpdate") ||
                fullName.Contains("UIWidgetsSamples."))
            {
                return false;
            }

            return true;
        }

        static List<string> checkAssemblies = new List<string>(new string[]
        {
            "UnityEngine.UI,",
            "UnityEngine,",
            "Assembly-CSharp,",
            "Assembly-CSharp-firstpass,"
        });

        static bool IsAssemblie(Assembly assembly)
        {
            foreach (string key in checkAssemblies)
            {
                if (assembly.FullName.StartsWith(key))
                {
                    return true;
                }
            }

            return false;
        }

        [UnityEditor.MenuItem("XIL/委托自动生成")]
        public static void Build()
        {
            HashSet<System.Type> types = new HashSet<System.Type>(); 
            var assemblies = System.AppDomain.CurrentDomain.GetAssemblies();
#if USE_HOT
            string dll = Paths.HOT_DLL_PATH;
            if (System.IO.File.Exists(dll))
            {
                Assembly assembly = Assembly.LoadFile(dll);
                types.UnionWith(assembly.GetTypes());
            }
#endif
            foreach (var assemblie in assemblies)
            {
                if (!IsAssemblie(assemblie))
                    continue;

                types.UnionWith(assemblie.GetTypes());
            }

            Export(types);
        }

        [UnityEditor.MenuItem("XIL/清除委托自动生成脚本")]
        public static void Clear()
        {
            BuildDelegate(new Dictionary<System.Type, UseTypeInfo>(), new Dictionary<System.Type, UseTypeInfo>());
        }

        static void Export(HashSet<System.Type> types)
        {
            HashSet<System.Type> hotTypes = new HashSet<System.Type>(); // 热更新所有类型
            Dictionary<System.Type, UseTypeInfo> csharpDelegate = new Dictionary<System.Type, UseTypeInfo>(); // C#层当中的的所有委托类型
            HashSet<System.Type> Checks = new HashSet<System.Type>();
            foreach (var type in types)
            {
                BuildType(type, hotTypes, csharpDelegate, Checks);
            }

            var hotTDic = new Dictionary<System.Type, UseTypeInfo>();
            Checks.Clear();
            foreach (var d in hotTypes)
            {
                CheckHotType(d, hotTDic, Checks);
            }

            // 先把C#层的委托注册到管理器当中
            wxb.L.LogFormat("csharpDelegate Count:{0} hotTDic:{1}", csharpDelegate.Count, hotTDic.Count);
            BuildDelegate(csharpDelegate, hotTDic);
        }

        //[UnityEditor.MenuItem("XIL/TestBuildCode")]
        static void TestBuildCode()
        {
            var tests = new HashSet<System.Type>(new System.Type[]
            {
                //typeof(System.Func<wProtobuf.RPC.Error, NetProto.Hot.ActivityRemindRespone>),
                //typeof(ParamList),
                //typeof(MikuArtTech.EditorTools.ColorSchemeAsset),
                //typeof(List<MikuArtTech.EditorTools.ColorSchemeAsset.ColorScheme>),
                //typeof(System.Action<wProtobuf.RPC.Error, NetProto.Hot.DemonplotRespone>),
                //IL.Help.GetTypeByFullName("hot.ChatUtil"),
                //IL.Help.GetTypeByFullName("xys.ClothConfig"),                
                //IL.Help.GetTypeByFullName("hot.UI.HotObtainItemShowMgr"),
                //IL.Help.GetTypeByFullName("hot.UI.HotObtainItemShowMgr"),
                //typeof(AssetRequest),
            });

            Export(tests);
        }

        // 类型的使用情况
        class UseTypeInfo
        {
            public HashSet<MemberInfo> infos = new HashSet<MemberInfo>();

            public void Add(MemberInfo info)
            {
                infos.Add(info);
            }

            public void To(System.Text.StringBuilder sb, string suffix)
            {
                foreach (var info in infos)
                {
                    sb.Append(suffix);
                    sb.Append("// ");
                    if (info is MethodInfo)
                    {
                        var m = (MethodInfo)info;
                        var ps = m.GetParameters();
                        for (int i = 0; i < ps.Length; ++i)
                        {
                            if (ps[i].IsOut || ps[i].ParameterType.IsByRef)
                                continue;
                        }

                        sb.Append(string.Format("{0} {1}.{2}(", GetClassRealClsName(m.ReturnType), GetClassRealClsName(m.ReflectedType), m.Name));
                        if (ps.Length == 0)
                            sb.Append(");");
                        else
                        {
                            sb.Append(GetClassRealClsName(ps[0].ParameterType));
                            for (int i = 1; i < ps.Length; ++i)
                                sb.AppendFormat(", {0}", GetClassRealClsName(ps[i].ParameterType));
                            sb.Append(");");
                        }
                        sb.AppendLine();
                    }
                    else if (info is FieldInfo)
                    {
                        var f = (FieldInfo)info;
                        sb.AppendFormat("{0} {1}.{2}", GetClassRealClsName(f.FieldType), GetClassRealClsName(info.DeclaringType), f.Name);
                        sb.AppendLine();
                    }
                    else if (info is PropertyInfo)
                    {
                        var p = (PropertyInfo)info;
                        sb.AppendFormat("{0} {1}.{2}", GetClassRealClsName(p.PropertyType), GetClassRealClsName(p.PropertyType), p.Name);
                        sb.AppendLine();
                    }
                    else if (info is ConstructorInfo)
                    {
                        var ctor = info as ConstructorInfo;
                        var ps = ctor.GetParameters();
                        for (int i = 0; i < ps.Length; ++i)
                        {
                            if (ps[i].IsOut || ps[i].ParameterType.IsByRef)
                                continue;
                        }

                        sb.Append(string.Format("{0}.{0}(", GetClassRealClsName(ctor.ReflectedType)));
                        if (ps.Length == 0)
                            sb.Append(");");
                        else
                        {
                            sb.Append(GetClassRealClsName(ps[0].ParameterType));
                            for (int i = 1; i < ps.Length; ++i)
                                sb.AppendFormat(", {0}", GetClassRealClsName(ps[i].ParameterType));
                            sb.Append(");");
                        }
                        sb.AppendLine();
                    }
                    else
                    {
                        wxb.L.LogErrorFormat("info:{0}", info.GetType().FullName);
                    }
                }
            }
        }

        static bool IsSystemDelegate(System.Type type)
        {
            if (type.FullName.StartsWith("System.Func`") || type.FullName.StartsWith("System.Action`"))
                return true;
            return false;
        }

        static bool IsExportType(System.Type type)
        {
            if (IsEditorType(type) || !IsHotUsed(type))
                return false; // 不需要导出的类型

            return true;
        }

        static bool Add(Dictionary<System.Type, UseTypeInfo> csharpDelegate, System.Type type, MemberInfo member)
        {
            UseTypeInfo info = null;
            if (!csharpDelegate.TryGetValue(type, out info))
            {
                if (IsDelegate(type))
                {
                    info = new UseTypeInfo();
                    if (member != null)
                        info.Add(member);
                    csharpDelegate.Add(type, info);
                    return true;
                }

                return false;
            }
            else
            {
                if (member != null)
                    info.Add(member);

                return true;
            }
        }

        static List<System.Type> GetMethodBodyType(MethodInfo method)
        {
            List<System.Type> types = new List<System.Type>();
            if (method.DeclaringType.IsArray)
                return types;

            var body = method.GetMethodBody();
            if (body != null && body.LocalVariables != null)
            {
                foreach (var t in body.LocalVariables)
                    types.Add(t.LocalType);
            }

            return types;
        }

        // 是否为模块类，并没有实例化出来
        static bool IsTemplateAndNotInstance(System.Type type)
        {
            if (type.IsGenericType)
            {
                if (type.ContainsGenericParameters)
                    return true;
            }

            return false;
        }

        static bool IsCheck(System.Type type, HashSet<System.Type> Checks)
        {
            if (type == null)
                return false;

            if (Checks.Contains(type))
                return false;

            Checks.Add(type);
            if (string.IsNullOrEmpty(type.FullName))
                return false;

            if (IsTemplateAndNotInstance(type))
                return false;

            return true;
        }

        static void BuildType(System.Type type, HashSet<System.Type> hotTypes, Dictionary<System.Type, UseTypeInfo> csharpDelegate, HashSet<System.Type> Checks)
        {
            if (!IsCheck(type, Checks))
                return;

            if (csharpDelegate.ContainsKey(type))
                return;

            if (IsHotType(type))
            {
                hotTypes.Add(type); // 热更当中的类型
                BindingFlags flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.Static | BindingFlags.NonPublic;
                //// C#层当中的类型，遍历下所有的方法，查看下，方法的参数是否有委托类型的
                foreach (var method in type.GetMethods(flags))
                {
                    BuildType(method.ReturnType, hotTypes, csharpDelegate, Checks);
                    foreach (var p in method.GetParameters())
                        BuildType(p.ParameterType, hotTypes, csharpDelegate, Checks);

                    //foreach (var t in GetMethodBodyType(method))
                    //    BuildType(t, hotTypes, csharpDelegate, Checks);
                }

                foreach (var field in type.GetFields(flags))
                {
                    BuildType(field.FieldType, hotTypes, csharpDelegate, Checks);
                }

                foreach (var p in type.GetProperties(flags))
                {
                    BuildType(p.PropertyType, hotTypes, csharpDelegate, Checks);
                }

                return;
            }

            if (!IsExportType(type))
                return;

            if (IsDelegate(type))
            {
                if (!csharpDelegate.ContainsKey(type))
                {
                    if (!IsExportType(type))
                        return;

                    csharpDelegate.Add(type, new UseTypeInfo()); // C#层的委托
                }
            }
            else
            {
                if (!IsPublic(type) || !type.IsClass)
                    return;
                CheckMethods(type, hotTypes, csharpDelegate, Checks);
            }
        }

        static void CheckMethods(System.Type type, HashSet<System.Type> hotTypes, Dictionary<System.Type, UseTypeInfo> csharpDelegate, HashSet<System.Type> Checks)
        {
            //BindingFlags flags = BindingFlags.Default;
            // C#层当中的类型，遍历下所有的方法，查看下，方法的参数是否有委托类型的
            foreach (var ctor in type.GetConstructors())
            {
                if (ctor.ContainsGenericParameters)
                    continue;

                bool isBreak = false;
                foreach (var p in ctor.GetParameters())
                {
                    if (!IsExportType(p.ParameterType))
                    {
                        isBreak = true;
                        break;
                    }
                }

                if (isBreak)
                    continue;

                foreach (var p in ctor.GetParameters())
                {
                    if (!Add(csharpDelegate, p.ParameterType, ctor))
                    {
                        BuildType(p.ParameterType, hotTypes, csharpDelegate, Checks);
                    }
                }
            }

            HashSet<System.Type> types = new HashSet<System.Type>();
            foreach (var method in type.GetMethods(/*flags*/))
            {
                if (method.ContainsGenericParameters)
                    continue;

                if (!IsExportType(method.ReturnType))
                    continue;

                bool isBreak = false;
                foreach (var p in method.GetParameters())
                {
                    if (!IsExportType(p.ParameterType))
                    {
                        isBreak = true;
                        break;
                    }
                }

                if (isBreak)
                    continue;

                if (method.ReturnType != typeof(void))
                {
                    if (!Add(csharpDelegate, method.ReturnType, method))
                    {
                        types.Add(method.ReturnType);
                        //BuildType(method.ReturnType, hotTypes, csharpDelegate, Checks);
                    }
                }

                foreach (var p in method.GetParameters())
                {
                    if (!Add(csharpDelegate, p.ParameterType, method))
                    {
                        types.Add(p.ParameterType);
                        //BuildType(p.ParameterType, hotTypes, csharpDelegate, Checks);
                    }
                }
            }

            foreach (var field in type.GetFields(/*flags*/))
            {
                types.Add(field.FieldType);
                //BuildType(field.FieldType, hotTypes, csharpDelegate, Checks);
            }

            foreach (var p in type.GetProperties(/*flags*/))
            {
                types.Add(p.PropertyType);
                //BuildType(p.PropertyType, hotTypes, csharpDelegate, Checks);
            }

            foreach (var ator in types)
            {
                BuildType(ator, hotTypes, csharpDelegate, Checks);
            }
        }

        static bool IsHotMethod(MethodInfo method)
        {
            if (method == null)
                return false;

            if (IsHotType(method.ReturnType))
                return true;

            foreach (var p in method.GetParameters())
                if (IsHotType(p.ParameterType))
                    return true;

            return false;
        }

        struct DelegateKey
        {
            public DelegateKey(MethodInfo method)
            {
                returnType = method.ReturnType;
                parameters = new List<System.Type>();
                foreach (var p in method.GetParameters())
                    parameters.Add(p.ParameterType);

                cache_reg = 0;
            }

            public System.Type returnType;
            public List<System.Type> parameters;

            int cache_reg;

            public bool isReg
            {
                get
                {
                    switch (cache_reg)
                    {
                    case 0:
                        {
                            cache_reg = IsReg() ? 1 : 2;
                        }
                        break;
                    }

                    return cache_reg == 1 ? true : false;
                }
            }

            bool IsReg()
            {
                bool isVoid = returnType.Name == "Void";
                if (isVoid && parameters.Count == 0)
                    return false;
                if (parameters.Count >= 5)
                    return false;

                if (!IsHotUsed(returnType))
                    return false;
                foreach (var t in parameters)
                {
                    if (t.IsByRef)
                        return false;

                    if (!IsHotUsed(t))
                        return false;
                }

                return true;
            }

            public override bool Equals(object obj)
            {
                var dk = (DelegateKey)obj;
                if (dk.returnType != returnType)
                    return false;
                if (dk.parameters.Count != parameters.Count)
                    return false;

                for (int i = 0; i < parameters.Count; ++i)
                {
                    if (parameters[i] != dk.parameters[i])
                        return false;
                }

                return true;
            }

            public override int GetHashCode()
            {
                var hash = returnType.GetHashCode() ^ parameters.Count;
                for (int i = 0; i < parameters.Count; ++i)
                    hash ^= parameters[i].GetHashCode();
                return hash;
            }
        }

        const string suffix = "            ";
        
        class SB
        {
            public System.Text.StringBuilder RegisterFunctionDelegate = new System.Text.StringBuilder();
            public System.Text.StringBuilder RegisterDelegateConvertor = new System.Text.StringBuilder();
            public System.Text.StringBuilder RegisterMethodDelegate = new System.Text.StringBuilder();
        }

        public static void GetPlatform(out string marc, out string suffix)
        {
#if UNITY_IOS
            marc = "UNITY_IOS";
            suffix = "ios";
#elif UNITY_ANDROID
            marc = "UNITY_ANDROID";
            suffix = "ad";
#elif UNITY_WEBGL
            marc = "UNITY_WEBGL";
            suffix = "webgl";
#elif UNITY_STANDALONE_OSX
            marc = "UNITY_STANDALONE_OSX";
            suffix = "mac";
#elif UNITY_STANDALONE_LINUX
            marc = "UNITY_STANDALONE_LINUX";
            suffix = "linux";
#else
            marc = "UNITY_STANDALONE_WIN";
            suffix = "pc";
#endif
        }

        static void BuildDelegate(Dictionary<System.Type, UseTypeInfo> csharpDelegate, Dictionary<System.Type, UseTypeInfo> hotTDic)
        {
            string marco, file_suffix;
            GetPlatform(out marco, out file_suffix);
            string file_path = string.Format("Assets/XIL/Auto/ILRegType_{0}.cs", file_suffix);

            Dictionary<DelegateKey, List<System.Type>> keys = new Dictionary<DelegateKey, List<System.Type>>();

            foreach (var ator in csharpDelegate)
            {
                var d = ator.Key;
                var method = d.GetMethod("Invoke");
                if (method == null)
                {
                    wxb.L.LogErrorFormat("method:{0}", d.FullName);
                    continue;
                }

                var dk = new DelegateKey(method);
                List<System.Type> ds = null;
                if (!keys.TryGetValue(dk, out ds))
                {
                    ds = new List<System.Type>();
                    keys.Add(dk, ds);
                }

                ds.Add(d);
            }

            SB sb = new SB();
            //System.Text.StringBuilder RegisterFunctionDelegate = new System.Text.StringBuilder();
            //System.Text.StringBuilder RegisterDelegateConvertor = new System.Text.StringBuilder();
            //System.Text.StringBuilder RegisterMethodDelegate = new System.Text.StringBuilder();
            foreach (var ator in keys)
            {
                if (ator.Key.isReg)
                {
                    //sb.Append(suffix);
                    DelegateAdapter(ator.Key, sb, suffix, (ssb)=> 
                    {
                        //foreach (var v in ator.Value)
                        //{
                        //    ssb.Append(suffix);
                        //    ssb.Append("// ");
                        //    ssb.AppendLine(GetClassRealClsName(v));

                        //    UseTypeInfo info = null;
                        //    if (csharpDelegate.TryGetValue(v, out info))
                        //    {
                        //        if (info.infos.Count == 0)
                        //            continue;

                        //        info.To(ssb, suffix);
                        //    }
                        //}

                    });
                    //sb.AppendLine();
                }

                foreach (var v in ator.Value)
                {
                    if (IsSystemDelegate(v))
                        continue;

                    ConvertToDelegate(v, sb.RegisterDelegateConvertor, suffix);
                    sb.RegisterDelegateConvertor.AppendLine();
                }

                //sb.AppendLine();
            }

            System.Text.StringBuilder tsb = new System.Text.StringBuilder();
            BuildDHot(hotTDic, tsb);

            System.IO.Directory.CreateDirectory(file_path.Substring(0, file_path.LastIndexOf('/')));
            System.IO.File.WriteAllText(file_path, string.Format(RegTextFile,
                marco,
                sb.RegisterFunctionDelegate, 
                sb.RegisterDelegateConvertor, 
                sb.RegisterMethodDelegate, tsb.ToString()), System.Text.Encoding.UTF8);

            UnityEditor.AssetDatabase.ImportAsset(file_path);
            UnityEditor.AssetDatabase.Refresh();
        }

        static string GetTypeByHot(System.Type type)
        {
            if (IsHotType(type))
                return "ILRuntime.Runtime.Intepreter.ILTypeInstance";
            return GetClassRealClsName(type);
        }

        static void BuildDHot(Dictionary<System.Type, UseTypeInfo> hotTDic, System.Text.StringBuilder sb)
        {
            sb.Append(suffix);
            sb.AppendLine("System.Type v = null;");
            foreach (var d in hotTDic)
            {
                //d.Value.To(sb, suffix);

                sb.Append(suffix);

                if (d.Key.FullName.StartsWith("System.Collections.Generic.Dictionary`2[["))
                {
                    // 为Map类型
                    var parameter = d.Key.GetGenericArguments();
                    sb.AppendLine(string.Format("v = typeof(System.Collections.Generic.Dictionary<{0},{1}>);", GetTypeByHot(parameter[0]), GetTypeByHot(parameter[1])));
                }
                else
                {
                    sb.AppendLine(string.Format("v = typeof({0});", GetClassRealClsName(d.Key)));
                }

                sb.AppendLine();
            }
        }
    }
}