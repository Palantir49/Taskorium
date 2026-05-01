using System.Reflection;

namespace Taskorium.ServiceDefaults;

public static class BuildTimeDetector
{
    public static bool IsBuildTime { get; } = Assembly.GetEntryAssembly()?.GetName().Name == "GetDocument.Insider";
}
