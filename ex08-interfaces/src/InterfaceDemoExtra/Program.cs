namespace InterfaceDemoExtra
{
    internal class Program
    {
        static void Main(string[] args)
        {
            DualRunner runner = new DualRunner();

            IRunnable runnable = runner;
            runnable.Run();

            IRunnableWithResult runnableWithResult = runner;
            int result = runnableWithResult.Run();
            Console.WriteLine(result);
        }
    }
}
