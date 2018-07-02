using Athame.Properties;

namespace Athame.UI
{
    partial class MainForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.idTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.dlButton = new System.Windows.Forms.Button();
            this.queueListView = new System.Windows.Forms.ListView();
            this.statusCol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.trackNumberCol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.titleCol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.artistCol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.albumCol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.locCol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.collectionProgressBar = new System.Windows.Forms.ProgressBar();
            this.collectionStatusLabel = new System.Windows.Forms.Label();
            this.totalStatusLabel = new System.Windows.Forms.Label();
            this.totalProgressBar = new System.Windows.Forms.ProgressBar();
            this.settingsButton = new System.Windows.Forms.Button();
            this.queueMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.removeTrackToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeGroupToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.showCollectionInFileBrowserToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clearButton = new System.Windows.Forms.LinkLabel();
            this.pasteButton = new System.Windows.Forms.LinkLabel();
            this.urlValidStateLabel = new System.Windows.Forms.LinkLabel();
            this.startDownloadButton = new System.Windows.Forms.Button();
            this.mMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.bulkAddToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBox2.SuspendLayout();
            this.queueMenu.SuspendLayout();
            this.mMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // idTextBox
            // 
            resources.ApplyResources(this.idTextBox, "idTextBox");
            this.idTextBox.Name = "idTextBox";
            this.idTextBox.TextChanged += new System.EventHandler(this.idTextBox_TextChanged);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // dlButton
            // 
            resources.ApplyResources(this.dlButton, "dlButton");
            this.dlButton.Name = "dlButton";
            this.dlButton.UseVisualStyleBackColor = true;
            this.dlButton.Click += new System.EventHandler(this.button1_Click);
            // 
            // queueListView
            // 
            resources.ApplyResources(this.queueListView, "queueListView");
            this.queueListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.statusCol,
            this.trackNumberCol,
            this.titleCol,
            this.artistCol,
            this.albumCol,
            this.columnHeader1,
            this.locCol});
            this.queueListView.FullRowSelect = true;
            this.queueListView.Name = "queueListView";
            this.queueListView.UseCompatibleStateImageBehavior = false;
            this.queueListView.View = System.Windows.Forms.View.Details;
            this.queueListView.SelectedIndexChanged += new System.EventHandler(this.queueListView_SelectedIndexChanged);
            this.queueListView.KeyDown += new System.Windows.Forms.KeyEventHandler(this.queueListView_KeyDown);
            this.queueListView.MouseClick += new System.Windows.Forms.MouseEventHandler(this.queueListView_MouseClick);
            this.queueListView.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.queueListView_MouseDoubleClick);
            this.queueListView.MouseHover += new System.EventHandler(this.queueListView_MouseHover);
            // 
            // statusCol
            // 
            resources.ApplyResources(this.statusCol, "statusCol");
            // 
            // trackNumberCol
            // 
            resources.ApplyResources(this.trackNumberCol, "trackNumberCol");
            // 
            // titleCol
            // 
            resources.ApplyResources(this.titleCol, "titleCol");
            // 
            // artistCol
            // 
            resources.ApplyResources(this.artistCol, "artistCol");
            // 
            // albumCol
            // 
            resources.ApplyResources(this.albumCol, "albumCol");
            // 
            // columnHeader1
            // 
            resources.ApplyResources(this.columnHeader1, "columnHeader1");
            // 
            // locCol
            // 
            resources.ApplyResources(this.locCol, "locCol");
            // 
            // groupBox2
            // 
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.Controls.Add(this.collectionProgressBar);
            this.groupBox2.Controls.Add(this.collectionStatusLabel);
            this.groupBox2.Controls.Add(this.totalStatusLabel);
            this.groupBox2.Controls.Add(this.totalProgressBar);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // collectionProgressBar
            // 
            resources.ApplyResources(this.collectionProgressBar, "collectionProgressBar");
            this.collectionProgressBar.Name = "collectionProgressBar";
            // 
            // collectionStatusLabel
            // 
            resources.ApplyResources(this.collectionStatusLabel, "collectionStatusLabel");
            this.collectionStatusLabel.Name = "collectionStatusLabel";
            this.collectionStatusLabel.UseMnemonic = false;
            // 
            // totalStatusLabel
            // 
            resources.ApplyResources(this.totalStatusLabel, "totalStatusLabel");
            this.totalStatusLabel.Name = "totalStatusLabel";
            this.totalStatusLabel.UseMnemonic = false;
            // 
            // totalProgressBar
            // 
            resources.ApplyResources(this.totalProgressBar, "totalProgressBar");
            this.totalProgressBar.Name = "totalProgressBar";
            // 
            // settingsButton
            // 
            this.settingsButton.Image = global::Athame.Properties.Resources.menu_arrow;
            resources.ApplyResources(this.settingsButton, "settingsButton");
            this.settingsButton.Name = "settingsButton";
            this.settingsButton.UseVisualStyleBackColor = true;
            this.settingsButton.Click += new System.EventHandler(this.settingsButton_Click);
            // 
            // queueMenu
            // 
            this.queueMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.removeTrackToolStripMenuItem,
            this.removeGroupToolStripMenuItem,
            this.toolStripSeparator2,
            this.showCollectionInFileBrowserToolStripMenuItem});
            this.queueMenu.Name = "queueMenu";
            resources.ApplyResources(this.queueMenu, "queueMenu");
            this.queueMenu.Opening += new System.ComponentModel.CancelEventHandler(this.queueMenu_Opening);
            // 
            // removeTrackToolStripMenuItem
            // 
            this.removeTrackToolStripMenuItem.Name = "removeTrackToolStripMenuItem";
            resources.ApplyResources(this.removeTrackToolStripMenuItem, "removeTrackToolStripMenuItem");
            this.removeTrackToolStripMenuItem.Click += new System.EventHandler(this.removeTrackToolStripMenuItem_Click);
            // 
            // removeGroupToolStripMenuItem
            // 
            this.removeGroupToolStripMenuItem.Name = "removeGroupToolStripMenuItem";
            resources.ApplyResources(this.removeGroupToolStripMenuItem, "removeGroupToolStripMenuItem");
            this.removeGroupToolStripMenuItem.Click += new System.EventHandler(this.removeGroupToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            resources.ApplyResources(this.toolStripSeparator2, "toolStripSeparator2");
            // 
            // showCollectionInFileBrowserToolStripMenuItem
            // 
            resources.ApplyResources(this.showCollectionInFileBrowserToolStripMenuItem, "showCollectionInFileBrowserToolStripMenuItem");
            this.showCollectionInFileBrowserToolStripMenuItem.Name = "showCollectionInFileBrowserToolStripMenuItem";
            this.showCollectionInFileBrowserToolStripMenuItem.Click += new System.EventHandler(this.showCollectionInFileBrowserToolStripMenuItem_Click);
            // 
            // clearButton
            // 
            resources.ApplyResources(this.clearButton, "clearButton");
            this.clearButton.Name = "clearButton";
            this.clearButton.TabStop = true;
            this.clearButton.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.clearButton_LinkClicked);
            // 
            // pasteButton
            // 
            resources.ApplyResources(this.pasteButton, "pasteButton");
            this.pasteButton.Name = "pasteButton";
            this.pasteButton.TabStop = true;
            this.pasteButton.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.pasteButton_LinkClicked);
            // 
            // urlValidStateLabel
            // 
            this.urlValidStateLabel.Image = global::Athame.Properties.Resources.error;
            resources.ApplyResources(this.urlValidStateLabel, "urlValidStateLabel");
            this.urlValidStateLabel.Name = "urlValidStateLabel";
            this.urlValidStateLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.urlValidStateLabel_LinkClicked);
            // 
            // startDownloadButton
            // 
            resources.ApplyResources(this.startDownloadButton, "startDownloadButton");
            this.startDownloadButton.Name = "startDownloadButton";
            this.startDownloadButton.UseVisualStyleBackColor = true;
            this.startDownloadButton.Click += new System.EventHandler(this.startDownloadButton_Click);
            // 
            // mMenu
            // 
            this.mMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.bulkAddToolStripMenuItem,
            this.settingsToolStripMenuItem,
            this.toolStripSeparator1,
            this.aboutToolStripMenuItem});
            this.mMenu.Name = "mMenu";
            resources.ApplyResources(this.mMenu, "mMenu");
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            resources.ApplyResources(this.settingsToolStripMenuItem, "settingsToolStripMenuItem");
            this.settingsToolStripMenuItem.Click += new System.EventHandler(this.settingsToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            resources.ApplyResources(this.aboutToolStripMenuItem, "aboutToolStripMenuItem");
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // bulkAddToolStripMenuItem
            // 
            this.bulkAddToolStripMenuItem.Name = "bulkAddToolStripMenuItem";
            resources.ApplyResources(this.bulkAddToolStripMenuItem, "bulkAddToolStripMenuItem");
            this.bulkAddToolStripMenuItem.Click += new System.EventHandler(this.bulkAddToolStripMenuItem_Click);
            // 
            // MainForm
            // 
            this.AcceptButton = this.dlButton;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.queueListView);
            this.Controls.Add(this.startDownloadButton);
            this.Controls.Add(this.urlValidStateLabel);
            this.Controls.Add(this.pasteButton);
            this.Controls.Add(this.clearButton);
            this.Controls.Add(this.settingsButton);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.dlButton);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.idTextBox);
            this.Name = "MainForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.Shown += new System.EventHandler(this.MainForm_Shown);
            this.ResizeEnd += new System.EventHandler(this.MainForm_ResizeEnd);
            this.Move += new System.EventHandler(this.MainForm_Move);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.queueMenu.ResumeLayout(false);
            this.mMenu.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox idTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button dlButton;
        private System.Windows.Forms.ListView queueListView;
        private System.Windows.Forms.ColumnHeader titleCol;
        private System.Windows.Forms.ColumnHeader artistCol;
        private System.Windows.Forms.ColumnHeader albumCol;
        private System.Windows.Forms.ColumnHeader trackNumberCol;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ProgressBar totalProgressBar;
        private System.Windows.Forms.Label collectionStatusLabel;
        private System.Windows.Forms.Label totalStatusLabel;
        private System.Windows.Forms.Button settingsButton;
        private System.Windows.Forms.ContextMenuStrip queueMenu;
        private System.Windows.Forms.LinkLabel clearButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.LinkLabel pasteButton;
        private System.Windows.Forms.LinkLabel urlValidStateLabel;
        private System.Windows.Forms.Button startDownloadButton;
        private System.Windows.Forms.ToolStripMenuItem removeGroupToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip mMenu;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem removeTrackToolStripMenuItem;
        private System.Windows.Forms.ColumnHeader statusCol;
        private System.Windows.Forms.ToolStripMenuItem showCollectionInFileBrowserToolStripMenuItem;
        private System.Windows.Forms.ColumnHeader locCol;
        private System.Windows.Forms.ProgressBar collectionProgressBar;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ToolStripMenuItem bulkAddToolStripMenuItem;
    }
}

