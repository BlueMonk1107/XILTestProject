using IL;
using wxb;
using UnityEngine;
using Hot.Mgr;


[ReplaceType(typeof(Root))]
public class HotRoot
{
    public static Transform UiRoot;
    [ReplaceFunction]
    public static void Init(Root root,Transform uiRoot)
    {
        UnityEngine.Debug.LogFormat("Execute Hot Init");
        if (uiRoot != null)
            return;
        UiRoot = uiRoot;
        Object.DontDestroyOnLoad(uiRoot);
        LifeCycleMgr.Single.Init();
    }

    [ReplaceFunction]
    public static void Update(Root root)
    {
        LifeCycleMgr.Single.Update();
    }
}