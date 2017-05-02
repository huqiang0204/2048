using System;
using System.Collections.Generic;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml;
using Windows.UI;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Input;
using Windows.UI.Input;
using Windows.Foundation;

namespace huqiang2048.rs
{
    class GameManage
    {
        const double startx = 12;//10+2
        const double starty = 3;//61+3
        const double gap = 125;
        const int movetime= 200;
        static GameManage gm;
        public static GameManage Current {
            get { if (gm == null) gm = new GameManage();return gm; } }
        Border[] bor_buff;
        List<Animat> ani_buff;
        Animat[] anis=new Animat[16];
        SolidColorBrush[] color_buff;
        Border CreateItem()
        {
            Border bor = new Border();
            bor.Width = 120;
            bor.Height = 120;
            bor.CornerRadius = new CornerRadius(12,12,12,12);
            bor.Background = color_buff[10];
            TextBlock tb= new TextBlock();
            tb.FontSize = 36;
            tb.HorizontalAlignment = HorizontalAlignment.Center;
            tb.VerticalAlignment = VerticalAlignment.Center;
            bor.Child = tb;
            return bor;
        }
        private GameManage()
        {
            if(color_buff==null)
            {
                color_buff = new SolidColorBrush[] {
                    new SolidColorBrush(Colors.Azure),
                    new SolidColorBrush(Colors.Brown),
                    new SolidColorBrush(Colors.Yellow),
                    new SolidColorBrush(Colors.Green),
                    new SolidColorBrush(Colors.HotPink),
                    new SolidColorBrush(Colors.Blue),
                    new SolidColorBrush(Colors.Red),
                    new SolidColorBrush(Colors.Silver),
                    new SolidColorBrush(Colors.Goldenrod),
                    new SolidColorBrush(Colors.Aqua),
                    new SolidColorBrush(Colors.Coral),
                    new SolidColorBrush(Colors.DarkBlue),
                    new SolidColorBrush(Colors.LightGreen),
                        new SolidColorBrush(Colors.DeepPink)
                };
                bor_buff = new Border[16];
                ani_buff = new List<Animat>();
                for (int i = 0; i < 16; i++)
                {
                    var b = CreateItem();
                    b.Visibility = Visibility.Collapsed;
                    bor_buff[i] = b;
                    var ani = AnimationManage.Manage.CreateAnimat(b);
                    var si = new SquareInfo();
                    si.index = i;
                    ani.DataContext = si;
                    ani_buff.Add(ani);
                }
            }
            CompositionTarget.Rendering += Update;
            lasttime = DateTime.Now.Millisecond;
            r = new Random();
        }
        int lasttime;
        int speed=1;
        void Update(object o,object e)
        {
            int t = DateTime.Now.Millisecond;
            int st = t - lasttime;
            lasttime = t;
            if (st < 0)
                st += 1000;
            Update(st);
        }
        void Update(int slicetime)
        {
            slicetime *= speed;
            AnimationManage.Manage.Update(slicetime);
            if(create)
            {
                delay -= slicetime;
                if(delay<=0)
                {
                    create = false;
                    CreateNew();
                }
            }
        }
        public void Acceleration()
        {
            speed = 2;
        }
        void Reset()
        {
            for (int i = 0; i < 16; i++)
            {
                bor_buff[i].Visibility = Visibility.Collapsed;
                bor_buff[i].Background = color_buff[0];
                (bor_buff[i].Child as TextBlock).Foreground = color_buff[10];
                var si = ani_buff[i].DataContext as SquareInfo;
                si.Number = 0;
                si.levevl = 0;
                si.Merge = false;
                anis[i] = null;
            }
        }
        Window mainwindow;
        Canvas Content;
        Canvas main_can;
        ImageBrush pressd;
        ImageBrush unpressd;
        Border pressbor;
        Button gamebutton;
        public void Inital(Window p)
        {
            mainwindow = p;
            Content = new Canvas();
            p.Content = Content;
            main_can = new Canvas();
            Content.Children.Add(main_can);
            Image img = new Image();
            img.Width = 500;
            img.Height = 500;
            img.Source = new BitmapImage(new Uri("ms-appx:///rs/2048.png"));
            img.Margin = new Thickness(10,0,0,0);
            main_can.Children.Add(img);
            for (int i = 0; i < 16; i++)
                    main_can.Children.Add(bor_buff[i]);
            var bi= new BitmapImage(new Uri("ms-appx:///rs/dir.png"));
            unpressd = new ImageBrush();
            unpressd.ImageSource = bi;
            bi = new BitmapImage(new Uri("ms-appx:///rs/dirp.png"));
            pressd = new ImageBrush();
            pressd.ImageSource = bi;
            pressbor = new Border();
            pressbor.Width = 100;
            pressbor.Height = 100;
            RotateTransform rt = new RotateTransform();
            rt.CenterX = 50f;
            rt.CenterY = 50f;
            pressbor.Background = unpressd;
            pressbor.RenderTransform = rt;
            pressbor.PointerPressed += PointerPressd;
            pressbor.PointerReleased += PointerReleasd;
            Content.Children.Add(pressbor);

            gamebutton = new Button();
            gamebutton.Content = "开始游戏";
            gamebutton.Click += (o, e) => { StartGame(); };
            Content.Children.Add(gamebutton);
            
            ScaleTransform st = new ScaleTransform();
            st.CenterX = 0.5f;
            st.CenterY =0.5f;
            main_can.RenderTransform = st;
            mainwindow.SizeChanged += (o,e)=> { Resize(); };
            Resize();
        }
        void Resize()
        {
            double w = mainwindow.Bounds.Width;
            double h = mainwindow.Bounds.Height;
            if (w > h)
            {
                double x = w - 120;
                w -= 120;
                (main_can.RenderTransform as ScaleTransform).ScaleX  = w / 520;
                (main_can.RenderTransform as ScaleTransform).ScaleY  = h / 570;
                gamebutton.Margin = new Thickness(x, h - 50, 0, 0);
                pressbor.Margin = new Thickness(x, h * 0.5f - 50, 0, 0);
            }
            else
            {
                double y = h - 120;
                h -= 60;
                (main_can.RenderTransform as ScaleTransform).ScaleX  = w / 520;
                (main_can.RenderTransform as ScaleTransform).ScaleY  = h / 570;
                gamebutton.Margin = new Thickness(10, y+50, 0, 0);
                pressbor.Margin = new Thickness(w * 0.5f - 50, y, 0, 0);
            }
        }
        Random r;
        bool playing=false;
        int createposition;
        int CreateNumber()
        {
           int n= r.Next(60,100);
            n /= 40;
            return n;
        }
      
