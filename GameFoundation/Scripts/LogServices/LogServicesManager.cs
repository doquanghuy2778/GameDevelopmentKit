namespace UniTemplate.LogServices
{
    using UnityEngine;

    public class LogServicesManager : ILogServices
    {
        public void LogCallerInfor(
            [System.Runtime.CompilerServices.CallerLineNumber] int    line       = 0,
            [System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
            [System.Runtime.CompilerServices.CallerFilePath]   string filePath   = ""
        )
        {
            Debug.Log($"{line} :: {memberName} :: {filePath}");
        }

        public void LogError(string message)
        {
            Debug.LogError(message);
        }

        public void Log(string message)
        {
            Debug.Log(message);
        }

        public void LogException(string message)
        {
            Debug.LogException(new System.Exception(message));
        }

        public void LogWarning(string message)
        {
            Debug.LogWarning(message);
        }

        public void LogWithColor(string message, Color? color = null)
        {
            var colorText                = Color.white;
            if (color != null) colorText = (Color)color;
            Debug.Log($"<color=#{(byte)(colorText.r * 255f):X2}{(byte)(colorText.g * 255f):X2}{(byte)(colorText.b * 255f):X2}>{message}</color>");
        }
    }
}