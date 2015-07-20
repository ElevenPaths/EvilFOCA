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
using System.Net;
using System.Windows.Forms;

namespace evilfoca.Controls.IpTextBox
{
    public enum IPVersion
    {
        IPv4,
        IPv6,
        Both
    }
    public class IPTextBox : TextBox
    {
        private IPVersion _version;
        public bool IsValid
        {
            get
            {
                return ValidateIPFromString(this.Text);
            }
        }

        public IPVersion Version
        {
            get
            {
                return _version;
            }
            set
            {
                _version = value;
                if (_version == IPVersion.IPv4)
                    this.MaxLength = 15;
                else
                    this.MaxLength = 39;
            }

        }

        public IPAddress IpValue
        {
            get
            {
                IPAddress addr;
                if (ValidateIPFromString(this.Text, out addr))
                    return addr;
                else
                    return null;
            }
        }

        public void IpTextBox()
        {
            this.Version = IPVersion.IPv4;
            this.MaxLength = 12;
        }

        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            base.OnKeyPress(e);

            switch (this.Version)
            {
                case IPVersion.IPv4:
                    if (!char.IsDigit(e.KeyChar) && e.KeyChar != '.' && e.KeyChar != (char)Keys.Back)
                        e.Handled = true;
                    break;
                case IPVersion.IPv6:
                    if (!Uri.IsHexDigit(e.KeyChar) && e.KeyChar != ':' && e.KeyChar != (char)Keys.Back)
                        e.Handled = true;
                    break;
                case IPVersion.Both:
                    if (!char.IsDigit(e.KeyChar) && e.KeyChar != '.' && !Uri.IsHexDigit(e.KeyChar) && e.KeyChar != ':' && e.KeyChar != (char)Keys.Back)
                        e.Handled = true;
                    break;
            }
        }

        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {

                case 0x302: //WM_PASTE
                    {
                        string text = Clipboard.GetText();
                        if (ValidateIPFromString(text))
                            this.Text = text;
                        break;
                    }
            }
            base.WndProc(ref m);
        }

        private bool ValidateIPFromString(string text)
        {
            IPAddress addr = null;
            return IPAddress.TryParse(text, out addr);
        }

        private bool ValidateIPFromString(string text, out IPAddress addr)
        {
            addr = null;
            return IPAddress.TryParse(text, out addr);
        }
    }
}
