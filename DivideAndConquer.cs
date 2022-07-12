using CGUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGAlgorithms.Algorithms.ConvexHull
{
    public class DivideAndConquer : Algorithm
    {
        public static List<Point> ClockSort(ref List<Point> points)
        {
            int numberOfPoints = points.Count;

            if (numberOfPoints < 3)
                return points;
            int minY = 0;
            for (int i = 1; i < numberOfPoints; i++)
                if (points[i].Y < points[minY].Y)
                    minY = i;
            int start = minY;
            int end;
            List<Point> outPoints = new List<Point>();
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
            outPoints.Reverse();
            return outPoints;
        }
        public static Tuple<int,int> GetMaxMin(ref List<Point> A, ref List<Point> B)
        {
            Tuple<int, int> ans = new Tuple<int, int>((int)A[0].X, (int)B[0].X);
            Tuple<int, int> j = new Tuple<int, int>(0, 0);
            for (int i = 1; i < A.Count; i++) {
                if (ans.Item1 < (int)A[i].X)
                {//Max A
                    ans = new Tuple<int, int>((int)A[i].X, ans.Item2);
                    j = new Tuple<int, int>(i, j.Item2);
                }
            }
            for (int i = 1; i < B.Count; i++){
                if (ans.Item2 > (int)B[i].X)
                {//Min B
                    ans = new Tuple<int, int>(ans.Item1, (int)B[i].X);
                    j = new Tuple<int, int>(j.Item1, i);
                }
            }
            return j;
        }
        public static Tuple<int, int> GetUpper(ref List<Point> A, ref List<Point> B, Tuple<int, int> idx)
        {
            int inda = idx.Item1, indb = idx.Item2;
            int n1 = A.Count, n2 = B.Count,ffa=0,ffb=0;
            bool done = false;
            while (!done)
            {
                done = true;
                int co = 0;
                while (HelperMethods.orientation(B[indb], A[inda], A[(inda + 1) % n1]) >= 0)
                {
                    inda = (inda + 1) % n1;
                    if (co > A.Count)
                    {
                        inda = idx.Item1;ffa = 1;
                        break; 
                    }
                    co++;
                }
                co = 0;
                while (HelperMethods.orientation(A[inda], B[indb], B[(n2 + indb - 1) % n2]) <=0)
                {
                    indb = (n2 + indb - 1) % n2;
                    done = false;
                    if (co > B.Count)
                    {
                        indb = idx.Item2;ffb = 1;
                        break;
                    }
                    co++;
                }
                if (ffa==1 || ffb==1) break;
            }
            return new Tuple<int, int>(inda, indb);
        }
        public static Tuple<int, int> GetLower(ref List<Point> A, ref List<Point> B, Tuple<int, int> idx)
        {
            int inda = idx.Item1, indb = idx.Item2;
            int n1 = A.Count, n2 = B.Count,ffa=0,ffb=0;
            bool done = false;
            while (!done)
            {
                done = true;
                int co = 0;
                while (HelperMethods.orientation(A[inda], B[indb], B[(indb + 1) % n2]) >= 0)
                {
                    indb = (indb + 1) % n2;
                    if (co > B.Count)
                    {
                        indb = idx.Item2;ffb = 1;
                        break;
                    }
                    co++;
                }
                co = 0;
                while (HelperMethods.orientation(B[indb], A[inda], A[(n1 + inda - 1) % n1]) <=0)
                {
                    inda = (n1 + inda - 1) % n1;
                    done = false;
                    if (co > A.Count)
                    {
                        inda = idx.Item1;ffa = 1;
                        break;
                    }
                    co++;
                }
                if (ffa==1 || ffb==1) break;
            }
            int lowera = inda, lowerb = indb;
            return new Tuple<int, int>(inda, indb);
        }
        public static List<Point> Merge(ref List<Point> A,ref List<Point> B)
        {
            //sort clock wishe and merge 
            List<Point> Ans = new List<Point>();
            Tuple<int, int> idx = GetMaxMin(ref A,ref B);
            Tuple<int, int> upper = GetUpper(ref A, ref B, idx);
            Tuple<int, int> lower = GetLower(ref A, ref B, idx);
            int ind = upper.Item1;
            Ans.Add(A[ind]);
            while (ind != lower.Item1)
            {
                ind = (ind + 1) % A.Count;
                Ans.Add(A[ind]);
            }

            ind = lower.Item2;
            Ans.Add(B[ind]);
            while (ind != upper.Item2)
            {
                ind = (ind + 1) % B.Count;
                Ans.Add(B[ind]);
            }
            A.Clear(); A = null;
            B.Clear(); B = null;
            return Ans;
        }
        public static List<Point> solve(ref List<Point> Points,int start,int end)
        {
            if (end-start+1 <= 5)
            {
                List<Point> Temp = new List<Point>();
                for (int i = start; i <= end; i++)
                    Temp.Add(Points[i]);
                return ClockSort(ref Temp);
            }

            List<Point> A=null,B=null;
            int mid = (start + end) / 2;
            A = solve(ref Points,start,mid);
            B = solve(ref Points,mid+1,end);
            return Merge(ref A,ref B);
        }
        public override void Run(List<Point> points, List<Line> lines, List<Polygon> polygons, ref List<Point> outPoints, ref List<Line> outLines, ref List<Polygon> outPolygons)
        {
            points.Sort(delegate (Point p1, Point p2) { return (p1.X!=p2.X)?p1.X.CompareTo(p2.X): p1.Y.CompareTo(p2.Y); });
            outPoints = solve(ref points,0,points.Count-1);
        }

        public override string ToString()
        {
            return "Convex Hull - Divide & Conquer";
        }

    }
}

