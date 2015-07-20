/* 
Evil FOCA
Copyright (C) 2015 ElevenPaths

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/
namespace evilfoca
{
    partial class FormMain
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
            System.Windows.Forms.Panel panelIzquierda;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.lbFilter = new System.Windows.Forms.Label();
            this.tbSearchFilter = new System.Windows.Forms.TextBox();
            this.cMenuNeighbors = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addNeighborToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.panelButtons = new System.Windows.Forms.Panel();
            this.btAddNeighbor = new System.Windows.Forms.Button();
            this.btSearchRouters = new System.Windows.Forms.Button();
            this.btNetworkDiscovery = new System.Windows.Forms.Button();
            this.splitContainer4 = new System.Windows.Forms.SplitContainer();
            this.splitContainerHelp = new System.Windows.Forms.SplitContainer();
            this.panelMainOptions = new System.Windows.Forms.TabControl();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tabevilfoca = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.btStartNeighborSpoof = new System.Windows.Forms.Button();
            this.tabPage8 = new System.Windows.Forms.TabPage();
            this.label6 = new System.Windows.Forms.Label();
            this.tbSlaacMitmPrefix = new System.Windows.Forms.TextBox();
            this.btMitmSLAAC = new System.Windows.Forms.Button();
            this.tabPage9 = new System.Windows.Forms.TabPage();
            this.tbIPRange_IPv6DHCP = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.tbDNS_IPv6DHCP = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.btStart_IPv6DHCP = new System.Windows.Forms.Button();
            this.tabPage13 = new System.Windows.Forms.TabPage();
            this.btnWpadv6Attack = new System.Windows.Forms.Button();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.tabMitmIpv4 = new System.Windows.Forms.TabControl();
            this.tabPage7 = new System.Windows.Forms.TabPage();
            this.btStartARPSpoofing = new System.Windows.Forms.Button();
            this.tabPage11 = new System.Windows.Forms.TabPage();
            this.buttonEnableIPRouting = new System.Windows.Forms.Button();
            this.labelIPRouting = new System.Windows.Forms.Label();
            this.textBoxDHCPAckMAC = new System.Windows.Forms.TextBox();
            this.textBoxDNS = new System.Windows.Forms.TextBox();
            this.textBoxGateway = new System.Windows.Forms.TextBox();
            this.labelFakeGateway = new System.Windows.Forms.Label();
            this.labelMAC = new System.Windows.Forms.Label();
            this.radioButtonComputerWithMAC = new System.Windows.Forms.RadioButton();
            this.radioButtonAllcomputers = new System.Windows.Forms.RadioButton();
            this.checkBoxUseAsGateway = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.buttonDHCPACKInjection = new System.Windows.Forms.Button();
            this.tabPage14 = new System.Windows.Forms.TabPage();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnWpadAttack = new System.Windows.Forms.Button();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.tabDoSIpv6 = new System.Windows.Forms.TabControl();
            this.tabPage10 = new System.Windows.Forms.TabPage();
            this.buttonStartSLAACDoS = new System.Windows.Forms.Button();
            this.tabPage6 = new System.Windows.Forms.TabPage();
            this.tabDoSIpv4 = new System.Windows.Forms.TabControl();
            this.tabPage12 = new System.Windows.Forms.TabPage();
            this.pbInvMacSpoofedIp = new System.Windows.Forms.PictureBox();
            this.pbInvMacSpoofTarget = new System.Windows.Forms.PictureBox();
            this.tbIpSpoofedIpv4 = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.tbTargetSpoofedIpv4 = new System.Windows.Forms.TextBox();
            this.btStartInvalidMacSpoof = new System.Windows.Forms.Button();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lbAdvertisement = new System.Windows.Forms.Label();
            this.btAddDNSHijack = new System.Windows.Forms.Button();
            this.tbDomainDNSHijack = new System.Windows.Forms.TextBox();
            this.cbWildcardDnsHijack = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tbIpDNSHijack = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.imgListTrick = new System.Windows.Forms.ImageList(this.components);
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.configurationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.interfaceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.treeView = new evilfoca.BufferedTreeView();
            this.panelNeighborAdvSpoof = new evilfoca.PanelTargets();
            this.panelTargetSLAACMitm = new evilfoca.PanelTarget();
            this.wpadv6TargetPanel = new evilfoca.PanelTarget();
            this.panelMitmIpv4 = new evilfoca.PanelTargets();
            this.wpadTargetPanel = new evilfoca.PanelTarget();
            this.panelTargetSLAACDoS = new evilfoca.PanelTarget();
            this.help = new evilfoca.ControlHelp.ControlHelp();
            this.listViewExAttacks = new evilfoca.Controls.ListViewEx.ListViewEx();
            this.attackType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.attack = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Active = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.panelLogs = new evilfoca.PanelLogs();
            panelIzquierda = new System.Windows.Forms.Panel();
            panelIzquierda.SuspendLayout();
            this.cMenuNeighbors.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.panelButtons.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer4)).BeginInit();
            this.splitContainer4.Panel1.SuspendLayout();
            this.splitContainer4.Panel2.SuspendLayout();
            this.splitContainer4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerHelp)).BeginInit();
            this.splitContainerHelp.Panel1.SuspendLayout();
            this.splitContainerHelp.Panel2.SuspendLayout();
            this.splitContainerHelp.SuspendLayout();
            this.panelMainOptions.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabevilfoca.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage8.SuspendLayout();
            this.tabPage9.SuspendLayout();
            this.tabPage13.SuspendLayout();
            this.tabPage5.SuspendLayout();
            this.tabMitmIpv4.SuspendLayout();
            this.tabPage7.SuspendLayout();
            this.tabPage11.SuspendLayout();
            this.tabPage14.SuspendLayout();
            this.panel2.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.tabDoSIpv6.SuspendLayout();
            this.tabPage10.SuspendLayout();
            this.tabPage6.SuspendLayout();
            this.tabDoSIpv4.SuspendLayout();
            this.tabPage12.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbInvMacSpoofedIp)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbInvMacSpoofTarget)).BeginInit();
            this.tabPage3.SuspendLayout();
            this.panel1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelIzquierda
            // 
            panelIzquierda.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            panelIzquierda.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            panelIzquierda.Controls.Add(this.lbFilter);
            panelIzquierda.Controls.Add(this.tbSearchFilter);
            panelIzquierda.Controls.Add(this.treeView);
            panelIzquierda.Location = new System.Drawing.Point(0, 0);
            panelIzquierda.Name = "panelIzquierda";
            panelIzquierda.Size = new System.Drawing.Size(263, 409);
            panelIzquierda.TabIndex = 6;
            // 
            // lbFilter
            // 
            this.lbFilter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lbFilter.AutoSize = true;
            this.lbFilter.BackColor = System.Drawing.Color.White;
            this.lbFilter.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbFilter.Location = new System.Drawing.Point(222, 389);
            this.lbFilter.Name = "lbFilter";
            this.lbFilter.Size = new System.Drawing.Size(35, 13);
            this.lbFilter.TabIndex = 3;
            this.lbFilter.Text = "[Filter]";
            this.lbFilter.Click += new System.EventHandler(this.lbFilter_Click);
            // 
            // tbSearchFilter
            // 
            this.tbSearchFilter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbSearchFilter.AutoCompleteCustomSource.AddRange(new string[] {
            "hola",
            "adios",
            "eeeeeeeee"});
            this.tbSearchFilter.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
            this.tbSearchFilter.Location = new System.Drawing.Point(-1, 386);
            this.tbSearchFilter.Name = "tbSearchFilter";
            this.tbSearchFilter.Size = new System.Drawing.Size(263, 20);
            this.tbSearchFilter.TabIndex = 0;
            this.tbSearchFilter.TextChanged += new System.EventHandler(this.tbSearchFilter_TextChanged);
            this.tbSearchFilter.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbSearchFilter_KeyPress);
            // 
            // cMenuNeighbors
            // 
            this.cMenuNeighbors.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addNeighborToolStripMenuItem,
            this.copyToolStripMenuItem});
            this.cMenuNeighbors.Name = "cMenuNeighbors";
            this.cMenuNeighbors.Size = new System.Drawing.Size(148, 48);
            this.cMenuNeighbors.Opening += new System.ComponentModel.CancelEventHandler(this.cMenuNeighbors_Opening);
            // 
            // addNeighborToolStripMenuItem
            // 
            this.addNeighborToolStripMenuItem.Image = global::evilfoca.Properties.Resources.AddNeighborSmall;
            this.addNeighborToolStripMenuItem.Name = "addNeighborToolStripMenuItem";
            this.addNeighborToolStripMenuItem.Size = new System.Drawing.Size(147, 22);
            this.addNeighborToolStripMenuItem.Text = "&Add neighbor";
            this.addNeighborToolStripMenuItem.Click += new System.EventHandler(this.addNeighborToolStripMenuItem_Click);
            // 
            // copyToolStripMenuItem
            // 
            this.copyToolStripMenuItem.Image = global::evilfoca.Properties.Resources.Copy;
            this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
            this.copyToolStripMenuItem.Size = new System.Drawing.Size(147, 22);
            this.copyToolStripMenuItem.Text = "&Copy";
            this.copyToolStripMenuItem.Click += new System.EventHandler(this.copyToolStripMenuItem_Click);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "ethernet-card-Vista.ico");
            this.imageList1.Images.SetKeyName(1, "signal-Vista.ico");
            this.imageList1.Images.SetKeyName(2, "globe-Vista.ico");
            this.imageList1.Images.SetKeyName(3, "home.ico");
            this.imageList1.Images.SetKeyName(4, "computer.gif");
            this.imageList1.Images.SetKeyName(5, "reverse.png");
            this.imageList1.Images.SetKeyName(6, "ARP.gif");
            this.imageList1.Images.SetKeyName(7, "Network-Connections-black-red.ico");
            this.imageList1.Images.SetKeyName(8, "Network-Connections-black-red-64.png");
            this.imageList1.Images.SetKeyName(9, "windows.gif");
            this.imageList1.Images.SetKeyName(10, "linux.png");
            this.imageList1.Images.SetKeyName(11, "monitor.png");
            this.imageList1.Images.SetKeyName(12, "arrow_switch.png");
            this.imageList1.Images.SetKeyName(13, "chart_organisation.png");
            this.imageList1.Images.SetKeyName(14, "bullet_green.png");
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 24);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.splitContainer1);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.panelLogs);
            this.splitContainer2.Panel2MinSize = 150;
            this.splitContainer2.Size = new System.Drawing.Size(954, 638);
            this.splitContainer2.SplitterDistance = 472;
            this.splitContainer2.TabIndex = 11;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(3, 3);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(panelIzquierda);
            this.splitContainer1.Panel1.Controls.Add(this.panelButtons);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer4);
            this.splitContainer1.Size = new System.Drawing.Size(954, 466);
            this.splitContainer1.SplitterDistance = 263;
            this.splitContainer1.SplitterWidth = 1;
            this.splitContainer1.TabIndex = 9;
            // 
            // panelButtons
            // 
            this.panelButtons.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelButtons.BackColor = System.Drawing.Color.White;
            this.panelButtons.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelButtons.Controls.Add(this.btAddNeighbor);
            this.panelButtons.Controls.Add(this.btSearchRouters);
            this.panelButtons.Controls.Add(this.btNetworkDiscovery);
            this.panelButtons.Location = new System.Drawing.Point(0, 410);
            this.panelButtons.Name = "panelButtons";
            this.panelButtons.Size = new System.Drawing.Size(263, 54);
            this.panelButtons.TabIndex = 0;
            // 
            // btAddNeighbor
            // 
            this.btAddNeighbor.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btAddNeighbor.FlatAppearance.BorderSize = 0;
            this.btAddNeighbor.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btAddNeighbor.Image = global::evilfoca.Properties.Resources.AddNeighbor;
            this.btAddNeighbor.Location = new System.Drawing.Point(211, 3);
            this.btAddNeighbor.Name = "btAddNeighbor";
            this.btAddNeighbor.Size = new System.Drawing.Size(44, 44);
            this.btAddNeighbor.TabIndex = 6;
            this.toolTip.SetToolTip(this.btAddNeighbor, "Add neighbor");
            this.btAddNeighbor.UseVisualStyleBackColor = true;
            this.btAddNeighbor.Click += new System.EventHandler(this.btAddNeighbor_Click);
            // 
            // btSearchRouters
            // 
            this.btSearchRouters.FlatAppearance.BorderSize = 0;
            this.btSearchRouters.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btSearchRouters.Image = global::evilfoca.Properties.Resources.SearchRouters;
            this.btSearchRouters.Location = new System.Drawing.Point(56, 3);
            this.btSearchRouters.Name = "btSearchRouters";
            this.btSearchRouters.Size = new System.Drawing.Size(44, 44);
            this.btSearchRouters.TabIndex = 5;
            this.toolTip.SetToolTip(this.btSearchRouters, "Search for routers");
            this.btSearchRouters.UseVisualStyleBackColor = true;
            this.btSearchRouters.Click += new System.EventHandler(this.btSearchRouters_Click);
            // 
            // btNetworkDiscovery
            // 
            this.btNetworkDiscovery.FlatAppearance.BorderSize = 0;
            this.btNetworkDiscovery.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btNetworkDiscovery.Image = global::evilfoca.Properties.Resources.Search;
            this.btNetworkDiscovery.Location = new System.Drawing.Point(3, 3);
            this.btNetworkDiscovery.Name = "btNetworkDiscovery";
            this.btNetworkDiscovery.Size = new System.Drawing.Size(44, 44);
            this.btNetworkDiscovery.TabIndex = 3;
            this.toolTip.SetToolTip(this.btNetworkDiscovery, "Discover new neighbors");
            this.btNetworkDiscovery.UseVisualStyleBackColor = true;
            this.btNetworkDiscovery.Click += new System.EventHandler(this.btNetworkDiscovery_Click);
            // 
            // splitContainer4
            // 
            this.splitContainer4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer4.Location = new System.Drawing.Point(0, 0);
            this.splitContainer4.Name = "splitContainer4";
            this.splitContainer4.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer4.Panel1
            // 
            this.splitContainer4.Panel1.Controls.Add(this.splitContainerHelp);
            // 
            // splitContainer4.Panel2
            // 
            this.splitContainer4.Panel2.Controls.Add(this.listViewExAttacks);
            this.splitContainer4.Size = new System.Drawing.Size(690, 466);
            this.splitContainer4.SplitterDistance = 291;
            this.splitContainer4.TabIndex = 0;
            // 
            // splitContainerHelp
            // 
            this.splitContainerHelp.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerHelp.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainerHelp.IsSplitterFixed = true;
            this.splitContainerHelp.Location = new System.Drawing.Point(0, 0);
            this.splitContainerHelp.Name = "splitContainerHelp";
            // 
            // splitContainerHelp.Panel1
            // 
            this.splitContainerHelp.Panel1.Controls.Add(this.panelMainOptions);
            // 
            // splitContainerHelp.Panel2
            // 
            this.splitContainerHelp.Panel2.Controls.Add(this.help);
            this.splitContainerHelp.Panel2.Padding = new System.Windows.Forms.Padding(0, 20, 3, 0);
            this.splitContainerHelp.Size = new System.Drawing.Size(690, 291);
            this.splitContainerHelp.SplitterDistance = 501;
            this.splitContainerHelp.TabIndex = 0;
            // 
            // panelMainOptions
            // 
            this.panelMainOptions.Controls.Add(this.tabPage2);
            this.panelMainOptions.Controls.Add(this.tabPage5);
            this.panelMainOptions.Controls.Add(this.tabPage4);
            this.panelMainOptions.Controls.Add(this.tabPage6);
            this.panelMainOptions.Controls.Add(this.tabPage3);
            this.panelMainOptions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMainOptions.ImageList = this.imageList1;
            this.panelMainOptions.Location = new System.Drawing.Point(0, 0);
            this.panelMainOptions.Name = "panelMainOptions";
            this.panelMainOptions.SelectedIndex = 0;
            this.panelMainOptions.Size = new System.Drawing.Size(501, 291);
            this.panelMainOptions.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.panelMainOptions.TabIndex = 0;
            this.panelMainOptions.Selected += new System.Windows.Forms.TabControlEventHandler(this.panelMainOptions_Selected);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.tabevilfoca);
            this.tabPage2.Location = new System.Drawing.Point(4, 23);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(493, 264);
            this.tabPage2.TabIndex = 0;
            this.tabPage2.Text = "MITM IPv6";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // tabevilfoca
            // 
            this.tabevilfoca.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabevilfoca.Controls.Add(this.tabPage1);
            this.tabevilfoca.Controls.Add(this.tabPage8);
            this.tabevilfoca.Controls.Add(this.tabPage9);
            this.tabevilfoca.Controls.Add(this.tabPage13);
            this.tabevilfoca.Location = new System.Drawing.Point(3, 3);
            this.tabevilfoca.Name = "tabevilfoca";
            this.tabevilfoca.SelectedIndex = 0;
            this.tabevilfoca.Size = new System.Drawing.Size(487, 258);
            this.tabevilfoca.TabIndex = 6;
            this.tabevilfoca.SelectedIndexChanged += new System.EventHandler(this.tabMitm_SelectedIndexChanged);
            this.tabevilfoca.Selected += new System.Windows.Forms.TabControlEventHandler(this.tabMitm_Selected);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.panelNeighborAdvSpoof);
            this.tabPage1.Controls.Add(this.btStartNeighborSpoof);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(479, 232);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Neighbor advertisement spoofing";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // btStartNeighborSpoof
            // 
            this.btStartNeighborSpoof.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btStartNeighborSpoof.Image = global::evilfoca.Properties.Resources.Start;
            this.btStartNeighborSpoof.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btStartNeighborSpoof.Location = new System.Drawing.Point(3, 200);
            this.btStartNeighborSpoof.Name = "btStartNeighborSpoof";
            this.btStartNeighborSpoof.Size = new System.Drawing.Size(70, 25);
            this.btStartNeighborSpoof.TabIndex = 3;
            this.btStartNeighborSpoof.Text = "Start";
            this.btStartNeighborSpoof.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btStartNeighborSpoof.UseVisualStyleBackColor = true;
            this.btStartNeighborSpoof.Click += new System.EventHandler(this.btStartNeighborSpoof_Click);
            // 
            // tabPage8
            // 
            this.tabPage8.Controls.Add(this.label6);
            this.tabPage8.Controls.Add(this.tbSlaacMitmPrefix);
            this.tabPage8.Controls.Add(this.btMitmSLAAC);
            this.tabPage8.Controls.Add(this.panelTargetSLAACMitm);
            this.tabPage8.Location = new System.Drawing.Point(4, 22);
            this.tabPage8.Name = "tabPage8";
            this.tabPage8.Size = new System.Drawing.Size(479, 232);
            this.tabPage8.TabIndex = 1;
            this.tabPage8.Text = "SLAAC";
            this.tabPage8.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(20, 15);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(106, 13);
            this.label6.TabIndex = 42;
            this.label6.Text = "Targets Prefix (IPv6):";
            // 
            // tbSlaacMitmPrefix
            // 
            this.tbSlaacMitmPrefix.Location = new System.Drawing.Point(155, 15);
            this.tbSlaacMitmPrefix.Name = "tbSlaacMitmPrefix";
            this.tbSlaacMitmPrefix.Size = new System.Drawing.Size(159, 20);
            this.tbSlaacMitmPrefix.TabIndex = 41;
            // 
            // btMitmSLAAC
            // 
            this.btMitmSLAAC.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btMitmSLAAC.Image = global::evilfoca.Properties.Resources.Start;
            this.btMitmSLAAC.Location = new System.Drawing.Point(3, 200);
            this.btMitmSLAAC.Name = "btMitmSLAAC";
            this.btMitmSLAAC.Size = new System.Drawing.Size(70, 25);
            this.btMitmSLAAC.TabIndex = 35;
            this.btMitmSLAAC.Text = "Start";
            this.btMitmSLAAC.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btMitmSLAAC.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btMitmSLAAC.UseVisualStyleBackColor = true;
            this.btMitmSLAAC.Click += new System.EventHandler(this.btMitmSLAAC_Click);
            // 
            // tabPage9
            // 
            this.tabPage9.Controls.Add(this.tbIPRange_IPv6DHCP);
            this.tabPage9.Controls.Add(this.label9);
            this.tabPage9.Controls.Add(this.tbDNS_IPv6DHCP);
            this.tabPage9.Controls.Add(this.label7);
            this.tabPage9.Controls.Add(this.btStart_IPv6DHCP);
            this.tabPage9.Location = new System.Drawing.Point(4, 22);
            this.tabPage9.Name = "tabPage9";
            this.tabPage9.Size = new System.Drawing.Size(479, 232);
            this.tabPage9.TabIndex = 2;
            this.tabPage9.Text = "DHCPv6";
            this.tabPage9.UseVisualStyleBackColor = true;
            // 
            // tbIPRange_IPv6DHCP
            // 
            this.tbIPRange_IPv6DHCP.Location = new System.Drawing.Point(127, 47);
            this.tbIPRange_IPv6DHCP.Name = "tbIPRange_IPv6DHCP";
            this.tbIPRange_IPv6DHCP.Size = new System.Drawing.Size(232, 20);
            this.tbIPRange_IPv6DHCP.TabIndex = 36;
            this.tbIPRange_IPv6DHCP.Text = "fc00:5::0";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(34, 50);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(82, 13);
            this.label9.TabIndex = 35;
            this.label9.Text = "Fake IP Range:";
            // 
            // tbDNS_IPv6DHCP
            // 
            this.tbDNS_IPv6DHCP.Location = new System.Drawing.Point(127, 21);
            this.tbDNS_IPv6DHCP.Name = "tbDNS_IPv6DHCP";
            this.tbDNS_IPv6DHCP.Size = new System.Drawing.Size(232, 20);
            this.tbDNS_IPv6DHCP.TabIndex = 33;
            this.tbDNS_IPv6DHCP.Text = "ca:ca:ca:ca:ca:ca:ca:10";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(34, 24);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(60, 13);
            this.label7.TabIndex = 30;
            this.label7.Text = "Fake DNS:";
            // 
            // btStart_IPv6DHCP
            // 
            this.btStart_IPv6DHCP.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btStart_IPv6DHCP.Image = global::evilfoca.Properties.Resources.Start;
            this.btStart_IPv6DHCP.Location = new System.Drawing.Point(3, 200);
            this.btStart_IPv6DHCP.Name = "btStart_IPv6DHCP";
            this.btStart_IPv6DHCP.Size = new System.Drawing.Size(70, 25);
            this.btStart_IPv6DHCP.TabIndex = 34;
            this.btStart_IPv6DHCP.Text = "Start";
            this.btStart_IPv6DHCP.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btStart_IPv6DHCP.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btStart_IPv6DHCP.UseVisualStyleBackColor = true;
            this.btStart_IPv6DHCP.Click += new System.EventHandler(this.btStart_IPv6DHCP_Click);
            // 
            // tabPage13
            // 
            this.tabPage13.Controls.Add(this.btnWpadv6Attack);
            this.tabPage13.Controls.Add(this.wpadv6TargetPanel);
            this.tabPage13.Location = new System.Drawing.Point(4, 22);
            this.tabPage13.Name = "tabPage13";
            this.tabPage13.Size = new System.Drawing.Size(479, 232);
            this.tabPage13.TabIndex = 3;
            this.tabPage13.Text = "WPADv6";
            this.tabPage13.UseVisualStyleBackColor = true;
            // 
            // btnWpadv6Attack
            // 
            this.btnWpadv6Attack.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnWpadv6Attack.Image = global::evilfoca.Properties.Resources.Start;
            this.btnWpadv6Attack.Location = new System.Drawing.Point(3, 200);
            this.btnWpadv6Attack.Name = "btnWpadv6Attack";
            this.btnWpadv6Attack.Size = new System.Drawing.Size(70, 25);
            this.btnWpadv6Attack.TabIndex = 36;
            this.btnWpadv6Attack.Text = "Start";
            this.btnWpadv6Attack.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnWpadv6Attack.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnWpadv6Attack.UseVisualStyleBackColor = true;
            this.btnWpadv6Attack.Click += new System.EventHandler(this.btnWpadv6Attack_Click);
            // 
            // tabPage5
            // 
            this.tabPage5.Controls.Add(this.tabMitmIpv4);
            this.tabPage5.Location = new System.Drawing.Point(4, 23);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage5.Size = new System.Drawing.Size(493, 264);
            this.tabPage5.TabIndex = 2;
            this.tabPage5.Text = "MITM IPv4";
            this.tabPage5.UseVisualStyleBackColor = true;
            // 
            // tabMitmIpv4
            // 
            this.tabMitmIpv4.Controls.Add(this.tabPage7);
            this.tabMitmIpv4.Controls.Add(this.tabPage11);
            this.tabMitmIpv4.Controls.Add(this.tabPage14);
            this.tabMitmIpv4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabMitmIpv4.Location = new System.Drawing.Point(3, 3);
            this.tabMitmIpv4.Name = "tabMitmIpv4";
            this.tabMitmIpv4.SelectedIndex = 0;
            this.tabMitmIpv4.Size = new System.Drawing.Size(487, 258);
            this.tabMitmIpv4.TabIndex = 0;
            this.tabMitmIpv4.SelectedIndexChanged += new System.EventHandler(this.tabMitmIpv4_SelectedIndexChanged);
            this.tabMitmIpv4.Selected += new System.Windows.Forms.TabControlEventHandler(this.tabMitmIpv4_Selected);
            // 
            // tabPage7
            // 
            this.tabPage7.Controls.Add(this.panelMitmIpv4);
            this.tabPage7.Controls.Add(this.btStartARPSpoofing);
            this.tabPage7.Location = new System.Drawing.Point(4, 22);
            this.tabPage7.Name = "tabPage7";
            this.tabPage7.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage7.Size = new System.Drawing.Size(479, 232);
            this.tabPage7.TabIndex = 0;
            this.tabPage7.Text = "ARP spoofing";
            this.tabPage7.UseVisualStyleBackColor = true;
            // 
            // btStartARPSpoofing
            // 
            this.btStartARPSpoofing.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btStartARPSpoofing.Image = global::evilfoca.Properties.Resources.Start;
            this.btStartARPSpoofing.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btStartARPSpoofing.Location = new System.Drawing.Point(3, 200);
            this.btStartARPSpoofing.Name = "btStartARPSpoofing";
            this.btStartARPSpoofing.Size = new System.Drawing.Size(70, 25);
            this.btStartARPSpoofing.TabIndex = 4;
            this.btStartARPSpoofing.Text = "Start";
            this.btStartARPSpoofing.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btStartARPSpoofing.UseVisualStyleBackColor = true;
            this.btStartARPSpoofing.Click += new System.EventHandler(this.btStartARPSpoofing_Click);
            // 
            // tabPage11
            // 
            this.tabPage11.Controls.Add(this.buttonEnableIPRouting);
            this.tabPage11.Controls.Add(this.labelIPRouting);
            this.tabPage11.Controls.Add(this.textBoxDHCPAckMAC);
            this.tabPage11.Controls.Add(this.textBoxDNS);
            this.tabPage11.Controls.Add(this.textBoxGateway);
            this.tabPage11.Controls.Add(this.labelFakeGateway);
            this.tabPage11.Controls.Add(this.labelMAC);
            this.tabPage11.Controls.Add(this.radioButtonComputerWithMAC);
            this.tabPage11.Controls.Add(this.radioButtonAllcomputers);
            this.tabPage11.Controls.Add(this.checkBoxUseAsGateway);
            this.tabPage11.Controls.Add(this.label4);
            this.tabPage11.Controls.Add(this.buttonDHCPACKInjection);
            this.tabPage11.Location = new System.Drawing.Point(4, 22);
            this.tabPage11.Name = "tabPage11";
            this.tabPage11.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage11.Size = new System.Drawing.Size(479, 232);
            this.tabPage11.TabIndex = 1;
            this.tabPage11.Text = "DHCP ACK Injection";
            this.tabPage11.UseVisualStyleBackColor = true;
            // 
            // buttonEnableIPRouting
            // 
            this.buttonEnableIPRouting.Location = new System.Drawing.Point(215, 124);
            this.buttonEnableIPRouting.Name = "buttonEnableIPRouting";
            this.buttonEnableIPRouting.Size = new System.Drawing.Size(135, 23);
            this.buttonEnableIPRouting.TabIndex = 33;
            this.buttonEnableIPRouting.Text = "Enable IP Routing";
            this.buttonEnableIPRouting.UseVisualStyleBackColor = true;
            this.buttonEnableIPRouting.Click += new System.EventHandler(this.buttonEnableIPRouting_Click);
            // 
            // labelIPRouting
            // 
            this.labelIPRouting.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelIPRouting.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelIPRouting.ForeColor = System.Drawing.Color.Red;
            this.labelIPRouting.Location = new System.Drawing.Point(60, 95);
            this.labelIPRouting.Name = "labelIPRouting";
            this.labelIPRouting.Size = new System.Drawing.Size(390, 57);
            this.labelIPRouting.TabIndex = 32;
            this.labelIPRouting.Text = "To use this computer as gateway you need to activate Windows IP Routing, do you w" +
    "ant to activate it?";
            // 
            // textBoxDHCPAckMAC
            // 
            this.textBoxDHCPAckMAC.Enabled = false;
            this.textBoxDHCPAckMAC.Location = new System.Drawing.Point(320, 68);
            this.textBoxDHCPAckMAC.Name = "textBoxDHCPAckMAC";
            this.textBoxDHCPAckMAC.Size = new System.Drawing.Size(130, 20);
            this.textBoxDHCPAckMAC.TabIndex = 30;
            // 
            // textBoxDNS
            // 
            this.textBoxDNS.Location = new System.Drawing.Point(150, 23);
            this.textBoxDNS.Name = "textBoxDNS";
            this.textBoxDNS.Size = new System.Drawing.Size(111, 20);
            this.textBoxDNS.TabIndex = 29;
            this.textBoxDNS.Text = "8.8.8.8";
            // 
            // textBoxGateway
            // 
            this.textBoxGateway.Enabled = false;
            this.textBoxGateway.Location = new System.Drawing.Point(150, 49);
            this.textBoxGateway.Name = "textBoxGateway";
            this.textBoxGateway.Size = new System.Drawing.Size(111, 20);
            this.textBoxGateway.TabIndex = 28;
            this.textBoxGateway.Text = "192.168.0.253";
            // 
            // labelFakeGateway
            // 
            this.labelFakeGateway.AutoSize = true;
            this.labelFakeGateway.Enabled = false;
            this.labelFakeGateway.Location = new System.Drawing.Point(57, 52);
            this.labelFakeGateway.Name = "labelFakeGateway";
            this.labelFakeGateway.Size = new System.Drawing.Size(77, 13);
            this.labelFakeGateway.TabIndex = 27;
            this.labelFakeGateway.Text = "Fake gateway:";
            // 
            // labelMAC
            // 
            this.labelMAC.AutoSize = true;
            this.labelMAC.Enabled = false;
            this.labelMAC.Location = new System.Drawing.Point(281, 71);
            this.labelMAC.Name = "labelMAC";
            this.labelMAC.Size = new System.Drawing.Size(33, 13);
            this.labelMAC.TabIndex = 25;
            this.labelMAC.Text = "MAC:";
            // 
            // radioButtonComputerWithMAC
            // 
            this.radioButtonComputerWithMAC.AutoSize = true;
            this.radioButtonComputerWithMAC.Location = new System.Drawing.Point(280, 45);
            this.radioButtonComputerWithMAC.Name = "radioButtonComputerWithMAC";
            this.radioButtonComputerWithMAC.Size = new System.Drawing.Size(170, 17);
            this.radioButtonComputerWithMAC.TabIndex = 22;
            this.radioButtonComputerWithMAC.Text = "Attack computer with this MAC";
            this.radioButtonComputerWithMAC.UseVisualStyleBackColor = true;
            this.radioButtonComputerWithMAC.CheckedChanged += new System.EventHandler(this.radioButtonComputerWithMAC_CheckedChanged);
            // 
            // radioButtonAllcomputers
            // 
            this.radioButtonAllcomputers.AutoSize = true;
            this.radioButtonAllcomputers.Checked = true;
            this.radioButtonAllcomputers.Location = new System.Drawing.Point(280, 22);
            this.radioButtonAllcomputers.Name = "radioButtonAllcomputers";
            this.radioButtonAllcomputers.Size = new System.Drawing.Size(121, 17);
            this.radioButtonAllcomputers.TabIndex = 21;
            this.radioButtonAllcomputers.TabStop = true;
            this.radioButtonAllcomputers.Text = "Attack all computers";
            this.radioButtonAllcomputers.UseVisualStyleBackColor = true;
            // 
            // checkBoxUseAsGateway
            // 
            this.checkBoxUseAsGateway.AutoSize = true;
            this.checkBoxUseAsGateway.Checked = true;
            this.checkBoxUseAsGateway.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxUseAsGateway.Location = new System.Drawing.Point(60, 75);
            this.checkBoxUseAsGateway.Name = "checkBoxUseAsGateway";
            this.checkBoxUseAsGateway.Size = new System.Drawing.Size(168, 17);
            this.checkBoxUseAsGateway.TabIndex = 18;
            this.checkBoxUseAsGateway.Text = "Use this computer as gateway";
            this.checkBoxUseAsGateway.UseVisualStyleBackColor = true;
            this.checkBoxUseAsGateway.CheckedChanged += new System.EventHandler(this.checkBoxUseAsGateway_CheckedChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(57, 26);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(60, 13);
            this.label4.TabIndex = 15;
            this.label4.Text = "Fake DNS:";
            // 
            // buttonDHCPACKInjection
            // 
            this.buttonDHCPACKInjection.Image = global::evilfoca.Properties.Resources.Start;
            this.buttonDHCPACKInjection.Location = new System.Drawing.Point(60, 155);
            this.buttonDHCPACKInjection.Name = "buttonDHCPACKInjection";
            this.buttonDHCPACKInjection.Size = new System.Drawing.Size(70, 25);
            this.buttonDHCPACKInjection.TabIndex = 31;
            this.buttonDHCPACKInjection.Text = "Start";
            this.buttonDHCPACKInjection.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.buttonDHCPACKInjection.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.buttonDHCPACKInjection.UseVisualStyleBackColor = true;
            this.buttonDHCPACKInjection.Click += new System.EventHandler(this.buttonDHCPACKInjection_Click);
            // 
            // tabPage14
            // 
            this.tabPage14.Controls.Add(this.panel2);
            this.tabPage14.Location = new System.Drawing.Point(4, 22);
            this.tabPage14.Name = "tabPage14";
            this.tabPage14.Size = new System.Drawing.Size(479, 232);
            this.tabPage14.TabIndex = 2;
            this.tabPage14.Text = "WPAD";
            this.tabPage14.UseVisualStyleBackColor = true;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.wpadTargetPanel);
            this.panel2.Controls.Add(this.btnWpadAttack);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(479, 232);
            this.panel2.TabIndex = 8;
            // 
            // btnWpadAttack
            // 
            this.btnWpadAttack.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnWpadAttack.Image = global::evilfoca.Properties.Resources.Start;
            this.btnWpadAttack.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnWpadAttack.Location = new System.Drawing.Point(3, 200);
            this.btnWpadAttack.Name = "btnWpadAttack";
            this.btnWpadAttack.Size = new System.Drawing.Size(70, 25);
            this.btnWpadAttack.TabIndex = 7;
            this.btnWpadAttack.Text = "Start";
            this.btnWpadAttack.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnWpadAttack.UseVisualStyleBackColor = true;
            this.btnWpadAttack.Click += new System.EventHandler(this.btnWpadAttack_Click);
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.tabDoSIpv6);
            this.tabPage4.Location = new System.Drawing.Point(4, 23);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(493, 264);
            this.tabPage4.TabIndex = 1;
            this.tabPage4.Text = "DoS IPv6";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // tabDoSIpv6
            // 
            this.tabDoSIpv6.Controls.Add(this.tabPage10);
            this.tabDoSIpv6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabDoSIpv6.Location = new System.Drawing.Point(3, 3);
            this.tabDoSIpv6.Name = "tabDoSIpv6";
            this.tabDoSIpv6.SelectedIndex = 0;
            this.tabDoSIpv6.Size = new System.Drawing.Size(487, 258);
            this.tabDoSIpv6.TabIndex = 1;
            this.tabDoSIpv6.Selected += new System.Windows.Forms.TabControlEventHandler(this.tabDoSIpv6_Selected);
            // 
            // tabPage10
            // 
            this.tabPage10.Controls.Add(this.panelTargetSLAACDoS);
            this.tabPage10.Controls.Add(this.buttonStartSLAACDoS);
            this.tabPage10.Location = new System.Drawing.Point(4, 22);
            this.tabPage10.Name = "tabPage10";
            this.tabPage10.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage10.Size = new System.Drawing.Size(479, 232);
            this.tabPage10.TabIndex = 0;
            this.tabPage10.Text = "SLAAC DoS";
            this.tabPage10.UseVisualStyleBackColor = true;
            // 
            // buttonStartSLAACDoS
            // 
            this.buttonStartSLAACDoS.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonStartSLAACDoS.Image = global::evilfoca.Properties.Resources.Start;
            this.buttonStartSLAACDoS.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.buttonStartSLAACDoS.Location = new System.Drawing.Point(3, 200);
            this.buttonStartSLAACDoS.Name = "buttonStartSLAACDoS";
            this.buttonStartSLAACDoS.Size = new System.Drawing.Size(70, 25);
            this.buttonStartSLAACDoS.TabIndex = 4;
            this.buttonStartSLAACDoS.Text = "Start";
            this.buttonStartSLAACDoS.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.buttonStartSLAACDoS.UseVisualStyleBackColor = true;
            this.buttonStartSLAACDoS.Click += new System.EventHandler(this.buttonStartSLAACDoS_Click);
            // 
            // tabPage6
            // 
            this.tabPage6.Controls.Add(this.tabDoSIpv4);
            this.tabPage6.Location = new System.Drawing.Point(4, 23);
            this.tabPage6.Name = "tabPage6";
            this.tabPage6.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage6.Size = new System.Drawing.Size(493, 264);
            this.tabPage6.TabIndex = 3;
            this.tabPage6.Text = "DoS IPv4";
            this.tabPage6.UseVisualStyleBackColor = true;
            // 
            // tabDoSIpv4
            // 
            this.tabDoSIpv4.Controls.Add(this.tabPage12);
            this.tabDoSIpv4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabDoSIpv4.Location = new System.Drawing.Point(3, 3);
            this.tabDoSIpv4.Name = "tabDoSIpv4";
            this.tabDoSIpv4.SelectedIndex = 0;
            this.tabDoSIpv4.Size = new System.Drawing.Size(487, 258);
            this.tabDoSIpv4.TabIndex = 0;
            this.tabDoSIpv4.Selected += new System.Windows.Forms.TabControlEventHandler(this.tabDoSIpv4_Selected);
            // 
            // tabPage12
            // 
            this.tabPage12.Controls.Add(this.pbInvMacSpoofedIp);
            this.tabPage12.Controls.Add(this.pbInvMacSpoofTarget);
            this.tabPage12.Controls.Add(this.tbIpSpoofedIpv4);
            this.tabPage12.Controls.Add(this.label5);
            this.tabPage12.Controls.Add(this.label1);
            this.tabPage12.Controls.Add(this.tbTargetSpoofedIpv4);
            this.tabPage12.Controls.Add(this.btStartInvalidMacSpoof);
            this.tabPage12.Location = new System.Drawing.Point(4, 22);
            this.tabPage12.Name = "tabPage12";
            this.tabPage12.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage12.Size = new System.Drawing.Size(479, 232);
            this.tabPage12.TabIndex = 0;
            this.tabPage12.Text = "Invalid MAC Spoofing";
            this.tabPage12.UseVisualStyleBackColor = true;
            // 
            // pbInvMacSpoofedIp
            // 
            this.pbInvMacSpoofedIp.Image = global::evilfoca.Properties.Resources.Add;
            this.pbInvMacSpoofedIp.Location = new System.Drawing.Point(336, 67);
            this.pbInvMacSpoofedIp.Name = "pbInvMacSpoofedIp";
            this.pbInvMacSpoofedIp.Size = new System.Drawing.Size(15, 15);
            this.pbInvMacSpoofedIp.TabIndex = 11;
            this.pbInvMacSpoofedIp.TabStop = false;
            this.pbInvMacSpoofedIp.Click += new System.EventHandler(this.pbInvMacSpoofedIp_Click);
            // 
            // pbInvMacSpoofTarget
            // 
            this.pbInvMacSpoofTarget.Image = global::evilfoca.Properties.Resources.Add;
            this.pbInvMacSpoofTarget.Location = new System.Drawing.Point(336, 33);
            this.pbInvMacSpoofTarget.Name = "pbInvMacSpoofTarget";
            this.pbInvMacSpoofTarget.Size = new System.Drawing.Size(15, 15);
            this.pbInvMacSpoofTarget.TabIndex = 10;
            this.pbInvMacSpoofTarget.TabStop = false;
            this.pbInvMacSpoofTarget.Click += new System.EventHandler(this.pbInvMacSpoofTarget_Click);
            // 
            // tbIpSpoofedIpv4
            // 
            this.tbIpSpoofedIpv4.Location = new System.Drawing.Point(192, 64);
            this.tbIpSpoofedIpv4.Name = "tbIpSpoofedIpv4";
            this.tbIpSpoofedIpv4.Size = new System.Drawing.Size(138, 20);
            this.tbIpSpoofedIpv4.TabIndex = 9;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(57, 67);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(129, 13);
            this.label5.TabIndex = 8;
            this.label5.Text = "Lose communication with:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(57, 34);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(86, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "Target to attack:";
            // 
            // tbTargetSpoofedIpv4
            // 
            this.tbTargetSpoofedIpv4.Location = new System.Drawing.Point(192, 31);
            this.tbTargetSpoofedIpv4.Name = "tbTargetSpoofedIpv4";
            this.tbTargetSpoofedIpv4.Size = new System.Drawing.Size(138, 20);
            this.tbTargetSpoofedIpv4.TabIndex = 6;
            // 
            // btStartInvalidMacSpoof
            // 
            this.btStartInvalidMacSpoof.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btStartInvalidMacSpoof.Image = global::evilfoca.Properties.Resources.Start;
            this.btStartInvalidMacSpoof.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btStartInvalidMacSpoof.Location = new System.Drawing.Point(60, 179);
            this.btStartInvalidMacSpoof.Name = "btStartInvalidMacSpoof";
            this.btStartInvalidMacSpoof.Size = new System.Drawing.Size(70, 25);
            this.btStartInvalidMacSpoof.TabIndex = 5;
            this.btStartInvalidMacSpoof.Text = "Start";
            this.btStartInvalidMacSpoof.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btStartInvalidMacSpoof.UseVisualStyleBackColor = true;
            this.btStartInvalidMacSpoof.Click += new System.EventHandler(this.btStartInvalidMacSpoof_Click);
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.panel1);
            this.tabPage3.Location = new System.Drawing.Point(4, 23);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(493, 264);
            this.tabPage3.TabIndex = 4;
            this.tabPage3.Text = "DNS Hijacking";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.lbAdvertisement);
            this.panel1.Controls.Add(this.btAddDNSHijack);
            this.panel1.Controls.Add(this.tbDomainDNSHijack);
            this.panel1.Controls.Add(this.cbWildcardDnsHijack);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.tbIpDNSHijack);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(493, 264);
            this.panel1.TabIndex = 7;
            // 
            // lbAdvertisement
            // 
            this.lbAdvertisement.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbAdvertisement.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbAdvertisement.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbAdvertisement.ForeColor = System.Drawing.Color.Red;
            this.lbAdvertisement.Location = new System.Drawing.Point(65, 15);
            this.lbAdvertisement.Name = "lbAdvertisement";
            this.lbAdvertisement.Size = new System.Drawing.Size(368, 71);
            this.lbAdvertisement.TabIndex = 0;
            this.lbAdvertisement.Text = "To perform a DNS hijacking attack you\'ve to make any type of MITM between the tar" +
    "get and gateway or target and local DNS";
            // 
            // btAddDNSHijack
            // 
            this.btAddDNSHijack.Image = global::evilfoca.Properties.Resources.Start;
            this.btAddDNSHijack.Location = new System.Drawing.Point(65, 175);
            this.btAddDNSHijack.Name = "btAddDNSHijack";
            this.btAddDNSHijack.Size = new System.Drawing.Size(70, 23);
            this.btAddDNSHijack.TabIndex = 6;
            this.btAddDNSHijack.Text = "Start";
            this.btAddDNSHijack.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btAddDNSHijack.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btAddDNSHijack.UseVisualStyleBackColor = true;
            this.btAddDNSHijack.Click += new System.EventHandler(this.btAddDNSHijack_Click);
            // 
            // tbDomainDNSHijack
            // 
            this.tbDomainDNSHijack.Location = new System.Drawing.Point(128, 111);
            this.tbDomainDNSHijack.Name = "tbDomainDNSHijack";
            this.tbDomainDNSHijack.Size = new System.Drawing.Size(100, 20);
            this.tbDomainDNSHijack.TabIndex = 3;
            // 
            // cbWildcardDnsHijack
            // 
            this.cbWildcardDnsHijack.AutoSize = true;
            this.cbWildcardDnsHijack.Location = new System.Drawing.Point(234, 113);
            this.cbWildcardDnsHijack.Name = "cbWildcardDnsHijack";
            this.cbWildcardDnsHijack.Size = new System.Drawing.Size(65, 17);
            this.cbWildcardDnsHijack.TabIndex = 5;
            this.cbWildcardDnsHijack.Text = "wildcard";
            this.cbWildcardDnsHijack.UseVisualStyleBackColor = true;
            this.cbWildcardDnsHijack.CheckedChanged += new System.EventHandler(this.cbWildcardDnsHijack_CheckedChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(62, 114);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(43, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Domain";
            // 
            // tbIpDNSHijack
            // 
            this.tbIpDNSHijack.Location = new System.Drawing.Point(128, 138);
            this.tbIpDNSHijack.Name = "tbIpDNSHijack";
            this.tbIpDNSHijack.Size = new System.Drawing.Size(100, 20);
            this.tbIpDNSHijack.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(62, 141);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(17, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "IP";
            // 
            // imgListTrick
            // 
            this.imgListTrick.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imgListTrick.ImageStream")));
            this.imgListTrick.TransparentColor = System.Drawing.Color.Transparent;
            this.imgListTrick.Images.SetKeyName(0, "Cd-black-red.PNG");
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.configurationToolStripMenuItem,
            this.aboutToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.menuStrip1.Size = new System.Drawing.Size(954, 24);
            this.menuStrip1.TabIndex = 8;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("fileToolStripMenuItem.Image")));
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(81, 20);
            this.fileToolStripMenuItem.Text = "&Program";
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Image = global::evilfoca.Properties.Resources.Exit;
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(92, 22);
            this.exitToolStripMenuItem.Text = "&Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // configurationToolStripMenuItem
            // 
            this.configurationToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.interfaceToolStripMenuItem,
            this.settingsToolStripMenuItem});
            this.configurationToolStripMenuItem.Image = global::evilfoca.Properties.Resources.Configuration;
            this.configurationToolStripMenuItem.Name = "configurationToolStripMenuItem";
            this.configurationToolStripMenuItem.Size = new System.Drawing.Size(109, 20);
            this.configurationToolStripMenuItem.Text = "&Configuration";
            // 
            // interfaceToolStripMenuItem
            // 
            this.interfaceToolStripMenuItem.Image = global::evilfoca.Properties.Resources.Interface;
            this.interfaceToolStripMenuItem.Name = "interfaceToolStripMenuItem";
            this.interfaceToolStripMenuItem.Size = new System.Drawing.Size(120, 22);
            this.interfaceToolStripMenuItem.Text = "&Interface";
            this.interfaceToolStripMenuItem.Click += new System.EventHandler(this.interfaceToolStripMenuItem_Click);
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.Image = global::evilfoca.Properties.Resources.Settings;
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(120, 22);
            this.settingsToolStripMenuItem.Text = "&Settings";
            this.settingsToolStripMenuItem.Click += new System.EventHandler(this.settingsToolStripMenuItem_Click);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Image = global::evilfoca.Properties.Resources.Help;
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(68, 20);
            this.aboutToolStripMenuItem.Text = "&About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // treeView
            // 
            this.treeView.AllowDrop = true;
            this.treeView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.treeView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.treeView.ContextMenuStrip = this.cMenuNeighbors;
            this.treeView.ForeColor = System.Drawing.Color.Black;
            this.treeView.ImageIndex = 11;
            this.treeView.ImageList = this.imageList1;
            this.treeView.Location = new System.Drawing.Point(0, 0);
            this.treeView.Name = "treeView";
            this.treeView.SelectedImageIndex = 0;
            this.treeView.Size = new System.Drawing.Size(263, 382);
            this.treeView.TabIndex = 7;
            this.treeView.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.treeView_ItemDrag);
            this.treeView.MouseDown += new System.Windows.Forms.MouseEventHandler(this.treeView_MouseDown);
            // 
            // panelNeighborAdvSpoof
            // 
            this.panelNeighborAdvSpoof.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelNeighborAdvSpoof.ForeColor = System.Drawing.SystemColors.WindowText;
            this.panelNeighborAdvSpoof.Location = new System.Drawing.Point(0, 0);
            this.panelNeighborAdvSpoof.Name = "panelNeighborAdvSpoof";
            this.panelNeighborAdvSpoof.Size = new System.Drawing.Size(475, 194);
            this.panelNeighborAdvSpoof.TabIndex = 4;
            // 
            // panelTargetSLAACMitm
            // 
            this.panelTargetSLAACMitm.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelTargetSLAACMitm.Location = new System.Drawing.Point(18, 41);
            this.panelTargetSLAACMitm.Name = "panelTargetSLAACMitm";
            this.panelTargetSLAACMitm.Size = new System.Drawing.Size(441, 155);
            this.panelTargetSLAACMitm.TabIndex = 40;
            // 
            // wpadv6TargetPanel
            // 
            this.wpadv6TargetPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.wpadv6TargetPanel.Location = new System.Drawing.Point(3, 3);
            this.wpadv6TargetPanel.Name = "wpadv6TargetPanel";
            this.wpadv6TargetPanel.Size = new System.Drawing.Size(473, 188);
            this.wpadv6TargetPanel.TabIndex = 0;
            // 
            // panelMitmIpv4
            // 
            this.panelMitmIpv4.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelMitmIpv4.Location = new System.Drawing.Point(0, 0);
            this.panelMitmIpv4.Name = "panelMitmIpv4";
            this.panelMitmIpv4.Size = new System.Drawing.Size(475, 194);
            this.panelMitmIpv4.TabIndex = 5;
            // 
            // wpadTargetPanel
            // 
            this.wpadTargetPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.wpadTargetPanel.Location = new System.Drawing.Point(3, 3);
            this.wpadTargetPanel.Name = "wpadTargetPanel";
            this.wpadTargetPanel.Size = new System.Drawing.Size(473, 188);
            this.wpadTargetPanel.TabIndex = 6;
            // 
            // panelTargetSLAACDoS
            // 
            this.panelTargetSLAACDoS.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelTargetSLAACDoS.Location = new System.Drawing.Point(1, 0);
            this.panelTargetSLAACDoS.Name = "panelTargetSLAACDoS";
            this.panelTargetSLAACDoS.Size = new System.Drawing.Size(475, 194);
            this.panelTargetSLAACDoS.TabIndex = 5;
            // 
            // help
            // 
            this.help.Dock = System.Windows.Forms.DockStyle.Fill;
            this.help.Location = new System.Drawing.Point(0, 20);
            this.help.Margin = new System.Windows.Forms.Padding(0);
            this.help.Name = "help";
            this.help.Size = new System.Drawing.Size(182, 271);
            this.help.TabIndex = 0;
            // 
            // listViewExAttacks
            // 
            this.listViewExAttacks.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.attackType,
            this.attack,
            this.Active});
            this.listViewExAttacks.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listViewExAttacks.GridLines = true;
            this.listViewExAttacks.Location = new System.Drawing.Point(0, 0);
            this.listViewExAttacks.MultiSelect = false;
            this.listViewExAttacks.Name = "listViewExAttacks";
            this.listViewExAttacks.Size = new System.Drawing.Size(690, 171);
            this.listViewExAttacks.StateImageList = this.imgListTrick;
            this.listViewExAttacks.TabIndex = 2;
            this.listViewExAttacks.UseCompatibleStateImageBehavior = false;
            this.listViewExAttacks.View = System.Windows.Forms.View.Details;
            // 
            // attackType
            // 
            this.attackType.Text = "Attack type";
            this.attackType.Width = 123;
            // 
            // attack
            // 
            this.attack.Text = "Attack";
            this.attack.Width = 449;
            // 
            // Active
            // 
            this.Active.Text = "Active";
            // 
            // panelLogs
            // 
            this.panelLogs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelLogs.Location = new System.Drawing.Point(0, 0);
            this.panelLogs.Name = "panelLogs";
            this.panelLogs.Size = new System.Drawing.Size(954, 162);
            this.panelLogs.TabIndex = 10;
            // 
            // FormMain
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(954, 662);
            this.Controls.Add(this.splitContainer2);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.MinimumSize = new System.Drawing.Size(970, 700);
            this.Name = "FormMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Evil FOCA";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormMain_FormClosing);
            this.Load += new System.EventHandler(this.FormMain_Load);
            panelIzquierda.ResumeLayout(false);
            panelIzquierda.PerformLayout();
            this.cMenuNeighbors.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.panelButtons.ResumeLayout(false);
            this.splitContainer4.Panel1.ResumeLayout(false);
            this.splitContainer4.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer4)).EndInit();
            this.splitContainer4.ResumeLayout(false);
            this.splitContainerHelp.Panel1.ResumeLayout(false);
            this.splitContainerHelp.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerHelp)).EndInit();
            this.splitContainerHelp.ResumeLayout(false);
            this.panelMainOptions.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabevilfoca.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage8.ResumeLayout(false);
            this.tabPage8.PerformLayout();
            this.tabPage9.ResumeLayout(false);
            this.tabPage9.PerformLayout();
            this.tabPage13.ResumeLayout(false);
            this.tabPage5.ResumeLayout(false);
            this.tabMitmIpv4.ResumeLayout(false);
            this.tabPage7.ResumeLayout(false);
            this.tabPage11.ResumeLayout(false);
            this.tabPage11.PerformLayout();
            this.tabPage14.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.tabPage4.ResumeLayout(false);
            this.tabDoSIpv6.ResumeLayout(false);
            this.tabPage10.ResumeLayout(false);
            this.tabPage6.ResumeLayout(false);
            this.tabDoSIpv4.ResumeLayout(false);
            this.tabPage12.ResumeLayout(false);
            this.tabPage12.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbInvMacSpoofedIp)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbInvMacSpoofTarget)).EndInit();
            this.tabPage3.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private BufferedTreeView treeView;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Button btStartNeighborSpoof;
        private System.Windows.Forms.TabControl tabevilfoca;
        private System.Windows.Forms.TabPage tabPage1;
        private Controls.ListViewEx.ListViewEx listViewExAttacks;
        private System.Windows.Forms.ColumnHeader attackType;
        private System.Windows.Forms.ColumnHeader attack;
        private System.Windows.Forms.ColumnHeader Active;
        public PanelLogs panelLogs;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.Button btStartARPSpoofing;
        private System.Windows.Forms.SplitContainer splitContainer4;
        private System.Windows.Forms.TabControl panelMainOptions;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.TabPage tabPage8;
        private System.Windows.Forms.TabPage tabPage9;
        private System.Windows.Forms.TabPage tabPage5;
        private System.Windows.Forms.TabControl tabMitmIpv4;
        private System.Windows.Forms.TabPage tabPage7;
        private System.Windows.Forms.TabPage tabPage6;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem configurationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem interfaceToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private PanelTargets panelNeighborAdvSpoof;
        private PanelTargets panelMitmIpv4;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.Label lbAdvertisement;
        private System.Windows.Forms.CheckBox cbWildcardDnsHijack;
        private System.Windows.Forms.TextBox tbIpDNSHijack;
        private System.Windows.Forms.TextBox tbDomainDNSHijack;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btAddDNSHijack;
        private System.Windows.Forms.ImageList imgListTrick;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TabControl tabDoSIpv6;
        private System.Windows.Forms.TabPage tabPage10;
        private System.Windows.Forms.Button buttonStartSLAACDoS;
        private PanelTarget panelTargetSLAACDoS;
        private System.Windows.Forms.TabPage tabPage11;
        private System.Windows.Forms.RadioButton radioButtonComputerWithMAC;
        private System.Windows.Forms.RadioButton radioButtonAllcomputers;
        private System.Windows.Forms.CheckBox checkBoxUseAsGateway;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label labelFakeGateway;
        private System.Windows.Forms.Label labelMAC;
        private System.Windows.Forms.ContextMenuStrip cMenuNeighbors;
        private System.Windows.Forms.ToolStripMenuItem addNeighborToolStripMenuItem;
        private System.Windows.Forms.TextBox textBoxDHCPAckMAC;
        private System.Windows.Forms.TextBox textBoxDNS;
        private System.Windows.Forms.TextBox textBoxGateway;
        private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem;
        private System.Windows.Forms.Button buttonDHCPACKInjection;
        private System.Windows.Forms.TabControl tabDoSIpv4;
        private System.Windows.Forms.TabPage tabPage12;
        private System.Windows.Forms.Button btStartInvalidMacSpoof;
        private System.Windows.Forms.TextBox tbIpSpoofedIpv4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbTargetSpoofedIpv4;
        private System.Windows.Forms.PictureBox pbInvMacSpoofedIp;
        private System.Windows.Forms.PictureBox pbInvMacSpoofTarget;
        private ControlHelp.ControlHelp help;
        public System.Windows.Forms.SplitContainer splitContainerHelp;
        private System.Windows.Forms.Button buttonEnableIPRouting;
        private System.Windows.Forms.Label labelIPRouting;
        private System.Windows.Forms.Panel panelButtons;
        private System.Windows.Forms.Button btNetworkDiscovery;
        private System.Windows.Forms.Button btSearchRouters;
        private System.Windows.Forms.Button btStart_IPv6DHCP;
        private System.Windows.Forms.TextBox tbDNS_IPv6DHCP;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button btMitmSLAAC;
        private System.Windows.Forms.Button btAddNeighbor;
        private PanelTarget panelTargetSLAACMitm;
        private System.Windows.Forms.TextBox tbSearchFilter;
        private System.Windows.Forms.Label lbFilter;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox tbSlaacMitmPrefix;
        private System.Windows.Forms.TextBox tbIPRange_IPv6DHCP;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TabPage tabPage13;
        private System.Windows.Forms.TabPage tabPage14;
        private System.Windows.Forms.Button btnWpadAttack;
        private PanelTarget wpadTargetPanel;
        private System.Windows.Forms.Button btnWpadv6Attack;
        private PanelTarget wpadv6TargetPanel;
        private System.Windows.Forms.Panel panel2;
    }
}

