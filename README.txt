## Classmates RPG Battle Simulator

### Project Description

This project is a simple turn-based RPG battle simulator developed using C# and Windows Forms. It allows two players to select characters and engage in a battle with randomized attacks and a best-of-3 round system. The UI displays character models, health bars, battle logs, and turn indicators.

### Object-Oriented Programming (OOP) Principles Applied

1.  **Encapsulation**: The `Character` class encapsulates the common properties (Name, Health, Speed, etc.) and behaviors (Attack, TakeDamage, IsAlive) of all characters. The derived classes also encapsulate their specific attack names and properties.
2.  **Inheritance**: Derived character classes (`KeithRozen`, `CarlEzekiel`, `EdwardJames`, `AeronJames`) inherit from the abstract `Character` base class. This allows them to share common functionality while also providing their own specialized implementations (e.g., overriding the `GetAttackNames()` method).
3.  **Polymorphism**: The `GetAttackNames()` method in the `Character` base class is marked as `virtual`, allowing derived classes to override it and provide their own specific attack names. This means that each character can have unique attack names while sharing the same base damage calculation.
4.  **Abstraction**: The `Character` class is marked as `abstract`, meaning it cannot be instantiated directly. It serves as a blueprint for concrete character types, defining a common interface without providing a complete implementation for all methods. The `CharacterModel` class provides an abstraction layer between the character data and the UI rendering.

### Battle System

The game features a turn-based battle system with the following mechanics:

1. **Base Stats**:
   - All characters have 100 HP
   - Base attack damage is randomized between 15-30
   - Each character has unique attack names that reflect their theme

2. **Damage System**:
   - All characters use the same standardized damage system
   - Base damage is randomly calculated between 15-30
   - Damage is consistently displayed across:
     - Battle log messages
     - Floating damage numbers
     - Health bar reduction

3. **Turn System**:
   - Players take turns attacking (Space for Player 1, Enter for Player 2)
   - The loser of each round attacks first in the next round
   - Best-of-3 rounds to determine the match winner

### Characters

The game features four unique characters, each with their own personality and style:

*   **Rozen**: A methodical character who uses attacks like "Swat Bug", "Fix Glitch", and "Optimize Code". Does not use projectiles.
*   **Ezekiel**: A defensive character with attacks like "Block Connection", "Filter Packet", and "Deny Access". Does not use projectiles.
*   **A-James**: An aggressive character who uses attacks like "Inject Malware", "Corrupt Data", and "Spread Infection". Uses projectiles.
*   **E-James**: A strategic character with attacks like "Trigger Exception", "Syntax Error", and "System Crash". Uses projectiles.

### How to Use
1. Enter names for both players
2. Select character classes for each player
3. Click "Start Battle" to begin
4. Player 1 attacks using the Space key
5. Player 2 attacks using the Enter key
6. The first player to win 2 rounds wins the match
7. Click "Play Again" to start a new match

Note: This is a fun project created for educational purposes. All character names and classes are fictional and not meant to represent real people. 