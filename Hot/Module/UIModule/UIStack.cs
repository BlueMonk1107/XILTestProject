using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace UIModule
{
	public class UIStack
	{
		public string UIName { get; private set; }
		public IView View { get; private set; }
		private Stack<UIStack> _uiStack;
		private UILayer _subLayer;

		public UIStack(string uiName,IView view)
		{
			UIName = uiName;
			View = view;
			var layer = view.Self.GetComponent<ILayer>();
			_subLayer = layer.Layer + 1;
			_uiStack = new Stack<UIStack>();
			InitAll();
			ShowAll();
		}

		public IView Show()
		{
			ShowAll();
			if(_uiStack.Count >0 )
				_uiStack.Peek().Show();

			return View;
		}

		public IView Show(string name, IView view)
		{
			var layer = view.Self.GetComponent<ILayer>();
			
			ShowAll();
			
			if (layer.Layer == _subLayer)
			{
				UIStack stack = new UIStack(name,view);
				_uiStack.Push(stack);
				_uiStack.Peek().Show();
			}
			else if(layer.Layer > _subLayer)
			{
				_uiStack.Peek().Show(name, view);
			}

			return view;
		}

		public void Hide()
		{
			HideAll();
			
			if(_uiStack.Count > 0)
				_uiStack.Peek().Hide();
		}

		public void Back(string curUiName)
		{
			if (_uiStack.Count > 0)
			{
				if (_uiStack.Peek().UIName == curUiName)
				{
					 _uiStack.Pop().Hide();
					 if (_uiStack.Count > 0)
						 _uiStack.Peek().Show();
					 else
						 ShowAll();
				}
			}
			else
			{
				HideAll();
			}
		}

		private void InitAll()
		{
			foreach (var show in View.Self.GetComponents<IInit>())
			{
				show.Init(View.Self);
			}
		}
		
		private void ShowAll()
		{
			foreach (IShow show in View.Self.GetComponents<IShow>())
			{
				show.Show();
			}
		}
		
		private void HideAll()
		{
			foreach (var show in View.Self.GetComponents<IHide>())
			{
				show.Hide();
			}
		}
	}
}
