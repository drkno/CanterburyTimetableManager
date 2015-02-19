using System;
using System.Collections.Generic;
using System.Linq;
using UniTimetable.Model.Timetable;
using Type = UniTimetable.Model.Timetable.Type;

namespace UniTimetable.Model.Import.CanterburyData
{
    internal class CanterburyImporter : Importer
    {
        public CanterburyImporter()
        {
            RequiresPassword = true;
        }

        /// <summary>
        ///     Takes the user-selected files and builds the stream data for a timetable.
        /// </summary>
        /// <returns>The stream data parsed from the files if it succeeded, null if it failed.</returns>
        protected override Timetable.Timetable Parse()
        {
            try
            {
                var cantaLoader = new CanterburyLoader(Username, Password);
                cantaLoader.GetData();
                var timetable = new Timetable.Timetable();
                foreach (var data in cantaLoader.CanterburyDatas)
                {
                    var streamMap = new Dictionary<string, int>();
                    var streamNo = 1;
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
                                    type.Required = false;
                                    break;
                                default:
                                    type.Required = true;
                                    break;
                            }
                            timetable.TypeList.Add(type);
                        }

                        // Set the session
                        int streamNumber;
                        if (!int.TryParse(subs.Value.activity_code, out streamNumber))
                        {
                            if (streamMap.ContainsKey(subs.Value.activity_code))
                            {
                                streamNumber = streamMap[subs.Value.activity_code];
                            }
                            else
                            {
                                streamMap[subs.Value.activity_code] = streamNo;
                                streamNumber = streamNo++;
                            }
                        }
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
                    }
                }
                return timetable;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public bool Export(Timetable.Timetable timetable, Action<string, bool> modifyList)
        {
            var response = true;
            try
            {
                var canterburyLoader = new CanterburyLoader(Username, Password);
                foreach (var stream in timetable.StreamList.Where(stream => stream.Selected))
                {
                    modifyList(
                        "Setting " + stream.Type.Subject + ": " + stream.Type.Code + " to stream " + stream.Number,
                        false);
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