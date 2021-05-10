using System;
using System.Collections.Generic;
using System.Text;

namespace TourPlanner.BusinessLayer
{
    public class Route
    {
        public string sessionID;//Cant be a GUId object because mapquest doesn't send a valid GUID
        public double distance;
    }
}
