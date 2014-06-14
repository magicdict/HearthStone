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
            this.lblHealthPoint = new System.Windows.Forms.Label();
            this.lblShieldPoint = new System.Windows.Forms.Label();
            this.lblSecret1 = new System.Windows.Forms.Label();
            this.lblSecret2 = new System.Windows.Forms.Label();
            this.lblSecret3 = new System.Windows.Forms.Label();
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
            // lblSecret1
            // 
            this.lblSecret1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.lblSecret1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSecret1.ForeColor = System.Drawing.Color.Yellow;
            this.lblSecret1.Location = new System.Drawing.Point(55, 0);
            this.lblSecret1.Name = "lblSecret1";
            this.lblSecret1.Size = new System.Drawing.Size(24, 24);
            this.lblSecret1.TabIndex = 4;
            this.lblSecret1.Text = "?";
            this.lblSecret1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblSecret2
            // 
            this.lblSecret2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.lblSecret2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSecret2.ForeColor = System.Drawing.Color.Yellow;
            this.lblSecret2.Location = new System.Drawing.Point(3, 46);
            this.lblSecret2.Name = "lblSecret2";
            this.lblSecret2.Size = new System.Drawing.Size(24, 24);
            this.lblSecret2.TabIndex = 5;
            this.lblSecret2.Text = "?";
            this.lblSecret2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblSecret3
            // 
            this.lblSecret3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.lblSecret3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSecret3.ForeColor = System.Drawing.Color.Yellow;
            this.lblSecret3.Location = new System.Drawing.Point(103, 46);
            this.lblSecret3.Name = "lblSecret3";
            this.lblSecret3.Size = new System.Drawing.Size(24, 24);
            this.lblSecret3.TabIndex = 6;
            this.lblSecret3.Text = "?";
            this.lblSecret3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ctlHero
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.lblSecret3);
            this.Controls.Add(this.lblSecret2);
            this.Controls.Add(this.lblSecret1);
            this.Controls.Add(this.lblShieldPoint);
            this.Controls.Add(this.lblHealthPoint);
            this.Name = "ctlHero";
            this.Size = new System.Drawing.Size(126, 126);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblHealthPoint;
        private System.Windows.Forms.Label lblShieldPoint;
        private System.Windows.Forms.Label lblSecret1;
        private System.Windows.Forms.Label lblSecret2;
        private System.Windows.Forms.Label lblSecret3;
    }
}
