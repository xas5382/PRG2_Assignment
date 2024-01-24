//==========================================================
// Student Number : S10257400
// Student Name : See Wai Kee, Audrey
//==========================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S10257400_PRG2Assignment
{
    class Topping
    {
        private string type;

        public string Type 
        {
            get { return type; }
            set { type = value; } 
        }

        public Topping() { }

        public Topping(string type) 
        {
            Type = type;
        }

        public override string ToString() 
        {
            return Type;
        }
    }
}
