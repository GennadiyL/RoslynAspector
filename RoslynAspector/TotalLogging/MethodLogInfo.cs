using RoslynAspector.TotalLoggingData;

namespace RoslynAspector.TotalLogging
{
	internal record MethodLogInfo
	{
		public LogWrapperLevel OnEnterLogLevel { get; init; }
		public LogWrapperLevel OnExitLogLevel { get; init; }
		public LogWrapperLevel OnErrorLogLevel { get; init; }
		public bool IsExceptionLoggingEnabled { get; init; }
	}
}