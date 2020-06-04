using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using wxb.Editor;

public class AutoSetProject : MonoBehaviour
{
    [InitializeOnLoadMethod]
    public static void AutoRunAfterComplier()
    {
        Hotfix.Inject();
    }
}
