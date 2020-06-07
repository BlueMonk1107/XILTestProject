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
        var dll = await Addressables.LoadAssetAsync<TextAsset>(Consts.Paths.HOT_DLL_NAME).Task;
        hotMgr.Init(dll);
        Init(transform);
    }

    public void Init(Transform uiRoot)
    {
        
    }
    
    public void Update()
    {
        
    }
}
