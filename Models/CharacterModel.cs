using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Collections.Generic;
using Classmates_RPG_Battle_Simulator.Models;

namespace Classmates_RPG_Battle_Simulator.Models
{
    // This class is a model used for rendering character information on the form.
    // It holds display-related properties and methods for drawing the character,
    // health bar, name, class name, and turn indicator.
    // It acts as a bridge between the character data (Character class) and the UI.
    public class CharacterModel
    {
        // Properties
        // These properties store the visual and state information needed for drawing.
        public Rectangle Bounds { get; internal set; }
        public Color Color { get; private set; }
        public string Name { get; private set; }
        public string ClassName { get; private set; }
        public int Health { get; set; }
        public int MaxHealth { get; private set; }
        public bool UsesProjectiles { get; set; }
        public bool IsCurrentTurn { get; set; }

        // Add properties for hit effect
        // These are used to create a visual flash effect when the character is hit.
        private bool isHit = false;
        private int hitEffectTimer = 0;
        private const int HIT_EFFECT_DURATION = 15; // Duration in frames (e.g., 15 frames = 0.25 seconds at 60 FPS)

        // Constructor
        // Initializes the CharacterModel with display properties and links it to character data.
        public CharacterModel(string name, string className, Rectangle bounds, Color color, int maxHealth, bool usesProjectiles = false)
        {
            Name = name;
            ClassName = className;
            Bounds = bounds;
            Color = color;
            MaxHealth = maxHealth;
            Health = maxHealth;
            UsesProjectiles = usesProjectiles;
        }

        // Method to draw the character model on the graphics context.
        // This includes drawing the character shape, health bar, names, and turn indicator.
        public void Draw(Graphics g)
        {
            // Determine character color - potentially change if hit
            Color drawColor = Color;
            if (isHit)
            {
                drawColor = Color.White; // Flash white when hit
            }

            // Draw character (simple representation)
            using (var brush = new SolidBrush(drawColor))
            {
                g.FillRectangle(brush, Bounds.X, Bounds.Y, Bounds.Width, Bounds.Height);
            }

            // Draw border
            using (var pen = new Pen(Color.Darken(0.3f), 2))
            {
                g.DrawRectangle(pen, Bounds.X, Bounds.Y, Bounds.Width, Bounds.Height);
            }

            // Calculate health bar position (now below name)
            int barWidth = 100;
            int barHeight = 10;
            int barX = Bounds.X + (Bounds.Width - barWidth) / 2;
            int barY = Bounds.Y - 20; // Adjusted position to be closer

            // Draw health bar
            DrawHealthBar(g, barX, barY, barWidth, barHeight);

            // Draw turn indicator (standard red triangle above name)
            if (IsCurrentTurn)
            {
                // Calculate position relative to the name's drawing position (name is at Bounds.Y - 50)
                // Use the already calculated barX, barY, barWidth for positioning
                int trianglePeakY = Bounds.Y - 55; // Position the peak of the triangle 5 pixels above the name drawing position
                int triangleHeight = 10; // Height of the triangle
                int triangleBaseY = trianglePeakY - triangleHeight; // Position the base above the peak

                using (var turnBrush = new SolidBrush(Color.Red))
                {
                    Point[] trianglePoints = new Point[]
                    {
                        // Position triangle above the center of the name/health bar area
                        new Point(barX + barWidth / 2, trianglePeakY), // Bottom point (peak)
                        new Point(barX + barWidth / 2 - 10, triangleBaseY), // Top-left point
                        new Point(barX + barWidth / 2 + 10, triangleBaseY)  // Top-right point
                    };
                    g.FillPolygon(turnBrush, trianglePoints);
                }
            }

            // Draw player input name (above health bar)
            using (var font = new Font("Arial", 10))
            using (var brush = new SolidBrush(Color.Black))
            {
                var nameSize = g.MeasureString(Name, font);
                g.DrawString(Name, font, brush,
                    barX + (barWidth - nameSize.Width) / 2, // Center above the health bar
                    barY - 20); // Position 20 pixels above the health bar
            }

            // Draw class name (inside model)
            using (var font = new Font("Arial", 10))
            using (var brush = new SolidBrush(Color.Black))
            {
                var classNameSize = g.MeasureString(ClassName, font);
                g.DrawString(ClassName, font, brush, 
                    Bounds.X + (Bounds.Width - classNameSize.Width) / 2, 
                    Bounds.Y + (Bounds.Height - classNameSize.Height) / 2); // Position class name inside the bounds, centered
            }
        }

        // Helper method to draw the character's health bar.
        // Calculates the width based on current health and positions the health text.
        private void DrawHealthBar(Graphics g, int barX, int barY, int barWidth, int barHeight)
        {
            // Draw background
            using (var brush = new SolidBrush(Color.DarkGray))
            {
                g.FillRectangle(brush, barX, barY, barWidth, barHeight);
            }

            // Draw health
            float healthPercentage = (float)Health / MaxHealth;
            int healthWidth = (int)Math.Round((double)Health / MaxHealth * barWidth);
            using (var brush = new SolidBrush(Color.LightGreen)) // Use LightGreen for health
            {
                g.FillRectangle(brush, barX, barY, healthWidth, barHeight);
            }

            // Draw border
            using (var pen = new Pen(Color.Black, 1))
            {
                g.DrawRectangle(pen, barX, barY, barWidth, barHeight);
            }

            // Draw health value text for debugging
            using (var font = new Font("Arial", 8))
            using (var brush = new SolidBrush(Color.White))
            {
                string healthText = $"{Health}/{MaxHealth}";
                var textSize = g.MeasureString(healthText, font);
                
                // Determine text position based on character model position
                float textX;
                if (Bounds.X < g.VisibleClipBounds.Width / 2) // Assume left side is Player 1
                {
                    // Position text to the right of the health bar for Player 1
                    textX = barX + barWidth + 5;
                }
                else // Assume right side is Player 2
                {
                    // Position text to the left of the health bar for Player 2
                    textX = barX - textSize.Width - 5; // 5 pixels padding to the left
                }

                g.DrawString(healthText, font, brush,
                             textX,
                             barY + (barHeight - textSize.Height) / 2);
            }
        }

        // Resets temporary states like the hit effect.
        public void ResetStates()
        {
            isHit = false;
            hitEffectTimer = 0;
        }

        // Initiates the hit effect.
        public void OnHit()
        {
            isHit = true;
            hitEffectTimer = HIT_EFFECT_DURATION;
        }

        // Updates the model's state over time, specifically managing the hit effect timer.
        public void Update()
        {
            if (isHit)
            {
                hitEffectTimer--;
                if (hitEffectTimer <= 0)
                {
                    isHit = false;
                }
            }
        }
    }

    // Extension method for Color to easily darken a color.
    public static class ColorExtensions
    {
        public static Color Darken(this Color color, float factor)
        {
            return Color.FromArgb(
                color.A,
                (int)(color.R * factor),
                (int)(color.G * factor),
                (int)(color.B * factor)
            );
        }
    }
} 