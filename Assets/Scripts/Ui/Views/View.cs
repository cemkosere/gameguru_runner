using UnityEngine;
using UnityEngine.Events;
using Zenject;

namespace Ui.Views
{
    public abstract class View : MonoBehaviour
    {
        #region Statics
        private static UnityAction<ViewType> _onChangeViewType = delegate {  };
        private static ViewType _currentViewType = ViewType.None;
        public static void ChangeView(ViewType viewType)
        {
            if(viewType == _currentViewType) return;
            _currentViewType = viewType;
            _onChangeViewType?.Invoke(viewType);
        }
        #endregion
        
        protected abstract ViewType Type{get;}
        private Canvas _canvas;
        private Canvas Canvas
        {
            get
            {
                if (!_canvas)
                    _canvas = GetComponent<Canvas>();
                return _canvas;
            }
        }
        
        protected virtual void OnPreShow(){}
        protected virtual void OnPostShow(){}
        protected virtual void OnPreHide(){}
        protected virtual void OnPostHide(){}

        [Inject]
        private void OnInject()
        {
            _onChangeViewType += (viewType) =>
            {
                if (Type == viewType)
                {
                    Show();
                }
                else
                {
                    Hide();
                }
            };
        }
        
        private void Show()
        {
            if(Canvas.enabled) return;
            OnPreShow();
            Canvas.enabled = true;
            OnPostShow();
        }

        private void Hide()
        {
            if(!Canvas.enabled) return;
            OnPreHide();
            Canvas.enabled = false;
            OnPostHide();
        }
    }
    
    public enum ViewType
    {
        None,
        MainMenu,
        GamePlay,
        Success,
        Fail
    }
}