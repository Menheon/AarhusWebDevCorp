using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Umbraco.Core.Composing;
using Umbraco.Web;
using Umbraco.Web.Mvc;
using Umbraco.Web.PublishedModels;

namespace AarhusWebDevCoop.Util
{
    public class SetDefaultRenderMvcControllerComposer : IUserComposer
    {
        public void Compose(Composition composition)
        {
            RouteTable.Routes.MapUmbracoRoute(
                "FilterProjectRoute",
                "projects/Status/{status}",
                new
                {
                    controller = "ProjectsOverview",
                    action = "Index",
                    status = UrlParameter.Optional
                },
                new UmbracoVirtualNodeByIdRouteHandler(1072)
                );
        }
    }
}