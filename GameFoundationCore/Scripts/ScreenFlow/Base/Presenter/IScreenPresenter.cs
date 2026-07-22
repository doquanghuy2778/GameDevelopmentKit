namespace GameDevelopmentKit.GameFoundationCore.Scripts.ScreenFlow.Base.Presenter
{
    using System;
    using Cysharp.Threading.Tasks;
    using GameDevelopmentKit.GameFoundationCore.Scripts.MVP;
    using UnityEngine;

    public interface IScreenPresenter : IUIPresenter, IDisposable
    {
        public string       ScreenId { get; }
        public ScreenStatus ScreenStatus   { get; set; }
        public void         SetViewParent(Transform parent);
        public Transform    GetViewParent();
        public UniTask      BindData();
        public UniTask      OpenViewAsync();
        public UniTask      CloseViewAsync();
        public void         CloseView();
        public void         HideView();
        public void         DestroyView();
    }

    public interface IScreenPresenter<in TModel> : IScreenPresenter
    {
        public UniTask OpenView(TModel model);
    }

    public enum ScreenStatus
    {
        Opened,
        Closed,
        Hide,
        Destroyed
    }
}