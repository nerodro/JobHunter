using Microsoft.EntityFrameworkCore;
using VacancieDomain.Model;
using VacancieRepository;
using VacancieRepository.ResponseLogic;

namespace ResponseRepository.ResponseLogic
{
    public class ResponseLogic<T> : IResponseLogic<T> where T : ResponseModel
    {
        private readonly VacancyContext _ResponseContext;
        private DbSet<T> _dbSet;
        public ResponseLogic(VacancyContext context)
        {
            this._ResponseContext = context;
            _dbSet = context.Set<T>();
        }
        public async Task Create(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            await _dbSet.AddAsync(entity);
            await _ResponseContext.SaveChangesAsync();
        }

        public async Task Delete(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            _dbSet.Remove(entity);
            await _ResponseContext.SaveChangesAsync();
        }

        public async Task<T> Get(int id)
        {
            if (id == 0)
            {
                throw new ArgumentNullException("entity");
            }
            var Response = await _dbSet.SingleOrDefaultAsync(x => x.Id == id);
            if (Response == null)
            {
                throw new ArgumentException($"Вакансий с Id {id}, не найдено");
            }
            return Response;
        }

        public void Remove(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            _dbSet.Remove(entity);
        }

        public async Task SaveChanges()
        {
            await _ResponseContext.SaveChangesAsync();
        }

        public async Task Update(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            await _ResponseContext.SaveChangesAsync();
        }
        public IEnumerable<T> GetAll()
        {
            return _dbSet.AsEnumerable();
        }
    }
}
