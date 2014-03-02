using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Xml.Serialization;

namespace UniTimetable
{
    public class Subject : IComparable<Subject>
    {
        string Name_ = "";
        Color Color_ = Color.White;

        List<Type> Types_ = new List<Type>();

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
            this.Name_ = other.Name_;
            this.Color_ = other.Color_;
            this.Types_ = new List<Type>(other.Types_);
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
            get
            {
                return Name_;
            }
            set
            {
                Name_ = value;
            }
        }

        [XmlIgnore]
        public Color Color
        {
            get
            {
                return Color_;
            }
            set
            {
                Color_ = value;
            }
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
            get
            {
                return Types_;
            }
            set
            {
                Types_ = value;
            }
        }

        #endregion

        #region IComparable<Subject> Members

        public int CompareTo(Subject other)
        {
            return this.Name_.CompareTo(other.Name_);
        }

        #endregion

        #region Base methods

        public override string ToString()
        {
            return Name_;
        }

        #endregion
    }
}
