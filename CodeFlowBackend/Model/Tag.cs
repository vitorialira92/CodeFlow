using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFlowBackend.Model
{
    public class Tag
    {
        public long Id { get; private set; }
        public string Name { get; private set; }
        public Color Color {  get; private set; }

        public Tag(string name, Color color)
        {
            Name = name;
            Color = color;
        }

        public Tag(long id, string name, Color color)
        {
            Id = id;
            Name = name;
            Color = color;
        }
    }
}
