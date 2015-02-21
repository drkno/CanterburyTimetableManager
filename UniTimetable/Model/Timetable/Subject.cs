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
        private Color _color = Color.White;
        private string _name = "";
        private List<Type> _types = new List<Type>();

        #region IComparable<Subject> Members

        public int CompareTo(Subject other)
        {
            return String.Compare(_name, other._name, StringComparison.Ordinal);
        }

        #endregion

        #region Base methods

        public override string ToString()
        {
            return _name;
        }

        #endregion

        #region Constructors

        public Subject()
        {
        }

        public Subject(string name)
        {
            _name = name;
        }

        public Subject(Subject other)
        {
            _name = other._name;
            _color = other._color;
            _types = new List<Type>(other._types);
        }

        public Subject(string name, Color color)
        {
            _name = name;
            _color = color;
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
            get { return _name; }
            set { _name = value; }
        }

        [XmlIgnore]
        public Color Color
        {
            get { return _color; }
            set { _color = value; }
        }

        [XmlAttribute("color")]
        public int ColorInt
        {
            get { return ColorTranslator.ToWin32(_color); }
            set { _color = ColorTranslator.FromWin32(value); }
        }

        [XmlArray("types"), XmlArrayItem("type")]
        public List<Type> Types
        {
            get { return _types; }
            set { _types = value; }
        }

        #endregion
    }
}