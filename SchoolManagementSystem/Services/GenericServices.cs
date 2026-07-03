using SchoolManagementSystem.Data;

namespace SchoolManagementSystem.Services

{
    public class GenericServices <TEntity> where TEntity : class
    {
        private readonly AppDbContext context;
        public GenericServices(AppDbContext context)
        {
            this.context = context;
        }
        public void Add(TEntity entity)
        {
            context.Set<TEntity>().Add(entity);
            context.SaveChanges();
        }
        public void AddFille(TEntity entity)
        {
          
        }

    }
}
