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
            playGraph = new Button();
            graphCreate = new Panel();
            TorusCreate = new Panel();
            computeOrGame = new Panel();
            ((System.ComponentModel.ISupportInitialize)GraphPicture).BeginInit();
            graphCreate.SuspendLayout();
            TorusCreate.SuspendLayout();
            computeOrGame.SuspendLayout();
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
            vertexInput.BackColor = SystemColors.Desktop;
            vertexInput.Font = new Font("Segoe UI", 12F);
            vertexInput.ForeColor = SystemColors.ControlLight;
            vertexInput.Location = new Point(87, 24);
            vertexInput.Name = "vertexInput";
            vertexInput.Size = new Size(245, 29);
            vertexInput.TabIndex = 1;
            // 
            // headline
            // 
            headline.AutoSize = true;
            headline.Font = new Font("Segoe UI", 14F);
            headline.ForeColor = SystemColors.ControlLight;
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
            input.ForeColor = SystemColors.ControlLight;
            input.Location = new Point(0, 0);
            input.Name = "input";
            input.Size = new Size(119, 21);
            input.TabIndex = 3;
            input.Text = "Graph erstellen:";
            // 
            // newVertex
            // 
            newVertex.BackColor = SystemColors.Desktop;
            newVertex.Font = new Font("Segoe UI", 12F);
            newVertex.ForeColor = SystemColors.ControlLight;
            newVertex.Location = new Point(338, 24);
            newVertex.Name = "newVertex";
            newVertex.Size = new Size(103, 31);
            newVertex.TabIndex = 4;
            newVertex.Text = "Hinzufügen";
            newVertex.UseVisualStyleBackColor = false;
            newVertex.Click += newVertex_Click;
            // 
            // entanglement
            // 
            entanglement.BackColor = SystemColors.Desktop;
            entanglement.Font = new Font("Segoe UI", 12F);
            entanglement.ForeColor = SystemColors.ControlLight;
            entanglement.Location = new Point(0, 84);
            entanglement.Name = "entanglement";
            entanglement.Size = new Size(231, 31);
            entanglement.TabIndex = 5;
            entanglement.Text = "Entanglement berechnen";
            entanglement.UseVisualStyleBackColor = false;
            entanglement.Click += entanglement_Click;
            // 
            // TextOutput
            // 
            TextOutput.BackColor = SystemColors.Desktop;
            TextOutput.Font = new Font("Segoe UI", 12F);
            TextOutput.ForeColor = SystemColors.ControlLight;
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
            vertex.ForeColor = SystemColors.ControlLight;
            vertex.Location = new Point(0, 26);
            vertex.Name = "vertex";
            vertex.Size = new Size(62, 21);
            vertex.TabIndex = 7;
            vertex.Text = "Knoten:";
            // 
            // edgeFrom
            // 
            edgeFrom.AutoSize = true;
            edgeFrom.Font = new Font("Segoe UI", 12F);
            edgeFrom.ForeColor = SystemColors.ControlLight;
            edgeFrom.Location = new Point(0, 62);
            edgeFrom.Name = "edgeFrom";
            edgeFrom.Size = new Size(82, 21);
            edgeFrom.TabIndex = 8;
            edgeFrom.Text = "Kante von:";
            // 
            // edgeTo
            // 
            edgeTo.AutoSize = true;
            edgeTo.Font = new Font("Segoe UI", 12F);
            edgeTo.ForeColor = SystemColors.ControlLight;
            edgeTo.Location = new Point(194, 62);
            edgeTo.Name = "edgeTo";
            edgeTo.Size = new Size(29, 21);
            edgeTo.TabIndex = 9;
            edgeTo.Text = "zu:";
            // 
            // edgeSource
            // 
            edgeSource.BackColor = SystemColors.Desktop;
            edgeSource.Font = new Font("Segoe UI", 12F);
            edgeSource.ForeColor = SystemColors.ControlLight;
            edgeSource.Location = new Point(88, 59);
            edgeSource.Name = "edgeSource";
            edgeSource.Size = new Size(100, 29);
            edgeSource.TabIndex = 10;
            // 
            // edgeTarget
            // 
            edgeTarget.BackColor = SystemColors.Desktop;
            edgeTarget.Font = new Font("Segoe UI", 12F);
            edgeTarget.ForeColor = SystemColors.ControlLight;
            edgeTarget.Location = new Point(229, 62);
            edgeTarget.Name = "edgeTarget";
            edgeTarget.Size = new Size(103, 29);
            edgeTarget.TabIndex = 11;
            // 
            // addEdge
            // 
            addEdge.BackColor = SystemColors.Desktop;
            addEdge.Font = new Font("Segoe UI", 12F);
            addEdge.ForeColor = SystemColors.ControlLight;
            addEdge.Location = new Point(338, 62);
            addEdge.Name = "addEdge";
            addEdge.Size = new Size(103, 31);
            addEdge.TabIndex = 12;
            addEdge.Text = "Hinzufügen";
            addEdge.UseVisualStyleBackColor = false;
            addEdge.Click += addEdge_Click;
            // 
            // deleteGraph
            // 
            deleteGraph.BackColor = SystemColors.Desktop;
            deleteGraph.Font = new Font("Segoe UI", 12F);
            deleteGraph.ForeColor = SystemColors.GradientActiveCaption;
            deleteGraph.Location = new Point(447, 26);
            deleteGraph.Name = "deleteGraph";
            deleteGraph.Size = new Size(103, 67);
            deleteGraph.TabIndex = 13;
            deleteGraph.Text = "Graph löschen";
            deleteGraph.UseVisualStyleBackColor = false;
            deleteGraph.Click += deleteGraph_Click;
            // 
            // startPos
            // 
            startPos.Font = new Font("Segoe UI", 12F);
            startPos.ForeColor = SystemColors.ControlLight;
            startPos.Location = new Point(0, 0);
            startPos.Name = "startPos";
            startPos.Size = new Size(231, 46);
            startPos.TabIndex = 14;
            startPos.Text = "Startposition des Diebes eingeben:";
            // 
            // startPosInput
            // 
            startPosInput.BackColor = SystemColors.Desktop;
            startPosInput.Font = new Font("Segoe UI", 12F);
            startPosInput.ForeColor = SystemColors.ControlLight;
            startPosInput.Location = new Point(0, 49);
            startPosInput.Name = "startPosInput";
            startPosInput.Size = new Size(231, 29);
            startPosInput.TabIndex = 15;
            // 
            // torusN
            // 
            torusN.BackColor = SystemColors.Desktop;
            torusN.Font = new Font("Segoe UI", 12F);
            torusN.ForeColor = SystemColors.ControlLight;
            torusN.Location = new Point(4, 55);
            torusN.Name = "torusN";
            torusN.Size = new Size(100, 29);
            torusN.TabIndex = 16;
            // 
            // torusM
            // 
            torusM.BackColor = SystemColors.Desktop;
            torusM.Font = new Font("Segoe UI", 12F);
            torusM.ForeColor = SystemColors.ControlLight;
            torusM.Location = new Point(131, 55);
            torusM.Name = "torusM";
            torusM.Size = new Size(100, 29);
            torusM.TabIndex = 17;
            // 
            // x
            // 
            x.AutoSize = true;
            x.Font = new Font("Segoe UI", 12F);
            x.ForeColor = SystemColors.ControlLight;
            x.Location = new Point(110, 58);
            x.Name = "x";
            x.Size = new Size(17, 21);
            x.TabIndex = 18;
            x.Text = "x";
            // 
            // torusGraph
            // 
            torusGraph.Font = new Font("Segoe UI", 12F);
            torusGraph.ForeColor = SystemColors.ControlLight;
            torusGraph.Location = new Point(0, 0);
            torusGraph.Name = "torusGraph";
            torusGraph.Size = new Size(231, 52);
            torusGraph.TabIndex = 19;
            torusGraph.Text = "Bitte Größe des Torusgraphen eingeben:";
            // 
            // createTorusGraph
            // 
            createTorusGraph.BackColor = SystemColors.Desktop;
            createTorusGraph.Font = new Font("Segoe UI", 12F);
            createTorusGraph.ForeColor = SystemColors.ControlLight;
            createTorusGraph.Location = new Point(4, 90);
            createTorusGraph.Name = "createTorusGraph";
            createTorusGraph.Size = new Size(227, 31);
            createTorusGraph.TabIndex = 20;
            createTorusGraph.Text = "Torusgraph erstellen";
            createTorusGraph.UseVisualStyleBackColor = false;
            createTorusGraph.Click += createTorusGraph_Click;
            // 
            // playGraph
            // 
            playGraph.BackColor = SystemColors.Desktop;
            playGraph.Font = new Font("Segoe UI", 12F);
            playGraph.ForeColor = SystemColors.ControlLight;
            playGraph.Location = new Point(0, 121);
            playGraph.Name = "playGraph";
            playGraph.Size = new Size(231, 31);
            playGraph.TabIndex = 21;
            playGraph.Text = "Auf Graph spielen";
            playGraph.UseVisualStyleBackColor = false;
            playGraph.Click += playGraph_Click;
            // 
            // graphCreate
            // 
            graphCreate.Controls.Add(input);
            graphCreate.Controls.Add(vertexInput);
            graphCreate.Controls.Add(newVertex);
            graphCreate.Controls.Add(vertex);
            graphCreate.Controls.Add(edgeFrom);
            graphCreate.Controls.Add(edgeTo);
            graphCreate.Controls.Add(edgeSource);
            graphCreate.Controls.Add(edgeTarget);
            graphCreate.Controls.Add(addEdge);
            graphCreate.Controls.Add(deleteGraph);
            graphCreate.Location = new Point(12, 406);
            graphCreate.Name = "graphCreate";
            graphCreate.Size = new Size(556, 101);
            graphCreate.TabIndex = 22;
            // 
            // TorusCreate
            // 
            TorusCreate.Controls.Add(torusGraph);
            TorusCreate.Controls.Add(torusN);
            TorusCreate.Controls.Add(torusM);
            TorusCreate.Controls.Add(createTorusGraph);
            TorusCreate.Controls.Add(x);
            TorusCreate.Location = new Point(652, 195);
            TorusCreate.Name = "TorusCreate";
            TorusCreate.Size = new Size(237, 151);
            TorusCreate.TabIndex = 23;
            // 
            // computeOrGame
            // 
            computeOrGame.Controls.Add(startPos);
            computeOrGame.Controls.Add(entanglement);
            computeOrGame.Controls.Add(startPosInput);
            computeOrGame.Controls.Add(playGraph);
            computeOrGame.Location = new Point(652, 37);
            computeOrGame.Name = "computeOrGame";
            computeOrGame.Size = new Size(231, 149);
            computeOrGame.TabIndex = 24;
            // 
            // MainScreen
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.Desktop;
            ClientSize = new Size(895, 528);
            Controls.Add(computeOrGame);
            Controls.Add(TorusCreate);
            Controls.Add(graphCreate);
            Controls.Add(TextOutput);
            Controls.Add(headline);
            Controls.Add(GraphPicture);
            Name = "MainScreen";
            Text = "Entanglement ist ein Maß für die Komläxität eines Graphs";
            ((System.ComponentModel.ISupportInitialize)GraphPicture).EndInit();
            graphCreate.ResumeLayout(false);
            graphCreate.PerformLayout();
            TorusCreate.ResumeLayout(false);
            TorusCreate.PerformLayout();
            computeOrGame.ResumeLayout(false);
            computeOrGame.PerformLayout();
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
        private Button playGraph;
        private Panel graphCreate;
        private Panel TorusCreate;
        private Panel computeOrGame;
    }
}
