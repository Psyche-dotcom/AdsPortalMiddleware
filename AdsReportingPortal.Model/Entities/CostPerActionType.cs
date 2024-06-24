using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdsReportingPortal.Model.Entities
{
    public class CostPerActionType : BaseEntity
    {
        public  MetaAgeGenderCategory MetaAgeGender { get; set; }
        public string MetaAgeGenderId { get; set; }
        public string action_type { get; set; }
        public string value { get; set; }
    }
}
