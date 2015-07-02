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
    public class TranslationAnimation : Animation
    {        
        public Vector2 toPosition;
        public Vector2 fromPosition;

        public Vector2 originalPosition;

        private Vector2 currentPosition;

        private Vector2 velocity = new Vector2();

        private Vector2 Sign = new Vector2(1,1);

        private Vector2 totalDistance;

        public override void Update(GameTime gameTime)
        {
            if (!isStarted) return;        
            // UtilityHelper.ApplyVelocity(ref currentPosition, UtilityHelper.MultiplyVector(velocity, Sign), gameTime);
            Vector2 deltaDistance = graphFunction.ApplyVelocity(CurrentTime, CurrentTime.Add(gameTime.ElapsedGameTime), Duration, totalDistance);

            CurrentTime = CurrentTime.Add(gameTime.ElapsedGameTime);
            // if (CurrentTime > Duration) CurrentTime = TimeSpan.Zero;
            deltaDistance = Vector2.Multiply(deltaDistance, Sign);

            currentPosition = Vector2.Add(currentPosition, deltaDistance);

            currentPosition.X = MathHelper.Clamp(currentPosition.X, Math.Min(fromPosition.X, toPosition.X), Math.Max(fromPosition.X, toPosition.X));            
            currentPosition.Y = MathHelper.Clamp(currentPosition.Y, Math.Min(fromPosition.Y, toPosition.Y), Math.Max(fromPosition.Y, toPosition.Y));

            sprite.SetPosition(currentPosition);

            if (isLoop)
            {
                int completed = 0;
                if (currentPosition.X == fromPosition.X || currentPosition.X == toPosition.X)
                {
                    Sign.X *= -1;
                    completed++;
                }
                if (currentPosition.Y == fromPosition.Y || currentPosition.Y == toPosition.Y)
                {
                    Sign.Y *= -1;
                    completed++;
                }
                if (completed == 2)
                {
                    CurrentTime = TimeSpan.Zero;
                }
                
            }
            else
            {
                if (currentPosition.X == toPosition.X && currentPosition.Y == toPosition.Y)
                {
                    if (!isInfinite)
                        this.Stop();
                    return;
                }
            }

           
        }

        public override void Stop()
        {
            if (!isStarted) return;

            isStarted = false;
            if (isReserveProperty)
            {
                sprite.SetPosition(this.originalPosition);
            }

            storyboard.SendCompleted();
        }
        public override void Start()
        {
            isStarted = true;            
            
            this.originalPosition = new Vector2(sprite.Left, sprite.Top);

            if (duration == 0)
            {
                sprite.SetPosition(this.toPosition);
                if (!isInfinite) this.Stop();
                return;
            }

            if (isFromNull) fromPosition = this.originalPosition;

            Sign = UtilityHelper.CalculateSign(this.fromPosition, this.toPosition);

            currentPosition = this.fromPosition;

            if (isAnimatedFromOrigin)
            {
                currentPosition = this.originalPosition;
                Sign = UtilityHelper.CalculateSign(currentPosition, this.fromPosition);
            }
            else
            {
                sprite.SetPosition(this.currentPosition);
            }

            CurrentTime = TimeSpan.Zero;
            Duration = TimeSpan.FromSeconds(duration);
            totalDistance = UtilityHelper.VectorAbs(Vector2.Subtract(toPosition, fromPosition));

            if (graphFunction ==null)
                graphFunction = new ConstantGraphFunction(duration);
        }

        public TranslationAnimation(Storyboard storyboard, Sprite2D sprite, float duration, Vector2 toPosition, bool isReserveProperty = true, Vector2? fromPosition = null, bool isAnimatedFromOrigin = false, bool isLoop = false, bool isInfinite = false)
        {
            this.isInfinite = isInfinite;
            this.isLoop = isLoop;
            this.storyboard = storyboard;
            this.sprite = sprite;
            this.duration = duration;
            this.isReserveProperty = isReserveProperty;            
            this.toPosition = toPosition;

            isFromNull = fromPosition == null;

            if (!isFromNull)
            {
                this.fromPosition = fromPosition.Value;
            }            
            
        }
    }
}
