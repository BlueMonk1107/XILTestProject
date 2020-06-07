using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UIModule
{
    public abstract class ControllerBase : IController
    {
        protected Transform _transform;
        public virtual void Init(Transform self)
        {
            _transform = self;
            InitChild();
        }
        
        protected abstract void InitChild();

        public virtual void Show()
        {
        }

        public virtual void Hide()
        {
        }

        public virtual void UpdateFun()
        {
        }

        public virtual void AddUpdateListener(Action update)
        {
        }

        public virtual void Reacquire()
        {
        }
    }
    
    public abstract class RootControllerBase : ControllerBase
    {
        private List<IControllerHide> _hides;
        private List<IControllerInit> _inits;
        private Action _onUpdate;
        private List<IControllerShow> _shows;
        private List<IControllerUpdate> _updates;

        public override void Init(Transform self)
        {
            base.Init(self);
            InitAllComponents();
            InitComponents();
            AddUpdateAction();
        }

        public override void Reacquire()
        {
            base.Reacquire();
            InitInterface();
            InitComponents();
        }

        public override void Show()
        {
            base.Show();
            foreach (var component in _shows) 
                component.Show();
        }

        public override void Hide()
        {
            base.Hide();
            foreach (var component in _hides) 
                component.Hide();
        }

        public override void UpdateFun()
        {
            base.UpdateFun();
            foreach (var component in _updates)
                component.UpdateFun();
        }

        public override void AddUpdateListener(Action update)
        {
            base.AddUpdateListener(update);
            _onUpdate += update;
        }

       

        private void AddUpdateAction()
        {
            foreach (var button in _transform.GetComponentsInChildren<Button>())
                button.onClick.AddListener(() =>
                {
                    if (_onUpdate != null)
                        _onUpdate();

                    UpdateFun();
                });
            
            foreach (var input in _transform.GetComponentsInChildren<InputField>())
                input.onEndEdit.AddListener((content) =>
                {
                    if (_onUpdate != null)
                        _onUpdate();

                    UpdateFun();
                });
        }

        private void InitAllComponents()
        {
            _inits = new List<IControllerInit>();
            _shows = new List<IControllerShow>();
            _hides = new List<IControllerHide>();
            _updates = new List<IControllerUpdate>();

            InitInterface();
        }
        
        private void InitInterface()
        {
            InitComponent(_inits, this);
            InitComponent(_shows, this);
            InitComponent(_hides, this);
            InitComponent(_updates, this);
        }

        private void InitComponent<T>(List<T> components, T removeObject)
        {
            var temp = _transform.GetComponentsInChildren<T>(true);

            components.AddRange(temp);

            components.Remove(removeObject);
        }

        private void InitComponents()
        {
            foreach (var component in _inits)
            {
                component.Init(_transform);
            }
        }
    }
    
    public abstract class SubControllerBase : ControllerBase
    {
    }
}