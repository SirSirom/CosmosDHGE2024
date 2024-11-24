using Cosmos.System.ExtendedASCII;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace nectarOS
{
    public class ConsoleUtils
    {

        public static void setBackgroundColor(string color)
        {
            setTextBackgroundColor(color);
            Console.Clear();
        }
        public static void setTextBackgroundColor(string color)
        {
            switch (color)
            {
                case "red":
                    Console.BackgroundColor = ConsoleColor.Red;
                    break;
                case "magenta":
                    Console.BackgroundColor = ConsoleColor.Magenta;
                    break;
                case "blue":
                    Console.BackgroundColor = ConsoleColor.Blue;
                    break;
                case "cyan":
                    Console.BackgroundColor = ConsoleColor.Cyan;
                    break;
                case "green":
                    Console.BackgroundColor = ConsoleColor.Green;
                    break;
                case "yellow":
                    Console.BackgroundColor = ConsoleColor.Yellow;
                    break;
                case "dred":
                    Console.BackgroundColor = ConsoleColor.DarkRed;
                    break;
                case "dmagenta":
                    Console.BackgroundColor = ConsoleColor.DarkMagenta;
                    break;
                case "dblue":
                    Console.BackgroundColor = ConsoleColor.DarkBlue;
                    break;
                case "dcyan":
                    Console.BackgroundColor = ConsoleColor.DarkCyan;
                    break;
                case "dgreen":
                    Console.BackgroundColor = ConsoleColor.DarkGreen;
                    break;
                case "dyellow":
                    Console.BackgroundColor = ConsoleColor.DarkYellow;
                    break;
                case "gray":
                    Console.BackgroundColor = ConsoleColor.Gray;
                    break;
                case "dgray":
                    Console.BackgroundColor = ConsoleColor.DarkGray;
                    break;
                case "black":
                    Console.BackgroundColor = ConsoleColor.Black;
                    break;
                case "white":
                    Console.BackgroundColor = ConsoleColor.White;
                    break;
                default:
                    break;
            }
        }
        public static void setConsoleColor(string color)
            {
            switch (color)
            {
                case "red":
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case "magenta":
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    break;
                case "blue":
                    Console.ForegroundColor = ConsoleColor.Blue;
                    break;
                case "cyan":
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    break;
                case "green":
                    Console.ForegroundColor = ConsoleColor.Green;
                    break;
                case "yellow":
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                case "dred":
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    break;
                case "dmagenta":
                    Console.ForegroundColor = ConsoleColor.DarkMagenta;
                    break;
                case "dblue":
                    Console.ForegroundColor = ConsoleColor.DarkBlue;
                    break;
                case "dcyan":
                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    break;
                case "dgreen":
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    break;
                case "dyellow":
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    break;
                case "gray":
                    Console.ForegroundColor = ConsoleColor.Gray;
                    break;
                case "dgray":
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    break;
                case "black":
                    Console.ForegroundColor = ConsoleColor.Black;
                    break;
                default:
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
                }
            }
        public static void writeWithColor(string text)
        {
            ConsoleColor defaultForegroundColor = Console.ForegroundColor;
            ConsoleColor defaultBackgroundColor = Console.BackgroundColor;
            int offset = 0;
            for (int commandStart = text.IndexOf('<'); commandStart > -1; commandStart = text.IndexOf('<', offset))
            {
                int parameterStart = text.IndexOf(':', offset);
                int parameterEnd = text.IndexOf('>', parameterStart);
                if (parameterStart == -1 || parameterEnd == -1)
                {
                    break;
                }

                offset = parameterEnd;
                int nextCommandStart = text.IndexOf('<', offset) >0 ? text.IndexOf('<', offset) : text.Length;
                offset = nextCommandStart;
                string command = text.Substring(commandStart+1, parameterStart-commandStart - 1);
                string parameter = text.Substring(parameterStart+ 1, parameterEnd - parameterStart - 1);
                if (command == "c")
                {
                    setConsoleColor(parameter);
                }
                else if (command == "b")
                {
                    setTextBackgroundColor(parameter);
                }
                string textValue = text.Substring(parameterEnd+1 , nextCommandStart - parameterEnd - 1);
                Console.Write(textValue);
            }
            Console.Write(text.Substring(offset,text.Length-offset));
            Console.ForegroundColor = defaultForegroundColor;
            Console.BackgroundColor = defaultBackgroundColor;
        }

        public static void writeLineWithColor(string text) {
            writeWithColor(text);
            Console.WriteLine();
        }
    }
}
