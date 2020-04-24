using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Perpustakaan.Helpers
{
    public class AppConstants
    {
        public class SessionKey
        {
            public const string USER_SESSION = "UserSession";
            public const string POPUP_MESSAGE = "PopupMessage";
        }

        public class ViewBagKey
        {
            public const string FORBIDEN = "Forbiden";
            public const string POPUP_MESSAGE = "PopupMessage";
        }
    }
}