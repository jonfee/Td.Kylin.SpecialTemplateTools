using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TemplateFactory.Core;
using TemplateFactory.Data;
using TemplateFactory.Model;

namespace TemplateFactory
{
    public partial class MainForm : Form
    {
        #region //常、变量

        const string TEMPLATES_ROOT = @"temp\templates";            //模板路径        
        const string TEMPLATE_SKIN_FOLDERNAME = @"skin";            //模板皮肤路径
        const string TEMPLATE_FILE_HTML = @"html.txt";              //模板HTML文件名
        const string TEMPLATE_FILE_CONFIG = @"config.txt";          //模板配置信息文件名
        const string TEMPLATE_FILE_COMPONENTS = @"components.txt";  //模板组件信息文件名
        const string TEMPLATE_FILE_SKIN_CONFIG = @"config.txt";     //模板皮肤配置信息文件名

        const string COMPONENTS_ROOT = @"temp\components";              //组件路径
        const string COMPONENT_STYLE_FOLDERNAME = @"style";             //组件风格路径
        const string COMPONENT_FILE_HTML = @"html.txt";                 //组件HTML文件名
        const string COMPONENT_FILE_CONFIG = @"config.txt";             //组件配置信息文件名
        const string COMPONENT_FILE_DEFAULTSTYLE = @"defaultstyle.txt"; //组件默认风格文件名
        const string COMPONENT_FILE_STYLE_CONFIG = @"config.txt";       //组件风格配置信息文件名
        const string COMPONENT_FILE_STYLE_CSS = @"css.txt";             //组件风格样式信息文件名
        const string COMPONENT_FILE_STYLE_Rules = @"rules.txt";         //组件风格规则信息文件名

        Encoding encoding = Encoding.Default;

        Dictionary<string, string[]> dbTemplateSkins = null;     //库中模板及皮肤
        Dictionary<string, string[]> dbComponentStyles = null;  //库中组件及风格

        List<TemplateModel> udTemplateSkins = null;    //可更新的模板及皮肤
        List<ComponentModel> udComponentStyles = null;  //可更新组件及风格

        SpecialService service = null;  //数据操作服务实例

        #endregion

        public MainForm()
        {
            InitializeComponent();

            service = new SpecialService();

            updateTemplateComponentInfo();

            this.combUpdateTemplates.DrawMode = DrawMode.OwnerDrawVariable;
            this.combUpdateTemplates.DrawItem += new DrawItemEventHandler(combUpdateTemplates_DrawItem);
            this.combUpdateComponents.DrawMode = DrawMode.OwnerDrawVariable;
            this.combUpdateComponents.DrawItem += new DrawItemEventHandler(combUpdateComponents_DrawItem);

            bindTemplatesUpdates();
            bindComponentsUpdates();

        }

        #region 重绘ComboBox的高度

