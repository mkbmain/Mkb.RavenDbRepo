using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Mkb.RavenDbRepo.Entities;

namespace Mkb.RavenDbRepo.Sync.Interfaces
{
    public interface IRavenReaderRepo<in T> where T : RavenEntity
    {
        bool Any<TEntity>(Expression<Func<TEntity, bool>> where,bool includeSoftDelete =false) where TEntity : T;
        
        TEntity Get<TEntity>(Expression<Func<TEntity, bool>> where,
            Expression<Func<TEntity, object>> orderBy = null, bool orderByDescending = false, bool includeSoftDelete = false) where TEntity : T;

        TOut Get<TEntity, TOut>(Expression<Func<TEntity, bool>> where, Expression<Func<TEntity, TOut>> projection, Expression<Func<TEntity, object>> orderBy = null,
            bool orderByDescending = false,
            bool includeSoftDelete = false)
            where TEntity : T;

        List<TEntity> GetAll<TEntity>(Expression<Func<TEntity, bool>> where,
            Expression<Func<TEntity, object>> orderBy = null,
            bool orderByDescending = false,
            bool includeSoftDelete = false) where TEntity : T;

        List<TOut> GetAll<TEntity, TOut>(Expression<Func<TEntity, bool>> where, Expression<Func<TEntity, TOut>> projection,
            Expression<Func<TEntity, object>> orderBy = null,
            bool orderByDescending = false,
            bool includeSoftDelete = false) where TEntity : T;
    }
}