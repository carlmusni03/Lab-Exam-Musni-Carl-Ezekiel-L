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