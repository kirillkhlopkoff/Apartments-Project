using System.ComponentModel.DataAnnotations;

namespace ClientMVCApartments.ViewModels
{
    public class ApartmentViewModel
    {
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Title { get; set; }
        [Required]
        [StringLength(50)]
        public string Address { get; set; }

        [Required]
        public int Rooms { get; set; }

        [Required]
        public double Price { get; set; }

        [Required]
        public string Description { get; set; }
    }
}
