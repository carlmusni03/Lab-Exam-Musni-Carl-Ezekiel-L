using System;
using System.Drawing;

namespace Classmates_RPG_Battle_Simulator.Models
{
    public class DamageText
    {
        private string text;
        private Point position;
        private Color color;
        private int lifetime = 60; // 1 second at 60 FPS
        private float alpha = 1.0f;
        private float yOffset = 0;

        public DamageText(string text, Point position, Color color)
        {
            this.text = text;
            this.position = position;
            this.color = color;
        }

        public void Update()
        {
            lifetime--;
            alpha = (float)lifetime / 60;
            yOffset -= 0.5f; // Move upward
        }

        public bool IsDead()
        {
            return lifetime <= 0;
        }

        public void Draw(Graphics g)
        {
            if (IsDead()) return;

            using (var font = new Font("Arial", 12, FontStyle.Bold))
            using (var brush = new SolidBrush(Color.FromArgb((int)(alpha * 255), color)))
            {
                var size = g.MeasureString(text, font);
                g.DrawString(text, font, brush,
                    position.X - size.Width / 2,
                    position.Y + yOffset - size.Height / 2);
            }
        }
    }
} 