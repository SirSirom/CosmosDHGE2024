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

        public void start()
        {
            while (run()){}
        }
        protected abstract bool run();
        protected void initProcess(int blocks, uint blockSize)
        {
            this.processID = this.GetHashCode();
            MemManager.initializeMemory(processID, blocks, blockSize);
        }

        protected void write(byte[] payload, int block)
        {
            MemManager.writeMemory(processID, block, payload);
        }

        protected byte[] read(int block)
        {
            return MemManager.readMemory(processID, block);
        }
    }
}
