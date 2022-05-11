using System;
using System.Collections.Generic;
using System.Text;

namespace CustomerApp.Models
{
    public class GraphThumbnailsUrlModel
    {
        public string id { get; set; }
        public Large large { get; set; }
        public Medium medium { get; set; }
        public Small small { get; set; }
    }
    public class Large
    {
        public int height { get; set; }
        public string url { get; set; }
        public int width { get; set; }
    }
    public class Medium
    {
        public int height { get; set; }
        public string url { get; set; }
        public int width { get; set; }
    }
    public class Small
    {
        public int height { get; set; }
        public string url { get; set; }
        public int width { get; set; }
    }
}
