namespace Models.EF
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Language")]
    public partial class Language
    {
        [StringLength(2)]
        public string ID { get; set; }

        [StringLength(50)]
        public string Name { get; set; }

        public bool IsDefault { get; set; }
    }
}