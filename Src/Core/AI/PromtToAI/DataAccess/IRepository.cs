using Base.DataBaseAndIdentity.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PromptToAI.Models;
using System.Reflection;

namespace PromptToAI.DataAccess;

public interface IRepository
{
    Task<Guid> createAnHistory(string action, string? plantUml, Guid userId, CancellationToken c);
    Task<bool> saveMessageToHistory(Guid historyId, string content, string type, CancellationToken c);
    Task<bool> updateHistory(string? plantUml, Guid historyId, CancellationToken c);
}
