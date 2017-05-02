using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Windows.UI.Xaml;

namespace huqiang2048.rs
{
    public struct Vector
    {
        public double X;
        public double Y;
        public Vector(double x, double y) { X = x; Y = y; }
        public static Vector operator -(Vector v1, Vector v2)
        {
            Vector v = new Vector();
            v.X = v1.X - v2.X;
            v.Y = v1.Y - v2.Y;
            return v;
        }
        public static Vector operator +(Vector v1, Vector v2)
        {
            Vector v = new Vector();
            v.X = v1.X + v2.X;
            v.Y = v1.Y + v2.Y;
            return v;
        }
    }
    public class AnimationManage
    {
        static AnimationManage am;
        public static AnimationManage Manage { get { if (am == null) am = new AnimationManage(); return am; } }
        //public void Add
        List<Animat> Actions;
        AnimationManage()
        {
            Actions = new List<Animat>();
        }
        public void Update(int timeslice)
        {
            for (int i = 0; i < Actions.Count; i++)
            {
                if (Actions[i] != null)
                    Actions[i].Update(timeslice);
            }
        }
        public Animat CreateAnimat(FrameworkElement target)
        {
            var ani = new Animat(target);
            Actions.Add(ani);
            return ani;
        }
        public void ReleaseAnimat(Animat ani)
        {
            Actions.Remove(ani);
        }
        public void ReleaseAll()
        {
            Actions.Clear();
        }
    }
    public class Animat
    {
        public object DataContext { get; set; }
        public Animat(FrameworkElement t)
        {
            Target = t;
        }
        public FrameworkElement Target { get; private set; }
        public Vector StartPosition { get; set; }
        public Vector EndPosition { get; set; }
        public Vector StartAngle { get; set; }
        public Vector EndAngle { get; set; }
        float time;
        public float Delay { get; set; }
        public float Time { get { return time; } set { SurplusTime = time = value; } }
        float SurplusTime;
        bool playing = false;
        public Action<Animat> OnPlayOver;
        public void Play()
        {
            playing = true;
        }
        public void Pause()
        {
            playing = false;
        }
        public void Update(int timeslice)
        {
            if (playing)
            {
                if (Delay > 0)
                { Delay -= timeslice; }
                else
                {
                    SurplusTime -= timeslice;
                    if (SurplusTime <= 0)
                    {
                        playing = false;
                        Target.Margin = new Thickness(EndPosition.X,EndPosition.Y,0,0);
                        if (OnPlayOver != null)
                            OnPlayOver(this);
                    }
                    else
                    {
                        float r = 1 - SurplusTime / time;
                        Vector v = EndPosition - StartPosition;
                        Thickness t = new Thickness();
                        t.Left = v.X * r + StartPosition.X;
                        t.Top = v.Y * r + StartPosition.Y;
                        Target.Margin = t;
                    }
                }
            }
        }
    }
}
