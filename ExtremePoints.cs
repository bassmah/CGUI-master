using CGUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGAlgorithms.Algorithms.ConvexHull
{
    public class ExtremePoints : Algorithm
    {
        public override void Run(List<Point> points, List<Line> lines, List<Polygon> polygons, ref List<Point> outPoints, ref List<Line> outLines, ref List<Polygon> outPolygons)
        {
            bool[] notExtreme = new bool[(points.Count)];
            //points.Sort(delegate (Point c1, Point c2) { return c1.X.CompareTo(c2.X); });
            for (int i = 0; i < points.Count; i++)
            {
                if (notExtreme[i] == true) continue;
                for (int j = 0; j < points.Count; j++)
                {
                    if (notExtreme[j] == true || j == i) continue;
                    if (points[i] == points[j])
                    {
                        notExtreme[i] = true; continue;
                    }
                    double ij = HelperMethods.get_length(points[i], points[j]);
                    for (int k = 0; k < points.Count; k++)
                    {
                        if (notExtreme[k] == true || k == j || k == i) continue;
                        double jk = HelperMethods.get_length(points[j], points[k]);
                        double ik = HelperMethods.get_length(points[i], points[k]);
                        if (Math.Abs(ik + jk - ij) < Constants.Epsilon) { notExtreme[k] = true; continue; }
                        if (Math.Abs(ij + jk - ik) < Constants.Epsilon) { notExtreme[j] = true; break; }
                        if (Math.Abs(ij + ik - jk) < Constants.Epsilon) { notExtreme[i] = true; break; }
                        for (int t = 0; t < points.Count; t++)
                        {
                            if (notExtreme[t] == true || t == j || t == i || t == k) continue;
                            if (HelperMethods.PointInTriangle(points[t], points[i], points[j], points[k])
                                == Enums.PointInPolygon.Inside)
                                notExtreme[t] = true;
                        }
                    }
                }
            }
            for (int i = 0; i < points.Count; i++)
                if (notExtreme[i] == false)
                    outPoints.Add(points[i]);
        }

        public override string ToString()
        {
            return "Convex Hull - Extreme Points";
        }
    }
}

