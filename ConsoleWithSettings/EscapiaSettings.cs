
namespace ConsoleWithSettings
{
	public class EscapiaSettings
	{
		public string IsProduction { get; set; }
		public string Username { get; set; }
		public string Password { get; set; }
		public string MaxUnitShopsPerRequest { get; set; }
		public string MaxConcurrentTasks { get; set; }
		public string MaxResponsesBatchedToSave { get; set; }
		public string BookingWindowDaysLimit { get; set; }
	}
}
