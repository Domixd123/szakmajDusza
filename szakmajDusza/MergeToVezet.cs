using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace szakmajDusza
{
    public class MergeToVezet
    {
        public static void haromToVezer(Card elso, Card masodik, Card harmaid)
        {
            //MainWindow.Gyujtemeny;
            //MainWindow.AllLeaders;
            if (elso.Vezer||masodik.Vezer||harmaid.Vezer)
            {
                return;
                //Kristóf implement error vmessage in UI
            }
            List<Card> obtainedVezer = new List<Card>();
            foreach (var item in MainWindow.Gyujtemeny)
            {
                if (item.Vezer)
                {
                    obtainedVezer.Add(item);
                }
            }
            if (obtainedVezer.Count>=MainWindow.AllCardsDict.Values.Count)
            {
                return;
                //Kristóf implement error vmessage in UI
            }
            for (int i = 0; i < MainWindow.Gyujtemeny.Count; i++)
            {
                if (MainWindow.Gyujtemeny[i].Name==elso.Name|| MainWindow.Gyujtemeny[i].Name == masodik.Name|| MainWindow.Gyujtemeny[i].Name == harmaid.Name)
                {
                    MainWindow.Gyujtemeny.RemoveAt(i);
                }
            }
            for (int i = 0; i < MainWindow.Jatekos.Count; i++)
            {
                if (MainWindow.Jatekos[i].Name == elso.Name || MainWindow.Jatekos[i].Name == masodik.Name || MainWindow.Jatekos[i].Name == harmaid.Name)
                {
                    MainWindow.Jatekos.RemoveAt(i);
                }
            }
            //int x = 0;
            foreach (var item in MainWindow.AllCardsDict.Values)
            {
                int x = 0;
                for (int j = 0; j < obtainedVezer.Count; j++)
                {
                    if (item.Name == obtainedVezer[j].Name)
                    {
                        x++;
                    }
                }
                if (x == 0)
                {
                    Card ujVezer = new Card(item.Name, item.Damage, item.HP, item.Tipus.ToString(), true);
                    MainWindow.Gyujtemeny.Add(ujVezer);
                    break;
                }
            }
            
        }
    }
}
