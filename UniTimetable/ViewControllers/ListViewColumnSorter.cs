using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Windows.Forms;

namespace UniTimetable
{
    class ListViewColumnSorter : IComparer
    {
        private List<int> ColumnOrder_;
        private List<SortOrder> OrderOfSort_;
        private CaseInsensitiveComparer ObjectCompare_;
        private List<bool> NameOfDay_;
        private List<bool> LeadingZeros_;

        public ListViewColumnSorter()
        {
            ColumnOrder_ = new List<int>();
            OrderOfSort_ = new List<SortOrder>();
            ObjectCompare_ = new CaseInsensitiveComparer();
            NameOfDay_ = new List<bool>();
            LeadingZeros_ = new List<bool>();
        }

        private static int MaxDigits(string text)
        {
            int length = 0;
            int maxLength = 0;
            for (int i = 0; i < text.Length; i++)
            {
                if (text.IndexOfAny("0123456789".ToCharArray(), i, 1) > -1)
                {
                    length++;
                    if (length > maxLength)
                        maxLength = length;
                }
                else
                {
                    length = 0;
                }
            }
            return maxLength;
        }

        private static int MaxDigits(string textX, string textY)
        {
            int digitsX = MaxDigits(textX);
            int digitsY = MaxDigits(textY);
            if (digitsX > digitsY)
            {
                return digitsX;
            }
            else
            {
                return digitsY;
            }
        }

        private string InsertLeadingZeros(string text, int digits)
        {
            int length;
            for (int i = 0; i < text.Length; i++)
            {
                if (text.IndexOfAny("0123456789".ToCharArray(), i, 1) > -1)
                {
                    length = 1;
                    while (i + length < text.Length && text.IndexOfAny("0123456789".ToCharArray(), i + length, 1) > -1)
                        length++;
                    while (length < digits)
                    {
                        text = text.Insert(i, "0");
                        length++;
                    }
                    i += length;    //not (length-1) as following character is not a digit
                }
            }
            return text;
        }

        public void ClearLists()
        {
            ColumnOrder_.Clear();
            OrderOfSort_.Clear();
            NameOfDay_.Clear();
            LeadingZeros_.Clear();
        }

        public void AddSorter(int column, SortOrder order)
        {
            AddSorter(column, order, false, false);
        }

        public void AddSorter(int column, SortOrder order, bool day, bool zeros)
        {
            ColumnOrder_.Add(column);
            OrderOfSort_.Add(order);
            NameOfDay_.Add(day);
            LeadingZeros_.Add(zeros);
        }

        #region IComparer Members

        public int Compare(object x, object y)
        {
            int compareResult = 0;
            ListViewItem itemX = (ListViewItem)x, itemY = (ListViewItem)y;

            for (int i = 0; i < ColumnOrder_.Count && i < OrderOfSort_.Count; i++)
            {
                string textX = itemX.SubItems[ColumnOrder_[i]].Text;
                string textY = itemY.SubItems[ColumnOrder_[i]].Text;

                if (NameOfDay_[i])
                {
                    if (Timetable.Days.Contains(textX))    //prefix with (char)1 and numeric index of day
                    {
                        textX = (char)1 + Timetable.Days.IndexOf(textX).ToString() + textX;
                    }
                    if (Timetable.Days.Contains(textY))
                    {
                        textY = (char)1 + Timetable.Days.IndexOf(textY).ToString() + textY;
                    }
                }

                if (LeadingZeros_[i])
                {
                    int digits = MaxDigits(textX, textY);
                    textX = InsertLeadingZeros(textX, digits);
                    textY = InsertLeadingZeros(textY, digits);
                }

                compareResult = ObjectCompare_.Compare(textX, textY);
                if (compareResult == 0)
                    continue;
                if (OrderOfSort_[i] == SortOrder.Ascending)
                {
                    return compareResult;
                }
                else if (OrderOfSort_[i] == SortOrder.Descending)
                {
                    return (-compareResult);
                }
            }

            return 0;
        }

        #endregion
    }
}
