namespace Project2D;
public static class Logger
{
	public static void LogMs(Action code, string hint = "")
    {
        var watch = new Stopwatch();
        watch.Start();
        code();
        watch.Stop();
        GD.Print($"{hint} {watch.ElapsedMilliseconds} ms");
    }
}
