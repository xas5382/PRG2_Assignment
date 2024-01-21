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

        public void ModifyIceCream(int index)
        {
            IceCream iceCreamToChange = iceCreamList[index - 1];
            for (int i = 0; i < iceCreamList.Count; i++) 
            {
                iceCreamToChange = iceCreamList[index - 1]; 
            }
        }

        public void AddIceCream(IceCream iceCream)
        {
            IceCreamList.Add(iceCream);
        }

        public void DeleteIceCream(IceCream iceCream)
        {
            IceCreamList.Remove(iceCream);
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


            foreach (IceCream iceCream in IceCreamList)
            {
                orderInfo.Append($"{iceCream}");
            }

            return orderInfo.ToString();
        }
    }
}
