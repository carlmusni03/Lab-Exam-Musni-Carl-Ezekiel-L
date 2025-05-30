using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using Classmates_RPG_Battle_Simulator.Models;

namespace Classmates_RPG_Battle_Simulator
{
    // This is the main form of the Classmates RPG Battle Simulator.
    // It handles the UI setup, user input, game logic, battle flow, and rendering.
    // It manages the state of the battle, including player turns, health, rounds, and win conditions.
    public partial class Form1 : Form
    {
        private Character player1;
        private Character player2;
        private CharacterModel player1Model;
        private CharacterModel player2Model;
        private int roundNumber = 1;
        private int player1Wins = 0;
        private int player2Wins = 0;
        private bool isRoundOver = false;
        private const int ROUND_END_DELAY = 180; // 3 seconds at 60 FPS
        private Panel controlPanel;
        private Panel battlePanel;
        private Panel logPanel;
        private TextBox battleLog;
        private Button btnStartBattle;
        private bool battleInProgress = false;
        private List<DamageText> damageTexts = new List<DamageText>();
        private string matchWinnerName = string.Empty;
        private string finalScoreText = string.Empty;
        private bool showGameOverScreen = false;
        private Button btnPlayAgain;
        private bool isPlayer1Turn = true; // Player 1 starts

        // Variable to store the winner of the previous round, used to determine who attacks first in the next round.
        // Challenge: Ensuring the loser attacks first required tracking the previous round's outcome.
        private string lastRoundWinner = string.Empty;

        // Add timer for round end delay
        // This timer is used to introduce a delay between rounds and display the countdown.
        private Timer roundEndTimerObject;
        private int roundEndCountdown = 0;

        // Add timer for game updates
        // This timer drives the main game loop, handling animations (like damage text) and state updates.
        private Timer gameUpdateTimer;

        // Add timer and variables for attack interval
        // This timer enforces a delay between character attacks, creating a turn-based feel.
        // Challenge: Implementing a reliable attack cooldown required a separate timer and tracking the countdown.
        private Timer attackIntervalTimer;
        private int attackCooldownCountdown = 0;
        private const int ATTACK_INTERVAL_DURATION = 30; // 0.5 seconds at 60 FPS

        // Control declarations
        // UI elements for player input and starting the battle.
        private TextBox txtPlayer1Name;
        private TextBox txtPlayer2Name;
        private ComboBox cmbPlayer1Character;
        private ComboBox cmbPlayer2Character;

        public Form1()
        {
            InitializeComponent();
            this.DoubleBuffered = true; // Reduces flickering during drawing
            this.KeyPreview = true; // Allows the form to capture key events before controls
            this.BackColor = Color.Black;
            InitializeForm(); // Custom method for setting up UI elements programmatically

            // Initialize round end timer
            roundEndTimerObject = new Timer();
            roundEndTimerObject.Interval = 1000; // 1 second
            roundEndTimerObject.Tick += RoundEndTimerObject_Tick;

            // Initialize game update timer
            gameUpdateTimer = new Timer();
            gameUpdateTimer.Interval = 16; // Approx 60 FPS (1000 ms / 60 frames)
            gameUpdateTimer.Tick += GameUpdateTimer_Tick;
            gameUpdateTimer.Start(); // Start the game update timer

            // Initialize attack interval timer
            attackIntervalTimer = new Timer();
            attackIntervalTimer.Interval = 1000 / 60; // Approx 60 FPS
            attackIntervalTimer.Tick += AttackIntervalTimer_Tick;
        }

        // Custom method to initialize and configure UI elements programmatically.
        private void InitializeForm()
        {
            this.Text = "Classmates RPG Battle Simulator";
            this.MinimumSize = new Size(500, 630);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(45, 45, 48);
            this.ForeColor = Color.White;
            this.Font = new Font("Segoe UI", 9F);

            // Control Panel (Player Selection)
            controlPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(37, 37, 38),
                Padding = new Padding(20),
                Visible = true
            };

