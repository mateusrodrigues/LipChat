using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LipChat.Library
{
    public static class Constants
    {
        public static string GetAPIAddress(string environment = "Development")
        {
            switch (environment)
            {
                case "Development":
                    return "https://localhost:44332";
                case "Production":
                    return "https://lipchat-api.azurewebsites.net";
                default:
                    return "https://localhost:44332";
            }
        }
    }
}
