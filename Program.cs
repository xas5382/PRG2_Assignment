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
    class Program
    {
        static void Main(string[] args)
        {
            Dictionary<int, Customer> customerDict = new Dictionary<int, Customer>();
            bool correctFile = CreateCustomers(customerDict);
            //AddCompletedOrders();
            CreateCustomerOrderHistory(customerDict);

            Queue<Order> regularQueue = new Queue<Order>();
            Queue<Order> goldQueue = new Queue<Order>();

            List<string> iceCreamFlavourList = CreateIceCreaFlavourList();
            List<string> toppingList = CreateToppingList();

            while (correctFile)
            {
                int choice = DisplayMenu();

                if (choice == 1)
                {
                    DisplayCustomers(customerDict);
                    Console.WriteLine();
                }
                else if (choice == 2)
                {
                    ListCurrentOrders(regularQueue, goldQueue);
                }
                else if (choice == 3)
                {
                    DisplayCustomers(customerDict);
                    Customer customer = SelectCustomer(customerDict);

                    if (customer == null)
                    {
                        continue;
                    }
                    if (customer.CurrentOrder != null)
                    {
                        Console.WriteLine("\n" + "Customer currently has one current order. Complete that order before adding another order. \n");
                        continue;
                    }
                    Order newOrder = new Order(regularQueue.Count() + 1, DateTime.Now);
                    Console.WriteLine();

                    if (customer != null)
                    {
                        IceCream iceCream = CreateCustomerOrder(iceCreamFlavourList, toppingList);
                        newOrder.AddIceCream(iceCream);

                        while (true)
                        {
                            Console.Write("\n" + "Would you like to add another ice cream to the order [Y/N]? ");
                            string addIceCream = Console.ReadLine();

                            if (addIceCream.ToLower() == "n")
                            {
                                break;
                            }
                            else if (addIceCream.ToLower() == "y")
                            {
                                Console.WriteLine();
                                IceCream additionalIceCream = CreateCustomerOrder(iceCreamFlavourList, toppingList);
                                newOrder.AddIceCream(additionalIceCream);
                                continue;
                            }
                            else
                            {
                                Console.WriteLine("Please reply with either \"Y\" or \"N\".");
                            }
                        }

                        customer.CurrentOrder = newOrder;
                        
                        if (customer.Rewards.Tier == "Gold")
                        {
                            newOrder.Id = goldQueue.Count() + 1;
                            goldQueue.Enqueue(newOrder);
                        }
                        else
                        {
                            regularQueue.Enqueue(newOrder);
                        }

                        Console.WriteLine("\n" + "Order has been made successfully");
                    }

                    Console.WriteLine();
                }
                else if (choice == 4)
                {
                    DisplayCustomers(customerDict);
                    Customer customer = SelectCustomer(customerDict);

                    if (customer.CurrentOrder != null)
                    {
                        Order customerOrder = customer.CurrentOrder;
                        
                        // display the customer's current order
                        Console.WriteLine("\n" + $"Ice Creams ordered by {customer.Name}" + "\n" + "----------------------");
                        int count = 1;
                        foreach (IceCream iceCream in customerOrder.IceCreamList)
                        {
                            Console.Write($"[{count}] {iceCream}");
                            count++;
                        }
                        Console.WriteLine();

                        int optionChosen = OrderModificationMenu();
                        Console.WriteLine();
                        
                        if (optionChosen == 1)
                        {
                            int iceCreamToChangeIndex = GetIceCreamToChange(customerOrder);
                            Console.WriteLine();
                            customerOrder.ModifyIceCream(iceCreamToChangeIndex);
                        }
                        else if (optionChosen == 2)
                        {
                            Console.WriteLine();
                            Order currentOrder = customer.CurrentOrder;
                            IceCream iceCream = CreateCustomerOrder(iceCreamFlavourList, toppingList);
                            currentOrder.AddIceCream(iceCream);
                            Console.WriteLine("Order has been made successfully");
                        }
                        else if (optionChosen == 3)
                        {
                            if (customerOrder.IceCreamList.Count() == 1)
                            {
                                Console.WriteLine("You cannot have zero ice creams order in your order");
                            }
                            else
                            {
                                int iceCreamIndex = GetIceCreamToChange(customerOrder) - 1;
                                customerOrder.DeleteIceCream(iceCreamIndex);
                            }
                        }
                        else
                        {
                            continue;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Customer has no current orders. Create an order before using this option.");
                    }

                    Console.WriteLine();
                }
                else if (choice == 5)
                {
                    DisplayAmountSpent(customerDict);
                    Console.WriteLine();
                }
                else
                {
                    Console.WriteLine("\n" + "Goodbye!");
                    break;
                }
            }
        }

        static bool CreateCustomers(Dictionary<int, Customer> customerDict)
        {
            string[] csvLines = File.ReadAllLines("customers.csv");
            string firstLine = csvLines[0];

            bool correctFile = false;

            if (firstLine.Contains("Name") && firstLine.Contains("MemberId") && firstLine.Contains("DOB"))
            {
                correctFile = true;
            }
            if (!correctFile)
            {
                Console.WriteLine("Please check the provided file to ensure that it contains the required information.");
            }

            for (int i = 1; i < csvLines.Length; i++)
            {
                string[] info = csvLines[i].Split(',');

                int memberID = Convert.ToInt32(info[1]);
                DateTime dob = Convert.ToDateTime(info[2]);

                Customer customer = new Customer(info[0], memberID, dob);
                customer.Rewards.Tier = info[3];
                customer.Rewards.Points = Convert.ToInt32(info[4]);
                customer.Rewards.PunchCard = Convert.ToInt32(info[5]);

                customerDict.Add(memberID, customer);
            }

            return correctFile;
        }

        static List<string> CreateIceCreaFlavourList()
        {
            List<string> iceCreamFlavourList = new List<string>();

            string[] csvLines = File.ReadAllLines("flavours.csv");

            for (int i = 1; i < csvLines.Length; i++)
            { 
                iceCreamFlavourList.Add(csvLines[i]);
            }

            return iceCreamFlavourList;
        }

        static List<string> CreateToppingList()
        {
            List<string> toppingList = new List<string>();

            string[] csvLines = File.ReadAllLines("toppings.csv");

            for (int i = 1; i < csvLines.Length; i++)
            {
                toppingList.Add(csvLines[i]);
            }

            return toppingList;
        }

        static void CreateCustomerOrderHistory(Dictionary<int, Customer> customerDict)
        {
            Dictionary<DateTime, Order> completedOrderDict = new Dictionary<DateTime, Order>();

            string[] csvLines = File.ReadAllLines("orders.csv");

            for (int i = 1; i < csvLines.Length; i++)
            {
                string[] info = csvLines[i].Split(',');

                int orderID = Convert.ToInt32(info[0]);
                int memberID = Convert.ToInt32(info[1]);
                DateTime timeReceived = Convert.ToDateTime(info[2]);
                DateTime timeFulfilled = Convert.ToDateTime(info[3]);
                string option = info[4];
                int numOfScoops = Convert.ToInt32(info[5]);
                string waffleFlavour = info[7];

                List<Flavour> flavourList = new List<Flavour>();
                for (int x = 8; x < 11; x++)
                {
                    if (info[x] != "")
                    {
                        if (info[x] == "Durian" || info[x] == "Ube" || info[x] == "Sea Salt")
                        {
                            Flavour flavour = new Flavour(info[x], true);
                            flavourList.Add(flavour);
                        }
                        else
                        {
                            Flavour flavour = new Flavour(info[x], false);
                            flavourList.Add(flavour);
                        }
                    }
                }

                List<Topping> toppingList = new List<Topping>();
                for (int x = 11; x < 15; x++)
                {
                    if (info[x] != "")
                    {
                        Topping topping = new Topping(info[x]);
                        toppingList.Add(topping);
                    }
                }

                IceCream iceCream = null;
                if (info[6] == "TRUE" || info[6] == "FALSE")
                {
                    iceCream = new Cone(option, numOfScoops, flavourList, toppingList, Convert.ToBoolean(info[6]));
                }

                if (waffleFlavour != "")
                {
                    iceCream = new Waffle(option, numOfScoops, flavourList, toppingList, waffleFlavour);
                }

                if (info[6] == "" && waffleFlavour == "")
                {
                    iceCream = new Cup(option, numOfScoops, flavourList, toppingList);
                }
                bool addIceCream = false;
                foreach (KeyValuePair<int, Customer> kvp in customerDict)
                {
                    foreach (Order order in kvp.Value.OrderHistory)
                    {
                        if (order.TimeReceived.ToString("g") == timeReceived.ToString("g"))
                        {
                            order.AddIceCream(iceCream);
                            addIceCream = true;
                            break;
                        }
                    }
                }

                if (!addIceCream && customerDict.TryGetValue(memberID, out Customer customer))
                {
                    Order pastOrder = new Order(orderID, timeReceived);
                    pastOrder.TimeFulfilled = timeFulfilled;
                    pastOrder.AddIceCream(iceCream);
                    customer.OrderHistory.Add(pastOrder);
                }
            }
        }

        static int DisplayMenu()
        {
            Console.WriteLine("---------------- M E N U -----------------");
            Console.WriteLine("[1] List all customers");
            Console.WriteLine("[2] List all current orders");
            Console.WriteLine("[3] Create a customer’s order");
            Console.WriteLine("[4] Modify order details");
            Console.WriteLine("[5] Display monthly charged amounts breakdown & total charged amounts for the year");
            Console.WriteLine("[0] Exit");
            Console.WriteLine("------------------------------------------");

            int choice;

            while (true)        // Loop to ensure user input is an integer between 0 and 5
            {
                Console.Write("Enter your option: ");

                try
                {
                    choice = Convert.ToInt32(Console.ReadLine());
                }
                catch (FormatException)
                {
                    Console.WriteLine("Invalid option. Please enter a number from the menu.");
                    continue;
                }

                if (choice >= 0 && choice <= 5)
                {
                    return choice;
                }
                else
                {
                    Console.WriteLine("Invalid option. Please enter a number from the menu.");
                }
            }
        }

        static void DisplayCustomers(Dictionary<int, Customer> customerDict)
        {
            Console.WriteLine("\n" + "Display Customer Information" + "\n" + "----------------------------");
            Console.WriteLine("{0,-11} {1,-12} {2,-13} {3,-11} {4,-9} {5}", "Name", "Member ID", "DOB", "Status", "Points", "Punch Card");

            foreach (KeyValuePair<int, Customer> kvp in customerDict)
            {
                Console.WriteLine(kvp.Value);
            }
        }

        static void ListCurrentOrders(Queue<Order> regularQueue, Queue<Order> goldQueue)
        {
            if (goldQueue.Count == 0)
            {
                Console.WriteLine("\n" + "There are no orders in the gold members queue." + "\n");
            }
            else
            {
                Console.WriteLine("Queue System for Gold Member(s)" + "\n" + "-------------------------------");

                foreach (Order order in goldQueue)
                {
                    Console.WriteLine(order);
                }
            }

            if (regularQueue.Count == 0)
            {
                Console.WriteLine("There are no orders in the regular members queue." + "\n");
            }
            else
            {
                Console.WriteLine("Queue System for Sliver and Ordinary Member(s)" + "\n" + "-------------------------------");

                foreach (Order order in regularQueue)
                {
                    Console.WriteLine(order);
                }
            }
        }

        static Customer SelectCustomer(Dictionary<int, Customer> customerDict)
        {
            Console.WriteLine();

            int memberID;
            while (true)
            {
                Console.Write("Enter the member ID: ");

                try
                {
                    memberID = Convert.ToInt32(Console.ReadLine());

                    if (customerDict.TryGetValue(memberID, out Customer customer))
                    {
                        return customer; // Customer found
                    }
                    else
                    {
                        Console.WriteLine($"Customer with member ID {memberID} does not exist. Please add the customer first.");
                        return null; // Customer not found
                    }
                }
                catch (FormatException)
                {
                    Console.WriteLine("Invalid input. Please enter a valid member ID.");
                }
            }
        }

        static IceCream CreateCustomerOrder(List<string> iceCreamFlavourList, List<string> toppingList)
        {
            string typeOfIceCream = GetIceCreamType();
            int scoopsOfIceCream = GetNumScoopsOfIceCream();

            List<Flavour> customerFlavourList = new List<Flavour>();
            List<Topping> customerToppingList = new List<Topping>();

            IceCream iceCream = null;
            if (typeOfIceCream.ToLower() == "cup")
            {
                iceCream = new Cup(typeOfIceCream, scoopsOfIceCream, customerFlavourList, customerToppingList);
            }
            else if (typeOfIceCream.ToLower() == "cone")
            {
                iceCream = new Cone(typeOfIceCream, scoopsOfIceCream, customerFlavourList, customerToppingList, false);
                Cone cone = (Cone)iceCream;
                cone.ModifyConeFlavour();
            }
            else
            {
                iceCream = new Waffle(typeOfIceCream, scoopsOfIceCream, customerFlavourList, customerToppingList, "");
                Waffle waffle = (Waffle)iceCream;
                waffle.ModifyWaffleFlavour();
            }

            iceCream.ModifyIceCreamFlavours();
            iceCream.ModifyIceCreamToppings();
            return iceCream;
        }

        static string GetIceCreamType()
        {
            Console.WriteLine("Types of Ice Cream Available");
            Console.WriteLine("[1] Cup \n" + "[2] Cone \n" + "[3] Waffle" + "\n");

            string iceCreamOption;
            while (true)
            {
                Console.Write("Enter type of ice cream desired: ");
                iceCreamOption = Console.ReadLine();

                if (iceCreamOption == "1")
                {
                    return "Cup";
                }
                else if (iceCreamOption == "2")
                {
                    return "Cone";
                }
                else if (iceCreamOption == "3")
                {
                    return "Waffle";
                }
                else if (iceCreamOption.ToLower() != "cup" && iceCreamOption.ToLower() != "cone" && iceCreamOption.ToLower() != "waffle")
                {
                    Console.WriteLine("Please enter an available option that is shown above.");
                }
                else
                {
                    return (char.ToUpper(iceCreamOption[0]) + iceCreamOption.Substring(1));
                }
            }
        }

        static int GetNumScoopsOfIceCream()
        {
            int scoopsOfIceCream;

            while (true)
            {
                Console.Write("\n" + "How many scoops of Ice Cream do you want? ");
                try
                {
                    scoopsOfIceCream = Convert.ToInt32(Console.ReadLine());

                    if (scoopsOfIceCream <= 0)
                    {
                        Console.WriteLine("You need to order at least 1 scoop of ice cream.");
                    }
                    else if (scoopsOfIceCream > 3)
                    {
                        Console.WriteLine("You can only order at most 3 scoops of ice cream.");
                    }
                    else
                    {
                        break;
                    }
                }
                catch (FormatException)
                {
                    Console.WriteLine("Invalid number of ice cream scoops ordered. Please enter a quantity from 1 to 3.");
                }
            }

            return scoopsOfIceCream;
        }

        static int OrderModificationMenu()
        {
            Console.WriteLine("Options to Modify your Order");
            Console.WriteLine("[1] Choose an existing ice cream object to modify \n" +
                "[2] Add an entirely new ice cream object to the order \n" +
                "[3] Choose an existing ice cream object to delete from the order \n" +
                "[0] Exit \n");

            int choice;

            while (true)        // Loop to ensure user input is an integer between 0 and 3
            {
                Console.Write("Enter your option: ");

                try
                {
                    choice = Convert.ToInt32(Console.ReadLine());
                }
                catch (FormatException)
                {
                    Console.WriteLine("Invalid option. Please enter an option from 0 to 3 as seen in above the menu.");
                    continue;
                }

                if (choice >= 0 && choice <= 3)
                {
                    return choice;
                }
                else
                {
                    Console.WriteLine("Invalid option. Please enter an option from 0 to 3 as seen in above the menu.");
                }
            }
        }

        static int GetIceCreamToChange(Order customerOrder)
        {
            int iceCreamToChangeIndex;
            while (true)
            {
                Console.Write("Which Ice Cream do you want to change? ");
                try
                {
                    iceCreamToChangeIndex = Convert.ToInt32(Console.ReadLine());

                    if (iceCreamToChangeIndex >= 1 && iceCreamToChangeIndex <= customerOrder.IceCreamList.Count())
                    {
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Enter a number that corresponds to the Ice Cream to change");
                    }
                }
                catch (FormatException)
                {
                    Console.WriteLine("Enter a number that corresponds to the Ice Cream to change");
                }
            }

            return iceCreamToChangeIndex;
        }

        static void DisplayAmountSpent(Dictionary<int, Customer> customerDict)  
        {
            Dictionary<string, double> monthlyProfitsDict = new Dictionary<string, double>();

            string[] months = { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };

            
            foreach (string month in months)
            {
                monthlyProfitsDict.Add(month, 0);
            }

            int year;

            while (true)
            {
                Console.Write("Enter the year: ");

                try
                {
                    year = Convert.ToInt32(Console.ReadLine());
                    DateTime current = DateTime.Now;

                    if (year < 2023)
                    {
                        Console.WriteLine("Please enter a year starting from 2023");
                        continue;
                    }
                    else if (year > current.Year)
                    {
                        Console.WriteLine("Please enter the current year or years that came before the current year");
                        continue;
                    }
                    else
                    {
                        break;
                    }
                }
                catch (FormatException ex)
                {
                    Console.WriteLine(ex.Message);
                    Console.WriteLine("Please enter a valid year");
                }
            }

            Console.WriteLine();
            double yearlyProfits = 0;
            double priceFreeIceCream = 0;
            foreach (KeyValuePair<int, Customer> kvp in customerDict)
            {
                List<Order> ordersInYear = new List<Order>();

                foreach (Order order in kvp.Value.OrderHistory)
                {
                    DateTime timeFulfilled = Convert.ToDateTime(order.TimeFulfilled);

                    if (timeFulfilled.Year == year)
                    {
                        ordersInYear.Add(order);
                    }
                }

                foreach (Order order in ordersInYear)
                {
                    string month = Convert.ToDateTime(order.TimeFulfilled).ToString("MMM");
                    double orderCost = order.CalculateTotal();

                    if (order.TimeReceived.ToString("dd/MMMM") == kvp.Value.Dob.ToString("dd/MMMM") && order == ordersInYear[0])
                    {
                        Console.WriteLine(ordersInYear[0]);
                        foreach (IceCream iceCream in ordersInYear[0].IceCreamList)
                        {
                            if (priceFreeIceCream < iceCream.CalculatePrice())
                            {
                                priceFreeIceCream = iceCream.CalculatePrice();
                            }
                        }

                        orderCost -= priceFreeIceCream;
                    }

                    yearlyProfits += orderCost;
                    monthlyProfitsDict[month] += orderCost;
                }
            }
            
            foreach (KeyValuePair<string, double> kvp in monthlyProfitsDict)
            {
                Console.WriteLine($"{kvp.Key} {year}:   ${kvp.Value:F2}");

            }

            Console.WriteLine("\n" + $"Total:   ${yearlyProfits:F2}");
        }
    }
}