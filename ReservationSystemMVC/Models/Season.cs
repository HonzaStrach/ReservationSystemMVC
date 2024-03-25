using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReservationSystemMVC.Models
{
    public class Season
    {
        [Key]
        public int SeasonId { get; set; }

        [Required(ErrorMessage = "Je třeba zadat pojmenování sezóny")]
        [DisplayName("Popisek sezóny")]
        public required string Name { get; set; }

        // Navigation property for SeasonDate
        public ICollection<SeasonDate> SeasonDates { get; set; } = new List<SeasonDate>();

        // Navigation property for RoomRate
        public ICollection<RoomRate> RoomRates { get; set; } = new List<RoomRate>();

        // Foreign key for Room
        [DisplayName("Vyberte pokoj pro danou sezónu")]
        public int? RoomId { get; set; }
        [DisplayName("Přiřazený pokoj")]
        public virtual Room? Room { get; set; }
    }

    public class SeasonDate
    {
        [Key]
        public int SeasonDateId { get; set; }

        [Required]
        [Column(TypeName = "date")]
        [DisplayName("Datum(y), pro které je sezóna uplatňována")]
        public DateTime DateApplied { get; set; }

        // Foreign key for Season
        public int SeasonId { get; set; }
        public required virtual Season Season { get; set; }

    }
}