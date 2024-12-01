using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace nectarOS
{
	internal class Calculator : Process
	{
        public static Dictionary<string, Command> commands = new Dictionary<string, Command>
        {
            {Exit.call, new Exit() },
        };

        public static string call { get => "cal"; }
        public static string help
        {
            get =>
                "input two numbers an an operator to get the solution to the equasion." +
                "They must be seperated by one space." +
                "Enter save after the equasion to save it." +
                "Enter cal read to get the last saved equation";
        }

        public Calculator() {
            initProcess( 8, 64);
        }

		int count = 0;

		protected override bool run()
		{
			
            ConsoleUtils.writeWithColor("<c:green>- <c:yellow>Calculator Input<c:green>:");
            var input = Console.ReadLine();
            string[] argv = input.Split(' ');
			return handleInput(argv);
        }

		private bool handleInput(string[] argv) {

            try
            {
                return commands[argv[0]].run(argv.Skip(1).ToArray());
            }
            catch (NullReferenceException)
            {

                if (argv.Length == 1 && argv[0] == "read")
                {
                    if (count < 8)
                    {
                        for (int i = 0; i < count; i++)
                        {
                            Console.WriteLine(Encoding.ASCII.GetString(this.read(i)));
                        }
                    }
                    else
                    {
                        for (int i = 0; i <= 7; i++)
                        {
                            Console.WriteLine(Encoding.ASCII.GetString(this.read(i)));
                        }
                    }
                }
                else if (argv.Length >= 3)
                {
                    float erg = 0;
                    if (float.TryParse(argv[0], out float first) && float.TryParse(argv[2], out float second))
                    {
                        switch (argv[1])
                        {
                            case "+": erg = first + second; break;
                            case "-": erg = first - second; break;
                            case "*": erg = first * second; break;
                            case "/": erg = first / second; break;
                            default: ConsoleUtils.writeWithColor("<c:red><b:black>" + "Kein gültiges Rechenzeichen"); Console.WriteLine(); break;
                        }
                        Console.WriteLine(erg);
                    }
                    else
                    {
                        ConsoleUtils.writeWithColor("<c:red><b:black>" + "Keine gültige Eingabe");
                        Console.WriteLine();
                    }

                    if (argv.Length == 4 && argv[3] == "write")
                    {
                        String saver = "";
                        for (int i = 0; i <= 2; i++)
                        {
                            saver += argv[i];
                        }
                        saver += "=" + erg;
                        this.write(Encoding.ASCII.GetBytes(saver), count % 8);
                        count++;

                    }
                }
                return true;
            }
		}
	}
}