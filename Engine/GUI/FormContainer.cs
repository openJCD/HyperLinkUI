﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HyperLinkUI.Engine.GUI
{
    public class FormContainer : Container
    {
        public List<FormItem> FormItems;
        public FormContainer(Container parent, string tag) : base(parent)
        {
            FormItems = new List<FormItem>();
            Tag = tag;
        }
        public void Add(FormItem item)
        {
            ChildWidgets.Add((Widget)item);
            FormItems.Add(item);
        }
        public string[] ReadAll()
        {
            string[] data = { };
            foreach (FormItem item in FormItems)
            {
                data.Append(item.ReadValueAsString());
            }
            return data;
        }
    }
}
