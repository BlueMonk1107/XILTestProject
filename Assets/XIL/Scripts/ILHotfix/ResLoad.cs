using UnityEngine;

namespace wxb
{
    using System.IO;

    public interface IResLoad
    {
        Stream GetStream(string path);
    }

#if UNITY_EDITOR
    // 编辑器下的资源加载
    class EditorResLoad : IResLoad
    {
        public EditorResLoad()
        {
        }

        Stream IResLoad.GetStream(string path)
        {
            string filepath = path;
            if (!File.Exists(filepath))
            {
                Debug.LogError("未找到对应文件，路径："+path);
                return null;
            }

            try
            {
                return new MemoryStream(File.ReadAllBytes(filepath));
            }
            catch (System.Exception ex)
            {
                wxb.L.LogException(ex);
            }

            return null;
        }
    }
#endif

    public static class ResLoad
    {
        static IResLoad current = null;

        public static void Set(IResLoad load)
        {
            current = load;
        }

        public static Stream GetStream(string path)
        {
            if (current == null)
            {
                Debug.LogError("当前Load对象为空，请先调用Set()");
                return null;
            }
            return current.GetStream(path);
        }
    }
}