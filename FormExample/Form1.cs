using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Threading;


namespace FormExample
{
    public partial class Form1 : Form
    {
        public static Form form;
        public static Thread thread;
        public static double s = 0;
        public static int fps = 60;
        public static double running_fps = 60.0;
        public static Sprite sprite;
        public static long desired = 0;
        public static Image brando = Image.FromFile("brando.png");

        public Form1()
        {
            InitializeComponent();
            DoubleBuffered = true;
            form = this;
            thread = new Thread(new ThreadStart(run));
            thread.Start();
            sprite = new Sprite(ClientSize.Height - 20, ClientSize.Width - 20);

        }

        public static void run()
        {
            DateTime last = DateTime.Now;
            DateTime now = last;
            TimeSpan frameTime = new TimeSpan(10000000 / fps);
            while (true)
            {
                DateTime temp = DateTime.Now;
                running_fps = .9 * running_fps + .1 * 1000.0 / (temp - now).TotalMilliseconds;
                Console.WriteLine(running_fps);
                now = temp;
                TimeSpan diff = now - last;
                if (diff.TotalMilliseconds< frameTime.TotalMilliseconds)
                    Thread.Sleep((frameTime-diff).Milliseconds);
                last = DateTime.Now;
                BrandoSwarm();
                s += 1.0 / running_fps;
                form.Invoke(new MethodInvoker(form.Refresh));
                
            }
        }

        private void UpdateSize()
        {
            sprite.setDimensions(ClientSize.Height - 20, ClientSize.Width - 20);
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            thread.Abort();
        }

        protected override void OnResize(EventArgs e)
        {

            
            Refresh();
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            //Refresh();
            base.OnMouseDown(e);
        }

        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            base.OnKeyPress(e);
            if (e.KeyChar == 'a' || e.KeyChar == '0' || e.KeyChar == '1') 
            {
                desired += 1;
            }
            else if (e.KeyChar == 'r')
            {
                if (desired > 0)
                {
                    desired -= 1;
                }
            }
            else if (e.KeyChar == 'c')
            {
                sprite.clear();
                desired = 0;
            }
            else if (e.KeyChar == '2') 
            {
                desired += 10;
            }
            else if (e.KeyChar == '3')
            {
                desired += 100;
            }
            else if (isDigit(e.KeyChar))
            {
                desired += 10 * Int32.Parse(e.KeyChar+"");
            }
            else
            {
                Random rand = new Random();
                desired += rand.Next(1, 101);
            }
        }

        protected Boolean isDigit(char c)
        {
            return (c == '0' || c == '1' || c == '2' || c == '3' || c == '4' || c == '5' || c == '6' || c == '7' || c == '8' || c == '9');
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            //Refresh();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            //Console.WriteLine(s);
            sprite.render(e.Graphics);
            e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(200, Color.Black)), 0, 0, ClientSize.Width, 20);
            String s1 = "Brando Count: " + sprite.count();
            int s1x = 5;
            e.Graphics.DrawString(s1, new Font("Comic Sans MS", 10), Brushes.LawnGreen, s1x, 0);
            String s2 = "FPS: " + running_fps;
            int s2x = 200;
            e.Graphics.DrawString(s2, new Font("Comic Sans MS", 10), Brushes.LawnGreen, s2x, 0);
            double adjSpeed = 1.0 / running_fps;
            String s3 = "Speed per tick: " + adjSpeed;
            int s3x = 400;
            e.Graphics.DrawString(s3, new Font("Comic Sans MS", 10), Brushes.LawnGreen, s3x, 0);
            String s4 = "Speed per second: " + (running_fps * adjSpeed);
            int s4x = 650;
            e.Graphics.DrawString(s4, new Font("Comic Sans MS", 10), Brushes.LawnGreen, s4x, 0);
            float v = (float)(Math.Abs(200 + 100 * Math.Sin((s * 2 / Math.PI))));
            e.Graphics.DrawRectangle(new Pen(Color.Navy, 10), 0, 0, v, v);

        }

        protected static void BrandoSwarm()
        {
            while (sprite.count() < desired)
            {
                sprite.add(new Face(form.ClientSize.Height - 20, form.ClientSize.Width - 20));
            }
            while(sprite.count() > desired)
            {
                if (sprite.count() <= 0)
                {
                    break;
                }
                else
                {
                    sprite.remove();
                }
            }
        }


    }
    
}
