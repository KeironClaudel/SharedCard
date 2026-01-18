using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SharedCard.Api.Data;
using SharedCard.Api.DTOs;
using SharedCard.Api.Models;

namespace SharedCard.Api.Controller
{
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

            return Ok(bill);
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

    }
}
