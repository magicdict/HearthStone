namespace 炉边传说
{
    partial class ctlWeapon
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
            this.lblAttackPoint = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblHealthPoint
            // 
            this.lblHealthPoint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblHealthPoint.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.lblHealthPoint.Location = new System.Drawing.Point(29, 35);
            this.lblHealthPoint.Name = "lblHealthPoint";
            this.lblHealthPoint.Size = new System.Drawing.Size(19, 13);
            this.lblHealthPoint.TabIndex = 10;
            this.lblHealthPoint.Text = "99";
            this.lblHealthPoint.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblAttackPoint
            // 
            this.lblAttackPoint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblAttackPoint.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.lblAttackPoint.Location = new System.Drawing.Point(-1, 35);
            this.lblAttackPoint.Name = "lblAttackPoint";
            this.lblAttackPoint.Size = new System.Drawing.Size(21, 13);
            this.lblAttackPoint.TabIndex = 9;
            this.lblAttackPoint.Text = "99";
            this.lblAttackPoint.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ctlWeapon
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Controls.Add(this.lblHealthPoint);
            this.Controls.Add(this.lblAttackPoint);
            this.Name = "ctlWeapon";
            this.Size = new System.Drawing.Size(48, 48);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblHealthPoint;
        private System.Windows.Forms.Label lblAttackPoint;
    }
}
