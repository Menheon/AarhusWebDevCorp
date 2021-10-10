using AarhusWebDevCoop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Core.PropertyEditors;
using Umbraco.Web.Models;
using Umbraco.Web.PublishedModels;

namespace AarhusWebDevCoop.Controllers
{
    public class ProjectsOverviewController : Umbraco.Web.Mvc.RenderMvcController
    {
        public override ActionResult Index(ContentModel model)
        {
            // Initialize the view model using the old model.
            var vm = new ProjectsOverviewViewModel(model.Content);

            // Different project status.
            var projectStatusDataType = Services.DataTypeService.GetDataType("Project Status Radio Button");
            var projectStatusValues = (ValueListConfiguration)projectStatusDataType.Configuration;
            List<string> projectStatusList = projectStatusValues
                .Items.ConvertAll(status => status.Value.ToString());
            vm.ProjectStatusList = projectStatusList;

            // Default status.
            var projectStatus = "Completed";

            // All projects.
            IEnumerable<ProjectItem> projects =
                model.Content.Parent.Children
                .OfType<ProjectsOverview>()
                .First()
                .Children
                .Cast<ProjectItem>();

            // Filter the projects based on the route data
            if (RouteData.Values["status"] != null)
            {
                projectStatus = RouteData.Values["status"].ToString();
                if (!projectStatusList.Contains(projectStatus))
                {
                    throw new HttpException(404, "Project status not found");
                }
            }
            vm.ProjectStatus = projectStatus;

            // Filtered projects
            projects = projects.Where(p => p.ProjectStatus == projectStatus);
            vm.Projects = projects;

            return View("ProjectsOverview", vm);
        }
    }
}