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
            // Error attacks: Trigger Exception, Syntax Error, System Crash
            int damage = random.Next(22, 33);
            string attackName = "Trigger Exception"; // Default attack name

            int chosenAttack = random.Next(3); // Randomly choose one of three attacks
            switch(chosenAttack)
            {
                case 0:
                    attackName = "Trigger Exception";
                    damage = random.Next(22, 33); // Base damage for Trigger Exception
                    break;
                case 1:
                    attackName = "Syntax Error";
                    damage = random.Next(20, 30); // Base damage for Syntax Error (slightly less powerful)
                    break;
                case 2:
                    attackName = "System Crash";
                    damage = random.Next(28, 40); // Base damage for System Crash (more powerful)
                    break;
            }
            
            // 18% chance for Critical Hit (System Crash - 80% more damage)
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