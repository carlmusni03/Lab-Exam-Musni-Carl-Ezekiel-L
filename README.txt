Classmates RPG Battle Simulator

Project Description

This is a simple turn-based RPG battle simulator developed using C# and Windows Forms. Two players select characters and battle using randomized attacks in a best-of-3 round format. The UI includes character models, health bars, battle logs, and turn indicators.

Object-Oriented Programming (OOP) Principles Applied

1. Encapsulation  
   The `Character` class encapsulates common properties (Name, Health, etc.) and behaviors (Attack, TakeDamage, IsAlive). Derived classes encapsulate their own specific attack names and logic.
2. Inheritance  
   Classes like KeithRozen, CarlEzekiel, EdwardJames, and AeronJames inherit from the abstract base class `Character`. This allows them to share base functionality and override methods like `GetAttackNames()`.
3. Polymorphism  
   The `GetAttackNames()` method is marked `virtual` in the base class and overridden by derived classes to provide unique attack names. The same method call behaves differently based on the actual character type.
4. Abstraction  
   The abstract `Character` class defines the structure all characters follow but can't be instantiated directly. The `CharacterModel` class adds a layer between character data and the UI rendering.

Battle System

Base Stats
- All characters start with 100 HP
- Attack damage is randomized between 15–30
- Each character has unique attack names based on their theme

Damage System
- All characters follow a standardized damage logic
- Damage appears in:
  - Battle log messages
  - Floating damage numbers
  - Health bar animations

Turn System
- Players take turns attacking:
  - Player 1: Press Space
  - Player 2: Press Enter
- Match follows a best-of-3 rounds system

Characters
Each character has a distinct style and set of attack names:

Rozen  
A methodical character  
Attacks: "Swat Bug", "Fix Glitch", "Optimize Code"

Ezekiel  
A defensive character  
Attacks: "Block Connection", "Filter Packet", "Deny Access"  

A-James  
An aggressive character  
Attacks: "Inject Malware", "Corrupt Data", "Spread Infection"  

E-James  
A strategic character  
Attacks: "Trigger Exception", "Syntax Error", "System Crash"  

How to Use
1. Select a character class for each player
2. Enter names for both players
3. Click Start Battle  
4. Player 1 attacks with Space  
5. Player 2 attacks with Enter  
6. First to win 2 rounds wins the match  
7. Click Play Again to restart  

Note: This project was created for educational purposes.
