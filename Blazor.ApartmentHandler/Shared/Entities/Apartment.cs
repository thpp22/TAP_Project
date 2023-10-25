using System.ComponentModel.DataAnnotations;

namespace Blazor.ApartmentHandler.Shared.Entities
{
    public  class Apartment
    {
        public int Id { get; set; }
        public int Nr { get; set; }
        public int BlockId { get;set; }
        public virtual Block Block { get; set; }
        public int NumberOfPersons { get; set; }
        public virtual List<Bill> Bills { get; set; }
        //public virtual List<Bill> Bills { get; set; }=new List<Bill>();
    }

    public class ApartmentDTOForUpdate  // DTOForUpdate e folosit pentru operatia de PUT (Update)
    {
        [Required]
        [Range(1, 30, ErrorMessage = "A block can have a maximum number of 30 apartments.")]
        public int Nr { get; set; }

        [Required]
        [Range(1, 10, ErrorMessage = "An apartment can have between 1 and 10 persons.")]
        public int NumberOfPersons { get; set; }
    }

    public class ApartmentDTO : ApartmentDTOForUpdate // DTO e folosit pentru operatia de GET sau POST
    {  
        public int Id { get; set; }
        [Required]
        public int BlockId { get; set; }
        public List<int> BillsIds { get; set; } = new List<int>();

    }
}
