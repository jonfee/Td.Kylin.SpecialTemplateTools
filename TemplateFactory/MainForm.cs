using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
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

        Dictionary<string, string[]> dbTemplateSkins = null;    //库中模板及皮肤
        Dictionary<string, string[]> dbComponentStyles = null;  //库中组件及风格

        List<TemplateModel> localTemplateSkins = null;      //本地模板及皮肤
        List<ComponentModel> localComponentStyles = null;   //本地组件及风格

        List<TemplateModel> canInsertTemplateSkins = null;      //可入库的模板及皮肤
        List<ComponentModel> canInsertComponentStyles = null;   //可入库的组件及风格

        List<TemplateModel> canUpdateTemplateSkins = null;      //可更新的模板及皮肤
        List<ComponentModel> canUpdateComponentStyles = null;   //可更新的组件及风格

        SpecialService service = null;  //数据操作服务实例

        #endregion

        public MainForm()
        {
            InitializeComponent();

            this.combDbLibs.DrawMode = DrawMode.OwnerDrawVariable;
            this.combInsertTemplates.DrawMode = DrawMode.OwnerDrawVariable;
            this.combInsertComponents.DrawMode = DrawMode.OwnerDrawVariable;
            this.combUpdateTemplates.DrawMode = DrawMode.OwnerDrawVariable;
            this.combUpdateComponents.DrawMode = DrawMode.OwnerDrawVariable;
            this.combDbLibs.DrawItem += new DrawItemEventHandler(comboBox_DrawItem);
            this.combInsertTemplates.DrawItem += new DrawItemEventHandler(comboBox_DrawItem);
            this.combInsertComponents.DrawItem += new DrawItemEventHandler(comboBox_DrawItem);
            this.combUpdateTemplates.DrawItem += new DrawItemEventHandler(comboBox_DrawItem);
            this.combUpdateComponents.DrawItem += new DrawItemEventHandler(comboBox_DrawItem);

            this.btnCheckUpdate.Visible = false;

            bindDbLib();
        }

        #region 重绘ComboBox的高度

        /// <summary>
        /// comboBox_DrawItem
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void comboBox_DrawItem(object sender, DrawItemEventArgs e)
        {
            ComboBox combo = (ComboBox)sender;

            if (combo == null) return;

            if (e.Index < 0)
            {
                return;
            }
            e.DrawBackground();
            e.DrawFocusRectangle();
            e.Graphics.DrawString(combo.GetItemText(combo.Items[e.Index]).ToString(), e.Font, new SolidBrush(e.ForeColor), e.Bounds.X, e.Bounds.Y + 3);
        }

        #endregion

        /// <summary>
        /// 重绘窗体
        /// </summary>
        void formDraw()
        {
            checkData();
            checkUI();
        }

        #region 绑定UI

        /// <summary>
        /// 绑定目标库
        /// </summary>
        void bindDbLib()
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("0", "请选择");

            foreach (ConnectionStringSettings item in ConfigurationManager.ConnectionStrings)
            {
                if (item.Name == "LocalSqlServer") continue;
                dic.Add(item.ConnectionString, item.Name);
            }
            this.combDbLibs.DataSource = new BindingSource(dic, null);
            this.combDbLibs.DisplayMember = "value";
            this.combDbLibs.ValueMember = "key";
            this.combDbLibs.SelectedIndex = 0;
        }

        /// <summary>
        /// 校正UI展示
        /// </summary>
        void checkUI()
        {
            bindCurrentInfo();
            bindTemplatesInsert();
            bindComponentsInsert();
            bindTemplatesUpdate();
            bindComponentsUpdate();
        }

        /// <summary>
        /// 绑定当前库中模板及组件信息
        /// </summary>
        void bindCurrentInfo()
        {
            //当前库中模板及组件信息
            int compCount = dbComponentStyles.Keys.Count();
            int compStyleCount = dbComponentStyles.Values.Sum(p => p.Count());
            this.lbComponents.Text = compCount.ToString();
            this.lbStyles.Text = compStyleCount.ToString();

            int tempCount = dbTemplateSkins.Keys.Count();
            int tempSkinCount = dbTemplateSkins.Values.Sum(p => p.Count());
            this.lbTemplates.Text = tempCount.ToString();
            this.lbSkins.Text = tempSkinCount.ToString();

            //更新消息
            int incompCount = canInsertComponentStyles.Count();
            int incompStyleCount = canInsertComponentStyles.Sum(p => p.Styles.Count());
            string inCompTips = incompCount > 0 ? $"有{incompCount}个组件，{incompStyleCount}套风格可以更新入库" : "已是最新！";
            this.lbComponentsUpdateTips.Text = inCompTips;

            int intempCount = canInsertTemplateSkins.Count();
            int intempSkinCount = canInsertTemplateSkins.Sum(p => p.Skins.Count());
            string inTempTips = intempCount > 0 ? $"有{intempCount}套模板，{intempSkinCount}套皮肤可以更新入库" : "已是最新！";
            this.lbTemplateUpdateTips.Text = inTempTips;
        }

        /// <summary>
        /// 绑定可入库的模板
        /// </summary>
        void bindTemplatesInsert()
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();

            if (canInsertTemplateSkins == null || canInsertTemplateSkins.Count < 1)
            {
                dic.Add("-1", "请选择");
            }
            else
            {
                dic.Add("0", "全部可入库模板");
                foreach (var item in canInsertTemplateSkins)
                {
                    dic.Add(item.Code, $"{item.Config.Name}({item.Code})模板，共{item.Skins.Length}套皮肤");
                }
            }
            this.combInsertTemplates.DataSource = null;
            this.combInsertTemplates.Items.Clear();
            this.combInsertTemplates.DataSource = new BindingSource(dic, null);
            this.combInsertTemplates.DisplayMember = "value";
            this.combInsertTemplates.ValueMember = "key";
        }

        /// <summary>
        /// 绑定可入库的组件
        /// </summary>
        void bindComponentsInsert()
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            if (canInsertComponentStyles == null || canInsertComponentStyles.Count < 1)
            {
                dic.Add("-1", "请选择");
            }
            else
            {
                dic.Add("0", "全部可更新组件");
                foreach (var item in canInsertComponentStyles)
                {
                    dic.Add(item.Code, $"{item.Name}({item.Code})组件，共{item.Styles.Length}套风格");
                }
            }
            this.combInsertComponents.DataSource = null;
            this.combInsertComponents.Items.Clear();
            this.combInsertComponents.DataSource = new BindingSource(dic, null);
            this.combInsertComponents.DisplayMember = "value";
            this.combInsertComponents.ValueMember = "key";
        }

        /// <summary>
        /// 绑定可更新的模板
        /// </summary>
        void bindTemplatesUpdate()
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();

            if (canUpdateTemplateSkins == null || canUpdateTemplateSkins.Count < 1)
            {
                dic.Add("-1", "请选择");
            }
            else
            {
                dic.Add("0", "全部可更新模板");
                foreach (var item in canUpdateTemplateSkins)
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
        void bindComponentsUpdate()
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            if (canUpdateComponentStyles == null || canUpdateComponentStyles.Count < 1)
            {
                dic.Add("-1", "请选择");
            }
            else
            {
                dic.Add("0", "全部可更新组件");
                foreach (var item in canUpdateComponentStyles)
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

        #region 模板、组件数据处理

        /// <summary>
        /// 校正数据
        /// </summary>
        void checkData()
        {
            service = service ?? new SpecialService();

            //获取库中的模板及皮肤
            dbTemplateSkins = service.GetTemplatesSkins();
            //获取库中的组件及风格
            dbComponentStyles = service.GetComponentsStyles();

            //获取本地所有模板及皮肤
            localTemplateSkins = GetLocationTemplatesAtSkins();
            //获取本地所有组件及风格
            localComponentStyles = GetLocationComponentsAtStyles();

            resolveTemplates();
            resolveComponents();
        }

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
        /// 分解出可新增和可更新的模板及皮肤
        /// </summary>
        void resolveTemplates()
        {
            canInsertTemplateSkins = new List<TemplateModel>();
            canUpdateTemplateSkins = new List<TemplateModel>();

            foreach (var t in localTemplateSkins)
            {
                TemplateSkin[] insertSkins = null;
                TemplateSkin[] updateSkins = null;

                if (!dbTemplateSkins.ContainsKey(t.Code))
                {
                    insertSkins = t.Skins;
                }
                else
                {
                    insertSkins = t.Skins.Where(p => !dbTemplateSkins[t.Code].Contains(p.Code)).ToArray();//库中没有的可新增
                    updateSkins = t.Skins.Where(p => dbTemplateSkins[t.Code].Contains(p.Code)).ToArray();//库中有的可更新
                }

                if (insertSkins != null && insertSkins.Length > 0)
                {
                    var inItem = t.Clone();
                    inItem.Skins = insertSkins;
                    canInsertTemplateSkins.Add(inItem);
                }

                if (updateSkins != null && updateSkins.Length > 0)
                {
                    var upItem = t.Clone();
                    upItem.Skins = updateSkins;
                    canUpdateTemplateSkins.Add(upItem);
                }
            }
        }

        /// <summary>
        /// 分解出可新增和可更新的组件及风格
        /// </summary>
        void resolveComponents()
        {
            canInsertComponentStyles = new List<ComponentModel>();
            canUpdateComponentStyles = new List<ComponentModel>();

            foreach (var t in localComponentStyles)
            {
                ComponentStyle[] insertStyles = null;
                ComponentStyle[] updateStyles = null;

                if (!dbComponentStyles.ContainsKey(t.Code))
                {
                    insertStyles = t.Styles;
                }
                else
                {
                    insertStyles = t.Styles.Where(p => !dbComponentStyles[t.Code].Contains(p.Code)).ToArray();//库中没有的可新增
                    updateStyles = t.Styles.Where(p => dbComponentStyles[t.Code].Contains(p.Code)).ToArray();//库中有的可更新
                }

                if (insertStyles != null && insertStyles.Length > 0)
                {
                    var inItem = t.Clone();
                    inItem.Styles = insertStyles;
                    canInsertComponentStyles.Add(inItem);
                }
                if (updateStyles != null && updateStyles.Length > 0)
                {
                    var upItem = t.Clone();
                    upItem.Styles = updateStyles;
                    canUpdateComponentStyles.Add(upItem);
                }
            }

            bindComponentsInsert();
        }

        #endregion

        #region 按钮事件

        /// <summary>
        /// 模板入库
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnInsertTemplates_Click(object sender, EventArgs e)
        {
            var tempCode = (this.combInsertTemplates.SelectedValue ?? "-1").ToString();

            if (tempCode == "-1")
            {
                MessageBox.Show("请选择需要入库的模板！");
                return;
            }

            List<TemplateModel> data = canInsertTemplateSkins;

            if (tempCode != "0")
            {
                data = canInsertTemplateSkins.Where(p => p.Code.Equals(tempCode, StringComparison.OrdinalIgnoreCase)).ToList();
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
                    formDraw();
                }
                else
                {
                    err = "添加模板失败！";
                }
            }

            if (!string.IsNullOrWhiteSpace(err))
            {
                MessageBox.Show(err);
            }
            else
            {
                MessageBox.Show("添加模板成功！");
            }
        }

        /// <summary>
        /// 组件入库
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnInsertComponents_Click(object sender, EventArgs e)
        {
            var compCode = (this.combInsertComponents.SelectedValue ?? "-1").ToString();

            if (compCode == "-1")
            {
                MessageBox.Show("请选择需要添加的组件！");
                return;
            }

            List<ComponentModel> data = canInsertComponentStyles;

            if (compCode != "0")
            {
                data = canInsertComponentStyles.Where(p => p.Code.Equals(compCode, StringComparison.OrdinalIgnoreCase)).ToList();
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
                    formDraw();
                }
                else
                {
                    err = "添加组件失败！";
                }
            }

            if (!string.IsNullOrWhiteSpace(err))
            {
                MessageBox.Show(err);
            }
            else
            {
                MessageBox.Show("添加组件成功！");
            }
        }

        /// <summary>
        /// 模板更新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUpdateTemplates_Click(object sender, EventArgs e)
        {
            var tempCode = (this.combUpdateTemplates.SelectedValue ?? "-1").ToString();

            if (tempCode == "-1")
            {
                MessageBox.Show("请选择需要更新的模板！");
                return;
            }

            List<TemplateModel> data = canUpdateTemplateSkins;

            if (tempCode != "0")
            {
                data = canUpdateTemplateSkins.Where(p => p.Code.Equals(tempCode, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            string err = null;
            int rows = 0;
            try
            {
                rows = service.UpdateTemplates(data);
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
                if (rows > 0)
                {
                    formDraw();
                }
                else if (rows == 0)
                {
                    err = "模板库中已是最新！";
                }
                else if (string.IsNullOrWhiteSpace(err))
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
        /// 组件更新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUpdateComponents_Click(object sender, EventArgs e)
        {
            var compCode = (this.combUpdateComponents.SelectedValue ?? "-1").ToString();

            if (compCode == "-1")
            {
                MessageBox.Show("请选择需要更新的组件！");
                return;
            }

            List<ComponentModel> data = canUpdateComponentStyles;

            if (compCode != "0")
            {
                data = canUpdateComponentStyles.Where(p => p.Code.Equals(compCode, StringComparison.OrdinalIgnoreCase)).ToList();
            }
            string err = null;
            int rows = 0;

            try
            {
                rows = service.UpdateComponents(data);
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
                if (rows > 0)
                {
                    formDraw();
                }
                else if (rows == 0)
                {
                    err = "组件库中已是最新！";
                }
                else if (string.IsNullOrWhiteSpace(err))
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
        /// 检测更新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCheckUpdate_Click(object sender, EventArgs e)
        {
            formDraw();
        }

        #endregion

        #region 目标库选择
        /// <summary>
        /// 目标库选择
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void combDbLibs_SelectedValueChanged(object sender, EventArgs e)
        {
            var connectString = ((KeyValuePair<string, string>)this.combDbLibs.SelectedItem).Key;

            if (connectString != "0")
            {
                this.btnCheckUpdate.Visible = true;

                Connect.ConnectString = connectString;

                formDraw();
            }
            else
            {
                this.btnCheckUpdate.Visible = false;
            }
        }
        #endregion
    }
}
