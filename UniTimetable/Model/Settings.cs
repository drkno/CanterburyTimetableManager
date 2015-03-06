namespace UniTimetable.Model
{
    public class Settings
    {
        public int HourEnd { get; set; }
        public int HourStart { get; set; }
        public bool ResetWindow { get; set; }
        public bool ShowGhost { get; private set; }
        public bool ShowGray { get; private set; }
        public bool ShowLocation { get; private set; }
        public bool ShowWeekend { get; private set; }
        public bool ImportUnselectable { get; private set; }

        public Settings()
        {
            ImportUnselectable = Properties.Settings.Default.ImportUnselectable;
            ShowGhost = Properties.Settings.Default.ShowGhost;
            ShowWeekend = Properties.Settings.Default.ShowWeekend;
            ShowGray = Properties.Settings.Default.ShowGray;
            ShowLocation = Properties.Settings.Default.ShowLocation;
            HourStart = Properties.Settings.Default.HourStart;
            HourEnd = Properties.Settings.Default.HourEnd;
            ResetWindow = false;
        }

        public Settings(
            bool importUnselectable,
            bool showGhost,
            bool showWeekend,
            bool showGray,
            bool showLocation,
            int hourStart,
            int hourEnd,
            bool resetWindow)
        {
            ImportUnselectable = importUnselectable;
            ShowGhost = showGhost;
            ShowWeekend = showWeekend;
            ShowGray = showGray;
            ShowLocation = showLocation;
            HourStart = hourStart;
            HourEnd = hourEnd;
            ResetWindow = resetWindow;
        }

        public void Save()
        {
            Properties.Settings.Default.ImportUnselectable = ImportUnselectable;
            Properties.Settings.Default.ShowGhost = ShowGhost;
            Properties.Settings.Default.ShowWeekend = ShowWeekend;
            Properties.Settings.Default.ShowLocation = ShowGray;
            Properties.Settings.Default.ShowLocation = ShowLocation;
            Properties.Settings.Default.HourStart = HourStart;
            Properties.Settings.Default.HourEnd = HourEnd;
            Properties.Settings.Default.Save();
        }
    }
}