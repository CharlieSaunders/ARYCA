namespace Common.DTO.Configs
{
	public class UpdateAppConfigRequest
	{
		public string AppName { get; set; }
		public DateTime Date { get; set; }
		public string ImageText { get; set; }
		public string ImageUrl { get; set; }
	}
}
