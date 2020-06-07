using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.AddressableAssets;

namespace UIModule
{
    public abstract class UIMgrBase
    {
        protected Transform _root;
        private UILayerMgr _layerMgr;
        private Stack<UIStack> _baseUiStack;
        private Dictionary<string, IView> _views;

        public virtual void Init(Transform canvas)
        {
            _root = canvas;
            _baseUiStack = new Stack<UIStack>();
            _views = new Dictionary<string, IView>();
            InitComponent(canvas);
        }

        protected virtual void InitComponent(Transform canvas)
        {
            _layerMgr = new UILayerMgr();
            _layerMgr.Init(canvas);
        }

        public async virtual void Show(string uiName,Action complete = null)
        {
            if (!_views.ContainsKey(uiName))
            {
                var go = await Addressables.InstantiateAsync(uiName, _root).Task;
                _views[uiName] = GetNewView(uiName,go);
                SetLayerParent(_views[uiName], _layerMgr);
            }

            IView view = _views[uiName];
            var layer = view.Self.GetComponent<ILayer>();
            
            if (layer.Layer == UILayer.BASE_UI)
            {
                //正在显示的base界面隐藏
                if(_baseUiStack.Count >0)
                    _baseUiStack.Peek().Hide();
                
                //显示当前要显示的界面
                UIStack group = new UIStack(uiName,view);
                _baseUiStack.Push(group);
            }
            else
            {
                _baseUiStack.Peek().Show(uiName,view);
            }

            if(complete != null)
            {
                complete();
            }
        }
        
        private void SetLayerParent(IView view,UILayerMgr layerMgr)
        {
            var layer = view.Self.GetComponent<ILayer>();
            view.Self.SetParent(layerMgr.GetUILayer(layer.Layer));
        }

        public virtual void Back(string curUiName)
        {
            if (_baseUiStack.Count > 0)
            {
                _baseUiStack.Pop().Back(curUiName);
                if(_baseUiStack.Count > 0)
                    _baseUiStack.Peek().Show();
            }
        }

        protected abstract IView GetNewView(string uiName, GameObject go);
    }
}
