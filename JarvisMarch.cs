using CGUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGAlgorithms.Algorithms.ConvexHull
{
    public class JarvisMarch : Algorithm
    {

        public override void Run(List<Point> points, List<Line> lines, List<Polygon> polygons, ref List<Point> outPoints, ref List<Line> outLines, ref List<Polygon> outPolygons)
        {
            int numberOfPoints = points.Count;

            if (numberOfPoints < 3)
            {
                outPoints = points;
                return;
            }

            int minY = 0;
            for (int i = 1; i < numberOfPoints; i++)
                if (points[i].Y < points[minY].Y)
                    minY = i;

            int start = minY;
            int end;

            outPoints.Add(points[start]);
            while (true)
            {
                end = (start + 1) % numberOfPoints;

                for (int i = 0; i < numberOfPoints; i++)
                {
                    if (i == start || i == end) continue;

                    if (HelperMethods.CheckTurn(new Line(points[start], points[i]), points[end]) == Enums.TurnType.Colinear
                        && HelperMethods.get_length(points[start], points[i]) > HelperMethods.get_length(points[start], points[end]))
                        end = i;

                    if (HelperMethods.CheckTurn(new Line(points[start], points[i]), points[end]) == Enums.TurnType.Right)
                        end = i;
                }

                start = end;

                if (start == minY)
                    break;

                outPoints.Add(points[start]);
            }

        }

        public override string ToString()
        {
            return "Convex Hull - Jarvis March";
        }
    }
}

