using System;
using System.Collections.Generic;

namespace Classmates_RPG_Battle_Simulator.Models
{
    // This class represents the 'E-James' character, inheriting from the base Character class.
    // It demonstrates inheritance and overrides base class methods for specific behavior.
    public class EdwardJames : Character
    {
        // Constructor for EdwardJames.
        // It calls the base class constructor using 'base()' to set the common character properties.
        public EdwardJames(string name) : base(name, "E-James", 100, 10, true)
        {
        }

        // Overrides the base Attack method to implement Edward's unique attack logic.
        // This method includes random selection of attack types and a chance for a critical hit.
        public override Tuple<int, string> Attack()
        {
            // Generate random damage between 15 and 30 (base range)
            int damage = random.Next(15, 31);
            string attackName = GetRandomAttackName();
            
            // 18% chance for Critical Hit (80% more damage)
            if (random.Next(100) < 18)
            {
                damage = (int)(damage * 1.8f);
                attackName += " (Critical Hit)"; // Indicate critical hit in the name
            }

            return new Tuple<int, string>(damage, attackName);
        }

        // Overrides the base TakeDamage method.
        // Currently, it just calls the base class implementation, meaning Edward takes normal damage.
        public override void TakeDamage(int damage)
        {
            // Normal damage taken
            base.TakeDamage(damage);
        }

        // Overrides the base class method to provide specific attack names for the E-James character.
        protected override List<string> GetAttackNames()
        {
            return new List<string>()
            {
                "Trigger Exception",
                "Syntax Error",
                "System Crash"
            };
        }
    }
} 