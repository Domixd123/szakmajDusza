namespace szakmajDusza
{
	public class Kazamata
	{
		public string Name { get; set; }
		public List<Card> Defenders { get; set; }
		public KazamataType Tipus { get; set; }
		public KazamataReward reward {  get; set; }
		public List<string> GetDefenderNames()
		{
			 List<string> rat = new List<string>();
			foreach (Card card in Defenders)
			{
				rat.Add(card.Name);
			}
			return rat;

		}
		public Kazamata(string name, string type, string reward,List<Card> defenders)
		{
			Name = name;
			Tipus=StringToKazamataType(type);
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
				KazamataReward.arany => "arany",
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
				"arany"=>KazamataReward.arany,
				_ => KazamataReward.newcard
			};
			return Reward;
		}
		public static string KazamataTypeToString(KazamataType type)
		{
			string Type;
			Type = type switch
			{
				KazamataType.egyszeru => "egyszeru",
				KazamataType.kis => "kis",
				KazamataType.nagy => "nagy",
				_ => "hiba"
			};
			return Type;
		}
		public static KazamataType StringToKazamataType(string type)
		{
			KazamataType Type;
			Type = type switch
			{
				"egyszeru" => KazamataType.egyszeru,
				"kis" => KazamataType.kis,
				"nagy" => KazamataType.nagy,
				"egyszerű"=> KazamataType.egyszeru,
				
				_ => KazamataType.kis
			};
			return Type;
		}
		public Kazamata GetCopy()
		{
			return new Kazamata(Name,KazamataTypeToString(Tipus),KazamataRewardToString(reward),Card.GetListCopy(Defenders));
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
		arany,
		newcard
	}
}
