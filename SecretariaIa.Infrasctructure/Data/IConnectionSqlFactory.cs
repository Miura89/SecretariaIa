using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace SecretariaIa.Infrasctructure.Data
{
	public interface IConnectionSqlFactory
	{
		DbConnection CreateConnection();
	}
}
