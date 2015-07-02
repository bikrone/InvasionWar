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
    public class RotationAnimation : Animation
    {
        public float toDegree;
        public float fromDegree;

        public float originalPosition;

        private float currentDegree;

        private float velocity = new float();

        private float Sign = 1;

        private float totalDistance;

        public override void Update(GameTime gameTime)
        {
            if (!isStarted) return;
            // UtilityHelper.ApplyVelocity(ref currentDegree, UtilityHelper.MultiplyVector(velocity, Sign), gameTime);
            float deltaDistance = graphFunction.ApplyVelocity(CurrentTime, CurrentTime.Add(gameTime.ElapsedGameTime), Duration, totalDistance);

            CurrentTime = CurrentTime.Add(gameTime.ElapsedGameTime);
            // if (CurrentTime > Duration) CurrentTime = TimeSpan.Zero;
            deltaDistance = deltaDistance * Sign;

            currentDegree += deltaDistance;

            if (!isInfinite)
            {
                currentDegree = MathHelper.Clamp(currentDegree, Math.Min(fromDegree, toDegree), Math.Max(fromDegree, toDegree));
            }
            else
            {
                while (currentDegree > MathHelper.Pi * 2) currentDegree -= MathHelper.Pi * 2;
                while (currentDegree < -MathHelper.Pi * 2) currentDegree += MathHelper.Pi * 2;
            }


            sprite.SetRotation(currentDegree);

            if (isLoop)
            {
      
                if (currentDegree == fromDegree || currentDegree == toDegree)
                {
                    Sign *= -1;
                    CurrentTime = TimeSpan.Zero;
                }
                             
            }
            else
            {
                if (currentDegree == toDegree)
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
                sprite.SetRotation(this.originalPosition);
            }

            storyboard.SendCompleted();
        }
        public override void Start()
        {
            isStarted = true;

            this.originalPosition = sprite.GetRotation();

            if (duration == 0)
            {
                sprite.SetRotation(this.toDegree);
                if (!isInfinite) this.Stop();
                return;
            }

            if (isFromNull) fromDegree = this.originalPosition;

            Sign = UtilityHelper.CalculateSign(this.fromDegree, this.toDegree);

            currentDegree = this.fromDegree;

            if (isAnimatedFromOrigin)
            {
                currentDegree = this.originalPosition;
                Sign = UtilityHelper.CalculateSign(currentDegree, this.fromDegree);
            }
            else
            {
                sprite.SetRotation(this.currentDegree);
            }

            CurrentTime = TimeSpan.Zero;
            Duration = TimeSpan.FromSeconds(duration);
            totalDistance = Math.Abs(toDegree- fromDegree);

            if (graphFunction == null)
                graphFunction = new ConstantGraphFunction(duration);
        }

        public RotationAnimation(Storyboard storyboard, Sprite2D sprite, float duration, float toDegree, bool isReserveProperty = true, float? fromDegree = null, bool isAnimatedFromOrigin = false, bool isLoop = false, bool isInfinite = false)
        {
            this.isInfinite = isInfinite;
            this.isLoop = isLoop;
            this.storyboard = storyboard;
            this.sprite = sprite;
            this.duration = duration;
            this.isReserveProperty = isReserveProperty;
            this.toDegree = toDegree;

            isFromNull = fromDegree == null;

            if (!isFromNull)
            {
                this.fromDegree = fromDegree.Value;
            }

        }
    }
}
