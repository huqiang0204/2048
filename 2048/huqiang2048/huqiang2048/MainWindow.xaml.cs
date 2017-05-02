using System;
using System.Collections.Generic;
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
using huqiang2048.rs;
namespace huqiang2048
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            GameManage.Current.SetParent(main_can);
            start.Click = (o) => { GameManage.Current.StartGame(); };
            main_can.KeyDown += (o, e) => {
                if (e.Key == Key.Left | e.Key == Key.Right | e.Key == Key.Up | e.Key == Key.Down)
                    GameManage.Current.Acceleration();
            };
            main_can.KeyUp += (o, e) =>
            {
                switch(e.Key)
                {
                    case Key.Left:
                        GameManage.Current.MoveLeft();
                        break;
                    case Key.Right:
                        GameManage.Current.MoveRight();
                        break;
                    case Key.Up:
                        GameManage.Current.MoveUp();
                        break;
                    case Key.Down:
                        GameManage.Current.MoveDown();
                        break;
                }
            };
        }
    }
}
