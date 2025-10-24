namespace UniTemplate.LogServices
{
    using UnityEngine;

    public interface ILogServices
    {
        public void LogCallerInfor(int line, string memberName, string sourceFilePath);

        public void LogError(string message);

        public void Log(string message);

        public void LogException(string message);

        public void LogWarning(string message);

        public void LogWithColor(string message, Color? color = null);
    }
}