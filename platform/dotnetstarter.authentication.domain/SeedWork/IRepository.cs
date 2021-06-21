using System;
using System.Collections.Generic;
using System.Text;

namespace dotnetstarter.authentication.domain.SeedWork
{
    public interface IRepository<T> where T : IAggregateRoot
    {
        IUnitOfWork UnitOfWork { get; }
    }
}
