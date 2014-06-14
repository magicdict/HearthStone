namespace 炉边传说
{
    partial class ctlHandCard
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
            this.lblCostPoint = new System.Windows.Forms.Label();
            this.lblDescription = new System.Windows.Forms.Label();
            this.lblName = new System.Windows.Forms.Label();
            this.lblHealthPoint = new System.Windows.Forms.Label();
            this.lblAttackPoint = new System.Windows.Forms.Label();
            this.btnUse = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblCostPoint
            // 
            this.lblCostPoint.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.lblCostPoint.Location = new System.Drawing.Point(2, 0);
            this.lblCostPoint.Name = "lblCostPoint";
            this.lblCostPoint.Size = new System.Drawing.Size(21, 20);
            this.lblCostPoint.TabIndex = 2;
            this.lblCostPoint.Text = "99";
            this.lblCostPoint.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblDescription
            // 
            this.lblDescription.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblDescription.Location = new System.Drawing.Point(4, 20);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(79, 64);
            this.lblDescription.TabIndex = 4;
            this.lblDescription.Text = "Description";
            this.lblDescription.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // lblName
            // 
            this.lblName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.lblName.Location = new System.Drawing.Point(22, 0);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(66, 20);
            this.lblName.TabIndex = 5;
            this.lblName.Text = "Name";
            this.lblName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblHealthPoint
            // 
            this.lblHealthPoint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblHealthPoint.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.lblHealthPoint.Location = new System.Drawing.Point(65, 106);
            this.lblHealthPoint.Name = "lblHealthPoint";
            this.lblHealthPoint.Size = new System.Drawing.Size(19, 13);
            this.lblHealthPoint.TabIndex = 7;
            this.lblHealthPoint.Text = "99";
            this.lblHealthPoint.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblAttackPoint
            // 
            this.lblAttackPoint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblAttackPoint.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.lblAttackPoint.Location = new System.Drawing.Point(1, 106);
            this.lblAttackPoint.Name = "lblAttackPoint";
            this.lblAttackPoint.Size = new System.Drawing.Size(21, 13);
            this.lblAttackPoint.TabIndex = 6;
            this.lblAttackPoint.Text = "99";
            this.lblAttackPoint.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnUse
            // 
            this.btnUse.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnUse.Location = new System.Drawing.Point(21, 100);
            this.btnUse.Name = "btnUse";
            this.btnUse.Size = new System.Drawing.Size(43, 20);
            this.btnUse.TabIndex = 8;
            this.btnUse.Text = "使用";
            this.btnUse.UseVisualStyleBackColor = true;
            this.btnUse.Click += new System.EventHandler(this.btnUse_Click);
            // 
            // ctlHandCard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.btnUse);
            this.Controls.Add(this.lblHealthPoint);
            this.Controls.Add(this.lblAttackPoint);
            this.Controls.Add(this.lblName);
            this.Controls.Add(this.lblDescription);
            this.Controls.Add(this.lblCostPoint);
            this.Name = "ctlHandCard";
            this.Size = new System.Drawing.Size(87, 119);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblCostPoint;
        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.Label lblHealthPoint;
        private System.Windows.Forms.Label lblAttackPoint;
        private System.Windows.Forms.Button btnUse;
    }
}