            // Title Label
            var titleLabel = new Label
            {
                Text = "Classmates RPG Battle Simulator",
                Font = new Font("Segoe UI", 20, FontStyle.Bold),
                ForeColor = Color.FromArgb(0, 122, 204),
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Top,
                Height = 60,
                Margin = new Padding(0),
                BackColor = Color.FromArgb(30, 30, 30)
            };
            controlPanel.Controls.Add(titleLabel);

            // Player 1 Panel
            var player1Panel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 150,
                Margin = new Padding(20, 20, 20, 10),
                BackColor = Color.FromArgb(45, 45, 48)
            };

            var player1Label = new Label
            {
                Text = "Player 1 (Space to Attack)",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.FromArgb(0, 122, 204),
                Dock = DockStyle.Top,
                Height = 25,
                TextAlign = ContentAlignment.MiddleLeft
            };
            player1Panel.Controls.Add(player1Label);

            txtPlayer1Name = new TextBox
            {
                Text = "Enter Player 1 Name",
                Dock = DockStyle.Top,
                Height = 25,
                Margin = new Padding(0, 5, 0, 5),
                BackColor = Color.FromArgb(51, 51, 55),
                ForeColor = Color.Gray,
                BorderStyle = BorderStyle.FixedSingle
            };
            txtPlayer1Name.Enter += (s, e) => {
                if (txtPlayer1Name.Text == "Enter Player 1 Name")
                {
                    txtPlayer1Name.Text = "";
                    txtPlayer1Name.ForeColor = Color.White;
                }
            };
            txtPlayer1Name.Leave += (s, e) => {
                if (string.IsNullOrWhiteSpace(txtPlayer1Name.Text))
                {
                    txtPlayer1Name.Text = "Enter Player 1 Name";
                    txtPlayer1Name.ForeColor = Color.Gray;
                }
            };
            player1Panel.Controls.Add(txtPlayer1Name);

            cmbPlayer1Character = new ComboBox
            {
                Dock = DockStyle.Top,
                Height = 25,
                Margin = new Padding(0, 5, 0, 5),
                BackColor = Color.FromArgb(51, 51, 55),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbPlayer1Character.Items.AddRange(new string[] { 
                "Rozen", 
                "Ezekiel", 
                "A-James", 
                "E-James" 
            });
            cmbPlayer1Character.SelectedIndex = 0;
            player1Panel.Controls.Add(cmbPlayer1Character);

            controlPanel.Controls.Add(player1Panel);

            // Player 2 Panel
            var player2Panel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 150,
                Margin = new Padding(0, 0, 0, 10),
                BackColor = Color.FromArgb(45, 45, 48)
            };

            var player2Label = new Label
            {
                Text = "Player 2 (Enter to Attack)",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.FromArgb(0, 122, 204),
                Dock = DockStyle.Top,
                Height = 25,
                TextAlign = ContentAlignment.MiddleLeft
            };
            player2Panel.Controls.Add(player2Label);

            txtPlayer2Name = new TextBox
            {
                Text = "Enter Player 2 Name",
                Dock = DockStyle.Top,
                Height = 25,
                Margin = new Padding(0, 5, 0, 5),
                BackColor = Color.FromArgb(51, 51, 55),
                ForeColor = Color.Gray,
                BorderStyle = BorderStyle.FixedSingle
            };
            txtPlayer2Name.Enter += (s, e) => {
                if (txtPlayer2Name.Text == "Enter Player 2 Name")
                {
                    txtPlayer2Name.Text = "";
                    txtPlayer2Name.ForeColor = Color.White;
                }
            };
            txtPlayer2Name.Leave += (s, e) => {
                if (string.IsNullOrWhiteSpace(txtPlayer2Name.Text))
                {
                    txtPlayer2Name.Text = "Enter Player 2 Name";
                    txtPlayer2Name.ForeColor = Color.Gray;
                }
            };
            player2Panel.Controls.Add(txtPlayer2Name);

