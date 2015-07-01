using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InvasionWar.GameEntities.Invisible.Effects.GraphFunctions
{
    public class ConstantGraphFunction : GraphFunction
    {
        public double antiDerivative(double x)
        {
            return x;
        }
        public ConstantGraphFunction(float duration) 
        {
            this.AddSegment(new PartialGraph(0, duration, antiDerivative));            
        }

        public override Vector2 ApplyVelocity(TimeSpan from, TimeSpan to, TimeSpan totalDuration, Vector2 totalDistance)
        {
            float deltaTime = (float)(to.TotalSeconds - from.TotalSeconds);
            totalDistance = Vector2.Divide(totalDistance, (float)totalDuration.TotalSeconds);
            return Vector2.Multiply(totalDistance, deltaTime);
        }
    }
}
