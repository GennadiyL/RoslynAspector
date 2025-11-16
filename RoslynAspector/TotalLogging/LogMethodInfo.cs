using RoslynAspector.TotalLoggingData;

namespace RoslynAspector.TotalLogging
{
	internal record LogMethodInfo
	{
		public LogWrapperLevel OnEntryLogLevel { get; init; }
		public LogWrapperLevel OnExitLogLevel { get; init; }
		public LogWrapperLevel OnErrorLogLevel { get; init; }
		public bool IsExceptionLoggingEnabled { get; init; }
	}
}