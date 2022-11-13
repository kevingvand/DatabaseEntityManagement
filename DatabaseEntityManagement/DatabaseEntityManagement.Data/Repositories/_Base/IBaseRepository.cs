using DatabaseEntityManagement.Data.Entities._Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseEntityManagement.Data.Repositories._Base
{
    public interface IBaseRepository<TEntity> where TEntity : BaseEntity
    {
        void Insert(TEntity entity, bool save = true);
        void Update(TEntity entity, bool save = true);

        void Delete(TEntity entity);

        void UpdateProperties(TEntity entity);

        ICollection<TEntity> GetAll(bool includeDeprecated = false);

        TEntity GetById(int id);

        bool IsAttached(TEntity entity);

        void Attach(TEntity entity);
        void Detach(TEntity entity);

        void SetState(TEntity entity, EEntityState state);

        void SetValues(TEntity entity, TEntity source);

        void SetPropertyModified(TEntity entity, string propertyName, object value);
        void SetReferenceModified(TEntity entity, string propertyName, object value);
        void SetCollectionModified(TEntity entity, string collectionName, object value);

        void Load(TEntity entity, string propertyName);

        void LoadCollection(TEntity entity, string propertyName);

        void SaveChanges();
    }
}
