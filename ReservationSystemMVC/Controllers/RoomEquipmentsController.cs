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
    public class RoomEquipmentsController : Controller
    {
        private readonly ReservationSystemMVCContext _context;

        public RoomEquipmentsController(ReservationSystemMVCContext context)
        {
            _context = context;
        }

        // GET: RoomEquipments
        public async Task<IActionResult> Index()
        {
            return View(await _context.RoomEquipment.ToListAsync());
        }

        // GET: RoomEquipments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var roomEquipment = await _context.RoomEquipment
                .FirstOrDefaultAsync(m => m.RoomEquipmentId == id);
            if (roomEquipment == null)
            {
                return NotFound();
            }

            return View(roomEquipment);
        }

        // GET: RoomEquipments/Create
        public IActionResult Create()
        {
            PopulateRoomEquipmentIconsDropDownList();
            return View();
        }

        // POST: RoomEquipments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RoomEquipmentId,Icon,Name,IsDefault,Description")] RoomEquipment roomEquipment)
        {
            if (ModelState.IsValid)
            {
                _context.Add(roomEquipment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(roomEquipment);
        }

        // GET: RoomEquipments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var roomEquipment = await _context.RoomEquipment.FindAsync(id);
            if (roomEquipment == null)
            {
                return NotFound();
            }
            PopulateRoomEquipmentIconsDropDownList();
            return View(roomEquipment);
        }

        // POST: RoomEquipments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RoomEquipmentId,Icon,Name,IsDefault,Description")] RoomEquipment roomEquipment)
        {
            if (id != roomEquipment.RoomEquipmentId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(roomEquipment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RoomEquipmentExists(roomEquipment.RoomEquipmentId))
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
            PopulateRoomEquipmentIconsDropDownList();
            return View(roomEquipment);
        }

        // GET: RoomEquipments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var roomEquipment = await _context.RoomEquipment
                .FirstOrDefaultAsync(m => m.RoomEquipmentId == id);
            if (roomEquipment == null)
            {
                return NotFound();
            }

            return View(roomEquipment);
        }

        // POST: RoomEquipments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var roomEquipment = await _context.RoomEquipment.FindAsync(id);
            if (roomEquipment != null)
            {
                // Find all RoomRoomEquipment entries that reference the RoomEquipment being deleted
                var roomRoomEquipments = await _context.RoomRoomEquipments
                    .Where(rre => rre.RoomEquipmentId == id)
                    .ToListAsync();

                // Remove all found RoomRoomEquipment entries
                foreach (var rre in roomRoomEquipments)
                {
                    _context.RoomRoomEquipments.Remove(rre);
                }

                // Remove the RoomEquipment entity
                _context.RoomEquipment.Remove(roomEquipment);

                // Save changes to the database
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }


        private bool RoomEquipmentExists(int id)
        {
            return _context.RoomEquipment.Any(e => e.RoomEquipmentId == id);
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
    }
}
