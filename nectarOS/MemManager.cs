using Cosmos.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nectarOS
{
    public class MemManager
    {
        private Dictionary<int, uint> blockSizes = new Dictionary<int, uint>();
        private Dictionary<int, ManagedMemoryBlock[]> memory = new Dictionary<int, ManagedMemoryBlock[]>();
        public int initializeMemory(int pid, int blocks, uint blockSize)
        {
 
            blockSizes[pid] = blockSize;
            memory[pid] = new ManagedMemoryBlock[blocks];
            return 0;
        }

        public int writeMemory(int pid,int blockIndex, byte[] payload)
        {
            if (!memory.ContainsKey(pid))
            {
                return 1;
            }

            if (memory[pid][blockIndex] == null)
            {
                memory[pid][blockIndex] = new ManagedMemoryBlock(blockSizes[pid]);
            }
            for (int i = 0; i < payload.Length; i++)
            {
                memory[pid][blockIndex].memory = payload;
            }
            return 0;
        }

        public byte[] readMemory(int pid, int blockIndex)
        {
            if (!memory.ContainsKey(pid))
            {
                return null;
            }
            return memory[pid][blockIndex].memory;
        }
    }
}
