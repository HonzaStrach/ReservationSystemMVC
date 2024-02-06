using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace ReservationSystemMVC.Models
{
    public class Room
    {
        public int RoomId { get; set; }

        [Required(ErrorMessage = "Číslo pokoje je třeba zadat")]
        [DisplayName("Číslo pokoje")]
        public string RoomNumber { get; set; }

        [DisplayName("Typ pokoje")]
        public int? RoomTypeId { get; set; } // Foreign key to RoomType

        [Required(ErrorMessage = "Je třeba zadat maximální počet lůžek. Mimo přistýlky.")]
        [Range(1, 99, ErrorMessage = "Minimální počet lůžek je 1, maximální 99.")]
        [DisplayName("Počet lůžek")]
        public int MaxOccupancy { get; set; }

        [Required(ErrorMessage = "Je třeba zvolit, zda je k dispozici přistýlka. Zvýší počet lůžek o jedno.")]
        [DisplayName("Přistýlka")]
        public bool ExtraBedAvailable { get; set; }

        // Navigation property to RoomType
        public RoomType RoomType { get; set; }
    }
    public class RoomType
    {
        public int RoomTypeId { get; set; }

        [Required(ErrorMessage = "Je třeba zadat typ pokoje")]
        [DisplayName("Nový typ pokoje")]
        public string Type { get; set; }

        [DisplayName("Popis typu pokoje (pouze pro vaši referenci)")]
        public string? Description { get; set; }

        // Navigation property for the one-to-many relationship
        public List<Room> Rooms { get; set; }
    }

}
