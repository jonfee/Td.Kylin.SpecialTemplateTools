namespace TemplateFactory
{
    partial class MainForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnUpdateTemplates = new System.Windows.Forms.Button();
            this.combUpdateTemplates = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnCheckTemplates = new System.Windows.Forms.Button();
            this.btnCheckComponents = new System.Windows.Forms.Button();
            this.lbSkins = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.lbTemplates = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.lbTemplateUpdateTips = new System.Windows.Forms.Label();
            this.lbComponentsUpdateTips = new System.Windows.Forms.Label();
            this.lbStyles = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lbComponents = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnUpdateComponents = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.combUpdateComponents = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnUpdateTemplates);
            this.groupBox1.Controls.Add(this.combUpdateTemplates);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(13, 217);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(574, 76);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "更新模板";
            // 
            // btnUpdateTemplates
            // 
            this.btnUpdateTemplates.Location = new System.Drawing.Point(483, 31);
            this.btnUpdateTemplates.Name = "btnUpdateTemplates";
            this.btnUpdateTemplates.Size = new System.Drawing.Size(75, 23);
            this.btnUpdateTemplates.TabIndex = 2;
            this.btnUpdateTemplates.Text = "入库";
            this.btnUpdateTemplates.UseVisualStyleBackColor = true;
            this.btnUpdateTemplates.Click += new System.EventHandler(this.btnUpdateTemplates_Click);
            // 
            // combUpdateTemplates
            // 
            this.combUpdateTemplates.FormattingEnabled = true;
            this.combUpdateTemplates.Location = new System.Drawing.Point(89, 33);
            this.combUpdateTemplates.Name = "combUpdateTemplates";
            this.combUpdateTemplates.Size = new System.Drawing.Size(317, 20);
            this.combUpdateTemplates.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(18, 37);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "选择模板：";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnCheckTemplates);
            this.groupBox2.Controls.Add(this.btnCheckComponents);
            this.groupBox2.Controls.Add(this.lbSkins);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.lbTemplates);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.lbTemplateUpdateTips);
            this.groupBox2.Controls.Add(this.lbComponentsUpdateTips);
            this.groupBox2.Controls.Add(this.lbStyles);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.lbComponents);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Location = new System.Drawing.Point(13, 13);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(574, 100);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "组件";
            // 
            // btnCheckTemplates
            // 
            this.btnCheckTemplates.Location = new System.Drawing.Point(483, 58);
            this.btnCheckTemplates.Name = "btnCheckTemplates";
            this.btnCheckTemplates.Size = new System.Drawing.Size(75, 23);
            this.btnCheckTemplates.TabIndex = 12;
            this.btnCheckTemplates.Text = "检测更新";
            this.btnCheckTemplates.UseVisualStyleBackColor = true;
            this.btnCheckTemplates.Click += new System.EventHandler(this.btnCheckTemplates_Click);
            // 
            // btnCheckComponents
            // 
            this.btnCheckComponents.Location = new System.Drawing.Point(483, 27);
            this.btnCheckComponents.Name = "btnCheckComponents";
            this.btnCheckComponents.Size = new System.Drawing.Size(75, 23);
            this.btnCheckComponents.TabIndex = 11;
            this.btnCheckComponents.Text = "检测更新";
            this.btnCheckComponents.UseVisualStyleBackColor = true;
            this.btnCheckComponents.Click += new System.EventHandler(this.btnCheckComponents_Click);
            // 
            // lbSkins
            // 
            this.lbSkins.AutoSize = true;
            this.lbSkins.Location = new System.Drawing.Point(170, 63);
            this.lbSkins.Name = "lbSkins";
            this.lbSkins.Size = new System.Drawing.Size(11, 12);
            this.lbSkins.TabIndex = 10;
            this.lbSkins.Text = "0";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(127, 63);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(41, 12);
            this.label6.TabIndex = 9;
            this.label6.Text = "皮肤：";
            // 
            // lbTemplates
            // 
            this.lbTemplates.AutoSize = true;
            this.lbTemplates.Location = new System.Drawing.Point(68, 63);
            this.lbTemplates.Name = "lbTemplates";
            this.lbTemplates.Size = new System.Drawing.Size(11, 12);
            this.lbTemplates.TabIndex = 8;
            this.lbTemplates.Text = "0";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(15, 63);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 12);
            this.label5.TabIndex = 7;
            this.label5.Text = "模板数：";
            // 
            // lbTemplateUpdateTips
            // 
            this.lbTemplateUpdateTips.AutoSize = true;
            this.lbTemplateUpdateTips.ForeColor = System.Drawing.Color.Blue;
            this.lbTemplateUpdateTips.Location = new System.Drawing.Point(240, 63);
            this.lbTemplateUpdateTips.Name = "lbTemplateUpdateTips";
            this.lbTemplateUpdateTips.Size = new System.Drawing.Size(0, 12);
            this.lbTemplateUpdateTips.TabIndex = 6;
            // 
            // lbComponentsUpdateTips
            // 
            this.lbComponentsUpdateTips.AutoSize = true;
            this.lbComponentsUpdateTips.ForeColor = System.Drawing.Color.Blue;
            this.lbComponentsUpdateTips.Location = new System.Drawing.Point(240, 32);
            this.lbComponentsUpdateTips.Name = "lbComponentsUpdateTips";
            this.lbComponentsUpdateTips.Size = new System.Drawing.Size(0, 12);
            this.lbComponentsUpdateTips.TabIndex = 6;
            // 
            // lbStyles
            // 
            this.lbStyles.AutoSize = true;
            this.lbStyles.Location = new System.Drawing.Point(170, 32);
            this.lbStyles.Name = "lbStyles";
            this.lbStyles.Size = new System.Drawing.Size(11, 12);
            this.lbStyles.TabIndex = 5;
            this.lbStyles.Text = "0";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(127, 32);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 12);
            this.label3.TabIndex = 4;
            this.label3.Text = "风格：";
            // 
            // lbComponents
            // 
            this.lbComponents.AutoSize = true;
            this.lbComponents.Location = new System.Drawing.Point(68, 32);
            this.lbComponents.Name = "lbComponents";
            this.lbComponents.Size = new System.Drawing.Size(11, 12);
            this.lbComponents.TabIndex = 2;
            this.lbComponents.Text = "0";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 32);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "组件数：";
            // 
            // btnUpdateComponents
            // 
            this.btnUpdateComponents.Location = new System.Drawing.Point(483, 29);
            this.btnUpdateComponents.Name = "btnUpdateComponents";
            this.btnUpdateComponents.Size = new System.Drawing.Size(75, 23);
            this.btnUpdateComponents.TabIndex = 3;
            this.btnUpdateComponents.Text = "入库";
            this.btnUpdateComponents.UseVisualStyleBackColor = true;
            this.btnUpdateComponents.Click += new System.EventHandler(this.btnUpdateComponents_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.combUpdateComponents);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Controls.Add(this.btnUpdateComponents);
            this.groupBox3.Location = new System.Drawing.Point(13, 129);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(574, 71);
            this.groupBox3.TabIndex = 3;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "更新组件";
            // 
            // combUpdateComponents
            // 
            this.combUpdateComponents.FormattingEnabled = true;
            this.combUpdateComponents.Location = new System.Drawing.Point(89, 30);
            this.combUpdateComponents.Name = "combUpdateComponents";
            this.combUpdateComponents.Size = new System.Drawing.Size(317, 20);
            this.combUpdateComponents.TabIndex = 4;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(18, 34);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 12);
            this.label4.TabIndex = 0;
            this.label4.Text = "选择组件：";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(599, 305);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "MainForm";
            this.Text = "专题模板工具";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox combUpdateTemplates;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnUpdateTemplates;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label lbComponents;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnUpdateComponents;
        private System.Windows.Forms.Label lbStyles;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lbComponentsUpdateTips;
        private System.Windows.Forms.Label lbSkins;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label lbTemplates;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lbTemplateUpdateTips;
        private System.Windows.Forms.Button btnCheckComponents;
        private System.Windows.Forms.Button btnCheckTemplates;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox combUpdateComponents;
    }
}

