using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nectarOS
{
    public class ConsoleUtils
    {
        public static void writeWithColor(string input)
        {
            int index = input.IndexOf("/useColor(") ;

            if (index < 0)
            {
                Console.WriteLine(input) ;
                return;
            }

            while (index < input.Length)
            {
                if (input.Substring(index).StartsWith("/useColor("))
                {
                    int colorStart = index + "/useColor(".Length;
                    int colorEnd = input.IndexOf("):", colorStart);

                    if (colorEnd != -1)
                    {
                        string color = input.Substring(colorStart, colorEnd - colorStart);
                        setConsoleColor(color);
                        int textStart = colorEnd + 2;
                        int textEnd = input.IndexOf("/useColor(", index);
                        if (textEnd != -1)
                        {
                            Console.Write(input.Substring(textStart,textEnd));
                            index = textEnd;
                        }
                        else
                        {
                            Console.Write(input.Substring(textStart,input.Length -1));
                            index = input.Length;
                        }

                    }
                } 
            }
        }

        private static void setConsoleColor(string color)
        {
            switch (color)
            {
                case "red":
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                default:
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
            }
        }
    }
}
