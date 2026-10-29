namespace InterfaceDemo
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Program1 p1 = new Program1();
            //p1.Run();
            //Program2 p2 = new Program2();
            //p2.Go();

            Program1 program1 = new Program1();
            Program2 program2 = new Program2();

            IStartable startable1 = program1;
            startable1.Start();
            IStoppable stoppable = program1;
            stoppable.Stop();

            IStartable startable2 = program2;
            startable2.Start();
        }
    }
}
