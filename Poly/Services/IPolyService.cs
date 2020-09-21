using Poly.Models;

namespace Poly.Services
{
    public interface IPolyService
    {
        bool InPolygon(PolygonData data);
    }
}