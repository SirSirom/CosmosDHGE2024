using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nectarOS
{
    internal abstract class Process
    {
        protected int processID;
        protected MemManager MemoryManager;

        protected void initProcess(MemManager manager, int blocks, uint blockSize)
        {
            this.MemoryManager = manager;
            this.processID = this.GetHashCode();
            Console.WriteLine("exec Mem init");
            manager.initializeMemory(processID, blocks, blockSize);
        }

        protected void write(byte[] payload, int block)
        {
            MemoryManager.writeMemory(processID, block, payload);
        }

        protected byte[] read(int block)
        {
            return MemoryManager.readMemory(processID, block);
        }
    }
}
