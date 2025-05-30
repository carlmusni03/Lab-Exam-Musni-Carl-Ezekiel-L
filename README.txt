## Classmates RPG Battle Simulator

### Project Description

This project is a simple turn-based RPG battle simulator developed using C# and Windows Forms. It allows two players to select characters and engage in a battle with randomized attacks and a best-of-3 round system. The UI displays character models, health bars, battle logs, and turn indicators.

### Object-Oriented Programming (OOP) Principles Applied

1.  **Encapsulation**: The `Character` class encapsulates the common properties (Name, Health, Speed, etc.) and behaviors (Attack, TakeDamage, IsAlive) of all characters. The derived classes also encapsulate their specific attack logic and properties.
2.  **Inheritance**: Derived character classes (`KeithRozen`, `CarlEzekiel`, `EdwardJames`, `AeronJames`) inherit from the abstract `Character` base class. This allows them to share common functionality while also providing their own specialized implementations (e.g., overriding the `Attack()` or `GetAttackNames()` methods).
3.  **Polymorphism**: The `Attack()` and `TakeDamage()` methods in the `Character` base class are marked as `virtual`, allowing derived classes to override them (`override`) and provide their own specific implementations. This means that the same method call (`character.Attack()`) can result in different behaviors depending on the actual type of the `character` object (e.g., `KeithRozen` vs `CarlEzekiel`).
4.  **Abstraction**: The `Character` class is marked as `abstract`, meaning it cannot be instantiated directly. It serves as a blueprint for concrete character types, defining a common interface without providing a complete implementation for all methods (like `Attack()`, which is often overridden). The `CharacterModel` class provides an abstraction layer between the character data and the UI rendering.

### Challenges and How They Were Overcome

1.  **Real-time vs. Turn-based Transition**: Initially, the project had real-time elements. Transitioning to a turn-based system required significant refactoring of the battle loop, input handling, and turn management logic in `Form1.cs`.
2.  **Derived Class Method Overrides**: Ensuring derived classes correctly overrode methods like `Attack()` and `GetAttackNames()` with the correct return types (`Tuple<int, string>` for `Attack()`) was initially a challenge, requiring careful review and correction of method signatures.
3.  **Randomness in Damage Calculation**: Using `System.Random` for damage calculation led to issues with non-random or similar damage values when multiple `Random` objects were created in quick succession. This was resolved by making the `Random` instance in the base `Character` class `static`, ensuring a single instance is shared across all character objects.
4.  **Turn Order Logic**: Implementing the rule that the loser of the previous round attacks first in the next round required adding logic in the `EndBattle` and `StartNewRound` methods to track the previous winner and set the `isPlayer1Turn` flag accordingly.
5.  **UI Responsiveness**: Making the character models and health bars adaptive to form resizing required adding a `Form1_Resize` event handler to recalculate and update the positions of the `CharacterModel` instances based on the new panel size.
6.  **Attack Cooldown**: Introducing a delay between attacks necessitated a separate `System.Windows.Forms.Timer` (`attackIntervalTimer`) and associated logic (`attackCooldownCountdown` and `AttackIntervalTimer_Tick`) to manage the cooldown state and prevent rapid-fire attacks.
7.  **Visual Indicators**: Implementing visual cues like the turn indicator triangle and numerical health display involved calculating positions relative to other UI elements and handling the drawing within the `Form1_Paint` method and `CharacterModel.Draw`.
8.  **File Restructuring**: Moving `DamageText.cs` from the `Effects` folder to the `Models` folder required updating namespaces in the file itself and all files that referenced it (`CharacterModel.cs`, `Form1.cs`), highlighting the importance of namespace management when refactoring.

### Characters

The game features four unique characters, each with their own personality and style:

*   **Rozen**: A methodical character who uses attacks like "Swat Bug", "Fix Glitch", and "Optimize Code".
*   **Ezekiel**: A defensive character with attacks like "Block Connection", "Filter Packet", and "Deny Access".
*   **A-James**: An aggressive character who uses attacks like "Inject Malware", "Corrupt Data", and "Spread Infection".
*   **E-James**: A strategic character with attacks like "Trigger Exception", "Syntax Error", and "System Crash".

All characters share the same base stats:
* 100 HP
* Randomized damage between 15-30 per attack
* Unique attack names that reflect their character theme

The battle system is turn-based, with each character taking turns to attack. The loser of each round gets to attack first in the next round, adding a strategic element to the gameplay.

### How to Use
1. Enter names for both players
2. Select character classes for each player
3. Click "Start Battle" to begin
4. Player 1 attacks using the Space key
5. Player 2 attacks using the Enter key
6. The first player to win 2 rounds wins the match
7. Click "Play Again" to start a new match

Note: This is a fun project created for educational purposes. All character names and classes are fictional and not meant to represent real people. 