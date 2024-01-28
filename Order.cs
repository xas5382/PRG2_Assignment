//==========================================================
// Student Number : S10257400
// Student Name : See Wai Kee, Audrey
//==========================================================

using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S10257400_PRG2Assignment
{
    class Order
    {
        private int id;
        private DateTime timeReceived;
        private DateTime? timeFulfilled;
        private List<IceCream> iceCreamList;

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public DateTime TimeReceived
        {
            get { return timeReceived; }
            set { timeReceived = value; }
        }

        public DateTime? TimeFulfilled
        {
            get { return timeFulfilled; }
            set { timeFulfilled = value; }
        }

        public List<IceCream> IceCreamList
        {
            get { return iceCreamList; }
            set { iceCreamList = value; }
        }

        public Order() { }

        public Order(int id, DateTime timeReceived)
        {
            Id = id;
            TimeReceived = timeReceived;
            IceCreamList = new List<IceCream>();
        }

        public void ModifyIceCream(int index)   // method to determine how to modify ice cream object, not found in the class diagram
        {
            IceCream iceCreamToChange = iceCreamList[index - 1];
            int modificationChoice = IceCreamModificationMenu(iceCreamToChange);

            if (modificationChoice == 1)
            {
                IceCream newIceCream = ModifyIceCreamType(iceCreamToChange);
                DeleteIceCream(index - 1);
                AddIceCream(newIceCream);
            }
            else if (modificationChoice == 2)
            {
                ModifyIceCreamScoops(iceCreamToChange);
                iceCreamToChange.ModifyIceCreamFlavours();
            }
            else if (modificationChoice == 3)
            {
                iceCreamToChange.ModifyIceCreamFlavours();
            }
            else if (modificationChoice == 4)
            {
                iceCreamToChange.ModifyIceCreamToppings();
            }
            else if (modificationChoice == 5)
            {
                Cone cone = (Cone)iceCreamToChange;
                cone.ModifyConeFlavour();
            }
            else if (modificationChoice == 6)
            {
                Waffle waffle = (Waffle)iceCreamToChange;
                waffle.ModifyWaffleFlavour();
            }
        }

        public int IceCreamModificationMenu(IceCream iceCreamToChange)   // method to display types of modifications user can make to their ice cream object, not found in the class diagram
        {
            List<string> menuList = new List<string> { "Type of Ice Cream", "Number of Scoops", "Flavour of Ice Cream", 
                "Toppings chosen", "Change the cone flavour", "Change the waffle flavour" };
            Console.WriteLine("Parts of Ice Cream that can be changed");

            if (iceCreamToChange is Cup)
            {
                menuList.RemoveAt(5);
                menuList.RemoveAt(4);
            }
            if (iceCreamToChange is Cone)
            {
                menuList.RemoveAt(5);
            }
            if (iceCreamToChange is Waffle)
            {
                menuList.RemoveAt(4);
            }

            for (int i = 0; i < menuList.Count; i++)
            {
                Console.WriteLine($"[{i+1}] {menuList[i]}");
            }
            Console.WriteLine($"[0] Exit");
            Console.WriteLine();

            int choice;
            while (true)        // Loop to ensure user input is an integer between 0 and and the total number of options in menuList to prevent an error from occuring
            {
                Console.Write("Enter your option: ");

                try
                {
                    choice = Convert.ToInt32(Console.ReadLine());
                }
                catch (FormatException)
                {
                    Console.WriteLine($"Invalid option. Please enter an option from 0 to {menuList.Count()} as seen in above the menu.");
                    continue;
                }

                if (choice >= 0 && choice <= menuList.Count())
                {
                    return choice;
                }
                else
                {
                    Console.WriteLine($"Invalid option. Please enter an option from 0 to {menuList.Count()} as seen in above the menu.");
                }
            }
        }

        public IceCream ModifyIceCreamType(IceCream iceCreamToChange)
        {
            List<string> iceCreamTypeList = new List<string> { "Cup", "Cone", "Waffle" };

            int x = 1;
            iceCreamTypeList.Remove(iceCreamToChange.Option);
            Console.WriteLine("\n" + "Types of Ice Cream you can change to");
            foreach (string iceCreamOption in iceCreamTypeList)
            {
                Console.WriteLine($"[{x}] {iceCreamOption}");
                x++;
            }

            Console.WriteLine();

            while (true)
            {
                Console.Write("What do you want to change it to? ");
                string newIceCream = Console.ReadLine();

                if ((newIceCream == "1" && iceCreamTypeList[0] == "Cup") || (newIceCream == "2" && iceCreamTypeList[1] == "Cup") || newIceCream == "cup" || newIceCream == "Cup")
                {
                    IceCream iceCream = new Cup("Cup", iceCreamToChange.Scoops, iceCreamToChange.Flavours, iceCreamToChange.Toppings);
                    return iceCream;
                }
                else if ((newIceCream == "1" && iceCreamTypeList[0] == "Cone") || (newIceCream == "2" && iceCreamTypeList[1] == "Cone") || newIceCream == "cone" || newIceCream == "Cone")
                {
                    IceCream iceCream = new Cone("Cone", iceCreamToChange.Scoops, iceCreamToChange.Flavours, iceCreamToChange.Toppings, false);
                    Cone cone = (Cone)iceCream;
                    cone.ModifyConeFlavour();
                    return iceCream;
                }
                else if ((newIceCream == "1" && iceCreamTypeList[0] == "Waffle") || (newIceCream == "2" && iceCreamTypeList[1] == "Waffle") || newIceCream == "waffle" || newIceCream == "Waffle")
                {
                    IceCream iceCream = new Waffle("Waffle", iceCreamToChange.Scoops, iceCreamToChange.Flavours, iceCreamToChange.Toppings, "null");
                    Waffle waffle = (Waffle)iceCream;
                    waffle.ModifyWaffleFlavour();
                    return iceCream;
                }
                else
                {
                    Console.WriteLine("Please enter a choice from the list shown above.");
                    continue;
                }
            }
        }

        public void ModifyIceCreamScoops(IceCream iceCreamToChange)
        {
            List<int> availableOption = new List<int> { 1, 2, 3 };
            availableOption.Remove(iceCreamToChange.Scoops);

            int numIceCreamScoops;
            while (true)
            {
                Console.Write("\n" + "Enter the updated number of Ice Cream scoops: ");

                try
                {
                    numIceCreamScoops = Convert.ToInt32(Console.ReadLine());
                    
                    if (availableOption.Contains(numIceCreamScoops))
                    {
                        break;
                    }
                    else if (numIceCreamScoops == iceCreamToChange.Scoops)
                    {
                        Console.WriteLine($"Please do not enter the previously entered quantity of {iceCreamToChange.Scoops} sccops of Ice Cream");
                    }
                    else
                    {
                        Console.WriteLine($"Please enter a quantity that is either {availableOption[0]} scoop(s) or {availableOption[1]} scoop(s)");
                    }
                }
                catch
                {
                    Console.WriteLine($"Please enter a quantity that is either {availableOption[0]} scoop(s) or {availableOption[1]} scoop(s)");
                }
            }

            

            iceCreamToChange.Scoops = numIceCreamScoops;
        }
        
        public void AddIceCream(IceCream iceCream)
        {
            IceCreamList.Add(iceCream);
        }

        public void DeleteIceCream(int index)
        {
            IceCreamList.RemoveAt(index);
        }

        public double CalculateTotal()
        {
            double total = 0;

            foreach (IceCream iceCream in IceCreamList)
            {
                total += iceCream.CalculatePrice();
            }

            return total;
        }

        public override string ToString()
        {
            StringBuilder orderInfo = new StringBuilder();

            orderInfo.Append($"Order ID: {Id:D2} \n");
            orderInfo.Append($"Timestamp: {TimeReceived.ToString("G")} \n");
            
            if (TimeFulfilled != null)
            {
                orderInfo.Append($"Time Fulfilled: {TimeReceived.ToString("G")} \n");
            }

            foreach (IceCream iceCream in IceCreamList)
            {
                orderInfo.Append($"{iceCream}");
            }

            return orderInfo.ToString();
        }
    }
}

/*
                if (iceCreamToChange is Cone)
                {
                    
                }
                else
                {
                    Console.WriteLine("\n" + "Ice Cream chosen in not a Cone Ice Cream" + "\n" + "Please choose a Cone Ice Cream");
                }
 */