using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdsReportingPortal.Model.Entities
{
    public class BaseEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public string Id { get; set; }
        public DateTime Created { get; set; } = DateTime.UtcNow;
        public DateTime? DateUpdated { get; set; }
        public bool IsDeleted { get; set; } = false;

    }
}
