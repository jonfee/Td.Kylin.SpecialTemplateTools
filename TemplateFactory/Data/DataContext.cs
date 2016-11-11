using Microsoft.EntityFrameworkCore;
using System.Configuration;
using Td.Kylin.Entity;
using System;

namespace TemplateFactory.Data
{
    public partial class DataContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (string.IsNullOrEmpty(Connect.ConnectString))
                throw new ArgumentNullException(nameof(Connect.ConnectString));

            optionsBuilder.UseSqlServer(Connect.ConnectString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region 专题页

            //专题页模板
            modelBuilder.Entity<Special_Templates>(entity =>
            {
                entity.Property(p => p.TemplateId).ValueGeneratedNever();
                entity.HasKey(p => p.TemplateId);
            });

            //专题页模板皮肤
            modelBuilder.Entity<Special_TemplateSkin>(entity =>
            {
                entity.Property(p => p.SkinId).ValueGeneratedNever();
                entity.HasKey(p => p.SkinId);
            });

            //专题组件库
            modelBuilder.Entity<Special_Components>(entity =>
            {
                entity.Property(p => p.ComponentId).ValueGeneratedNever();
                entity.HasKey(p => p.ComponentId);
            });

            //组件风格
            modelBuilder.Entity<Special_ComponentStyle>(entity =>
            {
                entity.Property(p => p.StyleId).ValueGeneratedNever();
                entity.HasKey(p => p.StyleId);
            });

            //专题模板组件关联
            modelBuilder.Entity<Special_TemplateComponents>(entity =>
            {
                entity.Property(p => p.TemplateComponentId).ValueGeneratedNever();
                entity.HasKey(p => p.TemplateComponentId);
            });

            //专题页
            modelBuilder.Entity<Special_Page>(entity =>
            {
                entity.Property(p => p.PageId).ValueGeneratedNever();
                entity.HasKey(p => p.PageId);
            });

            //专题页组件
            modelBuilder.Entity<Special_PageComponents>(entity =>
            {
                entity.Property(p => p.PageComponentId).ValueGeneratedNever();
                entity.HasKey(p => p.PageComponentId);
            });

            #endregion
        }

        #region 专题页

        /// <summary>
        /// 专题页模板
        /// </summary>
        public DbSet<Special_Templates> Special_Templates { get { return Set<Special_Templates>(); } }

        /// <summary>
        /// 专题页模板皮肤
        /// </summary>
        public DbSet<Special_TemplateSkin> Special_TemplateSkin { get { return Set<Special_TemplateSkin>(); } }

        /// <summary>
        /// 专题组件库
        /// </summary>
        public DbSet<Special_Components> Special_Components { get { return Set<Special_Components>(); } }

        /// <summary>
        /// 组件风格
        /// </summary>
        public DbSet<Special_ComponentStyle> Special_ComponentStyle { get { return Set<Special_ComponentStyle>(); } }

        /// <summary>
        /// 专题模板组件关联
        /// </summary>
        public DbSet<Special_TemplateComponents> Special_TemplateComponents { get { return Set<Special_TemplateComponents>(); } }

        /// <summary>
        /// 专题页
        /// </summary>
        public DbSet<Special_Page> Special_Page { get { return Set<Special_Page>(); } }

        /// <summary>
        /// 专题页组件
        /// </summary>
        public DbSet<Special_PageComponents> Special_PageComponents { get { return Set<Special_PageComponents>(); } }

        #endregion
    }
}
