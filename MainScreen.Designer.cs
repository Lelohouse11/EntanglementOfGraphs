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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainScreen));
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
            resources.ApplyResources(GraphPicture, "GraphPicture");
            GraphPicture.BorderStyle = BorderStyle.FixedSingle;
            GraphPicture.Name = "GraphPicture";
            GraphPicture.TabStop = false;
            GraphPicture.Paint += GraphPicture_Paint;
            // 
            // vertexInput
            // 
            vertexInput.BackColor = SystemColors.Window;
            resources.ApplyResources(vertexInput, "vertexInput");
            vertexInput.ForeColor = SystemColors.WindowText;
            vertexInput.Name = "vertexInput";
            // 
            // headline
            // 
            resources.ApplyResources(headline, "headline");
            headline.ForeColor = SystemColors.WindowText;
            headline.Name = "headline";
            // 
            // input
            // 
            resources.ApplyResources(input, "input");
            input.ForeColor = SystemColors.WindowText;
            input.Name = "input";
            // 
            // newVertex
            // 
            newVertex.BackColor = SystemColors.ButtonHighlight;
            resources.ApplyResources(newVertex, "newVertex");
            newVertex.ForeColor = SystemColors.ControlText;
            newVertex.Name = "newVertex";
            newVertex.UseVisualStyleBackColor = false;
            newVertex.Click += newVertex_Click;
            // 
            // entanglement
            // 
            entanglement.BackColor = SystemColors.ButtonHighlight;
            resources.ApplyResources(entanglement, "entanglement");
            entanglement.ForeColor = SystemColors.ControlText;
            entanglement.Name = "entanglement";
            entanglement.UseVisualStyleBackColor = false;
            entanglement.Click += entanglement_Click;
            // 
            // TextOutput
            // 
            resources.ApplyResources(TextOutput, "TextOutput");
            TextOutput.BackColor = SystemColors.Window;
            TextOutput.ForeColor = SystemColors.WindowText;
            TextOutput.Name = "TextOutput";
            TextOutput.ReadOnly = true;
            // 
            // vertex
            // 
            resources.ApplyResources(vertex, "vertex");
            vertex.ForeColor = SystemColors.WindowText;
            vertex.Name = "vertex";
            // 
            // edgeFrom
            // 
            resources.ApplyResources(edgeFrom, "edgeFrom");
            edgeFrom.ForeColor = SystemColors.WindowText;
            edgeFrom.Name = "edgeFrom";
            // 
            // edgeTo
            // 
            resources.ApplyResources(edgeTo, "edgeTo");
            edgeTo.ForeColor = SystemColors.WindowText;
            edgeTo.Name = "edgeTo";
            // 
            // edgeSource
            // 
            edgeSource.BackColor = SystemColors.Window;
            resources.ApplyResources(edgeSource, "edgeSource");
            edgeSource.ForeColor = SystemColors.WindowText;
            edgeSource.Name = "edgeSource";
            // 
            // edgeTarget
            // 
            edgeTarget.BackColor = SystemColors.Window;
            resources.ApplyResources(edgeTarget, "edgeTarget");
            edgeTarget.ForeColor = SystemColors.WindowText;
            edgeTarget.Name = "edgeTarget";
            // 
            // addEdge
            // 
            addEdge.BackColor = SystemColors.ButtonHighlight;
            resources.ApplyResources(addEdge, "addEdge");
            addEdge.ForeColor = SystemColors.ControlText;
            addEdge.Name = "addEdge";
            addEdge.UseVisualStyleBackColor = false;
            addEdge.Click += addEdge_Click;
            // 
            // deleteGraph
            // 
            deleteGraph.BackColor = SystemColors.ButtonHighlight;
            resources.ApplyResources(deleteGraph, "deleteGraph");
            deleteGraph.ForeColor = SystemColors.ControlText;
            deleteGraph.Name = "deleteGraph";
            deleteGraph.UseVisualStyleBackColor = false;
            deleteGraph.Click += deleteGraph_Click;
            // 
            // startPos
            // 
            resources.ApplyResources(startPos, "startPos");
            startPos.ForeColor = SystemColors.WindowText;
            startPos.Name = "startPos";
            // 
            // startPosInput
            // 
            startPosInput.BackColor = SystemColors.Window;
            resources.ApplyResources(startPosInput, "startPosInput");
            startPosInput.ForeColor = SystemColors.WindowText;
            startPosInput.Name = "startPosInput";
            // 
            // playGraph
            // 
            resources.ApplyResources(playGraph, "playGraph");
            playGraph.BackColor = SystemColors.ButtonHighlight;
            playGraph.ForeColor = SystemColors.ControlText;
            playGraph.Name = "playGraph";
            playGraph.UseVisualStyleBackColor = false;
            playGraph.Click += playGraph_Click;
            // 
            // graphCreate
            // 
            resources.ApplyResources(graphCreate, "graphCreate");
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
            graphCreate.Name = "graphCreate";
            // 
            // doNothingDet
            // 
            doNothingDet.BackColor = SystemColors.ButtonHighlight;
            resources.ApplyResources(doNothingDet, "doNothingDet");
            doNothingDet.ForeColor = SystemColors.ControlText;
            doNothingDet.Name = "doNothingDet";
            doNothingDet.UseVisualStyleBackColor = false;
            doNothingDet.Click += doNothingDet_Click;
            // 
            // moveDetective
            // 
            moveDetective.BackColor = SystemColors.ButtonHighlight;
            resources.ApplyResources(moveDetective, "moveDetective");
            moveDetective.ForeColor = SystemColors.ControlText;
            moveDetective.Name = "moveDetective";
            moveDetective.UseVisualStyleBackColor = false;
            moveDetective.Click += moveDetective_Click;
            // 
            // movedDet
            // 
            movedDet.BackColor = SystemColors.Window;
            resources.ApplyResources(movedDet, "movedDet");
            movedDet.ForeColor = SystemColors.WindowText;
            movedDet.Name = "movedDet";
            // 
            // detective
            // 
            resources.ApplyResources(detective, "detective");
            detective.ForeColor = SystemColors.WindowText;
            detective.Name = "detective";
            // 
            // chooseDetPlay
            // 
            resources.ApplyResources(chooseDetPlay, "chooseDetPlay");
            chooseDetPlay.ForeColor = SystemColors.WindowText;
            chooseDetPlay.Name = "chooseDetPlay";
            // 
            // computeEnt
            // 
            resources.ApplyResources(computeEnt, "computeEnt");
            computeEnt.Controls.Add(startPos);
            computeEnt.Controls.Add(entanglement);
            computeEnt.Controls.Add(startPosInput);
            computeEnt.Name = "computeEnt";
            // 
            // startPos2
            // 
            resources.ApplyResources(startPos2, "startPos2");
            startPos2.ForeColor = SystemColors.WindowText;
            startPos2.Name = "startPos2";
            // 
            // GameSettings
            // 
            resources.ApplyResources(GameSettings, "GameSettings");
            GameSettings.Controls.Add(playDetective);
            GameSettings.Controls.Add(playThief);
            GameSettings.Controls.Add(detectiveAmountInput);
            GameSettings.Controls.Add(detectiveAmount);
            GameSettings.Controls.Add(startPosInput2);
            GameSettings.Controls.Add(startPos2);
            GameSettings.Name = "GameSettings";
            // 
            // playDetective
            // 
            playDetective.BackColor = SystemColors.ButtonHighlight;
            resources.ApplyResources(playDetective, "playDetective");
            playDetective.ForeColor = SystemColors.ControlText;
            playDetective.Name = "playDetective";
            playDetective.UseVisualStyleBackColor = false;
            playDetective.Click += playDetective_Click;
            // 
            // playThief
            // 
            playThief.BackColor = SystemColors.ButtonHighlight;
            resources.ApplyResources(playThief, "playThief");
            playThief.ForeColor = SystemColors.ControlText;
            playThief.Name = "playThief";
            playThief.UseVisualStyleBackColor = false;
            playThief.Click += playThief_Click;
            // 
            // detectiveAmountInput
            // 
            detectiveAmountInput.BackColor = SystemColors.Window;
            resources.ApplyResources(detectiveAmountInput, "detectiveAmountInput");
            detectiveAmountInput.ForeColor = SystemColors.WindowText;
            detectiveAmountInput.Name = "detectiveAmountInput";
            // 
            // detectiveAmount
            // 
            resources.ApplyResources(detectiveAmount, "detectiveAmount");
            detectiveAmount.ForeColor = SystemColors.WindowText;
            detectiveAmount.Name = "detectiveAmount";
            // 
            // startPosInput2
            // 
            startPosInput2.BackColor = SystemColors.Window;
            resources.ApplyResources(startPosInput2, "startPosInput2");
            startPosInput2.ForeColor = SystemColors.WindowText;
            startPosInput2.Name = "startPosInput2";
            // 
            // restartGame
            // 
            resources.ApplyResources(restartGame, "restartGame");
            restartGame.Name = "restartGame";
            restartGame.UseVisualStyleBackColor = true;
            restartGame.Click += restartGame_Click_1;
            // 
            // editGraph
            // 
            resources.ApplyResources(editGraph, "editGraph");
            editGraph.BackColor = SystemColors.ButtonHighlight;
            editGraph.ForeColor = SystemColors.ControlText;
            editGraph.Name = "editGraph";
            editGraph.UseVisualStyleBackColor = false;
            editGraph.Click += editGraph_Click;
            // 
            // detMovement
            // 
            resources.ApplyResources(detMovement, "detMovement");
            detMovement.Controls.Add(label1);
            detMovement.Controls.Add(doNothingDet);
            detMovement.Controls.Add(chooseDetPlay);
            detMovement.Controls.Add(moveDetective);
            detMovement.Controls.Add(movedDet);
            detMovement.Controls.Add(detective);
            detMovement.Name = "detMovement";
            // 
            // label1
            // 
            resources.ApplyResources(label1, "label1");
            label1.Name = "label1";
            // 
            // thiefMovement
            // 
            resources.ApplyResources(thiefMovement, "thiefMovement");
            thiefMovement.Controls.Add(label2);
            thiefMovement.Controls.Add(moveThiefToTarget);
            thiefMovement.Controls.Add(targetThiefInput);
            thiefMovement.Controls.Add(targetVertexofThief);
            thiefMovement.Controls.Add(chooseThiefPlay);
            thiefMovement.Name = "thiefMovement";
            // 
            // label2
            // 
            resources.ApplyResources(label2, "label2");
            label2.Name = "label2";
            // 
            // moveThiefToTarget
            // 
            resources.ApplyResources(moveThiefToTarget, "moveThiefToTarget");
            moveThiefToTarget.Name = "moveThiefToTarget";
            moveThiefToTarget.UseVisualStyleBackColor = true;
            moveThiefToTarget.Click += moveThiefToTarget_Click;
            // 
            // targetThiefInput
            // 
            resources.ApplyResources(targetThiefInput, "targetThiefInput");
            targetThiefInput.Name = "targetThiefInput";
            // 
            // targetVertexofThief
            // 
            resources.ApplyResources(targetVertexofThief, "targetVertexofThief");
            targetVertexofThief.Name = "targetVertexofThief";
            // 
            // chooseThiefPlay
            // 
            resources.ApplyResources(chooseThiefPlay, "chooseThiefPlay");
            chooseThiefPlay.Name = "chooseThiefPlay";
            // 
            // TorusCreate
            // 
            resources.ApplyResources(TorusCreate, "TorusCreate");
            TorusCreate.Controls.Add(torusGraph);
            TorusCreate.Controls.Add(torusN);
            TorusCreate.Controls.Add(torusM);
            TorusCreate.Controls.Add(createTorusGraph);
            TorusCreate.Controls.Add(x);
            TorusCreate.Name = "TorusCreate";
            // 
            // torusGraph
            // 
            resources.ApplyResources(torusGraph, "torusGraph");
            torusGraph.ForeColor = SystemColors.WindowText;
            torusGraph.Name = "torusGraph";
            // 
            // torusN
            // 
            torusN.BackColor = SystemColors.Window;
            resources.ApplyResources(torusN, "torusN");
            torusN.ForeColor = SystemColors.WindowText;
            torusN.Name = "torusN";
            // 
            // torusM
            // 
            torusM.BackColor = SystemColors.Window;
            resources.ApplyResources(torusM, "torusM");
            torusM.ForeColor = SystemColors.WindowText;
            torusM.Name = "torusM";
            // 
            // createTorusGraph
            // 
            createTorusGraph.BackColor = SystemColors.ButtonHighlight;
            resources.ApplyResources(createTorusGraph, "createTorusGraph");
            createTorusGraph.ForeColor = SystemColors.ControlText;
            createTorusGraph.Name = "createTorusGraph";
            createTorusGraph.UseVisualStyleBackColor = false;
            createTorusGraph.Click += createTorusGraph_Click;
            // 
            // x
            // 
            resources.ApplyResources(x, "x");
            x.ForeColor = SystemColors.WindowText;
            x.Name = "x";
            // 
            // chooseGraphTyp
            // 
            resources.ApplyResources(chooseGraphTyp, "chooseGraphTyp");
            chooseGraphTyp.DropDownStyle = ComboBoxStyle.DropDownList;
            chooseGraphTyp.FormattingEnabled = true;
            chooseGraphTyp.Items.AddRange(new object[] { resources.GetString("chooseGraphTyp.Items"), resources.GetString("chooseGraphTyp.Items1"), resources.GetString("chooseGraphTyp.Items2"), resources.GetString("chooseGraphTyp.Items3"), resources.GetString("chooseGraphTyp.Items4") });
            chooseGraphTyp.Name = "chooseGraphTyp";
            chooseGraphTyp.SelectedIndexChanged += chooseGraphTyp_SelectedIndexChanged;
            // 
            // createUnCircleGraph
            // 
            resources.ApplyResources(createUnCircleGraph, "createUnCircleGraph");
            createUnCircleGraph.Controls.Add(unCircleSize);
            createUnCircleGraph.Controls.Add(createUndirectedCircleGraph);
            createUnCircleGraph.Controls.Add(unCircleSizeInput);
            createUnCircleGraph.Name = "createUnCircleGraph";
            // 
            // unCircleSize
            // 
            resources.ApplyResources(unCircleSize, "unCircleSize");
            unCircleSize.Name = "unCircleSize";
            // 
            // createUndirectedCircleGraph
            // 
            resources.ApplyResources(createUndirectedCircleGraph, "createUndirectedCircleGraph");
            createUndirectedCircleGraph.Name = "createUndirectedCircleGraph";
            createUndirectedCircleGraph.UseVisualStyleBackColor = true;
            createUndirectedCircleGraph.Click += createUndirectedCircleGraph_Click;
            // 
            // unCircleSizeInput
            // 
            resources.ApplyResources(unCircleSizeInput, "unCircleSizeInput");
            unCircleSizeInput.Name = "unCircleSizeInput";
            // 
            // createDiCircleGraph
            // 
            resources.ApplyResources(createDiCircleGraph, "createDiCircleGraph");
            createDiCircleGraph.Controls.Add(diCircleLabel);
            createDiCircleGraph.Controls.Add(createDirectedCircleGraph);
            createDiCircleGraph.Controls.Add(diCircleSizeInput);
            createDiCircleGraph.Name = "createDiCircleGraph";
            // 
            // diCircleLabel
            // 
            resources.ApplyResources(diCircleLabel, "diCircleLabel");
            diCircleLabel.Name = "diCircleLabel";
            // 
            // createDirectedCircleGraph
            // 
            resources.ApplyResources(createDirectedCircleGraph, "createDirectedCircleGraph");
            createDirectedCircleGraph.Name = "createDirectedCircleGraph";
            createDirectedCircleGraph.UseVisualStyleBackColor = true;
            createDirectedCircleGraph.Click += createDirectedCircleGraph_Click;
            // 
            // diCircleSizeInput
            // 
            resources.ApplyResources(diCircleSizeInput, "diCircleSizeInput");
            diCircleSizeInput.Name = "diCircleSizeInput";
            // 
            // openFileDialog1
            // 
            openFileDialog1.FileName = "openFileDialog1";
            // 
            // createFullyConGraphPanel
            // 
            resources.ApplyResources(createFullyConGraphPanel, "createFullyConGraphPanel");
            createFullyConGraphPanel.Controls.Add(fullyConCreate);
            createFullyConGraphPanel.Controls.Add(fullyConSizeInput);
            createFullyConGraphPanel.Controls.Add(fullyConSize);
            createFullyConGraphPanel.Name = "createFullyConGraphPanel";
            // 
            // fullyConCreate
            // 
            resources.ApplyResources(fullyConCreate, "fullyConCreate");
            fullyConCreate.Name = "fullyConCreate";
            fullyConCreate.UseVisualStyleBackColor = true;
            fullyConCreate.Click += fullyConCreate_Click;
            // 
            // fullyConSizeInput
            // 
            resources.ApplyResources(fullyConSizeInput, "fullyConSizeInput");
            fullyConSizeInput.Name = "fullyConSizeInput";
            // 
            // fullyConSize
            // 
            resources.ApplyResources(fullyConSize, "fullyConSize");
            fullyConSize.Name = "fullyConSize";
            // 
            // createUnaryFunc
            // 
            resources.ApplyResources(createUnaryFunc, "createUnaryFunc");
            createUnaryFunc.Controls.Add(unaryFuncDomainLabel);
            createUnaryFunc.Controls.Add(lunaryFuncToLabel);
            createUnaryFunc.Controls.Add(unaryFuncStartDomImput);
            createUnaryFunc.Controls.Add(unaryFuncEndDomImput);
            createUnaryFunc.Controls.Add(unaryFuncCreate);
            createUnaryFunc.Controls.Add(unaryFuncInput);
            createUnaryFunc.Controls.Add(unaryFuncLabel);
            createUnaryFunc.Name = "createUnaryFunc";
            // 
            // unaryFuncDomainLabel
            // 
            resources.ApplyResources(unaryFuncDomainLabel, "unaryFuncDomainLabel");
            unaryFuncDomainLabel.Name = "unaryFuncDomainLabel";
            // 
            // lunaryFuncToLabel
            // 
            resources.ApplyResources(lunaryFuncToLabel, "lunaryFuncToLabel");
            lunaryFuncToLabel.Name = "lunaryFuncToLabel";
            // 
            // unaryFuncStartDomImput
            // 
            resources.ApplyResources(unaryFuncStartDomImput, "unaryFuncStartDomImput");
            unaryFuncStartDomImput.Name = "unaryFuncStartDomImput";
            // 
            // unaryFuncEndDomImput
            // 
            resources.ApplyResources(unaryFuncEndDomImput, "unaryFuncEndDomImput");
            unaryFuncEndDomImput.Name = "unaryFuncEndDomImput";
            // 
            // unaryFuncCreate
            // 
            resources.ApplyResources(unaryFuncCreate, "unaryFuncCreate");
            unaryFuncCreate.Name = "unaryFuncCreate";
            unaryFuncCreate.UseVisualStyleBackColor = true;
            unaryFuncCreate.Click += unaryFuncCreate_Click;
            // 
            // unaryFuncInput
            // 
            resources.ApplyResources(unaryFuncInput, "unaryFuncInput");
            unaryFuncInput.Name = "unaryFuncInput";
            // 
            // unaryFuncLabel
            // 
            resources.ApplyResources(unaryFuncLabel, "unaryFuncLabel");
            unaryFuncLabel.Name = "unaryFuncLabel";
            // 
            // MainScreen
            // 
            resources.ApplyResources(this, "$this");
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.Window;
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
            Name = "MainScreen";
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
