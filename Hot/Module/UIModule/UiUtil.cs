using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UIModule
{
    public class UiUtil
    {
        private Dictionary<string, UiUtilData> _datas;
        private Transform _root;

        public void Init(Transform root)
        {
            _root = root;
            _datas = new Dictionary<string, UiUtilData>();
            var rect = root.GetComponent<RectTransform>();
            foreach (RectTransform rectTransform in rect) 
                _datas.Add(rectTransform.name, new UiUtilData(rectTransform));
        }

        public UiUtilData Self()
        {
            if (_datas == null)
            {
                Debug.LogError("当前UiUtil未初始化，请先调用Init");
            }
            
            if (_datas.ContainsKey(_root.name)) 
                return _datas[_root.name];
            
            _datas.Add(_root.name,new UiUtilData(_root.GetComponent<RectTransform>()));
            return _datas[_root.name];
        }

        public UiUtilData Get(string name)
        {
            if(_datas == null)
            {
                Debug.LogError("当前UiUtil未初始化，请先调用Init");
            }

            if (_datas.ContainsKey(name)) 
                return _datas[name];

            var temp = _root.Find(name);
            if (temp == null)
            {
                Debug.LogError("无法按照路径查找到物体，路径为：" + name);
                return null;
            }

            _datas.Add(name, new UiUtilData(temp.GetComponent<RectTransform>()));
            return _datas[name];
        }
    }

    public class UiUtilData
    {
        public UiUtilData(RectTransform rectTrans)
        {
            RectTrans = rectTrans;
            Go = rectTrans.gameObject;
            Img = RectTrans.GetComponent<Image>();
            Text = rectTrans.GetComponent<Text>();
        }

        public GameObject Go { get; }
        public RectTransform RectTrans { get; }
        public Image Img { get; }
        public Text Text { get; }

        public void SetSprite(Sprite sprite)
        {
            if (Img != null)
                Img.sprite = sprite;
            else
                Debug.LogError("当前物体上没有image组件，物体名称为" + Go.name);
        }

        public void SetText(int content)
        {
            SetText(content.ToString());
        }

        public void SetText(float content)
        {
            SetText(content.ToString());
        }

        public void SetText(string content)
        {
            if (Text != null)
                Text.text = content;
            else
                Debug.LogError("当前物体上没有Text组件，物体名称为" + Go.name);
        }

        public T Get<T>() where T : Component
        {
            if (Go != null) return Go.GetComponent<T>();

            Debug.LogError("当前gameobject为空");
            return null;
        }

        public T Add<T>() where T : Component
        {
            if (Go != null)
            {
                return Go.AddComponent<T>();
            }

            Debug.LogError("当前gameobject为空");
            return null;
        }
    }
}