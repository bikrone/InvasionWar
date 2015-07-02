using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InvasionWar.Helper
{
    public class UtilityHelper
    {
        public static Vector2 ApplySign(Vector2 from, Vector2 to)
        {
            Vector2 sign = new Vector2(1, 1);
            if (from.X > to.X) sign.X = -1;
            if (from.Y > to.Y) sign.Y = -1;
            return sign;
        }

        public static float ApplySign(float from, float to)
        {
            if (from <= to) return 1;
            return -1;
        }

        public static Vector2 CalculateVelocity(Vector2 from, Vector2 to, float seconds)
        {
            Vector2 velocity = new Vector2();

            velocity.X = (to.X - from.X) / seconds;
            velocity.Y = (to.Y - from.Y) / seconds;

            return velocity;
        }

        public static Vector4 CalculateVelocity(Vector4 from, Vector4 to, float seconds)
        {
            Vector4 velocity = new Vector4();

            velocity = Vector4.Divide(Vector4.Subtract(to, from), seconds);

            return velocity;
        }

        public static float Sign(float a)
        {
            return a < 0 ? -1 : 1;
        }

        public static void ApplyVelocity(ref Vector2 position, Vector2 Velocity, GameTime gameTime)
        {
            position.X += (float)((double)Velocity.X * gameTime.ElapsedGameTime.TotalSeconds);
            position.Y += (float)((double)Velocity.Y * gameTime.ElapsedGameTime.TotalSeconds);
        }

        public static void ApplyVelocity(ref Vector4 position, Vector4 Velocity, GameTime gameTime)
        {
            position = Vector4.Add(position, Vector4.Multiply(Velocity, (float)gameTime.ElapsedGameTime.TotalSeconds));
        }

        public static Vector2 MultiplyVector(Vector2 a, Vector2 b)
        {
            return new Vector2(a.X * b.X, a.Y * b.Y);
        }

        public static Vector2 CalculateSign(Vector2 from, Vector2 to)
        {
            Vector2 subtract = Vector2.Subtract(to, from);
            subtract.X = Sign(subtract.X);
            subtract.Y = Sign(subtract.Y);
            return subtract;
        }

        public static float CalculateSign(float from, float to)
        {
            return Sign(to - from);
        }

        public static Vector4 CalculateSign(Vector4 from, Vector4 to)
        {
            Vector4 subtract = Vector4.Subtract(to, from);
            subtract.X = Sign(subtract.X);
            subtract.Y = Sign(subtract.Y);
            subtract.Z = Sign(subtract.Z);
            subtract.W = Sign(subtract.W);
            return subtract;
        }

        public static Vector2 VectorAbs(Vector2 a)
        {
            return new Vector2(Math.Abs(a.X), Math.Abs(a.Y));
        }

        public static Vector4 VectorAbs(Vector4 a)
        {
            return new Vector4(Math.Abs(a.X), Math.Abs(a.Y), Math.Abs(a.Z), Math.Abs(a.W));
        }
    }
}
