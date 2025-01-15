namespace Domain.Contracts;

public interface IUnitOfWork
{
    Task CompleteASync();
}