using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ReservationSystemMVC.Data;
using ReservationSystemMVC.Models;

namespace ReservationSystemMVC.Controllers
{
    public class RoomsController : Controller
    {
        private readonly ReservationSystemMVCContext _context;

        public RoomsController(ReservationSystemMVCContext context)
        {
            _context = context;
        }

        // GET: Rooms
        public async Task<IActionResult> Index()
        {
            var reservationSystemMVCContext = _context.Room.Include(r => r.RoomType);
            return View(await reservationSystemMVCContext.ToListAsync());
        }

        // GET: Rooms/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var room = await _context.Room
                .Include(r => r.RoomType)
                .FirstOrDefaultAsync(m => m.RoomId == id);
            if (room == null)
            {
                return NotFound();
            }

            return View(room);
        }

        // GET: Rooms/Create
        public async Task<IActionResult> Create()
        {
            await PopulateRoomTypeDropDownList();
            PopulateRoomEquipmentIconsDropDownList();
            await PopulateRoomEquipmentOfferAsync();
            await RoomRateRebateOffer();

            return View();
        }

        // POST: Rooms/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RoomId,RoomNumber,RoomTypeId,MaxOccupancy,ExtraBedAvailable")] Room room, int[] SelectedEquipments, string? SelectedDatesJson, int NightRate, int MinNights, int? ExtraBedRate, int[] SelectedRebates)
        {
            if (ModelState.IsValid)
            {
                // Add the room to the context, but don't save yet
                _context.Add(room);

                // If any equipment was selected, add it to the room
                if (SelectedEquipments != null && SelectedEquipments.Length > 0)
                {
                    foreach (int equipmentId in SelectedEquipments)
                    {
                        // Create a new RoomRoomEquipment object
                        var roomEquipment = new RoomRoomEquipment { RoomId = room.RoomId, RoomEquipmentId = equipmentId };
                        // Add it to the Room's RoomRoomEquipments navigation property
                        room.RoomRoomEquipments.Add(roomEquipment);
                    }
                }

                // Handle Room Rates and Rebates
                List<DateTime> selectedDates = string.IsNullOrEmpty(SelectedDatesJson) ? new List<DateTime>() : JsonSerializer.Deserialize<List<DateTime>>(SelectedDatesJson);
                if (selectedDates.Count == 0)
                {
                    // Apply rates and rebates for all dates
                    var roomRate = new RoomRate
                    {
                        NightRate = NightRate,
                        MinNights = MinNights,
                        ExtraBedRate = ExtraBedRate,
                        DateApplied = null,
                        Room = room
                    };

                    _context.Add(roomRate);

                    // Handle Room Rate Rebates
                    if (SelectedRebates != null && SelectedRebates.Length > 0)
                    {
                        foreach (var rebateId in SelectedRebates)
                        {
                            var roomRateRebate = new RoomRateRoomRateRebate { RoomRateId = roomRate.RoomRateId, RoomRateRebateId = rebateId };
                            roomRate.RoomRateRoomRateRebates.Add(roomRateRebate);
                        }
                    }
                }
                else
                {
                    foreach (DateTime date in selectedDates)
                    {
                        var roomRate = new RoomRate
                        {
                            NightRate = NightRate,
                            MinNights = MinNights,
                            ExtraBedRate = ExtraBedRate,
                            DateApplied = date,
                            Room = room
                        };

                        _context.RoomRate.Add(roomRate);

                        // Link rebates to each room rate if any rebates are selected
                        if (SelectedRebates != null && SelectedRebates.Length > 0)
                        {
                            foreach (var rebateId in SelectedRebates)
                            {
                                var roomRateRebate = new RoomRateRoomRateRebate { RoomRateId = roomRate.RoomRateId, RoomRateRebateId = rebateId };
                                roomRate.RoomRateRoomRateRebates.Add(roomRateRebate);
                            }
                        }
                    }
                }

                // Now that the RoomRoomEquipments have been added, save the changes
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            await PopulateRoomTypeDropDownList();
            PopulateRoomEquipmentIconsDropDownList();
            await PopulateRoomEquipmentOfferAsync();
            await RoomRateRebateOffer();
            return View(room);
        }

        // GET: Rooms/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Include RoomRoomEquipments to ensure the equipment associated with the room is available
            var room = await _context.Room
                                     .Include(r => r.RoomRoomEquipments)
                                     .Include(r => r.RoomRates)
                                     .ThenInclude(rr => rr.RoomRateRoomRateRebates)
                                     .FirstOrDefaultAsync(m => m.RoomId == id);
            if (room == null)
            {
                return NotFound();
            }

            var selectedDatesForRoom = room.RoomRates
                .Select(rr => rr.DateApplied?.ToString("yyyy-MM-dd"))
                .Where(d => d!= null)
                .ToList();
            ViewBag.SelectedDatesForRoom = selectedDatesForRoom;

            var roomRate = room.RoomRates.FirstOrDefault() ?? new RoomRate { RoomId = room.RoomId };
            ViewBag.RoomRate = roomRate;
            await PopulateRoomTypeDropDownList(room.RoomTypeId);
            PopulateRoomEquipmentIconsDropDownList();
            await PopulateRoomEquipmentOfferAsync();
            await RoomRateRebateOffer();
            return View(room);
        }


