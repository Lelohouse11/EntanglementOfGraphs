using System;
using System.Windows.Forms;
namespace EntanglementOfGraphs
{
    partial class Form1 : Form
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
            GraphPicture = new PictureBox();
            ((System.ComponentModel.ISupportInitialize)GraphPicture).BeginInit();
            SuspendLayout();
            // 
            // GraphPicture
            // 
            GraphPicture.Location = new Point(83, 46);
            GraphPicture.Name = "GraphPicture";
            GraphPicture.Size = new Size(634, 363);
            GraphPicture.TabIndex = 0;
            GraphPicture.TabStop = false;
            GraphPicture.Paint += GraphPicture_Paint;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(GraphPicture);
            Name = "Form1";
            Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)GraphPicture).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private PictureBox GraphPicture;
    }
}
