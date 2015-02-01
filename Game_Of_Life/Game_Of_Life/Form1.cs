using System;
using System.Drawing;
using System.Windows.Forms;

namespace Game_Of_Life
{
    public partial class Form1 : Form
    {
        int SIZEX = 10;
        int SIZEY = 10;
        const int CELLSIZE = 10;
        Cell[,] Life;
        bool[,] nextLife;

        System.Drawing.Graphics graphicsObj;
        Pen myPen = new Pen(System.Drawing.Color.Black, 1);

        #region Form
        public Form1()
        {
            InitializeComponent();
        }

        #endregion

        #region Buttons
        private void btnDraw_Click(object sender, EventArgs e)
        {
            graphicsObj = this.CreateGraphics();
            try
            {
                SIZEX = int.Parse(txtX.Text);
                SIZEY = int.Parse(txtY.Text);
                btnDraw.Enabled = false;
                txtX.Enabled = false;
                txtY.Enabled = false;
            }
            catch (Exception)
            {

                MessageBox.Show("Données non valides!");
            }

            Life = new Cell[SIZEX, SIZEY];
            nextLife = new bool[SIZEX, SIZEY];

            for (int x = 0; x < SIZEX; x++)
            {
                for (int y = 0; y < SIZEY; y++)
                {
                    Life[x, y] = new Cell();
                    Life[x, y].cell = new Rectangle((x * CELLSIZE), (y * CELLSIZE), CELLSIZE, CELLSIZE);
                    graphicsObj.FillRectangle(new SolidBrush(Color.White), Life[x, y].cell);
                    graphicsObj.DrawRectangle(myPen, Life[x, y].cell);
                    Life[x, y].alive = false;
                }
            }

            button2.Enabled = true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            button1_Click(null, null);
            btnDraw.Enabled = true;
            button2.Enabled = false;
            txtY.Enabled = true;
            txtX.Enabled = true;
            for (int x = 0; x < SIZEX; x++)
            {
                for (int y = 0; y < SIZEY; y++)
                {
                    graphicsObj.FillRectangle(new SolidBrush(Color.White), Life[x, y].cell);
                    graphicsObj.DrawRectangle(myPen, Life[x, y].cell);
                    Life[x, y].alive = false;
                }
            }
            Life = null;
            this.Refresh();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            button2.Enabled = false;
            button1.Enabled = true;
            timer2.Start();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            button2.Enabled = true;
            timer2.Stop();
        }
        #endregion

        #region Timer
        private void timer1_Tick(object sender, EventArgs e)
        {
            color();
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            run();
        }
        #endregion

        #region Logic
        private void run()
        {
            for (int i = 0; i < SIZEX; i++)
            {
                for (int j = 0; j < SIZEY; j++)
                {
                    int count = entorno(i, j);
                    bool result = false;

                    if (Life[i, j].alive)
                    {
                        if ((count == 2 || count == 3))
                            result = true;
                        else result = false;
                    }

                    else
                    {
                        if (count == 3)
                            result = true;
                        else result = false;
                    }

                    nextLife[i, j] = result;
                }
            }

            for (int i = 0; i < SIZEX; i++)
            {
                for (int j = 0; j < SIZEY; j++)
                {
                    Life[i, j].alive = nextLife[i, j];

                    if (Life[i, j].alive)
                    {
                        graphicsObj.FillRectangle(new SolidBrush(Color.Blue), Life[i, j].cell);
                        graphicsObj.DrawRectangle(myPen, Life[i, j].cell);
                    }
                    else
                    {
                        graphicsObj.FillRectangle(new SolidBrush(Color.White), Life[i, j].cell);
                        graphicsObj.DrawRectangle(myPen, Life[i, j].cell);
                    }
                }
            }
        }

        private int entorno(int x, int y)
        {
            int c = 0;

            int si = (x - 1 < 0) ? 0 : x - 1;
            int sj = (y - 1 < 0) ? 0 : y - 1;
            int fi = (x + 2 > SIZEX) ? SIZEX : x + 2;
            int fj = (y + 2 > SIZEY) ? SIZEY : y + 2;

            for (int i = si; i < fi; i++)
            {
                for (int j = sj; j < fj; j++)
                {
                    if (i != x || j != y)
                        if (Life[i, j].alive)
                        {
                            c++;
                        }
                }

            }

            return c;
        }
        #endregion

        #region Cell paint
        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            timer1.Enabled = true;
            timer1.Start();
        }

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            timer1.Stop();
        }

        private void color()
        {
            Point point = this.PointToClient(MousePosition);

            int x = point.X / CELLSIZE;
            int y = point.Y / CELLSIZE;

            try
            {
                if (!Life[x, y].alive)
                {
                    graphicsObj.FillRectangle(new SolidBrush(Color.Blue), Life[x, y].cell);
                    graphicsObj.DrawRectangle(myPen, Life[x, y].cell);
                    Life[x, y].alive = true;
                }
                else
                {
                    graphicsObj.FillRectangle(new SolidBrush(Color.White), Life[x, y].cell);
                    graphicsObj.DrawRectangle(myPen, Life[x, y].cell);
                    Life[x, y].alive = false;
                }
            }
            catch (Exception)
            {
            }
        }
        #endregion

        #region Class Cell
        class Cell
        {
            public Rectangle cell { get; set; }
            public bool alive { get; set; }
        }
        #endregion

        private void Form1_Load(object sender, EventArgs e)
        {

        }


    }
}
