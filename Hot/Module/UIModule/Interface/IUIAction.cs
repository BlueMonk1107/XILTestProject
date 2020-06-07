using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UIModule
{
    public interface IInit
    {
        void Init(Transform self);
    }

    public interface IShow
    {
        void Show();
    }

    public interface IHide
    {
        void Hide();
    }
    
    public interface IUpdate
    {
        void UpdateFun();
    }
}

