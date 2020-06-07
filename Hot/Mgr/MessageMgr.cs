using System;
using UnityEngine;

namespace Hot.Mgr
{
    public class MessageMgr : NormalSingleton<MessageMgr>, IMessageSystem
    {
        private readonly MessageSystem _msgSystem;

        public MessageMgr()
        {
            _msgSystem = new MessageSystem();
        }

        public void AddListener(int key, Action<object[]> callback)
        {
            _msgSystem.AddListener(key, callback);
        }

        public void RemoveListener(int key, Action<object[]> callback)
        {
            _msgSystem.RemoveListener(key, callback);
        }

        public void DispatchMsg(int key, params object[] args)
        {
            _msgSystem.DispatchMsg(key, args);
        }

        public void AddListener(string key, Action<object[]> callback)
        {
            _msgSystem.AddListener(key, callback);
        }

        public void RemoveListener(string key, Action<object[]> callback)
        {
            _msgSystem.RemoveListener(key, callback);
        }

        public void DispatchMsg(string key, params object[] args)
        {
            _msgSystem.DispatchMsg(key, args);
        }
    }
}