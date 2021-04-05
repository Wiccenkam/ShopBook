using System;
using System.Collections.Generic;
using System.Text;

namespace ShopBook.Contractors
{
    public abstract class Field
    {
        public string Label { get; }
        public string Name { get; }
        public string Values { get; set; }
        protected Field(string label, string name, string values)
        {
            Label = label;
            Name = name;
            Values = values;
        }
    }
    public class HiddenField : Field
    {
        public HiddenField(string label, string name, string values) : base(label, name, values)
        {

        }
    }
    public class SelectionField : Field
    {
        public IReadOnlyDictionary<string, string> Items { get; }
        public SelectionField(string label, string name, string values,IReadOnlyDictionary<string,string> items) : base(label, name, values)
        {
            Items = items;
        }
    }
}
