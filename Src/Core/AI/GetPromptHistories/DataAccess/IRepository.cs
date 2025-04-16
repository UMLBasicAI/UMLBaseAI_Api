using Base.DataBaseAndIdentity.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GetPromptHistories.Models;
using System.Reflection;

namespace GetPromptHistories.DataAccess;

public interface IRepository
{
    Task<List<HistoryEntity>> GetHistoriesByUserId(string userId, int page, int size, CancellationToken cancellationToken);
    Task<int> CountHistoriesByUserId(string userId, CancellationToken cancellationToken);
}
