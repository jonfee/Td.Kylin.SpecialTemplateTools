using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Td.Kylin.Entity;
using TemplateFactory.Core;
using TemplateFactory.Model;

namespace TemplateFactory.Data
{
    /// <summary>
    /// 专题数据服务
    /// </summary>
    public class SpecialService
    {
        /// <summary>
        /// 获取组件及组件风格键值集合
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, string[]> GetComponentsStyles()
        {
            using (var db = new DataContext())
            {
                var query = from comp in db.Special_Components
                            join style in db.Special_ComponentStyle
                            on comp.ComponentId equals style.ComponentId
                            group style.Code by comp.Code
                          into g
                            select new
                            {
                                ComponentCode = g.Key,
                                Styles = g.ToArray()
                            };

                return query.ToDictionary(k => k.ComponentCode, v => v.Styles);
            }
        }

        /// <summary>
        /// 获取模板及模板皮肤键值集合
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, string[]> GetTemplatesSkins()
        {
            using (var db = new DataContext())
            {
                var query = from skin in db.Special_TemplateSkin
                            join temp in db.Special_Templates
                            on skin.TemplateId equals temp.TemplateId
                            group skin.Code by temp.Code
                          into g
                            select new
                            {
                                TempCode = g.Key,
                                Skins = g.ToArray()
                            };

                return query.ToDictionary(k => k.TempCode, v => v.Skins);
            }
        }

        /// <summary>
        /// 添加模板
        /// </summary>
        /// <param name="templates"></param>
        /// <returns></returns>
        public bool AddTemplates(IEnumerable<TemplateModel> templates)
        {
            if (templates == null) return false;

            List<Special_Templates> inTemplates = new List<Special_Templates>();
            List<Special_TemplateSkin> inSkins = new List<Special_TemplateSkin>();
            List<Special_TemplateComponents> inTempComponents = new List<Special_TemplateComponents>();

            using (var db = new DataContext())
            {
                #region 可入库模板

                //操作的模板ID集合
                long[] tempIds = templates.Select(t => t.Config.TemplateId).ToArray();

                //数据库中已存在的模板
                var dbTempIds = db.Special_Templates.Where(p => tempIds.Contains(p.TemplateId)).Select(p => p.TemplateId).ToArray();

                //需要入库的模板
                inTemplates = templates.Where(p => !dbTempIds.Contains(p.Config.TemplateId)).Select(p => new Special_Templates
                {
                    TemplateId = p.Config.TemplateId,
                    BuyCounts = 0,
                    Code = p.Code,
                    CreateTime = DateTime.Now,
                    DefaultSkinId = p.Config.DefaultSkinId,
                    IsEnabled = true,
                    IsFree = true,
                    Name = p.Config.Name,
                    PreviewImage = p.Config.PreviewImage,
                    Price = 0.00M,
                    TemplateHtml = p.HTML
                }).ToList();

                //最终入库的模板ID集合
                var upTempIds = inTemplates.Select(p => p.TemplateId).ToArray();

                #endregion

                //获取需要入库的皮肤及模板的组件
                foreach (var item in templates)
                {
                    #region 可入库皮肤
                    var upSkinIds = item.Skins.Select(p => p.SkinId).ToArray();
                    //数据库中已存在的皮肤
                    var dbSkinIds = db.Special_TemplateSkin.Where(p => upSkinIds.Contains(p.SkinId)).Select(p => p.SkinId).ToArray();

                    var skins = item.Skins.Where(p => !dbSkinIds.Contains(p.SkinId)).Select(p => new Special_TemplateSkin
                    {
                        Code = p.Code,
                        CreateTime = DateTime.Now,
                        Name = p.Name,
                        PreviewImage = p.PreviewImage,
                        SkinId = p.SkinId,
                        TemplateId = item.Config.TemplateId,
                        UseCounts = 0
                    }).ToList();

                    if (skins.Any()) inSkins.AddRange(skins);
                    #endregion

                    #region 可入库模板组件

                    if (upTempIds.Contains(item.Config.TemplateId))
                    {
                        var components = item.Components.Select(p => new Special_TemplateComponents
                        {
                            ComponentId = p.ComponentId,
                            ComponentStyleId = p.StyleId,
                            Index = p.Index,
                            PreviewImage = null,
                            TemplateComponentId = p.TempComponentId,
                            TemplateId = item.Config.TemplateId
                        }).ToList();

                        if (components.Any()) inTempComponents.AddRange(components);
                    }

                    #endregion
                }

                #region //更新模板组件中的风格预览图
                var styleIds = inTempComponents.Select(p => p.ComponentStyleId).Distinct().ToArray();
                var styleDic = db.Special_ComponentStyle.Where(p => styleIds.Contains(p.StyleId)).Select(p => new { StyleId = p.StyleId, Image = p.PreviewImage }).ToDictionary(k => k.StyleId, v => v.Image);
                foreach (var item in inTempComponents)
                {
                    if (string.IsNullOrWhiteSpace(item.PreviewImage))
                    {
                        var img = styleDic.ContainsKey(item.ComponentStyleId) ? styleDic[item.ComponentStyleId] : null;

                        if (string.IsNullOrWhiteSpace(img)) throw new CustomException("请先确定当前模板中的组件均已入库且数据完整。");

                        item.PreviewImage = img;
                    }
                }
                #endregion

                //模板入库
                if (inTemplates.Any()) db.Special_Templates.AddRange(inTemplates);
                //皮肤入库
                if (inSkins.Any()) db.Special_TemplateSkin.AddRange(inSkins);
                //模板组件入库
                if (inTempComponents.Any()) db.Special_TemplateComponents.AddRange(inTempComponents);

                return db.SaveChanges() > 0;
            }
        }

