using Domain.Contracts;
using LMS.Infrastructure.Data;

namespace LMS.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly LmsContext context;

    public UnitOfWork(LmsContext context)
    {
        this.context = context;
    }

    public async Task CompleteASync()
    {
        await context.SaveChangesAsync();
    }
}
