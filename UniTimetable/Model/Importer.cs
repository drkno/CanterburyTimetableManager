using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Windows.Forms;
using System.IO;
using System.Drawing;
using System.Text.RegularExpressions;
using UniTimetable.CanterburyData;
using UniTimetable.Properties;

namespace UniTimetable
{
    class Importer
    {
        #region Property variables

        public bool RequiresPassword = false;

        protected string FormatName_ = null;
        protected string University_ = null;
        protected string CreatedBy_ = null;
        protected string LastUpdated_ = null;

        protected string File1Description_ = null;
        protected string File2Description_ = null;
        protected string File3Description_ = null;
        protected string FileInstructions_ = null;

        protected OpenFileDialog File1Dialog_ = null;
        protected OpenFileDialog File2Dialog_ = null;
        protected OpenFileDialog File3Dialog_ = null;

        protected Image Logo_ = Properties.Resources.Unknown;

        #endregion

        #region Property accessors

        public string FormatName { get { return FormatName_; } }
        public string University { get { return University_; } }
        public string CreatedBy { get { return CreatedBy_; } }
        public string LastUpdated { get { return LastUpdated_; } }

        public string File1Description { get { return File1Description_; } }
        public string File2Description { get { return File2Description_; } }
        public string File3Description { get { return File3Description_; } }
        public string FileInstructions { get { return FileInstructions_; } }

        public OpenFileDialog File1Dialog { get { return File1Dialog_; } }
        public OpenFileDialog File2Dialog { get { return File2Dialog_; } }
        public OpenFileDialog File3Dialog { get { return File3Dialog_; } }

        public Image Logo { get { return Logo_; } }

        #endregion

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

        #region Importing

        /// <summary>
        /// Takes the user-selected files and builds the stream data for a timetable.
        /// </summary>
        /// <returns>The stream data parsed from the files if it succeeded, null if it failed.</returns>
        protected virtual Timetable Parse()
        {
            throw new Exception("Parse method not implemented!");
        }

        protected string Username, Password;

        public virtual void SetLogin(string user, string pass)
        {
            Username = user;
            Password = pass;
        }

        public Timetable Import()
        {
            // try and parse files
            Timetable t = Parse();
            // if parsing failed
            if (t == null || !t.HasData())
            {
                return null;
            }
            // do colours
            SetColors(t);

            return t;
        }

