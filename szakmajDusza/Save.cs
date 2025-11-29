using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace szakmajDusza
{
	public class Save
	{
		public static string fileName = "";
		public static void SaveProgress()
		{
			if (fileName == null || fileName == "")
			{
				//ask for a unique filename
			}
			Directory.CreateDirectory("saves");
			StreamWriter sw =new StreamWriter("saves/"+fileName);

			foreach (var item in MainWindow.AllCardsDict.Values)
			{
				sw.WriteLine($"uj kartya;{item.Name};{item.Damage};{item.HP};{Card.TipusToString(item.Tipus)}");
			}
			sw.WriteLine();
			foreach (var item in MainWindow.AllLeadersDict.Values)
			{
				sw.WriteLine($"uj vezer;{item.Name};{item.OriginName};{item.Bonus}");
			}
			sw.WriteLine();
			foreach(var kazamata in MainWindow.AllKazamataDict.Values)
			{
				string kazamataString = $"uj kazamata;{Kazamata.KazamataTypeToString(kazamata.Tipus)};{kazamata.Name};";
				foreach (var defender in kazamata.Defenders)
				{
					string defenderItems = "";
					foreach (var item in defender.Items)
					{
						defenderItems += $"{item.Name}-{item.Level},";
					}
					defenderItems=defenderItems.Substring(0,defenderItems.Length - 1);
					kazamataString += $"{defender.Name}:{defenderItems};";
				}
				if (kazamata.Tipus == KazamataType.egyszeru || kazamata.Tipus == KazamataType.kis)
				{
					kazamataString += $"{Kazamata.KazamataRewardToString(kazamata.reward)}";
				}
				else
				{
					kazamataString = kazamataString.Substring(0, kazamataString.Length - 1);
				}
				sw.WriteLine(kazamataString);
			}
			sw.WriteLine();
			foreach (var item in MainWindow.Gyujtemeny)
			{
				sw.WriteLine($"felvetel gyujtemenybe;{item.Name}");
			}
			sw.WriteLine();
			string pakliString = "";
			foreach (var item in MainWindow.Jatekos)
			{
				pakliString += item.Name + ",";
			}
			pakliString = pakliString.Substring(0,pakliString.Length-1);
			sw.WriteLine($"uj pakli;{pakliString}");
			sw.WriteLine();
			foreach (var item in MainWindow.Gyujtemeny)
			{
				int eleteroAmmount = item.HP - MainWindow.AllCardsDict[item.Name].HP;
				if(eleteroAmmount > 0)
				{
					sw.WriteLine($"kartyafejlesztes;{item.Name};eletero;{eleteroAmmount}");
				}
				int sebzesAmmount = item.Damage - MainWindow.AllCardsDict[item.Name].Damage;
				if (sebzesAmmount > 0)
				{
					sw.WriteLine($"kartyafejlesztes;{item.Name};sebzes;{sebzesAmmount}");
				}
			}
			sw.WriteLine();
			sw.WriteLine($"arany;{Item.GoldOwned}");
			sw.WriteLine($"shopRefreshPrice;{Item.shopRefreshPrice}");
			sw.WriteLine($"shopItemCount;{Item.shopItemCount}");
			sw.WriteLine();
			foreach (var item in Item.Items.Values)
			{
				string itemBuyable = item.Buyable ? "igen" : "nem";
				sw.WriteLine($"item;{item.Name};{itemBuyable};{item.MaxLevel};{item.Price};{item.Level};{item.OwnedCount};{item.BaseVariable};{item.InRotation}");
			}
			sw.WriteLine();
			foreach(var item in MainWindow.Gyujtemeny)
			{
				string itemString = "";
				foreach (var item2 in item.Items)
				{
					itemString += item2.Name+",";
				}
				itemString=itemString.Substring(0,itemString.Length-1);
				sw.WriteLine($"jatekos kartya items;{item.Name};{itemString}");
			}
			sw.Close();
		}
	}
}
