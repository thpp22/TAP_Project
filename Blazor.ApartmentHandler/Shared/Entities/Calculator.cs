namespace Blazor.ApartmentHandler.Shared.Entities
{
    //public class Calculator
    //{
    //    public virtual double Calculate(Bill bill) { return 0; }
    //}

    //public class NumberOfPersonsOver5: Calculator
    //{
    //    public override double Calculate(Bill bill)
    //    {
    //      return bill.WaterConsumption * 4 + bill.Apartment.NumberOfPersons * 1.25; 
    //    }
    //}
    ////return bill.WaterConsumption * 5 + bill.Apartment.NumberOfPersons * 1.5;

    //public class NumberOfPersonsUnder5 : Calculator
    //{
    //    public override double Calculate(Bill bill)
    //    {
    //        return bill.WaterConsumption * 5 + bill.Apartment.NumberOfPersons * 1.5;
    //    }
    //}

    public class Calculator
    {
        public double Calculate(Bill bill)
        {
            if (bill.WaterConsumption == 0)
                return 0;
            if (bill.Apartment.NumberOfPersons > 5)
                return bill.WaterConsumption * 4 + bill.Apartment.NumberOfPersons * 1.25;
            if (bill.Apartment.NumberOfPersons <= 5)
                return bill.WaterConsumption * 5 + bill.Apartment.NumberOfPersons * 1.5;
            return 0;
        }

        public double Calculate(int numberOfPersons, double WaterConsumption)
        {
            if (WaterConsumption == 0)
                return 0;
            if (numberOfPersons > 5)
                return WaterConsumption * 4 + numberOfPersons * 1.25;
            if (numberOfPersons <= 5)
                return WaterConsumption * 5 + numberOfPersons * 1.5;
            return 0;
        }
    }
}