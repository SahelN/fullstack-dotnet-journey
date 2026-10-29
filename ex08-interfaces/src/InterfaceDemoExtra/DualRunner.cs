namespace InterfaceDemoExtra
{
    public class DualRunner : IRunnable, IRunnableWithResult
    {
        public int Run()
        {
            Console.WriteLine("Metoden IRunnableWithResult.Run() körs");
            return 7;
        }

        void IRunnable.Run()
        {
            Console.WriteLine("Metoden IRunnable.Run() körs");
        }
    }
}
