using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFlowBackend.Model
{
    public class ChecklistItem
    {
        public long Id { get; private set; }
        public string Name { get; private set; }
        public bool IsChecked { get; set; }

        public ChecklistItem(long id, string name, bool isChecked)
        {
            Id = id;
            Name = name;
            IsChecked = isChecked;
        }
    }
}
