namespace file_searching {
    partial class repair_modal {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.repairButton = new System.Windows.Forms.Button();
            this.lastButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // repairButton
            // 
            this.repairButton.Dock = System.Windows.Forms.DockStyle.Top;
            this.repairButton.Location = new System.Drawing.Point(0, 0);
            this.repairButton.Name = "repairButton";
            this.repairButton.Size = new System.Drawing.Size(282, 27);
            this.repairButton.TabIndex = 2;
            this.repairButton.Text = "Přeinstalovat pD";
            this.repairButton.UseVisualStyleBackColor = true;
            this.repairButton.Click += new System.EventHandler(this.repairButton_Click);
            // 
            // lastButton
            // 
            this.lastButton.Dock = System.Windows.Forms.DockStyle.Top;
            this.lastButton.Location = new System.Drawing.Point(0, 27);
            this.lastButton.Name = "lastButton";
            this.lastButton.Size = new System.Drawing.Size(282, 27);
            this.lastButton.TabIndex = 3;
            this.lastButton.Text = "Spustit předchozí verzi";
            this.lastButton.UseVisualStyleBackColor = true;
            this.lastButton.Click += new System.EventHandler(this.lastButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Location = new System.Drawing.Point(195, 101);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 4;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // repair_modal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(282, 136);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.lastButton);
            this.Controls.Add(this.repairButton);
            this.Name = "repair_modal";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Vyberte akci:";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button repairButton;
        private System.Windows.Forms.Button lastButton;
        private System.Windows.Forms.Button cancelButton;
    }
}