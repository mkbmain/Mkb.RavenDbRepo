using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Mkb.RavenDbRepo.Entities;

namespace Mkb.RavenDbRepo.Async.Interfaces
{
    public interface IRavenReaderRepoAsync<in T> where T : RavenEntity
    {
        Task<TEntity> Get<TEntity>(Expression<Func<TEntity, bool>> where,
            Expression<Func<TEntity, object>> orderBy = null, bool orderByDescending = false, bool includeSoftDelete = false) where TEntity : T;

        Task<TOut> Get<TEntity, TOut>(Expression<Func<TEntity, bool>> where, Expression<Func<TEntity, TOut>> projection, Expression<Func<TEntity, object>> orderBy = null,
            bool orderByDescending = false,
            bool includeSoftDelete = false)
            where TEntity : T;

        Task<List<TEntity>> GetAll<TEntity>(Expression<Func<TEntity, bool>> where,
            Expression<Func<TEntity, object>> orderBy = null,
            bool orderByDescending = false,
            bool includeSoftDelete = false) where TEntity : T;

        Task<List<TOut>> GetAll<TEntity, TOut>(Expression<Func<TEntity, bool>> where, Expression<Func<TEntity, TOut>> projection,
            Expression<Func<TEntity, object>> orderBy = null,
            bool orderByDescending = false,
            bool includeSoftDelete = false) where TEntity : T;
    }
}