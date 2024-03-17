using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            
        }

        private void map_Load(object sender, EventArgs e)
        {
            map.MapProvider = CzechTuristMapProvider.Instance;
            map.CacheLocation = Application.StartupPath + @"\maps\OSMCache";
            GMaps.Instance.Mode = AccessMode.ServerAndCache;
            map.CanDragMap = true;
            map.DragButton = MouseButtons.Left;
            map.MouseWheelZoomEnabled = true;
            map.MouseWheelZoomType = MouseWheelZoomType.MousePositionWithoutCenter;
            map.MinZoom = 9;
            map.MaxZoom = 16;
            map.Zoom = 12;
            map.Position = new PointLatLng(51.672, 39.1843);
            Createmarcker();
        }
        private void Createmarcker()
        {
            GMapOverlay markers_overlay = new GMapOverlay("MARKERS");
            GMapMarker gMapMarker = new GMarkerGoogle(new PointLatLng(51.672, 39.1843), GMarkerGoogleType.blue_dot);
            markers_overlay.Markers.Add(gMapMarker);
            map.Overlays.Add(markers_overlay);  

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            panel_of_admin panel = new panel_of_admin();
            this.Hide();
            panel.ShowDialog();
            this.Show();
            Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form1 form1 = new Form1();
            this.Hide();
            form1.ShowDialog();
            this.Show();
            Close();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }
    }
}
