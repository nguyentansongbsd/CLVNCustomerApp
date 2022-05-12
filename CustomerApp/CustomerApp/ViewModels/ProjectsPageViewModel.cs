using System;
using CustomerApp.Models;
using Xamarin.Forms;

namespace CustomerApp.ViewModels
{
    public class ProjectsPageViewModel : ListViewBaseViewModel2<ProjectList>
    {
        public string Keyword { get; set; }
        public ProjectsPageViewModel()
        {
            PreLoadData = new Command(() =>
            {
                EntityName = "bsd_projects";
                FetchXml = $@"<fetch version='1.0' count='15' page='{Page}' output-format='xml-platform' mapping='logical' distinct='false'>
                                <entity name='bsd_project'>
                                    <attribute name='bsd_projectid'/>
                                    <attribute name='bsd_projectcode'/>
                                    <attribute name='bsd_name'/>
                                    <attribute name='createdon' />
                                    <attribute name ='statuscode' />
                                    <attribute name ='bsd_address' />
                                    <attribute name ='bsd_projecttype' />
                                    <order attribute='bsd_name' descending='false' />
                                    <filter type='or'>
                                      <condition attribute='bsd_projectcode' operator='like' value='%25{Keyword}%25' />
                                      <condition attribute='bsd_name' operator='like' value='%25{Keyword}%25' />
                                    </filter>
                                  </entity>
                            </fetch>";
            });
        }
    }
}
