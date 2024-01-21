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
        }

        public void AddPoints(int addPoints)
        {
            Points += points;
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
                punchCard = 0;
            }
            else
            {
                punchCard++;
            }
        }
    }
}
