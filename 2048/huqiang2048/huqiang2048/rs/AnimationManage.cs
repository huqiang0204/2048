using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace huqiang2048.rs
{
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
        public Point StartPosition { get; set; }
        public Point EndPosition { get; set; }
        public Point StartAngle { get; set; }
        public Point EndAngle { get; set; }
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
