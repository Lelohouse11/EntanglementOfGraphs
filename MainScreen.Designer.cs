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
            playGraph = new Button();
            graphCreate = new Panel();
            doNothingDet = new Button();
            moveDetective = new Button();
            movedDet = new TextBox();
            detective = new Label();
            chooseDetPlay = new Label();
            computeEnt = new Panel();
            startPos2 = new Label();
            GameSettings = new Panel();
            playDetective = new Button();
            playThief = new Button();
            detectiveAmountInput = new TextBox();
            detectiveAmount = new Label();
            startPosInput2 = new TextBox();
            restartGame = new Button();
            editGraph = new Button();
            detMovement = new Panel();
            label1 = new Label();
            thiefMovement = new Panel();
            label2 = new Label();
            moveThiefToTarget = new Button();
            targetThiefInput = new TextBox();
            targetVertexofThief = new Label();
            chooseThiefPlay = new Label();
            TorusCreate = new Panel();
            torusGraph = new Label();
            torusN = new TextBox();
            torusM = new TextBox();
            createTorusGraph = new Button();
            x = new Label();
            chooseGraphTyp = new ComboBox();
            createUnCircleGraph = new Panel();
            unCircleSize = new Label();
            createUndirectedCircleGraph = new Button();
            unCircleSizeInput = new TextBox();
            createDiCircleGraph = new Panel();
            diCircleLabel = new Label();
            createDirectedCircleGraph = new Button();
            diCircleSizeInput = new TextBox();
            openFileDialog1 = new OpenFileDialog();
            createFullyConGraphPanel = new Panel();
            fullyConCreate = new Button();
            fullyConSizeInput = new TextBox();
            fullyConSize = new Label();
            createUnaryFunc = new Panel();
            unaryFuncDomainLabel = new Label();
            lunaryFuncToLabel = new Label();
            unaryFuncStartDomImput = new TextBox();
            unaryFuncEndDomImput = new TextBox();
            unaryFuncCreate = new Button();
            unaryFuncInput = new TextBox();
            unaryFuncLabel = new Label();
            ((System.ComponentModel.ISupportInitialize)GraphPicture).BeginInit();
            graphCreate.SuspendLayout();
            computeEnt.SuspendLayout();
            GameSettings.SuspendLayout();
            detMovement.SuspendLayout();
            thiefMovement.SuspendLayout();
            TorusCreate.SuspendLayout();
            createUnCircleGraph.SuspendLayout();
            createDiCircleGraph.SuspendLayout();
            createFullyConGraphPanel.SuspendLayout();
            createUnaryFunc.SuspendLayout();
            SuspendLayout();
            // 
            // GraphPicture
            // 
            GraphPicture.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            GraphPicture.BorderStyle = BorderStyle.FixedSingle;
            GraphPicture.Location = new Point(12, 37);
            GraphPicture.Name = "GraphPicture";
            GraphPicture.Size = new Size(634, 379);
            GraphPicture.SizeMode = PictureBoxSizeMode.Zoom;
            GraphPicture.TabIndex = 0;
            GraphPicture.TabStop = false;
            GraphPicture.Paint += GraphPicture_Paint;
            // 
            // vertexInput
            // 
            vertexInput.BackColor = SystemColors.Window;
            vertexInput.Font = new Font("Segoe UI", 12F);
            vertexInput.ForeColor = SystemColors.WindowText;
            vertexInput.Location = new Point(87, 24);
            vertexInput.Name = "vertexInput";
            vertexInput.Size = new Size(245, 29);
            vertexInput.TabIndex = 1;
            // 
            // headline
            // 
            headline.AutoSize = true;
            headline.Font = new Font("Segoe UI", 14F);
            headline.ForeColor = SystemColors.WindowText;
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
            input.ForeColor = SystemColors.WindowText;
            input.Location = new Point(0, 0);
            input.Name = "input";
            input.Size = new Size(119, 21);
            input.TabIndex = 3;
            input.Text = "Graph erstellen:";
            // 
            // newVertex
            // 
            newVertex.BackColor = SystemColors.ButtonHighlight;
            newVertex.Font = new Font("Segoe UI", 12F);
            newVertex.ForeColor = SystemColors.ControlText;
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
            entanglement.BackColor = SystemColors.ButtonHighlight;
            entanglement.Font = new Font("Segoe UI", 12F);
            entanglement.ForeColor = SystemColors.ControlText;
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
            TextOutput.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            TextOutput.BackColor = SystemColors.Window;
            TextOutput.Font = new Font("Segoe UI", 12F);
            TextOutput.ForeColor = SystemColors.WindowText;
            TextOutput.Location = new Point(652, 419);
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
            vertex.ForeColor = SystemColors.WindowText;
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
            edgeFrom.ForeColor = SystemColors.WindowText;
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
            edgeTo.ForeColor = SystemColors.WindowText;
            edgeTo.Location = new Point(194, 62);
            edgeTo.Name = "edgeTo";
            edgeTo.Size = new Size(29, 21);
            edgeTo.TabIndex = 9;
            edgeTo.Text = "zu:";
            // 
            // edgeSource
            // 
            edgeSource.BackColor = SystemColors.Window;
            edgeSource.Font = new Font("Segoe UI", 12F);
            edgeSource.ForeColor = SystemColors.WindowText;
            edgeSource.Location = new Point(88, 59);
            edgeSource.Name = "edgeSource";
            edgeSource.Size = new Size(100, 29);
            edgeSource.TabIndex = 10;
            // 
            // edgeTarget
            // 
            edgeTarget.BackColor = SystemColors.Window;
            edgeTarget.Font = new Font("Segoe UI", 12F);
            edgeTarget.ForeColor = SystemColors.WindowText;
            edgeTarget.Location = new Point(229, 59);
            edgeTarget.Name = "edgeTarget";
            edgeTarget.Size = new Size(103, 29);
            edgeTarget.TabIndex = 11;
            // 
            // addEdge
            // 
            addEdge.BackColor = SystemColors.ButtonHighlight;
            addEdge.Font = new Font("Segoe UI", 12F);
            addEdge.ForeColor = SystemColors.ControlText;
            addEdge.Location = new Point(338, 57);
            addEdge.Name = "addEdge";
            addEdge.Size = new Size(103, 31);
            addEdge.TabIndex = 12;
            addEdge.Text = "Hinzufügen";
            addEdge.UseVisualStyleBackColor = false;
            addEdge.Click += addEdge_Click;
            // 
            // deleteGraph
            // 
            deleteGraph.BackColor = SystemColors.ButtonHighlight;
            deleteGraph.Font = new Font("Segoe UI", 12F);
            deleteGraph.ForeColor = SystemColors.ControlText;
            deleteGraph.Location = new Point(447, 24);
            deleteGraph.Name = "deleteGraph";
            deleteGraph.Size = new Size(103, 64);
            deleteGraph.TabIndex = 13;
            deleteGraph.Text = "Graph löschen";
            deleteGraph.UseVisualStyleBackColor = false;
            deleteGraph.Click += deleteGraph_Click;
            // 
            // startState
            // 
            startPos.Font = new Font("Segoe UI", 12F);
            startPos.ForeColor = SystemColors.WindowText;
            startPos.Location = new Point(0, 0);
            startPos.Name = "startPos";
            startPos.Size = new Size(231, 46);
            startPos.TabIndex = 14;
            startPos.Text = "Startposition des Diebes eingeben:";
            // 
            // startPosInput
            // 
            startPosInput.BackColor = SystemColors.Window;
            startPosInput.Font = new Font("Segoe UI", 12F);
            startPosInput.ForeColor = SystemColors.WindowText;
            startPosInput.Location = new Point(0, 49);
            startPosInput.Name = "startPosInput";
            startPosInput.Size = new Size(231, 29);
            startPosInput.TabIndex = 15;
            // 
            // playGraph
            // 
            playGraph.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            playGraph.BackColor = SystemColors.ButtonHighlight;
            playGraph.Font = new Font("Segoe UI", 12F);
            playGraph.ForeColor = SystemColors.ControlText;
            playGraph.Location = new Point(656, 3);
            playGraph.Name = "playGraph";
            playGraph.Size = new Size(231, 31);
            playGraph.TabIndex = 21;
            playGraph.Text = "Auf Graph spielen";
            playGraph.UseVisualStyleBackColor = false;
            playGraph.Click += playGraph_Click;
            // 
            // graphCreate
            // 
            graphCreate.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
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
            graphCreate.Location = new Point(12, 422);
            graphCreate.Name = "graphCreate";
            graphCreate.Size = new Size(638, 101);
            graphCreate.TabIndex = 22;
            // 
            // doNothingDet
            // 
            doNothingDet.BackColor = SystemColors.ButtonHighlight;
            doNothingDet.Font = new Font("Segoe UI", 12F);
            doNothingDet.ForeColor = SystemColors.ControlText;
            doNothingDet.Location = new Point(3, 59);
            doNothingDet.Name = "doNothingDet";
            doNothingDet.Size = new Size(340, 31);
            doNothingDet.TabIndex = 14;
            doNothingDet.Text = "Keinen Detektiv bewegen";
            doNothingDet.UseVisualStyleBackColor = false;
            doNothingDet.Click += doNothingDet_Click;
            // 
            // moveDetective
            // 
            moveDetective.BackColor = SystemColors.ButtonHighlight;
            moveDetective.Font = new Font("Segoe UI", 12F);
            moveDetective.ForeColor = SystemColors.ControlText;
            moveDetective.Location = new Point(193, 22);
            moveDetective.Name = "moveDetective";
            moveDetective.Size = new Size(150, 31);
            moveDetective.TabIndex = 14;
            moveDetective.Text = "Auf Dieb bewegen";
            moveDetective.UseVisualStyleBackColor = false;
            moveDetective.Click += moveDetective_Click;
            // 
            // movedDet
            // 
            movedDet.BackColor = SystemColors.Window;
            movedDet.Font = new Font("Segoe UI", 12F);
            movedDet.ForeColor = SystemColors.WindowText;
            movedDet.Location = new Point(87, 24);
            movedDet.Name = "movedDet";
            movedDet.Size = new Size(100, 29);
            movedDet.TabIndex = 14;
            // 
            // detective
            // 
            detective.AutoSize = true;
            detective.Font = new Font("Segoe UI", 12F);
            detective.ForeColor = SystemColors.WindowText;
            detective.Location = new Point(3, 27);
            detective.Name = "detective";
            detective.Size = new Size(78, 21);
            detective.TabIndex = 14;
            detective.Text = "Detektive:";
            // 
            // chooseDetPlay
            // 
            chooseDetPlay.AutoSize = true;
            chooseDetPlay.Font = new Font("Segoe UI", 12F);
            chooseDetPlay.ForeColor = SystemColors.WindowText;
            chooseDetPlay.Location = new Point(0, 0);
            chooseDetPlay.Name = "chooseDetPlay";
            chooseDetPlay.Size = new Size(138, 21);
            chooseDetPlay.TabIndex = 4;
            chooseDetPlay.Text = "Wähle deinen Zug:";
            // 
            // computeEnt
            // 
            computeEnt.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            computeEnt.Controls.Add(startPos);
            computeEnt.Controls.Add(entanglement);
            computeEnt.Controls.Add(startPosInput);
            computeEnt.Location = new Point(652, 37);
            computeEnt.Name = "computeEnt";
            computeEnt.Size = new Size(237, 118);
            computeEnt.TabIndex = 24;
            // 
            // startPos2
            // 
            startPos2.Font = new Font("Segoe UI", 12F);
            startPos2.ForeColor = SystemColors.WindowText;
            startPos2.Location = new Point(0, 0);
            startPos2.Name = "startPos2";
            startPos2.Size = new Size(231, 46);
            startPos2.TabIndex = 15;
            startPos2.Text = "Startposition des Diebes eingeben:";
            // 
            // GameSettings
            // 
            GameSettings.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            GameSettings.Controls.Add(playDetective);
            GameSettings.Controls.Add(playThief);
            GameSettings.Controls.Add(detectiveAmountInput);
            GameSettings.Controls.Add(detectiveAmount);
            GameSettings.Controls.Add(startPosInput2);
            GameSettings.Controls.Add(startPos2);
            GameSettings.Location = new Point(652, 37);
            GameSettings.Name = "GameSettings";
            GameSettings.Size = new Size(237, 326);
            GameSettings.TabIndex = 25;
            GameSettings.Visible = false;
            // 
            // playDetective
            // 
            playDetective.BackColor = SystemColors.ButtonHighlight;
            playDetective.Font = new Font("Segoe UI", 12F);
            playDetective.ForeColor = SystemColors.ControlText;
            playDetective.Location = new Point(3, 185);
            playDetective.Name = "playDetective";
            playDetective.Size = new Size(231, 31);
            playDetective.TabIndex = 20;
            playDetective.Text = "Detektive spielen";
            playDetective.UseVisualStyleBackColor = false;
            playDetective.Click += playDetective_Click;
            // 
            // playThief
            // 
            playThief.BackColor = SystemColors.ButtonHighlight;
            playThief.Font = new Font("Segoe UI", 12F);
            playThief.ForeColor = SystemColors.ControlText;
            playThief.Location = new Point(3, 148);
            playThief.Name = "playThief";
            playThief.Size = new Size(231, 31);
            playThief.TabIndex = 19;
            playThief.Text = "Dieb spielen";
            playThief.UseVisualStyleBackColor = false;
            playThief.Click += playThief_Click;
            // 
            // detectiveAmountInput
            // 
            detectiveAmountInput.BackColor = SystemColors.Window;
            detectiveAmountInput.Font = new Font("Segoe UI", 12F);
            detectiveAmountInput.ForeColor = SystemColors.WindowText;
            detectiveAmountInput.Location = new Point(0, 114);
            detectiveAmountInput.Name = "detectiveAmountInput";
            detectiveAmountInput.Size = new Size(231, 29);
            detectiveAmountInput.TabIndex = 18;
            // 
            // detectiveAmount
            // 
            detectiveAmount.Font = new Font("Segoe UI", 12F);
            detectiveAmount.ForeColor = SystemColors.WindowText;
            detectiveAmount.Location = new Point(3, 81);
            detectiveAmount.Name = "detectiveAmount";
            detectiveAmount.Size = new Size(231, 30);
            detectiveAmount.TabIndex = 17;
            detectiveAmount.Text = "Anzahl der Diebe eingeben:";
            // 
            // startPosInput2
            // 
            startPosInput2.BackColor = SystemColors.Window;
            startPosInput2.Font = new Font("Segoe UI", 12F);
            startPosInput2.ForeColor = SystemColors.WindowText;
            startPosInput2.Location = new Point(3, 49);
            startPosInput2.Name = "startPosInput2";
            startPosInput2.Size = new Size(231, 29);
            startPosInput2.TabIndex = 16;
            // 
            // restartGame
            // 
            restartGame.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            restartGame.Font = new Font("Segoe UI", 12F);
            restartGame.Location = new Point(652, 366);
            restartGame.Name = "restartGame";
            restartGame.Size = new Size(231, 31);
            restartGame.TabIndex = 29;
            restartGame.Text = "Spiel neustarten";
            restartGame.UseVisualStyleBackColor = true;
            restartGame.Visible = false;
            restartGame.Click += restartGame_Click_1;
            // 
            // editGraph
            // 
            editGraph.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            editGraph.BackColor = SystemColors.ButtonHighlight;
            editGraph.Font = new Font("Segoe UI", 12F);
            editGraph.ForeColor = SystemColors.ControlText;
            editGraph.Location = new Point(658, 3);
            editGraph.Name = "editGraph";
            editGraph.Size = new Size(231, 31);
            editGraph.TabIndex = 26;
            editGraph.Text = "Graph bearbeiten";
            editGraph.UseVisualStyleBackColor = false;
            editGraph.Visible = false;
            editGraph.Click += editGraph_Click;
            // 
            // detMovement
            // 
            detMovement.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            detMovement.Controls.Add(label1);
            detMovement.Controls.Add(doNothingDet);
            detMovement.Controls.Add(chooseDetPlay);
            detMovement.Controls.Add(moveDetective);
            detMovement.Controls.Add(movedDet);
            detMovement.Controls.Add(detective);
            detMovement.Location = new Point(12, 422);
            detMovement.Name = "detMovement";
            detMovement.Size = new Size(638, 101);
            detMovement.TabIndex = 28;
            detMovement.Visible = false;
            // 
            // label1
            // 
            label1.Font = new Font("Segoe UI", 12F);
            label1.Location = new Point(349, 0);
            label1.Name = "label1";
            label1.Size = new Size(290, 101);
            label1.TabIndex = 15;
            label1.Text = "Roter Knoten = Position des Diebes\r\nDiamand Form = Position der Detektive\r\n";
            // 
            // thiefMovement
            // 
            thiefMovement.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            thiefMovement.Controls.Add(label2);
            thiefMovement.Controls.Add(moveThiefToTarget);
            thiefMovement.Controls.Add(targetThiefInput);
            thiefMovement.Controls.Add(targetVertexofThief);
            thiefMovement.Controls.Add(chooseThiefPlay);
            thiefMovement.Location = new Point(12, 419);
            thiefMovement.Name = "thiefMovement";
            thiefMovement.Size = new Size(635, 101);
            thiefMovement.TabIndex = 30;
            thiefMovement.Visible = false;
            // 
            // label2
            // 
            label2.Font = new Font("Segoe UI", 12F);
            label2.Location = new Point(349, 0);
            label2.Name = "label2";
            label2.Size = new Size(290, 101);
            label2.TabIndex = 16;
            label2.Text = "Roter Knoten = Position des Diebes\r\nDiamand Form = Position der Detektive\r\n";
            // 
            // moveThiefToTarget
            // 
            moveThiefToTarget.Font = new Font("Segoe UI", 12F);
            moveThiefToTarget.Location = new Point(198, 22);
            moveThiefToTarget.Name = "moveThiefToTarget";
            moveThiefToTarget.Size = new Size(145, 31);
            moveThiefToTarget.TabIndex = 3;
            moveThiefToTarget.Text = "Dieb bewegen";
            moveThiefToTarget.UseVisualStyleBackColor = true;
            moveThiefToTarget.Click += moveThiefToTarget_Click;
            // 
            // targetThiefInput
            // 
            targetThiefInput.Font = new Font("Segoe UI", 12F);
            targetThiefInput.Location = new Point(92, 24);
            targetThiefInput.Name = "targetThiefInput";
            targetThiefInput.Size = new Size(100, 29);
            targetThiefInput.TabIndex = 2;
            // 
            // targetVertexofThief
            // 
            targetVertexofThief.AutoSize = true;
            targetVertexofThief.Font = new Font("Segoe UI", 12F);
            targetVertexofThief.Location = new Point(0, 27);
            targetVertexofThief.Name = "targetVertexofThief";
            targetVertexofThief.Size = new Size(86, 21);
            targetVertexofThief.TabIndex = 1;
            targetVertexofThief.Text = "Zielknoten:";
            // 
            // chooseThiefPlay
            // 
            chooseThiefPlay.AutoSize = true;
            chooseThiefPlay.Font = new Font("Segoe UI", 12F);
            chooseThiefPlay.Location = new Point(0, 0);
            chooseThiefPlay.Name = "chooseThiefPlay";
            chooseThiefPlay.Size = new Size(138, 21);
            chooseThiefPlay.TabIndex = 0;
            chooseThiefPlay.Text = "Wähle deinen Zug:";
            // 
            // TorusCreate
            // 
            TorusCreate.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            TorusCreate.Controls.Add(torusGraph);
            TorusCreate.Controls.Add(torusN);
            TorusCreate.Controls.Add(torusM);
            TorusCreate.Controls.Add(createTorusGraph);
            TorusCreate.Controls.Add(x);
            TorusCreate.Location = new Point(652, 196);
            TorusCreate.Name = "TorusCreate";
            TorusCreate.Size = new Size(231, 164);
            TorusCreate.TabIndex = 23;
            TorusCreate.Visible = false;
            // 
            // torusGraph
            // 
            torusGraph.Font = new Font("Segoe UI", 12F);
            torusGraph.ForeColor = SystemColors.WindowText;
            torusGraph.Location = new Point(0, 0);
            torusGraph.Name = "torusGraph";
            torusGraph.Size = new Size(231, 45);
            torusGraph.TabIndex = 19;
            torusGraph.Text = "Größe des Torusgraphen eingeben:";
            // 
            // torusN
            // 
            torusN.BackColor = SystemColors.Window;
            torusN.Font = new Font("Segoe UI", 12F);
            torusN.ForeColor = SystemColors.WindowText;
            torusN.Location = new Point(0, 48);
            torusN.Name = "torusN";
            torusN.Size = new Size(100, 29);
            torusN.TabIndex = 16;
            // 
            // torusM
            // 
            torusM.BackColor = SystemColors.Window;
            torusM.Font = new Font("Segoe UI", 12F);
            torusM.ForeColor = SystemColors.WindowText;
            torusM.Location = new Point(131, 48);
            torusM.Name = "torusM";
            torusM.Size = new Size(100, 29);
            torusM.TabIndex = 17;
            // 
            // createTorusGraph
            // 
            createTorusGraph.BackColor = SystemColors.ButtonHighlight;
            createTorusGraph.Font = new Font("Segoe UI", 12F);
            createTorusGraph.ForeColor = SystemColors.ControlText;
            createTorusGraph.Location = new Point(0, 83);
            createTorusGraph.Name = "createTorusGraph";
            createTorusGraph.Size = new Size(231, 31);
            createTorusGraph.TabIndex = 20;
            createTorusGraph.Text = "Torusgraph erstellen";
            createTorusGraph.UseVisualStyleBackColor = false;
            createTorusGraph.Click += createTorusGraph_Click;
            // 
            // x
            // 
            x.AutoSize = true;
            x.Font = new Font("Segoe UI", 12F);
            x.ForeColor = SystemColors.WindowText;
            x.Location = new Point(107, 51);
            x.Name = "x";
            x.Size = new Size(17, 21);
            x.TabIndex = 18;
            x.Text = "x";
            // 
            // chooseGraphTyp
            // 
            chooseGraphTyp.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            chooseGraphTyp.DropDownStyle = ComboBoxStyle.DropDownList;
            chooseGraphTyp.Font = new Font("Segoe UI", 12F);
            chooseGraphTyp.FormattingEnabled = true;
            chooseGraphTyp.Items.AddRange(new object[] { "Torusgraph", "Ungerichteter Kreisgraph", "Gerichteter Kreisgraph", "Graph von unärer Funktion", "Komplett verbundener Graph" });
            chooseGraphTyp.Location = new Point(652, 161);
            chooseGraphTyp.Name = "chooseGraphTyp";
            chooseGraphTyp.RightToLeft = RightToLeft.No;
            chooseGraphTyp.Size = new Size(231, 29);
            chooseGraphTyp.TabIndex = 0;
            chooseGraphTyp.SelectedIndexChanged += chooseGraphTyp_SelectedIndexChanged;
            // 
            // createUnCircleGraph
            // 
            createUnCircleGraph.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            createUnCircleGraph.Controls.Add(unCircleSize);
            createUnCircleGraph.Controls.Add(createUndirectedCircleGraph);
            createUnCircleGraph.Controls.Add(unCircleSizeInput);
            createUnCircleGraph.Location = new Point(652, 196);
            createUnCircleGraph.Name = "createUnCircleGraph";
            createUnCircleGraph.Size = new Size(231, 164);
            createUnCircleGraph.TabIndex = 31;
            createUnCircleGraph.Visible = false;
            // 
            // unCircleSize
            // 
            unCircleSize.AutoSize = true;
            unCircleSize.Font = new Font("Segoe UI", 12F);
            unCircleSize.Location = new Point(0, 0);
            unCircleSize.Name = "unCircleSize";
            unCircleSize.Size = new Size(124, 21);
            unCircleSize.TabIndex = 2;
            unCircleSize.Text = "Größe eingeben:";
            // 
            // createUndirectedCircleGraph
            // 
            createUndirectedCircleGraph.Font = new Font("Segoe UI", 12F);
            createUndirectedCircleGraph.Location = new Point(0, 59);
            createUndirectedCircleGraph.Name = "createUndirectedCircleGraph";
            createUndirectedCircleGraph.Size = new Size(231, 31);
            createUndirectedCircleGraph.TabIndex = 1;
            createUndirectedCircleGraph.Text = "Graph erstellen";
            createUndirectedCircleGraph.UseVisualStyleBackColor = true;
            createUndirectedCircleGraph.Click += createUndirectedCircleGraph_Click;
            // 
            // unCircleSizeInput
            // 
            unCircleSizeInput.Font = new Font("Segoe UI", 12F);
            unCircleSizeInput.Location = new Point(0, 24);
            unCircleSizeInput.Name = "unCircleSizeInput";
            unCircleSizeInput.Size = new Size(231, 29);
            unCircleSizeInput.TabIndex = 0;
            // 
            // createDiCircleGraph
            // 
            createDiCircleGraph.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            createDiCircleGraph.Controls.Add(diCircleLabel);
            createDiCircleGraph.Controls.Add(createDirectedCircleGraph);
            createDiCircleGraph.Controls.Add(diCircleSizeInput);
            createDiCircleGraph.Location = new Point(652, 196);
            createDiCircleGraph.Name = "createDiCircleGraph";
            createDiCircleGraph.Size = new Size(231, 166);
            createDiCircleGraph.TabIndex = 32;
            createDiCircleGraph.Visible = false;
            // 
            // diCircleLabel
            // 
            diCircleLabel.AutoSize = true;
            diCircleLabel.Font = new Font("Segoe UI", 12F);
            diCircleLabel.Location = new Point(3, 3);
            diCircleLabel.Name = "diCircleLabel";
            diCircleLabel.Size = new Size(124, 21);
            diCircleLabel.TabIndex = 3;
            diCircleLabel.Text = "Größe eingeben:";
            // 
            // createDirectedCircleGraph
            // 
            createDirectedCircleGraph.Font = new Font("Segoe UI", 12F);
            createDirectedCircleGraph.Location = new Point(0, 59);
            createDirectedCircleGraph.Name = "createDirectedCircleGraph";
            createDirectedCircleGraph.Size = new Size(231, 31);
            createDirectedCircleGraph.TabIndex = 2;
            createDirectedCircleGraph.Text = "Graph erstellen";
            createDirectedCircleGraph.UseVisualStyleBackColor = true;
            createDirectedCircleGraph.Click += createDirectedCircleGraph_Click;
            // 
            // diCircleSizeInput
            // 
            diCircleSizeInput.Font = new Font("Segoe UI", 12F);
            diCircleSizeInput.Location = new Point(0, 24);
            diCircleSizeInput.Name = "diCircleSizeInput";
            diCircleSizeInput.Size = new Size(231, 29);
            diCircleSizeInput.TabIndex = 1;
            // 
            // openFileDialog1
            // 
            openFileDialog1.FileName = "openFileDialog1";
            // 
            // createFullyConGraphPanel
            // 
            createFullyConGraphPanel.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            createFullyConGraphPanel.Controls.Add(fullyConCreate);
            createFullyConGraphPanel.Controls.Add(fullyConSizeInput);
            createFullyConGraphPanel.Controls.Add(fullyConSize);
            createFullyConGraphPanel.Location = new Point(652, 196);
            createFullyConGraphPanel.Name = "createFullyConGraphPanel";
            createFullyConGraphPanel.Size = new Size(231, 164);
            createFullyConGraphPanel.TabIndex = 33;
            createFullyConGraphPanel.Visible = false;
            // 
            // fullyConCreate
            // 
            fullyConCreate.Font = new Font("Segoe UI", 12F);
            fullyConCreate.Location = new Point(0, 62);
            fullyConCreate.Name = "fullyConCreate";
            fullyConCreate.Size = new Size(231, 31);
            fullyConCreate.TabIndex = 5;
            fullyConCreate.Text = "Graph erstellen";
            fullyConCreate.UseVisualStyleBackColor = true;
            fullyConCreate.Click += fullyConCreate_Click;
            // 
            // fullyConSizeInput
            // 
            fullyConSizeInput.Font = new Font("Segoe UI", 12F);
            fullyConSizeInput.Location = new Point(0, 27);
            fullyConSizeInput.Name = "fullyConSizeInput";
            fullyConSizeInput.Size = new Size(231, 29);
            fullyConSizeInput.TabIndex = 4;
            // 
            // fullyConSize
            // 
            fullyConSize.AutoSize = true;
            fullyConSize.Font = new Font("Segoe UI", 12F);
            fullyConSize.Location = new Point(3, 3);
            fullyConSize.Name = "fullyConSize";
            fullyConSize.Size = new Size(124, 21);
            fullyConSize.TabIndex = 3;
            fullyConSize.Text = "Größe eingeben:";
            // 
            // createUnaryFunc
            // 
            createUnaryFunc.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            createUnaryFunc.Controls.Add(unaryFuncDomainLabel);
            createUnaryFunc.Controls.Add(lunaryFuncToLabel);
            createUnaryFunc.Controls.Add(unaryFuncStartDomImput);
            createUnaryFunc.Controls.Add(unaryFuncEndDomImput);
            createUnaryFunc.Controls.Add(unaryFuncCreate);
            createUnaryFunc.Controls.Add(unaryFuncInput);
            createUnaryFunc.Controls.Add(unaryFuncLabel);
            createUnaryFunc.Location = new Point(652, 196);
            createUnaryFunc.Name = "createUnaryFunc";
            createUnaryFunc.Size = new Size(231, 164);
            createUnaryFunc.TabIndex = 34;
            createUnaryFunc.Visible = false;
            // 
            // unaryFuncDomainLabel
            // 
            unaryFuncDomainLabel.AutoSize = true;
            unaryFuncDomainLabel.Font = new Font("Segoe UI", 12F);
            unaryFuncDomainLabel.Location = new Point(5, 59);
            unaryFuncDomainLabel.Name = "unaryFuncDomainLabel";
            unaryFuncDomainLabel.Size = new Size(207, 21);
            unaryFuncDomainLabel.TabIndex = 10;
            unaryFuncDomainLabel.Text = "Definitionsbereich eingeben:";
            // 
            // lunaryFuncToLabel
            // 
            lunaryFuncToLabel.AutoSize = true;
            lunaryFuncToLabel.Font = new Font("Segoe UI", 12F);
            lunaryFuncToLabel.Location = new Point(102, 86);
            lunaryFuncToLabel.Name = "lunaryFuncToLabel";
            lunaryFuncToLabel.Size = new Size(30, 21);
            lunaryFuncToLabel.TabIndex = 9;
            lunaryFuncToLabel.Text = "bis";
            // 
            // unaryFuncStartDomImput
            // 
            unaryFuncStartDomImput.Font = new Font("Segoe UI", 12F);
            unaryFuncStartDomImput.Location = new Point(1, 83);
            unaryFuncStartDomImput.Name = "unaryFuncStartDomImput";
            unaryFuncStartDomImput.Size = new Size(95, 29);
            unaryFuncStartDomImput.TabIndex = 8;
            // 
            // unaryFuncEndDomImput
            // 
            unaryFuncEndDomImput.Font = new Font("Segoe UI", 12F);
            unaryFuncEndDomImput.Location = new Point(136, 83);
            unaryFuncEndDomImput.Name = "unaryFuncEndDomImput";
            unaryFuncEndDomImput.Size = new Size(95, 29);
            unaryFuncEndDomImput.TabIndex = 7;
            // 
            // unaryFuncCreate
            // 
            unaryFuncCreate.Font = new Font("Segoe UI", 12F);
            unaryFuncCreate.Location = new Point(0, 118);
            unaryFuncCreate.Name = "unaryFuncCreate";
            unaryFuncCreate.Size = new Size(231, 31);
            unaryFuncCreate.TabIndex = 6;
            unaryFuncCreate.Text = "Graph erstellen";
            unaryFuncCreate.UseVisualStyleBackColor = true;
            unaryFuncCreate.Click += unaryFuncCreate_Click;
            // 
            // unaryFuncInput
            // 
            unaryFuncInput.Font = new Font("Segoe UI", 12F);
            unaryFuncInput.Location = new Point(0, 27);
            unaryFuncInput.Name = "unaryFuncInput";
            unaryFuncInput.Size = new Size(231, 29);
            unaryFuncInput.TabIndex = 5;
            // 
            // unaryFuncLabel
            // 
            unaryFuncLabel.AutoSize = true;
            unaryFuncLabel.Font = new Font("Segoe UI", 12F);
            unaryFuncLabel.Location = new Point(3, 3);
            unaryFuncLabel.Name = "unaryFuncLabel";
            unaryFuncLabel.Size = new Size(186, 21);
            unaryFuncLabel.TabIndex = 4;
            unaryFuncLabel.Text = "unäre Funktion eingeben:";
            // 
            // MainScreen
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.Window;
            ClientSize = new Size(895, 544);
            Controls.Add(createUnaryFunc);
            Controls.Add(createFullyConGraphPanel);
            Controls.Add(createUnCircleGraph);
            Controls.Add(createDiCircleGraph);
            Controls.Add(restartGame);
            Controls.Add(detMovement);
            Controls.Add(GraphPicture);
            Controls.Add(editGraph);
            Controls.Add(computeEnt);
            Controls.Add(graphCreate);
            Controls.Add(playGraph);
            Controls.Add(TextOutput);
            Controls.Add(headline);
            Controls.Add(GameSettings);
            Controls.Add(chooseGraphTyp);
            Controls.Add(TorusCreate);
            Controls.Add(thiefMovement);
            MinimumSize = new Size(911, 567);
            Name = "MainScreen";
            Text = "Entanglement ist ein Maß für die Komläxität eines Graphs";
            ((System.ComponentModel.ISupportInitialize)GraphPicture).EndInit();
            graphCreate.ResumeLayout(false);
            graphCreate.PerformLayout();
            computeEnt.ResumeLayout(false);
            computeEnt.PerformLayout();
            GameSettings.ResumeLayout(false);
            GameSettings.PerformLayout();
            detMovement.ResumeLayout(false);
            detMovement.PerformLayout();
            thiefMovement.ResumeLayout(false);
            thiefMovement.PerformLayout();
            TorusCreate.ResumeLayout(false);
            TorusCreate.PerformLayout();
            createUnCircleGraph.ResumeLayout(false);
            createUnCircleGraph.PerformLayout();
            createDiCircleGraph.ResumeLayout(false);
            createDiCircleGraph.PerformLayout();
            createFullyConGraphPanel.ResumeLayout(false);
            createFullyConGraphPanel.PerformLayout();
            createUnaryFunc.ResumeLayout(false);
            createUnaryFunc.PerformLayout();
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
        private Button playGraph;
        private Panel graphCreate;
        private Panel computeEnt;
        private Label startPos2;
        private Panel GameSettings;
        private Label detectiveAmount;
        private TextBox startPosInput2;
        private Button playDetective;
        private Button playThief;
        private TextBox detectiveAmountInput;
        private Button editGraph;
        private Label chooseDetPlay;
        private TextBox movedDet;
        private Label detective;
        private Button moveDetective;
        private Button doNothingDet;
        private Panel detMovement;
        private Button restartGame;
        private Label label1;
        private Panel thiefMovement;
        private Label targetVertexofThief;
        private Label chooseThiefPlay;
        private Label label2;
        private Button moveThiefToTarget;
        private TextBox targetThiefInput;
        private Panel TorusCreate;
        private Label torusGraph;
        private TextBox torusN;
        private TextBox torusM;
        private Button createTorusGraph;
        private Label x;
        private ComboBox chooseGraphTyp;
        private Panel createUnCircleGraph;
        private Button createUndirectedCircleGraph;
        private TextBox unCircleSizeInput;
        private Label unCircleSize;
        private Panel createDiCircleGraph;
        private Button createDirectedCircleGraph;
        private TextBox diCircleSizeInput;
        private Label diCircleLabel;
        private OpenFileDialog openFileDialog1;
        private Panel createFullyConGraphPanel;
        private Button fullyConCreate;
        private TextBox fullyConSizeInput;
        private Label fullyConSize;
        private Panel createUnaryFunc;
        private Button unaryFuncCreate;
        private TextBox unaryFuncInput;
        private Label unaryFuncLabel;
        private TextBox unaryFuncEndDomImput;
        private TextBox unaryFuncStartDomImput;
        private Label lunaryFuncToLabel;
        private Label unaryFuncDomainLabel;
    }
}
