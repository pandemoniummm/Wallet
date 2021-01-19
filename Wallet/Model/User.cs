using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Wallet.Model
{
    public class User
    {
        public int Id { get; set; }

        public Dictionary<string, double> Wallet { get; set; }
    }
}