        #region create
        void StartGame()
        {
            Reset();
            int a = r.Next(0, 15);
            int b = r.Next(0, 15);
            if (a == b)
                b++;
            if (b > 15)
                b = 0;
            int i = a / 4;
            int j = a % 4;
            var bor = bor_buff[0];
            bor.Visibility = Visibility.Visible;
            bor.Background = color_buff[0];
            var t = bor.Child as TextBlock;
            t.FontSize = 72;
            t.Foreground = color_buff[1];
            t.Text = "2";
            bor.Margin = new Thickness(startx + j * gap, starty + i * gap, 0, 0);
            anis[a] = ani_buff[0];
            (anis[a].DataContext as SquareInfo).Number = 2;

            i = b / 4;
            j = b % 4;
            bor = bor_buff[1];
            bor.Background = color_buff[0];
            t = bor.Child as TextBlock;
            t.FontSize = 72;
            t.Foreground = color_buff[1];
            t.Text = "2";
            bor.Visibility = Visibility.Visible;
            bor.Margin = new Thickness(startx + j * gap, starty + i * gap, 0, 0);
            anis[b] = ani_buff[1];
            (anis[b].DataContext as SquareInfo).Number = 2;
            playing = true;
        }
        void CaculOver()
        {
            for (int i = 0; i < 16; i++)
            {
                var si = ani_buff[i].DataContext as SquareInfo;
                si.Merge = false;
            }
        }
        int GetZeroAni()
        {
            for (int i = 0; i < 16; i++)
                if (bor_buff[i].Visibility == Visibility.Collapsed)
                    return i;
            return 0;
        }
        void CreateNew()
        {
            if (createposition == 0)
                createposition = r.Next(0, 15);
            for (int o = 0; o < 16; o++)
            {
                if (anis[createposition] != null)
                {
                    createposition++;
                    if (createposition > 15)
                        createposition = 0;
                }
                else
                {
                    int i = createposition / 4;
                    int j = createposition % 4;
                    int level = CreateNumber();
                    int n = level * 2;
                    level--;
                    int index = GetZeroAni();
                    var bor = bor_buff[index];
                    bor.Visibility = Visibility.Visible;
                    bor.Background = color_buff[level];
                    var t = bor.Child as TextBlock;
                    t.FontSize = 72;
                    t.Foreground = color_buff[level+1];
                    t.Text = n.ToString();
                    bor.Margin = new Thickness(startx + j * gap, starty + i * gap, 0, 0);
                    anis[createposition] = ani_buff[index];
                    var si = anis[createposition].DataContext as SquareInfo;
                    si.Number = n;
                    si.levevl = level;
                    return;
                }
            }
        }
        int delay;
        bool create;
        #endregion
     
