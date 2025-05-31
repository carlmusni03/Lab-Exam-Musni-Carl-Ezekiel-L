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