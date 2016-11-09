using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemplateFactory.Model
{
   public class TemplateConfig
    {
        public long TemplateId { get; set; }

        public bool IsEnabled { get; set; }

        public bool IsFree { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public long DefaultSkinId { get; set; }

        public string PreviewImage { get; set; }
    }
}
