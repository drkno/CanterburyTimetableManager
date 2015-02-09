#region

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Xml.Serialization;

#endregion

namespace UniTimetable.Model.Timetable
{
    public class Subject : IComparable<Subject>
    {
        private Color Color_ = Color.White;
        private string Name_ = "";
        private List<Type> Types_ = new List<Type>();

        #region IComparable<Subject> Members

        public int CompareTo(Subject other)
        {
            return Name_.CompareTo(other.Name_);
        }

        #endregion

        #region Base methods

        public override string ToString()
        {
            return Name_;
        }

        #endregion

        #region Constructors

        public Subject()
        {
        }

        public Subject(string name)
        {
            Name_ = name;
        }

        public Subject(Subject other)
        {
            Name_ = other.Name_;
            Color_ = other.Color_;
            Types_ = new List<Type>(other.Types_);
        }

        public Subject(string name, Color color)
        {
            Name_ = name;
            Color_ = color;
        }

        public Subject Clone()
        {
            return new Subject(this);
        }

        #endregion

        #region Accessors

        [XmlAttribute("name")]
        public string Name
        {
            get { return Name_; }
            set { Name_ = value; }
        }

        [XmlIgnore]
        public Color Color
        {
            get { return Color_; }
            set { Color_ = value; }
        }

        [XmlAttribute("color")]
        public int ColorInt
        {
            get { return ColorTranslator.ToWin32(Color_); }
            set { Color_ = ColorTranslator.FromWin32(value); }
        }

        [XmlArray("types"), XmlArrayItem("type")]
        public List<Type> Types
        {
            get { return Types_; }
            set { Types_ = value; }
        }

        #endregion
    }
}