        //    x0 x1 x2 x3 
        //y0
        //y1
        //y2
        //y3
        //left      x0y0 x1y0 x2y0 x3y0  x0y1 x1y1 x2y1 x3y1
        //right    x3y0 x2y0 x2y1 x0y0  x3y1 x2y1 x1y1 x0y1
        //up       x0y0 x0y1 x0y2 x0y3  x1y0 x1y1 x1y2 x1y3
        //down  x0y3 x0y2 x0y1 x0y0  x1y3 x1y2 x1y1 x1y0
        #region move x
        public void MoveLeft()
        {
            speed = 1;
            if (!playing)
                return;
            createposition = 0;
            for (int j = 0; j < 16; j += 4)
            {
                int left = j;
                SquareInfo ssi = null;
                if (anis[left] != null)
                    ssi = anis[left].DataContext as SquareInfo;
                for (int i = 1; i < 4; i++)
                {
                    int s = j + i;
                    if (anis[s] != null)
                    {
                        var si = anis[s].DataContext as SquareInfo;
                        if (si.Number > 0)
                        {
                            if (ssi != null)
                            {
                                if (ssi.Number == si.Number & ssi.Merge == false)
                                {
                                    ssi.Number +=ssi.Number;
                                    ssi.levevl++;
                                    MoveMerge(s, left);
                                    ssi.Merge = true;
                                    anis[s] = null;
                                }
                                else
                                {
                                    left++;
                                    ssi = si;
                                    if (s > left)
                                    {
                                        anis[left] = anis[s];
                                        Move(s, left);
                                        anis[s] = null;
                                    }
                                }
                            }
                            else
                            {
                                ssi = si;
                                anis[left] = anis[s];
                                Move(s, left);
                                anis[s] = null;
                            }
                        }
                    }
                }
            }
            CaculOver();
        }
        public void MoveRight()
        {
            speed = 1;
            if (!playing)
                return;
            createposition = 0;
            for (int j = 0; j < 16; j += 4)
            {
                int left = j+3;
                SquareInfo ssi = null;
                if (anis[left] != null)
                    ssi = anis[left].DataContext as SquareInfo;
                for (int i = 2; i >= 0; i--)
                {
                    int s = j + i;
                    if (anis[s] != null)
                    {
                        var si = anis[s].DataContext as SquareInfo;
                        if (si.Number > 0)
                        {
                            if (ssi != null)
                            {
                                if (ssi.Number == si.Number & ssi.Merge == false)
                                {
                                    ssi.Number <<= 1;
                                    ssi.levevl++;
                                    MoveMerge(s, left);
                                    ssi.Merge = true;
                                    anis[s] = null;
                                }
                                else
                                {
                                    left--;
                                    ssi = si;
                                    if (s < left)
                                    {
                                        anis[left] = anis[s];
                                        Move(s, left);
                                        anis[s] = null;
                                    }
                                }
                            }
                            else
                            {
                                ssi = si;
                                anis[left] = anis[s];
                                Move(s, left);
                                anis[s] = null;
                            }
                        }
                    }
                }
            }
            CaculOver();
        }
        void Move(int s,int e)
        {
            double x = s % 4;
            x *= gap;
            x += startx;
            double y = s / 4;
            y *= gap;
            y += starty;
            anis[s].StartPosition = new Vector(x,y);
            x = e % 4;
            x *= gap;
            x += startx;
            y = e / 4;
            y *= gap;
            y += starty;
            anis[s].EndPosition = new Vector(x, y);
            anis[s].Time = movetime;
            anis[s].Play();
            if (e == createposition)
                createposition = s;
            create = true;
            delay = movetime;
        }
        void MoveMerge(int s,int e)
        {
            var si = anis[s].DataContext as SquareInfo;
            si.Target = anis[e];
            si.Index_s = s;
            si.Index_e = e;
            double x = s % 4;
            x *= gap;
            x += startx;
            double y = s / 4;
            y *= gap;
            y += starty;
            anis[s].StartPosition = new Vector(x, y);
            x = e % 4;
            x *= gap;
            x += startx;
            y = e / 4;
            y *= gap;
            y += starty;
            anis[s].EndPosition = new Vector(x, y);
            anis[s].Time = movetime;
            anis[s].Play();
            anis[s].OnPlayOver = MoveMerge;
            createposition = s;
            create = true;
            delay = movetime;
        }
        void MoveMerge(Animat ani)
        {
            ani.OnPlayOver = null;
            var si = ani.DataContext as SquareInfo;
            (ani.Target as Border).Visibility = Visibility.Collapsed;
            Animat a = si.Target;
            si = a.DataContext as SquareInfo;
            Border bor = a.Target as Border;
            bor.Background = color_buff[si.levevl];
            TextBlock tb = bor.Child as TextBlock;
            tb.Text = si.Number.ToString();
            if (si.Number > 1000)
                tb.FontSize = 36;
            tb.Foreground =color_buff[si.levevl+1];
        }
        #endregion

