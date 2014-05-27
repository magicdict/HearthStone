namespace 炉边传说
{
    partial class frmStartGame
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
            this.btnCreateGame = new System.Windows.Forms.Button();
            this.lstWaitGuest = new System.Windows.Forms.ListBox();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnJoinGame = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtCardPath = new System.Windows.Forms.TextBox();
            this.btnPickCard = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnCreateGame
            // 
            this.btnCreateGame.Location = new System.Drawing.Point(12, 95);
            this.btnCreateGame.Name = "btnCreateGame";
            this.btnCreateGame.Size = new System.Drawing.Size(119, 23);
            this.btnCreateGame.TabIndex = 0;
            this.btnCreateGame.Text = "新建一局游戏";
            this.btnCreateGame.UseVisualStyleBackColor = true;
            this.btnCreateGame.Click += new System.EventHandler(this.btnCreateGame_Click);
            // 
            // lstWaitGuest
            // 
            this.lstWaitGuest.FormattingEnabled = true;
            this.lstWaitGuest.Location = new System.Drawing.Point(12, 124);
            this.lstWaitGuest.Name = "lstWaitGuest";
            this.lstWaitGuest.Size = new System.Drawing.Size(333, 95);
            this.lstWaitGuest.TabIndex = 1;
            // 
            // btnRefresh
            // 
            this.btnRefresh.Location = new System.Drawing.Point(274, 95);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(71, 23);
            this.btnRefresh.TabIndex = 2;
            this.btnRefresh.Text = "刷新";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // btnJoinGame
            // 
            this.btnJoinGame.Location = new System.Drawing.Point(137, 95);
            this.btnJoinGame.Name = "btnJoinGame";
            this.btnJoinGame.Size = new System.Drawing.Size(119, 23);
            this.btnJoinGame.TabIndex = 3;
            this.btnJoinGame.Text = "加入一局游戏";
            this.btnJoinGame.UseVisualStyleBackColor = true;
            this.btnJoinGame.Click += new System.EventHandler(this.btnJoinGame_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(23, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(73, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "卡牌文件夹：";
            // 
            // txtCardPath
            // 
            this.txtCardPath.Location = new System.Drawing.Point(26, 41);
            this.txtCardPath.Name = "txtCardPath";
            this.txtCardPath.Size = new System.Drawing.Size(319, 20);
            this.txtCardPath.TabIndex = 5;
            // 
            // btnPickCard
            // 
            this.btnPickCard.Location = new System.Drawing.Point(116, 10);
            this.btnPickCard.Name = "btnPickCard";
            this.btnPickCard.Size = new System.Drawing.Size(75, 23);
            this.btnPickCard.TabIndex = 6;
            this.btnPickCard.Text = "选择..";
            this.btnPickCard.UseVisualStyleBackColor = true;
            this.btnPickCard.Click += new System.EventHandler(this.btnPickCard_Click);
            // 
            // frmStartGame
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(360, 231);
            this.Controls.Add(this.btnPickCard);
            this.Controls.Add(this.txtCardPath);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnJoinGame);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.lstWaitGuest);
            this.Controls.Add(this.btnCreateGame);
            this.Name = "frmStartGame";
            this.Text = "开始游戏";
            this.Load += new System.EventHandler(this.frmStartGame_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCreateGame;
        private System.Windows.Forms.ListBox lstWaitGuest;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnJoinGame;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtCardPath;
        private System.Windows.Forms.Button btnPickCard;
    }
}