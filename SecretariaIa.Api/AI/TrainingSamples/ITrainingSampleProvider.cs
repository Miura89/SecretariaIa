namespace SecretariaIa.Api.AI.TrainingSamples
{
	public interface ITrainingSamplesProvider
	{
		Task<string> GetCreateExpenseSamplesAsync(CancellationToken ct);
	}
}
