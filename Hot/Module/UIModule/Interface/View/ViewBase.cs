using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UIModule
{
    public abstract  class ViewBase : IView
    {
        private UiUtil _util;
        private RectTransform _self;
        protected Transform _transform;

        public RectTransform Self
        {
            get
            {
                if (_self == null)
                    _self = _transform.GetComponent<RectTransform>();

                return _self;
            }
        }

        public virtual void Reacquire()
        {
        }
        
        protected UiUtil Util
        {
            get
            {
                if (_util == null)
                {
                    _util = new UiUtil();
                    _util.Init(_transform);
                }

                return _util;
            }
        }

        public virtual void Init(Transform self)
        {
            _transform = self;
            InitChild();
        }

        public virtual void Show()
        {
            _transform.gameObject.SetActive(true);
            UpdateFun();
        }

        public virtual void Hide()
        {
            _transform.gameObject.SetActive(false);
        }

        public virtual void UpdateFun()
        {
        }
        
        protected abstract void InitChild();
    }

    public abstract class RootViewBase : ViewBase,ILayer
    {
        public abstract UILayer Layer { get; }
        
        private List<IViewHide> _viewHides;
        private List<IViewInit> _viewInits;
        private List<IViewShow> _viewShows;
        private List<IViewUpdate> _viewUpdates;
        
        public override void Init(Transform self)
        {
            base.Init(self);
            InitSubView();
        }

        public override void Show()
        {
            foreach (var view in _viewShows)
            {
                view.Show();
            }
            
            base.Show();
        }

        public override void Hide()
        {
            base.Hide();
            foreach (var view in _viewHides)
                view.Hide();
        }

        public override void UpdateFun()
        {
            base.UpdateFun();
            foreach (var update in _viewUpdates) 
                update.UpdateFun();
        }

        public override void Reacquire()
        {
            InitInterface();
            InitAllSubView();
        }

        private void InitSubView()
        {
            _viewInits = new List<IViewInit>();
            InitViewInterface(_viewInits);
            InitAllSubView();
            
            _viewShows = new List<IViewShow>();
            _viewHides = new List<IViewHide>();
            _viewUpdates = new List<IViewUpdate>();
            InitInterface();
        }

        private void InitAllSubView()
        {
            foreach (var view in _viewInits)
            {
                view.Init(_transform);
            }
        }

        private void InitInterface()
        {
            InitViewInterface(_viewShows);
            InitViewInterface(_viewHides);
            InitViewInterface(_viewUpdates);
        }
        
        private void InitViewInterface<T>(List<T> list) where T : class
        {
            T self = this as T;
            var components = _transform.GetComponentsInChildren<T>(true).ToList();
            components.Remove(self);
            list.AddRange(components);
        }
    }

    public abstract class BaseViewBase : RootViewBase
    {
        public override UILayer Layer
        {
            get { return UILayer.BASE_UI; }
        }
    }
    
    public abstract class OverlayViewBase : RootViewBase
    {
        public override UILayer Layer
        {
            get { return UILayer.OVERLAY_UI; }
        }
    }
    
    public abstract class TopViewBase : RootViewBase
    {
        public override UILayer Layer
        {
            get { return UILayer.TOP_UI; }
        }
    }
    
    public abstract class SubViewBase : ViewBase
    {
    }
}