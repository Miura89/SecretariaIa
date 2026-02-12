using SecretariaIa.Domain.Entities;
using SecretariaIa.Domain.Interfaces;
using SecretariaIa.Infrasctructure.Data.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretariaIa.Infrasctructure.Data.Repositories
{
	public class AppointmentRepository : Repository<Guid, Appointment>, IAppointmentRepository
	{
		public AppointmentRepository(ApplicationContext context) : base(context)
		{
		}
	}
}
