namespace GameDevelopmentKit.GameFoundationCore.Scripts.ScreenFlow
{
    using GameDevelopmentKit.GameFoundationCore.Scripts.ScreenFlow.Manager;
    using global::GameFoundationCore.Scripts.ScreenFlow.Base.View;

    public static class ScreenHelper
    {
        public static string GetScreenId<TView>() where TView : IScreenView
        {
            return $"{SceneDirector.CurrentSceneName}/{typeof(TView).Name}";
        }
    }
}