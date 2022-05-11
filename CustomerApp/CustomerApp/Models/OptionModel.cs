using System;
namespace CustomerApp.Models
{
    public class OptionModel
    {
        public string Id { get; set; }
        public string Value { get; set; }
        public string Background { get; set; }
        public OptionModel(string id,string value,string background = null)
        {
            Id = id;
            Value = value;
            Background = background;
        }
    }
}
