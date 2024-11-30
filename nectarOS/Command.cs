using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using nectarOS;

namespace nectarOS
{
    public interface Command

    {
        public string help { get; }
        public static string call { get; }
        public void run(string[] args);
    }

    public class Cal : Command
    {
        private Calculator calculator;
        public Cal(MemManager manager) {
            this.calculator = new Calculator(manager);
        }

        public static string call { get => "cal";}
        public string help { get => "" +
                "input two numbers an an operator to get the solution to the equasion." +
                "They must be seperated by one space." +
                "Enter save after the equasion to save it." +
                "Enter cal read to get the last saved equation"; }
        public void run(string[] args)
        {
            calculator.run(args);
        }
    }

    public class Echo : Command
    {
        public static string call { get => "echo";}

        public string help { get => "will repeat the following input"; }

        public void run(string[] argv)
        {
            foreach (var arg in argv)
            {
                Console.Write("{0} ", arg);
            }
            Console.WriteLine();
        }

    }

    public class Exit : Command
    {
        public static string call { get => "exit"; }

        public string help { get => "shutdown the system"; }

        public void run(string[] argv)
        {
            Cosmos.System.Power.Shutdown();
        }

    }

    public class SysInfo : Command
    {
        public static string call { get => "sysInfo"; }
        public string help { get => "get Information about the system"; }

        public SysInfo(DateTime sysStartTime)
        {
            this.sysStartTime = sysStartTime;
        }

        public DateTime sysStartTime;

        public void run(string[] argv)
        {
            Console.WriteLine("running since: {0}",sysStartTime);
            Console.WriteLine("runtime: {0} ",getSysRunTime());
        }

        public TimeSpan getSysRunTime()
        {
            return DateTime.Now - sysStartTime;
        }

    }

    public class Help : Command
    {
        public static string call { get => "help"; }
        public string help { get => "get Information about a command"; }

        public Help(Dictionary<String,Command> commands)
        {
            this.commands = commands;
        }
        private Dictionary<String,Command> commands;

        public void run(string[] argv)
        {
            try
            {
                Console.WriteLine(commands[argv[0]].help);
            }
            catch (NullReferenceException)
            {
                Console.WriteLine("couldnt find specified Command: {0}", argv[0]);
            }
        }
    }

    public class Time: Command
    {
        public static string call { get => "time"; }
        public string help { get => "time the execution of a Command"; }

        public Time(Dictionary<String, Command> commands)
        {
            this.commands = commands;
        }
        private Dictionary<String, Command> commands;

        public void run(string[] argv) {
            Stopwatch sw = new Stopwatch();
            try
            {
                commands[argv[0]].run(argv.Skip(1).ToArray());
            }
            catch (NullReferenceException)
            {
                Console.WriteLine("couldnt find specified Command: {0}", argv[0]);
            }
            sw.Stop();
            Console.WriteLine("waited {sw.ElapsedMilliseconds} for execution of {argv[0]}");
        }
    }
}
