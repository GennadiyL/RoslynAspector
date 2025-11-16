namespace RoslynAspector.TotalLoggingData
{
	public interface ILogAdapter
	{
		LogWrapperLevel Level { get; }
		void Write(string message, Exception? ex, LogWrapperLevel level, string file, string member, int line);
	}
}
