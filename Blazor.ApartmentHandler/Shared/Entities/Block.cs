using System.ComponentModel.DataAnnotations;

namespace Blazor.ApartmentHandler.Shared.Entities
{
    public class Block
    {
        public int Id { get; set; }
        //public List<Apartment> ApartmentList { get; set; }
        public virtual List<Apartment> ApartmentList { get; set; } = new List<Apartment>();   
    }

    public class BlockDTONoApartments
    {
        [Required]
        public int Id { set; get; }
    }

    public class BlockDTO : BlockDTONoApartments
    {
        public List<int> ApartmentIds { get; set; } = new List<int>();
    }
}
