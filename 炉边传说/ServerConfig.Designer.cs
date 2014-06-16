namespace 火炉服务器
{
    partial class ServerConfig
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lbl套牌牌数上限 = new System.Windows.Forms.Label();
            this.lbl英雄生命值上限 = new System.Windows.Forms.Label();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown2 = new System.Windows.Forms.NumericUpDown();
            this.btnStart = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.btnPickCard = new System.Windows.Forms.Button();
            this.txtCardPath = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lblIP = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).BeginInit();
            this.SuspendLayout();
            // 
            // lbl套牌牌数上限
            // 
            this.lbl套牌牌数上限.AutoSize = true;
            this.lbl套牌牌数上限.Location = new System.Drawing.Point(12, 9);
            this.lbl套牌牌数上限.Name = "lbl套牌牌数上限";
            this.lbl套牌牌数上限.Size = new System.Drawing.Size(79, 13);
            this.lbl套牌牌数上限.TabIndex = 0;
            this.lbl套牌牌数上限.Text = "套牌牌数上限";
            // 
            // lbl英雄生命值上限
            // 
            this.lbl英雄生命值上限.AutoSize = true;
            this.lbl英雄生命值上限.Location = new System.Drawing.Point(12, 35);
            this.lbl英雄生命值上限.Name = "lbl英雄生命值上限";
            this.lbl英雄生命值上限.Size = new System.Drawing.Size(91, 13);
            this.lbl英雄生命值上限.TabIndex = 1;
            this.lbl英雄生命值上限.Text = "英雄生命值上限";
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Location = new System.Drawing.Point(129, 7);
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(54, 20);
            this.numericUpDown1.TabIndex = 2;
            // 
            // numericUpDown2
            // 
            this.numericUpDown2.Location = new System.Drawing.Point(129, 33);
            this.numericUpDown2.Name = "numericUpDown2";
            this.numericUpDown2.Size = new System.Drawing.Size(54, 20);
            this.numericUpDown2.TabIndex = 3;
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(18, 146);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(75, 23);
            this.btnStart.TabIndex = 4;
            this.btnStart.Text = "启动服务";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // btnStop
            // 
            this.btnStop.Enabled = false;
            this.btnStop.Location = new System.Drawing.Point(108, 146);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(75, 23);
            this.btnStop.TabIndex = 5;
            this.btnStop.Text = "停止服务";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // btnPickCard
            // 
            this.btnPickCard.Location = new System.Drawing.Point(129, 60);
            this.btnPickCard.Name = "btnPickCard";
            this.btnPickCard.Size = new System.Drawing.Size(75, 23);
            this.btnPickCard.TabIndex = 9;
            this.btnPickCard.Text = "选择..";
            this.btnPickCard.UseVisualStyleBackColor = true;
            this.btnPickCard.Click += new System.EventHandler(this.btnPickCard_Click);
            // 
            // txtCardPath
            // 
            this.txtCardPath.Location = new System.Drawing.Point(15, 91);
            this.txtCardPath.Name = "txtCardPath";
            this.txtCardPath.Size = new System.Drawing.Size(319, 20);
            this.txtCardPath.TabIndex = 8;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 65);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(73, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "卡牌文件夹：";
            // 
            // lblIP
            // 
            this.lblIP.AutoSize = true;
            this.lblIP.Location = new System.Drawing.Point(18, 120);
            this.lblIP.Name = "lblIP";
            this.lblIP.Size = new System.Drawing.Size(61, 13);
            this.lblIP.TabIndex = 10;
            this.lblIP.Text = "IP Address:";
            // 
            // ServerConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(352, 181);
            this.Controls.Add(this.lblIP);
            this.Controls.Add(this.btnPickCard);
            this.Controls.Add(this.txtCardPath);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.numericUpDown2);
            this.Controls.Add(this.numericUpDown1);
            this.Controls.Add(this.lbl英雄生命值上限);
            this.Controls.Add(this.lbl套牌牌数上限);
            this.Name = "ServerConfig";
            this.Text = "服务器配置";
            this.Load += new System.EventHandler(this.ServerConfig_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbl套牌牌数上限;
        private System.Windows.Forms.Label lbl英雄生命值上限;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.NumericUpDown numericUpDown2;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Button btnPickCard;
        private System.Windows.Forms.TextBox txtCardPath;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblIP;
    }
}