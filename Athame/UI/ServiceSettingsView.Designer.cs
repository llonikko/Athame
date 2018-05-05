namespace Athame.UI
{
    partial class ServiceSettingsView
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.authPanel = new System.Windows.Forms.Panel();
            this.restoreButton = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.signInStatusLabel = new System.Windows.Forms.Label();
            this.signInButton = new System.Windows.Forms.Button();
            this.servicePanel = new System.Windows.Forms.Panel();
            this.authPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // authPanel
            // 
            this.authPanel.Controls.Add(this.restoreButton);
            this.authPanel.Controls.Add(this.panel2);
            this.authPanel.Controls.Add(this.signInStatusLabel);
            this.authPanel.Controls.Add(this.signInButton);
            this.authPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.authPanel.Location = new System.Drawing.Point(0, 0);
            this.authPanel.Name = "authPanel";
            this.authPanel.Size = new System.Drawing.Size(631, 43);
            this.authPanel.TabIndex = 1;
            // 
            // restoreButton
            // 
            this.restoreButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.restoreButton.Location = new System.Drawing.Point(447, 3);
            this.restoreButton.Name = "restoreButton";
            this.restoreButton.Size = new System.Drawing.Size(87, 27);
            this.restoreButton.TabIndex = 3;
            this.restoreButton.Text = "Restore";
            this.restoreButton.UseVisualStyleBackColor = true;
            this.restoreButton.Click += new System.EventHandler(this.restoreButton_Click);
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.BackColor = System.Drawing.Color.Gainsboro;
            this.panel2.Location = new System.Drawing.Point(6, 37);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(621, 1);
            this.panel2.TabIndex = 2;
            // 
            // signInStatusLabel
            // 
            this.signInStatusLabel.Location = new System.Drawing.Point(3, 3);
            this.signInStatusLabel.Name = "signInStatusLabel";
            this.signInStatusLabel.Size = new System.Drawing.Size(354, 35);
            this.signInStatusLabel.TabIndex = 1;
            this.signInStatusLabel.Text = "Sign in status goes here";
            this.signInStatusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // signInButton
            // 
            this.signInButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.signInButton.Location = new System.Drawing.Point(540, 3);
            this.signInButton.Name = "signInButton";
            this.signInButton.Size = new System.Drawing.Size(87, 27);
            this.signInButton.TabIndex = 0;
            this.signInButton.Text = "Sign in/out";
            this.signInButton.UseVisualStyleBackColor = true;
            this.signInButton.Click += new System.EventHandler(this.signInButton_Click);
            // 
            // servicePanel
            // 
            this.servicePanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.servicePanel.Location = new System.Drawing.Point(0, 43);
            this.servicePanel.Name = "servicePanel";
            this.servicePanel.Size = new System.Drawing.Size(631, 319);
            this.servicePanel.TabIndex = 2;
            // 
            // ServiceSettingsView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.servicePanel);
            this.Controls.Add(this.authPanel);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "ServiceSettingsView";
            this.Size = new System.Drawing.Size(631, 362);
            this.authPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel authPanel;
        private System.Windows.Forms.Label signInStatusLabel;
        private System.Windows.Forms.Button signInButton;
        private System.Windows.Forms.Panel servicePanel;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button restoreButton;
    }
}
