using System;
using System.Collections.Generic;

#nullable disable

namespace LeveldetailsAPI.Models
{
    public partial class Leveldetail
    {
        public int Leveldetailid { get; set; }
        public string Leveldetailname { get; set; }
        public int Levelid { get; set; }
        public int Sequenceid { get; set; }
        public int Superleveldetailid { get; set; }

    }
}
