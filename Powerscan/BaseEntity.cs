using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Powerscan
{
    public class BaseEntity
    {
        private static int _nextId = 1;

        public int EntryId { get; set; }

        public BaseEntity()
        {
           // EntryId = _nextId++;
        }
        
        public static int GetNextId()
        {
            return _nextId++;
        }
        public static void SetNextId(int nextId)
        {
           _nextId = nextId;
        }
    }
}
