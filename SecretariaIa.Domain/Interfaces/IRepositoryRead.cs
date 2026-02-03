using SecretariaIa.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace SecretariaIa.Domain.Interfaces
{
	public interface IRepositoryRead<TId, TEntity> where TEntity : Entity<TId>
	{
		/// <summary>
		/// Procura uma entidade pelo ID.
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		Task<TEntity?> FindAsync(TId id, bool ignoreExcluded = true);
		/// <summary>
		/// Procura uma entidade pela condição do predicado, podendo incluir entidades relacionadas (Join)
		/// </summary>
		/// <param name="predicate"></param>
		/// <param name="includes"></param>
		/// <returns></returns>
		Task<TEntity?> FindAsync(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object?>>[] includes);
		/// <summary>
		/// Procura uma entidade pela condição do predicado.
		/// </summary>
		/// <param name="predicate"></param>
		/// <returns></returns>
		Task<TEntity?> FindAsync(Expression<Func<TEntity, bool>> predicate);
	}
}
