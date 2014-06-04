namespace 炉边传说
{
    partial class ctlHero
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ctlHero));
            this.lblHealthPoint = new System.Windows.Forms.Label();
            this.lblShieldPoint = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblHealthPoint
            // 
            this.lblHealthPoint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblHealthPoint.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.lblHealthPoint.Location = new System.Drawing.Point(97, 103);
            this.lblHealthPoint.Name = "lblHealthPoint";
            this.lblHealthPoint.Size = new System.Drawing.Size(29, 23);
            this.lblHealthPoint.TabIndex = 2;
            this.lblHealthPoint.Text = "99";
            this.lblHealthPoint.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblShieldPoint
            // 
            this.lblShieldPoint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblShieldPoint.BackColor = System.Drawing.Color.Gainsboro;
            this.lblShieldPoint.Location = new System.Drawing.Point(97, 80);
            this.lblShieldPoint.Name = "lblShieldPoint";
            this.lblShieldPoint.Size = new System.Drawing.Size(29, 23);
            this.lblShieldPoint.TabIndex = 3;
            this.lblShieldPoint.Text = "99";
            this.lblShieldPoint.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ctlHero
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.lblShieldPoint);
            this.Controls.Add(this.lblHealthPoint);
            this.Name = "ctlHero";
            this.Size = new System.Drawing.Size(126, 126);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblHealthPoint;
        private System.Windows.Forms.Label lblShieldPoint;
    }
}
