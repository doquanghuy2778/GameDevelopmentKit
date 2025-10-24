namespace UniTemplate.ScreenFlow.Base
{
    using Cysharp.Threading.Tasks;
    using UnityEngine;

    public interface IScreenLifecycle
    {
        public string       ScreenId { get; }
        public ScreenStatus Status   { get; set; }
        public void         SetViewParent(Transform parent);
        public Transform    GetViewParent();
        public UniTask      BindData();
        public UniTask      OpenViewAsync();
        public UniTask      CloseViewAsync();
        public void         CloseView();
        public void         HideView();
        public void         DestroyView();
    }
    
    public interface IScreenLifecycle<in TModel> : IScreenLifecycle
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