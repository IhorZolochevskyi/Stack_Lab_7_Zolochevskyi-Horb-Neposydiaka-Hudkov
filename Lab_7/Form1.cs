using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace Lab_7
{
    public partial class Form1 : Form
    {
        private double ScreenW, ScreenH;
        private float devX, devY, centerX, centerY;
        private PerformanceCounter cpuCounter;
        private float[] cpuLoadHistory;
        private const int historyLength = 60; // Довжина історії графіка (60 секунд)
        private Font labelFont;

        public Form1()
        {
            InitializeComponent();
            cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
            cpuLoadHistory = new float[historyLength];
            labelFont = new Font("Arial", 12, FontStyle.Regular); // Створюємо більший шрифт
            Load += Form1_Load;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            GL.ClearColor(Color.White);
            GL.Enable(EnableCap.Blend); // Вмикаємо прозорість
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

            GL.Viewport(0, 0, AnT.Width, AnT.Height);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();

            ScreenW = 40.0; // Збільшуємо розмір віртуального екрану
            ScreenH = 30.0;
            GL.Ortho(-ScreenW / 2, ScreenW / 2, -ScreenH / 2, ScreenH / 2, -1, 1);

            devX = (float)ScreenW / AnT.Width;
            devY = (float)ScreenH / AnT.Height;
            centerX = 0;
            centerY = 0;

            GL.MatrixMode(MatrixMode.Modelview);

            PointInGraph.Interval = 1000;
            PointInGraph.Start();
        }

        private void DrawAxes()
        {
            GL.LineWidth(2); // Товщі лінії для осей

            // Ось X
            GL.Color3(Color.Black);
            GL.Begin(PrimitiveType.Lines);
            GL.Vertex2(-15, -10);
            GL.Vertex2(15, -10);

            // Стрілка для осі X
            GL.Vertex2(14.5f, -9.5f);
            GL.Vertex2(15, -10);
            GL.Vertex2(14.5f, -10.5f);
            GL.Vertex2(15, -10);
            GL.End();

            // Ось Y
            GL.Begin(PrimitiveType.Lines);
            GL.Vertex2(-15, -10);
            GL.Vertex2(-15, 10);

            // Стрілка для осі Y
            GL.Vertex2(-15.5f, 9.5f);
            GL.Vertex2(-15, 10);
            GL.Vertex2(-14.5f, 9.5f);
            GL.Vertex2(-15, 10);
            GL.End();

            // Позначки на осі Y (проценти)
            for (int i = 0; i <= 100; i += 20)
            {
                float y = -10 + (i / 100.0f) * 20;
                GL.Begin(PrimitiveType.Lines);
                GL.Vertex2(-15.3f, y);
                GL.Vertex2(-14.7f, y);
                GL.End();
                DrawText($"{i}%", -18, y - 2f, false);
            }

            // Позначки на осі X (час)
            for (int i = 0; i <= 60; i += 10)
            {
                float x = -15 + (i / 60.0f) * 30;
                GL.Begin(PrimitiveType.Lines);
                GL.Vertex2(x, -10.3f);
                GL.Vertex2(x, -9.7f);
                GL.End();
                DrawText($"{i}с", x - 14, -12.8f, true);
            }

            DrawGrid();
        }

        private void DrawGrid()
        {
            GL.LineWidth(1);
            GL.Color4(0.8f, 0.8f, 0.8f, 0.5f); // Світло-сірий колір з прозорістю

            // Горизонтальні лінії (кожні 10%)
            for (int i = 0; i <= 100; i += 10)
            {
                float y = -10 + (i / 100.0f) * 20;
                GL.Begin(PrimitiveType.Lines);
                GL.Vertex2(-15, y);
                GL.Vertex2(15, y);
                GL.End();
            }

            // Вертикальні лінії (кожні 5 секунд)
            for (int i = 0; i <= 60; i += 5)
            {
                float x = -15 + (i / 60.0f) * 30;
                GL.Begin(PrimitiveType.Lines);
                GL.Vertex2(x, -10);
                GL.Vertex2(x, 10);
                GL.End();
            }
        }

        private void DrawCpuLoadGraph()
        {
            GL.PushMatrix();
            GL.Translate(centerX, centerY, 0);

            // Градієнтна заливка під графіком
            GL.Begin(PrimitiveType.Polygon);
            for (int i = 0; i < historyLength; i++)
            {
                float x = -15 + (30.0f / historyLength) * i;
                float y = -10 + (cpuLoadHistory[i] / 100.0f) * 20;

                // Градієнт кольору залежно від навантаження
                float load = cpuLoadHistory[i] / 100.0f;
                GL.Color4(1.0f, 1.0f - load, 1.0f - load, 0.3f);
                GL.Vertex2(x, y);
                GL.Vertex2(x, -10);
            }
            GL.End();

            // Лінія графіку
            GL.LineWidth(3);
            GL.Begin(PrimitiveType.LineStrip);
            for (int i = 0; i < historyLength; i++)
            {
                float x = -15 + (30.0f / historyLength) * i;
                float y = -10 + (cpuLoadHistory[i] / 100.0f) * 20;
                float load = cpuLoadHistory[i] / 100.0f;
                GL.Color3(1.0f, 0.0f, 0.0f); // Червона лінія
                GL.Vertex2(x, y);
            }
            GL.End();

            // Точки даних
            GL.PointSize(6);
            GL.Begin(PrimitiveType.Points);
            for (int i = 0; i < historyLength; i++)
            {
                if (i % 5 == 0) // Малюємо точки кожні 5 секунд
                {
                    float x = -15 + (30.0f / historyLength) * i;
                    float y = -10 + (cpuLoadHistory[i] / 100.0f) * 20;
                    GL.Color3(0.0f, 0.0f, 0.0f); // Чорні точки
                    GL.Vertex2(x, y);
                }
            }
            GL.End();

            GL.PopMatrix();
        }

        private void DrawDescriptions()
        {
            // Заголовок графіку
            DrawText("Моніторинг завантаження процесора", -15, 11, true);

            // Підписи осей
            DrawText("Час (секунди)", -20, -14, true);
            DrawText("Завантаження CPU (%)", -19, 9.5f, false);

            // Поточне значення CPU
            float currentCpuLoad = cpuLoadHistory[historyLength - 1];
            DrawText($"Поточне завантаження: {currentCpuLoad:F1}%", -3, 8.5f, false);
        }

        private void DrawText(string text, float x, float y, bool centered)
        {
            using (Bitmap bmp = new Bitmap(800, 100))
            using (Graphics gfx = Graphics.FromImage(bmp))
            {
                gfx.Clear(Color.Transparent);
                gfx.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;

                SizeF textSize = gfx.MeasureString(text, labelFont);
                float offsetX = centered ? (bmp.Width - textSize.Width) / 2 : 0;
                float offsetY = (bmp.Height - textSize.Height) / 2;

                gfx.DrawString(text, labelFont, Brushes.Black, new PointF(offsetX, offsetY));

                // Відображення зображення по вертикалі
                bmp.RotateFlip(RotateFlipType.RotateNoneFlipY);

                BitmapData data = bmp.LockBits(
                    new Rectangle(0, 0, bmp.Width, bmp.Height),
                    ImageLockMode.ReadOnly,
                    System.Drawing.Imaging.PixelFormat.Format32bppArgb);

                GL.RasterPos2(x, y);
                GL.DrawPixels(bmp.Width, bmp.Height, OpenTK.Graphics.OpenGL.PixelFormat.Bgra,
                              PixelType.UnsignedByte, data.Scan0);

                bmp.UnlockBits(data);
            }
        }

        private void Draw()
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.LoadIdentity();

            DrawAxes();
            DrawCpuLoadGraph();
            DrawDescriptions();

            AnT.SwapBuffers();
        }

        private void UpdateCpuLoadHistory()
        {
            for (int i = 1; i < historyLength; i++)
            {
                cpuLoadHistory[i - 1] = cpuLoadHistory[i];
            }
            cpuLoadHistory[historyLength - 1] = cpuCounter.NextValue();
        }

        private void PointInGraph_Tick(object sender, EventArgs e)
        {
            UpdateCpuLoadHistory();
            Draw();
        }
    }
}