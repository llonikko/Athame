namespace Athame.UI
{
    partial class SettingsForm
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
            this.generalTab = new System.Windows.Forms.TabPage();
            this.confirmExitCheckBox = new System.Windows.Forms.CheckBox();
            this.pldSameAsAlbumTrack = new System.Windows.Forms.CheckBox();
            this.formatHelpButton = new System.Windows.Forms.Button();
            this.pldOptionsGroupBox = new System.Windows.Forms.GroupBox();
            this.compiledPlaylistPathFormatLabel = new System.Windows.Forms.Label();
            this.ignoreAlbumArtworkCheckBox = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.savePlaylistAsComboBox = new System.Windows.Forms.ComboBox();
            this.pldAskWhereToSaveRadioButton = new System.Windows.Forms.RadioButton();
            this.pldSaveToRadioButton = new System.Windows.Forms.RadioButton();
            this.pldSaveLocLabel = new System.Windows.Forms.Label();
            this.pldPathFormatTextBox = new System.Windows.Forms.TextBox();
            this.pldSaveLocBrowseButton = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.compiledPathFormatLabel = new System.Windows.Forms.Label();
            this.askWhereToSaveRadioButton = new System.Windows.Forms.RadioButton();
            this.saveToRadioButton = new System.Windows.Forms.RadioButton();
            this.saveLocLabel = new System.Windows.Forms.Label();
            this.pathFormatTextBox = new System.Windows.Forms.TextBox();
            this.saveLocBrowseButton = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
            this.artworkSaveAsFileRadioButton = new System.Windows.Forms.RadioButton();
            this.artworkSaveAsFormattedFileRadioButton = new System.Windows.Forms.RadioButton();
            this.artworkDontSaveRadioButton = new System.Windows.Forms.RadioButton();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.serviceAuthorLabel = new System.Windows.Forms.Label();
            this.serviceDescriptionLabel = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.serviceNameLabel = new System.Windows.Forms.Label();
            this.serviceWebsiteLabel = new System.Windows.Forms.LinkLabel();
            this.openPluginDirButton = new System.Windows.Forms.Button();
            this.serviceUiPanel = new System.Windows.Forms.Panel();
            this.servicesListBox = new System.Windows.Forms.ListBox();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.saveButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.mFolderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.label7 = new System.Windows.Forms.Label();
            this.serviceVersionLabel = new System.Windows.Forms.Label();
            this.tabControl1.SuspendLayout();
            this.generalTab.SuspendLayout();
            this.pldOptionsGroupBox.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.flowLayoutPanel2.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.generalTab);
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Location = new System.Drawing.Point(14, 16);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(716, 554);
            this.tabControl1.TabIndex = 0;
            // 
            // generalTab
            // 
            this.generalTab.Controls.Add(this.confirmExitCheckBox);
            this.generalTab.Controls.Add(this.pldSameAsAlbumTrack);
            this.generalTab.Controls.Add(this.formatHelpButton);
            this.generalTab.Controls.Add(this.pldOptionsGroupBox);
            this.generalTab.Controls.Add(this.groupBox1);
            this.generalTab.Controls.Add(this.label4);
            this.generalTab.Controls.Add(this.flowLayoutPanel2);
            this.generalTab.Location = new System.Drawing.Point(4, 24);
            this.generalTab.Name = "generalTab";
            this.generalTab.Padding = new System.Windows.Forms.Padding(3);
            this.generalTab.Size = new System.Drawing.Size(708, 526);
            this.generalTab.TabIndex = 0;
            this.generalTab.Text = "General";
            this.generalTab.UseVisualStyleBackColor = true;
            // 
            // confirmExitCheckBox
            // 
            this.confirmExitCheckBox.AutoSize = true;
            this.confirmExitCheckBox.Location = new System.Drawing.Point(6, 385);
            this.confirmExitCheckBox.Name = "confirmExitCheckBox";
            this.confirmExitCheckBox.Size = new System.Drawing.Size(280, 19);
            this.confirmExitCheckBox.TabIndex = 2;
            this.confirmExitCheckBox.Text = "Ask before exiting if there are items in the queue";
            this.confirmExitCheckBox.UseVisualStyleBackColor = true;
            this.confirmExitCheckBox.CheckedChanged += new System.EventHandler(this.confirmExitCheckBox_CheckedChanged);
            // 
            // pldSameAsAlbumTrack
            // 
            this.pldSameAsAlbumTrack.AutoSize = true;
            this.pldSameAsAlbumTrack.BackColor = System.Drawing.Color.White;
            this.pldSameAsAlbumTrack.Location = new System.Drawing.Point(167, 163);
            this.pldSameAsAlbumTrack.Name = "pldSameAsAlbumTrack";
            this.pldSameAsAlbumTrack.Padding = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.pldSameAsAlbumTrack.Size = new System.Drawing.Size(147, 19);
            this.pldSameAsAlbumTrack.TabIndex = 14;
            this.pldSameAsAlbumTrack.Text = "Same as album/track";
            this.pldSameAsAlbumTrack.UseVisualStyleBackColor = false;
            this.pldSameAsAlbumTrack.CheckedChanged += new System.EventHandler(this.pldSameAsAlbumTrack_CheckedChanged);
            // 
            // formatHelpButton
            // 
            this.formatHelpButton.Location = new System.Drawing.Point(542, 382);
            this.formatHelpButton.Name = "formatHelpButton";
            this.formatHelpButton.Size = new System.Drawing.Size(160, 23);
            this.formatHelpButton.TabIndex = 14;
            this.formatHelpButton.Text = "Help with path formats...";
            this.formatHelpButton.UseVisualStyleBackColor = true;
            this.formatHelpButton.Click += new System.EventHandler(this.formatHelpButton_Click);
            // 
            // pldOptionsGroupBox
            // 
            this.pldOptionsGroupBox.Controls.Add(this.compiledPlaylistPathFormatLabel);
            this.pldOptionsGroupBox.Controls.Add(this.ignoreAlbumArtworkCheckBox);
            this.pldOptionsGroupBox.Controls.Add(this.label3);
            this.pldOptionsGroupBox.Controls.Add(this.savePlaylistAsComboBox);
            this.pldOptionsGroupBox.Controls.Add(this.pldAskWhereToSaveRadioButton);
            this.pldOptionsGroupBox.Controls.Add(this.pldSaveToRadioButton);
            this.pldOptionsGroupBox.Controls.Add(this.pldSaveLocLabel);
            this.pldOptionsGroupBox.Controls.Add(this.pldPathFormatTextBox);
            this.pldOptionsGroupBox.Controls.Add(this.pldSaveLocBrowseButton);
            this.pldOptionsGroupBox.Controls.Add(this.label2);
            this.pldOptionsGroupBox.Location = new System.Drawing.Point(6, 163);
            this.pldOptionsGroupBox.Name = "pldOptionsGroupBox";
            this.pldOptionsGroupBox.Size = new System.Drawing.Size(696, 213);
            this.pldOptionsGroupBox.TabIndex = 13;
            this.pldOptionsGroupBox.TabStop = false;
            this.pldOptionsGroupBox.Text = "Playlist download options";
            // 
            // compiledPlaylistPathFormatLabel
            // 
            this.compiledPlaylistPathFormatLabel.AutoSize = true;
            this.compiledPlaylistPathFormatLabel.Location = new System.Drawing.Point(88, 119);
            this.compiledPlaylistPathFormatLabel.Name = "compiledPlaylistPathFormatLabel";
            this.compiledPlaylistPathFormatLabel.Size = new System.Drawing.Size(184, 15);
            this.compiledPlaylistPathFormatLabel.TabIndex = 17;
            this.compiledPlaylistPathFormatLabel.Text = "compiledPlaylistPathFormatLabel";
            // 
            // ignoreAlbumArtworkCheckBox
            // 
            this.ignoreAlbumArtworkCheckBox.AutoSize = true;
            this.ignoreAlbumArtworkCheckBox.Location = new System.Drawing.Point(9, 186);
            this.ignoreAlbumArtworkCheckBox.Name = "ignoreAlbumArtworkCheckBox";
            this.ignoreAlbumArtworkCheckBox.Size = new System.Drawing.Size(315, 19);
            this.ignoreAlbumArtworkCheckBox.TabIndex = 15;
            this.ignoreAlbumArtworkCheckBox.Text = "Don\'t save album artwork when downloading a playlist";
            this.ignoreAlbumArtworkCheckBox.UseVisualStyleBackColor = true;
            this.ignoreAlbumArtworkCheckBox.CheckedChanged += new System.EventHandler(this.ignoreAlbumArtworkCheckBox_CheckedChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 159);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(107, 15);
            this.label3.TabIndex = 15;
            this.label3.Text = "Save playlist as file:";
            // 
            // savePlaylistAsComboBox
            // 
            this.savePlaylistAsComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.savePlaylistAsComboBox.FormattingEnabled = true;
            this.savePlaylistAsComboBox.Items.AddRange(new object[] {
            "Don\'t save as file",
            "M3U (M3U8)",
            "PLS"});
            this.savePlaylistAsComboBox.Location = new System.Drawing.Point(119, 156);
            this.savePlaylistAsComboBox.Name = "savePlaylistAsComboBox";
            this.savePlaylistAsComboBox.Size = new System.Drawing.Size(121, 23);
            this.savePlaylistAsComboBox.TabIndex = 16;
            this.savePlaylistAsComboBox.SelectedIndexChanged += new System.EventHandler(this.savePlaylistAsComboBox_SelectedIndexChanged);
            // 
            // pldAskWhereToSaveRadioButton
            // 
            this.pldAskWhereToSaveRadioButton.AutoSize = true;
            this.pldAskWhereToSaveRadioButton.Location = new System.Drawing.Point(13, 55);
            this.pldAskWhereToSaveRadioButton.Name = "pldAskWhereToSaveRadioButton";
            this.pldAskWhereToSaveRadioButton.Size = new System.Drawing.Size(122, 19);
            this.pldAskWhereToSaveRadioButton.TabIndex = 12;
            this.pldAskWhereToSaveRadioButton.TabStop = true;
            this.pldAskWhereToSaveRadioButton.Text = "Ask me every time";
            this.pldAskWhereToSaveRadioButton.UseVisualStyleBackColor = true;
            this.pldAskWhereToSaveRadioButton.CheckedChanged += new System.EventHandler(this.pldAskWhereToSaveRadioButton_CheckedChanged);
            // 
            // pldSaveToRadioButton
            // 
            this.pldSaveToRadioButton.AutoSize = true;
            this.pldSaveToRadioButton.Location = new System.Drawing.Point(13, 30);
            this.pldSaveToRadioButton.Name = "pldSaveToRadioButton";
            this.pldSaveToRadioButton.Size = new System.Drawing.Size(66, 19);
            this.pldSaveToRadioButton.TabIndex = 11;
            this.pldSaveToRadioButton.TabStop = true;
            this.pldSaveToRadioButton.Text = "Save to:";
            this.pldSaveToRadioButton.UseVisualStyleBackColor = true;
            this.pldSaveToRadioButton.CheckedChanged += new System.EventHandler(this.pldSaveToRadioButton_CheckedChanged);
            // 
            // pldSaveLocLabel
            // 
            this.pldSaveLocLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pldSaveLocLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pldSaveLocLabel.Location = new System.Drawing.Point(85, 28);
            this.pldSaveLocLabel.Name = "pldSaveLocLabel";
            this.pldSaveLocLabel.Size = new System.Drawing.Size(524, 23);
            this.pldSaveLocLabel.TabIndex = 7;
            this.pldSaveLocLabel.Text = "Placeholder text ***DO NOT TRANSLATE***";
            this.pldSaveLocLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pldPathFormatTextBox
            // 
            this.pldPathFormatTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pldPathFormatTextBox.Location = new System.Drawing.Point(91, 93);
            this.pldPathFormatTextBox.Name = "pldPathFormatTextBox";
            this.pldPathFormatTextBox.Size = new System.Drawing.Size(599, 23);
            this.pldPathFormatTextBox.TabIndex = 10;
            this.pldPathFormatTextBox.TextChanged += new System.EventHandler(this.pldPathFormatTextBox_TextChanged);
            // 
            // pldSaveLocBrowseButton
            // 
            this.pldSaveLocBrowseButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pldSaveLocBrowseButton.Location = new System.Drawing.Point(615, 28);
            this.pldSaveLocBrowseButton.Name = "pldSaveLocBrowseButton";
            this.pldSaveLocBrowseButton.Size = new System.Drawing.Size(75, 23);
            this.pldSaveLocBrowseButton.TabIndex = 8;
            this.pldSaveLocBrowseButton.Text = "Browse...";
            this.pldSaveLocBrowseButton.UseVisualStyleBackColor = true;
            this.pldSaveLocBrowseButton.Click += new System.EventHandler(this.pldSaveLocBrowseButton_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 96);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(73, 15);
            this.label2.TabIndex = 9;
            this.label2.Text = "Path format:";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.compiledPathFormatLabel);
            this.groupBox1.Controls.Add(this.askWhereToSaveRadioButton);
            this.groupBox1.Controls.Add(this.saveToRadioButton);
            this.groupBox1.Controls.Add(this.saveLocLabel);
            this.groupBox1.Controls.Add(this.pathFormatTextBox);
            this.groupBox1.Controls.Add(this.saveLocBrowseButton);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Location = new System.Drawing.Point(6, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(696, 151);
            this.groupBox1.TabIndex = 11;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Album/Track download options";
            // 
            // compiledPathFormatLabel
            // 
            this.compiledPathFormatLabel.AutoSize = true;
            this.compiledPathFormatLabel.Location = new System.Drawing.Point(88, 111);
            this.compiledPathFormatLabel.Name = "compiledPathFormatLabel";
            this.compiledPathFormatLabel.Size = new System.Drawing.Size(147, 15);
            this.compiledPathFormatLabel.TabIndex = 2;
            this.compiledPathFormatLabel.Text = "compiledPathFormatLabel";
            // 
            // askWhereToSaveRadioButton
            // 
            this.askWhereToSaveRadioButton.AutoSize = true;
            this.askWhereToSaveRadioButton.Location = new System.Drawing.Point(13, 47);
            this.askWhereToSaveRadioButton.Name = "askWhereToSaveRadioButton";
            this.askWhereToSaveRadioButton.Size = new System.Drawing.Size(122, 19);
            this.askWhereToSaveRadioButton.TabIndex = 12;
            this.askWhereToSaveRadioButton.TabStop = true;
            this.askWhereToSaveRadioButton.Text = "Ask me every time";
            this.askWhereToSaveRadioButton.UseVisualStyleBackColor = true;
            this.askWhereToSaveRadioButton.CheckedChanged += new System.EventHandler(this.askWhereToSaveRadioButton_CheckedChanged);
            // 
            // saveToRadioButton
            // 
            this.saveToRadioButton.AutoSize = true;
            this.saveToRadioButton.Location = new System.Drawing.Point(13, 22);
            this.saveToRadioButton.Name = "saveToRadioButton";
            this.saveToRadioButton.Size = new System.Drawing.Size(66, 19);
            this.saveToRadioButton.TabIndex = 11;
            this.saveToRadioButton.TabStop = true;
            this.saveToRadioButton.Text = "Save to:";
            this.saveToRadioButton.UseVisualStyleBackColor = true;
            this.saveToRadioButton.CheckedChanged += new System.EventHandler(this.saveToRadioButton_CheckedChanged);
            // 
            // saveLocLabel
            // 
            this.saveLocLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.saveLocLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.saveLocLabel.Location = new System.Drawing.Point(85, 20);
            this.saveLocLabel.Name = "saveLocLabel";
            this.saveLocLabel.Size = new System.Drawing.Size(524, 23);
            this.saveLocLabel.TabIndex = 7;
            this.saveLocLabel.Text = "Placeholder text ***DO NOT TRANSLATE**";
            this.saveLocLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pathFormatTextBox
            // 
            this.pathFormatTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pathFormatTextBox.Location = new System.Drawing.Point(91, 85);
            this.pathFormatTextBox.Name = "pathFormatTextBox";
            this.pathFormatTextBox.Size = new System.Drawing.Size(599, 23);
            this.pathFormatTextBox.TabIndex = 10;
            this.pathFormatTextBox.TextChanged += new System.EventHandler(this.pathFormatTextBox_TextChanged);
            // 
            // saveLocBrowseButton
            // 
            this.saveLocBrowseButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.saveLocBrowseButton.Location = new System.Drawing.Point(615, 20);
            this.saveLocBrowseButton.Name = "saveLocBrowseButton";
            this.saveLocBrowseButton.Size = new System.Drawing.Size(75, 23);
            this.saveLocBrowseButton.TabIndex = 8;
            this.saveLocBrowseButton.Text = "Browse...";
            this.saveLocBrowseButton.UseVisualStyleBackColor = true;
            this.saveLocBrowseButton.Click += new System.EventHandler(this.saveLocBrowseButton_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 88);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(73, 15);
            this.label5.TabIndex = 9;
            this.label5.Text = "Path format:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 418);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(114, 15);
            this.label4.TabIndex = 2;
            this.label4.Text = "Save album artwork:";
            // 
            // flowLayoutPanel2
            // 
            this.flowLayoutPanel2.Controls.Add(this.artworkSaveAsFileRadioButton);
            this.flowLayoutPanel2.Controls.Add(this.artworkSaveAsFormattedFileRadioButton);
            this.flowLayoutPanel2.Controls.Add(this.artworkDontSaveRadioButton);
            this.flowLayoutPanel2.Location = new System.Drawing.Point(126, 418);
            this.flowLayoutPanel2.Name = "flowLayoutPanel2";
            this.flowLayoutPanel2.Size = new System.Drawing.Size(200, 100);
            this.flowLayoutPanel2.TabIndex = 3;
            // 
            // artworkSaveAsFileRadioButton
            // 
            this.artworkSaveAsFileRadioButton.AutoSize = true;
            this.artworkSaveAsFileRadioButton.Location = new System.Drawing.Point(3, 3);
            this.artworkSaveAsFileRadioButton.Name = "artworkSaveAsFileRadioButton";
            this.artworkSaveAsFileRadioButton.Size = new System.Drawing.Size(98, 19);
            this.artworkSaveAsFileRadioButton.TabIndex = 0;
            this.artworkSaveAsFileRadioButton.TabStop = true;
            this.artworkSaveAsFileRadioButton.Text = "As \"cover.ext\"";
            this.artworkSaveAsFileRadioButton.UseVisualStyleBackColor = true;
            this.artworkSaveAsFileRadioButton.CheckedChanged += new System.EventHandler(this.artworkSaveAsFileRadioButton_CheckedChanged);
            // 
            // artworkSaveAsFormattedFileRadioButton
            // 
            this.artworkSaveAsFormattedFileRadioButton.AutoSize = true;
            this.artworkSaveAsFormattedFileRadioButton.Location = new System.Drawing.Point(3, 28);
            this.artworkSaveAsFormattedFileRadioButton.Name = "artworkSaveAsFormattedFileRadioButton";
            this.artworkSaveAsFormattedFileRadioButton.Size = new System.Drawing.Size(160, 19);
            this.artworkSaveAsFormattedFileRadioButton.TabIndex = 1;
            this.artworkSaveAsFormattedFileRadioButton.TabStop = true;
            this.artworkSaveAsFormattedFileRadioButton.Text = "As \"{Artist} - {Album}.ext\"";
            this.artworkSaveAsFormattedFileRadioButton.UseVisualStyleBackColor = true;
            this.artworkSaveAsFormattedFileRadioButton.CheckedChanged += new System.EventHandler(this.artworkSaveAsFormattedFileRadioButton_CheckedChanged);
            // 
            // artworkDontSaveRadioButton
            // 
            this.artworkDontSaveRadioButton.AutoSize = true;
            this.artworkDontSaveRadioButton.Location = new System.Drawing.Point(3, 53);
            this.artworkDontSaveRadioButton.Name = "artworkDontSaveRadioButton";
            this.artworkDontSaveRadioButton.Size = new System.Drawing.Size(80, 19);
            this.artworkDontSaveRadioButton.TabIndex = 2;
            this.artworkDontSaveRadioButton.TabStop = true;
            this.artworkDontSaveRadioButton.Text = "Don\'t save";
            this.artworkDontSaveRadioButton.UseVisualStyleBackColor = true;
            this.artworkDontSaveRadioButton.CheckedChanged += new System.EventHandler(this.artworkDontSaveRadioButton_CheckedChanged);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.panel1);
            this.tabPage1.Controls.Add(this.openPluginDirButton);
            this.tabPage1.Controls.Add(this.serviceUiPanel);
            this.tabPage1.Controls.Add(this.servicesListBox);
            this.tabPage1.Location = new System.Drawing.Point(4, 24);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(708, 526);
            this.tabPage1.TabIndex = 1;
            this.tabPage1.Text = "Plugins and Services";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.panel1.Controls.Add(this.tableLayoutPanel1);
            this.panel1.Location = new System.Drawing.Point(185, 338);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(517, 134);
            this.panel1.TabIndex = 3;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 18.37524F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 81.62476F));
            this.tableLayoutPanel1.Controls.Add(this.label7, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.serviceAuthorLabel, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.serviceDescriptionLabel, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.label10, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.label8, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.label6, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.serviceNameLabel, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.serviceWebsiteLabel, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.serviceVersionLabel, 1, 4);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 5;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(517, 134);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // serviceAuthorLabel
            // 
            this.serviceAuthorLabel.AutoSize = true;
            this.serviceAuthorLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.serviceAuthorLabel.Location = new System.Drawing.Point(97, 56);
            this.serviceAuthorLabel.Name = "serviceAuthorLabel";
            this.serviceAuthorLabel.Size = new System.Drawing.Size(417, 28);
            this.serviceAuthorLabel.TabIndex = 9;
            this.serviceAuthorLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // serviceDescriptionLabel
            // 
            this.serviceDescriptionLabel.AutoSize = true;
            this.serviceDescriptionLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.serviceDescriptionLabel.Location = new System.Drawing.Point(97, 28);
            this.serviceDescriptionLabel.Name = "serviceDescriptionLabel";
            this.serviceDescriptionLabel.Size = new System.Drawing.Size(417, 28);
            this.serviceDescriptionLabel.TabIndex = 8;
            this.serviceDescriptionLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label10.Location = new System.Drawing.Point(3, 84);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(88, 28);
            this.label10.TabIndex = 6;
            this.label10.Text = "Website:";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label8.Location = new System.Drawing.Point(3, 56);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(88, 28);
            this.label8.TabIndex = 4;
            this.label8.Text = "Author:";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label6.Location = new System.Drawing.Point(3, 28);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(88, 28);
            this.label6.TabIndex = 2;
            this.label6.Text = "Description:";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(88, 28);
            this.label1.TabIndex = 0;
            this.label1.Text = "Name:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // serviceNameLabel
            // 
            this.serviceNameLabel.AutoSize = true;
            this.serviceNameLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.serviceNameLabel.Location = new System.Drawing.Point(97, 0);
            this.serviceNameLabel.Name = "serviceNameLabel";
            this.serviceNameLabel.Size = new System.Drawing.Size(417, 28);
            this.serviceNameLabel.TabIndex = 7;
            this.serviceNameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // serviceWebsiteLabel
            // 
            this.serviceWebsiteLabel.AutoSize = true;
            this.serviceWebsiteLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.serviceWebsiteLabel.Location = new System.Drawing.Point(97, 84);
            this.serviceWebsiteLabel.Name = "serviceWebsiteLabel";
            this.serviceWebsiteLabel.Size = new System.Drawing.Size(417, 28);
            this.serviceWebsiteLabel.TabIndex = 10;
            this.serviceWebsiteLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.serviceWebsiteLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.serviceWebsiteLabel_LinkClicked);
            // 
            // openPluginDirButton
            // 
            this.openPluginDirButton.Location = new System.Drawing.Point(6, 449);
            this.openPluginDirButton.Name = "openPluginDirButton";
            this.openPluginDirButton.Size = new System.Drawing.Size(173, 23);
            this.openPluginDirButton.TabIndex = 2;
            this.openPluginDirButton.Text = "Open plugin directory";
            this.openPluginDirButton.UseVisualStyleBackColor = true;
            this.openPluginDirButton.Click += new System.EventHandler(this.openPluginDirButton_Click);
            // 
            // serviceUiPanel
            // 
            this.serviceUiPanel.Location = new System.Drawing.Point(185, 6);
            this.serviceUiPanel.Name = "serviceUiPanel";
            this.serviceUiPanel.Size = new System.Drawing.Size(517, 326);
            this.serviceUiPanel.TabIndex = 1;
            // 
            // servicesListBox
            // 
            this.servicesListBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.servicesListBox.FormattingEnabled = true;
            this.servicesListBox.ItemHeight = 15;
            this.servicesListBox.Location = new System.Drawing.Point(6, 6);
            this.servicesListBox.Name = "servicesListBox";
            this.servicesListBox.Size = new System.Drawing.Size(173, 437);
            this.servicesListBox.TabIndex = 0;
            this.servicesListBox.SelectedIndexChanged += new System.EventHandler(this.servicesListBox_SelectedIndexChanged);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AutoSize = true;
            this.flowLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flowLayoutPanel1.Controls.Add(this.saveButton);
            this.flowLayoutPanel1.Controls.Add(this.cancelButton);
            this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(546, 573);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(186, 33);
            this.flowLayoutPanel1.TabIndex = 1;
            // 
            // saveButton
            // 
            this.saveButton.Location = new System.Drawing.Point(96, 3);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(87, 27);
            this.saveButton.TabIndex = 0;
            this.saveButton.Text = "Save";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Location = new System.Drawing.Point(3, 3);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(87, 27);
            this.cancelButton.TabIndex = 1;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label7.Location = new System.Drawing.Point(3, 112);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(88, 22);
            this.label7.TabIndex = 11;
            this.label7.Text = "Version:";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // serviceVersionLabel
            // 
            this.serviceVersionLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.serviceVersionLabel.Location = new System.Drawing.Point(97, 112);
            this.serviceVersionLabel.Name = "serviceVersionLabel";
            this.serviceVersionLabel.Size = new System.Drawing.Size(417, 22);
            this.serviceVersionLabel.TabIndex = 12;
            this.serviceVersionLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // SettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(744, 618);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Controls.Add(this.tabControl1);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SettingsForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Settings";
            this.Load += new System.EventHandler(this.SettingsForm_Load);
            this.tabControl1.ResumeLayout(false);
            this.generalTab.ResumeLayout(false);
            this.generalTab.PerformLayout();
            this.pldOptionsGroupBox.ResumeLayout(false);
            this.pldOptionsGroupBox.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.flowLayoutPanel2.ResumeLayout(false);
            this.flowLayoutPanel2.PerformLayout();
            this.tabPage1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage generalTab;
        private System.Windows.Forms.TextBox pathFormatTextBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button saveLocBrowseButton;
        private System.Windows.Forms.Label saveLocLabel;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.FolderBrowserDialog mFolderBrowserDialog;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.ListBox servicesListBox;
        private System.Windows.Forms.Panel serviceUiPanel;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
        private System.Windows.Forms.RadioButton artworkSaveAsFileRadioButton;
        private System.Windows.Forms.RadioButton artworkSaveAsFormattedFileRadioButton;
        private System.Windows.Forms.RadioButton artworkDontSaveRadioButton;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton saveToRadioButton;
        private System.Windows.Forms.RadioButton askWhereToSaveRadioButton;
        private System.Windows.Forms.GroupBox pldOptionsGroupBox;
        private System.Windows.Forms.RadioButton pldAskWhereToSaveRadioButton;
        private System.Windows.Forms.RadioButton pldSaveToRadioButton;
        private System.Windows.Forms.Label pldSaveLocLabel;
        private System.Windows.Forms.TextBox pldPathFormatTextBox;
        private System.Windows.Forms.Button pldSaveLocBrowseButton;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox pldSameAsAlbumTrack;
        private System.Windows.Forms.Button formatHelpButton;
        private System.Windows.Forms.Button openPluginDirButton;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label serviceAuthorLabel;
        private System.Windows.Forms.Label serviceDescriptionLabel;
        private System.Windows.Forms.Label serviceNameLabel;
        private System.Windows.Forms.LinkLabel serviceWebsiteLabel;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox savePlaylistAsComboBox;
        private System.Windows.Forms.CheckBox confirmExitCheckBox;
        private System.Windows.Forms.CheckBox ignoreAlbumArtworkCheckBox;
        private System.Windows.Forms.Label compiledPathFormatLabel;
        private System.Windows.Forms.Label compiledPlaylistPathFormatLabel;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label serviceVersionLabel;
    }
}