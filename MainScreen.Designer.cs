using System;
using System.Windows.Forms;
namespace EntanglementOfGraphs
{
    partial class MainScreen : Form
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
            vertexInput = new TextBox();
            headline = new Label();
            input = new Label();
            newVertex = new Button();
            entanglement = new Button();
            TextOutput = new TextBox();
            vertex = new Label();
            edgeFrom = new Label();
            edgeTo = new Label();
            edgeSource = new TextBox();
            edgeTarget = new TextBox();
            addEdge = new Button();
            deleteGraph = new Button();
            startPos = new Label();
            startPosInput = new TextBox();
            torusN = new TextBox();
            torusM = new TextBox();
            x = new Label();
            torusGraph = new Label();
            createTorusGraph = new Button();
            ((System.ComponentModel.ISupportInitialize)GraphPicture).BeginInit();
            SuspendLayout();
            // 
            // GraphPicture
            // 
            GraphPicture.Location = new Point(12, 37);
            GraphPicture.Name = "GraphPicture";
            GraphPicture.Size = new Size(634, 363);
            GraphPicture.TabIndex = 0;
            GraphPicture.TabStop = false;
            GraphPicture.Paint += GraphPicture_Paint;
            // 
            // vertexInput
            // 
            vertexInput.Font = new Font("Segoe UI", 12F);
            vertexInput.Location = new Point(99, 427);
            vertexInput.Name = "vertexInput";
            vertexInput.Size = new Size(245, 29);
            vertexInput.TabIndex = 1;
            // 
            // headline
            // 
            headline.AutoSize = true;
            headline.Font = new Font("Segoe UI", 14F);
            headline.Location = new Point(12, 9);
            headline.Name = "headline";
            headline.Size = new Size(460, 25);
            headline.TabIndex = 2;
            headline.Text = "Tool zum Berechnen vom Entanglement von Graphen";
            // 
            // input
            // 
            input.AutoSize = true;
            input.Font = new Font("Segoe UI", 12F);
            input.Location = new Point(12, 403);
            input.Name = "input";
            input.Size = new Size(119, 21);
            input.TabIndex = 3;
            input.Text = "Graph erstellen:";
            // 
            // newVertex
            // 
            newVertex.Font = new Font("Segoe UI", 12F);
            newVertex.Location = new Point(350, 427);
            newVertex.Name = "newVertex";
            newVertex.Size = new Size(103, 31);
            newVertex.TabIndex = 4;
            newVertex.Text = "Hinzufügen";
            newVertex.UseVisualStyleBackColor = true;
            newVertex.Click += newVertex_Click;
            // 
            // entanglement
            // 
            entanglement.Font = new Font("Segoe UI", 12F);
            entanglement.Location = new Point(652, 121);
            entanglement.Name = "entanglement";
            entanglement.Size = new Size(231, 31);
            entanglement.TabIndex = 5;
            entanglement.Text = "Entanglement berechnen";
            entanglement.UseVisualStyleBackColor = true;
            entanglement.Click += entanglement_Click;
            // 
            // TextOutput
            // 
            TextOutput.Font = new Font("Segoe UI", 12F);
            TextOutput.Location = new Point(652, 403);
            TextOutput.Multiline = true;
            TextOutput.Name = "TextOutput";
            TextOutput.ReadOnly = true;
            TextOutput.Size = new Size(231, 113);
            TextOutput.TabIndex = 6;
            // 
            // vertex
            // 
            vertex.AutoSize = true;
            vertex.Font = new Font("Segoe UI", 12F);
            vertex.Location = new Point(12, 429);
            vertex.Name = "vertex";
            vertex.Size = new Size(62, 21);
            vertex.TabIndex = 7;
            vertex.Text = "Knoten:";
            // 
            // edgeFrom
            // 
            edgeFrom.AutoSize = true;
            edgeFrom.Font = new Font("Segoe UI", 12F);
            edgeFrom.Location = new Point(12, 465);
            edgeFrom.Name = "edgeFrom";
            edgeFrom.Size = new Size(82, 21);
            edgeFrom.TabIndex = 8;
            edgeFrom.Text = "Kante von:";
            // 
            // edgeTo
            // 
            edgeTo.AutoSize = true;
            edgeTo.Font = new Font("Segoe UI", 12F);
            edgeTo.Location = new Point(206, 465);
            edgeTo.Name = "edgeTo";
            edgeTo.Size = new Size(29, 21);
            edgeTo.TabIndex = 9;
            edgeTo.Text = "zu:";
            // 
            // edgeSource
            // 
            edgeSource.Font = new Font("Segoe UI", 12F);
            edgeSource.Location = new Point(100, 462);
            edgeSource.Name = "edgeSource";
            edgeSource.Size = new Size(100, 29);
            edgeSource.TabIndex = 10;
            // 
            // edgeTarget
            // 
            edgeTarget.Font = new Font("Segoe UI", 12F);
            edgeTarget.Location = new Point(241, 465);
            edgeTarget.Name = "edgeTarget";
            edgeTarget.Size = new Size(103, 29);
            edgeTarget.TabIndex = 11;
            // 
            // addEdge
            // 
            addEdge.Font = new Font("Segoe UI", 12F);
            addEdge.Location = new Point(350, 465);
            addEdge.Name = "addEdge";
            addEdge.Size = new Size(103, 31);
            addEdge.TabIndex = 12;
            addEdge.Text = "Hinzufügen";
            addEdge.UseVisualStyleBackColor = true;
            addEdge.Click += addEdge_Click;
            // 
            // deleteGraph
            // 
            deleteGraph.Font = new Font("Segoe UI", 12F);
            deleteGraph.Location = new Point(459, 429);
            deleteGraph.Name = "deleteGraph";
            deleteGraph.Size = new Size(103, 67);
            deleteGraph.TabIndex = 13;
            deleteGraph.Text = "Graph löschen";
            deleteGraph.UseVisualStyleBackColor = true;
            deleteGraph.Click += deleteGraph_Click;
            // 
            // startPos
            // 
            startPos.Font = new Font("Segoe UI", 12F);
            startPos.Location = new Point(652, 37);
            startPos.Name = "startPos";
            startPos.Size = new Size(231, 46);
            startPos.TabIndex = 14;
            startPos.Text = "Startposition des Diebes eingeben:";
            // 
            // startPosInput
            // 
            startPosInput.Font = new Font("Segoe UI", 12F);
            startPosInput.Location = new Point(652, 86);
            startPosInput.Name = "startPosInput";
            startPosInput.Size = new Size(231, 29);
            startPosInput.TabIndex = 15;
            // 
            // torusN
            // 
            torusN.Font = new Font("Segoe UI", 12F);
            torusN.Location = new Point(656, 223);
            torusN.Name = "torusN";
            torusN.Size = new Size(100, 29);
            torusN.TabIndex = 16;
            // 
            // torusM
            // 
            torusM.Font = new Font("Segoe UI", 12F);
            torusM.Location = new Point(783, 223);
            torusM.Name = "torusM";
            torusM.Size = new Size(100, 29);
            torusM.TabIndex = 17;
            // 
            // x
            // 
            x.AutoSize = true;
            x.Font = new Font("Segoe UI", 12F);
            x.Location = new Point(762, 226);
            x.Name = "x";
            x.Size = new Size(17, 21);
            x.TabIndex = 18;
            x.Text = "x";
            // 
            // torusGraph
            // 
            torusGraph.Font = new Font("Segoe UI", 12F);
            torusGraph.Location = new Point(652, 168);
            torusGraph.Name = "torusGraph";
            torusGraph.Size = new Size(231, 52);
            torusGraph.TabIndex = 19;
            torusGraph.Text = "Bitte Größe des Torusgraphen eingeben:";
            // 
            // createTorusGraph
            // 
            createTorusGraph.Font = new Font("Segoe UI", 12F);
            createTorusGraph.Location = new Point(656, 258);
            createTorusGraph.Name = "createTorusGraph";
            createTorusGraph.Size = new Size(227, 31);
            createTorusGraph.TabIndex = 20;
            createTorusGraph.Text = "Torusgraph erstellen";
            createTorusGraph.UseVisualStyleBackColor = true;
            createTorusGraph.Click += this.createTorusGraph_Click;
            // 
            // EntanglementVonGraphen
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(895, 528);
            Controls.Add(createTorusGraph);
            Controls.Add(torusGraph);
            Controls.Add(x);
            Controls.Add(torusM);
            Controls.Add(torusN);
            Controls.Add(startPosInput);
            Controls.Add(startPos);
            Controls.Add(deleteGraph);
            Controls.Add(addEdge);
            Controls.Add(edgeTarget);
            Controls.Add(edgeSource);
            Controls.Add(edgeTo);
            Controls.Add(edgeFrom);
            Controls.Add(vertex);
            Controls.Add(TextOutput);
            Controls.Add(entanglement);
            Controls.Add(newVertex);
            Controls.Add(input);
            Controls.Add(headline);
            Controls.Add(vertexInput);
            Controls.Add(GraphPicture);
            Name = "EntanglementVonGraphen";
            Text = "Entanglement ist ein Maß für die Komläxität eines Graphs";
            ((System.ComponentModel.ISupportInitialize)GraphPicture).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private PictureBox GraphPicture;
        private TextBox vertexInput;
        private Label headline;
        private Label input;
        private Button newVertex;
        private Button entanglement;
        private TextBox TextOutput;
        private Label vertex;
        private Label edgeFrom;
        private Label edgeTo;
        private TextBox edgeSource;
        private TextBox edgeTarget;
        private Button addEdge;
        private Button deleteGraph;
        private Label startPos;
        private TextBox startPosInput;
        private TextBox torusN;
        private TextBox torusM;
        private Label x;
        private Label torusGraph;
        private Button createTorusGraph;
    }
}
