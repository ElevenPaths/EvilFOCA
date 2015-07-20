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
using evilfoca.Attacks;
using evilfoca.Core;
using evilfoca.Data;
using PacketDotNet;
using SharpPcap;
using SharpPcap.WinPcap;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Net;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace evilfoca
{
    public partial class FormMain : Form
    {
        private bool isSniffing = false;

        Core.NetworkDiscovery networkDiscovery;

        // Filter
        string strFilter = string.Empty;
        bool isFiltering = false;

        Attacks.NASpoofing NASpoofAttack;
        Attacks.ARPSpoofing ArpSpoofing;
        Attacks.DoSSLAAC DoSSLAAC;
        Attacks.InvalidMacSpoofIPV4 invalidMacSpoofIpv4;
        Attacks.SlaacMITM slaacMitm;

        Thread tStackPacket;
        Thread tNetworkDiscovery;

        public FormMain()
        {
            InitializeComponent();
        }

        private void device_OnPacketArrival(object sender, CaptureEventArgs e)
        {
            try
            {
                Packet p = Packet.ParsePacket(LinkLayers.Ethernet, e.Packet.Data);
                Program.CurrentProject.data.stackPacketsFIFO.Add(p);
            }
            catch
            {
                // Paquete perdido
            }
        }

        private bool IsNeighborFiltered(Neighbor neighbor)
        {
            // true -> El vecino NO se pinta
            // false -> El vecino SI se pinta

            // Se comprueba el nombre
            if (!string.IsNullOrEmpty(neighbor.computerName))
            {
                if (neighbor.computerName.ToLower().Contains(strFilter.ToLower()))
                    return false;
            }

            // Se comprueba la MAC
            if (neighbor.physicalAddress.ToString().ToLower().Contains(strFilter.ToLower()))
                return false;

            // Se comprueban las IPs
            for (int ip = 0; ip < neighbor.GetIPs().Count; ip++)
            {
                if (neighbor.GetIPs()[ip].ToString().ToLower().Contains(strFilter.ToLower()))
                    return false;
            }

            return true;
        }

        private void add_NewNeighbor(object sender, EventArgs e)
        {
            try
            {
                if (treeView.Nodes[mainNode] == null || treeView.Nodes[mainNode].Nodes[neighborsNode] == null)
                    return;

                Neighbor neighbor = (Neighbor)sender;
                NeighborEventArgs ea = (NeighborEventArgs)e;
                TreeNode tnNeighbor = null;

                if (isFiltering)
                {
                    if (IsNeighborFiltered(neighbor))
                        return;
                }

                treeView.Invoke(new MethodInvoker(delegate
                            {
                                treeView.BeginUpdate();
                            }));

                // Cuando se filtra y se quitan de la GUI vecinos, luego hay que restaurarlos y volver a pintarlos.
                // En memoria siguen estando pero en la GUI no, así que llegarán como eventos de 'actualizar', sin 
                // embargo hay que añadirlos a la GUI como si fueran nuevos. Aqui se hace el cambio de actualizar a añadir.
                if (ea.Tipo == NeighborOperacionTreeView.Actualizar)
                {
                    if (treeView.Nodes[mainNode].Nodes[neighborsNode].Nodes[neighbor.physicalAddress.ToString()] == null)
                        ea.Tipo = NeighborOperacionTreeView.Añadir;
                }

                switch (ea.Tipo)
                {
                    case NeighborOperacionTreeView.Añadir:
                        // No existe el vecino. Se agrega.
                        treeView.Invoke(new MethodInvoker(delegate
                        {
                            tnNeighbor = treeView.Nodes[mainNode].Nodes[neighborsNode].Nodes.Add(neighbor.physicalAddress.ToString(), neighbor.physicalAddress.ToString());

                            if (tnNeighbor == null)
                            {
                                treeView.EndUpdate();
                                return;
                            }

                            tnNeighbor.Tag = neighbor;
                            tnNeighbor.ImageIndex = tnNeighbor.SelectedImageIndex = 11;

                            if (treeView.Nodes[mainNode].Nodes[neighborsNode].Nodes.Count == 1)
                                treeView.Nodes[mainNode].Nodes[neighborsNode].Expand();
                        }));


                        break;
                    case NeighborOperacionTreeView.Actualizar:
                        // El vecino existe. Se insertan nodos desplegables por cada IP que tenga el vecino
                        tnNeighbor = treeView.Nodes[mainNode].Nodes[neighborsNode].Nodes[neighbor.physicalAddress.ToString()];
                        if (tnNeighbor == null)
                        {
                            treeView.EndUpdate();
                            return;
                        }
                        break;
                }

                List<IPAddress> listaIps = new List<IPAddress>(neighbor.GetIPs());
                foreach (IPAddress ipAddress in listaIps)
                {
                    if (!tnNeighbor.Nodes.ContainsKey(ipAddress.ToString()))
                    {
                        treeView.Invoke(new MethodInvoker(delegate
                        {
                            TreeNode tnNeighborIP = tnNeighbor.Nodes.Add(ipAddress.ToString(), ipAddress.ToString());
                            tnNeighborIP.ImageIndex = tnNeighborIP.SelectedImageIndex = 0;
                            tnNeighborIP.Tag = ipAddress;
                            tnNeighbor.Expand();

                            Thread tColorChanger = new Thread(new ParameterizedThreadStart(ChangeColor));
                            tColorChanger.IsBackground = true;
                            tColorChanger.Start(tnNeighborIP);
                        }));
                    }
                    else
                    {
                        // Si ya existia, se agregan los puertos en caso de que no existan.
                        TreeNode tnIP = tnNeighbor.Nodes[ipAddress.ToString()];
                        UpdatePortsOfIPNode(tnIP);
                    }
                }

                // Si el vecino tiene nombre, se le establece en el nodo
                if (!tnNeighbor.Text.StartsWith(neighbor.ToString()))
                {
                    treeView.Invoke(new MethodInvoker(delegate
                    {
                        tnNeighbor.Text = neighbor.ToString();
                        Thread tColorChanger = new Thread(new ParameterizedThreadStart(ChangeColorUpdate));
                        tColorChanger.IsBackground = true;
                        tColorChanger.Start(tnNeighbor);
                    }));
                }

                // Se le pone el iconcillo de OS
                if ((neighbor.osPlatform != Platform.Unknow) && (tnNeighbor.ImageIndex == 11))
                {
                    treeView.Invoke(new MethodInvoker(delegate
                    {
                        int imageOS = Program.CurrentProject.data.GetImageNumberOS(neighbor.osPlatform);
                        tnNeighbor.ImageIndex = tnNeighbor.SelectedImageIndex = imageOS;
                    }));
                }

                treeView.Invoke(new MethodInvoker(delegate
                {
                    treeView.EndUpdate();
                }));


                Update_Routers(neighbor);
            }
            catch (Exception ex)
            {
                ex.ToString();
            }

        }


        private void Update_Routers(Neighbor neighbor)
        {
            treeView.Invoke(new MethodInvoker(delegate
            {
                treeView.BeginUpdate();
            }));
            if (!treeView.Nodes[mainNode].Nodes[routersNode].Nodes.ContainsKey(neighbor.physicalAddress.ToString()))
            {
                // Si el vecino puede enrutar y no se ha agregado, se agrega
                if (neighbor.canRoutePackets == RouteStatus.CanRoute)
                {
                    treeView.Invoke(new MethodInvoker(delegate
                    {
                        TreeNode tn = treeView.Nodes[mainNode].Nodes[routersNode].Nodes.Add(neighbor.physicalAddress.ToString(), neighbor.physicalAddress.ToString());
                        tn.Tag = neighbor;
                        tn.ImageIndex = tn.SelectedImageIndex = 11;

                        if (treeView.Nodes[mainNode].Nodes[routersNode].Nodes.Count == 1)
                            treeView.Nodes[mainNode].Nodes[routersNode].Expand();
                    }));
                }
            }

            // Como se puede haber insertado el nodo en el IF anterior, se vuelve a comprobar a ver si podemos insertarle la IP
            if (treeView.Nodes[mainNode].Nodes[routersNode].Nodes.ContainsKey(neighbor.physicalAddress.ToString()))
            {
                // El router existe. Se insertan nodos desplegables por cada IP que tenga el router
                TreeNode tnRouter = treeView.Nodes[mainNode].Nodes[routersNode].Nodes[neighbor.physicalAddress.ToString()];

                foreach (IPAddress ipAddress in neighbor.GetIPs())
                {
                    // Se agrega la IP en caso de que no exista
                    if (!tnRouter.Nodes.ContainsKey(ipAddress.ToString()))
                    {
                        treeView.Invoke(new MethodInvoker(delegate
                        {
                            TreeNode tnRouterIP = tnRouter.Nodes.Add(ipAddress.ToString(), ipAddress.ToString());
                            tnRouterIP.ImageIndex = tnRouterIP.SelectedImageIndex = 0;
                            tnRouterIP.Tag = ipAddress;
                            tnRouter.Expand();

                            Thread tColorChanger = new Thread(new ParameterizedThreadStart(ChangeColor));
                            tColorChanger.IsBackground = true;
                            tColorChanger.Start(tnRouterIP);
                        }));
                    }
                    else
                    {
                        // Si ya existia, se agregan los puertos en caso de que no existan.
                        TreeNode tnIP = tnRouter.Nodes[ipAddress.ToString()];
                        UpdatePortsOfIPNode(tnIP);
                    }
                }

                // Si el router tiene nombre, se le establece en el nodo
                if (!tnRouter.Text.StartsWith(neighbor.ToString()))
                {
                    treeView.Invoke(new MethodInvoker(delegate
                    {
                        tnRouter.Text = neighbor.ToString();
                        Thread tColorChanger = new Thread(new ParameterizedThreadStart(ChangeColorUpdate));
                        tColorChanger.IsBackground = true;
                        tColorChanger.Start(tnRouter);
                    }));
                }

                // Se le pone el iconcillo de OS
                if ((neighbor.osPlatform != Platform.Unknow) && (tnRouter.ImageIndex == 11))
                {
                    treeView.Invoke(new MethodInvoker(delegate
                    {
                        int imageOS = Program.CurrentProject.data.GetImageNumberOS(neighbor.osPlatform);
                        tnRouter.ImageIndex = tnRouter.SelectedImageIndex = imageOS;
                    }));
                }
            }
            treeView.Invoke(new MethodInvoker(delegate
            {
                treeView.EndUpdate();
            }));

        }

        private void CierraSplash()
        {
            Program.fSplash.Invoke(new MethodInvoker(delegate
            {
                Program.fSplash.Close();
            }
            ));
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            this.Text = Application.ProductName + " - " + Application.ProductVersion;
            Program.CurrentProject.data.NewNeighbor += new EventHandler<NeighborEventArgs>(add_NewNeighbor);
            UpdateMainNodes();

            this.BringToFront();

            Program.CurrentProject.data.GetDevice().Open(SharpPcap.WinPcap.OpenFlags.Promiscuous /*| OpenFlags.NoCaptureLocal*/, 1);

            // Inicia el hilo que gestiona la stack de paquetes
            tStackPacket = new Thread(new ThreadStart(StackPacketGestor));
            tStackPacket.IsBackground = true;
            tStackPacket.Start();

            NASpoofAttack = new Attacks.NASpoofing(Program.CurrentProject.data.GetDevice(), Program.CurrentProject.data.GetAttacks());

            if (Program.CurrentProject.data.GetIPv4FromDevice(Program.CurrentProject.data.GetDevice()) != null)
                ArpSpoofing = new Attacks.ARPSpoofing(Program.CurrentProject.data.GetDevice(), Program.CurrentProject.data.GetIPv4FromDevice(Program.CurrentProject.data.GetDevice()), Program.CurrentProject.data.GetAttacks());

            DoSSLAAC = new Attacks.DoSSLAAC(Program.CurrentProject.data.GetDevice(), Program.CurrentProject.data.GetAttacks());
            invalidMacSpoofIpv4 = new Attacks.InvalidMacSpoofIPV4(Program.CurrentProject.data.GetDevice(), Program.CurrentProject.data.GetAttacks());
            slaacMitm = new Attacks.SlaacMITM(Program.CurrentProject.data.GetDevice(), Program.CurrentProject.data.GetAttacks());

            StartSniffing();

            // Inicializa el hilo del network discovery
            networkDiscovery = new Core.NetworkDiscovery();
            this.btNetworkDiscovery.Image = Properties.Resources.SearchStop;
            networkDiscovery.OnDiscoverFinish += new NetworkDiscovery.DiscoverFinishedDelegate(networkDiscovery_OnDiscoverFinish);
            networkDiscovery.SetDevice(Program.CurrentProject.data.GetDevice());
            tNetworkDiscovery = new Thread(new ThreadStart(networkDiscovery.StartNetworkDiscoveryJustOneTime));
            tNetworkDiscovery.IsBackground = true;
            tNetworkDiscovery.Start();

            listViewExAttacks.FullRowSelect = true;

            panelMitmIpv4.typeList = PanelTargets.TypeList.Ipv4;
            panelNeighborAdvSpoof.typeList = PanelTargets.TypeList.Ipv6;
            wpadTargetPanel.typeList = PanelTargets.TypeList.Ipv4;
            wpadv6TargetPanel.typeList = PanelTargets.TypeList.Ipv6;

            // Forzamos a cargar el panel de ayuda de mitm de neighbors ipv6, que es la pestaña que se carga al inicio
            tabMitm_Selected(null, null);

            AddShieldToButton(buttonEnableIPRouting);

            tbSlaacMitmPrefix.Text = "fc00::0"; //prefix por defecto Slaac

            //CierraSplash();
            Thread tCierraSplash = new Thread(new ThreadStart(CierraSplash));
            tCierraSplash.IsBackground = true;
            tCierraSplash.Start();
        }

        private void networkDiscovery_OnDiscoverFinish(object sender, EventArgs myArgs)
        {
            this.btNetworkDiscovery.Image = Properties.Resources.SearchStart;
        }

        #region UAC Elevate button
        [DllImport("user32")]
        public static extern UInt32 SendMessage
            (IntPtr hWnd, UInt32 msg, UInt32 wParam, UInt32 lParam);

        internal const int BCM_SETSHIELD = (0x1600 + 0x000C); //Elevate button
        static internal void AddShieldToButton(Button b)
        {
            b.FlatStyle = FlatStyle.System;
            SendMessage(b.Handle, BCM_SETSHIELD, 0, 0xFFFFFFFF);
        }
        #endregion

        #region StackFIFOGestorPaquetes

        private void StackPacketGestor()
        {
            int concurrentThreads = Environment.ProcessorCount;

            ManualResetEvent[] doneEvents = new ManualResetEvent[concurrentThreads];
            PasiveScan[] scanArray = new PasiveScan[concurrentThreads];
            for (int i = 0; i < concurrentThreads; i++)
            {
                doneEvents[i] = new ManualResetEvent(true);
                PasiveScan scanProc = new Core.PasiveScan(Program.CurrentProject.data.GetDevice(), Program.CurrentProject.data.GetAttacks(), Data.Data.neighbors, Data.Data.SlaacReqList);
                scanProc.NewICMPv6Solicitation += new EventHandler<Core.PacketEventArgs>(pasiveScan_NewICMPv6Solicitation);
                scanProc.NewNeighbor += new EventHandler<NeighborEventArgs>(pasiveScan_NewNeighbor);
                scanArray[i] = scanProc;
            }
            while (true)
            {
                if (Program.CurrentProject.data.stackPacketsFIFO.Count > 0)
                {
                    int index = WaitHandle.WaitAny(doneEvents);
                    doneEvents[index] = new ManualResetEvent(false);
                    Packet p = Program.CurrentProject.data.stackPacketsFIFO[0];
                    scanArray[index].packetToAnalyze = p;
                    Program.CurrentProject.data.stackPacketsFIFO.RemoveAt(0);
                    scanArray[index].doneEvent = doneEvents[index];
                    ThreadPool.QueueUserWorkItem(scanArray[index].ThreadPoolCallback, index);
                }
                else
                    System.Threading.Thread.Sleep(1);
            }
        }
        #endregion

        #region GUI_Updater

        private static string mainNode = "Network";
        private static string neighborsNode = "Neighbors";
        private static string routersNode = "Routers";
        private static string portsNode = "Ports";

        private void UpdateGUIBackgroundAsync()
        {
            try
            {
                int interval = 500;

                while (true)
                {
                    UpdateGUI();
                    System.Threading.Thread.Sleep(interval);
                }
            }
            catch (ThreadAbortException)
            {
                return;
            }
            catch (Exception ex)
            {
#if DEBUG
                MessageBox.Show("Se ha producido una excepción en la actualizacion de la GUI: " + ex.Message);
#endif
            }
        }

        private void UpdateGUI()
        {
            if (System.Windows.Forms.Application.OpenForms.Count == 0)
                return; // Evita condiciones de carrera donde se intenta actualizar la GUI antes de haber creado el identificador de ventana

            UpdateMainNodes();
            UpdateAttackList();
        }

        private void UpdatePortsOfIPNode(TreeNode tn)
        {
            if (tn == null)
                return;
            TreeNode tnPorts = null;

            if (tn.Tag is IPAddress)
            {
                IPAddress neighborIP = (IPAddress)tn.Tag;
                PhysicalAddress neighborMac = Program.CurrentProject.data.GetNeighborMAC(neighborIP);
                if (neighborMac == null)
                    return;
                Neighbor neighbor = Program.CurrentProject.data.GetNeighbor(neighborMac);
                if (neighbor == null)
                    return;

                SynchronizedCollection<Port> lstPorts = neighbor.GetPorts(neighborIP);
                if (lstPorts.Count > 0)
                {
                    if (!(tn.Nodes.ContainsKey(portsNode)))
                    {
                        treeView.Invoke(new MethodInvoker(delegate
                        {
                            tnPorts = tn.Nodes.Add(portsNode, portsNode);
                            tnPorts.ImageIndex = tnPorts.SelectedImageIndex = 13;
                        }));
                    }
                    else
                        tnPorts = tn.Nodes[portsNode];

                    foreach (Port p in lstPorts)
                    {
                        if (!(tnPorts.Nodes.ContainsKey(p.ToString())))
                        {
                            treeView.Invoke(new MethodInvoker(delegate
                            {
                                TreeNode tnPort = tnPorts.Nodes.Add(p.ToString(), p.ToString());
                                tnPort.ImageIndex = tnPort.SelectedImageIndex = 14;
                            }));
                        }
                    }

                }

            }
        }

        private void UpdateMainNodes()
        {
            bool firstTime = false;

            if (treeView.Nodes[mainNode] == null)
            {
                treeView.Invoke(new MethodInvoker(delegate
                    {
                        TreeNode tn = treeView.Nodes.Add(mainNode, mainNode);
                        tn.ImageIndex = tn.SelectedImageIndex = 2;
                    }));
                firstTime = true;
            }
            if (treeView.Nodes[mainNode].Nodes[neighborsNode] == null)
            {
                treeView.Invoke(new MethodInvoker(delegate
                {
                    TreeNode tn = treeView.Nodes[mainNode].Nodes.Add(neighborsNode, neighborsNode);
                    tn.ImageIndex = tn.SelectedImageIndex = 3;
                }));
            }

            if (treeView.Nodes[mainNode].Nodes[routersNode] == null)
            {
                treeView.Invoke(new MethodInvoker(delegate
                {
                    TreeNode tn = treeView.Nodes[mainNode].Nodes.Add(routersNode, routersNode);
                    tn.ImageIndex = tn.SelectedImageIndex = 12;
                }));
            }

            if (firstTime)
            {
                treeView.Invoke(new MethodInvoker(delegate
                    {
                        treeView.ExpandAll();
                    }));
            }
        }

        private static void ChangeColorUpdate(object o)
        {
            try
            {
                TreeNode tn = (TreeNode)o;
                tn.ForeColor = Color.Blue;
            }
            catch
            {
#if DEBUG
                MessageBox.Show("Excepcion coloreando nodos en ChangeColorUpdate(object o)");
#endif
            }
        }

        private static void ChangeColor(object o)
        {
            try
            {
                TreeNode tn = (TreeNode)o;
                tn.ForeColor = SystemColors.WindowText;
            }
            catch
            {
#if DEBUG
                MessageBox.Show("Excepcion coloreando nodos en ChangeColor(object o)");
#endif
            }
        }

        private void UpdateAttackList()
        {
            listViewExAttacks.Invoke(new MethodInvoker(delegate
            {
                foreach (ListViewItem lvi in listViewExAttacks.Items)
                {
                    Data.Attack attack = (Data.Attack)lvi.Tag;

                    if (lvi.SubItems[1].Tag is ControlMitmAttack)
                        ((ControlMitmAttack)lvi.SubItems[1].Tag).UpdateData();
                    if (lvi.SubItems[1].Tag is ControlDNSHijacking)
                        ((ControlDNSHijacking)lvi.SubItems[1].Tag).UpdateData();
                }
            }));
        }

        #endregion

        private void StartSniffing()
        {
            if (isSniffing == false)
            {

                isSniffing = true;
                Program.CurrentProject.data.GetDevice().OnPacketArrival += new PacketArrivalEventHandler(device_OnPacketArrival);
                Program.CurrentProject.data.GetDevice().OnPcapStatistics += new StatisticsModeEventHandler(FormMain_OnPcapStatistics);
                Program.CurrentProject.data.GetDevice().StartCapture();

                // Se inicializan los manejadores de cada tipo de ataque. Un hilo por cada manejador
                if (ArpSpoofing != null)
                    ArpSpoofing.Start();
                if (NASpoofAttack != null)
                    NASpoofAttack.Start();
                if (DoSSLAAC != null)
                    DoSSLAAC.Start();
                if (invalidMacSpoofIpv4 != null)
                    invalidMacSpoofIpv4.Start();
                if (slaacMitm != null)
                    slaacMitm.Start();
            }
        }

        void FormMain_OnPcapStatistics(object sender, StatisticsModeEventArgs e)
        {
        }

        void pasiveScan_NewNeighbor(object sender, NeighborEventArgs e)
        {
            if (!(e.Neighbor.physicalAddress.Equals(Program.CurrentProject.data.GetDevice().Interface.MacAddress)))
                Program.LogThis("New neighbor detected with " + e.Neighbor.physicalAddress.ToString() + " as physical address", Logs.Log.LogType.NeighborSpoofing);

        }

        void pasiveScan_NewICMPv6Solicitation(object sender, Core.PacketEventArgs e)
        {
            if (!(e.packet is EthernetPacket))
                return;

            Packet packet = e.packet;

            ICMPv6Packet pIcmp = (ICMPv6Packet)packet.PayloadPacket.PayloadPacket;
            IPAddress ipDestination = ((IPv6Packet)pIcmp.ParentPacket).DestinationAddress;
            IPAddress ipSource = ((IPv6Packet)pIcmp.ParentPacket).SourceAddress;

            PhysicalAddress macDestination = ((EthernetPacket)packet).DestinationHwAddress;
            PhysicalAddress macSource = ((EthernetPacket)packet).SourceHwAddress;

            if (ipDestination.IsIPv6Multicast)
            {
                ICMPv6.NeighborSolicitation solicitation = new ICMPv6.NeighborSolicitation(pIcmp.Bytes);
            }
        }

        private void StopSniffing()
        {
            if (isSniffing == true)
            {
                isSniffing = false;

                if (NASpoofAttack != null)
                    NASpoofAttack.Stop();
                if (ArpSpoofing != null)
                    ArpSpoofing.Stop();

                Program.CurrentProject.data.GetDevice().Close();
            }
        }

        private void treeView_ItemDrag(object sender, ItemDragEventArgs e)
        {
            (sender as TreeView).SelectedNode = (e.Item as TreeNode);

            if ((e.Item as TreeNode).Tag is IPAddress)
                DoDragDrop((e.Item as TreeNode).Tag, DragDropEffects.Move);
        }

        private void btStartNeighborSpoof_Click(object sender, EventArgs e)
        {
            List<IPAddress> lstGateway = panelNeighborAdvSpoof.GetGatewayList();
            List<IPAddress> lstTargets = panelNeighborAdvSpoof.GetTargetsList();

            if (lstGateway.Count == 0)
                ShowMessage("You've to set a 'gateway'", 1000, FormMessage.IconType.Error);
            else if (lstTargets.Count == 0)
                ShowMessage("You've to set at least any target", 1000, FormMessage.IconType.Error);
            else
            {
                IPAddress gateway = (IPAddress)lstGateway[0];

                foreach (IPAddress target in lstTargets)
                {
                    PhysicalAddress macTarget = Program.CurrentProject.data.GetNeighborMAC(target);
                    PhysicalAddress macGateway = Program.CurrentProject.data.GetNeighborMAC(gateway);
                    if ((macTarget == null) || (macGateway == null))
                        return; // Se descarta el target. Algo raro pasa, ¿Como tener la IP sin la mac en la lista de vecinos?
                    MitmAttack attack = new Data.MitmAttack(new Target(gateway, macGateway), new Target(target, macTarget), AttackType.NeighborAdvertisementSpoofing);
                    Program.CurrentProject.data.AddAttack(attack);
                    attack.Start();

                    AddAttackToListViewEx(attack);

                    Program.LogThis("Performing a MITM (Neighbor spoofing) attack between " + attack.t1.ip.ToString() + " and " + attack.t2.ip.ToString(), Logs.Log.LogType.NeighborSpoofing);
                }

                panelNeighborAdvSpoof.ClearGateway();
                panelNeighborAdvSpoof.ClearTargets();
            }
        }


        private void AddAttackToListViewEx(Attack attack)
        {
            ListViewItem lvi = new ListViewItem();
            lvi.Tag = attack;

            lvi.Text = attack.attackType.ToString(); // type
            lvi.SubItems.Add(""); // control
            lvi.SubItems.Add(""); // active
            listViewExAttacks.Items.Add(lvi);
            if (attack is MitmAttack)
            {
                if (attack.attackType == AttackType.SlaacMitm)
                {
                    ControlSlaacMitm contr = new ControlSlaacMitm(attack);
                    listViewExAttacks.AddEmbeddedControl(contr, 1, listViewExAttacks.Items.Count - 1);
                    lvi.SubItems[1].Tag = contr;
                }

                else
                {
                    ControlMitmAttack contr = new ControlMitmAttack(attack);
                    listViewExAttacks.AddEmbeddedControl(contr, 1, listViewExAttacks.Items.Count - 1);
                    lvi.SubItems[1].Tag = contr;
                }
            }
            else if (attack is DNSHijackAttack)
            {
                ControlDNSHijacking contr = new ControlDNSHijacking(attack);
                listViewExAttacks.AddEmbeddedControl(contr, 1, listViewExAttacks.Items.Count - 1);
                lvi.SubItems[1].Tag = contr;
            }
            else if (attack is InvalidMacSpoofAttackIpv4Attack)
            {
                ControlInvalidMacIpv4 contr = new ControlInvalidMacIpv4(attack);
                listViewExAttacks.AddEmbeddedControl(contr, 1, listViewExAttacks.Items.Count - 1);
                lvi.SubItems[1].Tag = contr;
            }
            else if (attack is DhcpIPv6)
            {
                ControlDHCPv6 contr = new ControlDHCPv6(attack);
                listViewExAttacks.AddEmbeddedControl(contr, 1, listViewExAttacks.Items.Count - 1);
                lvi.SubItems[1].Tag = contr;
            }
            else if (attack is DoSSLAACAttack)
            {
                ControlDosSlaac contr = new ControlDosSlaac(attack);
                listViewExAttacks.AddEmbeddedControl(contr, 1, listViewExAttacks.Items.Count - 1);
                lvi.SubItems[1].Tag = contr;
            }

            CheckBox cbActive = new CheckBox();
            if (attack.attackStatus == AttackStatus.Attacking)
                cbActive.Checked = true;
            else
                cbActive.Checked = false;
            cbActive.Tag = attack;
            cbActive.CheckedChanged += new EventHandler(cbActive_CheckedChanged);
            listViewExAttacks.AddEmbeddedControl(cbActive, 2, listViewExAttacks.Items.Count - 1);
        }

        void cbActive_CheckedChanged(object sender, EventArgs e)
        {
            Attack attack = (Attack)((CheckBox)sender).Tag;

            if (((CheckBox)sender).Checked == false)
                attack.attackStatus = AttackStatus.Stopping;
            else
                attack.attackStatus = AttackStatus.Attacking;
        }

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            StopSniffing();
        }

        private void btStartARPSpoofing_Click(object sender, EventArgs e)
        {
            List<IPAddress> lstGateway = panelMitmIpv4.GetGatewayList();
            List<IPAddress> lstTargets = panelMitmIpv4.GetTargetsList();

            if (lstGateway.Count == 0)
                ShowMessage("You've to set a 'gateway'", 1000, FormMessage.IconType.Error);
            else if (lstTargets.Count == 0)
                ShowMessage("You've to set at least any target", 1000, FormMessage.IconType.Error);
            else
            {
                IPAddress gateway = (IPAddress)lstGateway[0];

                foreach (IPAddress target in lstTargets)
                {
                    PhysicalAddress macTarget = Program.CurrentProject.data.GetNeighborMAC(target);
                    PhysicalAddress macGateway = Program.CurrentProject.data.GetNeighborMAC(gateway);
                    if ((macTarget == null) || (macGateway == null))
                        return; // Se descarta el target. Algo raro pasa, ¿Como tener la IP sin la mac en la lista de vecinos?

                    MitmAttack attack = new Data.MitmAttack(new Target(gateway, macGateway), new Target(target, macTarget), AttackType.ARPSpoofing);
                    Program.CurrentProject.data.AddAttack(attack);
                    attack.Start();

                    AddAttackToListViewEx(attack);

                    Program.LogThis("Performing a MITM (ARP) attack between " + attack.t1.ip.ToString() + " and " + attack.t2.ip.ToString(), Logs.Log.LogType.NeighborSpoofing);
                }

                panelNeighborAdvSpoof.ClearGateway();
                panelNeighborAdvSpoof.ClearTargets();
            }
        }

        private void interfaceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormInterfaces fInterfaces = new FormInterfaces(false);
            fInterfaces.ShowDialog();
            if (fInterfaces.device != null)
                Program.CurrentProject.data.SetDevice(fInterfaces.device);
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormSettings fs = new FormSettings();
            fs.ShowDialog();
        }

        private void cbWildcardDnsHijack_CheckedChanged(object sender, EventArgs e)
        {
            if (cbWildcardDnsHijack.Checked)
            {
                tbDomainDNSHijack.Text = "*";
                tbDomainDNSHijack.Enabled = false;
            }
            else
            {
                tbDomainDNSHijack.Text = "";
                tbDomainDNSHijack.Enabled = true;
            }
        }

        private void btAddDNSHijack_Click(object sender, EventArgs e)
        {
            try
            {
                IPAddress ip = IPAddress.Parse(tbIpDNSHijack.Text);
                DNSHijackAttack dnsHijack = new DNSHijackAttack(tbDomainDNSHijack.Text, ip, AttackType.DNSHijacking);
                Program.CurrentProject.data.AddAttack(dnsHijack);
                dnsHijack.Start();
                AddAttackToListViewEx(dnsHijack);
            }
            catch
            {
                ShowMessage("Invalid IP address", 1000, FormMessage.IconType.Error);
            }
        }

        private void treeView_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                TreeNode selectedNode = treeView.GetNodeAt(e.X, e.Y);
                treeView.SelectedNode = selectedNode;
            }
        }


        private void pictureI64_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.informatica64.com");
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormAbout fAbout = new FormAbout();
            fAbout.ShowDialog();
        }

        private void buttonStartSLAACDoS_Click(object sender, EventArgs e)
        {
            List<IPAddress> lstTargets = panelTargetSLAACDoS.GetTargetsList();

            if (lstTargets.Count == 0)
                ShowMessage("You've to set at least any target", 1000, FormMessage.IconType.Error);
            else
            {
                foreach (IPAddress target in lstTargets)
                {
                    PhysicalAddress macTarget = Program.CurrentProject.data.GetNeighborMAC(target);
                    if ((macTarget == null))
                        return; // Se descarta el target. Algo raro pasa, ¿Como tener la IP sin la mac en la lista de vecinos?

                    DoSSLAACAttack attack = new Data.DoSSLAACAttack(new Target(target, macTarget), AttackType.DoSSLAAC);
                    Program.CurrentProject.data.AddAttack(attack);
                    attack.Start();

                    AddAttackToListViewEx(attack);

                    Program.LogThis("Performing a IPv6 DoS (SLAAC) attack to " + attack.t1.ip.ToString(), Logs.Log.LogType.NeighborSpoofing);
                }
                panelTargetSLAACDoS.ClearTargets();
            }
        }

        private Form ShowForm(Form f)
        {
            f.StartPosition = FormStartPosition.CenterParent;
            f.ShowDialog();
            return f;
        }

        private void addNeighborToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bool added = false;

            FormAddNeighbor fAddNeighbor = new FormAddNeighbor();
            fAddNeighbor = (FormAddNeighbor)ShowForm(fAddNeighbor);

            if (fAddNeighbor.mac != null && fAddNeighbor.ip != null)
            {
                // Comprobar si ya existe un vecino con esa mac
                Neighbor n = Program.CurrentProject.data.GetNeighbor(fAddNeighbor.mac);
                if (n == null)
                {
                    // No existia. Se agrega.
                    n = new Neighbor();
                    n.physicalAddress = fAddNeighbor.mac;
                    n.AddIP(fAddNeighbor.ip);
                    Program.CurrentProject.data.AddNeighbor(n);
                    added = true;
                }
                else
                {
                    // Ya existia. Verificamos si ya tenia asignada la IP
                    if (n.ExistsIP(fAddNeighbor.ip))
                    {
                        // Existia
                        ShowMessage("IP Address '" + fAddNeighbor.ip.ToString() + "' is already asigned to a neighbor with '" + fAddNeighbor.mac.ToString() + "' as MAC address", 2000, FormMessage.IconType.Info);
                        return;
                    }
                    else
                    {
                        // No existia. Se agrega
                        n = new Neighbor();
                        n.physicalAddress = fAddNeighbor.mac;
                        n.AddIP(fAddNeighbor.ip);
                        Program.CurrentProject.data.AddNeighbor(n);
                        added = true;
                    }
                }

                if (added)
                    ShowMessage("IP '" + fAddNeighbor.ip.ToString() + "' added successfully with '" + fAddNeighbor.mac.ToString() + "' as MAC", 2000, FormMessage.IconType.Info);
            }
        }

        private void buttonDHCPACKInjection_Click(object sender, EventArgs e)
        {
            if (CheckDHCPACKConfigurationIpv4())
            {
                string gateway;
                if (checkBoxUseAsGateway.Checked)
                    gateway = Program.CurrentProject.data.GetIPv4FromDevice(Program.CurrentProject.data.GetDevice()).ToString();
                else
                    gateway = textBoxGateway.Text;
                string dns = textBoxDNS.Text;
                string MAC = string.Empty;
                if (radioButtonComputerWithMAC.Checked)
                    MAC = textBoxDHCPAckMAC.Text;
                DHCPACKInjectionAttack attack = new Data.DHCPACKInjectionAttack(AttackType.DHCPACKInjection, gateway, dns, MAC);
                Program.CurrentProject.data.AddAttack(attack);
                AddAttackToListViewEx(attack);
                Program.LogThis("Performing a IPv6 DHCP ACK Injection, waiting for new computers", Logs.Log.LogType.DHCPACKInjection);
            }
        }

        private bool CheckDHCPACKConfigurationIpv4()
        {
            IPAddress addr;
            if (!checkBoxUseAsGateway.Checked && !IPAddress.TryParse(textBoxGateway.Text, out addr))
            {
                ShowMessage("Invalid gateway address", 1000, FormMessage.IconType.Info);
                return false;
            }
            if (!IPAddress.TryParse(textBoxDNS.Text, out addr))
            {
                ShowMessage("Invalid DNS address", 1000, FormMessage.IconType.Info);
                return false;
            }
            if (radioButtonComputerWithMAC.Checked && !IsValidMAC(textBoxDHCPAckMAC.Text))
            {
                ShowMessage("Invalid MAC address", 1000, FormMessage.IconType.Info);
                return false;
            }
            return true;
        }

        private bool IsValidMAC(string mac)
        {
            try
            {
                PhysicalAddress.Parse(mac.ToUpper().Replace(":", "-"));
                return true;
            }
            catch
            {
                return false;
            }
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (treeView.SelectedNode != null)
                Clipboard.SetText(treeView.SelectedNode.Text);
        }

        private void cMenuNeighbors_Opening(object sender, CancelEventArgs e)
        {
            if (treeView.SelectedNode == null)
                copyToolStripMenuItem.Visible = false;
            else
                copyToolStripMenuItem.Visible = true;
        }

        private void checkBoxUseAsGateway_CheckedChanged(object sender, EventArgs e)
        {
            labelFakeGateway.Enabled = textBoxGateway.Enabled = !(sender as CheckBox).Checked;
            labelIPRouting.Visible = buttonEnableIPRouting.Visible = !Program.CurrentProject.data.settings.IPRoutingEnabled && (sender as CheckBox).Checked;
            bool r = Program.CurrentProject.data.settings.IPRoutingEnabled;
            bool t = r;
        }

        private void radioButtonComputerWithMAC_CheckedChanged(object sender, EventArgs e)
        {
            labelMAC.Enabled = textBoxDHCPAckMAC.Enabled = radioButtonComputerWithMAC.Checked;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btStartInvalidMacSpoof_Click(object sender, EventArgs e)
        {
            string error_InvalidIP = "Invalid IP Address";
            string error_notIpv4 = "This attack only allow IPv4 addresses";
            string error_macTargetNotFound = "The MAC address of the target has not been found";
            string error_macSpoofedNotFound = "The MAC address has not been found";

            IPAddress target = null;
            PhysicalAddress macTarget = null;
            IPAddress spoofedIp = null;
            PhysicalAddress macSpoofed = null;

            try
            {
                target = IPAddress.Parse(tbTargetSpoofedIpv4.Text);
            }
            catch
            {
                ShowMessage(error_InvalidIP, 1000, FormMessage.IconType.Error);
                return;
            }

            try
            {
                spoofedIp = IPAddress.Parse(tbIpSpoofedIpv4.Text);
            }
            catch
            {
                ShowMessage(error_InvalidIP, 1000, FormMessage.IconType.Error);
                return;
            }

            if ((target.AddressFamily != System.Net.Sockets.AddressFamily.InterNetwork) || (spoofedIp.AddressFamily != System.Net.Sockets.AddressFamily.InterNetwork))
            {
                ShowMessage(error_notIpv4, 1000, FormMessage.IconType.Error);
                return;
            }

            // Cogemos la mac del target de los vecinos
            macTarget = Program.CurrentProject.data.GetNeighborMAC(target);
            // En caso de que no la tengamos, le hacemos un ARP para coger su MAC
            if (macTarget == null)
            {
                string strMac = Utils.GetMacUsingARP(target.ToString());
                if (string.IsNullOrEmpty(strMac))
                {
                    ShowMessage(error_macTargetNotFound, 1000, FormMessage.IconType.Error);
                    return;
                }

                macTarget = PhysicalAddress.Parse(strMac.ToUpper());
            }

            // Cogemos la mac del target de los vecinos
            macSpoofed = Program.CurrentProject.data.GetNeighborMAC(spoofedIp);
            // En caso de que no la tengamos, le hacemos un ARP para coger su MAC
            if (macTarget == null)
            {
                string strMac = Utils.GetMacUsingARP(spoofedIp.ToString());
                if (string.IsNullOrEmpty(strMac))
                {
                    ShowMessage(error_macSpoofedNotFound, 1000, FormMessage.IconType.Error);
                    return;
                }
                macSpoofed = PhysicalAddress.Parse(strMac.ToUpper());
            }


            Data.InvalidMacSpoofAttackIpv4Attack invalidMacSpoofAttack = new InvalidMacSpoofAttackIpv4Attack(
                                                                            new Target(target, macTarget),
                                                                            new Target(spoofedIp, macSpoofed),
                                                                            AttackType.InvalidMacSpoofIpv4);

            Program.CurrentProject.data.AddAttack(invalidMacSpoofAttack);
            invalidMacSpoofAttack.Start();
            AddAttackToListViewEx(invalidMacSpoofAttack);
            Program.LogThis("Performing a DoS attack to " + invalidMacSpoofAttack.t1.ip.ToString() + " with " + invalidMacSpoofAttack.t2.ip.ToString(), Logs.Log.LogType.DoS);
        }

        private void pbInvMacSpoofTarget_Click(object sender, EventArgs e)
        {
            FormSelectIPs fSel = new FormSelectIPs(PanelTargets.TypeList.Ipv4, Data.Data.neighbors, false, null, null);
            fSel.ShowDialog();

            SynchronizedCollection<IPAddress> lstIps = fSel.GetSelectedIPs();

            if (lstIps.Count == 1)
            {
                tbTargetSpoofedIpv4.Text = lstIps[0].ToString();
            }
        }

        private void pbInvMacSpoofedIp_Click(object sender, EventArgs e)
        {
            FormSelectIPs fSel = new FormSelectIPs(PanelTargets.TypeList.Ipv4, Data.Data.neighbors, false, null, null);
            fSel.ShowDialog();

            SynchronizedCollection<IPAddress> lstIps = fSel.GetSelectedIPs();

            if (lstIps.Count == 1)
            {
                tbIpSpoofedIpv4.Text = lstIps[0].ToString();
            }
        }

        private void tabMitm_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void tabMitmIpv4_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (tabMitmIpv4.SelectedIndex)
            {
                case 0:
                    help.AddControl(new ControlHelp.MITMIpv4.ArpSpoofing.PanelArpSpoof());
                    break;
                case 1:
                    break;
                case 2:
                    break;
            }
        }

        private void tabMitmIpv4_Selected(object sender, TabControlEventArgs e)
        {
            switch (tabMitmIpv4.SelectedIndex)
            {
                case 0:
                    help.AddControl(new ControlHelp.MITMIpv4.ArpSpoofing.PanelArpSpoof());
                    break;
                case 1:
                    help.AddControl(new ControlHelp.MITMIpv4.DHCP_ACK_Injection.PanelDHCPAckInjection());
                    break;
                case 2:
                    help.AddControl(new ControlHelp.MITMIpv4.Wpad.PanelWpad());
                    break;
            }
        }

        private void tabMitm_Selected(object sender, TabControlEventArgs e)
        {
            switch (tabevilfoca.SelectedIndex)
            {
                case 0:
                    help.AddControl(new ControlHelp.evilfoca.NeighborAdvSpoofing.PanelNeighAdvSpoofing());
                    break;
                case 1:
                    help.AddControl(new ControlHelp.evilfoca.SLAAC.PanelSLAAC());
                    break;
                case 2:
                    help.AddControl(new ControlHelp.evilfoca.DHCPv6.panelDHCPv6());
                    break;
                case 3:
                    help.AddControl(new ControlHelp.evilfoca.WpadV6.PanelWpadV6());
                    break;
            }
        }

        private void panelMainOptions_Selected(object sender, TabControlEventArgs e)
        {
            switch (panelMainOptions.SelectedIndex)
            {
                case 0:
                    tabMitm_Selected(null, null);
                    break;
                case 1:
                    tabMitmIpv4_Selected(null, null);
                    break;
                case 2:
                    tabDoSIpv6_Selected(null, null);
                    break;
                case 3:
                    tabDoSIpv4_Selected(null, null);
                    break;
                case 4:
                    help.AddControl(new ControlHelp.DNSHijack.PanelDNSHijack());
                    break;
            }
        }

        private void tabDoSIpv6_Selected(object sender, TabControlEventArgs e)
        {
            switch (tabDoSIpv6.SelectedIndex)
            {
                case 0:
                    help.AddControl(new ControlHelp.DoSIPV6.SLAAC.PanelDoSSlaac());
                    break;
            }
        }

        private void tabDoSIpv4_Selected(object sender, TabControlEventArgs e)
        {
            switch (tabDoSIpv4.SelectedIndex)
            {
                case 0:
                    help.AddControl(new ControlHelp.DoSIpv4.InvalidMACSpoof.PanelInvalidMac());
                    break;
            }
        }

        private void buttonEnableIPRouting_Click(object sender, EventArgs e)
        {
            ProcessStartInfo processInfo = new ProcessStartInfo();
            processInfo.Verb = "runas";
            processInfo.FileName = Application.ExecutablePath;
            processInfo.Arguments = "ActivateIPRouting";
            try
            {
                Process.Start(processInfo);
            }
            catch
            {
                // Si el usuario cancela la elevación de permisos salta una excepcion
            }
        }

        private void btNetworkDiscovery_Click(object sender, EventArgs e)
        {
            if (networkDiscovery != null)
            {
                if (networkDiscovery.IsScanning)
                {
                    if (tNetworkDiscovery != null)
                    {
                        tNetworkDiscovery.Abort();
                        tNetworkDiscovery = null;
                    }
                    this.btNetworkDiscovery.Image = Properties.Resources.SearchStart;
                }
                else
                {
                    FormSelectSubNet fsubMask = new FormSelectSubNet();
                    if (fsubMask.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        if (fsubMask.IsMySubnetSelected)
                        {
                            tNetworkDiscovery = new Thread(new ThreadStart(networkDiscovery.StartNetworkDiscoveryJustOneTime));
                            tNetworkDiscovery.IsBackground = true;
                            this.btNetworkDiscovery.Image = Properties.Resources.SearchStop;
                            tNetworkDiscovery.Start();
                        }
                        else
                        {
                            tNetworkDiscovery = new Thread(new ParameterizedThreadStart(networkDiscovery.StartNetworkDiscoveryJustOneTime));
                            tNetworkDiscovery.IsBackground = true;
                            this.btNetworkDiscovery.Image = Properties.Resources.SearchStop;
                            tNetworkDiscovery.Start(new IPAddress[] { fsubMask.FromIP, fsubMask.ToIP });
                        }
                        ShowMessage("Discovering new neighbors", 0, FormMessage.IconType.Info);
                    }
                }
            }
        }

        private void btSearchRouters_Click(object sender, EventArgs e)
        {
            Thread tNeighborsCheckIfRoute = new Thread(new ThreadStart(_CheckIfNeighborsRoutePackets));
            tNeighborsCheckIfRoute.IsBackground = true;
            tNeighborsCheckIfRoute.Start();

            ShowMessage("Scanning for routers", 0, FormMessage.IconType.Info);
        }

        public static void ShowMessage(string sMessage, int retardo, FormMessage.IconType iconType)
        {
            FormMessage fMessage = new FormMessage(sMessage, retardo, iconType);
            fMessage.StartPosition = FormStartPosition.CenterParent;
            fMessage.Show(Program.formMain);
        }

        private void _CheckIfNeighborsRoutePackets()
        {
            IList<Neighbor> lstNeighbor = new List<Neighbor>(Data.Data.neighbors);
            foreach (Neighbor neighbor in lstNeighbor)
                Program.CurrentProject.data.CheckIfNeighborRoutesPackets(neighbor);
        }

        private void btStart_IPv6DHCP_Click(object sender, EventArgs e)
        {
            // El gateway no se puede asignar mediante DHCPv6. Unicamente se puede el dns?
            IPAddress dns = IPAddress.Parse(tbDNS_IPv6DHCP.Text);
            IPAddress ipRange = IPAddress.Parse(tbIPRange_IPv6DHCP.Text);

            DhcpIPv6 attack = new Data.DhcpIPv6(AttackType.DHCPIpv6, dns, ipRange);
            Program.CurrentProject.data.AddAttack(attack);
            attack.Start();
            AddAttackToListViewEx(attack);
        }

        private void btMitmSLAAC_Click(object sender, EventArgs e)
        {
            try
            {
                if (IPAddress.Parse(tbSlaacMitmPrefix.Text).GetAddressBytes().Length != 16)
                {
                    ShowMessage("Prefix must be an Ipv6 address", 1000, FormMessage.IconType.Error);
                    return;
                }
            }
            catch
            {
                ShowMessage("Invalid Prefix", 1000, FormMessage.IconType.Error);
                return;
            }
            List<IPAddress> lstTargets = panelTargetSLAACMitm.GetTargetsList();


            if (lstTargets.Count == 0)
                ShowMessage("You've to set at least any target", 1000, FormMessage.IconType.Error);
            else
            {
                foreach (IPAddress target in lstTargets)
                {
                    PhysicalAddress macTarget = Program.CurrentProject.data.GetNeighborMAC(target);

                    MitmAttack attack = new Data.MitmAttack(null, new Target(target, macTarget), AttackType.SlaacMitm) { prefix = (IPAddress.Parse(tbSlaacMitmPrefix.Text).GetAddressBytes()) };
                    Program.CurrentProject.data.AddAttack(attack);
                    attack.Start();

                    AddAttackToListViewEx(attack);

                    Program.LogThis(String.Format("Performing a MITM (SLAAC) attack between {0} and Attacker gateway ", attack.t2.ip.ToString()), Logs.Log.LogType.NeighborSpoofing);
                }

                panelTargetSLAACMitm.ClearTargets();
            }

        }

        private void btAddNeighbor_Click(object sender, EventArgs e)
        {
            addNeighborToolStripMenuItem_Click(null, null);
        }


        private void ForzePrintTreeview()
        {
            // Lo fuerza mediante eventos que saltaran al intentar agregar vecinos que ya están en memoria
            for (int i = 0; i < Data.Data.neighbors.Count; i++)
                Program.CurrentProject.data.AddNeighbor(Data.Data.neighbors[i]);
        }

        private void tbSearchFilter_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(tbSearchFilter.Text))
            {
                isFiltering = false;
                tbSearchFilter.BackColor = Color.White; // Color cuando no hay ningun filtro
                lbFilter.BackColor = Color.White;

                // Se fuerza a rescribir el treeview
                ForzePrintTreeview();

            }
            else
            {
                tbSearchFilter.BackColor = Color.Red; // Color cuando el fitro se ha modificado y no está aplicado
                lbFilter.BackColor = Color.Red;
            }
        }

        private void tbSearchFilter_KeyPress(object sender, KeyPressEventArgs e)
        {
            strFilter = tbSearchFilter.Text;

            if (e.KeyChar == '\r')
            {
                if (!string.IsNullOrEmpty(tbSearchFilter.Text))
                {
                    treeView.Invoke(new MethodInvoker(delegate
                        {
                            treeView.Nodes[mainNode].Nodes[neighborsNode].Nodes.Clear();
                            treeView.Nodes[mainNode].Nodes[routersNode].Nodes.Clear();
                            tbSearchFilter.BackColor = Color.ForestGreen; // Color cuando el fitro se ha aplicado
                            lbFilter.BackColor = Color.ForestGreen;
                        }));
                    isFiltering = true;

                    // Se fuerza a rescribir el treeview
                    ForzePrintTreeview();
                }
            }

        }

        private void lbFilter_Click(object sender, EventArgs e)
        {
            tbSearchFilter.Focus();
        }

        private void btActAsDNS_Click(object sender, EventArgs e)
        {
        }

        private void btnWpadAttack_Click(object sender, EventArgs e)
        {
            List<IPAddress> lstTargets = wpadTargetPanel.GetTargetsList();
            foreach (IPAddress target in lstTargets)
            {
                PhysicalAddress macTarget = Program.CurrentProject.data.GetNeighborMAC(target);
                if (macTarget == null)
                    return; // Se descarta el target. Algo raro pasa, ¿Como tener la IP sin la mac en la lista de vecinos?

                MitmAttack attack = new Data.MitmAttack(new Target(Program.CurrentProject.data.GetIPv4FromDevice(Program.CurrentProject.data.GetDevice()), null), new Target(target, macTarget), AttackType.WpadIPv4);
                Program.CurrentProject.data.AddAttack(attack);
                attack.Start();

                AddAttackToListViewEx(attack);

                Program.LogThis("Performing a MITM (WPAD) attack " + attack.t2.ip.ToString(), Logs.Log.LogType.WpadIPv4);
            }
            wpadTargetPanel.ClearTargets();
        }

        private void btnWpadv6Attack_Click(object sender, EventArgs e)
        {
            List<IPAddress> lstTargets = wpadv6TargetPanel.GetTargetsList();
            foreach (IPAddress target in lstTargets)
            {
                PhysicalAddress macTarget = Program.CurrentProject.data.GetNeighborMAC(target);
                if (macTarget == null)
                    return; // Se descarta el target. Algo raro pasa, ¿Como tener la IP sin la mac en la lista de vecinos?

                MitmAttack attack = new Data.MitmAttack(new Target(Program.CurrentProject.data.GetIPv6LocalLinkFromDevice(Program.CurrentProject.data.GetDevice()), null), new Target(target, macTarget), AttackType.WpadIPv6);
                Program.CurrentProject.data.AddAttack(attack);
                attack.Start();

                AddAttackToListViewEx(attack);

                Program.LogThis("Performing a MITM (WPAD) attack " + attack.t2.ip.ToString(), Logs.Log.LogType.WpadIPv6);
            }
            wpadv6TargetPanel.ClearTargets();
        }
    }
}
