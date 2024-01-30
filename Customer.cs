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
            OrderHistory = new List<Order>();
            Rewards = new PointCard();
        }

        public Order MakeOrder()
        {
            Order order = new Order(1, DateTime.Now);

            Console.WriteLine("Types of Ice Cream Available");
            Console.WriteLine("[1] Cup \n" + "[2] Cone \n" + "[3] Waffle" + "\n");

            IceCream iceCream = null;
            string iceCreamOption;
            while (true)
            {
                Console.Write("Enter type of ice cream desired: ");
                iceCreamOption = Console.ReadLine();

                if (iceCreamOption == "1" || iceCreamOption.ToLower() == "cup")
                {
                    iceCream = new Cup("Cup", 0, new List<Flavour>(), new List<Topping>());
                }
                else if (iceCreamOption == "2" || iceCreamOption.ToLower() == "cone")
                {
                    iceCream = new Cone("Cone", 0, new List<Flavour>(), new List<Topping>(), false);
                }
                else if (iceCreamOption == "3" || iceCreamOption.ToLower() == "waffle")
                {
                    iceCream = new Waffle("Waffle", 0, new List<Flavour>(), new List<Topping>(), "");
                }
                else
                {
                    Console.WriteLine("Please enter an available option that is shown above. \n");
                }

                if (iceCream != null)
                {
                    break;
                }
            }

            if (iceCream is Cone)
            {
                Cone cone = (Cone)iceCream;
                cone.ModifyConeFlavour();
            }
            
            if (iceCream is Waffle)
            {
                Waffle waffle = (Waffle)iceCream;
                waffle.ModifyWaffleFlavour();
            }

            iceCream.ModifyIceCreamScoops();
            iceCream.ModifyIceCreamFlavours();
            iceCream.ModifyIceCreamToppings();

            order.AddIceCream(iceCream);

            return order;
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
