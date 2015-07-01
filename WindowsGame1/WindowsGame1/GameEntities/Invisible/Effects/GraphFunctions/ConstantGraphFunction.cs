using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InvasionWar.GameEntities.Invisible.Effects.GraphFunctions
{
    public class ConstantGraphFunction : GraphFunction
    {
        public float antiDerivative(float x)
        {
            return x;
        }
        public ConstantGraphFunction() 
        {
            this.AddSegment(new PartialGraph(0, 1, antiDerivative));            
        }
    }
}
