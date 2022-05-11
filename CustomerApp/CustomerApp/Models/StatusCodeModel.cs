using System;
using System.Collections.Generic;
using System.Text;

namespace CustomerApp.Models
{
    public class StatusCodeModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Background { get; set; }

        public StatusCodeModel(string id, string name, string background)
        {
            Id = id;
            Name = name;
            Background = background;
        }
    }
}
