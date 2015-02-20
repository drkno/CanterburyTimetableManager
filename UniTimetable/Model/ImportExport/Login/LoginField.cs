namespace UniTimetable.Model.ImportExport.Login
{
    public class LoginField
    {
        public LoginFieldType Type { get; private set; }
        public string Name { get; private set; }
        public string[] Options { get; private set; }
        public string Value { get; set; }

        public LoginField(LoginFieldType type, string name)
        {
            Type = type;
            Name = name;
        }

        public LoginField(LoginFieldType type, string name, params string[] options)
        {
            Type = type;
            Name = name;
            Options = options;
        }
    }
}