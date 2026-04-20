using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Models;

namespace backend.Interfaces
{
    public interface IAlertRuleRepository
    {
        Task<List<AlertRule>>  GetAllRuleAsync();
		Task<List<AlertRule>> GetAllByWatchlistItemIdAsync(int id);
		Task<AlertRule> CreateAlertRuleAsync(AlertRule alertRuleModel);
		Task<AlertRule?> DeleteAsync(int id);

    }
}