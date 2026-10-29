namespace InterfaceDemo
{
    internal class Program1 : IStartable, IStoppable
    {
        //public void Run()
        //{
        //    Console.WriteLine("Nu kör vi program 1");
        //}

        public void Start()
        {
            Console.WriteLine("Nu startar vi program 1");
        }

        public void Stop()
        {
            Console.WriteLine("Nu stoppar vi program 1");
        }
    }
}
