using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nectarOS
{
    internal class Process
    {
        private int processID;
        public Process(MemManager manager) {
            processID = this.GetHashCode();
            manager.initializeMemory(processID, 1 , 77);
            manager.writeMemory(processID,0,Encoding.ASCII.GetBytes("Hello World"));
            Console.WriteLine(Encoding.ASCII.GetString(manager.readMemory(processID, 0)));
        }
    }
}
