namespace szakmajDusza
{
	public class Kazamata
	{
		public string Name { get; set; }
		public List<Card> Defenders { get; set; }
		public KazamataType Tipus { get; set; }
		public KazamataReward reward {  get; set; }
		public Kazamata(string name, string type, string reward,List<Card> defenders)
		{
			Name = name;
			Tipus = type switch
			{
				"egyszeru" => KazamataType.egyszeru,
				"kis" => KazamataType.kis,
				"nagy" => KazamataType.nagy,
				_ => KazamataType.kis
			};
			this.reward=StringToKazamataReward(reward);
			Defenders = defenders;
		}
		public bool Equals(Kazamata? obj)
		{
			if (obj.Name == this.Name && obj.Defenders==this.Defenders && obj.Tipus == this.Tipus&&obj.reward==this.reward)
			{
				return true;
			}
			return false;
		}
		public static string KazamataRewardToString(KazamataReward reward)
		{
			string Reward;
			Reward = reward switch
			{
				KazamataReward.eletero => "eletero",
				KazamataReward.sebzes => "sebzes",
				_ => ""
			};
			return Reward;
		}
		public static KazamataReward StringToKazamataReward(string reward)
		{
			KazamataReward Reward;
			Reward = reward switch
			{
				"eletero" => KazamataReward.eletero,
				"sebzes" => KazamataReward.sebzes,
				_ => KazamataReward.newcard
			};
			return Reward;
		}
	}
	public enum KazamataType : byte
	{
		egyszeru,
		kis,
		nagy
	}
	public enum KazamataReward : byte
	{
		eletero,
		sebzes,
		newcard
	}
}
