using Microsoft.Extensions.Configuration;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace SecretariaIa.Infrasctructure.Data
{
	public class ConnectionSqlFactory : IConnectionSqlFactory
	{
		private readonly string _connectionString;
		public ConnectionSqlFactory(IConfiguration configuration)
		{
			_connectionString = configuration.GetConnectionString("DefaultDB")
				?? throw new ArgumentNullException("Connection string 'DefaultConnection' not found in configuration.");
		}
		public DbConnection CreateConnection()
		{
			return new NpgsqlConnection(_connectionString);
		}
	}
}
