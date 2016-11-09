using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemplateFactory.Model
{
   public class TemplateComponent
    {
        public long TempComponentId { get; set; }
        public int Index { get; set; }
        public long ComponentId { get; set; }
        public long StyleId { get; set; }
    }
}
