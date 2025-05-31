using System;
using System.Collections.Generic;

namespace Classmates_RPG_Battle_Simulator.Models
{
    // This class represents the 'Ezekiel' character, inheriting from the base Character class.
    // It demonstrates inheritance and overrides base class methods for specific behavior.
    public class CarlEzekiel : Character
    {
        // Constructor for CarlEzekiel.
        // It calls the base class constructor using 'base()' to set the common character properties.
        public CarlEzekiel(string name) : base(name, "Ezekiel", 100, 10, false)
        {
        }

        // Overrides the base Attack method to implement Ezekiel's unique attack logic.
        // This method includes random selection of attack types and a chance for a critical hit,
        // demonstrating polymorphism and specific class behavior.
        public override Tuple<int, string> Attack()
        {
            // Generate random damage between 15 and 30 (base range)
            int damage = random.Next(15, 31);
            string attackName = GetRandomAttackName();

            // 35% chance for Critical Hit (40% more damage)
            if (random.Next(100) < 35)
            {
                damage = (int)(damage * 1.4f);
                attackName += " (Critical Hit)"; // Indicate critical hit in the name
            }

            return new Tuple<int, string>(damage, attackName);
        }

        // Overrides the base TakeDamage method to apply damage reduction specific to Ezekiel.
        // This demonstrates how derived classes can modify inherited behavior.
        public override void TakeDamage(int damage)
        {
            // Reduce damage due to strong defense
            damage = (int)(damage * 0.7f);
            base.TakeDamage(damage);
        }

        // Overrides the base class method to provide specific attack names for the Ezekiel character.
        protected override List<string> GetAttackNames()
        {
            return new List<string>()
            {
                "Block Connection",
                "Filter Packet",
                "Deny Access"
            };
        }
    }
} 