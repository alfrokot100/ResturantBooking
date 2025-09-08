using System.ComponentModel.DataAnnotations;

namespace ResturantBooking.Models
{
    public class Customer
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Namnet kan vara max 100 tecken")]
        public string Name { get; set; }

        [Required]
        [Phone(ErrorMessage = "Telefonnumret är ogiltigt")]
        [StringLength(20)]
        public string PhoneNbr { get; set; }
        public List<Booking> Bokkings  { get; set; }
    }
}
