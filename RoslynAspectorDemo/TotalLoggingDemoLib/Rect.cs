namespace TotalLoggingDemoLib;

public class Rect
{
	public int Left { get; set; }
	public int Top { get; set; }
	public int Width { get; set; }
	public int Height { get; set; }

	public int GetRight()
	{
		return Left + Width;
	}

	public int GetBottom()
	{
		return Top + Height;
	}

	public virtual void Move(int dx, int dy)
	{
		Left += dx;
		Top += dy;
	}

	public async Task<int> GetRightAsync()
	{
		return await Task.FromResult(Left + Width);
	}

	public async Task<int> GetBottomAsync()
	{
		await Task.Delay(100);
		return Top + Height;
	}

	public async Task MoveAsync([RoslynAspector.TotalLoggingData.LogIgnoreParameter] int dx, int dy)
	{
		await Task.Delay(100);
		MoveInner(dx, dy);
	}

	private void MoveInner(int dx, int dy)
	{
		Left += dx;
		Top += dy;
	}

	public override string ToString()
	{
		return $"{Left} {Top} {Height} {Width}";
	}
}