namespace WebApplication1
{
    #region Usings
    using System.Web;
    using System.Web.Mvc;
    #endregion

    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
