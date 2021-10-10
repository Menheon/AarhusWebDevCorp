using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Web.Models;
using Umbraco.Web.PublishedModels;

namespace AarhusWebDevCoop.Models
{
    public class ProjectsOverviewViewModel : ContentModel
    {
        public IEnumerable<ProjectItem> Projects { get; set; }
        public IEnumerable<string> ProjectStatusList { get; set; }
        public string ProjectStatus { get; set; }
        public ProjectsOverviewViewModel(IPublishedContent content) : base(content)
        {

        }
    }
}