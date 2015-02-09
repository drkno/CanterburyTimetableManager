namespace UniTimetable.Model
{
    public class Settings
    {
        public int HourEnd;
        public int HourStart;
        public bool ResetWindow;
        public bool ShowGhost;
        public bool ShowGray;
        public bool ShowLocation;
        public bool ShowWeekend;

        public Settings()
        {
            Load();
        }

        public Settings(
            bool showGhost,
            bool showWeekend,
            bool showGray,
            bool showLocation,
            int hourStart,
            int hourEnd,
            bool resetWindow)
        {
            ShowGhost = showGhost;
            ShowWeekend = showWeekend;
            ShowGray = showGray;
            ShowLocation = showLocation;
            HourStart = hourStart;
            HourEnd = hourEnd;
            ResetWindow = resetWindow;
        }

        public Settings(Settings other)
        {
            ShowGhost = other.ShowGhost;
            ShowWeekend = other.ShowWeekend;
            ShowGray = other.ShowGray;
            ShowLocation = other.ShowLocation;
            HourStart = other.HourStart;
            HourEnd = other.HourEnd;
            ResetWindow = other.ResetWindow;
        }

        public void Load()
        {
            ShowGhost = Properties.Settings.Default.ShowGhost;
            ShowWeekend = Properties.Settings.Default.ShowWeekend;
            ShowGray = Properties.Settings.Default.ShowGray;
            ShowLocation = Properties.Settings.Default.ShowLocation;
            HourStart = Properties.Settings.Default.HourStart;
            HourEnd = Properties.Settings.Default.HourEnd;
            ResetWindow = false;
        }

        public void Save()
        {
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