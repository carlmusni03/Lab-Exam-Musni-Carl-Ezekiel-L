using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Collections.Generic;

namespace Classmates_RPG_Battle_Simulator.Models
{
    public class Projectile
    {
        public Point Position { get; private set; }
        public Point Target { get; private set; }
        public Color Color { get; private set; }
        public bool IsActive { get; set; }
        public bool HasHitTarget { get; set; }
        private float speed = 15f;
        private float size = 20f;
        private float trailLength = 5f;
        private List<Point> trail = new List<Point>();

        public Projectile(Point start, Point target, Color color)
        {
            Position = start;
            Target = target;
            Color = color;
            IsActive = true;
            HasHitTarget = false;
        }

        public void Update()
        {
            if (!IsActive) return;

            // Calculate direction to target
            float dx = Target.X - Position.X;
            float dy = Target.Y - Position.Y;
            float distance = (float)Math.Sqrt(dx * dx + dy * dy);

            // Normalize direction
            if (distance > 0)
            {
                dx /= distance;
                dy /= distance;
            }

            // Update position
            Position = new Point(
                Position.X + (int)(dx * speed),
                Position.Y + (int)(dy * speed)
            );

            // Add to trail
            trail.Add(Position);
            if (trail.Count > trailLength)
            {
                trail.RemoveAt(0);
            }

            // Check if reached target
            if (distance < speed)
            {
                IsActive = false;
                HasHitTarget = true;
            }
        }

        public void Draw(Graphics g)
        {
            if (!IsActive) return;

            // Draw trail
            for (int i = 0; i < trail.Count - 1; i++)
            {
                float alpha = (float)i / trail.Count;
                using (var brush = new SolidBrush(Color.FromArgb((int)(alpha * 255), Color)))
                {
                    g.FillEllipse(brush, trail[i].X - size/2, trail[i].Y - size/2, size, size);
                }
            }

            // Draw projectile
            using (var brush = new SolidBrush(Color))
            {
                g.FillEllipse(brush, Position.X - size/2, Position.Y - size/2, size, size);
            }

            // Draw glow effect
            using (var glowBrush = new SolidBrush(Color.FromArgb(100, Color)))
            {
                g.FillEllipse(glowBrush, Position.X - size, Position.Y - size, size * 2, size * 2);
            }
        }
    }
} 