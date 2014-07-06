using System;
using System.Collections.Generic;
using System.Linq;

namespace QuadrantSpacePartitioning
{
    public class Partition
    {
        public static Node IntoQuadrants(IList<IAmAPoint> points)
        {
            return IntoQuadrants(points, points, null);
        }

        private static Node IntoQuadrants(IList<IAmAPoint> probablyClosePoints, IList<IAmAPoint> pointsInQuadrant, IAmAPoint parentCenter)
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

            var notNorthEast = probablyClosePoints.Where(p => (p.X - center.X) + (p.Y - center.Y) <= 0).ToList();
            var notNorthWest = probablyClosePoints.Where(p => -(p.X - center.X) + (p.Y - center.Y) <= 0).ToList();
            var notSouthWest = probablyClosePoints.Where(p => -(p.X - center.X) + -(p.Y - center.Y) <= 0).ToList();
            var notSouthEast = probablyClosePoints.Where(p => (p.X - center.X) + -(p.Y - center.Y) <= 0).ToList();


            return new Node
            {
                X = center.X,
                Y = center.Y,
                Points = probablyClosePoints,
                NorthEast = IntoQuadrants(notSouthWest, northEast, center),
                NorthWest = IntoQuadrants(notSouthEast, northWest, center),
                SouthWest = IntoQuadrants(notNorthEast, southWest, center),
                SouthEast = IntoQuadrants(notNorthWest, southEast, center)
            };
        }

        private static Func<IAmAPoint, decimal> DistanceTo(decimal x, decimal y)
        {
            return p => p.DistanceTo(x, y);
        }
    }
}