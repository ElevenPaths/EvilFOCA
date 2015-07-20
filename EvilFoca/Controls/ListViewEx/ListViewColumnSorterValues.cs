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
using System.Collections;
using System.Windows.Forms;

namespace evilfoca.Controls.ListViewEx
{
    public class ListViewColumnSorterValues : IComparer
    {
        public int SortColumn { set; get; }
        public SortOrder Order { set; get; }
        private CaseInsensitiveComparer ObjectCompare;

        public ListViewColumnSorterValues()
        {
            SortColumn = 0;
            Order = SortOrder.None;
            ObjectCompare = new CaseInsensitiveComparer();
        }

        public int Compare(object x, object y)
        {

            ListViewItem listviewX = (ListViewItem)x;
            ListViewItem listviewY = (ListViewItem)y;

            int compareResult = 0;
            if (SortColumn == 0)
                compareResult = ObjectCompare.Compare(listviewX.SubItems[SortColumn].Text, listviewY.SubItems[SortColumn].Text);
            else if (SortColumn == 1)
            {
                if (listviewX.SubItems.Count != 2 || listviewY.SubItems.Count != 2)
                    return 0;
                else
                {
                    int xi, yi;
                    if (int.TryParse(listviewX.SubItems[SortColumn].Text, out xi) &&
                        int.TryParse(listviewY.SubItems[SortColumn].Text, out yi))
                    {
                        compareResult = xi - yi;
                    }
                    else
                    {
                        compareResult = string.Compare(listviewX.SubItems[SortColumn].Text, listviewY.SubItems[SortColumn].Text);
                    }
                }
            }
            if (Order == SortOrder.Ascending)
                return compareResult;
            else if (Order == SortOrder.Descending)
                return -compareResult;
            else
                return 0;
        }
    }
}
