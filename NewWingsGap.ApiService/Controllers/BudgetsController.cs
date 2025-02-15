using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace NewWingsGap.ApiService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BudgetsController : ControllerBase
    {
        private readonly NewWingsGapContext _context;

        public BudgetsController(NewWingsGapContext context)
        {
            _context = context;
        }

        // GET: api/Budgets
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Budget>>> GetBudget()
        {
            return await _context.Budgets!.Include(x => x.User)
                .Include(x => x.BudgetItems)
                .Include(x => x.BudgetGoals)!.ToListAsync();
        }

        // GET: api/Budgets/users/5
        [HttpGet("users/{userId}")]
        public async Task<ActionResult<IEnumerable<Budget>>> GetBudgetsByUserId(int userId)
        {
            var budgets = await _context.Budgets!
                .Include(x => x.User)
                .Include(x => x.BudgetItems)
                .Include(x => x.BudgetGoals)!
                .Where(b => b.User.Id == userId)
                .ToListAsync();

            if (budgets == null || !budgets.Any())
            {
                return NotFound();
            }

            return budgets;
        }

        #region Budget Goals
        // PUT: api/BudgetGoals/budget/{budgetId}
        [HttpPut("budget/{budgetId}")]
        public async Task<IActionResult> PutBudgetGoalByBudgetId(int budgetId, BudgetGoal budgetGoal)
        {
            var budget = await _context.Budgets!
                .Include(b => b.BudgetGoals)
                .FirstOrDefaultAsync(b => b.Id == budgetId);

            if (budget == null)
            {
                return NotFound();
            }

            var existingBudgetGoal = budget.BudgetGoals!.FirstOrDefault(bg => bg.Id == budgetGoal.Id);
            if (existingBudgetGoal == null)
            {
                return NotFound();
            }

            if (budgetGoal.Id != existingBudgetGoal.Id)
            {
                return BadRequest();
            }

            existingBudgetGoal.Description = budgetGoal.Description;
            existingBudgetGoal.Amount = budgetGoal.Amount;
            existingBudgetGoal.LastModified = DateTime.UtcNow;

            _context.Entry(existingBudgetGoal).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BudgetGoalExists(budgetGoal.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/BudgetGoals/budget/{budgetId}
        [HttpPost("budget/{budgetId}")]
        public async Task<ActionResult<BudgetGoal>> PostBudgetGoalByBudgetId(int budgetId, BudgetGoal budgetGoal)
        {
            var budget = await _context.Budgets!
                .Include(b => b.BudgetGoals)
                .FirstOrDefaultAsync(b => b.Id == budgetId);

            if (budget == null)
            {
                return NotFound();
            }

            budgetGoal.Budget = budget;
            budgetGoal.LastModified = DateTime.UtcNow;

            _context.BudgetGoals!.Add(budgetGoal);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBudgetGoal", new { id = budgetGoal.Id }, budgetGoal);
        }

        // DELETE: api/BudgetGoals/budget/{budgetId}/goal/{goalId}
        [HttpDelete("budget/{budgetId}/budgetGoal/{budgetGoalId}")]
        public async Task<IActionResult> DeleteBudgetGoalByBudgetId(int budgetId, int budgetGoalId)
        {
            var budget = await _context.Budgets!
                .Include(b => b.BudgetGoals)
                .FirstOrDefaultAsync(b => b.Id == budgetId);

            if (budget == null)
            {
                return NotFound();
            }

            var budgetGoal = budget.BudgetGoals!.FirstOrDefault(bg => bg.Id == budgetGoalId);
            if (budgetGoal == null)
            {
                return NotFound();
            }

            _context.BudgetGoals.Remove(budgetGoal);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BudgetGoalExists(int id)
        {
            return _context.BudgetGoals!.Any(e => e.Id == id);
        }

        #endregion

        #region BudgetItems

        // PUT: api/BudgetItems/budget/{budgetId}
        [HttpPut("budget/{budgetId}/budgetItem/{budgetItemId}")]
        public async Task<IActionResult> PutBudgetItemByBudgetId(int budgetId, int budgetItemId, BudgetItem budgetItem)
        {
            var budget = await _context.Budgets!
                .Include(b => b.BudgetItems)
                .FirstOrDefaultAsync(b => b.Id == budgetId);

            if (budget == null)
            {
                return NotFound();
            }

            var existingBudgetItem = budget.BudgetItems!.FirstOrDefault(bi => bi.Id == budgetItemId);
            if (existingBudgetItem == null)
            {
                return NotFound();
            }

            if (budgetItem.Id != existingBudgetItem.Id)
            {
                return BadRequest();
            }

            existingBudgetItem.Description = budgetItem.Description;
            existingBudgetItem.Amount = budgetItem.Amount;
            existingBudgetItem.LastModified = DateTime.UtcNow;

            _context.Entry(existingBudgetItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BudgetItemExists(budgetItem.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/BudgetItems/budget/{budgetId}
        [HttpPost("budget/{budgetId}/budgetItem")]
        public async Task<ActionResult<BudgetItem>> PostBudgetItemByBudgetId(int budgetId, BudgetItem budgetItem)
        {
            var budget = await _context.Budgets!
                .Include(b => b.BudgetItems)
                .FirstOrDefaultAsync(b => b.Id == budgetId);

            if (budget == null)
            {
                return NotFound();
            }

            budgetItem.Budget = budget;
            budgetItem.LastModified = DateTime.UtcNow;

            _context.BudgetItems!.Add(budgetItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBudgetItem", new { id = budgetItem.Id }, budgetItem);
        }

        // DELETE: api/BudgetItems/budget/{budgetId}/budgetItem/{budgetItemId}
        [HttpDelete("budget/{budgetId}/budgetItem/{budgetItemId}")]
        public async Task<IActionResult> DeleteBudgetItemByBudgetId(int budgetId, int budgetItemId)
        {
            var budget = await _context.Budgets!
                .Include(b => b.BudgetItems)
                .FirstOrDefaultAsync(b => b.Id == budgetId);

            if (budget == null)
            {
                return NotFound();
            }

            var budgetItem = budget.BudgetItems!.FirstOrDefault(bi => bi.Id == budgetItemId);
            if (budgetItem == null)
            {
                return NotFound();
            }

            _context.BudgetItems.Remove(budgetItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BudgetItemExists(int id)
        {
            return _context.BudgetItems!.Any(e => e.Id == id);
        }

        #endregion
    }
}
