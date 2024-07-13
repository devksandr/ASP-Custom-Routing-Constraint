using System.Globalization;

namespace ASP_Custom_Routing_Constraint.RouteConstraints
{
    public class PositiveOddDoubleNumberRouteConstraint : IRouteConstraint
    {
        public bool Match(HttpContext? httpContext, IRouter? route, string routeKey, RouteValueDictionary values, RouteDirection routeDirection)
        {
            if (!values.TryGetValue(routeKey, out var routeValue))
            {
                return false;
            }

            var routeValueString = Convert.ToString(routeValue, CultureInfo.InvariantCulture);

            if (routeValueString is null)
            {
                return false;
            }

            if (routeValueString.Length != 2)
            {
                return false;
            }

            if (!int.TryParse(routeValueString, out var routeNumber))
            {
                return false;
            }

            if (routeNumber <= 0)
            {
                return false;
            }

            return routeNumber % 2 != 0;
        }
    }
}
