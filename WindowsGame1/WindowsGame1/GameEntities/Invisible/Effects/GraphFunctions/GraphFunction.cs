using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InvasionWar.GameEntities.Invisible.Effects.GraphFunctions
{
    public delegate float AntiDerivativeFunction(float x);

    public class PartialGraph
    {
        public float from;
        public float to;

        public AntiDerivativeFunction antiDerivative;

        public PartialGraph(float from, float to, AntiDerivativeFunction antiDerivative)
        {
            this.from = from; this.to = to; this.antiDerivative = antiDerivative;
        }

        public float Integral(float l, float r)
        {
            return antiDerivative(r) - antiDerivative(l);
        }
    }

    // Graph function line Constant, Linear, Spline...    
    public class GraphFunction
    {
        private float Left, Right;
        public List<PartialGraph> partials = new List<PartialGraph>();
        public float Area;

        public void AddSegment(PartialGraph partial)
        {
            partials.Add(partial);
            if (partials.Count == 1)
            {
                Left = partial.from;
                Right = partial.to;
            }
            else
            {
                Left = Math.Min(partial.from, Left);
                Right = Math.Max(partial.to, Right);
            }

            Area = Integral(Left, Right);
        }

        public float Integral(float l, float r)
        {
            float ll, rr, integral=0;
            
            foreach (var partial in partials)
            {
                ll = l; rr = r;

                if (ll > partial.to) continue;

                if (ll < partial.from) ll = partial.from;                

                if (r > partial.to)
                {
                    rr = partial.to;                    
                }

                integral += partial.Integral(ll, rr);

                if (rr == r) break;
            }

            return integral;
        }


        public Vector2 ApplyVelocity(TimeSpan from, TimeSpan to, TimeSpan totalDuration, Vector2 totalDistance)
        {
            if (from > totalDuration)
            {
                from = from.Subtract(totalDuration);
                to = to.Subtract(totalDuration);
            }
            else
            {
                if (to > totalDuration) to = totalDuration;
            }
            
            float scaleTime = ((float)totalDuration.TotalSeconds / (Right - Left));
            Vector2 scaleDistance = Vector2.Divide(totalDistance, Area);            
            
            float l = Left+(float)from.TotalSeconds / scaleTime;
            float r = Left+(float)to.TotalSeconds / scaleTime;

            float distance = Integral(l, r);
            Vector2 deltaDistance = Vector2.Multiply(scaleDistance, distance);

            return deltaDistance;
        }


        public Vector4 ApplyVelocity(TimeSpan from, TimeSpan to, TimeSpan totalDuration, Vector4 totalDistance)
        {

            float scaleTime = ((float)totalDuration.TotalSeconds / (Right - Left));
            Vector4 scaleDistance = Vector4.Divide(totalDistance, Area);

            float l = (float)from.TotalSeconds / scaleTime;
            float r = (float)to.TotalSeconds / scaleTime;

            float distance = Integral(l, r);
            Vector4 deltaDistance = Vector4.Multiply(scaleDistance, distance);

            return deltaDistance;
        }
    }
}
