using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DBTuntiLeimaus.ViewModels
{
    public class LeimausViewModel
    {
        //ID niin view ei näytä, mutta pystyy käyttämään
        public int Id { get; set; }

        //[DataType(DataType.DateTime)]
        //[DisplayFormat(DataFormatString = "{0:dd\\.MM\\.yyyy HH:mm:ss}", ApplyFormatInEditMode = false)]
        public DateTime? Sisään { get; set; }

        //[DataType(DataType.DateTime)]
        //[DisplayFormat(DataFormatString = "{0:dd\\.MM\\.yyyy HH:mm:ss}", ApplyFormatInEditMode = false)]
        public DateTime? Ulos { get; set; }
        public string Nimi { get; set; }
        public string Luokkahuone { get; set; }

    }
}