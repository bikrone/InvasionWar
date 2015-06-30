using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InvasionWar.Helper
{
    public class PolygonCollider
    {       
        public List<Vector2> vertices;

        public bool Inside(Vector2 point)
        {
            int nvert = vertices.Count;
            float testy = point.Y;
            float testx = point.X;

            int i, j;
            bool c = false;

            for (i = 0, j = nvert - 1; i < nvert; j = i++)
            {
                if (((vertices[i].Y > testy) != (vertices[j].Y > testy)) &&
                 (testx < (vertices[j].X - vertices[i].X) * (testy - vertices[i].Y) / (vertices[j].Y - vertices[i].Y) + vertices[i].X))
                    c = !c;
            }

            return c;
        }

        public PolygonCollider()
        {
            vertices = new List<Vector2>();
        }

        public void AddVertex(Vector2 vertex)
        {
            vertices.Add(vertex);
        }
    }
}