        #region move y
        public void MoveUp()
        {
            if (!playing)
                return;
            createposition = 0;
            for (int j = 0; j < 4; j ++)
            {
                int left = j;
                SquareInfo ssi = null;
                if (anis[left] != null)
                    ssi = anis[left].DataContext as SquareInfo;
                for (int i = 4; i < 15; i+=4)
                {
                    int s = j + i;
                    if (anis[s] != null)
                    {
                        var si = anis[s].DataContext as SquareInfo;
                        if (si.Number > 0)
                        {
                            if (ssi != null)
                            {
                                if (ssi.Number == si.Number & ssi.Merge == false)
                                {
                                    ssi.Number += ssi.Number;
                                    ssi.levevl++;
                                    MoveMerge(s, left);
                                    ssi.Merge = true;
                                    anis[s] = null;
                                }
                                else
                                {
                                    left+=4;
                                    ssi = si;
                                    if (s > left)
                                    {
                                        anis[left] = anis[s];
                                        Move(s, left);
                                        anis[s] = null;
                                    }
                                }
                            }
                            else
                            {
                                ssi = si;
                                anis[left] = anis[s];
                                Move(s, left);
                                anis[s] = null;
                            }
                        }
                    }
                }
            }
            CaculOver();
        }
        public void MoveDown()
        {
            if (!playing)
                return;
            createposition = 0;
            for (int j = 0; j < 4; j++)
            {
                int left =12+ j;
                SquareInfo ssi = null;
                if (anis[left] != null)
                    ssi = anis[left].DataContext as SquareInfo;
                for (int i = 8; i >=0; i -= 4)
                {
                    int s = j + i;
                    if (anis[s] != null)
                    {
                        var si = anis[s].DataContext as SquareInfo;
                        if (si.Number > 0)
                        {
                            if (ssi != null)
                            {
                                if (ssi.Number == si.Number & ssi.Merge == false)
                                {
                                    ssi.Number += ssi.Number;
                                    ssi.levevl++;
                                    MoveMerge(s, left);
                                    ssi.Merge = true;
                                    anis[s] = null;
                                }
                                else
                                {
                                    left -= 4;
                                    ssi = si;
                                    if (s < left)
                                    {
                                        anis[left] = anis[s];
                                        Move(s, left);
                                        anis[s] = null;
                                    }
                                }
                            }
                            else
                            {
                                ssi = si;
                                anis[left] = anis[s];
                                Move(s, left);
                                anis[s] = null;
                            }
                        }
                    }
                }
            }
            CaculOver();
        }
        #endregion

