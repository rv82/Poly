using Poly.Models;
using Poly.Services;
using Xunit;

namespace Poly.Tests
{
    public class PolyServiceTests
    {
        private readonly IPolyService _polyService;
        private readonly Point[] _polygonPoints;

        public PolyServiceTests()
        {
            _polyService = new PolyService();
            _polygonPoints = new Point[]
            {
                new Point{ X=143, Y=100 },
                new Point{ X=95, Y=270 },
                new Point{ X=95, Y=317 },
                new Point{ X=425, Y=155 },
                new Point{ X=269, Y=57 }
            };
        }

        /// <summary>
        /// Данный тест проверяет, что все передаваемые точки находятся внутри многоугольника.
        /// Через параметры передаются координаты точки, которую нужно проверить.
        /// </summary>
        /// <param name="x">абсцисса точки</param>
        /// <param name="y">ордината точки</param>
        [Theory]
        [InlineData(287, 156)]
        [InlineData(322, 160)]
        [InlineData(192, 233)]
        [InlineData(200, 130)]
        [InlineData(344, 120)]
        public void Point_In_Polygon(double x, double y)
        {
            PolygonData data = new PolygonData
            {
                Polygon = _polygonPoints,
                Point = new Point { X = x, Y = y }
            };
            bool result = _polyService.InPolygon(data);
            Assert.True(result);
        }

        /// <summary>
        /// Данный тест проверяет, что все передаваемые точки находятся вне многоугольника.
        /// Через параметры передаются координаты точки, которую нужно проверить.
        /// </summary>
        /// <param name="x">абсцисса точки</param>
        /// <param name="y">ордината точки</param>
        [Theory]
        [InlineData(94, 162)]
        [InlineData(170, 28)]
        [InlineData(392, 69)]
        [InlineData(438, 258)]
        [InlineData(167, 328)]
        public void Point_Not_In_Polygon(double x, double y)
        {
            PolygonData data = new PolygonData
            {
                Polygon = _polygonPoints,
                Point = new Point { X = x, Y = y }
            };
            bool result = _polyService.InPolygon(data);
            Assert.False(result);
        }

        /// <summary>
        /// Контрольная точка лежит на одной из вершин многоугольника.
        /// Для алгоритма она лежит вне многоугольника.
        /// Через параметры передаются координаты точки, которую нужно проверить.
        /// </summary>
        /// <param name="x">абсцисса точки</param>
        /// <param name="y">ордината точки</param>
        [Theory]
        [InlineData(143, 100)]
        public void Point_On_Polygon_Vertex(double x, double y)
        {
            PolygonData data = new PolygonData
            {
                Polygon = _polygonPoints,
                Point = new Point { X = x, Y = y }
            };
            bool result = _polyService.InPolygon(data);
            Assert.False(result);
        }

        /// <summary>
        /// Контрольная точка лежит на одном из рёбер многоугольника.
        /// Для алгоритма она лежит вне многоугольника.
        /// Через параметры передаются координаты точки, которую нужно проверить.
        /// </summary>
        /// <param name="x">абсцисса точки</param>
        /// <param name="y">ордината точки</param>
        [Theory]
        [InlineData(95, 300)]
        public void Point_On_Polygon_Edge(double x, double y)
        {
            PolygonData data = new PolygonData
            {
                Polygon = _polygonPoints,
                Point = new Point { X = x, Y = y }
            };
            bool result = _polyService.InPolygon(data);
            Assert.False(result);
        }
    }
}