using System;
using System.Linq;

namespace TodoList.Domain.SharedKernel
{
    public interface IRepository<T> : IDisposable where T : class
    {
        void Add(T obj);
        T GetById(Guid id);
        IQueryable<T> GetAll();
        void Update(T obj);
        void Remove(Guid id);
        int SaveChanges();
    }
}
