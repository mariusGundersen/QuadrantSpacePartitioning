using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using QuadrantSpacePartitioning;
using Shouldly;

namespace QuadrantSpacePartitioningTest
{
    [TestFixture]
    public class UnitTests
    {
        [Test]
        public void TestBasic()
        {
            var tree = Partition.IntoTree(new List<IAmAPoint>
            {
                new Point2D {X = 0, Y = 0},
                new Point2D {X = -2, Y = -3},
                new Point2D {X = -1, Y = 4},
                new Point2D {X = 3, Y = -2},
                new Point2D {X = 4, Y = 1}
            });

            tree.X.ShouldBe(0);
            tree.Y.ShouldBe(0);

            tree.Points.Count().ShouldBe(5);
            tree.NorthEast.Points.Where(p => p.X < 0 && p.Y < 0).ShouldBeEmpty();
            tree.NorthWest.Points.Where(p => p.X > 0 && p.Y < 0).ShouldBeEmpty();
            tree.SouthWest.Points.Where(p => p.X > 0 && p.Y > 0).ShouldBeEmpty();
            tree.SouthEast.Points.Where(p => p.X < 0 && p.Y > 0).ShouldBeEmpty();
        }

        [Test]
        public void TestDeeper()
        {
            var tree = Partition.IntoTree(new List<IAmAPoint>
            {
                new Point2D {X = 0, Y = 0},
                
                new Point2D {X = -2, Y = -2},
                new Point2D {X = -1, Y = -1},
                new Point2D {X = -1, Y = -3},
                new Point2D {X = -3, Y = -3},
                new Point2D {X = -3, Y = -1},

                new Point2D {X = -10, Y = 11},
                new Point2D {X = 13, Y = -12},
                new Point2D {X = 14, Y = 11}
            });

            tree.X.ShouldBe(0);
            tree.Y.ShouldBe(0);

            tree.Points.Count().ShouldBe(9);

            tree.SouthWest.NorthEast.Points.Where(p => p.X < -2 && p.Y < -2).ShouldBeEmpty();
            tree.SouthWest.NorthWest.Points.Where(p => p.X > -2 && p.Y < -2).ShouldBeEmpty();
            tree.SouthWest.SouthWest.Points.Where(p => p.X > -2 && p.Y > -2).ShouldBeEmpty();
            tree.SouthWest.SouthEast.Points.Where(p => p.X < -2 && p.Y > -2).ShouldBeEmpty();
        }

        [Test]
        public void TestFinder()
        {
            var point2Ds = new List<IAmAPoint>();
            var rand = new Random();
            for (var i = 0; i < 1000; i++)
            {
                point2Ds.Add(new Point2D { X = rand.Next(0, 1000), Y = rand.Next(0, 1000) });
            }

            var tree = Partition.IntoQuadrants(point2Ds);

            var testPoint = new Point2D { X = rand.Next(0, 1000), Y = rand.Next(0, 1000) };

            tree.FindClosestPointTo(testPoint).ShouldBe(point2Ds.OrderBy(p => (p.X - testPoint.X) * (p.X - testPoint.X) + (p.Y - testPoint.Y) * (p.Y - testPoint.Y)).First());
        }

        [Test]
        public void TestEdgeCase()
        {
            var points = new List<IAmAPoint>
            {
                new Point2D {X = 0, Y = 0},
                new Point2D {X = (decimal) -2.1, Y = (decimal) -2.1},
                new Point2D {X = (decimal) -0.3, Y = (decimal) 0.6},
                new Point2D {X = (decimal) 2.1, Y = (decimal) -2.1},
                new Point2D {X = (decimal) 2.1, Y = (decimal) 2.1}
            };
            var tree = Partition.IntoTree(points);
            tree.SouthWest.Points.Count().ShouldBe(4);

            var testPoint = new Point2D { X = -1, Y = (decimal)-0.1 };

            tree.FindClosestPointTo(testPoint).ShouldBe(points[2]);
        }
    }

    public class Point2D : IAmAPoint
    {
        public decimal X { get; set; }
        public decimal Y { get; set; }

        public override string ToString()
        {
            return String.Format("({0}, {1})", X, Y);
        }
    }
}
