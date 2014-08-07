namespace 炉边传说.CardEditor
{
    partial class CardManager
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
            this.lstCards = new System.Windows.Forms.ListView();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbCardFilter = new System.Windows.Forms.ComboBox();
            this.btnEdit = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnCreate = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lstCards
            // 
            this.lstCards.FullRowSelect = true;
            this.lstCards.GridLines = true;
            this.lstCards.Location = new System.Drawing.Point(30, 39);
            this.lstCards.Name = "lstCards";
            this.lstCards.Size = new System.Drawing.Size(667, 391);
            this.lstCards.TabIndex = 0;
            this.lstCards.UseCompatibleStateImageBehavior = false;
            this.lstCards.View = System.Windows.Forms.View.Details;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(48, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(31, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "过滤";
            // 
            // cmbCardFilter
            // 
            this.cmbCardFilter.FormattingEnabled = true;
            this.cmbCardFilter.Location = new System.Drawing.Point(93, 12);
            this.cmbCardFilter.Name = "cmbCardFilter";
            this.cmbCardFilter.Size = new System.Drawing.Size(121, 21);
            this.cmbCardFilter.TabIndex = 2;
            this.cmbCardFilter.SelectedIndexChanged += new System.EventHandler(this.cmbCardFilter_SelectedIndexChanged);
            // 
            // btnEdit
            // 
            this.btnEdit.Location = new System.Drawing.Point(492, 10);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(75, 23);
            this.btnEdit.TabIndex = 3;
            this.btnEdit.Text = "编辑";
            this.btnEdit.UseVisualStyleBackColor = true;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(573, 10);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 23);
            this.btnDelete.TabIndex = 4;
            this.btnDelete.Text = "禁用";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnCreate
            // 
            this.btnCreate.Location = new System.Drawing.Point(411, 10);
            this.btnCreate.Name = "btnCreate";
            this.btnCreate.Size = new System.Drawing.Size(75, 23);
            this.btnCreate.TabIndex = 5;
            this.btnCreate.Text = "新建";
            this.btnCreate.UseVisualStyleBackColor = true;
            // 
            // CardManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(711, 456);
            this.Controls.Add(this.btnCreate);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnEdit);
            this.Controls.Add(this.cmbCardFilter);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lstCards);
            this.Name = "CardManager";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "CardManager";
            this.Load += new System.EventHandler(this.CardManager_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView lstCards;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbCardFilter;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnCreate;
    }
}