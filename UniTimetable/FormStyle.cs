using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace UniTimetable
{
    partial class FormStyle : Form
    {
        Timetable Timetable_;

        List<Color> ColorList_ = new List<Color>();

        public FormStyle()
        {
            InitializeComponent();

            ddSchemes.Items.Clear();
            ddSchemes.Items.AddRange(ColorScheme.Schemes);
        }

        public DialogResult ShowDialog(Timetable timetable)
        {
            Timetable_ = timetable;
            return base.ShowDialog();
        }

        public new DialogResult ShowDialog()
        {
            throw new Exception("No input was provided.");
        }

        public void UpdateButton()
        {
            if (listBox1.SelectedIndex == -1)
            {
                btnColor.Enabled = false;
            }
            else
            {
                btnColor.Enabled = true;
            }
        }

        private void FormStyle_Load(object sender, EventArgs e)
        {
            // copy colors into color list and subject names to list box
            ColorList_.Clear();
            listBox1.Items.Clear();
            foreach (Subject subject in Timetable_.SubjectList)
            {
                ColorList_.Add(subject.Color);
                listBox1.Items.Add(subject);
            }
            ddSchemes.SelectedIndex = -1;
            UpdateButton();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateButton();
        }

        private void btnColor_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() != DialogResult.Cancel)
            {
                ColorList_[listBox1.SelectedIndex] = colorDialog1.Color;
                listBox1.Invalidate();
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            bool changed = false;
            // copy the colours back
            for (int i = 0; i < ColorList_.Count; i++)
            {
                if (Timetable_.SubjectList[i].Color != ColorList_[i])
                {
                    changed = true;
                    Timetable_.SubjectList[i].Color = ColorList_[i];
                }
            }
            DialogResult = (changed ? DialogResult.OK : DialogResult.Cancel);
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void listBox1_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index == -1)
                return;

            const int margin = 2;

            e.DrawBackground();
            e.DrawFocusRectangle();

            Graphics g = e.Graphics;
            Subject subject = (Subject)listBox1.Items[e.Index];

            Rectangle r = new Rectangle(e.Bounds.X + margin, e.Bounds.Y + margin, e.Bounds.Width - 2 * margin - 1, e.Bounds.Height - 2 * margin - 1);
            g.FillRectangle(TimetableControl.LinearGradient(r.Location, r.Width, r.Height, ColorList_[e.Index]), r);

            StringFormat format = new StringFormat();
            format.Alignment = StringAlignment.Center;
            format.LineAlignment = StringAlignment.Center;
            g.DrawString(subject.ToString(), listBox1.Font, Brushes.Black, r, format);

            g.DrawRectangle(Pens.Black, r);
        }

        private void ddSchemes_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddSchemes.SelectedIndex == -1)
                return;
            ColorScheme scheme = (ColorScheme)ddSchemes.SelectedItem;
            LoadScheme(scheme);
        }

        private void LoadScheme(ColorScheme scheme)
        {
            int n = scheme.Colors.Count;
            for (int i = 0; i < ColorList_.Count; i++)
            {
                ColorList_[i] = scheme.Colors[i % n];
            }
            listBox1.Invalidate();
        }

    }

    public class ColorScheme
    {
        public string Name;
        public List<Color> Colors;

        public static readonly ColorScheme[] Schemes = {
            new ColorScheme("Default", new Color[] {
                Color.Red,
                Color.Blue,
                Color.Green,
                Color.Gold,
                Color.Purple,
                Color.Orange,
                Color.SeaGreen
            }),
            new ColorScheme("Bushfire", new Color[] {
                Color.Gold,
                Color.Orange,
                Color.OrangeRed,
                Color.Red,
                Color.Maroon
            }),
            new ColorScheme("Forest", new Color[] {
                Color.FromArgb(127, 142, 43),
                Color.FromArgb(94, 119, 3),
                Color.FromArgb(114, 105, 77),
                Color.FromArgb(70, 77, 38),
                Color.FromArgb(129, 95, 62)
            }),
            new ColorScheme("Mellow", new Color[] {
                Color.FromArgb(146, 115, 101),
                Color.FromArgb(147, 104, 117),
                Color.FromArgb(132, 112, 134),
                Color.FromArgb(92, 123, 142),
                Color.FromArgb(106, 139, 137),
                Color.FromArgb(121, 137, 109)
            }),
            new ColorScheme("Rainbow", new Color[] {
                Color.FromArgb(0, 167, 216),
                Color.FromArgb(90, 221, 69),
                Color.FromArgb(255, 232, 5),
                Color.FromArgb(255, 162, 67),
                Color.FromArgb(255, 87, 94),
                Color.FromArgb(255, 28, 172),
                Color.FromArgb(215, 8, 178)
            })
        };

        public ColorScheme()
        {
            Name = "Empty";
            Colors = new List<Color>();
        }

        public ColorScheme(string name, IEnumerable<Color> colors)
        {
            Name = name;
            Colors = new List<Color>(colors);
        }

        public override string ToString()
        {
            return Name;
        }
    }
}