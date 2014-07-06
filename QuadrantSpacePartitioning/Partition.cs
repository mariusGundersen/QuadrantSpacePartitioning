using System;
using System.Collections.Generic;
using System.Linq;

namespace QuadrantSpacePartitioning
{
    public class Partition
    {
        private static readonly Vector NorthEast = new Vector(1, 1);
        private static readonly Vector NorthWest = new Vector(-1, 1);
        private static readonly Vector SouthWest = new Vector(-1, -1);
        private static readonly Vector SouthEast = new Vector(1, -1);

        public static ICanFindTheClosestPoint IntoQuadrants(IList<IAmAPoint> points)
        {
            return IntoTree(points);
        }
        
        public static Node IntoTree(IList<IAmAPoint> points)
        {
            return RecursivelyPartition(points, points, null);
        }

        private static Node RecursivelyPartition(IList<IAmAPoint> probablyClosePoints, IList<IAmAPoint> pointsInQuadrant, IAmAPoint parentCenter)
        {
            var avgX = pointsInQuadrant.Average(p => p.X);
            var avgY = pointsInQuadrant.Average(p => p.Y);

            var center = pointsInQuadrant.OrderBy(DistanceTo(avgX, avgY)).First();

            if (parentCenter == center || pointsInQuadrant.Count() <= 2)
            {
                return new Node
                {
                    Points = probablyClosePoints
                };
            }

            var northEast = pointsInQuadrant.Where(p => p.X >= center.X && p.Y >= center.Y).ToList();
            var northWest = pointsInQuadrant.Where(p => p.X <= center.X && p.Y >= center.Y).ToList();
            var southWest = pointsInQuadrant.Where(p => p.X <= center.X && p.Y <= center.Y).ToList();
            var southEast = pointsInQuadrant.Where(p => p.X >= center.X && p.Y <= center.Y).ToList();

            var notNorthEast = probablyClosePoints.Where(PointIsOnThisSideOf(center, NorthEast)).ToList();
            var notNorthWest = probablyClosePoints.Where(PointIsOnThisSideOf(center, NorthWest)).ToList();
            var notSouthWest = probablyClosePoints.Where(PointIsOnThisSideOf(center, SouthWest)).ToList();
            var notSouthEast = probablyClosePoints.Where(PointIsOnThisSideOf(center, SouthEast)).ToList();
            
            return new Node
            {
                X = center.X,
                Y = center.Y,
                Points = probablyClosePoints,
                NorthEast = RecursivelyPartition(notSouthWest, northEast, center),
                NorthWest = RecursivelyPartition(notSouthEast, northWest, center),
                SouthWest = RecursivelyPartition(notNorthEast, southWest, center),
                SouthEast = RecursivelyPartition(notNorthWest, southEast, center)
            };
        }

        private static Func<IAmAPoint, bool> PointIsOnThisSideOf(IAmAPoint center, Vector v)
        {
            var n = v.LeftNormal();
            var l = v.Rotate(22.5);
            var r = v.Rotate(-22.5);

            return p =>
            {
                var x = p.X - center.X;
                var y = p.Y - center.Y;
                return x*n.X + y*n.Y < 0
                    ? x*l.X + y*l.Y <= 0
                    : x*r.X + y*r.Y <= 0;
            };
        }

        private static Func<IAmAPoint, decimal> DistanceTo(decimal x, decimal y)
        {
            return p => p.DistanceTo(x, y);
        }

        private class Vector : IAmAPoint
        {
            private Vector(decimal x, decimal y)
            {
                X = x;
                Y = y;
            }

            private Vector(double x, double y)
            {
                X = (decimal) x;
                Y = (decimal) y;
            }

            public Vector(int x, int y)
            {
                X = x;
                Y = y;
            }

            public decimal X { get; set; }
            public decimal Y { get; set; }

            public Vector LeftNormal()
            {
                return new Vector(-Y, X);
            }

            public Vector Rotate(double degrees)
            {
                var r = degrees*Math.PI/180;
                var x = (double) X;
                var y = (double) Y;
                return new Vector(x*Math.Cos(r) - y*Math.Sin(r), x*Math.Sin(r) + y*Math.Cos(r));
            }
        }
    }
}