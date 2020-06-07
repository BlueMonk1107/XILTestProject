using System;
using System.Collections.Generic;
using UnityEngine;

namespace LifeCycle
{
    public abstract class LifeCycleMgrBase<T> where T : class, new()
    {
        private static T _single;

        public static T Single
        {
            get
            {
                if (_single == null)
                {
                    _single = new T();
                }
                return _single;
            }
        }
        private bool _initComplete = false;

        public void Init()
        {
            var config = GetConfig();
            config.Init();
            InitObject(config);

            LifeCycleConfig.LifeCycleFuns[LifeName.INIT]();

            _initComplete = true;
        }

        protected abstract LifeCycleAddConfigBase GetConfig();

        private void InitObject(LifeCycleAddConfigBase config)
        {
            foreach (var o in config.Objects)
            foreach (var cycle in LifeCycleConfig.LifeCycles)
                if (cycle.Value.Add(o))
                    break;
        }

        public void Add(LifeName name, object o)
        {
            LifeCycleConfig.LifeCycles[name].Add(o);

            if(name == LifeName.INIT && _initComplete)
            {
                if(o is IInit)
                {
                    (o as IInit).Init();
                }
            }
        }

        public void Remove(LifeName name, object o)
        {
            LifeCycleConfig.LifeCycles[name].Remove(o);
        }

        public void RemoveAll(object o)
        {
            foreach (var cycle in LifeCycleConfig.LifeCycles) cycle.Value.Remove(o);
        }

        public void Update()
        {
            LifeCycleConfig.LifeCycleFuns[LifeName.UPDATE]();
        }
    }

    public interface ILifeCycle
    {
        bool Add(object o);
        void Remove(object o);
        void Execute<T>(Action<T> execute);
    }

    public class LifeCycle<T> : ILifeCycle
    {
        private readonly List<object> _objects = new List<object>();

        public bool Add(object o)
        {
            if (o is T)
            {
                if (_objects.Contains(o))
                {
                    return false;
                }
                else
                {
                    _objects.Add(o);
                    return true;
                }
            }

            return false;
        }

        public void Remove(object o)
        {
            _objects.Remove(o);
        }

        public void Execute<T1>(Action<T1> execute)
        {
            for (int i = 0; i < _objects.Count; i++)
            {
                execute((T1) _objects[i]);
            }
        }
    }
}