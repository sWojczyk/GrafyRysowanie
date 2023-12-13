using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GrafyRysowanie
{
    public partial class Form1 : Form
    {
        private const int r = 10;
        private Graphics g;
        private Pen pWierzcholek;
        private Pen pWierzcholekAktywny;
        private Pen pKrawedz;
        private Wierzchlek MouseDownWierzcholek;

        private List<Wierzchlek> wierzcholki = new List<Wierzchlek>();

        public Form1()
        {
            InitializeComponent();

            pictureBox1.Image = new Bitmap(500,500);

            g = Graphics.FromImage(pictureBox1.Image);
            g.Clear(Color.White);
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

            pWierzcholek = new Pen(Color.Orange);
            pWierzcholek.Width = 3;
            pWierzcholekAktywny = new Pen(Color.Red);
            pWierzcholekAktywny.Width = 3;

            pKrawedz= new Pen(Color.Blue);
            pKrawedz.Width = 10;
            pKrawedz.EndCap = System.Drawing.Drawing2D.LineCap.ArrowAnchor;
        }
        

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                MouseDownWierzcholek = null;
                foreach (Wierzchlek w in wierzcholki)
                {
                    if (w.Odleglosc(e.Location) < r)
                    {
                        MouseDownWierzcholek = w;
                    }
                }
                odrysujGraf();
            }
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && MouseDownWierzcholek!=null)
            {
                foreach (Wierzchlek w in wierzcholki)
                {
                    if (w.Odleglosc(e.Location) < r)
                    {
                        MouseDownWierzcholek.Nastpniki.Add(w);
                        //w.Nastpniki.Add(MouseDownWierzcholek);
                    }
                }
                MouseDownWierzcholek = null;
                odrysujGraf();
            }
            else if (e.Button == MouseButtons.Middle)
            {
                wierzcholki.Add(new Wierzchlek(e.Location));
                odrysujGraf();
            }
        }

        private void odrysujGraf()
        {
            g.Clear(Color.White);
            foreach (Wierzchlek w in wierzcholki)
            {
                
                g.DrawEllipse(pWierzcholek, w.Polozenie.X-r, w.Polozenie.Y-r, 2*r, 2*r);
                g.DrawString(w.Id.ToString(),
                             new System.Drawing.Font("Microsoft Sans Serif", r),
                             new SolidBrush(Color.Red), 
                             w.Polozenie.X+r, 
                             w.Polozenie.Y+r);
                if (w==MouseDownWierzcholek)
                {
                    g.DrawEllipse(pWierzcholekAktywny, w.Polozenie.X-r, w.Polozenie.Y-r, 2*r, 2*r);
                }
                foreach(Wierzchlek wn in w.Nastpniki)
                {
                    g.DrawLine(pKrawedz, w.Polozenie, wn.Polozenie);
                }
            }
            pictureBox1.Refresh();
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && MouseDownWierzcholek!=null)
            {
                odrysujGraf();
                g.DrawLine(pKrawedz, MouseDownWierzcholek.Polozenie, e.Location);
                pictureBox1.Refresh();
            }

        }
    }
}
