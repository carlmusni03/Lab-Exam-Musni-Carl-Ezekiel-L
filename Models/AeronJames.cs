using System;
using System.Collections.Generic;

namespace Classmates_RPG_Battle_Simulator.Models
{
    // This class represents the 'A-James' character, inheriting from the base Character class.
    // It demonstrates inheritance by specializing the generic Character behavior.
    public class AeronJames : Character
    {
        // Constructor for AeronJames.
        // It calls the base class constructor using 'base()' to set the common character properties.
        public AeronJames(string name) : base(name, "A-James", 100, 10, true)
        {
        }

        // Overrides the base class method to provide specific attack names for the A-James character.
        protected override List<string> GetAttackNames()
        {
            return new List<string>()
            {
                "Inject Malware",
                "Corrupt Data",
                "Spread Infection"
            };
        }
    }
} 