using System.Collections.Generic;
namespace CityU.Contants
{
    public static class Menus
    {
        public static readonly IDictionary<string, string> controllers = new Dictionary<string, string>()
        {
            { "dashboard", "Dashboard"},
            { "home", "Home" },
            { "Index", "Index" },
            { "users", "Users" },
            { "login", "login"},
        };
        public static readonly IDictionary<string, string> actions = new Dictionary<string, string>()
        {
            { "index", "index" },
            { "users", "Users" },
            { "generate", "generate" }          
        };
    }
}