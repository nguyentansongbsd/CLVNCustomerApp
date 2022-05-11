using System;
using System.Collections.Generic;
using System.Linq;
using CustomerApp.Models;

namespace CustomerApp.Datas
{
    public class ProjectStatusData
    {
        public static StatusCodeModel GetProjectStatusById(string id)
        {
            return ProjectStatus().SingleOrDefault(x => x.Id == id);
        }

        public static List<StatusCodeModel> ProjectStatus()
        {
            return new List<StatusCodeModel>()
            {
                new StatusCodeModel("1","Active","#03ACF5"),
                new StatusCodeModel("861450002","Publish","#06CF79"),
                new StatusCodeModel("861450001","Unpublish","#333333"),
                new StatusCodeModel("2","Inactive","#04A388"),
            };
        }
    }
}
