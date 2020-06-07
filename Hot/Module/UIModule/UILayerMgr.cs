using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UIModule
{
    public enum UILayer
    {
        BASE_UI,
        OVERLAY_UI,
        TOP_UI,
        COUNT
    }

    /// <summary>
    /// UI自然层级管理器，自动生成每个UI层级的根物体
    /// </summary>
    public class UILayerMgr
    {
        private Dictionary<UILayer, RectTransform> _layerMaps;
        private Transform _canvas;

        public void Init(Transform canvas)
        {
            _canvas = canvas;
            _layerMaps = new Dictionary<UILayer, RectTransform>();
            SpawnLayerRoot();
        }

        private void SpawnLayerRoot()
        {
            for (UILayer i = 0; i < UILayer.COUNT; i++)
            {
                GameObject go = new GameObject(i.ToString());
                go.transform.SetParent(_canvas);
                _layerMaps[i] = go.AddComponent<RectTransform>();
                _layerMaps[i].anchoredPosition = Vector2.zero;
                _layerMaps[i].anchorMin = Vector2.zero;
                _layerMaps[i].anchorMax = Vector2.one;
                _layerMaps[i].offsetMin = Vector2.zero;
                _layerMaps[i].offsetMax = Vector2.zero;
            }
        }

        public RectTransform GetUILayer(UILayer layer)
        {
            if (_layerMaps.ContainsKey(layer))
            {
                return _layerMaps[layer];
            }
            else
            {
                Debug.LogError("当前层级未初始化");
                return null;
            }
        }
    }
}

