using System;
using System.Collections.Generic;

#nullable disable
namespace ResourcedetailsAPI.Models
{
    public partial class Resourcedetail
    {
        public int Resourcedetailid { get; set; }
        public string Projectname { get; set; }
        public string Resourcegroupname { get; set; }
        public string Subscriptionid { get; set; }
        public string Projectowneremail { get; set; }
        public int Leveldetailid { get; set; }
    }
}
