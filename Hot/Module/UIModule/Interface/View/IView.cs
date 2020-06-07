using UnityEngine;

namespace UIModule
{
    public interface IView : IViewInit, IViewShow, IViewHide, IViewUpdate
    {
        RectTransform Self { get; }
        void Reacquire();
    }
}