            cmbPlayer2Character = new ComboBox
            {
                Dock = DockStyle.Top,
                Height = 25,
                Margin = new Padding(0, 5, 0, 5),
                BackColor = Color.FromArgb(51, 51, 55),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbPlayer2Character.Items.AddRange(new string[] { 
                "Rozen", 
                "Ezekiel", 
                "A-James", 
                "E-James" 
            });
            cmbPlayer2Character.SelectedIndex = 0;
            player2Panel.Controls.Add(cmbPlayer2Character);

            controlPanel.Controls.Add(player2Panel);

            // Start Button
            btnStartBattle = new Button
            {
                Text = "Start Battle",
                Dock = DockStyle.Bottom,
                Height = 50,
                Margin = new Padding(20, 20, 20, 20),
                BackColor = Color.FromArgb(0, 122, 204),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnStartBattle.FlatAppearance.BorderSize = 0;
            btnStartBattle.FlatAppearance.MouseOverBackColor = Color.FromArgb(0, 102, 184);
            btnStartBattle.FlatAppearance.MouseDownBackColor = Color.FromArgb(0, 82, 164);
            btnStartBattle.Click += BtnStartBattle_Click;
            controlPanel.Controls.Add(btnStartBattle);

            // Battle Panel
            battlePanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(37, 37, 38),
                Visible = false
            };
            battlePanel.Paint += Form1_Paint;

            // Log Panel
            logPanel = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 230,
                BackColor = Color.FromArgb(30, 30, 30),
                Padding = new Padding(10),
                Visible = false
            };

            battleLog = new TextBox
            {
                Multiline = true,
                ReadOnly = true,
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(30, 30, 30),
                ForeColor = Color.FromArgb(200, 200, 200),
                BorderStyle = BorderStyle.None,
                Font = new Font("Consolas", 9F),
                ScrollBars = ScrollBars.Vertical
            };
            logPanel.Controls.Add(battleLog);

            // Add panels to form
            this.Controls.Add(controlPanel);
            this.Controls.Add(battlePanel);
            this.Controls.Add(logPanel);

