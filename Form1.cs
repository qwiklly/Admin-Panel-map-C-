using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;
using GMap.NET.WindowsForms.ToolTips;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static GMap.NET.Entity.OpenStreetMapRouteEntity;



namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        readonly DataBase database = new DataBase();
        public Form1()
        {
            InitializeComponent();
            this.Load += Form1_Load;


        }
        public class CPoint
        {
            public double x { get; set; }
            public double y { get; set; }

 
            public CPoint(double _x, double _y)
            {
                x = _x;
                y = _y;
            }
        }


        // Список слоёв к каждому месту
        List<CPoint> ListWithPoinsOfUser = new List<CPoint>();
        GMapOverlay PositionsForUser = new GMapOverlay("ПозицияпоЛКМ");
        GMapOverlay PositionFromTextBoxes = new GMapOverlay("ПозицияДляТекстБокса");
        GMapOverlay PositionForCircle = new GMapOverlay("ПозицияДляКруга");

        private void map_Load(object sender, EventArgs e)
        {

            //Функционал для контекстного меню
            ToolStripMenuItem saveMenuItem = new ToolStripMenuItem("Сохранить карту");
            ToolStripMenuItem GoogleMenuItem = new ToolStripMenuItem("Установить Google-карту");
            ToolStripMenuItem CzechTuristMapProvider1 = new ToolStripMenuItem("Установить CzechTuristMapProvider-карту");
            //Добавление в массив функционала(методов)
            contextMenuStrip1.Items.AddRange(new[] { saveMenuItem, GoogleMenuItem, CzechTuristMapProvider1 });
            //Контекстного меню (при нажатии пкм)
            map.ContextMenuStrip = contextMenuStrip1;

            saveMenuItem.Click += saveMenuItem_Click;
            GoogleMenuItem.Click += GoogleMenuItem_Click;
            CzechTuristMapProvider1.Click += CzechTuristMapProvider_Click;
            map.MapProvider = CzechTuristMapProvider.Instance;
            map.CacheLocation = Application.StartupPath + @"\maps\OSMCache";
            GMaps.Instance.Mode = AccessMode.ServerAndCache;
            map.CanDragMap = true;
            map.DragButton = MouseButtons.Left;
            map.MouseWheelZoomEnabled = true;
            map.MouseWheelZoomType = MouseWheelZoomType.MousePositionWithoutCenter;
            map.MinZoom = 7;
            map.MaxZoom = 17;
            map.Zoom = 12;
            map.Position = new PointLatLng(51.672, 39.1843);

            Createmarcker();
           

        }
        
        private void Createmarcker()
        {
            GMapOverlay markers_overlay = new GMapOverlay("MARKERS");
            GMapMarker gMapMarker = new GMarkerGoogle(new PointLatLng(51.678303442, 39.3032455444336), GMarkerGoogleType.blue_dot);
            GMapMarker gMapMarker2 = new GMarkerGoogle(new PointLatLng(55.3389083559637, 37.8080749511719), GMarkerGoogleType.blue_dot);
            markers_overlay.Markers.Add(gMapMarker);
            markers_overlay.Markers.Add(gMapMarker2);
            map.Overlays.Add(markers_overlay);  

        }
        void saveMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                using (SaveFileDialog dialogforsavemap = new SaveFileDialog())
                {
                    // Формат картинки
                    dialogforsavemap.Filter = "PNG (*.png)|*.png";

                    // Название картинки
                    dialogforsavemap.FileName = "Текущее положение карты";

                    Image image = map.ToImage();

                    if (image != null)
                    {
                        using (image)
                        {
                            if (dialogforsavemap.ShowDialog() == DialogResult.OK)
                            {
                                string fileName = dialogforsavemap.FileName;
                                if (!fileName.EndsWith(".png", StringComparison.OrdinalIgnoreCase))
                                    fileName += ".png";

                                image.Save(fileName);
                                MessageBox.Show("Карта успешно сохранена в директории: " + Environment.NewLine + dialogforsavemap.FileName, "GMap.NET", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                            }
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show("Ошибка при сохранении карты: " + Environment.NewLine + exception.Message, "GMap.NET", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }
        //Смена поставщика карт
      

        void GoogleMenuItem_Click(object sender, EventArgs e)
        {
            map.MapProvider = GMapProviders.GoogleMap;
            map.MinZoom = 7;
            map.MaxZoom = 17;
            map.Zoom = 12;
            map.Position = new PointLatLng(51.672, 39.1843);
        }
        void CzechTuristMapProvider_Click(object sender, EventArgs e)
        {
            map.MapProvider = CzechTuristMapProvider.Instance;
            map.CacheLocation = Application.StartupPath + @"\maps\OSMCache";
            GMaps.Instance.Mode = AccessMode.ServerAndCache;
            map.CanDragMap = true;
            map.DragButton = MouseButtons.Left;
            map.MouseWheelZoomEnabled = true;
            map.MouseWheelZoomType = MouseWheelZoomType.MousePositionWithoutCenter;
            map.MinZoom = 7;
            map.MaxZoom = 17;
            map.Zoom = 12;
            map.Position = new PointLatLng(51.672, 39.1843);
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
            PositionsForUser.Clear();
            string querystring = "select coordinate_x, coordinate_y, coordinate_text from coordinates";
            SqlCommand command = new SqlCommand(querystring, database.GetConnection());

            database.openConnection();
            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                double x = Convert.ToDouble(reader["coordinate_x"]);
                double y = Convert.ToDouble(reader["coordinate_y"]);
                string text = Convert.ToString(reader["coordinate_text"]);


                GMarkerGoogle marker = new GMarkerGoogle(new PointLatLng(y, x), GMarkerGoogleType.red_pushpin);
                marker.ToolTip = new GMapRoundedToolTip(marker);
                marker.ToolTipText = text;
                map.Overlays.Add(PositionsForUser);
                PositionsForUser.Markers.Add(marker);
            }

            database.closeConnection();

        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (PositionsForUser.Markers.Count > 0)
            {
                // Удаляем последнюю добавленную точку
                var markerToRemove = PositionsForUser.Markers[PositionsForUser.Markers.Count - 1];
                PositionsForUser.Markers.Remove(markerToRemove);

                // Обновляем карту
                map.Overlays.Clear();
                map.Overlays.Add(PositionsForUser);

                // Обновляем данные
                UpdateCoordinatesInDatabase();
            }
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
            this.WindowState = FormWindowState.Maximized;
        }
        //Добавление маркера при двойном нажатии
        private void map_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                map.Overlays.Add(PositionsForUser);

                // Широта - latitude - lat - с севера на юг
                double x = map.FromLocalToLatLng(e.X, e.Y).Lng;
                // Долгота - longitude - lng - с запада на восток
                double y = map.FromLocalToLatLng(e.X, e.Y).Lat;

                textBox2.Text = x.ToString();
                textBox1.Text = y.ToString();
                string text = textBox5.Text; 

                // Добавляем метку на слой
                GMarkerGoogle MarkerWithMyPosition = new GMarkerGoogle(new PointLatLng(y, x), GMarkerGoogleType.blue_pushpin);
                MarkerWithMyPosition.ToolTip = new GMapRoundedToolTip(MarkerWithMyPosition);
                MarkerWithMyPosition.ToolTipText = text;
                PositionsForUser.Markers.Add(MarkerWithMyPosition);

                // Сохранение координат
                string querystring = $"insert into coordinates(coordinate_x, coordinate_y, coordinate_text) values('{x}', '{y}', '{text}')";

                SqlCommand command = new SqlCommand(querystring, database.GetConnection());

                database.openConnection();
                command.ExecuteNonQuery();
                database.closeConnection();
            }
            else if (e.Button == MouseButtons.Right)
            {
                if (PositionsForUser.Markers.Count > 0)
                {
                    // Удаляем последнюю добавленную точку
                    var markerToRemove = PositionsForUser.Markers[PositionsForUser.Markers.Count - 1];
                    PositionsForUser.Markers.Remove(markerToRemove);

                    // Обновляем карту
                    map.Overlays.Clear();
                    map.Overlays.Add(PositionsForUser);

                    // Обновляем данные
                    UpdateCoordinatesInDatabase();
                }
            }
        }
        private void UpdateCoordinatesInDatabase()
        {
            // Очищаем старые координаты
            string deleteQuery = "delete from coordinates";
            SqlCommand deleteCommand = new SqlCommand(deleteQuery, database.GetConnection());

            database.openConnection();
            deleteCommand.ExecuteNonQuery();
            database.closeConnection();

            // Добавляем новые координаты
            foreach (GMarkerGoogle marker in PositionsForUser.Markers)
            {
                double x = marker.Position.Lng;
                double y = marker.Position.Lat;
                string text = textBox5.Text;

                string insertQuery = $"insert into coordinates(coordinate_x, coordinate_y, coordinate_text) values('{x}', '{y}', '{text}')";

                SqlCommand insertCommand = new SqlCommand(insertQuery, database.GetConnection());

                database.openConnection();
                insertCommand.ExecuteNonQuery();
                database.closeConnection();
            }
        }

        

        private void button7_Click(object sender, EventArgs e)
        {
            map.Overlays.Add(PositionFromTextBoxes);

            if (!string.IsNullOrEmpty(textBox3.Text) && !string.IsNullOrWhiteSpace(textBox3.Text) &&
                !string.IsNullOrEmpty(textBox4.Text) && !string.IsNullOrWhiteSpace(textBox4.Text))
            {
                double x = Convert.ToDouble(textBox4.Text);
                double y = Convert.ToDouble(textBox3.Text);

                GMarkerGoogle marker = new GMarkerGoogle(new PointLatLng(y, x), GMarkerGoogleType.green_pushpin);
                marker.ToolTip = new GMapRoundedToolTip(marker);
                marker.ToolTipText = "Метка пользователя";
                PositionFromTextBoxes.Markers.Add(marker);
                map.Overlays.Add(PositionFromTextBoxes);
            }
            else
                MessageBox.Show("Введите обе координаты", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }

        

        private void map_OnMarkerClick_1(GMapMarker item, MouseEventArgs e)
        {
            
        }

        private void button6_Click(object sender, EventArgs e)
        {
                database.openConnection();


                string deleteQuery = "delete from coordinates";

                var command = new SqlCommand(deleteQuery, database.GetConnection());
                command.ExecuteNonQuery();

                database.closeConnection();
                Form1 form1 = new Form1();
                this.Hide();
                form1.ShowDialog();
                this.Show();
                Close();


        }

        private void button8_Click(object sender, EventArgs e)
        {
            //метки для м4 дон
            GMapOverlay routes = new GMapOverlay("routes");
            GMapRoute route = new GMapRoute(new List<PointLatLng>(){
            new PointLatLng(55.5752, 37.6889),
            new PointLatLng(55.5406, 37.6975),
            new PointLatLng(54.826007999095, 38.0291748046875),
            new PointLatLng(54.7658396372816, 38.1500244140625),
            new PointLatLng(54.1334781428604, 38.0621337890625),
            new PointLatLng(53.7844264712943, 38.001708984375),
            new PointLatLng(52.8127231279155, 38.29833984375),
            new PointLatLng(52.5521411901852, 38.7158203125),
            new PointLatLng(52.3624976999357, 38.9331436157227),
            new PointLatLng(51.9953452950309, 39.2194747924805),//до этой точки все точно
            new PointLatLng(51.9880516573139, 39.2158699035645),
            new PointLatLng(51.9805982351684, 39.215784072876),
            new PointLatLng(51.9744125389844, 39.2219638824463),
            new PointLatLng(51.9665866741325, 39.2235946655273),
            new PointLatLng(51.8433610711596, 39.2143034934998),
            new PointLatLng(51.8413194716245, 39.2149901390076),
            new PointLatLng(51.839171715044, 39.2175006866455),
            new PointLatLng(51.830871396437, 39.2414474487305),
            new PointLatLng(51.8273373300261, 39.2468547821045),
            new PointLatLng(51.8226225996251, 39.2502880096436),
            new PointLatLng(51.8003353406177, 39.2550730705261),
            new PointLatLng(51.7971306410599, 39.2577338218689),
            new PointLatLng(51.7953457303379, 39.2607164382935),
            new PointLatLng(51.7867453189795, 39.2845988273621),
            new PointLatLng(51.7792584408245, 39.296293258667),
            new PointLatLng(51.7757269642698, 39.2995119094849),
            new PointLatLng(51.7627072359539, 39.3064749240875),
            new PointLatLng(51.7590217253176, 39.3070757389069),
            new PointLatLng(51.7493782095027, 39.3056058883667),
            new PointLatLng(51.7464820961325, 39.3059492111206),
            new PointLatLng(51.7307627934618, 39.3110990524292),
            new PointLatLng(51.7266560055536, 39.3111419677734),
            new PointLatLng(51.7124587971142, 39.3076229095459),
            new PointLatLng(51.6963153581804, 39.3038892745972),
            new PointLatLng(51.6811573585434, 39.3017327785492),
            new PointLatLng(51.6680305152628, 39.3003273010254),
            new PointLatLng(51.6652090446646, 39.3015718460083),
            new PointLatLng(51.6587669155174, 39.3023872375488),
            new PointLatLng(51.6551993921578, 39.3030846118927),
            new PointLatLng(51.65400, 39.30289)
            }, "М4 - Дон");
            route.Stroke = new Pen(Color.Red, 7);

            // Добавление Polyline на карту
            routes.Routes.Add(route);
            map.Overlays.Add(routes);
        }
    }
        
}

