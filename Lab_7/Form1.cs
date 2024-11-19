using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Lab_7
{
    public partial class Form1 : Form
    {
        double ScreenW, ScreenH;
        private float devX;
        private float devY;
        private float centerX, centerY;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            GL.ClearColor(Color.White);
            GL.Viewport(0, 0, AnT.Width, AnT.Height);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();

            if ((float)AnT.Width <= (float)AnT.Height)
            {
                ScreenW = 30.0;
                ScreenH = 30.0 * (float)AnT.Height / (float)AnT.Width;
                GL.Ortho(0.0, ScreenW, 0.0, ScreenH, -1, 1);
            }
            else
            {
                ScreenW = 30.0 * (float)AnT.Width / (float)AnT.Height;
                ScreenH = 30.0;
                GL.Ortho(0.0, 30.0 * (float)AnT.Width / (float)AnT.Height, 0.0, 30.0, -1, 1);
            }

            devX = (float)ScreenW / (float)AnT.Width;
            devY = (float)ScreenH / (float)AnT.Height;

            centerX = (float)ScreenW / 2;
            centerY = (float)ScreenH / 2;

            GL.MatrixMode(MatrixMode.Modelview);

            PointInGraph.Start();
        }

        private void PointInGraph_Tick(object sender, EventArgs e)
        {
            Draw();
        }

        private void Draw()
        {
            GL.ClearColor(Color.White);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.LoadIdentity();
            GL.Color3(Color.Black);

            DrawClockFace();
            DrawClockHands();

            AnT.SwapBuffers();
        }

        private void DrawClockFace()
        {
            GL.PushMatrix();
            GL.Translate(centerX, centerY, 0);

            // Внешний круг
            GL.Begin(PrimitiveType.LineLoop);
            for (int i = 0; i < 360; i++)
            {
                double angle = MathHelper.DegreesToRadians(i);
                GL.Vertex2(Math.Cos(angle) * 11, Math.Sin(angle) * 11);
            }
            GL.End();

            // Основной круг
            GL.Begin(PrimitiveType.LineLoop);
            for (int i = 0; i < 360; i++)
            {
                double angle = MathHelper.DegreesToRadians(i);
                GL.Vertex2(Math.Cos(angle) * 10, Math.Sin(angle) * 10);
            }
            GL.End();

            // Часовые метки
            for (int i = 0; i < 12; i++)
            {
                double angle = MathHelper.DegreesToRadians(i * 30);
                float x1 = (float)Math.Cos(angle) * 9;
                float y1 = (float)Math.Sin(angle) * 9;
                float x2 = (float)Math.Cos(angle) * 10;
                float y2 = (float)Math.Sin(angle) * 10;

                GL.Begin(PrimitiveType.Lines);
                GL.Vertex2(x1, y1);
                GL.Vertex2(x2, y2);
                GL.End();
            }
            GL.PopMatrix();
        }

        private void DrawClockHands()
        {
            DateTime now = DateTime.Now;

            float hourAngle = 360.0f * (now.Hour % 12) / 12.0f + 30.0f * (now.Minute / 60.0f);
            float minuteAngle = 360.0f * now.Minute / 60.0f;
            float secondAngle = 360.0f * now.Second / 60.0f;

            GL.Color3(Color.Black);
            GL.LineWidth(2.0f);
            DrawHand(hourAngle, 6, 0.5f);

            GL.LineWidth(1.0f);
            DrawHand(minuteAngle, 8, 0.3f);

            GL.Color3(Color.Red);
            DrawHand(secondAngle, 9, 0.1f);
        }

        private void DrawHand(float angle, float length, float width)
        {
            GL.PushMatrix();
            GL.Translate(centerX, centerY, 0);
            GL.Rotate(angle, 0, 0, -1);

            GL.Begin(PrimitiveType.Lines);
            GL.Vertex2(0, 0);
            GL.Vertex2(0, length);
            GL.End();

            GL.PopMatrix();
        }

        
    }
}
