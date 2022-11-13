using DatabaseEntityManagement.Data.Context.QueryContext.Service;
using DatabaseEntityManagement.Data.Entities._Base;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseEntityManagement.Data.Repositories._Base
{
    public class BaseRepository<TContext, TEntity> : IBaseRepository<TEntity>
        where TContext : DbContext
        where TEntity : BaseEntity
    {
        protected IQueryContextService QueryContextService;
        protected List<string> DefaultIncludes;

        protected TContext Context
        {
            get
            {
                var queryContext = QueryContextService.Get<TContext>();
                if (queryContext == null) throw new InvalidOperationException($"No ambient context found the context with name {typeof(TContext).FullName}. Make sure to call the repository method inside of a query scope.");

                return queryContext;
            }
        }

        protected DbSet<TEntity> Data => Context.Set<TEntity>();

        public BaseRepository(IQueryContextService queryContextService)
        {
            QueryContextService = queryContextService;
            this.DefaultIncludes = new List<string>();
        }

        public void Insert(TEntity entity, bool save = true)
        {
            entity.CreationDate = DateTime.Now;
            Data.Add(entity);

            if (save) Context.SaveChanges();
        }

        public void Update(TEntity entity, bool save = true)
        {
            entity.ModifiedDate = DateTime.Now;

            Data.Attach(entity);
            Context.Entry(entity).State = EntityState.Modified;

            if (save) Context.SaveChanges();
        }

        public void Delete(TEntity entity)
        {
            if (!IsAttached(entity))
                Attach(entity);

            SetPropertyModified(entity, nameof(BaseEntity.Deprecated), true);
            UpdateProperties(entity);
        }

        public void UpdateProperties(TEntity entity)
        {
            entity.ModifiedDate = DateTime.Now;
            Context.SaveChanges();
        }

        public ICollection<TEntity> GetAll(bool includeDeprecated = false)
        {
            return GetData().Where(entity => includeDeprecated || !entity.Deprecated).ToList();
        }

        public TEntity GetById(int id)
        {
            return GetData().FirstOrDefault(entity => entity.Id.Equals(id));
        }

        public bool IsAttached(TEntity entity)
        {
            return Data.Local.Any(localEntity => localEntity == entity);
        }

        public void Attach(TEntity entity)
        {
            Data.Attach(entity);
        }

        public void Detach(TEntity entity)
        {
            if (IsAttached(entity))
                SetState(entity, EEntityState.DETACHED);
        }

        public void SetState(TEntity entity, EEntityState state)
        {
            Context.Entry(entity).State = state.GetEntityState();
        }

        public void SetValues(TEntity entity, TEntity source)
        {
            Context.Entry(entity).CurrentValues.SetValues(source);
        }

        public void SetPropertyModified(TEntity entity, string propertyName, object value)
        {
            var property = Context.Entry(entity).Property(propertyName);
            if (property == null) return;

            property.CurrentValue = value;
            property.IsModified = true;
        }

        public void SetReferenceModified(TEntity entity, string propertyName, object value)
        {
            var reference = Context.Entry(entity).Reference(propertyName);
            if (reference == null) return;

            reference.CurrentValue = value;
        }

        public void SetCollectionModified(TEntity entity, string collectionName, object value)
        {
            var collection = Context.Entry(entity).Collection(collectionName);
            if (collection == null) return;

            collection.CurrentValue = value;
        }

        public void Load(TEntity entity, string propertyName)
        {
            Context.Entry(entity).Reference(propertyName).Load();
        }

        public void LoadCollection(TEntity entity, string propertyName)
        {
            Context.Entry(entity).Collection(propertyName).Load();
        }

        public void SaveChanges()
        {
            Context.SaveChanges();
        }

        protected DbQuery<TEntity> GetData()
        {
            DbQuery<TEntity> data = Data;

            foreach (var include in DefaultIncludes)
                data = data.Include(include);

            return data;
        }
    }
}
