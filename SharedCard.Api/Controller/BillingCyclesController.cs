using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SharedCard.Api.Data;

namespace SharedCard.Api.Controller
{
    public class BillingCyclesController : ControllerBase
    {
        private readonly SharedCardDBContext _context;

        public BillingCyclesController(SharedCardDBContext context)
        {
            _context = context;
        }

        [HttpGet("current")]
        public async Task<IActionResult> GetCurrentCycle()
        {
            var today = DateTime.Today;

            var cycle = await _context.BillingCycles
                .FirstOrDefaultAsync(c =>
                    today >= c.StartDate && today <= c.EndDate);

            if (cycle == null)
                return NotFound("No active billing cycle found.");

            return Ok(cycle);
        }
    }
}
