using System.ComponentModel.DataAnnotations;

namespace Blazor.ApartmentHandler.Shared.Entities
{
    public class Bill
    {
        /*public Bill(int nPersoane, double waterConsumption)
        {
            BillId = 0;
            Month = 1;
            Year = 2000;
            Apartment = new Apartment(nPersoane);
            ApartmentId = 0;
            WaterConsumption = waterConsumption;
        }*/
       
        public int BillId { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public int ApartmentId { get; set; }
        public virtual Apartment Apartment { get; set; }
        public double WaterConsumption { get; set; }
        public double BillValue { get; set; }
    }

    public class BillDTOForUpdate
	{
        [Required]
        public double WaterConsumption { get; set; }
        [Required]
        public double BillValue { get; set; }
    }

    public class BillDTO : BillDTOForUpdate
    {
        [Required]
        public int BillId { get; set; }

        [Required]
        [Range(1, 12, ErrorMessage = "Month should be a number between 1 and 12.")]
        public int Month { get; set; }

        [Required]
        [Range(2000, 2030, ErrorMessage = "Year should be a number between 2000 and 2030.")]
        public int Year { get; set; }

        [Required]
        public int ApartmentId { get; set; }
       
    }
}
