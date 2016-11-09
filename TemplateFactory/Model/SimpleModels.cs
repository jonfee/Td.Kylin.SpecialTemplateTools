using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemplateFactory.Model
{
    public class TemplateModel: ICloneable
    {
        public string Code { get; set; }

        public string HTML { get; set; }

        public TemplateConfig Config { get; set; }

        public TemplateSkin[] Skins { get; set; }

        public TemplateComponent[] Components { get; set; }

        public TemplateModel Clone()
        {
            return (TemplateModel)this.MemberwiseClone();
        }

        object ICloneable.Clone()
        {
            return this.Clone();
        }
    }

    public class TemplateSkin
    {
        public string Name { get; set; }

        public string Code { get; set; }

        public long SkinId { get; set; }

        public string PreviewImage { get; set; }
    }

    public class ComponentModel: ICloneable
    {
        public long ComponentId { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public int ComponentType { get; set; }

        public string DefaultData { get; set; }

        public string HTML { get; set; }

        public ComponentDefaultStyle DefaultStyle { get; set; }

        public ComponentStyle[] Styles { get; set; }

        object ICloneable.Clone()
        {
            return this.Clone();
        }

        public ComponentModel Clone()
        {
            return (ComponentModel)this.MemberwiseClone();
        }
    }

    public class ComponentStyle
    {
        public long StyleId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string PreviewImage { get; set; }
        public string Css { get; set; }
        public string Rules { get; set; }
    }
}
