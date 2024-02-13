using System;
using System.Collections.Generic;
using System.Linq;
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
        public IActionResult Create()
        {
            ViewData["RoomTypeId"] = new SelectList(_context.Set<RoomType>(), "RoomTypeId", "Type");
            var roomEquipmentIcons = new List<SelectListItem>
            {
                new SelectListItem { Text = HttpUtility.HtmlDecode("&#xf540;"), Value = "fa-solid fa-square-parking" },
                new SelectListItem { Text = HttpUtility.HtmlDecode("&#xf1eb;"), Value = "fa-solid fa-wifi" },
                new SelectListItem { Text = HttpUtility.HtmlDecode("&#xe51a;"), Value = "fa-solid fa-kitchen-set" },
                new SelectListItem { Text = HttpUtility.HtmlDecode("&#xf236;"), Value = "fa-solid fa-bed" },
                new SelectListItem { Text = HttpUtility.HtmlDecode("&#xf2cd;"), Value = "fa-solid fa-bath" },
                new SelectListItem { Text = HttpUtility.HtmlDecode("&#xf4b8;"), Value = "fa-solid fa-couch" },
                new SelectListItem { Text = HttpUtility.HtmlDecode("&#xf562;"), Value = "fa-solid fa-bell-concierge" },
                new SelectListItem { Text = HttpUtility.HtmlDecode("&#xf193;"), Value = "fa-solid fa-wheelchair" },
                new SelectListItem { Text = HttpUtility.HtmlDecode("&#xf4d8;"), Value = "fa-solid fa-seedling" },
                new SelectListItem { Text = HttpUtility.HtmlDecode("&#xf5c5;"), Value = "fa-solid fa-water-ladder" },
                new SelectListItem { Text = HttpUtility.HtmlDecode("&#xf2e7;"), Value = "fa-solid fa-utensils" },
                new SelectListItem { Text = HttpUtility.HtmlDecode("&#xf004;"), Value = "fa-solid fa-heart" },
                new SelectListItem { Text = HttpUtility.HtmlDecode("&#xf52b;"), Value = "fa-solid fa-door-open" },
                new SelectListItem { Text = HttpUtility.HtmlDecode("&#xf2b9;"), Value = "fa-regular fa-address-book" },
                new SelectListItem { Text = HttpUtility.HtmlDecode("&#xf023;"), Value = "fa-solid fa-lock" },
                new SelectListItem { Text = HttpUtility.HtmlDecode("&#xf1ab;"), Value = "fa-solid fa-language" }
            };
            ViewData["RoomEquipmentIcon"] = new SelectList(roomEquipmentIcons, "Value", "Text");

            var equipmentColumns = new List<RoomEquipment>[3] { new List<RoomEquipment>(), new List<RoomEquipment>(), new List<RoomEquipment>() }; 
            var allEquipments = GetEquipmentFromDatabase();
            int column = 0;
            foreach (var equipment in allEquipments)
            {
                equipmentColumns[column].Add(equipment);
                column = (column + 1) % 3; // This will cycle through 0, 1, 2
            }
            ViewData["RoomEquipmentColumns"] = equipmentColumns;
            return View();
        }

        // POST: Rooms/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RoomId,RoomNumber,RoomTypeId,MaxOccupancy,ExtraBedAvailable")] Room room)
        {
            if (ModelState.IsValid)
            {
                _context.Add(room);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["RoomTypeId"] = new SelectList(_context.Set<RoomType>(), "RoomTypeId", "Type", room.RoomTypeId);
            return View(room);
        }

        // GET: Rooms/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var room = await _context.Room.FindAsync(id);
            if (room == null)
            {
                return NotFound();
            }
            ViewData["RoomTypeId"] = new SelectList(_context.Set<RoomType>(), "RoomTypeId", "Type", room.RoomTypeId);
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
            ViewData["RoomTypeId"] = new SelectList(_context.Set<RoomType>(), "RoomTypeId", "Type", room.RoomTypeId);
            return View(room);
        }

        // GET: Rooms/Delete/5
        public async Task<IActionResult> Delete(int? id)
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

            return Json(new { success = false, message = "Zkontrolujte, že jste zadali typ pokoje." });
        }

        public async Task<IActionResult> AddRoomEquipment(RoomEquipment roomEquipment)
        {
            if (ModelState.IsValid)
            {
                _context.Add(roomEquipment);
                await _context.SaveChangesAsync();

                return Json(new { success = true, roomEquipmentId = roomEquipment.RoomEquipmentId, icon = roomEquipment.Icon, name = roomEquipment.Name, isDefault = roomEquipment.IsDefault, description = roomEquipment.Description });
            }

            return Json(new { success = false, message = "Zkontrolujte, že jste zadali všechny údaje." });
        }

        private List<RoomEquipment> GetEquipmentFromDatabase()
        {
            return _context.RoomEquipment.ToList();// Implement database access logic here
            // Return a list of Equipments objects
        }
    }
}
