using Microsoft.EntityFrameworkCore;
using SecretariaIa.Domain.Entities;
using SecretariaIa.Domain.Interfaces;
using SecretariaIa.Infrasctructure.Data.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SecretariaIa.Infrasctructure.Data.Repositories
{
	public class Repository<TId, TEntity> : IRepositoryWrite<TId, TEntity>, IRepositoryRead<TId, TEntity> where TEntity : Entity<TId>
	{
		protected readonly ApplicationContext _context;

		public Repository(ApplicationContext context)
			=> _context = context ?? throw new ArgumentNullException(nameof(context));

		public IUnitOfWork UnitOfWork => _context;

		public virtual async Task<TEntity?> FindAsync(TId id, bool ignoreExcluded = true)
		{
			var entity = await _context.Set<TEntity>().FindAsync(id);

			if (ignoreExcluded)
				return entity?.ExcludedAt is not null ? null : entity;

			return entity;
		}

		public async Task<TEntity?> FindAsync(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object?>>[] includes)
		{
			var query = includes?.Aggregate(_context.Set<TEntity>().AsQueryable(), (entity, inclusao)
					=> entity.Include(inclusao));

			if (query is not null)
			{
				var entity = await query.FirstOrDefaultAsync(predicate);
				return entity?.ExcludedAt is not null ? null : entity;
			}

			return null;
		}

		public async Task<TEntity?> FindAsync(Expression<Func<TEntity, bool>> predicate)
		{
			return await _context.Set<TEntity>().FirstOrDefaultAsync(predicate);
		}

		public async Task CreateAsync(TEntity entity, Guid? createdBy)
		{
			//await CheckTenantPermission(entity);

			entity.Create(createdBy);
			await _context.AddAsync(entity);
		}

		public async Task CreateAsync(IEnumerable<TEntity> entities, Guid? createdBy)
		{
			foreach (var entity in entities)
				entity.Create(createdBy);

			await _context.Set<TEntity>().AddRangeAsync(entities);
		}

		public async Task UpdateAsync(TEntity entity, Guid? updatedBy)
		{
			//await CheckTenantPermission(entity);

			entity.Update(updatedBy);
			_context.Set<TEntity>().Update(entity);

			await Task.CompletedTask;
		}

		public async Task UpdateAsync(IEnumerable<TEntity> entities, Guid? updatedBy)
		{
			foreach (var entity in entities)
				entity.Update(updatedBy);

			_context.Set<TEntity>().UpdateRange(entities);

			await Task.CompletedTask;
		}

		public async Task DeleteAsync(TId id)
		{
			var entity = await _context.Set<TEntity>().FindAsync(id);

			if (entity is not null)
			{
				//await CheckTenantPermission(entity);
				_context.Set<TEntity>().Remove(entity);
			}
		}

		public async Task DeleteAsync(TEntity entity)
		{
			if (entity is not null)
			{
				//await CheckTenantPermission(entity);
				_context.Set<TEntity>().Remove(entity);
			}
		}

		public async Task DeleteAsync(IEnumerable<TEntity> entities)
		{
			if (entities is not null && entities.Any())
				_context.Set<TEntity>().RemoveRange(entities);

			await Task.CompletedTask;
		}

		public async Task DeleteAsync(Expression<Func<TEntity, bool>> predicate)
		{
			var entity = await _context.Set<TEntity>().Where(predicate).ToListAsync();

			if (entity is not null)
				_context.Set<TEntity>().RemoveRange(entity);
		}

		public async Task<TEntity?> ExcludeAsync(TId id, Guid? excludedBy)
		{
			var entity = await _context.Set<TEntity>().FindAsync(id);

			if (entity is not null)
			{
				//await CheckTenantPermission(entity);
				entity.Exclude(excludedBy);
				_context.Set<TEntity>().Update(entity);
			}

			return entity;
		}

		public async Task ExcludeAsync(IEnumerable<TEntity> entities, Guid? excludedBy)
		{
			foreach (var entity in entities)
				entity.Exclude(excludedBy);

			_context.Set<TEntity>().UpdateRange(entities);
			await Task.CompletedTask;
		}
	}
}
