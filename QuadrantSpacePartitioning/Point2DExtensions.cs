namespace QuadrantSpacePartitioning
{
    internal static class Point2DExtensions
    {
        public static decimal DistanceTo(this IAmAPoint p, decimal x, decimal y)
        {
            return (p.X - x) * (p.X - x) + (p.Y - y) * (p.Y - y);
        }
    }
}
