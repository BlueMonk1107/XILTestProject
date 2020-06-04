using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Consts
{
    public class Paths
    {
        /// <summary>
        /// 自动给热更DLL添加的后缀
        /// </summary>
        public static readonly string HOT_DATA_POSTFIX = ".bytes";
        /// <summary>
        /// 热更dll名称
        /// </summary>
        public static readonly string HOT_DLL_NAME = "DyncDll.dll";
        /// <summary>
        /// 热更PDB名称
        /// </summary>
        public static readonly string HOT_PDB_NAME = "DyncDll.pdb";
        /// <summary>
        /// 热更数据工程下路径
        /// </summary>
        public static readonly string HOT_DATA_DIC = $"{Application.dataPath}/{HOT_DATA_DIC_NAME}/";
        /// <summary>
        /// 热更数据文件夹名称
        /// </summary>
        public const string HOT_DATA_DIC_NAME = "HotData";
        /// <summary>
        /// 热更dll路径
        /// </summary>
        public static readonly string HOT_DLL_PATH = HOT_DATA_DIC+HOT_DLL_NAME+HOT_DATA_POSTFIX;
        /// <summary>
        /// 热更PDB路径
        /// </summary>
        public static readonly string HOT_PDB_PATH = HOT_DATA_DIC+HOT_PDB_NAME+HOT_DATA_POSTFIX;
    }
}

