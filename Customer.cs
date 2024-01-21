using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S10257400_PRG2Assignment
{
    class Customer
    {
        private string name;
        private int memberId;
        private DateTime dob;
        private Order currentOrder;
        private List<Order> orderHistory;
        private PointCard rewards;

        public string Name 
        { 
            get { return name; } 
            set {name = value; } 
        }

        public int MemberId
        { 
            get { return memberId; } 
            set { memberId = value;} 
        }

        public DateTime Dob
        { 
            get { return dob; } 
            set { dob = value;} 
        }

        public Order CurrentOrder
        { 
            get { return currentOrder; } 
            set { currentOrder = value; } 
        }

        public List<Order> OrderHistory
        {
            get { return orderHistory; } 
            set { orderHistory = value; } 
        }

        public PointCard Rewards
        { 
            get { return rewards; } 
            set { rewards = value; } 
        }

        public Customer() { }

        public Customer(string name, int memberId, DateTime dob) 
        {
            Name = name;
            MemberId = memberId;
            Dob = dob;
            Rewards = new PointCard();
        }

        public Order MakeOrder()
        {
            CurrentOrder.TimeFulfilled = DateTime.Now;
            OrderHistory.Add(CurrentOrder);
            CurrentOrder = null;
            return OrderHistory.Last();
        }

        public bool isBirthday()
        {
            DateTime currentDate = DateTime.Now;
           
            if (currentDate.ToString("dd/MM/yyyy") == Dob.ToString("dd/MM/yyyy"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public override string ToString()
        {
            return ($"{Name,-11} {MemberId,-12} {Dob.ToString("dd/MM/yyyy"),-13} {Rewards.Tier,-8} {Rewards.Points,9} {Rewards.PunchCard,13}");
        }
    }
}
