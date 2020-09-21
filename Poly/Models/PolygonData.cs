using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Poly.Models
{
    public class PolygonData
    {
        public Point Point { get; set; }
        public Point[] Polygon { get; set; }
        public string Name { get; set; }
    }
}