        /// <summary>
        /// combUpdateTemplates
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void combUpdateTemplates_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0)
            {
                return;
            }
            e.DrawBackground();
            e.DrawFocusRectangle();
            e.Graphics.DrawString(combUpdateTemplates.GetItemText(combUpdateTemplates.Items[e.Index]).ToString(), e.Font, new SolidBrush(e.ForeColor), e.Bounds.X, e.Bounds.Y + 3);
        }

        /// <summary>
        /// combUpdateComponents
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void combUpdateComponents_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0)
            {
                return;
            }
            e.DrawBackground();
            e.DrawFocusRectangle();
            e.Graphics.DrawString(combUpdateComponents.GetItemText(combUpdateComponents.Items[e.Index]).ToString(), e.Font, new SolidBrush(e.ForeColor), e.Bounds.X, e.Bounds.Y + 3);
        }

        #endregion

        #region 初始化模板及组件信息

        /// <summary>
        /// 更新模板组件信息
        /// </summary>
        void updateTemplateComponentInfo()
        {
            dbTemplateSkins = service.GetTemplatesSkins();
            dbComponentStyles = service.GetComponentsStyles();

            int tempCount = dbTemplateSkins.Keys.Count();
            int tempSkinCount = dbTemplateSkins.Values.Sum(p => p.Count());

            int compCount = dbComponentStyles.Keys.Count();
            int compStyleCount = dbComponentStyles.Values.Sum(p => p.Count());

            this.lbComponents.Text = compCount.ToString();
            this.lbStyles.Text = compStyleCount.ToString();
            this.lbTemplates.Text = tempCount.ToString();
            this.lbSkins.Text = tempSkinCount.ToString();
        }

        #endregion

        #region 检测组件更新

        /// <summary>
        /// 获取本地模板及对应模板皮肤键值集合
        /// </summary>
        /// <returns></returns>
        List<TemplateModel> GetLocationTemplatesAtSkins()
        {
            List<TemplateModel> list = new List<TemplateModel>();

            //本地组件库根目录
            string root = Path.Combine(AppContext.BaseDirectory, TEMPLATES_ROOT);

            string[] templatesDirs = Directory.GetDirectories(root);

            if (null != templatesDirs)
            {
                //循环模板
                foreach (var dir in templatesDirs)
                {
                    //模板信息
                    var fileTempConfig = Path.Combine(dir, TEMPLATE_FILE_CONFIG);
                    var tempConfig = FileReader.Read<TemplateConfig>(fileTempConfig, encoding);

                    //模板HTML
                    var fileTempHtml = Path.Combine(dir, TEMPLATE_FILE_HTML);
                    var tempHtml = FileReader.ReadToString(fileTempHtml, encoding);

                    //模板组件
                    var fileTempComponents = Path.Combine(dir, TEMPLATE_FILE_COMPONENTS);
                    var tempComponents = FileReader.ReadToList<TemplateComponent>(fileTempComponents, encoding);

                    //模板编号
                    string templateCode = dir.Substring(dir.LastIndexOf('\\') + 1);

                    //模板皮肤
                    List<TemplateSkin> skins = null;

                    //向下寻找模板皮肤
                    string skinRoot = Path.Combine(dir, TEMPLATE_SKIN_FOLDERNAME);

                    string[] skinsDirs = Directory.GetDirectories(skinRoot);

                    if (null != skinsDirs)
                    {
                        skins = new List<TemplateSkin>();

                        foreach (var skinDir in skinsDirs)
                        {
                            //皮肤信息
                            var fileSkinConfig = Path.Combine(skinDir, TEMPLATE_FILE_SKIN_CONFIG);
                            var skin = FileReader.Read<TemplateSkin>(fileSkinConfig, encoding);

                            //皮肤编号
                            skin.Code = skinDir.Substring(skinDir.LastIndexOf('\\') + 1);

                            skins.Add(skin);
                        }
                    }

                    if (skins != null && skins.Count > 0) list.Add(new TemplateModel
                    {
                        Code = templateCode,
                        HTML = tempHtml,
                        Config = tempConfig,
                        Components = tempComponents.ToArray(),
                        Skins = skins.ToArray()
                    });
                }
            }

            return list;
        }

        /// <summary>
        /// 获取本地组件及对应组件风格键值集合
        /// </summary>
        /// <returns></returns>
        List<ComponentModel> GetLocationComponentsAtStyles()
        {
            List<ComponentModel> list = new List<ComponentModel>();

            //本地组件库根目录
            string root = Path.Combine(AppContext.BaseDirectory, COMPONENTS_ROOT);

            string[] componentsDirs = Directory.GetDirectories(root);

            if (null != componentsDirs)
            {
                //循环组件
                foreach (var dir in componentsDirs)
                {
                    //组件信息
                    var fileCompConfig = Path.Combine(dir, COMPONENT_FILE_CONFIG);
                    var comp = FileReader.Read<ComponentModel>(fileCompConfig, encoding);

                    //组件HTML
                    var fileCompHtml = Path.Combine(dir, COMPONENT_FILE_HTML);
                    comp.HTML = FileReader.ReadToString(fileCompHtml, encoding);

                    //组件默认风格信息
                    var fileCompDefStyle = Path.Combine(dir, COMPONENT_FILE_DEFAULTSTYLE);
                    comp.DefaultStyle = FileReader.Read<ComponentDefaultStyle>(fileCompDefStyle, encoding);

                    //组件编号
                    comp.Code = dir.Substring(dir.LastIndexOf('\\') + 1);

                    //组件风格
                    List<ComponentStyle> styles = null;

                    //向下寻找组件风格
                    string skinRoot = Path.Combine(dir, COMPONENT_STYLE_FOLDERNAME);

                    string[] stylesDirs = Directory.GetDirectories(skinRoot);

                    if (null != stylesDirs)
                    {
                        styles = new List<ComponentStyle>();
                        foreach (var styleDir in stylesDirs)
                        {
                            //风格信息
                            var fileStyleConfig = Path.Combine(styleDir, COMPONENT_FILE_STYLE_CONFIG);
                            var style = FileReader.Read<ComponentStyle>(fileStyleConfig, encoding);

                            //风格样式
                            var fileCss = Path.Combine(styleDir, COMPONENT_FILE_STYLE_CSS);
                            style.Css = FileReader.ReadToString(fileCss, encoding);

                            //风格规则
                            var fileRules = Path.Combine(styleDir, COMPONENT_FILE_STYLE_Rules);
                            style.Rules = FileReader.ReadToString(fileRules, encoding);

                            //风格编号
                            style.Code = styleDir.Substring(styleDir.LastIndexOf('\\') + 1);

                            styles.Add(style);
                        }
                    }

                    if (styles != null && styles.Count > 0)
                    {
                        comp.Styles = styles.ToArray();
                        list.Add(comp);
                    }
                }
            }

            return list;
        }

        /// <summary>
        /// 检测模板更新信息
        /// </summary>
        void checkTemplateUpdates()
        {
            udTemplateSkins = new List<TemplateModel>();

            var locTemplates = GetLocationTemplatesAtSkins();

            foreach (var t in locTemplates)
            {
                TemplateSkin[] skins = null;

                if (!dbTemplateSkins.ContainsKey(t.Code))
                {
                    skins = t.Skins;
                }
                else
                {
                    skins = t.Skins.Where(p => !dbTemplateSkins[t.Code].Contains(p.Code)).ToArray();
                }

                if (skins != null && skins.Length > 0)
                {
                    var upItem = t.Clone();
                    upItem.Skins = skins;
                    udTemplateSkins.Add(upItem);
                }
            }

            bindTemplatesUpdates();
        }

        /// <summary>
        /// 检测组件更新信息
        /// </summary>
        void checkComponentsUpdates()
        {
            udComponentStyles = new List<ComponentModel>();

            var locComponents = GetLocationComponentsAtStyles();

            foreach (var t in locComponents)
            {
                ComponentStyle[] styles = null;

                if (!dbComponentStyles.ContainsKey(t.Code))
                {
                    styles = t.Styles;
                }
                else
                {
                    styles = t.Styles.Where(p => !dbComponentStyles[t.Code].Contains(p.Code)).ToArray();
                }

                if (styles != null && styles.Length > 0)
                {
                    var upItem = t.Clone();
                    upItem.Styles = styles;
                    udComponentStyles.Add(upItem);
                }
            }

            bindComponentsUpdates();
        }

        /// <summary>
        /// 绑定可更新的模板
        /// </summary>
        void bindTemplatesUpdates()
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();

            if (udTemplateSkins == null || udTemplateSkins.Count < 1)
            {
                dic.Add("-1", "请选择");
            }
            else
            {
                dic.Add("0", "全部可更新模板");
                foreach (var item in udTemplateSkins)
                {
                    dic.Add(item.Code, $"{item.Config.Name}({item.Code})模板，共{item.Skins.Length}套皮肤");
                }
            }
            this.combUpdateTemplates.DataSource = null;
            this.combUpdateTemplates.Items.Clear();
            this.combUpdateTemplates.DataSource = new BindingSource(dic, null);
            this.combUpdateTemplates.DisplayMember = "value";
            this.combUpdateTemplates.ValueMember = "key";
        }

        /// <summary>
        /// 绑定可更新的组件
        /// </summary>
        void bindComponentsUpdates()
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            if (udComponentStyles == null || udComponentStyles.Count < 1)
            {
                dic.Add("-1", "请选择");
            }
            else
            {
                dic.Add("0", "全部可更新组件");
                foreach (var item in udComponentStyles)
                {
                    dic.Add(item.Code, $"{item.Name}({item.Code})组件，共{item.Styles.Length}套风格");
                }
            }
            this.combUpdateComponents.DataSource = null;
            this.combUpdateComponents.Items.Clear();
            this.combUpdateComponents.DataSource = new BindingSource(dic, null);
            this.combUpdateComponents.DisplayMember = "value";
            this.combUpdateComponents.ValueMember = "key";
        }

        #endregion

        #region 按钮事件

        /// <summary>
        /// 更新模板（入库） 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUpdateTemplates_Click(object sender, EventArgs e)
        {
            var tempCode = this.combUpdateTemplates.SelectedValue.ToString();

            if (tempCode == "-1")
            {
                MessageBox.Show("请选择需要更新的模板！");
                return;
            }

            List<TemplateModel> data = udTemplateSkins;

            if (tempCode != "0")
            {
                data = udTemplateSkins.Where(p => p.Code.Equals(tempCode, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            string err = null;
            bool success = false;
            try
            {
                success = service.AddTemplates(data);
            }
            catch (CustomException ex)
            {
                err = ex.Message;
            }
            catch (Exception ex)
            {
                err = $"程序异常，原因：{ex.Message}";
            }
            finally
            {
                if (success)
                {
                    var codes = data.Select(p => p.Code).ToArray();
                    udTemplateSkins = udTemplateSkins.Where(p => !codes.Contains(p.Code)).ToList();
                    bindTemplatesUpdates();
                }
                else
                {
                    err = "更新模板失败！";
                }
            }

            if (!string.IsNullOrWhiteSpace(err))
            {
                MessageBox.Show(err);
            }
            else
            {
                MessageBox.Show("更新模板成功！");
            }
        }

        /// <summary>
        /// 更新组件（入库）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUpdateComponents_Click(object sender, EventArgs e)
        {
            var compCode = this.combUpdateComponents.SelectedValue.ToString();

            if (compCode == "-1")
            {
                MessageBox.Show("请选择需要更新的组件！");
                return;
            }

            List<ComponentModel> data = udComponentStyles;

            if (compCode != "0")
            {
                data = udComponentStyles.Where(p => p.Code.Equals(compCode, StringComparison.OrdinalIgnoreCase)).ToList();
            }
            string err = null;
            bool success = false;

            try
            {
                success = service.AddComponents(data);
            }
            catch (CustomException ex)
            {
                err = ex.Message;
            }
            catch (Exception ex)
            {
                err = $"程序异常，原因：{ex.Message}";
            }
            finally
            {
                if (success)
                {
                    var codes = data.Select(p => p.Code).ToArray();
                    udComponentStyles = udComponentStyles.Where(p => !codes.Contains(p.Code)).ToList();
                    bindComponentsUpdates();
                }
                else
                {
                    err = "更新组件失败！";
                }
            }

            if (!string.IsNullOrWhiteSpace(err))
            {
                MessageBox.Show(err);
            }
            else
            {
                MessageBox.Show("更新组件成功！");
            }
        }

        /// <summary>
        /// 检测组件更新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCheckComponents_Click(object sender, EventArgs e)
        {
            checkComponentsUpdates();

            int compCount = udComponentStyles.Count();
            int compStyleCount = udComponentStyles.Sum(p => p.Styles.Count());

            bool hasUpdates = compCount > 0;

            string tips = hasUpdates ? $"有{compCount}个组件，{compStyleCount}套风格可以更新" : "已是最新！";

            this.lbComponentsUpdateTips.Text = tips;
        }

        /// <summary>
        /// 检测模板更新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCheckTemplates_Click(object sender, EventArgs e)
        {
            checkTemplateUpdates();

            int tempCount = udTemplateSkins.Count();
            int tempSkinCount = udTemplateSkins.Sum(p => p.Skins.Count());

            bool hasUpdates = tempCount > 0;

            string tips = hasUpdates ? $"有{tempCount}套模板，{tempSkinCount}套皮肤可以更新" : "已是最新！";

            this.lbTemplateUpdateTips.Text = tips;
        }

        #endregion
    }
}
