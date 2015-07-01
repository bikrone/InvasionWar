using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InvasionWar.GameEntities.Invisible.Effects.GraphFunctions
{
    public delegate double AntiDerivativeFunction(double x);

    public class PartialGraph
    {
        public double from;
        public double to;

        public AntiDerivativeFunction antiDerivative;

        public PartialGraph(double from, double to, AntiDerivativeFunction antiDerivative)
        {
            this.from = from; this.to = to; this.antiDerivative = antiDerivative;
        }

        public double Integral(double l, double r)
        {
            return antiDerivative(r) - antiDerivative(l);
        }
    }

    // Graph function line Constant, Linear, Spline...    
    public class GraphFunction
    {
        private double Left, Right;
        public List<PartialGraph> partials = new List<PartialGraph>();
        public double Area;

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

        public double Integral(double l, double r)
        {
            double ll, rr, integral=0;
            
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


        public virtual Vector2 ApplyVelocity(TimeSpan from, TimeSpan to, TimeSpan totalDuration, Vector2 totalDistance)
        {
            if (from > totalDuration)
            {
                while (from > totalDuration)
                {
                    from = from.Subtract(totalDuration);
                    to = to.Subtract(totalDuration);
                }
            }
            else
            {
                if (to > totalDuration) to = totalDuration;
            }
            
            double scaleTime = ((double)totalDuration.TotalSeconds / (Right - Left));
            Vector2 scaleDistance = Vector2.Divide(totalDistance, (float)Area);            
            
            double l = Left+(double)from.TotalSeconds / scaleTime;
            double r = Left+(double)to.TotalSeconds / scaleTime;

            double distance = Integral(l, r);
            Vector2 deltaDistance = Vector2.Multiply(scaleDistance, (float)distance);

            return deltaDistance;
        }


        public virtual Vector4 ApplyVelocity(TimeSpan from, TimeSpan to, TimeSpan totalDuration, Vector4 totalDistance)
        {
            if (from > totalDuration)
            {
                while (from > totalDuration)
                {
                    from = from.Subtract(totalDuration);
                    to = to.Subtract(totalDuration);
                }
            }
            else
            {
                if (to > totalDuration) to = totalDuration;
            }
            double scaleTime = ((double)totalDuration.TotalSeconds / (Right - Left));
            Vector4 scaleDistance = Vector4.Divide(totalDistance, (float)Area);

            double l = (double)from.TotalSeconds / scaleTime;
            double r = (double)to.TotalSeconds / scaleTime;

            double distance = Integral(l, r);
            Vector4 deltaDistance = Vector4.Multiply(scaleDistance, (float)distance);

            return deltaDistance;
        }
    }
}
