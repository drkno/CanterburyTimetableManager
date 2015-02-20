using System.IO;
using System.Reflection;
namespace UniTimetable.Resources
{
    public class Resources
    {
        public static Stream GetEmbeddedResourceStream(string name)
        {
            var thisAssem = Assembly.GetExecutingAssembly();
            return thisAssem.GetManifestResourceStream(name);
        }
    }
}