            // Add event handlers
            this.KeyDown += Form1_KeyDown;
            this.KeyUp += Form1_KeyUp;
            this.Resize += Form1_Resize;
        }

        // Event handler for the Start Battle button click.
        // This is where the battle setup begins, including input validation and character creation.
        // Challenge: Handling invalid or empty player names required adding validation checks.
        private void BtnStartBattle_Click(object sender, EventArgs e)
        {
            // Add explicit validation checks before the try-catch block
            if (string.IsNullOrWhiteSpace(txtPlayer1Name.Text) || txtPlayer1Name.Text == "Enter Player 1 Name" ||
                string.IsNullOrWhiteSpace(txtPlayer2Name.Text) || txtPlayer2Name.Text == "Enter Player 2 Name")
            {
                MessageBox.Show("Please enter names for both players!");
                return;
            }

            try
            {
                if (battleInProgress)
                {
                    MessageBox.Show("A battle is already in progress!");
                    return;
                }

                // Hide control panel
                controlPanel.Visible = false;

                // Show battle and log panels
                battlePanel.Visible = true;
                logPanel.Visible = true;

                // Create characters
                player1 = CreateCharacter(txtPlayer1Name.Text, cmbPlayer1Character.Text);
                player2 = CreateCharacter(txtPlayer2Name.Text, cmbPlayer2Character.Text);

                // Calculate initial positions
                int groundY = battlePanel.Height - 50; // Ground level
                int characterHeight = 100; // Character height

                // Create character models with projectile settings, passing class name
                player1Model = new CharacterModel(player1.Name, player1.ClassName,
                    new Rectangle(50, groundY - characterHeight, 50, characterHeight),
                    Color.Blue,
                    100,
                    player1.UsesProjectiles);

                player2Model = new CharacterModel(player2.Name, player2.ClassName,
                    new Rectangle(battlePanel.Width - 100, groundY - characterHeight, 50, characterHeight),
                    Color.Red,
                    100,
                    player2.UsesProjectiles);

                // Start battle
                battleInProgress = true;
                this.KeyPreview = true;
                battleLog.Clear();
                battleLog.AppendText($"Battle started between {player1.Name} and {player2.Name}!\r\n");
                battleLog.AppendText($"It is {player1.Name}'s turn.\r\n");
                isPlayer1Turn = true; // Reset turn to Player 1
                battlePanel.Invalidate();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
                battleInProgress = false;
            }
        }

        // Helper method to create character instances based on selected class name.
        // Demonstrates factory pattern principles for creating objects.
        private Character CreateCharacter(string name, string className)
        {
            // Use the selected nickname to determine the character class
            switch (className)
            {
                case "Rozen":
                    return new KeithRozen(name);
                case "Ezekiel":
                    return new CarlEzekiel(name);
                case "A-James":
                    return new AeronJames(name);
                case "E-James":
                    return new EdwardJames(name);
                default:
                    throw new ArgumentException("Invalid character class");
            }
        }

        // Event handler for the form's Paint event.
        // This method is responsible for drawing all game elements, such as character models,
        // health bars, and damage text.
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            if (!battleInProgress && !showGameOverScreen) return;

            // Draw background
            e.Graphics.FillRectangle(Brushes.SkyBlue, 0, 0, battlePanel.Width, battlePanel.Height);

            // Draw ground
            int groundHeight = 50;
            int groundY = battlePanel.Height - groundHeight;
            e.Graphics.FillRectangle(Brushes.SandyBrown, 0, groundY, battlePanel.Width, groundHeight);

            if (showGameOverScreen)
            {
                // Draw game over screen
                using (var font = new Font("Arial", 36, FontStyle.Bold))
                using (var brush = new SolidBrush(Color.White))
                using (var shadowBrush = new SolidBrush(Color.Black))
                {
                    string winnerText = $"{matchWinnerName} wins the match!";
                    string scoreText = finalScoreText;

                    var winnerSize = e.Graphics.MeasureString(winnerText, font);
                    var scoreSize = e.Graphics.MeasureString(scoreText, font);

                    // Draw winner text with shadow
                    e.Graphics.DrawString(winnerText, font, shadowBrush,
                        (battlePanel.Width - winnerSize.Width) / 2 + 2,
                        (battlePanel.Height / 2 - winnerSize.Height) + 2);
                    e.Graphics.DrawString(winnerText, font, brush,
                        (battlePanel.Width - winnerSize.Width) / 2,
                        (battlePanel.Height / 2 - winnerSize.Height));

                    // Draw score text with shadow
                     e.Graphics.DrawString(scoreText, font, shadowBrush,
                        (battlePanel.Width - scoreSize.Width) / 2 + 2,
                        (battlePanel.Height / 2) + 2);
                    e.Graphics.DrawString(scoreText, font, brush,
                        (battlePanel.Width - scoreSize.Width) / 2,
                        (battlePanel.Height / 2));
                }
            }
            else
            {
                // Draw characters, projectiles, and effects
                if (player1Model != null && player2Model != null)
                {
                    // Set turn indicator
                    player1Model.IsCurrentTurn = isPlayer1Turn;
                    player2Model.IsCurrentTurn = !isPlayer1Turn;

                    player1Model.Draw(e.Graphics);
                    player2Model.Draw(e.Graphics);

                    // Draw damage texts
                    foreach (var damageText in damageTexts)
                    {
                        damageText.Draw(e.Graphics);
                    }

                    // Draw round information
                    using (var font = new Font("Arial", 16, FontStyle.Bold))
                    using (var brush = new SolidBrush(Color.White))
                    {
                        string roundText = $"Round {roundNumber}";
                        string scoreText = $"{player1.Name}: {player1Wins} - {player2.Name}: {player2Wins}";
                        var roundSize = e.Graphics.MeasureString(roundText, font);
                        var scoreSize = e.Graphics.MeasureString(scoreText, font);

                        // Draw round number
                        e.Graphics.DrawString(roundText, font, brush,
                            (battlePanel.Width - roundSize.Width) / 2, 20);

                        // Draw score
                        e.Graphics.DrawString(scoreText, font, brush,
                            (battlePanel.Width - scoreSize.Width) / 2, 50);

                        // Draw current turn
                        string turnText = isPlayer1Turn ? $"{player1.Name}'s Turn" : $"{player2.Name}'s Turn";
                        var turnSize = e.Graphics.MeasureString(turnText, font);
                        e.Graphics.DrawString(turnText, font, brush,
                            (battlePanel.Width - turnSize.Width) / 2,
                            80); // Position below the score
                    }
                }
            }

            // Draw countdown if round is over but match is not
            if (isRoundOver && roundEndCountdown > 0)
            {
                using (var countdownFont = new Font("Arial", 48, FontStyle.Bold))
                using (var countdownBrush = new SolidBrush(Color.Yellow))
                using (var shadowBrush = new SolidBrush(Color.Black))
                {
                    string countdownText = $"Next round in {roundEndCountdown}";
                    var countdownSize = e.Graphics.MeasureString(countdownText, countdownFont);
                    
                    // Draw countdown with shadow
                    e.Graphics.DrawString(countdownText, countdownFont, shadowBrush,
                        (battlePanel.Width - countdownSize.Width) / 2 + 3,
                        (battlePanel.Height - countdownSize.Height) / 2 + 3);
                    e.Graphics.DrawString(countdownText, countdownFont, countdownBrush,
                        (battlePanel.Width - countdownSize.Width) / 2,
                        (battlePanel.Height - countdownSize.Height) / 2);
                }
            }
        }

        // Method to handle the end of a round.
        // Determines the round winner, updates scores, and prepares for the next round or game over.
        // Challenge: Managing the round end delay and countdown required integrating a timer.
        private void EndBattle(string winner)
        {
            // Update round wins
            if (winner == player1.Name)
            {
                player1Wins++;
            }
            else
            {
                player2Wins++;
            }

            // Store the winner of the round
            lastRoundWinner = winner;

            // Check if match is over (best of 3)
            if (player1Wins >= 2 || player2Wins >= 2)
            {
                matchWinnerName = player1Wins >= 2 ? player1.Name : player2.Name;
                finalScoreText = $"{player1.Name}: {player1Wins} - {player2.Name}: {player2Wins}";
                
                // Stop the game timer before showing the message
                battleInProgress = false;
                this.KeyPreview = false;

                showGameOverScreen = true;
                battlePanel.Invalidate(); // Redraw to show game over screen

                // Add Play Again button
                btnPlayAgain = new Button
                {
                    Text = "Play Again",
                    Size = new Size(150, 50),
                    Location = new Point(
                        (battlePanel.Width - 150) / 2, // Center horizontally
                        (battlePanel.Height / 2) + 70 // Position below the score
                    ),
                    BackColor = Color.FromArgb(0, 122, 204),
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat,
                    Font = new Font("Segoe UI", 14, FontStyle.Bold),
                    Cursor = Cursors.Hand,
                    Visible = true
                };
                btnPlayAgain.FlatAppearance.BorderSize = 0;
                btnPlayAgain.FlatAppearance.MouseOverBackColor = Color.FromArgb(0, 102, 184);
                btnPlayAgain.FlatAppearance.MouseDownBackColor = Color.FromArgb(0, 82, 164);
                btnPlayAgain.Click += BtnPlayAgain_Click;

                // Remove any existing Play Again button before adding a new one
                // This prevents adding the button multiple times if EndBattle is somehow called again
                if (battlePanel.Controls.Contains(btnPlayAgain))
                {
                    battlePanel.Controls.Remove(btnPlayAgain);
                }
                battlePanel.Controls.Add(btnPlayAgain);
                btnPlayAgain.BringToFront(); // Ensure button is on top

            }
            else
            {
                // Start next round
                roundNumber++;
                // StartNewRound(); // Directly start the next round

                // Start countdown for next round
                isRoundOver = true; // Set flag to indicate round is over, but match isn't
                roundEndCountdown = 3; // Start a 3-second countdown
                roundEndTimerObject.Start();
                battleLog.AppendText("\r\nRound Over. Next round starts in...");
                battlePanel.Invalidate(); // Redraw to show countdown
            }
        }

        // Method to start a new round.
        // Resets character states, increments round number, and determines the starting player.
        // Challenge: Implementing the logic for the loser of the previous round attacking first.
        private void StartNewRound()
        {
            // Reset character health
            player1.ResetHealth();
            player2.ResetHealth();
            player1Model.Health = player1.Health;
            player2Model.Health = player2.Health;

            // Reset character positions (simplified - just reset to initial spots)
            int groundY = battlePanel.Height - 50; // Ground level
            int characterHeight = player1Model.Bounds.Height;

            // Position characters on the ground
            player1Model.Bounds = new Rectangle(50, groundY - characterHeight, player1Model.Bounds.Width, player1Model.Bounds.Height);
            player2Model.Bounds = new Rectangle(battlePanel.Width - 100, groundY - characterHeight, player2Model.Bounds.Width, player2Model.Bounds.Height);

            // Reset character states
            ResetStates();

            // Reset turn to Player 1
            isPlayer1Turn = true;
            battleLog.AppendText($"\r\nRound {roundNumber} Start! It is {player1.Name}'s turn.\r\n");

            // Reset the round over flag
            isRoundOver = false;

            // Force redraw
            battlePanel.Invalidate();

            // Determine who attacks first in the new round (loser of the previous round)
            if (roundNumber == 1) // Player 1 always starts the very first round
            {
                isPlayer1Turn = true;
                battleLog.AppendText($"\r\nRound {roundNumber} Start! It is {player1.Name}'s turn.\r\n");
            }
            else
            {
                if (lastRoundWinner == player1.Name) // If Player 1 won last round, Player 2 goes first
                {
                    isPlayer1Turn = false;
                    battleLog.AppendText($"\r\nRound {roundNumber} Start! It is {player2.Name}'s turn.\r\n");
                }
                else // If Player 2 won last round, Player 1 goes first
                {
                    isPlayer1Turn = true;
                    battleLog.AppendText($"\r\nRound {roundNumber} Start! It is {player1.Name}'s turn.\r\n");
                }
            }

            // Reset the round over flag
            isRoundOver = false;

            // Force redraw
            battlePanel.Invalidate();
        }

        // Resets the visual states of character models (e.g., hit effect).
        private void ResetStates()
        {
            // Clear damage texts
            damageTexts.Clear();

            // Reset model states (health is already reset in StartNewRound)
            player1Model.Health = player1.MaxHealth;
            player2Model.Health = player2.MaxHealth;
            // Since CharacterModel is simplified, no other states to reset here
        }

        // Adds a new damage text element to be displayed on the screen.
        private void AddDamageText(int damage, Point position, Color? color = null)
        {
            AddDamageText(damage.ToString(), position, color ?? Color.Red);
        }

        // Adds a custom text message to be displayed on the screen.
        private void AddDamageText(string text, Point position, Color color)
        {
            damageTexts.Add(new DamageText(text, position, color));
        }

        // Event handler for the form loading.
        // Can be used for initial setup if needed.
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        // Event handler for the form resizing.
        // Adjusts the positions of character models to stay centered in the battle panel.
        // Challenge: Dynamically repositioning UI elements upon form resize.
        private void Form1_Resize(object sender, EventArgs e)
        {
            if (battleInProgress && player1Model != null && player2Model != null)
            {
                // Recalculate character positions based on new panel size
                int groundY = battlePanel.Height - 50; // Ground level, 50 pixels from the bottom
                int characterHeight = player1Model.Bounds.Height; // Keep character height constant
                int characterWidth = player1Model.Bounds.Width; // Keep character width constant
                int horizontalPadding = 50; // Distance from left/right edges

                // Position Player 1 on the left side
                player1Model.Bounds = new Rectangle(
                    horizontalPadding,
                    groundY - characterHeight,
                    characterWidth,
                    characterHeight
                );

                // Position Player 2 on the right side
                player2Model.Bounds = new Rectangle(
                    battlePanel.Width - characterWidth - horizontalPadding,
                    groundY - characterHeight,
                    characterWidth,
                    characterHeight
                );

                // Force redraw
                battlePanel.Invalidate();
            }
        }

        // Event handler for the Play Again button click.
        // Resets the game state to allow a new match.
        private void BtnPlayAgain_Click(object sender, EventArgs e)
        {
            // Hide game over screen elements
            showGameOverScreen = false;
            btnPlayAgain.Visible = false;
            battlePanel.Controls.Remove(btnPlayAgain); // Remove button from panel

            // Reset game state
            player1Wins = 0;
            player2Wins = 0;
            roundNumber = 1;
            battleInProgress = false;
            this.KeyPreview = true; // Re-enable form key events

            // Show character selection screen
            controlPanel.Visible = true;
            battlePanel.Visible = false;
            logPanel.Visible = false;

            // Clear and reset the battle log
            battleLog.Clear();

            // Reset character models (positions, states, etc.)
            if (player1Model != null && player2Model != null)
            {
                player1Model.ResetStates();
                player2Model.ResetStates();
            }

            // Re-enable input controls and reset text boxes
            txtPlayer1Name.Enabled = true;
            txtPlayer2Name.Enabled = true;
            cmbPlayer1Character.Enabled = true;
            cmbPlayer2Character.Enabled = true;
            btnStartBattle.Enabled = true;

            txtPlayer1Name.Text = "Enter Player 1 Name";
            txtPlayer1Name.ForeColor = Color.Gray;
            txtPlayer2Name.Text = "Enter Player 2 Name";
            txtPlayer2Name.ForeColor = Color.Gray;

            // Force redraws
            controlPanel.Invalidate();
            this.Invalidate();
        }

        // Event handler for key down events.
        // Used to detect player input for attacks (Space for Player 1, Enter for Player 2).
        // Challenge: Ensuring the correct player attacks only when it's their turn and not on cooldown.
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (battleInProgress && attackCooldownCountdown <= 0) // Only allow input if no attack cooldown is active
            {
                // Handle attacks on key press, only if it's their turn
                if (e.KeyCode == Keys.Space) // Player 1 Attack
                {
                    Player1Attack();
                }
                else if (e.KeyCode == Keys.Enter) // Player 2 Attack
                {
                    Player2Attack();
                }
            }
        }

        // Event handler for key up events.
        // Can be used for releasing keys if necessary, though not strictly needed for this input method.
        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            // No key up handling needed
        }

        // Method to handle Player 1's attack.
        // Checks turn, cooldown, performs attack, updates battle log, and switches turn.
        private void Player1Attack()
        {
            if (battleInProgress && isPlayer1Turn && attackCooldownCountdown <= 0) // Add cooldown check
            {
                // Player 1 attacks Player 2
                Tuple<int, string> attackResult = player1.Attack();
                int damage = attackResult.Item1;
                string attackName = attackResult.Item2;

                battleLog.Text += $"{player1.Name} uses {attackName} for {damage} damage!\r\n";
                player2.TakeDamage(damage);
                player2Model.Health = player2.Health; // Update model health for drawing
                battlePanel.Invalidate(player2Model.Bounds); // Invalidate the area of player 2

                // Call OnHit for the damaged player model
                player2Model.OnHit();

                // Check if Player 2 is defeated
                if (!player2.IsAlive())
                {
                    battleLog.AppendText($"{player2.Name} has been defeated!\r\n");
                    EndBattle(player1.Name);
                }
                else
                {
                    // Switch turn to Player 2 and start cooldown
                    isPlayer1Turn = false; // Player 2's turn next
                    battleLog.AppendText($"It is {player2.Name}'s turn.\r\n");
                    attackCooldownCountdown = ATTACK_INTERVAL_DURATION; // Start cooldown
                    attackIntervalTimer.Start();
                }
                battlePanel.Invalidate(); // Redraw to show health changes
            }
        }

        // Method to handle Player 2's attack.
        // Checks turn, cooldown, performs attack, updates battle log, and switches turn.
        private void Player2Attack()
        {
            if (battleInProgress && !isPlayer1Turn && attackCooldownCountdown <= 0) // Add cooldown check
            {
                // Player 2 attacks Player 1
                Tuple<int, string> attackResult = player2.Attack();
                int damage = attackResult.Item1;
                string attackName = attackResult.Item2;

                battleLog.Text += $"{player2.Name} uses {attackName} for {damage} damage!\r\n";
                player1.TakeDamage(damage);
                player1Model.Health = player1.Health; // Update model health for drawing
                battlePanel.Invalidate(player1Model.Bounds); // Invalidate the area of player 1

                // Call OnHit for the damaged player model
                player1Model.OnHit();

                // Check if Player 1 is defeated
                if (!player1.IsAlive())
                {
                    battleLog.AppendText($"{player1.Name} has been defeated!\r\n");
                    EndBattle(player2.Name);
                }
                else
                {
                    // Switch turn to Player 1 and start cooldown
                    isPlayer1Turn = true; // Player 1's turn next
                    battleLog.AppendText($"It is {player1.Name}'s turn.\r\n");
                    attackCooldownCountdown = ATTACK_INTERVAL_DURATION; // Start cooldown
                    attackIntervalTimer.Start();
                }
                battlePanel.Invalidate(); // Redraw to show health changes
            }
        }

        // Event handler for the round end timer tick.
        // Decrements the countdown and triggers starting a new round when the countdown reaches zero.
        private void RoundEndTimerObject_Tick(object sender, EventArgs e)
        {
            roundEndCountdown--;
            battlePanel.Invalidate(); // Redraw to show countdown

            if (roundEndCountdown <= 0)
            {
                roundEndTimerObject.Stop();
                StartNewRound();
            }
        }

        // Event handler for the main game update timer tick.
        // Updates the state of game elements like damage text and character models.
        private void GameUpdateTimer_Tick(object sender, EventArgs e)
        {
            if (battleInProgress)
            {
                // Update character models (for hit effect, etc.)
                player1Model?.Update();
                player2Model?.Update();

                // Update damage texts
                for (int i = damageTexts.Count - 1; i >= 0; i--)
                {
                    damageTexts[i].Update();
                    if (damageTexts[i].IsDead())
                    {
                        damageTexts.RemoveAt(i);
                    }
                }

                // Redraw the battle panel
                battlePanel.Invalidate();
            }
        }

        // Event handler for the attack interval timer tick.
        // Decrements the attack cooldown countdown.
        private void AttackIntervalTimer_Tick(object sender, EventArgs e)
        {
            if (attackCooldownCountdown > 0)
            {
                attackCooldownCountdown--;
                if (attackCooldownCountdown <= 0)
                {
                    // Cooldown finished, allow next turn
                    attackIntervalTimer.Stop();
                    // The turn switching logic is already in Player1Attack/Player2Attack
                    // We just need to ensure input is re-enabled or attacks can proceed
                }
            }
        }
    }
}
