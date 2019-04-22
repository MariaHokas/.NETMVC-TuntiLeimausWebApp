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

        //[Display(Name = "Sisään")]
        //[DataType(DataType.Date) ]
        //[DisplayFormat(DataFormatString = "{hh:mm:ss}", ApplyFormatInEditMode = true)]
        //[datatype(datatype.datetime), displayformat(applyformatineditmode = true, convertemptystringtonull = true, dataformatstring = "{0: dd/mmm/yyyy hh:mm:ss}", htmlencode = true, nulldisplaytext = "-")]
        //[display(name = "sisään")]
        //[required(allowemptystrings = false, errormessage = "last update is required.")]
        public DateTime? Sisään { get; set; }

        //[display(name = "ulos")]
        //[datatype(datatype.datetime)]
        //[displayformat(dataformatstring = "{0:dd.mm.yyyy hh:mm:ss}", applyformatineditmode = true)]
        public DateTime? Ulos { get; set; }
        public string Nimi { get; set; }
        public string Luokkahuone { get; set; }

    }
}