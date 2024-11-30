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
		public Calculator(MemManager manager) {
            initProcess(manager, 8, 64);
        }

		int count = 0;

		public void run(string[] args)
		{
			if (args.Length == 1 && args[0] == "read")
			{
				if (count < 8)
				{
					for (int i = 0; i < count; i++)
					{
						Console.WriteLine(Encoding.ASCII.GetString(this.read(i)));
					}
				} else
				{
					for (int i = 0; i <= 7; i++)
					{
                        Console.WriteLine(Encoding.ASCII.GetString(this.read(i)));
                    }
				}
			}
			else if (args.Length >= 3) {
				float erg = 0;
				if (float.TryParse(args[0], out float first) && float.TryParse(args[2], out float second)) {
					switch (args[1]) {
						case "+": erg = first + second; break;
						case "-": erg = first - second; break;
						case "*": erg = first * second; break;
						case "/": erg = first / second; break;
						default: ConsoleUtils.writeWithColor("<c:red><b:black>" + "Kein gültiges Rechenzeichen"); Console.WriteLine(); break;
					}
					Console.WriteLine(erg);
				} else {
					ConsoleUtils.writeWithColor("<c:red><b:black>" + "Keine gültige Eingabe");
					Console.WriteLine();
				}

                if (args.Length == 4 && args[3] == "write")
                {
                    String saver = "";
                    for (int i = 0; i <= 2; i++)
                    {
                        saver += args[i];
                    }
                    saver += "=" + erg;
                    this.write(Encoding.ASCII.GetBytes(saver), count%8);
					count++;
                }
            }
		}
	}
}