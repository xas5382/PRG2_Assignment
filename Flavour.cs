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
    class Flavour
    {
        private string type;
        private bool premium;
        private int quantity;

        public string Type
        { 
            get { return type; } 
            set { type = value; }
        }

        public bool Premium
        {
            get { return premium; }
            set { premium = value; }
        }

        public int Quantity
        {
            get { return quantity; }
            set { quantity = value; }
        }

        public Flavour() { }

        public Flavour(string type, bool premium, int quantity) 
        {
            Type = type;
            Premium = premium;
            Quantity = quantity;
        }

        public override string ToString() 
        {
            return ($"{Type,-13} {Premium, -8} {Quantity}");
        }
    }
}
