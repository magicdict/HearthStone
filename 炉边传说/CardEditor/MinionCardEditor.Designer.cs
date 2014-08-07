namespace 炉边传说.CardEditor
{
    partial class MinionCardEditor
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
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.numAttackPoint = new System.Windows.Forms.NumericUpDown();
            this.numLifePoint = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.chkDivineShield = new System.Windows.Forms.CheckBox();
            this.chkStealth = new System.Windows.Forms.CheckBox();
            this.chkTaunt = new System.Windows.Forms.CheckBox();
            this.chkWindfury = new System.Windows.Forms.CheckBox();
            this.chkCharge = new System.Windows.Forms.CheckBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.btnSelectDeathRattle = new System.Windows.Forms.Button();
            this.btnSelectBattleCry = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.ctlMinionBasicInfo = new 炉边传说.usrControl.ctlCommonPropertyEditor();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbSpecalEffect = new System.Windows.Forms.ComboBox();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numAttackPoint)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numLifePoint)).BeginInit();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.numAttackPoint);
            this.groupBox2.Controls.Add(this.numLifePoint);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.chkDivineShield);
            this.groupBox2.Controls.Add(this.chkStealth);
            this.groupBox2.Controls.Add(this.chkTaunt);
            this.groupBox2.Controls.Add(this.chkWindfury);
            this.groupBox2.Controls.Add(this.chkCharge);
            this.groupBox2.Location = new System.Drawing.Point(13, 112);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(353, 105);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "基本属性";
            // 
            // numAttackPoint
            // 
            this.numAttackPoint.Location = new System.Drawing.Point(87, 66);
            this.numAttackPoint.Name = "numAttackPoint";
            this.numAttackPoint.Size = new System.Drawing.Size(54, 20);
            this.numAttackPoint.TabIndex = 13;
            // 
            // numLifePoint
            // 
            this.numLifePoint.Location = new System.Drawing.Point(255, 65);
            this.numLifePoint.Name = "numLifePoint";
            this.numLifePoint.Size = new System.Drawing.Size(54, 20);
            this.numLifePoint.TabIndex = 12;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(200, 68);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(43, 13);
            this.label5.TabIndex = 11;
            this.label5.Text = "生命力";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(32, 68);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(43, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "攻击力";
            // 
            // chkDivineShield
            // 
            this.chkDivineShield.AutoSize = true;
            this.chkDivineShield.Location = new System.Drawing.Point(259, 33);
            this.chkDivineShield.Name = "chkDivineShield";
            this.chkDivineShield.Size = new System.Drawing.Size(50, 17);
            this.chkDivineShield.TabIndex = 6;
            this.chkDivineShield.Text = "圣盾";
            this.chkDivineShield.UseVisualStyleBackColor = true;
            // 
            // chkStealth
            // 
            this.chkStealth.AutoSize = true;
            this.chkStealth.Location = new System.Drawing.Point(203, 33);
            this.chkStealth.Name = "chkStealth";
            this.chkStealth.Size = new System.Drawing.Size(50, 17);
            this.chkStealth.TabIndex = 5;
            this.chkStealth.Text = "潜行";
            this.chkStealth.UseVisualStyleBackColor = true;
            // 
            // chkTaunt
            // 
            this.chkTaunt.AutoSize = true;
            this.chkTaunt.Location = new System.Drawing.Point(35, 33);
            this.chkTaunt.Name = "chkTaunt";
            this.chkTaunt.Size = new System.Drawing.Size(50, 17);
            this.chkTaunt.TabIndex = 2;
            this.chkTaunt.Text = "嘲讽";
            this.chkTaunt.UseVisualStyleBackColor = true;
            // 
            // chkWindfury
            // 
            this.chkWindfury.AutoSize = true;
            this.chkWindfury.Location = new System.Drawing.Point(147, 33);
            this.chkWindfury.Name = "chkWindfury";
            this.chkWindfury.Size = new System.Drawing.Size(50, 17);
            this.chkWindfury.TabIndex = 3;
            this.chkWindfury.Text = "风怒";
            this.chkWindfury.UseVisualStyleBackColor = true;
            // 
            // chkCharge
            // 
            this.chkCharge.AutoSize = true;
            this.chkCharge.Location = new System.Drawing.Point(91, 33);
            this.chkCharge.Name = "chkCharge";
            this.chkCharge.Size = new System.Drawing.Size(50, 17);
            this.chkCharge.TabIndex = 4;
            this.chkCharge.Text = "冲锋";
            this.chkCharge.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label7);
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Controls.Add(this.btnSelectDeathRattle);
            this.groupBox3.Controls.Add(this.btnSelectBattleCry);
            this.groupBox3.Location = new System.Drawing.Point(13, 237);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(353, 87);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "战吼亡语";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(32, 53);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(31, 13);
            this.label7.TabIndex = 11;
            this.label7.Text = "亡语";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(32, 24);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(31, 13);
            this.label6.TabIndex = 10;
            this.label6.Text = "战吼";
            // 
            // btnSelectDeathRattle
            // 
            this.btnSelectDeathRattle.Location = new System.Drawing.Point(245, 48);
            this.btnSelectDeathRattle.Name = "btnSelectDeathRattle";
            this.btnSelectDeathRattle.Size = new System.Drawing.Size(89, 23);
            this.btnSelectDeathRattle.TabIndex = 4;
            this.btnSelectDeathRattle.Text = "编号选择...";
            this.btnSelectDeathRattle.UseVisualStyleBackColor = true;
            // 
            // btnSelectBattleCry
            // 
            this.btnSelectBattleCry.Location = new System.Drawing.Point(245, 19);
            this.btnSelectBattleCry.Name = "btnSelectBattleCry";
            this.btnSelectBattleCry.Size = new System.Drawing.Size(89, 23);
            this.btnSelectBattleCry.TabIndex = 3;
            this.btnSelectBattleCry.Text = "编号选择...";
            this.btnSelectBattleCry.UseVisualStyleBackColor = true;
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.btnSave.Location = new System.Drawing.Point(258, 377);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(89, 23);
            this.btnSave.TabIndex = 3;
            this.btnSave.Text = "保存";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // ctlMinionBasicInfo
            // 
            this.ctlMinionBasicInfo.BackColor = System.Drawing.Color.White;
            this.ctlMinionBasicInfo.Location = new System.Drawing.Point(13, 12);
            this.ctlMinionBasicInfo.Name = "ctlMinionBasicInfo";
            this.ctlMinionBasicInfo.Size = new System.Drawing.Size(365, 94);
            this.ctlMinionBasicInfo.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(22, 344);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "特殊效果";
            // 
            // cmbSpecalEffect
            // 
            this.cmbSpecalEffect.FormattingEnabled = true;
            this.cmbSpecalEffect.Location = new System.Drawing.Point(100, 341);
            this.cmbSpecalEffect.Name = "cmbSpecalEffect";
            this.cmbSpecalEffect.Size = new System.Drawing.Size(121, 21);
            this.cmbSpecalEffect.TabIndex = 6;
            // 
            // MinionCardEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(390, 412);
            this.Controls.Add(this.cmbSpecalEffect);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ctlMinionBasicInfo);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Name = "MinionCardEditor";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "随从";
            this.Load += new System.EventHandler(this.CreateMinionCard_Load);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numAttackPoint)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numLifePoint)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox chkTaunt;
        private System.Windows.Forms.CheckBox chkWindfury;
        private System.Windows.Forms.CheckBox chkCharge;
        private System.Windows.Forms.CheckBox chkStealth;
        private System.Windows.Forms.CheckBox chkDivineShield;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button btnSelectDeathRattle;
        private System.Windows.Forms.Button btnSelectBattleCry;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.NumericUpDown numLifePoint;
        private System.Windows.Forms.NumericUpDown numAttackPoint;
        private usrControl.ctlCommonPropertyEditor ctlMinionBasicInfo;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbSpecalEffect;
    }
}