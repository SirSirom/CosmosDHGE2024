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
        public bool run(string[] args);
    }

    public class RunP : Command
    {
        private static Dictionary<string, Process> programms = new Dictionary<string, Process>
        {
            {Calculator.call,new Calculator()},
        };
        

        public static string call { get => "runP";}
        public string help { get =>"run a programm";
        }

        public bool run(string[] argv)
        {
            try
            {
                programms[argv[0]].start();
                return true;
            }
            catch (NullReferenceException)
            {
                ConsoleUtils.writeLineWithColor("<b:dred><c:red>couldnt find specified Programm: " + argv[0]);
                return true;
            }

        }
    }

    public class Echo : Command
    {
        public static string call { get => "echo";}

        public string help { get => "will repeat the following input"; }

        public bool run(string[] argv)
        {
            foreach (var arg in argv)
            {
                Console.Write("{0} ", arg);
            }
            Console.WriteLine();
            return true;
        }

    }

    public class Exit : Command
    {
        public static string call { get => "exit"; }

        public string help { get => "shutdown the system"; }

        public bool run(string[] argv)
        {
            return false;
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

        public bool run(string[] argv)
        {
            Console.WriteLine("running since: {0}",sysStartTime);
            Console.WriteLine("runtime: {0} ",getSysRunTime());
            return true;
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

        public bool run(string[] argv)
        {
            try
            {
                Console.WriteLine(commands[argv[0]].help);
            }
            catch (NullReferenceException)
            {
                Console.WriteLine("couldnt find specified Command: {0}", argv[0]);
            }
            return true;
        }
    }

    public class Time: Command
    {
        public static string call { get => "time"; }
        public string help { get => "time the execution of a Command"; }

        private Dictionary<String, Command> commands;
        public Time(Dictionary<String, Command> commands)
        {
            this.commands = commands;
        }

        public bool run(string[] argv) {
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
            return true;
        }
    }
}
