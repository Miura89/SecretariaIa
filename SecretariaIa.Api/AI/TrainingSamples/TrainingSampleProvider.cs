namespace SecretariaIa.Api.AI.TrainingSamples
{
	public class TrainingSamplesProvider : ITrainingSamplesProvider
	{
		private readonly IHostEnvironment _env;

		public TrainingSamplesProvider(IHostEnvironment env)
		{
			_env = env;
		}

		public async Task<string> GetCreateExpenseSamplesAsync(CancellationToken ct)
		{
			var path = Path.Combine(
				_env.ContentRootPath,
				"AI",
				"TrainingSamples",
				"training_command_create_expense.json"
			);

			if (!File.Exists(path))
				throw new FileNotFoundException("Arquivo de training samples não encontrado.", path);

			return await File.ReadAllTextAsync(path, ct);
		}
	}

}
