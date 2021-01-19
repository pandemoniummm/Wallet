using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wallet.Model;

namespace Wallet.Data
{
    public interface IRepositoryBase<T> where T : class
    {
        Task<IList<T>> GetList();
        Task<T> GetSingle(int id);
        Task Create(T item);
        Task Update(T item);
        Task Delete(int id);      
    }
}