        // POST: Rooms/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RoomId,RoomNumber,RoomTypeId,MaxOccupancy,ExtraBedAvailable")] Room room)
        {
            if (id != room.RoomId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(room);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RoomExists(room.RoomId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            await PopulateRoomTypeDropDownList();
            PopulateRoomEquipmentIconsDropDownList();
            await PopulateRoomEquipmentOfferAsync();
            await RoomRateRebateOffer();
            return View(room);
        }

        // POST: Rooms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var room = await _context.Room.FindAsync(id);
            if (room != null)
            {
                _context.Room.Remove(room);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RoomExists(int id)
        {
            return _context.Room.Any(e => e.RoomId == id);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddRoomType(RoomType roomType)
        {
            if (ModelState.IsValid)
            {
                _context.Add(roomType);
                await _context.SaveChangesAsync();
                return Json(new { success = true, roomTypeId = roomType.RoomTypeId, roomType = roomType.Type });
            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return Json(new { success = false, errors = errors });
            }
        }

        public async Task<IActionResult> AddRoomEquipment(RoomEquipment roomEquipment)
        {
            if (ModelState.IsValid)
            {
                _context.Add(roomEquipment);
                await _context.SaveChangesAsync();

                return Json(new { success = true, roomEquipmentId = roomEquipment.RoomEquipmentId, icon = roomEquipment.Icon, name = roomEquipment.Name, isDefault = roomEquipment.IsDefault, description = roomEquipment.Description });
            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return Json(new { success = false, errors = errors });
            }
        }

        private List<RoomEquipment> GetEquipmentFromDatabase()
        {
            return _context.RoomEquipment.ToList();// Implement database access logic here
            // Return a list of Equipments objects
        }

        public async Task<IActionResult> AddRoomRateRebate(RoomRateRebate roomRateRebate)
        {
            if (ModelState.IsValid)
            {
                _context.Add(roomRateRebate);
                await _context.SaveChangesAsync();

                return Json(new { success = true, roomRateRebateId = roomRateRebate.RoomRateRebateId, rateRebate = roomRateRebate.RateRebate, minNightStay = roomRateRebate.MinNightStay });
            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return Json(new { success = false, errors = errors });
            }
        }

        private async Task PopulateRoomTypeDropDownList(object selectedRoomType = null)
        {
            var roomTypesQuery = from rt in _context.Set<RoomType>()
                                 orderby rt.Type
                                 select rt;
            ViewData["RoomTypeId"] = new SelectList(await roomTypesQuery.ToListAsync(), "RoomTypeId", "Type", selectedRoomType);
        }

        private void PopulateRoomEquipmentIconsDropDownList()
        {
            var roomEquipmentIcons = new List<SelectListItem>
            {
                new SelectListItem { Text = HttpUtility.HtmlDecode("&#xf540;"), Value = "fa-solid fa-square-parking" },
                new SelectListItem { Text = HttpUtility.HtmlDecode("&#xf1eb;"), Value = "fa-solid fa-wifi" },
                new SelectListItem { Text = HttpUtility.HtmlDecode("&#xe51a;"), Value = "fa-solid fa-kitchen-set" },
                new SelectListItem { Text = HttpUtility.HtmlDecode("&#xf236;"), Value = "fa-solid fa-bed" },
                new SelectListItem { Text = HttpUtility.HtmlDecode("&#xf2cd;"), Value = "fa-solid fa-bath" },
                new SelectListItem { Text = HttpUtility.HtmlDecode("&#xf4b8;"), Value = "fa-solid fa-couch" },
                new SelectListItem { Text = HttpUtility.HtmlDecode("&#xf562;"), Value = "fa-solid fa-bell-concierge" },
                new SelectListItem { Text = HttpUtility.HtmlDecode("&#xf0f3;"), Value = "fa-solid fa-bell" },
                new SelectListItem { Text = HttpUtility.HtmlDecode("&#xf193;"), Value = "fa-solid fa-wheelchair" },
                new SelectListItem { Text = HttpUtility.HtmlDecode("&#xf4d8;"), Value = "fa-solid fa-seedling" },
                new SelectListItem { Text = HttpUtility.HtmlDecode("&#xf5c5;"), Value = "fa-solid fa-water-ladder" },
                new SelectListItem { Text = HttpUtility.HtmlDecode("&#xf2e7;"), Value = "fa-solid fa-utensils" },
                new SelectListItem { Text = HttpUtility.HtmlDecode("&#xf004;"), Value = "fa-solid fa-heart" },
                new SelectListItem { Text = HttpUtility.HtmlDecode("&#xf52b;"), Value = "fa-solid fa-door-open" },
                new SelectListItem { Text = HttpUtility.HtmlDecode("&#xf2b9;"), Value = "fa-regular fa-address-book" },
                new SelectListItem { Text = HttpUtility.HtmlDecode("&#xf023;"), Value = "fa-solid fa-lock" },
                new SelectListItem { Text = HttpUtility.HtmlDecode("&#xf1ab;"), Value = "fa-solid fa-language" },
                new SelectListItem { Text = HttpUtility.HtmlDecode("&#xf1b9;"), Value = "fa-solid fa-car" },
                new SelectListItem { Text = HttpUtility.HtmlDecode("&#xf5e4;"), Value = "fa-solid fa-car-side" },
                new SelectListItem { Text = HttpUtility.HtmlDecode("&#xf5b6;"), Value = "fa-solid fa-van-shuttle" },
                new SelectListItem { Text = HttpUtility.HtmlDecode("&#xf624;"), Value = "fa-solid fa-gauge" }
            };
            ViewData["RoomEquipmentIcon"] = new SelectList(roomEquipmentIcons, "Value", "Text");
        }

        private async Task PopulateRoomEquipmentOfferAsync()
        {
            ViewData["RoomEquipmentOffer"] = await _context.RoomEquipment.ToListAsync();
        }

        private async Task RoomRateRebateOffer()
        {
            ViewData["RoomRateRebateOffer"] = await _context.RoomRateRebate.ToListAsync();
        }
    }
}