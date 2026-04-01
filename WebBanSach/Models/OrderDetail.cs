namespace WebBanSach.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("OrderDetail")]
    public partial class OrderDetail
    {
        public int ID { get; set; }

        public int IDOrder { get; set; }

        public int IDProduct { get; set; }

        public int Quality { get; set; }

        public double Price { get; set; }

        public virtual Order Order { get; set; }
    }
}
