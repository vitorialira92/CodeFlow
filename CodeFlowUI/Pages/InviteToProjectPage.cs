﻿using System;
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
    public partial class InviteToProjectPage : Form
    {
        public InviteToProjectPage(long projectId)
        {
            InitializeComponent(projectId);
        }
    }
}
