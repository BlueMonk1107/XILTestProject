using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using wxb;

public class Root : MonoBehaviour
{
    // Start is called before the first frame update
    async void Start()
    {
        var dll = await Addressables.LoadAssetAsync<TextAsset>("DyncDll.dll").Task;
        hotMgr.Init(dll);
    }
}
