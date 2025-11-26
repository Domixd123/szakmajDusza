using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace szakmajDusza
{
    public class Kornyezet
    {
        public string Name { get; set; }
        public List<Card> SimaKartyak { get; set; }
        public List<Card> VezerKartyak { get; set; }
        public List<Kazamata> Kazamatak { get; set; }
        public List<Card> Gyujtemeny { get; set; }
    }
}
