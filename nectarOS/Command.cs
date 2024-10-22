using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using nectarOS;

namespace nectarOS
{
    public interface Command

    {
        public string help { get; }

        public void run(string[] args);
    }

    public class Echo : Command
    {
        private string _help = "will repeat the following input";

        public string help { get => _help; }

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
        private string _help = "shutdown the system";

        public string help { get => _help; }

        public void run(string[] argv)
        {
            Cosmos.System.Power.Shutdown();
        }

    }

    public class SysInfo : Command
    {
        private string _help = "get Information about the system";
        public string help { get => _help; }

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
        private string _help = "get Information about a command";
        public string help { get => _help; }

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
}
