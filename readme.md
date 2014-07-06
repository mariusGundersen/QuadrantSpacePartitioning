# Quadrant Space Partitioning

This is an implementation of an algorithm for recursively partitioning a set of points in a 2D space into quadrants,
so that the closest of these points to a test point can be found quickly. 

## Installation

Install using NuGet:

Install as a [Nuget package](https://www.nuget.org/packages/QuadrantSpacePartitioning/):

```ps
Install-Package QuadrantSpacePartitioning
```

## Usage

1. Create an implementation of the interface `IAmAPoint` (so that you can attach other information to the 2d point). 
2. Generate a list of points
3. Create a space of partitioned points
4. Find the closest point in the space

```csharp

List<IAmAPoint> points = GetListOfPointsSomehow();

var space = Partition.IntoQuadrants(points);

//later...

IAmAPoint aPoint = GetTestPointFromSomewhere();

space.FindClosestPointTo(aPoint);

```

## Original Sketches

![Sketch](https://raw.githubusercontent.com/mariusGundersen/QuadrantSpacePartitioning/master/documentation/sketches.jpg)