using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace QuadrantSpacePartitioning
{
    public class Node
    {
        public IEnumerable<IAmAPoint> Points { get; set; }
        public Node NorthEast { get; set; }
        public Node NorthWest { get; set; }
        public Node SouthWest { get; set; }
        public Node SouthEast { get; set; }
        public decimal X { get; set; }
        public decimal Y { get; set; }

        public IAmAPoint FindClosestTo(IAmAPoint point)
        {
            if (NorthEast == null)
            {
                Debug.WriteLine("{0} points to scan through", Points.Count());
                return Points.OrderBy(p => p.DistanceTo(point.X, point.Y)).FirstOrDefault();
            }
            
            if (point.X >= X && point.Y >= Y)
            {
                Debug.WriteLine("NorthEast ({0}, {1}) compared to {2}, {3}, {4} point", X, Y, point.X, point.Y, Points.Count());
                return NorthEast.FindClosestTo(point);
            }
            if (point.X <= X && point.Y >= Y)
            {
                Debug.WriteLine("NortWest ({0}, {1}) compared to {2}, {3}, {4} point", X, Y, point.X, point.Y, Points.Count());
                return NorthWest.FindClosestTo(point);
            }
            if (point.X <= X && point.Y <= Y)
            {
                Debug.WriteLine("SouthWest ({0}, {1}) compared to {2}, {3}, {4} point", X, Y, point.X, point.Y, Points.Count());
                return SouthWest.FindClosestTo(point);
            }
            if (point.X >= X && point.Y <= Y)
            {
                Debug.WriteLine("SouthEast ({0}, {1}) compared to {2}, {3}, {4} point", X, Y, point.X, point.Y, Points.Count());
                return SouthEast.FindClosestTo(point);
            }

            throw new Exception("This cannot happen");
        }
    }
}
