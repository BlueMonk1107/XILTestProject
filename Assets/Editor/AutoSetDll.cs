using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using Consts;

public class AutoSetDll :  AssetPostprocessor
{
    static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets,
        string[] movedFromAssetPaths)
    {
        foreach (var importedAsset in importedAssets)
        {
            if (importedAsset.Contains(Paths.HOT_DATA_DIC_NAME) && !importedAsset.Contains(Consts.Paths.HOT_DATA_POSTFIX))
            {
                string path = importedAsset + Consts.Paths.HOT_DATA_POSTFIX;
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
                File.Move(importedAsset, path);
            }
        }
    }
}
