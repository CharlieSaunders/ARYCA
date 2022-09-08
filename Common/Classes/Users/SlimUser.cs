using Common.Entities.Users;

namespace Common.Classes.Users;

public class SlimUser
{
	public SlimUser()
	{

	}

	public SlimUser(User user, int habits, int pledges, int stocks)
	{
		Reference = user.UserReference;
		FirstName = user.FirstName;
		SecondName = user.SecondName;
		AvatarUrl = user.AvatarUrl;
		Title = user.Title;
		Theme = user.Theme;
		Balance = user.Balance;
		Habits = habits;
		Pledges = pledges;
		Stocks = stocks;
		ParticleEffect = user.ParticleEffect;
		FontFamily = user.FontFamily;
	}

	public SlimUser(User user)
	{
		Reference = user.UserReference;
		FirstName = user.FirstName;
		SecondName = user.SecondName;
		AvatarUrl = user.AvatarUrl;
		Title = user.Title;
		Theme = user.Theme;
		Balance = user.Balance;
		Habits = 0;
		Pledges = 0;
		Stocks = 0;
		ParticleEffect = user.ParticleEffect;
		FontFamily = user.FontFamily;
	}

	public Guid Reference { get; set; }
	public string FirstName { get; set; }
	public string SecondName { get; set; }
	public string AvatarUrl { get; set; }
	public string Title { get; set; }
	public string Theme { get; set; }
	public decimal Balance { get; set; }
	public int Habits { get; set; }
	public int Pledges { get; set; }
	public int Stocks { get; set; }
	public string ParticleEffect { get; set; }
	public string FontFamily { get; set; }
}