#region

using System;
using System.Drawing;
using System.Windows.Forms;
using UniTimetable.Properties;
using UniTimetable.ViewControllers;

#endregion

namespace UniTimetable.Model.Import
{
    internal class Importer
    {
        protected Importer()
        {
            File1Dialog_ = DefaultFileDialog;
            File2Dialog_ = DefaultFileDialog;
            File3Dialog_ = DefaultFileDialog;
        }

        private static OpenFileDialog DefaultFileDialog
        {
            get
            {
                var dialog = new OpenFileDialog();
                dialog.RestoreDirectory = true;
                return dialog;
            }
        }

        #region Property variables

        public bool RequiresPassword;

        protected string FormatName_ = null;
        protected string University_ = null;
        protected string CreatedBy_ = null;
        protected string LastUpdated_ = null;

        protected string File1Description_ = null;
        protected string File2Description_ = null;
        protected string File3Description_ = null;
        protected string FileInstructions_ = null;

        protected OpenFileDialog File1Dialog_;
        protected OpenFileDialog File2Dialog_;
        protected OpenFileDialog File3Dialog_;

        protected Image Logo_ = Resources.Unknown;

        #endregion

        #region Property accessors

        public string FormatName
        {
            get { return FormatName_; }
        }

        public string University
        {
            get { return University_; }
        }

        public string CreatedBy
        {
            get { return CreatedBy_; }
        }

        public string LastUpdated
        {
            get { return LastUpdated_; }
        }

        public string File1Description
        {
            get { return File1Description_; }
        }

        public string File2Description
        {
            get { return File2Description_; }
        }

        public string File3Description
        {
            get { return File3Description_; }
        }

        public string FileInstructions
        {
            get { return FileInstructions_; }
        }

        public OpenFileDialog File1Dialog
        {
            get { return File1Dialog_; }
        }

        public OpenFileDialog File2Dialog
        {
            get { return File2Dialog_; }
        }

        public OpenFileDialog File3Dialog
        {
            get { return File3Dialog_; }
        }

        public Image Logo
        {
            get { return Logo_; }
        }

        #endregion

        #region Importing

        /// <summary>
        ///     Takes the user-selected files and builds the stream data for a timetable.
        /// </summary>
        /// <returns>The stream data parsed from the files if it succeeded, null if it failed.</returns>
        protected virtual Timetable.Timetable Parse()
        {
            throw new Exception("Parse method not implemented!");
        }

        protected string Username, Password;

        public virtual void SetLogin(string user, string pass)
        {
            Username = user;
            Password = pass;
        }

        public Timetable.Timetable Import()
        {
            // try and parse files
            Timetable.Timetable t = Parse();
            // if parsing failed
            if (t == null || !t.HasData())
            {
                return null;
            }
            // do colours
            SetColors(t);

            return t;
        }

        private void SetColors(Timetable.Timetable timetable)
        {
            ColorScheme scheme = ColorScheme.Schemes[0];
            for (int i = 0; i < timetable.SubjectList.Count; i++)
            {
                timetable.SubjectList[i].Color = scheme.Colors[i%scheme.Colors.Count];
                /*switch (i % 6)
                {
                    case 0:
                        timetable.SubjectList[i].Color = Color.Red;
                        break;
                    case 1:
                        timetable.SubjectList[i].Color = Color.Blue;
                        break;
                    case 2:
                        timetable.SubjectList[i].Color = Color.Green;
                        break;
                    case 3:
                        timetable.SubjectList[i].Color = Color.Yellow;
                        break;
                    case 4:
                        timetable.SubjectList[i].Color = Color.Purple;
                        break;
                    case 5:
                        timetable.SubjectList[i].Color = Color.Orange;
                        break;
                    default:
                        timetable.SubjectList[i].Color = Color.White;
                        break;
                }*/
            }
        }

        #endregion

        #region Overloaded sealed base members

        public override sealed string ToString()
        {
            return FormatName_;
        }

        public override sealed bool Equals(object obj)
        {
            return ReferenceEquals(this, obj);
        }

        public override sealed int GetHashCode()
        {
            return (FormatName_.Length + University_.Length + CreatedBy_.Length + LastUpdated_.Length)%16;
        }

        #endregion
    }
}