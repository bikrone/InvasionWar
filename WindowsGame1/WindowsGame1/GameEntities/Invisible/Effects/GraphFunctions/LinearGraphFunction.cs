using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InvasionWar.GameEntities.Invisible.Effects.GraphFunctions
{
    public class LinearGraphFunction : GraphFunction
    {
        public float antiDerivative(float x)
        {
            return x * x / 2.0f;
        }
        public LinearGraphFunction()
        {
            this.AddSegment(new PartialGraph(0, 1, antiDerivative));
        }
    }
}
