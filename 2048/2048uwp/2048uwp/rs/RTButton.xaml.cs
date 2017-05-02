using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace huqiang2048.rs
{
    /// <summary>
    /// Interaction logic for RTButton.xaml
    /// </summary>
    public partial class RTButton : UserControl
    {
        static Point start = new Point(0.5f,0);
        static Point end = new Point(0.5f,1);
        public static readonly LinearGradientBrush lga_gray = new LinearGradientBrush() {
            StartPoint = start, EndPoint = end, GradientStops = new GradientStopCollection() {
                new GradientStop() { Color=Color.FromScRgb(0.5f,0.5f,0.5f,0.5f),Offset=0},
                new GradientStop() { Color=Color.FromScRgb(0.8f,0.8f,0.8f,0.8f),Offset=0.5f},
                new GradientStop() { Color=Color.FromScRgb(0.5f,0.5f,0.5f,0.5f),Offset=1}
            }
        };
        public static readonly LinearGradientBrush lga_lightblue = new LinearGradientBrush()
        {
            StartPoint = start,
            EndPoint = end,
            GradientStops = new GradientStopCollection() {
                new GradientStop() { Color=Color.FromArgb(128,126,227,223),Offset=0},
                new GradientStop() { Color=Color.FromArgb(200,81,250,244),Offset=0.5f},
                new GradientStop() { Color=Color.FromArgb(128,126,227,223),Offset=1}
            }
        };
        public static readonly LinearGradientBrush lga_yellow = new LinearGradientBrush()
        {
            StartPoint = start,
            EndPoint = end,
            GradientStops = new GradientStopCollection() {
                new GradientStop() { Color=Color.FromArgb(128,197,210,86),Offset=0},
                new GradientStop() { Color=Color.FromArgb(200,232,252,62),Offset=0.5f},
                new GradientStop() { Color=Color.FromArgb(128,197,210,86),Offset=1}
            }
        };
        public static readonly SolidColorBrush scr_black = new SolidColorBrush(Colors.Black);
        public static readonly SolidColorBrush scr_red = new SolidColorBrush(Colors.Red);
        public static readonly SolidColorBrush scr_blue = new SolidColorBrush(Colors.Blue);
        public CornerRadius Corner { set { main.DataContext = value; } }
        public Brush MouseEnterBackgroundBrush { get; set; }
        public Brush MouseLeaveBackgroundBrush { get; set; }
        public Brush MouseDownBackgroundBrush { get; set; }
        public Brush MouseUpBackgroundBrush { get; set; }
        public Brush MouseEnterForegroundBrush { get; set; }
        public Brush MouseLeaveForegroundBrush { get; set; }
        public Brush MouseDownForegroundBrush { get; set; }
        public Brush MouseUpForegroundBrush { get; set; }
        public double RTFontSize { get { return main.FontSize; }
            set { main.FontSize = value; } }
        public object RTContent { get { return main.Content; }
        set { main.Content = value; } }
        public Action<RTButton> Click;
        public RTButton()
        {
            InitializeComponent();
            main.Background = lga_gray;
            main.MouseEnter += (o, e) => {
                if (MouseEnterBackgroundBrush != null)
                    main.Background = MouseEnterBackgroundBrush;
                if (MouseEnterForegroundBrush != null)
                    main.Foreground = MouseEnterForegroundBrush;
            };
            main.MouseLeave += (o, e) => {
                if (MouseLeaveBackgroundBrush != null)
                    main.Background = MouseLeaveBackgroundBrush;
                if (MouseLeaveForegroundBrush != null)
                    main.Foreground = MouseLeaveForegroundBrush;
            };
            main.PreviewMouseDown += (o, e) => {
                if (MouseDownBackgroundBrush != null)
                    main.Background = MouseDownBackgroundBrush;
                if (MouseDownForegroundBrush != null)
                    main.Foreground = MouseDownForegroundBrush;
            };
            main.PreviewMouseUp += (o, e) => {
                if (MouseEnterBackgroundBrush != null)
                    main.Background = MouseEnterBackgroundBrush;
                if (MouseEnterForegroundBrush != null)
                    main.Foreground = MouseEnterForegroundBrush;
            };
            main.Click += (o, e) => { if (Click != null) Click(this); };
            MouseEnterBackgroundBrush = lga_lightblue;
            MouseEnterForegroundBrush = scr_blue;
            MouseDownBackgroundBrush = lga_yellow;
            MouseDownForegroundBrush = scr_red;
            MouseLeaveBackgroundBrush = lga_gray;
            MouseLeaveForegroundBrush = scr_black;
        }
    }
}
