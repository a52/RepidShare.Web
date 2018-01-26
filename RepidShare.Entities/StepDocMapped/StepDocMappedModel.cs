using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepidShare.Entities
{
    public class StepDocMappedModel : BaseModel
    {
        public int StepDocMappedID { get; set; }
        public int StepID { get; set; }
        public int DocumentID { get; set; }
        public int DisplayOrder { get; set; }

        public string StepName { get; set; }
        public string StepDiscription { get; set; }
        public string DocumentTitle { get; set; }
        public float? Price { get; set; }
    }
    public class ViewStepDocMappedModel : ViewParameters
    {
        //   [LocalizedDisplayName(typeof(GroupResource), "GroupName")]
        public List<StepDocMappedModel> StepDocMappedList { get; set; }
        public int DeletedStepDocMappedID { get; set; }
        public int DeletedBy { get; set; }
    }
}
