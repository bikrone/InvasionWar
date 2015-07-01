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
        public List<PartialGraph> partials;

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

        public float Area()
        {
            return Integral(Left, Right);
        }
    }
}
