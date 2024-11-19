namespace Lab_7
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            AnT = new OpenTK.GLControl.GLControl();
            PointInGraph = new System.Windows.Forms.Timer(components);
            SuspendLayout();
            // 
            // AnT
            // 
            AnT.API = OpenTK.Windowing.Common.ContextAPI.OpenGL;
            AnT.APIVersion = new Version(3, 3, 0, 0);
            AnT.Flags = OpenTK.Windowing.Common.ContextFlags.Default;
            AnT.IsEventDriven = true;
            AnT.Location = new Point(0, -5);
            AnT.Margin = new Padding(4, 5, 4, 5);
            AnT.Name = "AnT";
            AnT.Profile = OpenTK.Windowing.Common.ContextProfile.Compatability;
            AnT.SharedContext = null;
            AnT.Size = new Size(1126, 760);
            AnT.TabIndex = 0;
            // 
            // PointInGraph
            // 
            PointInGraph.Enabled = true;
            PointInGraph.Interval = 1000;
            PointInGraph.Tick += PointInGraph_Tick;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1143, 750);
            Controls.Add(AnT);
            Margin = new Padding(4, 5, 4, 5);
            Name = "Form1";
            Text = "Form1";
            ResumeLayout(false);
        }

        #endregion

        private OpenTK.GLControl.GLControl AnT;
        private System.Windows.Forms.Timer PointInGraph;
    }
}
