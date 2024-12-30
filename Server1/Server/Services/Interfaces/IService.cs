//using System.Collections.Generic;
//using System.Threading.Tasks;

//namespace Server.Services.Interfaces
//{
//    public interface IBaseService<TEntity, TDTO, TCreateDTO>
//        where TEntity : class
//        where TDTO : class
//        where TCreateDTO : class
//    {
//        Task<IEnumerable<TDTO>> GetAllAsync();
//        Task<TDTO?> GetByIdAsync(int id);
//        Task<TDTO> AddAsync(TCreateDTO createDto);
//        Task<TDTO> UpdateAsync(int id, TCreateDTO updateDto);
//        Task<bool> DeleteAsync(int id);
//    }
//}
