using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Data;
using backend.Interfaces;
using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Repository
{
	public class AlertRuleRepository : IAlertRuleRepository
	{
		private readonly ApplicationDbContext _context;
		public AlertRuleRepository(ApplicationDbContext context)
		{
			_context = context;
		}
		public async Task<AlertRule> CreateAlertRuleAsync(AlertRule alertRuleModel)
		{
			await _context.AlertRule.AddAsync(alertRuleModel);
			await _context.SaveChangesAsync();
			return alertRuleModel;
		}

		public async Task<AlertRule?> DeleteAsync(int Id)
		{
			return await _context.AlertRule.FindAsync(Id);
		}

		public Task<AlertRule> EvaluateAlertAsync()
		{
			throw new NotImplementedException();
		}

		public async Task<List<AlertRule>> GetAllRuleAsync()
		{
			return await _context.AlertRule.ToListAsync();
		}

		public async Task<AlertRule?> GetByIdAsync(int id)
		{
			return await _context.AlertRule.FirstOrDefaultAsync(x => x.Id == id);
		}
	}
}