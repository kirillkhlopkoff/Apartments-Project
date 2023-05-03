using System.ComponentModel.DataAnnotations;

namespace ClientMVCApartments.Models
{
    public class Apartment
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Address { get; set; }
        public decimal Price { get; set; }
        public int Rooms { get; set; }
        public string Description { get; set; }
    }
}
