using System;
using System.Collections.Generic;

namespace Classmates_RPG_Battle_Simulator.Models
{
    // This class represents the 'Rozen' character, inheriting from the base Character class.
    // It demonstrates inheritance by specializing the generic Character behavior.
    public class KeithRozen : Character
    {
        // Constructor for KeithRozen.
        // It calls the base class constructor using 'base()' to set the common character properties
        // like name, class name, max health, speed, and projectile usage.
        public KeithRozen(string name) : base(name, "Rozen", 100, 10, false)
        {
        }

        // Overrides the base class method to provide specific attack names for the Rozen character.
        // This is an example of polymorphism, where a derived class provides its own
        // implementation of a base class method.
        protected override List<string> GetAttackNames()
        {
            return new List<string>()
            {
                "Swat Bug",
                "Fix Glitch",
                "Optimize Code"
            };
        }
    }
} 