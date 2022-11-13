using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseEntityManagement.Data.Repositories._Base
{
    public enum EEntityState
    {
        ADDED,
        MODIFIED,
        DELETED,
        DETACHED,
        UNCHANGED
    }

    public static class EntityStateExtension
    {
        public static EntityState GetEntityState(this EEntityState state)
        {
            switch (state)
            {
                case EEntityState.ADDED:
                    return EntityState.Added;
                case EEntityState.MODIFIED:
                    return EntityState.Modified;
                case EEntityState.DELETED:
                    return EntityState.Deleted;
                case EEntityState.DETACHED:
                    return EntityState.Detached;
                case EEntityState.UNCHANGED:
                default:
                    return EntityState.Unchanged;
            }
        }
    }
}
