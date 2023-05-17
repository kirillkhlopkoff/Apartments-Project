namespace ApiApartment.Models
{
    public class Apartment
    {
        /*public string UserId { get; set; }*/
        public string Owner { get; set; }
        public int Id { get; set; }
        public string Title { get; set; }
        public string Address { get; set; }
        public decimal Price { get; set; }
        public int Rooms { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
    }
}
