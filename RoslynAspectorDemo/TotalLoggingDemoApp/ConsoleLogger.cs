using RoslynAspector.TotalLoggingData;

namespace TotalLoggingDemoApp
{
	internal class ConsoleLogger : ILogAdapter
	{
		public LogWrapperLevel Level => LogWrapperLevel.Information;

		public void Write(string message, Exception? ex, LogWrapperLevel level, string file, string member, int line)
		{
			if (ex != null)
			{
				Console.WriteLine(ex);
			}

			Console.WriteLine($"{message} {level} {file} {member} {line}");
		}
	}
}