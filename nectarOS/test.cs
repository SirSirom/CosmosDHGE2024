using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nectarOS
{
    internal class Test : Process
    {

        public Test(MemManager manager, int blocks, uint blocksize) {
            this.MemoryManager = manager;
            initProcess(manager, blocks, blocksize);
        }

        public void run()
        {
            this.write(Encoding.ASCII.GetBytes("Hello World"), 0);
            Console.WriteLine(Encoding.ASCII.GetString(this.read(0)));
        }
    }
}
