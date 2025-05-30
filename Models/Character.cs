using System;
using System.Collections.Generic;

namespace Classmates_RPG_Battle_Simulator.Models
{
    // This abstract class serves as the base for all character types in the game.
    // It encapsulates common properties and behaviors shared by all characters,
    // demonstrating the principle of encapsulation.
    public abstract class Character
    {
        // Properties
        // These properties define the core attributes of a character. Private setters
        // for Name, ClassName, MaxHealth, Speed, and UsesProjectiles ensure that these
        // values are set only through the constructor or specific methods, maintaining
        // data integrity (encapsulation).
        public string Name { get; private set; }
        public string ClassName { get; private set; }
        public int MaxHealth { get; private set; }
        public int Health { get; protected set; }
        public int Speed { get; private set; }
        public int SpecialPower { get; protected set; }
        public int SpecialPowerGauge { get; protected set; }
        public int MaxSpecialPowerGauge { get; protected set; }
        public int Range { get; protected set; }
        public bool UsesProjectiles { get; private set; }
        

        // Protected static Random object to be shared among all character instances.
        // This helps avoid issues with similar seeds when creating multiple Random
        // objects in quick succession.
        protected static Random random = new Random();

        // Constructor
        // Initializes the core properties of a character when a new instance is created.
        // Derived classes will call this base constructor using 'base()'.
        protected Character(string name, string className, int maxHealth, int speed, bool usesProjectiles)
        {
            Name = name;
            ClassName = className;
            MaxHealth = maxHealth;
            Health = maxHealth;
            Speed = speed;
            UsesProjectiles = usesProjectiles;
        }

        // Virtual method for derived classes to implement attack logic, returning damage and attack name.
        // This method can be overridden by derived classes to provide specific attack behaviors,
        // demonstrating polymorphism.
        public virtual Tuple<int, string> Attack()
        {
            // Generate random damage between 15 and 30 (example range)
            int baseDamage = random.Next(15, 31); 
            string attackName = GetRandomAttackName(); // Use internal method to get a default name
            return new Tuple<int, string>(baseDamage, attackName);
        }

        // Method to handle taking damage.
        // This method can also be overridden by derived classes if specific damage reduction
        // or reaction logic is needed, further demonstrating polymorphism.
        public virtual void TakeDamage(int damage)
        {
            // Apply damage directly
            Health = Math.Max(0, Health - damage);
        }

        // Method to check if character is alive.
        public bool IsAlive()
        {
            return Health > 0;
        }

        // Method to get current health percentage.
        public double GetHealthPercentage()
        {
            return (double)Health / MaxHealth * 100;
        }

        public void ResetHealth()
        {
            Health = MaxHealth;
        }

        public void Reset()
        {
            Health = MaxHealth;
        }

        // Virtual method for derived classes to provide a list of possible attack names.
        // Derived classes can override this to define their unique attack names.
        protected virtual List<string> GetAttackNames()
        {
            return new List<string>() { "Attack" }; // Default list
        }

        // Internal helper method to randomly select an attack name from the list provided by GetAttackNames.
        // This keeps the logic for selecting a random name within the base class while allowing
        // derived classes to define the available names.
        internal string GetRandomAttackName()
        {
            List<string> names = GetAttackNames();
            if (names.Count == 0)
            {
                return "Attack"; // Default name if none are defined
            }
            int index = random.Next(names.Count);
            return names[index];
        }
    }
} 