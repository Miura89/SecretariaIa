using MediatR;
using SecretariaIa.Domain.Entities;
using SecretariaIa.Domain.Enums;
using SecretariaIa.Domain.Interfaces;
using SecretariaIa.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretariaIa.Domain.Commands.AppointmentCommands
{
	public class CreateAppointmentCommand : IRequest<CommandResultResponse<Guid>>
	{
		public CreateAppointmentCommand(string phone, string title, DateTime scheduledAt, int? remindBeforeMinutes, Guid? createdBy)
		{
			Phone = phone;
			Title = title;
			ScheduledAt = scheduledAt;
			RemindBeforeMinutes = remindBeforeMinutes;
			CreatedBy = createdBy;
		}

		public string Phone { get; set; }
		public string Title { get; set; } = string.Empty;
		public DateTime ScheduledAt { get; set; }
		public int? RemindBeforeMinutes { get; set; }
		public Guid? CreatedBy { get; set; }
	}
	public class CreateAppointmentCommandHandler : IRequestHandler<CreateAppointmentCommand, CommandResultResponse<Guid>>
	{
		private readonly IAppointmentRepository _appointmentRepository;
		private readonly IIdentityUserRepository _repository;
		public CreateAppointmentCommandHandler(IAppointmentRepository appointmentRepository, IIdentityUserRepository repository)
		{
			_appointmentRepository = appointmentRepository;
			_repository = repository;
		}
		public async Task<CommandResultResponse<Guid>> Handle(CreateAppointmentCommand request, CancellationToken cancellationToken)
		{
			CommandResultResponse<Guid> response = new();

			request.Phone = request.Phone.Replace("whatsapp:", "");
			var user = await _repository.FindAsync(x => x.Phone == request.Phone && x.Type == TypeUser.CUSTOMER);
			if (user == null)
				return response.AddNotifications("Usuario não encontrado");

			var scheduled = request.ScheduledAt;
			if (scheduled.Kind == DateTimeKind.Unspecified)
			{
				scheduled = DateTime.SpecifyKind(scheduled, DateTimeKind.Local);
			}

			var utcDate = scheduled.ToUniversalTime();

			Appointment entity = new(user.Id, user, request.Title, utcDate, request.RemindBeforeMinutes ?? 0, false);
			await _appointmentRepository.CreateAsync(entity, request.CreatedBy);
			await _appointmentRepository.UnitOfWork.Commit(cancellationToken);

			response.SetResult(entity.Id);
			return response;
		}
	}
}
