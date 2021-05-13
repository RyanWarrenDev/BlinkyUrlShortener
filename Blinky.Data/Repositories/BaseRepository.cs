using Blinky.Core.Entities;
using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blinky.Data.Repositories
{
    public abstract class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity
    {
        protected readonly ILiteDatabase Context;

        protected readonly ILiteCollection<T> Collection;

        public BaseRepository(BlinkyContext context)
        {
            Context = context.Context;
            Collection = Context.GetCollection<T>();
        }

        public virtual T Insert(T entity)
        {
            Collection.EnsureIndex(x => x.Id);

            entity.CreatedDate = DateTime.Now;
            entity.UpdatedDate = DateTime.Now;

            var entityId = Collection.Insert(entity);

            return Collection.FindById(entityId.AsInt32);
        }

        public virtual IEnumerable<T> All()
        {
            return Collection.FindAll();
        }

        public virtual T FindById(int id)
        {
            return Collection.FindById(id);
        }

        public virtual void Update(T entity)
        {
            entity.CreatedDate = DateTime.Now;
            entity.UpdatedDate = DateTime.Now;

            Collection.Upsert(entity);
        }

        public virtual bool Delete(int id)
        {
            return Collection.Delete(id);
        }
    }
}
