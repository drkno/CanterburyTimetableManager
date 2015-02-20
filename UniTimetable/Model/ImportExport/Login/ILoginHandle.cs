using System.Collections.Generic;

namespace UniTimetable.Model.ImportExport.Login
{
    public interface ILoginHandle
    {
        bool LoggedIn { get; }
        void Login();
        void Logout();
        string GetLoginPrompt();
        string GetPrivacyPromise();
        IEnumerable<LoginField> GetLoginFields();
        void SetLoginFields(IEnumerable<LoginField> loginFields);
    }
}
