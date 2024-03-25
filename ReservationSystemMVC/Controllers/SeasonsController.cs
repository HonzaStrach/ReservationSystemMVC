using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ReservationSystemMVC.Data;
using ReservationSystemMVC.Models;

namespace ReservationSystemMVC.Controllers
{
    public class SeasonsController : Controller
    {
        private readonly ReservationSystemMVCContext _context;

        public SeasonsController(ReservationSystemMVCContext context)
        {
            _context = context;
        }

        // GET: Seasons
        public async Task<IActionResult> Index()
        {
            var reservationSystemMVCContext = _context.Seasons.Include(s => s.Room);
            return View(await reservationSystemMVCContext.ToListAsync());
        }

        // GET: Seasons/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var season = await _context.Seasons
                .Include(s => s.Room)
                .FirstOrDefaultAsync(m => m.SeasonId == id);
            if (season == null)
            {
                return NotFound();
            }

            return View(season);
        }

        // GET: Seasons/Create
        public async Task<IActionResult> Create()
        {
            ViewData["RoomId"] = new SelectList(_context.Room, "RoomId", "RoomNumber");
            await RoomRateRebateOffer();
            return View();
        }

        // POST: Seasons/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SeasonId,Name,RoomId")] Season season, string? SelectedDatesJson, int NightRate, int MinNights, int? ExtraBedRate, int[] SelectedRebates)
        {
            // Manual validation
            if (NightRate <= 0)
            {
                ModelState.AddModelError("NightRate", "Cena za noc musí být větší než 0.");
            }
            if (MinNights <= 0)
            {
                ModelState.AddModelError("MinNights", "Minimální počet nocí musí být alespoň 1.");
            }
            if (ModelState.IsValid)
            {
                // Add the season to the context, but don't save yet
                _context.Add(season);

                // Handle Room rates and Rebates
                List<DateTime> selectedDates = string.IsNullOrEmpty(SelectedDatesJson) ? new List<DateTime>() : JsonSerializer.Deserialize<List<DateTime>>(SelectedDatesJson);
                if (selectedDates.Count == 0)
                {
                    // Apply Rates and Rebates for all dates
                    var roomRate = new RoomRate
                    {
                        NightRate = NightRate,
                        MinNights = MinNights,
                        ExtraBedRate = ExtraBedRate,
                        DateApplied = null,
                        Season = season
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
                            Season = season
                        };

                        _context.RoomRate.Add(roomRate);

                        // Link rebates to each Room rate if any rebates are selected
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
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["RoomId"] = new SelectList(_context.Room, "RoomId", "RoomNumber", season.RoomId);
            await RoomRateRebateOffer();
            return View(season);
        }

        // GET: Seasons/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var season = await _context.Seasons.FindAsync(id);
            if (season == null)
            {
                return NotFound();
            }

            var selectedDatesForSeason = season.RoomRates
                .Select(rr => rr.DateApplied?.ToString("yyyy-MM-dd"))
                .Where(d => d != null)
                .ToList();
            ViewBag.SelectedDatesForSeason = selectedDatesForSeason;

            var roomRate = season.RoomRates.FirstOrDefault() ?? new RoomRate { RoomId = season.RoomId };
            ViewBag.RoomRate = roomRate;
            ViewData["RoomId"] = new SelectList(_context.Room, "RoomId", "RoomNumber", season.RoomId);
            await RoomRateRebateOffer();
            return View(season);
        }

        // POST: Seasons/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SeasonId,Name,RoomId")] Season season)
        {
            if (id != season.SeasonId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(season);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SeasonExists(season.SeasonId))
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
            var selectedDatesForSeason = season.RoomRates
                .Select(rr => rr.DateApplied?.ToString("yyyy-MM-dd"))
                .Where(d => d != null)
                .ToList();
            ViewBag.SelectedDatesForSeason = selectedDatesForSeason;

            var roomRate = season.RoomRates.FirstOrDefault() ?? new RoomRate { RoomId = season.RoomId };
            ViewBag.RoomRate = roomRate;
            ViewData["RoomId"] = new SelectList(_context.Room, "RoomId", "RoomNumber", season.RoomId);
            await RoomRateRebateOffer();
            return View(season);
        }

        // GET: Seasons/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var season = await _context.Seasons
                .Include(s => s.Room)
                .FirstOrDefaultAsync(m => m.SeasonId == id);
            if (season == null)
            {
                return NotFound();
            }

            return View(season);
        }

        // POST: Seasons/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var season = await _context.Seasons.FindAsync(id);
            if (season != null)
            {
                _context.Seasons.Remove(season);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SeasonExists(int id)
        {
            return _context.Seasons.Any(e => e.SeasonId == id);
        }

        private async Task RoomRateRebateOffer()
        {
            ViewData["RoomRateRebateOffer"] = await _context.RoomRateRebate.ToListAsync();
        }
    }
}
