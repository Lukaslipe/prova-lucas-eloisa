using System;

namespace EloisaSantos.Models;

public class Consumo
{
        public int Id { get; set; }
        public string cpf { get; set; } = string.Empty;
        public int mes { get; set; }
        public int ano { get; set; }
        public double m3Consumidos { get; set; }
        public string bandeira { get; set; } = string.Empty;
        public bool possuiEsgoto { get; set; }
        
        
}
