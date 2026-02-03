using SecretariaIa.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace SecretariaIa.Domain.Interfaces
{
	public interface IRepositoryWrite<TId, TEntity> where TEntity : Entity<TId>
	{
		IUnitOfWork UnitOfWork { get; }

		/// <summary>
		/// Insere uma entidade no banco.
		/// </summary>
		/// <param name="entity"></param>
		/// <param name="createdBy"></param>
		/// <returns></returns>
		Task CreateAsync(TEntity entity, Guid? createdBy);
		/// <summary>
		/// Insere uma ou mais entidades no banco.
		/// </summary>
		/// <param name="entity"></param>
		/// <param name="createdBy"></param>
		/// <returns></returns>
		Task CreateAsync(IEnumerable<TEntity> entities, Guid? createdBy);
		/// <summary>
		/// Atualiza uma entidade no banco.
		/// </summary>
		/// <param name="entity"></param>
		/// <param name="updatedBy"></param>
		/// <returns></returns>
		Task UpdateAsync(TEntity entity, Guid? updatedBy);
		/// <summary>
		/// Atualiza uma ou mais entidade no banco.
		/// </summary>
		/// <param name="entities"></param>
		/// <param name="updatedBy"></param>
		/// <returns></returns>
		Task UpdateAsync(IEnumerable<TEntity> entities, Guid? updatedBy);
		/// <summary>
		/// Deleta fisicamente uma entidade do banco pelo ID.
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		Task DeleteAsync(TId id);
		/// <summary>
		/// Deleta fisicamente uma entidade do banco.
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		Task DeleteAsync(TEntity id);
		/// <summary>
		/// Deleta fisicamente uma ou mais entidades do banco.
		/// </summary>
		/// <param name="entities"></param>
		/// <returns></returns>
		Task DeleteAsync(IEnumerable<TEntity> entities);
		/// <summary>
		/// Deleta fisicamente uma entidade do banco pela condição do predicado.
		/// </summary>
		/// <param name="predicate"></param>
		/// <returns></returns>
		Task DeleteAsync(Expression<Func<TEntity, bool>> predicate);
		/// <summary>
		/// Exclui logicamente uma entidade do banco pelo ID.
		/// </summary>
		/// <param name="id"></param>
		/// <param name="excludedBy"></param>
		/// <returns></returns>
		Task<TEntity?> ExcludeAsync(TId id, Guid? excludedBy);
		/// <summary>
		/// Exclui logicamente uma ou mais entidades do banco.
		/// </summary>
		/// <param name="entities"></param>
		/// <param name="excludedBy"></param>
		/// <returns></returns>
		Task ExcludeAsync(IEnumerable<TEntity> entities, Guid? excludedBy);
	}
}
