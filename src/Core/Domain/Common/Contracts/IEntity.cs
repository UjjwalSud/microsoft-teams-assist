namespace Microsoft.Teams.Assist.Domain.Common.Contracts;

public interface IEntity
{
    //List<DomainEvent> DomainEvents { get; }
}

public interface IEntity<TId> : IEntity
{
    TId Id { get; }
}
