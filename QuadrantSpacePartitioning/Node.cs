using System;
using System.Collections.Generic;
using System.Linq;

namespace QuadrantSpacePartitioning
{
    public class Node : ICanFindTheClosestPoint
    {
        public IEnumerable<IAmAPoint> Points { get; set; }
        public Node NorthEast { get; set; }
        public Node NorthWest { get; set; }
        public Node SouthWest { get; set; }
        public Node SouthEast { get; set; }
        public decimal X { get; set; }
        public decimal Y { get; set; }

        public IAmAPoint FindClosestPointTo(IAmAPoint point)
        {
            if (NorthEast == null)
            {
                return Points.OrderBy(p => p.DistanceTo(point.X, point.Y)).FirstOrDefault();
            }
            if (point.X >= X && point.Y >= Y)
            {
                return NorthEast.FindClosestPointTo(point);
            }
            if (point.X <= X && point.Y >= Y)
            {
                return NorthWest.FindClosestPointTo(point);
            }
            if (point.X <= X && point.Y <= Y)
            {
                return SouthWest.FindClosestPointTo(point);
            }
            if (point.X >= X && point.Y <= Y)
            {
                return SouthEast.FindClosestPointTo(point);
            }

            throw new Exception("This cannot happen");
        }
    }
}
