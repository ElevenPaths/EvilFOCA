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
using System;
using System.Reflection;
using System.Windows.Forms;
using Microsoft.Win32;
using SharpPcap;
using SharpPcap.WinPcap;

namespace evilfoca
{
    static class Program
    {
        public static Project.Project CurrentProject;
        public static FormMain formMain;
        public static FormSplash fSplash;

        public static bool runtime = false;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            runtime = true;
            CurrentProject = new Project.Project();
            if (CheckArgsToElevate())
                return;

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            System.Threading.Thread tSplash = new System.Threading.Thread(new System.Threading.ThreadStart(delegate
                {
                    fSplash = new FormSplash();
                    Application.Run(fSplash);
                }));
            tSplash.Start();

            if (!(IsWinPcapIsInstalled()))
            {
                tSplash.Abort();
                MessageBox.Show("WinPcap library not found or no compatible device found. This is a mandatory requirement.\r\n\r\n\r\nPlease, download and install it from http://www.winpcap.org/.", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Application.Exit();
                return;
            }

            int defaultInterface = Program.CurrentProject.data.settings.Interface;
            //La primera vez se muestra, el resto no

            WinPcapDevice dev = null;
            if (defaultInterface > -1 && defaultInterface < CaptureDeviceList.Instance.Count)
            {
                dev = (WinPcapDevice)CaptureDeviceList.Instance[defaultInterface];
            }

            while (dev == null || Program.CurrentProject.data.GetIPv6LocalLinkFromDevice(dev) == null)
            {
                if (dev != null)
                    MessageBox.Show("IP Address Local-Link hasn't been found. Please, turn on IPv6 on your network interface and restart the application or select other interface", "IP Address Local-Link hasn't been found",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                FormInterfaces fInterfaces = new FormInterfaces(true, true);
                if (fInterfaces.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                {
                    tSplash.Abort();
                    Application.Exit();
                    return;
                }

                dev = fInterfaces.device;
            }
            Program.CurrentProject.data.SetDevice(dev);

            Program.UriDotBug();

            formMain = new FormMain();

            Application.Run(formMain);
        }

        private static bool IsWinPcapIsInstalled()
        {
            try
            {
                int cuenta = CaptureDeviceList.Instance.Count;
            }
            catch (DllNotFoundException)
            {
                return false; // Pcap no instalada
            }
            catch
            {
                return false; // Ppcap instalada, pero se ha producido algun error. Posiblemente versiones antiguas.
            }

            return true;
        }

        public static void LogThis(string message, Logs.Log.LogType logType)
        {
            Logs.Log log = new Logs.Log(message, logType);

            formMain.panelLogs.Invoke(new MethodInvoker(delegate
            {
                formMain.panelLogs.AddLog(log);
            }));
        }

        public static bool CheckArgsToElevate()
        {
            if (Environment.GetCommandLineArgs().Length == 2 && Environment.GetCommandLineArgs()[1] == "ActivateIPRouting")
            {
                System.Diagnostics.Debugger.Break();
                ActivateIPRouting();
                //Guardar en la configuracion
                CurrentProject.data.settings.IPRoutingEnabled = true;
                MessageBox.Show("IP Enable Routing activado");
                return true;
            }
            return false;
        }

        public static void ActivateIPRouting()
        {
            RegistryKey rk = Registry.LocalMachine.OpenSubKey(@"SYSTEM\CurrentControlSet\services\TcpIp\Parameters", true);
            rk.SetValue("IPEnableRouter", 1, RegistryValueKind.DWord);
        }


        private static void UriDotBug()
        {
            MethodInfo getSyntax = typeof(UriParser).GetMethod("GetSyntax", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic);
            FieldInfo flagsField = typeof(UriParser).GetField("m_Flags", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            if (getSyntax != null && flagsField != null)
            {
                foreach (string scheme in new[] { "http", "https" })
                {
                    UriParser parser = (UriParser)getSyntax.Invoke(null, new object[] { scheme });
                    if (parser != null)
                    {
                        int flagsValue = (int)flagsField.GetValue(parser);
                        // Clear the CanonicalizeAsFilePath attribute
                        if ((flagsValue & 0x1000000) != 0)
                            flagsField.SetValue(parser, flagsValue & ~0x1000000);
                    }
                }
            }
        }
    }
}
