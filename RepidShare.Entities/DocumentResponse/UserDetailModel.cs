using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepidShare.Entities.DocumentResponse
{
    public class UserDetailModel
    {
        public int DocumentID { get; set; }
        public int UserID { get; set; }
        public int StepID { get; set; }
        public int StatusId { get; set; }
        public string StatusName { get; set; }
        public enum StatusID
        {
            Complete = 1,
            PartialComplete = 2,
            Incomplete = 3,           
        }
    }
}
