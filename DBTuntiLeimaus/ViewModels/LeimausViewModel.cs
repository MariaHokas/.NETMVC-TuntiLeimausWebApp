using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DBTuntiLeimaus.ViewModels
{
    public class LeimausViewModel
    {



        //ID niin view ei näytä, mutta pystyy käyttämään
        public int ID { get; set; }
        public DateTime Sisaan { get; set; }
        public DateTime Ulos { get; set; }
        public string Nimi { get; set; }

    }
}