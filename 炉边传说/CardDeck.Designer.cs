namespace 炉边传说
{
    partial class CardDeck
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabAblity = new System.Windows.Forms.TabPage();
            this.tabMinion = new System.Windows.Forms.TabPage();
            this.lstSelectCardDeck = new System.Windows.Forms.ListBox();
            this.lblCardCount = new System.Windows.Forms.Label();
            this.AblityList = new 炉边传说.ctlCardList();
            this.MinionList = new 炉边传说.ctlCardList();
            this.btnSave = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtCardDeckName = new System.Windows.Forms.TextBox();
            this.tabControl1.SuspendLayout();
            this.tabAblity.SuspendLayout();
            this.tabMinion.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabAblity);
            this.tabControl1.Controls.Add(this.tabMinion);
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(507, 300);
            this.tabControl1.TabIndex = 0;
            // 
            // tabAblity
            // 
            this.tabAblity.Controls.Add(this.AblityList);
            this.tabAblity.Location = new System.Drawing.Point(4, 22);
            this.tabAblity.Name = "tabAblity";
            this.tabAblity.Padding = new System.Windows.Forms.Padding(3);
            this.tabAblity.Size = new System.Drawing.Size(499, 274);
            this.tabAblity.TabIndex = 0;
            this.tabAblity.Text = "法术/奥秘";
            this.tabAblity.UseVisualStyleBackColor = true;
            // 
            // tabMinion
            // 
            this.tabMinion.Controls.Add(this.MinionList);
            this.tabMinion.Location = new System.Drawing.Point(4, 22);
            this.tabMinion.Name = "tabMinion";
            this.tabMinion.Padding = new System.Windows.Forms.Padding(3);
            this.tabMinion.Size = new System.Drawing.Size(499, 274);
            this.tabMinion.TabIndex = 1;
            this.tabMinion.Text = "随从/武器";
            this.tabMinion.UseVisualStyleBackColor = true;
            // 
            // lstSelectCardDeck
            // 
            this.lstSelectCardDeck.FormattingEnabled = true;
            this.lstSelectCardDeck.Location = new System.Drawing.Point(552, 34);
            this.lstSelectCardDeck.Name = "lstSelectCardDeck";
            this.lstSelectCardDeck.Size = new System.Drawing.Size(228, 277);
            this.lstSelectCardDeck.TabIndex = 1;
            // 
            // lblCardCount
            // 
            this.lblCardCount.AutoSize = true;
            this.lblCardCount.Location = new System.Drawing.Point(696, 332);
            this.lblCardCount.Name = "lblCardCount";
            this.lblCardCount.Size = new System.Drawing.Size(84, 13);
            this.lblCardCount.TabIndex = 2;
            this.lblCardCount.Text = "套牌数量：0/30";
            // 
            // AblityList
            // 
            this.AblityList.BackColor = System.Drawing.Color.White;
            this.AblityList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.AblityList.Location = new System.Drawing.Point(3, 3);
            this.AblityList.Name = "AblityList";
            this.AblityList.Size = new System.Drawing.Size(493, 268);
            this.AblityList.TabIndex = 0;
            // 
            // MinionList
            // 
            this.MinionList.BackColor = System.Drawing.Color.White;
            this.MinionList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MinionList.Location = new System.Drawing.Point(3, 3);
            this.MinionList.Name = "MinionList";
            this.MinionList.Size = new System.Drawing.Size(493, 268);
            this.MinionList.TabIndex = 0;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(392, 332);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 4;
            this.btnSave.Text = "保存";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(29, 337);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(79, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "套牌文件名称";
            // 
            // txtCardDeckName
            // 
            this.txtCardDeckName.Location = new System.Drawing.Point(141, 334);
            this.txtCardDeckName.Name = "txtCardDeckName";
            this.txtCardDeckName.Size = new System.Drawing.Size(228, 20);
            this.txtCardDeckName.TabIndex = 6;
            // 
            // CardDeck
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(792, 377);
            this.Controls.Add(this.txtCardDeckName);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.lblCardCount);
            this.Controls.Add(this.lstSelectCardDeck);
            this.Controls.Add(this.tabControl1);
            this.Name = "CardDeck";
            this.Text = "套牌构成";
            this.Load += new System.EventHandler(this.CardDeck_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabAblity.ResumeLayout(false);
            this.tabMinion.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabAblity;
        private System.Windows.Forms.TabPage tabMinion;
        private ctlCardList AblityList;
        private ctlCardList MinionList;
        private System.Windows.Forms.ListBox lstSelectCardDeck;
        private System.Windows.Forms.Label lblCardCount;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtCardDeckName;
    }
}