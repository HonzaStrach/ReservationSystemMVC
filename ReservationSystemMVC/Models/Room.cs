﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace ReservationSystemMVC.Models
{
    public class Room
    {   
        public Room()
        {
            RoomRoomEquipments = new List<RoomRoomEquipment>();
        }
        public int RoomId { get; set; }

        [Required(ErrorMessage = "Číslo pokoje je třeba zadat")]
        [DisplayName("Číslo pokoje")]
        public string RoomNumber { get; set; }

        [DisplayName("Typ pokoje")]
        public int? RoomTypeId { get; set; }

        [Required(ErrorMessage = "Je třeba zadat maximální počet lůžek. Mimo přistýlky.")]
        [Range(1, 99, ErrorMessage = "Minimální počet lůžek je 1, maximální 99.")]
        [DisplayName("Počet lůžek")]
        public int MaxOccupancy { get; set; }

        [Required(ErrorMessage = "Je třeba zvolit, zda je k dispozici přistýlka. Zvýší počet lůžek o jedno.")]
        [DisplayName("Přistýlka")]
        public bool ExtraBedAvailable { get; set; }

        // Navigation property to RoomType
        public RoomType? RoomType { get; set; }

        // Navigation property for many-to-many relationship with RoomEquipment
        public List<RoomRoomEquipment>? RoomRoomEquipments { get; set; }

        // Navigation property for RoomRate
        public ICollection<RoomRate> RoomRates { get; set; } = new List<RoomRate>();
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
        public List<Room>? Rooms { get; set; }
    }

    public class RoomEquipment
    {
        public int RoomEquipmentId { get; set; }

        [Required(ErrorMessage = "Je třeba zvolit ikonu, která se u vybavení bude zobrazovat.")]
        [DisplayName("Ikona")]
        public string Icon { get; set; }

        [Required(ErrorMessage = "Musíte zvolit pojmenování vybavení pokoje. Zadejte krátký výstižný popis.")]
        [DisplayName("Pojmenování vybavení")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Rozhodněte se, zda má toto vybavení být přiřazeno ke všem pokojům automaticky.")]
        [DisplayName("Pro všechny pokoje")]
        public bool IsDefault { get; set; }

        [DisplayName("Popisek")]
        public string? Description { get; set; }

        // Navigation property for many-to-many relationship with Room
        public List<RoomRoomEquipment>? RoomRoomEquipments { get; set; }
    }

    public class RoomRoomEquipment
    {
        public int RoomId { get; set; }
        public Room Room { get; set; }

        public int RoomEquipmentId { get; set; }
        public RoomEquipment RoomEquipment { get; set; }
    }
}