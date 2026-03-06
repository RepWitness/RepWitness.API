using RepWitness.Domain.Generic;

namespace RepWitness.Domain.Interfaces;

public interface IRepository<T>
{
    public ResponseType<T> GetOne(Guid id);
    public ResponseType<T> GetAll();
    public ResponseType<T> Add(T entity);
    public ResponseType<T> Update(T entity);
    public ResponseType<T> Delete(Guid id);
}