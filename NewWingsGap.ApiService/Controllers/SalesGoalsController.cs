using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace NewWingsGap.ApiService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalesGoalsController : ControllerBase
    {
        private readonly NewWingsGapContext _context;

        public SalesGoalsController(NewWingsGapContext context)
        {
            _context = context;
        }

        // GET: api/SalesGoals
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SalesGoal>>> GetSalesGoals()
        {
            return await _context.SalesGoals!.Include(x => x.User)
                .Include(x => x.SalesGoalQuarters)!
                    .ThenInclude(sq => sq.SalesGoal)
                .ToListAsync();
        }

        // GET: api/SalesGoals/users/5
        [HttpGet("users/{userId}")]
        public async Task<ActionResult<IEnumerable<SalesGoal>>> GetSalesGoalsByUserId(int userId)
        {
            var salesGoals = await _context.SalesGoals!
                .Include(x => x.User)
                .Include(x => x.SalesGoalQuarters)
                .Where(b => b.User.Id == userId)
                .ToListAsync();

            if (salesGoals == null || !salesGoals.Any())
            {
                return NotFound();
            }

            return salesGoals;
        }
    }
}
