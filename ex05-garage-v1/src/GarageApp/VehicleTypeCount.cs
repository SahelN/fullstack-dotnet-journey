namespace GarageApp
{
    public class VehicleTypeCount
    {
        public string TypeName { get; }
        public int Count { get; }

        public VehicleTypeCount(string typeName, int count)
        {
            TypeName = typeName;
            Count = count;
        }
    }
}