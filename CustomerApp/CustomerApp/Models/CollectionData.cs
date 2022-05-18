﻿using System;
using System.Collections.Generic;
using System.Text;

namespace CustomerApp.Models
{
    public class CollectionData
    {
        public Guid Id { get; set; }
        public string MediaSourceId { get; set; }
        public string ImageSource { get; set; }
        public string Name { get; set; }
        public string Thumnail { get; set; }
        public string ImageSourceBase64 { get; set; }
        public SharePointType SharePointType { get; set; }
        public int Index { get; set; }
        public CollectionData()
        { }
    }
    public enum SharePointType
    {
        Video,
        Image
    }
}
