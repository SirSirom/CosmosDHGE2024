using System;
using System.Collections.Generic;
using System.Text;
using Sys = Cosmos.System;
using nectarOS;
using System.ComponentModel.Design;
using System.Collections.ObjectModel;
using System.Linq;

namespace nectarOS;

public class Kernel : Sys.Kernel
{
    public static int memorySize = 16;
    Cosmos.Core.ManagedMemoryBlock memoryBlock = new Cosmos.Core.ManagedMemoryBlock(16);
    public static Dictionary<string, Command> commands = new Dictionary<string, Command>
        {
            {"echo",new Echo()},
            {"sysinfo", new SysInfo(DateTime.Now) },
            {"exit", new Exit() }
        };



    protected override void BeforeRun()
    {
        commands.Add("help", new Help(commands));
        Console.WriteLine("Cosmos booted successfully. Type a line of text to get it echoed back.");
    }

    protected override void Run()
    {
        Console.Write("Input: ");
        var input = Console.ReadLine();
        string[] argv = input.Split(' ');
        try
        {
            commands[argv[0]].run(argv.Skip(1).ToArray());
        }
        catch (NullReferenceException)
        {
            Console.WriteLine("couldnt find specified Command: {0}", argv[0]);
        }
        
        switch (argv[0])
        {
            
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
        }
    }
}   

