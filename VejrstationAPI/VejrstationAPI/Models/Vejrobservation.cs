using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace VejrstationAPI.Models
{
    public class Vejrobservation
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int VejrobservationId { get; set; }
        public DateTime Tidspunkt { get; set; }
        public Sted Sted { get; set; }              //Navigational
        public string StedNavn { get; set; }        //Foreign Key
        public double Temperatur { get; set; }      //Celcius, Precistion 1 decimal
        public double Fahrenheit => 32 + (int) (Temperatur / 0.5556);
        [Range(0,100)]
        public int Luftfugtighed { get; set; }      //0-100 %
        public double Lufttryk { get; set; }        //Millibar, Precition 1 decimal 
    }
}
