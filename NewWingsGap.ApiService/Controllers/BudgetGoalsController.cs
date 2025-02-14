using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace NewWingsGap.ApiService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BudgetGoalsController : ControllerBase
    {
        private readonly NewWingsGapContext _context;

        public BudgetGoalsController(NewWingsGapContext context)
        {
            _context = context;
        }

        // GET: api/BudgetGoals
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BudgetGoal>>> GetBudgetGoals()
        {
            return await _context.BudgetGoals!.ToListAsync();
        }

        // GET: api/BudgetGoals/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BudgetGoal>> GetBudgetGoal(int id)
        {
            var budgetGoal = await _context.BudgetGoals!.FindAsync(id);

            if (budgetGoal == null)
            {
                return NotFound();
            }

            return budgetGoal;
        }

        // PUT: api/BudgetGoals/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBudgetGoal(int id, BudgetGoal budgetGoal)
        {
            if (id != budgetGoal.Id)
            {
                return BadRequest();
            }

            _context.Entry(budgetGoal).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BudgetGoalExists(id))
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

        // POST: api/BudgetGoals
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<BudgetGoal>> PostBudgetGoal(BudgetGoal budgetGoal)
        {
            _context.BudgetGoals!.Add(budgetGoal);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBudgetGoal", new { id = budgetGoal.Id }, budgetGoal);
        }

        // DELETE: api/BudgetGoals/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBudgetGoal(int id)
        {
            var budgetGoal = await _context.BudgetGoals!.FindAsync(id);
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
    }
}
