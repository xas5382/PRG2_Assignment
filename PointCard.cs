//==========================================================
// Student Number : S10257400
// Student Name : See Wai Kee, Audrey
//==========================================================

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace S10257400_PRG2Assignment
{
    class PointCard
    {
        private int points;
        private int punchCard;
        private string tier;

        public int Points
        { 
            get { return points; } 
            set { points = value; }
        }

        public int PunchCard
        {
            get { return punchCard; }
            set { punchCard = value; }
        }

        public string Tier
        { 
            get { return tier; } 
            set { tier = value; } 
        }

        public PointCard() { }

        public PointCard(int points, int punchCard) 
        {
            Points = points;
            PunchCard = punchCard;
            Tier = "Ordinary";
        }

        public void AddPoints(int amountSpent)
        {
            Points += Convert.ToInt32(Math.Floor(amountSpent * 0.72));
            
            if (Points >= 50 && Points < 100)
            {
                Tier = "Silver";
            }
            else if (Points >= 100)
            {
                Tier = "Gold";
            }
        }

        public void RedeemPoints(int redeemedPoints)
        {
            if (Tier != "Ordinary")
            {
                Points -= redeemedPoints;
            }
        }

        public void Punch()
        {
            if (PunchCard == 10)
            {
                Console.WriteLine("Customer has ordered 10 Ice Creams. This is the 11th Ice Cream.");
                PunchCard = 0;
            }
            else
            {
                PunchCard++;
            }
        }

        public override string ToString()
        {
            if (PunchCard == 10)
            {
                return ($"Punch card has been completed. Next Ice Cream ordered will be free.");
            }
            else
            {
                return ($"Customer is {10 - PunchCard} ice creams away from a free Ice Cream");
            }   
        }
    }
}
