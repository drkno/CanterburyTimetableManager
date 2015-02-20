using System.Collections.Generic;
using System.Drawing;

namespace UniTimetable.Model
{
    public class ColorScheme
    {
        public static readonly ColorScheme[] Schemes =
        {
            new ColorScheme("Default", new[]
                                       {
                                           Color.Red,
                                           Color.Blue,
                                           Color.Green,
                                           Color.Gold,
                                           Color.Purple,
                                           Color.Orange,
                                           Color.SeaGreen
                                       }),
            new ColorScheme("Bushfire", new[]
                                        {
                                            Color.Gold,
                                            Color.Orange,
                                            Color.OrangeRed,
                                            Color.Red,
                                            Color.Maroon
                                        }),
            new ColorScheme("Forest", new[]
                                      {
                                          Color.FromArgb(127, 142, 43),
                                          Color.FromArgb(94, 119, 3),
                                          Color.FromArgb(114, 105, 77),
                                          Color.FromArgb(70, 77, 38),
                                          Color.FromArgb(129, 95, 62)
                                      }),
            new ColorScheme("Mellow", new[]
                                      {
                                          Color.FromArgb(146, 115, 101),
                                          Color.FromArgb(147, 104, 117),
                                          Color.FromArgb(132, 112, 134),
                                          Color.FromArgb(92, 123, 142),
                                          Color.FromArgb(106, 139, 137),
                                          Color.FromArgb(121, 137, 109)
                                      }),
            new ColorScheme("Rainbow", new[]
                                       {
                                           Color.FromArgb(0, 167, 216),
                                           Color.FromArgb(90, 221, 69),
                                           Color.FromArgb(255, 232, 5),
                                           Color.FromArgb(255, 162, 67),
                                           Color.FromArgb(255, 87, 94),
                                           Color.FromArgb(255, 28, 172),
                                           Color.FromArgb(215, 8, 178)
                                       })
        };

        public List<Color> Colors;
        public string Name;

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