using RoslynAspector.TotalLoggingData;
using TotalLoggingDemoLib;

namespace TotalLoggingDemoApp;

internal class Program
{
	private static async Task Main()
	{
		LogWrapper.Configure(new ConsoleLogger());
		
		Rect rect = new Rect();
		rect.Left = 10;
		rect.Top = 20;
		rect.Width = 100;
		rect.Height = 200;

		rect.Move(10, 20);
		Console.WriteLine($"Right: {rect.GetRight()}");
		Console.WriteLine($"Bottom: {rect.GetBottom()}");

		await rect.MoveAsync(15, 25);
		Console.WriteLine($"Right: {await rect.GetRightAsync()}");
		Console.WriteLine($"Bottom: {await rect.GetBottomAsync()}");
	}
}
