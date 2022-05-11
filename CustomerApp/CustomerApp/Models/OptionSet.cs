using CustomerApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace CustomerApp.Models
{
    public class OptionSet : BaseViewModel
    {
        public Guid Id { get; set; }
        public string Val { get; set; }
        public string Label { get; set; }

        public string _name;
        public string Name { get => _name; set { _name = value; OnPropertyChanged(nameof(Name)); } }

        private bool _selected;
        public bool Selected { get => _selected; set { _selected = value; OnPropertyChanged(nameof(Selected)); } }
        public bool IsMultiple { get; set; }
        public string Detail { get; set; }

        public OptionSet()
        {

        }

        public OptionSet(string val, string label, bool selected = false)
        {
            Val = val;
            Label = label;
            Selected = selected;
        }

        public OptionSet(Guid id, string name, bool selected = false)
        {
            Id = id;
            Name = name;
            Selected = selected;
        }
    }
}
