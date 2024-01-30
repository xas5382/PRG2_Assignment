﻿//==========================================================
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
            CreateCustomerOrderHistory(customerDict);

            Queue<Order> regularQueue = new Queue<Order>();
            Queue<Order> goldQueue = new Queue<Order>();

            while (correctFile)
            {
                int choice = DisplayMenu();

                if (choice == 1)   // display all customers
                {
                    DisplayCustomers(customerDict);
                    Console.WriteLine();
                }
                else if (choice == 2)   // list all the current orders
                {
                    ListCurrentOrders(regularQueue, goldQueue);
                }
                else if (choice == 3)   // create a new order
                {
                    DisplayCustomers(customerDict);
                    Customer customer = SelectCustomer(customerDict);

                    if (customer == null)
                    {
                        continue;
                    }
                    if (customer.CurrentOrder != null)
                    {
                        Console.WriteLine("\n" + "Customer currently has one current order.");
                        Console.WriteLine("Complete that order before adding another order or modify the existing order. \n");
                        continue;
                    }

                    Console.WriteLine();
                    Order customerCurrentOrder = customer.MakeOrder();

                    while (true)   // whil loop to add a another ice cream to the current order and ensure user input is validated
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
                            IceCream additionalIceCream = CreateCustomerOrder();
                            customerCurrentOrder.AddIceCream(additionalIceCream);
                            continue;
                        }
                        else
                        {
                            Console.WriteLine("Please reply with either \"Y\" or \"N\".");
                        }
                    }

                    customer.CurrentOrder = customerCurrentOrder;

                    //  place the customer's order in one of the 2 queue systems (Gold/Normal) depending on their memebership tier
                    if (customer.Rewards.Tier == "Gold")
                    {
                        customerCurrentOrder.Id = goldQueue.Count() + 1;
                        goldQueue.Enqueue(customerCurrentOrder);
                    }
                    else
                    {
                        customerCurrentOrder.Id = regularQueue.Count() + 1;
                        regularQueue.Enqueue(customerCurrentOrder);
                    }

                    Console.WriteLine("\n" + "Order has been made successfully" + "\n");

                }
                else if (choice == 4)
                {
                    DisplayCustomers(customerDict);
                    Customer customer = SelectCustomer(customerDict);
                    
                    if (customer == null)
                    {
                        continue;
                    }

                    if (customer.CurrentOrder != null)   // ensure customer has a current order before proceeding to prevent error
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
                        
                        if (optionChosen == 1)   // customer wants to modify an IceCream in current order
                        {
                            int iceCreamToChangeIndex = GetIceCreamToChange(customerOrder);
                            Console.WriteLine();
                            customerOrder.ModifyIceCream(iceCreamToChangeIndex);
                        }
                        else if (optionChosen == 2)   // customer wants to add an IceCream to current order
                        {
                            Order currentOrder = customer.CurrentOrder;
                            IceCream iceCream = CreateCustomerOrder();
                            currentOrder.AddIceCream(iceCream);
                            Console.WriteLine("\n" + "Order has been made successfully");
                        }
                        else if (optionChosen == 3)   // customer wants to remove an IceCream from current order
                        {
                            // remove ice cream from current order only if there is more than 1 ice cream
                            if (customerOrder.IceCreamList.Count() == 1)   
                            {
                                Console.WriteLine("You cannot have zero ice creams order in your order");
                            }
                            else
                            {
                                int iceCreamIndex = GetIceCreamToChange(customerOrder) - 1;
                                customerOrder.DeleteIceCream(iceCreamIndex);
                                Console.WriteLine("\n" + "Order has been successfully deleted");
                            }
                        }
                        else
                        {
                            continue;
                        }
                    }
                    else
                    {
                        Console.WriteLine("\n" + "Customer has no current orders. Create an order before using this option.");
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

        static void CreateCustomerOrderHistory(Dictionary<int, Customer> customerDict)
        {
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

                // foreach loop to check if the order in which an ice cream object belongs alreadt exits in the customer's order history
                // check using the attribute TimeRecieved as that attribute is the most unique
                // if order exists, add ice cream object to the back of the cutomer's order history
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

                // if order does not exist, create a new completed order and add ice cream object to that order before adding the completed
                // order to the back of the cutomer's order history
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
                    Console.WriteLine("Invalid option. Please enter a number from 0 to 5 as seen in the menu.");
                    continue;
                }

                if (choice >= 0 && choice <= 5)
                {
                    return choice;
                }
                else
                {
                    Console.WriteLine("Invalid option. Please enter a number from 0 to 5 as seen in the menu.");
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
                Console.WriteLine("\n" + "Queue System for Gold Member(s)" + "\n" + "-------------------------------");

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
            while (true)   // while loop to validate the customer's choice of member ID
            {
                Console.Write("Enter the member ID: ");

                try
                {
                    memberID = Convert.ToInt32(Console.ReadLine());

                    if (customerDict.TryGetValue(memberID, out Customer customer))
                    {
                        return customer;   // Customer found as member ID exists
                    }
                    else
                    {
                        Console.WriteLine("\n" + $"Customer with member ID {memberID} does not exist. Please add the customer first." + "\n");
                        return null;   // Customer not found as member ID does not exist
                    }
                }
                catch (FormatException)
                {
                    Console.WriteLine("Invalid input. Please enter a valid member ID. \n");
                }
            }
        }

        static IceCream CreateCustomerOrder()
        {
            string typeOfIceCream = GetIceCreamType();
            int scoopsOfIceCream = 0;

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

            iceCream.ModifyIceCreamScoops();
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

                if (iceCreamOption == "1" || iceCreamOption.ToLower() == "cup")
                {
                    return "Cup";
                }
                else if (iceCreamOption == "2" || iceCreamOption.ToLower() == "cone")
                {
                    return "Cone";
                }
                else if (iceCreamOption == "3" || iceCreamOption.ToLower() == "waffle")
                {
                    return "Waffle";
                }
                else
                {
                    Console.WriteLine("Please enter an available option that is shown above.");
                }
            }
        }

        static int OrderModificationMenu()
        {
            Console.WriteLine("Options to Modify your Order");
            Console.WriteLine("[1] Choose an existing ice cream object to modify \n" +
                "[2] Add an entirely new ice cream object to the order \n" +
                "[3] Choose an existing ice cream object to delete from the order \n" +
                "[0] Exit \n");

            int choice;

            while (true)        // while loop to ensure user input is an integer between 0 and 3
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

        // This method does not take into consideration the points used, if any, when he/she is making payment for his/her order
        // as they is no way for me to know how many points are used, if any, seeing as how I am doing solo and I am not implementing
        // advanced option (a).
        // However, the free ice cream give on a customer birthday will be calculated and the cost of that ice cream will be deducted
        // from the total price of that customer's order.
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
                catch (FormatException)
                {
                    Console.WriteLine("Please enter a valid year");
                }
            }

            Console.WriteLine();
            double yearlyProfits = 0;
            double priceFreeIceCream = 0;

            foreach (KeyValuePair<int, Customer> kvp in customerDict)
            {
                List<Order> ordersInYear = new List<Order>();

                // determine which order was completed in the user's choice of year
                // append that order to orderInYears
                foreach (Order order in kvp.Value.OrderHistory)
                {
                    DateTime timeFulfilled = Convert.ToDateTime(order.TimeFulfilled);

                    if (timeFulfilled.Year == year)
                    {
                        ordersInYear.Add(order);
                    }
                }

                // calculate the total cost for each completed order in ordersPerYear
                foreach (Order order in ordersInYear)
                {
                    string month = Convert.ToDateTime(order.TimeFulfilled).ToString("MMM");
                    double orderCost = order.CalculateTotal();

                    // if the customer placed an order on his brithday, the most expensive ice cream in his first order of the day will be free
                    if (order.TimeReceived.ToString("dd/MMMM") == kvp.Value.Dob.ToString("dd/MMMM") && order == ordersInYear[0])
                    {
                        foreach (IceCream iceCream in ordersInYear[0].IceCreamList)
                        {
                            // determine the price of the most expensive ice cream
                            if (priceFreeIceCream < iceCream.CalculatePrice())
                            {
                                priceFreeIceCream = iceCream.CalculatePrice();
                            }
                        }

                        // minus off the cost of the expensive ice cream in the customer's first order of the day
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