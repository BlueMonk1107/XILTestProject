using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UIModule;
using Hot.Const;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Hot.Mgr
{
    public class UIMgr : UIMgrBase
    {
        private static UIMgr _uiMgr;

        public static UIMgr Single
        {
            get
            {
                if (_uiMgr == null)
                    _uiMgr = new UIMgr();

                return _uiMgr;
            }
        }

        public void Show(UIName name)
        {
            base.Show(name.ToString());
        }

        public void Back(UIName name)
        {
            base.Back(name.ToString());
        }

        protected override IView GetNewView(string uiName,GameObject go)
        {
            //自动添加view脚本
            AddComponent(uiName, go);
            //自动添加ctrl脚本
            string ctrlName = GetUiKey(uiName) + Const.Consts.CTRL_POSTFIX;
            AddComponent(ctrlName, go);
            AddUpdateListener(go);

            IView view = go.GetComponent<IView>();
            return view;
        }

        private bool AddComponent(string name, GameObject go)
        {
            Type viewType = Type.GetType(name);

            if (viewType == null)
            {
                Debug.LogError("未找到对应名称脚本，名称：" + name);
                return false;
            }
            else
            {
                go.AddComponent(viewType);
                return true;
            }
        }

        private void AddUpdateListener(GameObject viewGo)
        {
            var controller = viewGo.GetComponent<IController>();
            if (controller == null)
            {
                Debug.LogWarning("当前物体没有IController组件，物体名称:" + viewGo.name);
                return;
            }

            foreach (var update in viewGo.GetComponents<IUpdate>())
                controller.AddUpdateListener(update.UpdateFun);
        }

        private string GetUiKey(string uiName)
        {
            //获取去除view后的名字
            return uiName.Remove(uiName.Length - 4);
        }
    }
}