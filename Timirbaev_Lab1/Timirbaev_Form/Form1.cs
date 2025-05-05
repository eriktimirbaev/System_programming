using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Timirbaev_Form
{
    public partial class Form1 : Form
    {
        private TimirbaevSessions Sessions = new TimirbaevSessions();

        public Form1()
        {
            InitializeComponent();
            listView1.View = View.SmallIcon;
            UpdateListView();

            // Bind Sessions collection to ListView
            Sessions.Sessions.CollectionChanged += (s, e) =>
            {
                this.Invoke((MethodInvoker)delegate { UpdateListView(); });
            };
        }

        private void UpdateListView()
        {
            listView1.Items.Clear();
            foreach (var session in Sessions.Sessions)
            {
                listView1.Items.Add(session);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Sessions.SessionsCount = (int)numericUpDown1.Value;
            Sessions.StartSession();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Sessions.StopSession();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Sessions.CloseSessions();
        }
    }
}