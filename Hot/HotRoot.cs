using IL;
using wxb;
using UnityEngine;

[AutoInitAndRelease]
[ReplaceType(typeof(Root))]
public class HotRoot
{
    public static void Init()
    {
        UnityEngine.Debug.LogFormat("Hot Init by AutoInitAndRelease");
    }
}