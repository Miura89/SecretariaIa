namespace SecretariaIa.Api.DTOs
{
	public class ExpensesDTO 
	{
		public Guid Id { get; set; }
		public decimal Value { get; set; }
		public string Description { get; set; }
		public DateTime Date { get; set; }
		public string Category { get; set; }
	}
}
