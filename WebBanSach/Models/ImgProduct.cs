namespace WebBanSach.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ImgProduct")]
    public partial class ImgProduct
    {
        public int ID { get; set; }

        public int IDProduct { get; set; }

        public string Link { get; set; }

        public virtual Product Product { get; set; }
    }
}
