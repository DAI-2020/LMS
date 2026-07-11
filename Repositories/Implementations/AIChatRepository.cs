using LMS.API.Data;
using LMS.API.Models;
using LMS.API.Repositories.Interfaces;

namespace LMS.API.Repositories.Implementations;

public class AIChatRepository : Repository<AIAssistantChat>, IAIChatRepository
{
    public AIChatRepository(LMSDbContext context)
        : base(context)
    {
    }
}