using SitefinityWebApp.Mvc.Models;
using System;
using System.Web;
using Telerik.Microsoft.Practices.Unity;
using Telerik.Sitefinity.Abstractions;
using Telerik.Sitefinity.Data;
using Telerik.Sitefinity.DynamicModules.Events;
using Telerik.Sitefinity.Frontend;
using Telerik.Sitefinity.Frontend.Mvc.Infrastructure.Routing;
using Telerik.Sitefinity.Frontend.News.Mvc.Models;
using Telerik.Sitefinity.Mvc;
using Telerik.Sitefinity.Services;
using Telerik.Sitefinity.Model;
using Telerik.Sitefinity.Configuration;
using SitefinityWebApp.Configuration;

namespace SitefinityWebApp
{
    public class Global : System.Web.HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            Bootstrapper.Initialized += Bootstrapper_Initialized;
            Bootstrapper.Bootstrapped += Bootstrapper_Bootstrapped;
        }

        private void Bootstrapper_Initialized(object sender, ExecutedEventArgs e)
        {
            if (e.CommandName == "Bootstrapped")
            {
                FrontendModule.Current.DependencyResolver.Rebind<INewsModel>().To<CategoryFilterNewsModel>();
                Config.RegisterSection<IntegrationConfig>();
            }
        }

        protected void Bootstrapper_Bootstrapped(object sender, EventArgs e)
        {
            FeatherActionInvokerCustom.Register();
            EventHub.Subscribe<IDynamicContentCreatingEvent>(eventInfo => DynamicContentCreatingEventHandler(eventInfo));
        }

        private void DynamicContentCreatingEventHandler(IDynamicContentCreatingEvent eventInfo)
        {
            var userId = eventInfo.UserId;
            var dynamicContentItem = eventInfo.Item;
            var officeModel = new OfficeModel();
            if (dynamicContentItem.GetType().Equals(officeModel.OfficeType))
            {
                dynamicContentItem.SetString("Info", OfficeModel.LOREM_IPSUM);
            }
        }

        protected void Application_End(object sender, EventArgs e)
        {
            EventHub.Unsubscribe<IDynamicContentCreatingEvent>(DynamicContentCreatingEventHandler);
        }
    }
}