namespace 炉边传说
{
    partial class frmExport
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
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.btnExportXml = new System.Windows.Forms.Button();
            this.ExportFolderPicker = new 炉边传说.ctlFilePicker();
            this.ExcelPicker = new 炉边传说.ctlFilePicker();
            this.chkMinion = new System.Windows.Forms.CheckBox();
            this.chkAbility = new System.Windows.Forms.CheckBox();
            this.chkWeapon = new System.Windows.Forms.CheckBox();
            this.chkSecret = new System.Windows.Forms.CheckBox();
            this.btnExportJSON = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnExportXml
            // 
            this.btnExportXml.Location = new System.Drawing.Point(488, 94);
            this.btnExportXml.Name = "btnExportXml";
            this.btnExportXml.Size = new System.Drawing.Size(157, 23);
            this.btnExportXml.TabIndex = 2;
            this.btnExportXml.Text = "导出到Xml";
            this.btnExportXml.UseVisualStyleBackColor = true;
            this.btnExportXml.Click += new System.EventHandler(this.btnExportXml_Click);
            // 
            // XmlFolderPicker
            // 
            this.ExportFolderPicker.BackColor = System.Drawing.Color.Transparent;
            this.ExportFolderPicker.FileFilter = "";
            this.ExportFolderPicker.FileName = "";
            this.ExportFolderPicker.Location = new System.Drawing.Point(28, 49);
            this.ExportFolderPicker.Name = "XmlFolderPicker";
            this.ExportFolderPicker.PickerType = 炉边传说.ctlFilePicker.DialogType.Directory;
            this.ExportFolderPicker.SelectedPathOrFileName = "";
            this.ExportFolderPicker.Size = new System.Drawing.Size(629, 31);
            this.ExportFolderPicker.TabIndex = 3;
            this.ExportFolderPicker.Title = "XML文件夹";
            // 
            // ExcelPicker
            // 
            this.ExcelPicker.BackColor = System.Drawing.Color.Transparent;
            this.ExcelPicker.FileFilter = "";
            this.ExcelPicker.FileName = "";
            this.ExcelPicker.Location = new System.Drawing.Point(28, 12);
            this.ExcelPicker.Name = "ExcelPicker";
            this.ExcelPicker.PickerType = 炉边传说.ctlFilePicker.DialogType.OpenFile;
            this.ExcelPicker.SelectedPathOrFileName = "";
            this.ExcelPicker.Size = new System.Drawing.Size(629, 31);
            this.ExcelPicker.TabIndex = 0;
            this.ExcelPicker.Title = "炉石资料文件";
            // 
            // chkMinion
            // 
            this.chkMinion.AutoSize = true;
            this.chkMinion.Checked = true;
            this.chkMinion.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkMinion.Location = new System.Drawing.Point(106, 98);
            this.chkMinion.Name = "chkMinion";
            this.chkMinion.Size = new System.Drawing.Size(50, 17);
            this.chkMinion.TabIndex = 4;
            this.chkMinion.Text = "随从";
            this.chkMinion.UseVisualStyleBackColor = true;
            // 
            // chkAbility
            // 
            this.chkAbility.AutoSize = true;
            this.chkAbility.Checked = true;
            this.chkAbility.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAbility.Location = new System.Drawing.Point(192, 100);
            this.chkAbility.Name = "chkAbility";
            this.chkAbility.Size = new System.Drawing.Size(50, 17);
            this.chkAbility.TabIndex = 5;
            this.chkAbility.Text = "法术";
            this.chkAbility.UseVisualStyleBackColor = true;
            // 
            // chkWeapon
            // 
            this.chkWeapon.AutoSize = true;
            this.chkWeapon.Checked = true;
            this.chkWeapon.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkWeapon.Location = new System.Drawing.Point(288, 100);
            this.chkWeapon.Name = "chkWeapon";
            this.chkWeapon.Size = new System.Drawing.Size(50, 17);
            this.chkWeapon.TabIndex = 6;
            this.chkWeapon.Text = "武器";
            this.chkWeapon.UseVisualStyleBackColor = true;
            // 
            // chkSecret
            // 
            this.chkSecret.AutoSize = true;
            this.chkSecret.Checked = true;
            this.chkSecret.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSecret.Location = new System.Drawing.Point(374, 100);
            this.chkSecret.Name = "chkSecret";
            this.chkSecret.Size = new System.Drawing.Size(50, 17);
            this.chkSecret.TabIndex = 7;
            this.chkSecret.Text = "奥秘";
            this.chkSecret.UseVisualStyleBackColor = true;
            // 
            // btnExportJSON
            // 
            this.btnExportJSON.Location = new System.Drawing.Point(488, 123);
            this.btnExportJSON.Name = "btnExportJSON";
            this.btnExportJSON.Size = new System.Drawing.Size(157, 23);
            this.btnExportJSON.TabIndex = 8;
            this.btnExportJSON.Text = "导出到JSON";
            this.btnExportJSON.UseVisualStyleBackColor = true;
            this.btnExportJSON.Click += new System.EventHandler(this.btnExportJSON_Click);
            // 
            // frmExport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(672, 160);
            this.Controls.Add(this.btnExportJSON);
            this.Controls.Add(this.chkSecret);
            this.Controls.Add(this.chkWeapon);
            this.Controls.Add(this.chkAbility);
            this.Controls.Add(this.chkMinion);
            this.Controls.Add(this.ExportFolderPicker);
            this.Controls.Add(this.btnExportXml);
            this.Controls.Add(this.ExcelPicker);
            this.Name = "frmExport";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "资料导入导出";
            this.Load += new System.EventHandler(this.frmExport_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ctlFilePicker ExcelPicker;
        private System.Windows.Forms.Button btnExportXml;
        private ctlFilePicker ExportFolderPicker;
        private System.Windows.Forms.CheckBox chkMinion;
        private System.Windows.Forms.CheckBox chkAbility;
        private System.Windows.Forms.CheckBox chkWeapon;
        private System.Windows.Forms.CheckBox chkSecret;
        private System.Windows.Forms.Button btnExportJSON;
    }
}

