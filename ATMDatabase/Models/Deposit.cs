using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATMDatabase
{
    public class Deposit
    {
        public int Id { get; set; }
        public double Amount { get; set; }
        public DateTime Time { get; set; }

        public virtual User User { get; set; }
    }
}
