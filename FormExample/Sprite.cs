using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace FormExample
{
    public class Sprite
    {
        //instance variable
        private float x=0;
        protected int height=100;
        protected int width=100;

        public float X
        {
            get { return x; }
            set { x = value; }
        }

        private float y = 0;

        public float Y
        {
            get { return y; }
            set { y = value; }
        }

        private float scale = 1;

        public float Scale
        {
            get { return scale; }
            set { scale = value; }
        }

        protected List<Sprite> children = new List<Sprite>();


        //constructors

        public Sprite(int h, int w)
        {
            height = h;
            width = w;
        }

        public Sprite()
        {
        }

        //methods
        public void render(Graphics g)
        {
            Matrix original = g.Transform.Clone();
            g.ScaleTransform(scale,scale);
            g.TranslateTransform(x, y);
            paint(g);
            foreach(Sprite s in children)
            {
                s.render(g);
            }
            g.Transform = original;
        }

        public void setDimensions(int h, int w)
        {
            height = h;
            width = w;
            foreach (Sprite s in children)
            {
                s.setDimensions(height, width);
            }
        }

        public void clear()
        {
            children.Clear();
        }

        public virtual void paint(Graphics g)
        {

        }

        public void add(Sprite s)
        {
            children.Add(s);
        }

        public void remove()
        {
            children.Remove(children.Last());
        }

        public long count()
        {
            return children.Count;
        }

    }

    public class Face :Sprite
    {
        private Image breading = Image.FromFile("brando.png");
        private int hseed = 0;
        private int wseed = 0;
        private int hspeed = 1;
        private int wspeed = 1;
        private int s = 0;
        private int type;

        public Face(int h, int w) : base(h, w)
        {
            Random rand = new Random();
            hseed = rand.Next(500) + DateTime.Now.Millisecond % h;
            wseed = rand.Next(500) + DateTime.Now.Millisecond % w;
            hspeed = rand.Next(-5,5);
            wspeed = rand.Next(-5,5);
            Random randobrando = new Random();
            type = randobrando.Next(4);
        }
        
        public override void paint(Graphics g)
        {
            float x = 0;
            float y = 0;
            if (type == 0)
            {
                x = (float)(((width - 75) / 2) * Math.Cos((s / 50.0) + wspeed + wseed)) + ((width - 75) / 2);
                y = (float)(((height - 75) / 2) * Math.Sin((s / 50.0) + hspeed + hseed)) + ((height - 75) / 2);
            }
            else if (type == 1)
            {
                x = (float)(((width - 75) / 2) * Math.Sin((s / 50.0) + wspeed + wseed)) + ((width - 75) / 2);
                y = (float)(((height - 75) / 2) * Math.Cos((s / 50.0) + hspeed + hseed)) + ((height - 75) / 2);
            }
            else if (type == 2)
            {
                x = (float)(((width - 75) / 2) * Math.Cos((s / 50.0) + wspeed + wseed)) + ((width - 75) / 2);
                y = (float)(((height - 75) / 2) * Math.Cos((s / 50.0) + hspeed + hseed)) + ((height - 75) / 2);
            }
            else if (type == 3)
            {
                x = (float)(((width - 75) / 2) * Math.Sin((s / 50.0) + wspeed + wseed)) + ((width - 75) / 2);
                y = (float)(((height - 75) / 2) * Math.Sin((s / 50.0) + hspeed + hseed)) + ((height - 75) / 2);
            }
            g.DrawImage(breading, x, y);
            s++;
        }

    }
}
