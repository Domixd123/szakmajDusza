namespace szakmajDusza
{
	public class Kazamata
	{
		public string Name { get; set; }
		public List<Card> Defenders { get; set; }
		public Type Tipus { get; set; }
		public Reward reward {  get; set; }
		public Kazamata(string name, string type, string reward,List<Card> defenders)
		{
			Name = name;
			Tipus = type switch
			{
				"egyszeru" => Type.egyszeru,
				"kis" => Type.kis,
				"nagy" => Type.nagy,
				_ => Type.kis
			};
			this.reward = reward switch
			{
				"eletero" => Reward.eletero,
				"sebzes" => Reward.sebzes,
				_ => Reward.newcard
			};
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
		public enum Type : byte
		{
			egyszeru,
			kis,
			nagy
		}
		public enum Reward : byte
		{
			eletero,
			sebzes,
			newcard
		}
	}
}