        private void SetColors(Timetable timetable)
        {
            ColorScheme scheme = ColorScheme.Schemes[0];
            for (int i = 0; i < timetable.SubjectList.Count; i++)
            {
                timetable.SubjectList[i].Color = scheme.Colors[i % scheme.Colors.Count];
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

        public sealed override string ToString()
        {
            return FormatName_;
        }

        public sealed override bool Equals(object obj)
        {
            return Importer.ReferenceEquals(this, obj);
        }

        public sealed override int GetHashCode()
        {
            return (FormatName_.Length + University_.Length + CreatedBy_.Length + LastUpdated_.Length) % 16;
        }

        #endregion
    }

    class UnocImporter : Importer
    {
        public UnocImporter()
        {
            RequiresPassword = true;
        }

        protected override Timetable Parse()
        {
            try
            {
                var cantaLoader = new CanterburyLoader(Username, Password);
                cantaLoader.GetData();
                var timetable = new Timetable();
                foreach (var data in cantaLoader.CanterburyDatas)
                {
                    foreach (var subs in data.SubjectStreams)
                    {
                        var endTime = subs.Value.Date.AddMinutes(double.Parse(subs.Value.duration));
                        int currentDay;
                        switch (subs.Value.day_of_week)
                        {
                            case "Sun":
                                currentDay = 0;
                                break;
                            case "Mon":
                                currentDay = 1;
                                break;
                            case "Tue":
                                currentDay = 2;
                                break;
                            case "Wed":
                                currentDay = 3;
                                break;
                            case "Thu":
                                currentDay = 4;
                                break;
                            case "Fri":
                                currentDay = 5;
                                break;
                            case "Sat":
                                currentDay = 6;
                                break;
                            default:
                                continue;
                        }
                        var session = new Session(currentDay, subs.Value.Date.Hour,
                            subs.Value.Date.Minute, endTime.Hour, endTime.Minute, subs.Value.location);
                        
                        Subject subject;
                        if (timetable.SubjectList.Exists(element => element.Name == subs.Value.subject_code))
                        {
                            subject = timetable.SubjectList.Find(element => element.Name == subs.Value.subject_code);
                        }
                        else
                        {
                            subject = new Subject(subs.Value.subject_code);
                            timetable.SubjectList.Add(subject);
                        }

                        // Set the session type
                        Type type;
                        if (subject.Types.Exists(types => types.Code == subs.Value.activity_group_code))
                        {
                            type = subject.Types.Find(types => types.Code == subs.Value.activity_group_code);
                        }
                        else // The session type doesn't exist, create it.
                        {
                            type = new Type(subs.Value.activity_type, subs.Value.activity_group_code, subject);
                            switch (subs.Value.activity_group_code)
                            {
                                case "tes":
                                    type.Required = false; break;
                                default:
                                    type.Required = true; break;
                            }
                            timetable.TypeList.Add(type);
                        }

                        // Set the session
                        Stream stream;
                        if (type.Streams.Exists(x => x.Number == int.Parse(subs.Value.activity_code)))
                        {
                            stream = type.Streams.Find(x => x.Number == int.Parse(subs.Value.activity_code));
                        }
                        else
                        {
                            stream = new Stream(int.Parse(subs.Value.activity_code));
                            timetable.StreamList.Add(stream); // Add it to the stream list
                        }

                        // Link the subject and type
                        if (!subject.Types.Contains(type))
                        {
                            subject.Types.Add(type);
                            type.Subject = subject;
                        }

                        // Link the stream and type.
                        if (!type.Streams.Contains(stream))
                        {
                            type.Streams.Add(stream);
                            stream.Type = type;
                        }

                        // Link the stream and class together.
                        // Add it to our list of classes.
                        timetable.ClassList.Add(session);
                        stream.Classes.Add(session);
                        session.Stream = stream;
                    }
                }
                return timetable;
            }
            catch (Exception)
            {
                return null;
            }
        }

        protected Timetable OldParse()
        {
            /*
             * University of Canterbury Timetable Downloader
             * Allocate+ Student Module
             * Matthew Knox
             * 12/07/13
             * 
             * Heavy inspiration was taken from Jack Valmadre's University of Queensland parser for this code.
             * 
             * Allocate+ generates crappily formatted, standards NON complient HTML and doesnt provide an easy to access
             * API for non staff members. The following code will probably break the next time something is changed but
             * attempts to extract useful info from the inconsistantly named, sized and shaped tables that this system outputs.
             * 
             * LSV Data Arrangement:
             * 0. CourseCode
             * 1. Stream
             * 2. Type/Group
             * 3. Campus
             * 4. Day
             * 5. StartTime
             * 6. Building+Room
             * 7. Staff
             * 8. Length(min)
             * 9. Dates eg. 8/7-12/8, 2/9-7/10
             * 10. Description
            */

            #region TimetableImport
            var cinfo = new List<string>();

            /* Online Retreival */
            var cookieContainer = new CookieContainer();
            const string userAgent = "TimetableDL/1.0 (Based on http://jack.valmadre.net/timetable/; Keywords: webkit,gecko,khtml,trident,safari,firefox,chrome)";

            var webRequest = (HttpWebRequest)WebRequest.Create("https://mytimetable.canterbury.ac.nz/aplus/apstudent?fun=login");
            webRequest.Method = "POST";
            using (var s = webRequest.GetRequestStream())
            {
                var postData = "student_code=" + Username + "&password=" + Password;
                s.Write(System.Text.Encoding.ASCII.GetBytes(postData), 0, postData.Length);
            }
            webRequest.ContentType = "application/x-www-form-urlencoded";
            webRequest.UserAgent = userAgent;
            webRequest.CookieContainer = cookieContainer;
            webRequest.AllowAutoRedirect = true;

            var ss = "";
            using (var s = (HttpWebResponse)webRequest.GetResponse())
            {
                cookieContainer = webRequest.CookieContainer;
                System.IO.Stream stream;
                if ((stream = s.GetResponseStream()) == null)
                {
                    return null;
                }
                var streamReader = new StreamReader(stream);
                while (!streamReader.EndOfStream)
                {
                    var temp = streamReader.ReadLine();
                    if (temp == null || !temp.Contains("&ss=")) continue;
                    ss = temp.Substring(temp.IndexOf("&ss=", StringComparison.Ordinal) + 4);
                    break;
                }
                streamReader.Close();
                if (string.IsNullOrEmpty(ss))
                {
                    return null;    // HAVE YOU ENTERED YOUR USERNAME/PASSWORD (CORRECTLY)??? I BET NOT!
                }
                ss = ss.Replace("\n", "");
                ss = ss.Replace("\r", "");
            }

            webRequest =
                (HttpWebRequest)
                WebRequest.Create(
                    "https://mytimetable.canterbury.ac.nz/aplus/apstudent?fun=ListEnrolment&ss=" + ss);
            webRequest.CookieContainer = cookieContainer;
            webRequest.UserAgent = userAgent;
            webRequest.AllowAutoRedirect = true;

            using (var webResponse = (HttpWebResponse)webRequest.GetResponse())
            {
                System.IO.Stream s;
                if ((s = webResponse.GetResponseStream()) == null)
                {
                    return null;
                }
                var streamReader = new StreamReader(s);
                var regex = new Regex("apstudent\\?ss=[a-f0-9]+&fun=show_a(uto_single|ctivity_group)&spos=[0-9]&gpos=[0-9]&disp=[^\"]*");
                var links = new List<string>();
                foreach (var res in regex.Matches(streamReader.ReadToEnd()))
                {
                    links.Add("https://mytimetable.canterbury.ac.nz/aplus/" + res);
                }
                s.Close();

                foreach (var reqln in links)
                {
                    var webRequestTimetable = (HttpWebRequest) WebRequest.Create(reqln);
                    webRequestTimetable.UserAgent = userAgent;
                    webRequestTimetable.AllowAutoRedirect = true;
                    webRequestTimetable.CookieContainer = cookieContainer;
                    var webResponseTimetable = (HttpWebResponse)webRequestTimetable.GetResponse();
                    using (var s1 = webResponseTimetable.GetResponseStream())
                    {
                        if (s1 == null)
                        {
                            continue;
                        }
                        var streamReader1 = new StreamReader(s1);

                        var input = streamReader1.ReadToEnd();
                        var subjectTitleRegex = new Regex("[A-Z]{4}[0-9]{3}[-][0-9]{2}[a-zA-Z][0-9]*");
                        var subjectTitle = subjectTitleRegex.Matches(input)[0];

                        var classExtractorRegex =
                            new Regex(
                                      "<TD nowrap>(\r|\n|\r\n)([0-9]+(\r|\n|\r\n)</TD>((\r|\n|\r\n)<TD nowrap>(\r|\n|\r\n).*?(\r|\n|\r\n)</TD>|)(\r|\n|\r\n)<TD ALIGN=\"CENTER\" nowrap>(\r|\n|\r\n).*?(\r|\n|\r\n)</TD>(\r|\n|\r\n)<TD nowrap>(\r|\n|\r\n)[MTWFS][a-z]{2}(\r|\n|\r\n)</TD>(\r|\n|\r\n)<TD nowrap>(\r|\n|\r\n)[0-9]{2}[:][0-9]{2}(\r|\n|\r\n)</TD>(\r|\n|\r\n)<TD nowrap>(\r|\n|\r\n).*?(\r|\n|\r\n)</TD>(\r|\n|\r\n)<TD nowrap>(\r|\n|\r\n).*?(\r|\n|\r\n)</TD>(\r|\n|\r\n)<TD ALIGN=\"CENTER\" nowrap>(\r|\n|\r\n)[0-9]+(\r|\n|\r\n)</TD>(\r|\n|\r\n)<TD ALIGN=\"left\"( nowrap|)>(\r|\n|\r\n).*?(\r|\n|\r\n)</TD>(\r|\n|\r\n)<TD ALIGN=\"left\">(\r|\n|\r\n).*?(\r|\n|\r\n)(\r|\n|\r\n)</TD>)");
                        foreach (var regmatch in classExtractorRegex.Matches(input))
                        {
                            var regmatcha = Regex.Replace(regmatch.ToString(), "(\r|\n|\r\n)</TD>(\r|\n|\r\n)<TD.*?>(\r|\n|\r\n)", "|");
                            regmatcha = Regex.Replace(regmatcha, "(<TD nowrap>|(\r|\n|\r\n))", "");
                            var det = regmatcha.Split('|');
                            if (det.Length == 9) // Make sure that the correct number of values exists. If not add another.
                            {
                                var day = det[2]; // add the day values to reduce the occurances of a bug
                                switch (Regex.Matches(input, "(?<=(<B>))(Lecture|Computer|Lab|Tutorial|Test|Exam)(?=(</B>| ))")[0].ToString())
                                {
                                    case "Lecture": regmatcha = regmatcha.Insert(regmatcha.IndexOf("|", StringComparison.Ordinal), "|Lect " + day); break;
                                    case "Computer": regmatcha = regmatcha.Insert(regmatcha.IndexOf("|", StringComparison.Ordinal), "|Com"); break;
                                    case "Lab": regmatcha = regmatcha.Insert(regmatcha.IndexOf("|", StringComparison.Ordinal), "|Lab"); break;
                                    case "Tutorial": regmatcha = regmatcha.Insert(regmatcha.IndexOf("|", StringComparison.Ordinal), "|Tut"); break;
                                    case "Test": regmatcha = regmatcha.Insert(regmatcha.IndexOf("|", StringComparison.Ordinal), "|Test"); break;
                                    case "Exam": regmatcha = regmatcha.Insert(regmatcha.IndexOf("|", StringComparison.Ordinal), "|Exam"); break;
                                    default: regmatcha = regmatcha.Insert(regmatcha.IndexOf("|", StringComparison.Ordinal), "|Unkn " + day); break;
                                }
                            }
                            regmatcha = subjectTitle + "|" + regmatcha;
                            regmatcha = regmatcha.TrimEnd(new[] { '|' });
                            regmatcha = regmatcha.Substring(0, regmatcha.Length - 5);
                            cinfo.Add(regmatcha);
                        }
                        streamReader1.Close();
                    }
                }
            }

            /* Local Copy Read In * /
            for (var i = 0; i < 47; i++)
            {
                var reader = new StreamReader(@"C:\Users\Matthew\Desktop\folder\" + i.ToString(CultureInfo.InvariantCulture) + ".html");
                var input = reader.ReadToEnd() + "\n";
                reader.Close();

                var subjectTitleRegex = new Regex("[A-Z]{4}[0-9]{3}[-][0-9]{2}[a-zA-Z][0-9]");
                var subjectTitle = subjectTitleRegex.Matches(input)[0];

                var classExtractorRegex =
                    new Regex(
                              "<TD nowrap>(\r|\n|\r\n)([0-9]+(\r|\n|\r\n)</TD>((\r|\n|\r\n)<TD nowrap>(\r|\n|\r\n).*?(\r|\n|\r\n)</TD>|)(\r|\n|\r\n)<TD ALIGN=\"CENTER\" nowrap>(\r|\n|\r\n).*?(\r|\n|\r\n)</TD>(\r|\n|\r\n)<TD nowrap>(\r|\n|\r\n)[MTWFS][a-z]{2}(\r|\n|\r\n)</TD>(\r|\n|\r\n)<TD nowrap>(\r|\n|\r\n)[0-9]{2}[:][0-9]{2}(\r|\n|\r\n)</TD>(\r|\n|\r\n)<TD nowrap>(\r|\n|\r\n).*?(\r|\n|\r\n)</TD>(\r|\n|\r\n)<TD nowrap>(\r|\n|\r\n).*?(\r|\n|\r\n)</TD>(\r|\n|\r\n)<TD ALIGN=\"CENTER\" nowrap>(\r|\n|\r\n)[0-9]+(\r|\n|\r\n)</TD>(\r|\n|\r\n)<TD ALIGN=\"left\"( nowrap|)>(\r|\n|\r\n).*?(\r|\n|\r\n)</TD>(\r|\n|\r\n)<TD ALIGN=\"left\">(\r|\n|\r\n).*?(\r|\n|\r\n)(\r|\n|\r\n)</TD>)");
                foreach (var regmatch in classExtractorRegex.Matches(input))
                {
                    var regmatcha = Regex.Replace(regmatch.ToString(), "(\r|\n|\r\n)</TD>(\r|\n|\r\n)<TD.*?>(\r|\n|\r\n)", "|");
                    regmatcha = Regex.Replace(regmatcha, "(<TD nowrap>|(\r|\n|\r\n))", "");
                    var det = regmatcha.Split('|');
                    if (det.Length == 9) // Make sure that the correct number of values exists. If not add another.
                    {
                        var day = det[2]; // add the day values to reduce the occurances of a bug
                        switch (Regex.Matches(input, "<B>(Lecture|Computer|Lab|Tutorial|Test|Exam)</B>")[0].ToString())
                        {
                            case "<B>Lecture</B>": regmatcha = regmatcha.Insert(regmatcha.IndexOf("|", StringComparison.Ordinal), "|Lect " + day); break;
                            case "<B>Computer</B>": regmatcha = regmatcha.Insert(regmatcha.IndexOf("|", StringComparison.Ordinal), "|Com"); break;
                            case "<B>Lab</B>": regmatcha = regmatcha.Insert(regmatcha.IndexOf("|", StringComparison.Ordinal), "|Lab"); break;
                            case "<B>Tutorial</B>": regmatcha = regmatcha.Insert(regmatcha.IndexOf("|", StringComparison.Ordinal), "|Tut"); break;
                            case "<B>Test</B>": regmatcha = regmatcha.Insert(regmatcha.IndexOf("|", StringComparison.Ordinal), "|Test"); break;
                            case "<B>Exam</B>": regmatcha = regmatcha.Insert(regmatcha.IndexOf("|", StringComparison.Ordinal), "|Exam"); break;
                            default: regmatcha = regmatcha.Insert(regmatcha.IndexOf("|", StringComparison.Ordinal), "|Unkn " + day); break;
                        }
                    }
                    regmatcha = subjectTitle + "|" + regmatcha;
                    regmatcha = regmatcha.TrimEnd(new[] { '|' });
                    regmatcha = regmatcha.Substring(0, regmatcha.Length - 5);
                    cinfo.Add(regmatcha);
                }
            }
            //*/
            #endregion
            #region TimetableParse
            var timetable = new Timetable();

            foreach (var match in cinfo)
            {
                // Separate the values
                var sessionInfo = match.Split('|');
                
                // Set the current day
                int currentDay;
                switch (sessionInfo[4])
                {
                    case "Sun":
                        currentDay = 0;
                        break;
                    case "Mon":
                        currentDay = 1;
                        break;
                    case "Tue":
                        currentDay = 2;
                        break;
                    case "Wed":
                        currentDay = 3;
                        break;
                    case "Thu":
                        currentDay = 4;
                        break;
                    case "Fri":
                        currentDay = 5;
                        break;
                    case "Sat":
                        currentDay = 6;
                        break;
                    default:
                        continue;
                }

                // Build a session object.
                var session = new Session {Day = currentDay};

                // Set the subject
                Subject subject;
                if (timetable.SubjectList.Exists(element => element.Name == sessionInfo[0]))
                {
                    subject = timetable.SubjectList.Find(element => element.Name == sessionInfo[0]);
                }
                else
                {
                    subject = new Subject(sessionInfo[0]);
                    timetable.SubjectList.Add(subject);
                }

                // Set the stream number
                var streamNumber = 0;
                if (Regex.Match(sessionInfo[1], "[0-9]+").Success)
                {
                    streamNumber = Convert.ToInt32(sessionInfo[1]);
                }

                // Set the session type
                Type type;
                if(subject.Types.Exists(types => types.Code == sessionInfo[2]))
                {
                    if(sessionInfo[2] == "Unknown")
                    {
                        var i = 1;
                        while (subject.Types.Exists(types => types.Code == sessionInfo[2] + i.ToString(CultureInfo.InvariantCulture)))
                        {
                            i++;
                        }
                        type = new Type(sessionInfo[2]+i.ToString(CultureInfo.InvariantCulture), "Uno", subject)
                        {
                            Required = false // Allocate+ doesnt provide information as to if this class is required
                        };
                        timetable.TypeList.Add(type);
                    }
                    else
                    {
                        type = subject.Types.Find(types => types.Code == sessionInfo[2]);
                    }
                }
                else // The session type doesn't exist, create it.
                {
                    var sessionCodeD = sessionInfo[2].Trim().Substring(0, 3);
                    type = new Type(sessionInfo[2], sessionInfo[2], subject);
                    switch (sessionCodeD.ToLower())
                    {
                        case "com":
                        case "wor":
                        case "lab":
                        case "lec":
                        case "tut":
                            type.Required = true; break;
                        /*case "exa": 
                        case "tes":
                        case "unk":*/
                        default:
                            type.Required = false; break;
                    }
                    timetable.TypeList.Add(type);
                }               

                // Set the session
                Stream stream;
                if (type.Streams.Exists(x => x.Number == streamNumber))
                {
                    stream = type.Streams.Find(x => x.Number == streamNumber);
                }
                else
                {
                    stream = new Stream(streamNumber);
                    timetable.StreamList.Add(stream); // Add it to the stream list
                }

                // Link the subject and type
                if (!subject.Types.Contains(type))
                {
                    subject.Types.Add(type);
                    type.Subject = subject;
                }

                // Link the stream and type.
                if (!type.Streams.Contains(stream))
                {
                    type.Streams.Add(stream);
                    stream.Type = type;
                }

                // Link the stream and class together.
                // Add it to our list of classes.
                timetable.ClassList.Add(session);
                stream.Classes.Add(session);
                session.Stream = stream;

                // Start Time
                var startTime = sessionInfo[5];
                var colonIndex = startTime.IndexOf(":", StringComparison.Ordinal);
                session.StartHour = Convert.ToInt32(startTime.Substring(0, colonIndex));
                session.StartMinute = Convert.ToInt32(startTime.Substring(colonIndex + 1, 2));

                // End Time
                session.EndHour = ((int)((Convert.ToDouble(sessionInfo[8].Trim())) / 60.0) + session.StartHour);
                session.EndHour = (session.EndHour >= 24)?session.EndHour - 24:session.EndHour;
                session.EndMinute = (int) (Convert.ToInt32(sessionInfo[8].Trim()) - ((int)((Convert.ToDouble(sessionInfo[8].Trim())) / 60.0) * 60.0));
                
                // Insert Building Location
                var buildingRoom = sessionInfo[6].Trim().IndexOf(' ');
                session.Location = (buildingRoom != -1) ? sessionInfo[6].Trim().Substring(0, buildingRoom).Trim() + " - " + sessionInfo[6].Trim().Substring(buildingRoom + 1).Trim() : sessionInfo[6];
            }
            #endregion

            return timetable;
        }

        public bool Export(Timetable timetable, Action<string, bool> modifyList)
        {
            var response = true;
            try
            {
                var canterburyLoader = new CanterburyLoader(Username, Password);
                foreach (var stream in timetable.StreamList.Where(stream => stream.Selected))
                {
                    modifyList("Setting " + stream.Type.Subject + ": " + stream.Type.Code + " to stream " + stream.Number, false);
                    var inrep = canterburyLoader.SetCourse(stream.Type.Subject, stream.Type.Code, stream.Number);

                    if (inrep != null)
                    {
                        response = response && inrep.success;
                    }

                    if (inrep == null)
                    {
                        modifyList(
                            "UNKO: Setting " + stream.Type.Subject + ": " + stream.Type.Code + " to stream " +
                            stream.Number, true);
                        modifyList("ERRO: No data was returned so the status is unknown", false);
                    }
                    else if (!inrep.success)
                    {
                        modifyList(
                            "FAIL: Setting " + stream.Type.Subject + ": " + stream.Type.Code + " to stream " +
                            stream.Number, true);
                        modifyList("ERRO: " + inrep.msg, false);
                    }
                    else
                    {
                        modifyList(
                            "DONE: Setting " + stream.Type.Subject + ": " + stream.Type.Code + " to stream " +
                            stream.Number, true);
                    }
                }
            }
            catch (Exception e)
            {
                modifyList("A critical error occurred: " + e.Message, false);
                response = false;
            }

            return response;
        }
    }
}
