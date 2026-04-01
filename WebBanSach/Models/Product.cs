namespace WebBanSach.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Product")]
    public partial class Product
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Product()
        {
            CartDetail = new HashSet<CartDetail>();
            ImgProduct = new HashSet<ImgProduct>();
        }

        public int ID { get; set; }

        [Required]
        public string ProductName { get; set; }

        public int IDSupplier { get; set; }

        public int IDCategory { get; set; }

        public double ListPrice { get; set; }

        public double? Discout { get; set; }

        public int Status { get; set; }

        public string Detail { get; set; }
        public string Author { get; set; }

        public int Quality { get; set; }
        


        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CartDetail> CartDetail { get; set; }

        public virtual Category Category { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ImgProduct> ImgProduct { get; set; }

        public virtual Supplier Supplier { get; set; }
    }
}
