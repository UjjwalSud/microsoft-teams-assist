﻿namespace Microsoft.Teams.Assist.Domain.Common.Contracts;

public abstract class BaseEntity : BaseEntity<DefaultIdType>
{
    // No need to set the Id explicitly; it will be generated by the database.
}

public abstract class BaseEntity<TId> : IEntity<TId>
{
    public TId Id { get; protected set; } = default!;

    //[NotMapped]
    //public List<DomainEvent> DomainEvents { get; } = new();
}
