using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CodeFlowUI.Pages
{
    public partial class ProjectSettings : Form
    {
        public ProjectSettings(long projectId)
        {
            InitializeComponent(projectId);
        }
    }
}
