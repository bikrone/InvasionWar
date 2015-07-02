using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using InvasionWar.GameEntities.Visible;
using InvasionWar.Helper;
using InvasionWar.GameEntities.Invisible.Effects.GraphFunctions;

namespace InvasionWar.Effects.Animations
{
    public class ColorAnimation : Animation
    {                
        public Vector4 toColor;        
        public Vector4 fromColor;

        public Vector4 originalColor;

        private Vector4 currentColor;

        private Vector4 velocity = new Vector4();

        private Vector4 Sign = new Vector4(1, 1, 1, 1);

        Vector4 totalDistance;

        public override void Update(GameTime gameTime)
        {
            if (!isStarted) return;

            // UtilityHelper.ApplyVelocity(ref currentColor, Vector4.Multiply(velocity, Sign), gameTime);            
            Vector4 deltaDistance = graphFunction.ApplyVelocity(CurrentTime, CurrentTime.Add(gameTime.ElapsedGameTime), Duration, totalDistance);

            CurrentTime = CurrentTime.Add(gameTime.ElapsedGameTime);
            // if (CurrentTime > Duration) CurrentTime = TimeSpan.Zero;
            deltaDistance = Vector4.Multiply(deltaDistance, Sign);

            currentColor = Vector4.Add(currentColor, deltaDistance);

            currentColor.X = MathHelper.Clamp(currentColor.X, Math.Min(fromColor.X, toColor.X), Math.Max(fromColor.X, toColor.X));
            currentColor.Y = MathHelper.Clamp(currentColor.Y, Math.Min(fromColor.Y, toColor.Y), Math.Max(fromColor.Y, toColor.Y));
            currentColor.Z = MathHelper.Clamp(currentColor.Z, Math.Min(fromColor.Z, toColor.Z), Math.Max(fromColor.Z, toColor.Z));
            currentColor.W = MathHelper.Clamp(currentColor.W, Math.Min(fromColor.W, toColor.W), Math.Max(fromColor.W, toColor.W));

            sprite.SetOverlay(currentColor);

            if (isLoop)
            {
                int completed = 0;
                if (currentColor.X == fromColor.X || currentColor.X == toColor.X)
                {
                    Sign.X *= -1;
                    completed++;
                }
                if (currentColor.Y == fromColor.Y || currentColor.Y == toColor.Y)
                {
                    Sign.Y *= -1;
                    completed++;
                }
                if (currentColor.Z == fromColor.Z || currentColor.Z == toColor.Z)
                {
                    Sign.Z *= -1;
                    completed++;
                }
                if (currentColor.W == fromColor.W || currentColor.W == toColor.W)
                {
                    Sign.W *= -1;
                    completed++;
                }

                if (completed == 4)
                {
                    CurrentTime = TimeSpan.Zero;
                }
            }
            else
            {
                if (currentColor.X == toColor.X && currentColor.Y == toColor.Y && currentColor.Z == toColor.Z && currentColor.W == toColor.W)
                {
                    if (!isInfinite) this.Stop();
                    return;
                }
            }           
        }

        public override void Start() {
            isStarted = true;
            if (duration == 0)
            {
                sprite.SetOverlay(this.toColor);
                if (!isInfinite) this.Stop();
                return;
            }

            this.originalColor = sprite.GetOverlay();
            if (isFromNull) this.fromColor = this.originalColor;    

            // velocity = UtilityHelper.CalculateVelocity(this.fromColor, this.toColor, duration);
            Sign = UtilityHelper.CalculateSign(this.fromColor, this.toColor);

            currentColor = this.fromColor;

            if (isAnimatedFromOrigin)
            {
                currentColor = this.originalColor;
                Sign = UtilityHelper.CalculateSign(currentColor, this.fromColor);
            }
            else
            {
                sprite.SetOverlay(this.currentColor);
            }

            CurrentTime = TimeSpan.Zero;
            Duration = TimeSpan.FromSeconds(duration);
            totalDistance = UtilityHelper.VectorAbs(Vector4.Subtract(toColor, fromColor));

            if (graphFunction == null)
                graphFunction = new ConstantGraphFunction(duration);
        }

        public override void Stop()
        {
            if (!isStarted) return;

            isStarted = false;
            if (isReserveProperty)
            {
                sprite.SetOverlay(this.originalColor);
            }

            storyboard.SendCompleted();
        }


        public ColorAnimation(Storyboard storyboard, Sprite2D sprite, float duration, Vector4 toColor, bool isReserveProperty = true, Vector4? fromColor = null, bool isAnimatedFromOrigin = false, bool isLoop = false, bool isInfinite = false)
        {
            this.isInfinite = isInfinite;
            this.isAnimatedFromOrigin = isAnimatedFromOrigin;
            this.isLoop = isLoop;
            this.storyboard = storyboard;
            this.sprite = sprite;
            this.duration = duration;
            this.isReserveProperty = isReserveProperty;            
            this.toColor = toColor;

            if (!isFromNull)
            {
                this.fromColor = fromColor.Value;
            }

            isFromNull = fromColor == null;                                       
        }
    }
}
