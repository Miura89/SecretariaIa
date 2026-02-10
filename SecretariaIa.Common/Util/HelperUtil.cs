using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretariaIa.Common.Util
{
	public static class CategoryFormatter
	{
		public static string Format(int category)
		{
			return category switch
			{
				1 => "Alimentação",
				2 => "Transporte",
				3 => "Moradia",
				4 => "Saúde",
				5 => "Lazer",
				6 => "Contas",
				7 => "Compras",
				8 => "Educação",
				9 => "Outros",
				_ => "Não categorizado"
			};
		}
	}
	public static class DateFormatter
	{
		public static string FormatDateTimeHuman(DateTime dateTime)
		{
			var today = DateTime.Today;
			var yesterday = today.AddDays(-1);

			if (dateTime.Date == today)
				return $"hoje às {dateTime:HH:mm}";

			if (dateTime.Date == yesterday)
				return $"ontem às {dateTime:HH:mm}";

			return dateTime.ToString("dd/MM/yyyy 'às' HH:mm");
		}
	}


}
