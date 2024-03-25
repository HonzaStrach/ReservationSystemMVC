using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace ReservationSystemMVC.Models
{
    public class RoomRate
    {
        public RoomRate()
        {
            RoomRateRoomRateRebates = new List<RoomRateRoomRateRebate>();
        }
        public int RoomRateId { get; set; }

        [Required]
        [DisplayName("Cena za noc")]
        [Range(1, int.MaxValue, ErrorMessage = "Cena za noc musí být větší než 0.")]
        public int NightRate { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Minimální počet nocí musí být alespoň 1.")]
        [DisplayName("Minimální počet nocí")]
        public int MinNights { get; set; } = 1; // Default to 1

        [DisplayName("Cena za přistýlku")]
        public int? ExtraBedRate { get; set; }

        public DateTime? DateApplied { get; set; }

        // Foreign key to Season
        public int SeasonId { get; set; }
        public Season Season { get; set; }

        // Foreign key to Room - delete when finished moving from RoomsController to SeasonsController
        public int? RoomId { get; set; }
        public Room? Room { get; set; }

        // Navigation property for the many-to-many relationship
        public List<RoomRateRoomRateRebate> RoomRateRoomRateRebates { get; set; }
    }

    public class RoomRateRebate
    {
        public int RoomRateRebateId { get; set; }

        [Range(1, 100, ErrorMessage = "Sleva na ubytování musí být v rozsahu od 1 do 100%.")]
        [DisplayName("Výše slevy v %")]
        public decimal RateRebate { get; set;}

        [Range(1, int.MaxValue, ErrorMessage = "Slevu je třeba uplatnit nejméně na jednu noc pobytu.")]
        [DisplayName("Minimální počet nocí pro získání slevy")]
        public int MinNightStay { get; set; }

        // Navigation property for the many-to-many relationship
        public List<RoomRateRoomRateRebate>? RoomRateRoomRateRebates { get; set; }
    }

    public class RoomRateRoomRateRebate
    {
        public int RoomRateId { get; set; }
        public RoomRate RoomRate { get; set; }

        public int RoomRateRebateId { get; set; }
        public RoomRateRebate RoomRateRebate { get; set; }
    }
}