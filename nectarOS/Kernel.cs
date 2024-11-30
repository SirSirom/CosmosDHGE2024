using System;
using System.Collections.Generic;
using System.Text;
using Sys = Cosmos.System;
using nectarOS;
using System.ComponentModel.Design;
using System.Collections.ObjectModel;
using System.Linq;
using Cosmos.HAL;
using Cosmos.System.Graphics;
using System.Text.RegularExpressions;
using System.Diagnostics;
using Cosmos.System.Graphics.Fonts;
using Cosmos.System.ExtendedASCII;

namespace nectarOS;

public class Kernel : Sys.Kernel
{
    public static int memorySize = 16;
    public static MemManager memManager = new MemManager();
    Cosmos.Core.ManagedMemoryBlock memoryBlock = new Cosmos.Core.ManagedMemoryBlock(16);
    public static Dictionary<string, Command> commands = new Dictionary<string, Command>
        {
            {Echo.call,new Echo()},
            {SysInfo.call, new SysInfo(DateTime.Now) },
            {Exit.call, new Exit() },
            {RunP.call, new RunP() }
        };


    protected override void BeforeRun()
    {
        commands.Add(Help.call, new Help(commands));
        commands.Add(Time.call, new Time(commands));

        Encoding.RegisterProvider(Cosmos.System.ExtendedASCII.CosmosEncodingProvider.Instance);
        Console.OutputEncoding = CosmosEncodingProvider.Instance.GetEncoding(437);
        SetKeyboardScanMap(new Sys.ScanMaps.DE_Standard());

        Console.Clear();

        ConsoleUtils.writeWithColor("<c:yellow><b:dyellow>" +
            "  ░░░░░░░░  ░░░░  ▒▒▒▒▒▒▒▒▒▒▒▒░░░░░░░░  ░░    ░░  ░░▒▒░░▒▒▒▒▒▒  ░░░░░░░░▒▒░░░░░░" +
            "▒▒▒▒▒▒▒▒▒▒░░  ░░  ▒▒▒▒▓▓▒▒▒▒▒▒  ▒▒▒▒▒▒▓▓▒▒▒▒  ░░  ░░▒▒▒▒▒▒▒▒▒▒░░▒▒▒▒▓▓▒▒▒▒▒▒  ░░" +
            "▒▒▓▓▓▓▓▓▓▓▒▒  ░░▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒░░░░▒▒▒▒▒▒▒▒░░  ░░░░░░▒▒▒▒▒▒  ░░░░▒▒▒▒▒▒▓▓▒▒░░░░" +
            "▒▒▒▒▓▓▓▓▒▒▓▓▒▒  ▒▒▒▒▒▒▒▒▒▒▒▒░░░░░░▒▒▒▒▒▒▒▒▒▒▒▒    ░░░░▒▒▒▒░░░░░░░░▒▒▒▒▒▒▒▒▓▓▒▒  " +
            "░░▒▒▒▒▒▒▓▓▒▒▒▒  ░░░░░░░░░░  ░░░░░░▒▒▒▒▒▒▒▒▒▒▒▒░░    ░░        ░░░░░░▒▒▒▒▒▒▒▒▒▒░░" +
            "░░▒▒▒▒▒▒▒▒▒▒▒▒  ░░▒▒▒▒▒▒▒▒▒▒  ░░  ▒▒▒▒▓▓▓▓▒▒▒▒  ░░▒▒▒▒▒▒▒▒▒▒  ░░  ▒▒▒▒▒▒▒▒▒▒▒▒  " +
            "▒▒▒▒▒▒▒▒▒▒▒▒▒▒░░▒▒▒▒▓▓▓▓▒▒▓▓░░░░▒▒▒▒▒▒▓▓▓▓▓▓░░░░▒▒▒▒▒▒▒▒▒▒▓▓▒▒  ▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒" +
            "░░▓▓▒▒▓▓▒▒▒▒  ▒▒▒▒▒▒▓╔════════════════════════════════════╗▓▓▓  ░░▒▒▒▒▒▒▒▒▒▒░░▒▒" +
            "░░▒▒▒▒▒▒▒▒░░░░░░▒▒▒▒▓║              ╖           ╓──╖ ╓──┐ ║▒▓▓▒▒      ░░░░  ░░░░" +
            "░░░░░░░░░░░░░░▒▒  ▒▒▓║ ╓┐ ╓ ╓─╖ ╓── ╫── ╓─╖ ╖┌─ ║  ║ ╙──╖ ║▒▒▒░░▒▒▒▒▒▒▒▒▒▒░░  ░░" +
            "▒▒▒▒▓▓▓▓▓▓▓▓  ░░▒▒▒▒▒║ ║└┐║ ╟─╜ ║   ║   ║┌╢ ╟┘  ║  ║ │  ║ ║▒░░▒▒▒▒▒▒▒▒▒▒▓▓▒▒░░░░" +
            "▒▒▒▒▒▒▓▓▓▓▓▓▒▒  ░░▒▒▒║ ╜ └╜ ╨── ╨── ╨─┘ ╙┘╨ ╨   ╙──╜ └──╜ ║▒░░▒▒░░▒▒▓▓▒▒▓▓▒▒▒▒  " +
            "▒▒▒▒▓▓▓▓▓▓▓▓▓▓░░░░▒▒░╚═╦══════════════════════════════════╝ ▒▒░░  ▒▒▒▒▒▒▒▒▒▒▒▒░░" +
            "▒▒▓▓▒▒▒▒▒▒▓▓▒▒░░░░▒▒▒▒▒└ Made by Niklas,Elias,Conner,Tim ▒  ░░░░  ▒▒▒▒▒▒▒▒▒▒▒▒  " +
            "▓▓▓▓▒▒▒▒▓▓▓▓▒▒  ▒▒▒▒▓▓▓▓▓▓▓▓░░░░▒▒▒▒▒▒▒▒▒▒▒▒▒▒░░▒▒▒▒▓▓▒▒▓▓▓▓  ░░  ░░▒▒▒▒▒▒▒▒░░░░" +
            "░░▒▒▓▓▒▒▓▓▒▒░░▒▒▒▒▒▒▓▓▓▓▒▒▒▒▒▒  ░░▒▒▒▒▒▒▒▒▒▒  ▒▒▒▒▒▒▒▒▒▒▓▓▓▓░░▒▒  ░░▒▒▒▒▒▒▒▒  ▒▒" +
            "░░▒▒▒▒▒▒▒▒▒▒░░▒▒░░▒▒▒▒▒▒▒▒▒▒▓▓░░  ░░░░▒▒▒▒░░░░░░░░▒▒▓▓▓▓▒▒▓▓▓▓░░░░░░░░░░░░░░  ▒▒" +
            "  ░░░░░░░░  ░░░░  ▒▒▒▒▒▒▒▒▒▒▒▒▒▒░░▒▒▒▒▒▒▒▒  ░░░░  ▒▒▒▒▓▓▒▒▒▒▒▒  ▒▒▒▒▒▒▒▒▒▒░░  ░░" +
            "▒▒▒▒▓▓▓▓▓▓▒▒░░░░░░▒▒▒▒▒▒▒▒▒▒▒▒░░▒▒▒▒▓▓▓▓▒▒▒▒      ▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▓▓▓▓▒▒▓▓▓▓░░  " +
            "▒▒▒▒▒▒▓▓▒▒▓▓▒▒  ▒▒▓▓▒▒▓▓▓▓▒▒▒▒▒▒▒▒▒▒▒▒▒▒▓▓▒▒░░░░▒▒  ▒▒▒▒▒▒▒▒░░▒▒▒▒▒▒▒▒▒▒▓▓▒▒▒▒  " +
            "░░▒▒▒▒▒▒▒▒▒▒▒▒░░  ▒▒▒▒▒▒▒▒▒▒░░▒▒░░▒▒▒▒▒▒▒▒▒▒▒▒  ▒▒░░░░▒▒▒▒░░░░▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒  "
        );
        System.Threading.Thread.Sleep(3000);
        Console.Clear();
    }

    protected override void Run()
    {
        bool kr = true;
        ConsoleUtils.writeWithColor("<c:green>- <c:yellow>Input<c:green>:");
        var input = Console.ReadLine();
        string[] argv = input.Split(' ');
        try
        {
            kr = commands[argv[0]].run(argv.Skip(1).ToArray());
        }
        catch (NullReferenceException)
        {
            ConsoleUtils.writeLineWithColor("<b:dred><c:red>couldnt find specified Command: "+ argv[0]);
        }
        if (!kr) {Cosmos.System.Power.Shutdown();}
    }
}   

