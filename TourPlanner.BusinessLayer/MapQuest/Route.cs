namespace TourPlanner.BusinessLayer.MapQuest
{
    public class Route
    {
        public string sessionID;//Cant be a GUId object because mapquest doesn't send a valid GUID
        public double distance;
    }
}
