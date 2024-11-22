namespace HomeListingAPI
{
    //Enum for heating types
    public enum HeatingTypes
    {
        CentralHeating,
        Furnace,
        Boiler,
        HeatPump,
        ForcedAirHeat,
        RadiantFloorHeating,
        ElectricHeating,
        WoodStove,
        GasFireplace,
        OilHeat,
        PropaneHeat,
        HotWaterRadiators,
    }
    //Enum for cooling types
    public enum CoolingTypes
    {
        CentralAir,
        WindowAirConditioner,
        EvaporativeCooler,
        CeilingFans,
        ExhaustFans,
        ChilledBeams,
        IceStorageCooling,
        RadiantCooling
    }

    //Holds all data for heating and cooling
    public class TemperatureControl : ICloneable<TemperatureControl>
    {
        //Fields 
        private int? temperatureControlID;
        private HeatingTypes heating;
        private CoolingTypes cooling;

        //Constructor without id
        public TemperatureControl(HeatingTypes heating, CoolingTypes cooling)
        {
            temperatureControlID = null;
            this.heating = heating;
            this.cooling = cooling;
        }
        //Constructor with id
        public TemperatureControl(int? temperatureControlID, HeatingTypes heating, CoolingTypes cooling)
        {
            this.temperatureControlID = temperatureControlID;
            this.heating = heating;
            this.cooling = cooling;
        }

        //Get Set
        public int? TemperatureControlID
        {
            get { return temperatureControlID; }
            set { temperatureControlID = value; }
        }
        public HeatingTypes Heating
        {
            get { return heating; }
            set { heating = value; }
        }
        public CoolingTypes Cooling
        {
            get { return cooling; }
            set { cooling = value; }
        }

        //return DeepCopy
        public TemperatureControl Clone()
        {
            return new TemperatureControl(TemperatureControlID, Heating, Cooling);
        }
    }
}
