﻿using System;
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

        public List<IceCream>IceCreamList
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
            foreach (IceCream iceCream in IceCreamList)
            {

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

        public override string ToString()
        {
            StringBuilder orderInfo = new StringBuilder();

            orderInfo.Append($"{Id,-7:D2} {TimeReceived.ToString("t"),-11}");

            foreach (IceCream iceCream in IceCreamList)
            {
                orderInfo.Append($" {iceCream}");
            }

            return orderInfo.ToString();
        }
    }
}
