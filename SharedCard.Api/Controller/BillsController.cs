using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SharedCard.Api.Data;
using SharedCard.Api.DTOs;
using SharedCard.Api.Models;
using SharedCardAPI.DTOs;

namespace SharedCard.Api.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class BillsController : ControllerBase
    {
        private readonly SharedCardDBContext _context;

        public BillsController(SharedCardDBContext context)
        {
            _context = context;
        }

            [HttpPost]
            public async Task<IActionResult> Create([FromBody] CreateBillDto dto)
            {
                // 🔎 Validate person
                var personExists = await _context.People
                    .AnyAsync(p => p.PersonId == dto.PersonId && p.Active);

                if (!personExists)
                    return BadRequest("Invalid person.");

                // 🔎 Find billing cycle automatically
                var cycle = await _context.BillingCycles
                    .FirstOrDefaultAsync(c =>
                        dto.BillDate.Date >= c.StartDate &&
                        dto.BillDate.Date <= c.EndDate);

                if (cycle == null)
                    return BadRequest("No billing cycle found for this date.");

                var bill = new Bill
                {
                    BillDate = dto.BillDate,
                    Commerce = dto.Commerce,
                    Amount = dto.Amount,
                    Currency = dto.Currency,
                    PersonId = dto.PersonId,
                    CycleId = cycle.CycleId,
                    Notes = dto.Notes,
                    LoggedAt = DateTime.Now
                };

                _context.Bills.Add(bill);
                await _context.SaveChangesAsync();

                return Ok(new BillResponseDto
                {
                    BillId = bill.BillId,
                    BillDate = bill.BillDate,
                    Commerce = bill.Commerce,
                    Amount = bill.Amount,
                    Currency = bill.Currency,
                    PersonId = bill.PersonId,
                    CycleId = bill.CycleId
                });
        }


        [HttpGet("summary/{cycleId}")]
        public async Task<IActionResult> GetSummaryByCycle(int cycleId)
        {
            var summary = await _context.Bills
                .Where(b => b.CycleId == cycleId)
                .GroupBy(b => new { b.PersonId, b.Person.Name, b.Currency })
                .Select(g => new BillSummaryDto
                {
                    PersonId = g.Key.PersonId,
                    Name = g.Key.Name,
                    Currency = g.Key.Currency,
                    TotalAmount = g.Sum(x => x.Amount)
                })
                .ToListAsync();

            return Ok(summary);
        }

        [HttpGet("current-cycle")]
        public async Task<IActionResult> GetCurrentCycleBills()
        {
            var today = DateTime.Now.Date;

            var cycle = await _context.BillingCycles
                .FirstOrDefaultAsync(c =>
                    today >= c.StartDate &&
                    today <= c.EndDate);

            if (cycle == null)
            {
                return NotFound("No active billing cycle found.");
            }

            var bills = await _context.Bills
                .Where(b => b.CycleId == cycle.CycleId)
                .Include(b => b.Person)
                .OrderByDescending(b => b.BillDate)
                .Select(b => new BillListItemDto
                {
                    BillId = b.BillId,
                    BillDate = b.BillDate,
                    Commerce = b.Commerce,
                    Amount = b.Amount,
                    Currency = b.Currency,
                    PersonId = b.PersonId,
                    PersonName = b.Person.Name
                })
                .ToListAsync();

            return Ok(bills);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateBillDto dto)
        {
            var bill = await _context.Bills.FindAsync(id);

            if (bill == null)
                return NotFound();

            var cycle = await _context.BillingCycles
            .FirstOrDefaultAsync(c =>
                dto.BillDate.Date >= c.StartDate &&
                dto.BillDate.Date <= c.EndDate);

            if (cycle == null)
                return BadRequest("No billing cycle found for this date");

            bill.BillDate = dto.BillDate;
            bill.Commerce = dto.Commerce;
            bill.Amount = dto.Amount;
            bill.Currency = dto.Currency;
            bill.PersonId = dto.PersonId;
            bill.Notes = dto.Notes;
            bill.CycleId = cycle.CycleId;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var bill = await _context.Bills.FindAsync(id);
            if (bill == null)
                return NotFound();

            _context.Bills.Remove(bill);
            await _context.SaveChangesAsync();

            return NoContent();
        }

    }
}
