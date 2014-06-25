namespace 炉边传说
{
    partial class ctlHeroAbility
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

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.lblEnable = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblEnable
            // 
            this.lblEnable.AutoSize = true;
            this.lblEnable.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.lblEnable.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblEnable.Location = new System.Drawing.Point(0, 0);
            this.lblEnable.Name = "lblEnable";
            this.lblEnable.Size = new System.Drawing.Size(46, 13);
            this.lblEnable.TabIndex = 0;
            this.lblEnable.Text = "             ";
            // 
            // ctlHeroAbility
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Controls.Add(this.lblEnable);
            this.Name = "ctlHeroAbility";
            this.Size = new System.Drawing.Size(48, 48);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblEnable;
    }
}
