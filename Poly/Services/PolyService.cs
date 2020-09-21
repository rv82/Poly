using Poly.Models;

namespace Poly.Services
{
    public class PolyService : IPolyService
    {
        public bool InPolygon(PolygonData data)
        {
            double x = data.Point.X;
            double y = data.Point.Y;

            // массив абсцисс вершин многоугольника
            double[] xArray = new double[data.Polygon.Length];
            // массив ординат вершин многоугольника
            double[] yArray = new double[data.Polygon.Length];

            for (int i = 0; i < data.Polygon.Length; i++)
            {
                xArray[i] = data.Polygon[i].X;
                yArray[i] = data.Polygon[i].Y;
            }

            int j = xArray.Length - 1;
            bool inPoly = false;
            for (int i = 0; i < xArray.Length; i++)
            {
                if (((yArray[i] <= y && y < yArray[j]) || (yArray[j] <= y && y < yArray[i])) &&
                    (yArray[j] - yArray[i]) != 0 &&
                    (x > (xArray[j] - xArray[i]) * (y - yArray[i]) / (yArray[j] - yArray[i]) + xArray[i]))
                {
                    inPoly = !inPoly;
                }
                j = i;
            }
            return inPoly;
        }
    }
}