        /// <summary>
        /// 添加组件
        /// </summary>
        /// <param name="components"></param>
        /// <returns></returns>
        public bool AddComponents(IEnumerable<ComponentModel> components)
        {
            if (components == null) return false;

            List<Special_Components> inComponents = new List<Special_Components>();
            List<Special_ComponentStyle> inStyles = new List<Special_ComponentStyle>();

            //当前组件ID集合
            long[] compIds = components.Select(p => p.ComponentId).ToArray();
            //当前所有组件的风格ID集合
            long[] styleIds = null;

            List<long> sl = new List<long>();
            foreach (var item in components)
            {
                var tl = item.Styles.Select(p => p.StyleId).ToArray();
                if (tl != null && tl.Count() > 0) sl.AddRange(tl);
            }
            styleIds = sl.ToArray();

            using (var db = new DataContext())
            {
                //数据库中已有组件
                var dbCompIds = db.Special_Components.Where(p => compIds.Contains(p.ComponentId)).Select(p => p.ComponentId).ToArray();
                //数据库中已有风格
                var dbStyleIds = db.Special_ComponentStyle.Where(p => styleIds.Contains(p.StyleId)).Select(p => p.StyleId).ToArray();

                foreach (var item in components)
                {
                    #region //可入库的组件，数据库中不存在即可入库
                    if (!dbCompIds.Contains(item.ComponentId))
                    {
                        inComponents.Add(new Special_Components
                        {
                            ComponentId = item.ComponentId,
                            Code = item.Code,
                            ComponentType = item.ComponentType,
                            Config = item.DefaultStyle.DefaultStyleConfig,
                            CreateTime = DateTime.Now,
                            DefaultData = item.DefaultData,
                            DefaultStyleId = item.DefaultStyle.DefaultStyleId,
                            Name = item.Name,
                            PreviewImage = item.DefaultStyle.PreviewImage,
                            TemplateHtml = item.HTML
                        });
                    }
                    #endregion

                    #region //可入库的风格

                    var styles = item.Styles.Where(p => !dbStyleIds.Contains(p.StyleId)).Select(p => new Special_ComponentStyle
                    {
                        Code = p.Code,
                        ComponentId = item.ComponentId,
                        CreateTime = DateTime.Now,
                        Name = p.Name,
                        PreviewImage = p.PreviewImage,
                        Rules = p.Rules,
                        StyleId = p.StyleId,
                        StyleText = p.Css
                    });

                    if (styles.Any()) inStyles.AddRange(styles);
                    #endregion
                }

                //组件入库
                if (inComponents.Any()) db.Special_Components.AddRange(inComponents);
                //风格入库
                if (inStyles.Any()) db.Special_ComponentStyle.AddRange(inStyles);

                return db.SaveChanges() > 0;
            }
        }
    }
}
