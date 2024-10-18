using System;
using System.Collections.Generic;
using System.Text;
using Sys = Cosmos.System;

namespace test_Os
{
    public class Kernel : Sys.Kernel
    {
        DateTime sysStartTime = DateTime.Now;
        public static int memorySize = 16;
        Cosmos.Core.ManagedMemoryBlock memoryBlock = new Cosmos.Core.ManagedMemoryBlock(16);
        protected override void BeforeRun()
        {
            Console.WriteLine("Cosmos booted successfully. Type a line of text to get it echoed back.");
        }

        protected override void Run()
        {
            Console.Write("Input: ");
            var input = Console.ReadLine();
            string[] argv = input.Split(' ');
            switch (argv[0])
            {
                case "echo":
                    {
                        Console.WriteLine("printing now");
                        break;
                    }
                case "help":
                    {
                        Console.WriteLine("Currently Helping");
                        break;
                    }
                case "sysruntime":
                    {
                        Console.WriteLine("{0}", getSysRunTime());
                        break;
                    }
                case "save":
                    {

                        memoryBlock.Write16((uint)Convert.ToInt16(argv[1]), (ushort)Convert.ToInt16(argv[2]));
                        break;
                    }
                case "load":
                    {

                        Console.WriteLine("Reading: {0}", memoryBlock.Read16((uint)Convert.ToInt16(argv[1])));
                        break;
                    }
                case "exit":
                    {
                        Cosmos.System.Power.Shutdown();
                        break;
                    }
            }
        }

        public TimeSpan getSysRunTime()
        {
            return DateTime.Now - sysStartTime;
        }


    }
}
