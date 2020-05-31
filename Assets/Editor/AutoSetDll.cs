using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class AutoSetDll :  AssetPostprocessor
{
//    [MenuItem("Tools/修改热更dll为txt")]
//    public static void ChangeDllName()
//    {
//        if (File.Exists(DLLPATH))
//        {
//            string targetPath = DLLPATH + ".txt";
//            if (File.Exists(targetPath))
//            {
//                File.Delete(targetPath);
//            }
//            File.Move(DLLPATH, targetPath);
//        }
//
//        if (File.Exists(PDBPATH))
//        {
//            string targetPath = PDBPATH + ".txt";
//            if (File.Exists(targetPath))
//            {
//                File.Delete(targetPath);
//            }
//            File.Move(PDBPATH, targetPath);
//        }
//        AssetDatabase.Refresh();
//    }

    static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets,
        string[] movedFromAssetPaths)
    {
        foreach (var importedAsset in importedAssets)
        {
            if (importedAsset.Contains("HotData") && !importedAsset.Contains(".bytes"))
            {
                string path = importedAsset + ".bytes";
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
                File.Move(importedAsset, path);
            }
        }
    }
}