        void PointerPressd(object o, PointerRoutedEventArgs e)
        {
            speed = 2;
            if(playing)
            {
                var pp = PointerPoint.GetCurrentPoint(e.Pointer.PointerId);
                float x = (float)(pp.Position.X - pressbor.Margin.Left-50);
                float y = (float)(pressbor.Margin.Top+50 - pp.Position.Y);
                if (x * x + y * y > 2500)
                    return;
                float a = atan(x, y);
                if (a < 45)
                    (pressbor.RenderTransform as RotateTransform).Angle = 0;
                else if (a < 135)
                    (pressbor.RenderTransform as RotateTransform).Angle = 270;
                else if (a < 225)
                    (pressbor.RenderTransform as RotateTransform).Angle = 180;
                else if (a < 315)
                    (pressbor.RenderTransform as RotateTransform).Angle = 90;
                else (pressbor.RenderTransform as RotateTransform).Angle = 0;
                pressbor.Background = pressd;
            }
        }
        void PointerReleasd(object o, PointerRoutedEventArgs e)
        {
            speed = 1;
            pressbor.Background = unpressd;
            if (!playing)
                return;
            var pp = PointerPoint.GetCurrentPoint(e.Pointer.PointerId);
            float x = (float)(pp.Position.X - pressbor.Margin.Left - 50);
            float y = (float)(pressbor.Margin.Top + 50 - pp.Position.Y);
            if (x * x + y * y > 2500)
                return;
            float a = atan(x,y);
            if (a < 45)
              MoveUp(); 
            else if (a < 135)
                MoveLeft();
            else if (a < 225)
                MoveDown();
            else if (a < 315)
                MoveRight();
            else MoveUp();
        }
        public static float atan(float dx, float dy)
        {
            //ax<ay
            float ax = dx < 0 ? -dx : dx, ay = dy < 0 ? -dy : dy;
            float a;
            if (ax < ay) a = ax / ay; else a = ay / ax;
            float s = a * a;
            float r = ((-0.0464964749f * s + 0.15931422f) * s - 0.327622764f) * s * a + a;
            if (ay > ax) r = 1.57079637f - r;
            r *= 57.32484f;
            if (dx < 0)
            {
                if (dy < 0)
                    r += 90;
                else r = 90 - r;
            }
            else
            {
                if (dy < 0)
                    r = 270 - r;
                else r += 270;
            }
            return r;
        }
    }
    class SquareInfo
    {
        public int levevl;
        public int index;
        public int Number;
        public int Index_s;
        public int Index_e;
        public bool Merge;
        public Animat Target;
    }
}
