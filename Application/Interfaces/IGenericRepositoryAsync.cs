using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IGenericRepositoryAsync<T> where T : class
    {
        Task<IReadOnlyList<T>> GetAllAsync();

        Task<T> AddAsync(T entity);
    }
}
