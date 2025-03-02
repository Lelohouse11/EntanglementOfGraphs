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
            graph_PictureBox = new PictureBox();
            vertexInput_TextBox = new TextBox();
            headline_Label = new Label();
            vertexInput_Label = new Label();
            newVertex_Button = new Button();
            entanglement_Button = new Button();
            TextOutput_TextBox = new TextBox();
            vertex_Label = new Label();
            edgeFrom_Label = new Label();
            edgeTo_Label = new Label();
            edgeSource_TextBox = new TextBox();
            edgeTarget_TextBox = new TextBox();
            addEdge_Button = new Button();
            deleteGraph_Button = new Button();
            startPosEnt_Label = new Label();
            playGraph_Button = new Button();
            graphCreate_Panel = new Panel();
            deleteEdge_Button = new Button();
            deleteVertex_Button = new Button();
            doNothingDet_Button = new Button();
            moveDetective_Button = new Button();
            movedDet = new TextBox();
            detective_Label = new Label();
            chooseDetPlay_Label = new Label();
            computeEnt_Panel = new Panel();
            chooseDFSOrBFS_ComboBox = new ComboBox();
            startPosPlay_Label = new Label();
            GameSettings_Panel = new Panel();
            playDetective_Button = new Button();
            playThief_Button = new Button();
            detectiveAmountInput_TextBox = new TextBox();
            detectiveAmount_Label = new Label();
            startPosInputPlay_TextBox = new TextBox();
            restartGame_Button = new Button();
            editGraph_Button = new Button();
            detMovement_Panel = new Panel();
            label1_Label = new Label();
            thiefMovement_Panel = new Panel();
            label2_Label = new Label();
            moveThiefToTarget_Button = new Button();
            targetThiefInput_TextBox = new TextBox();
            targetVertexofThief_Label = new Label();
            chooseThiefPlay_Label = new Label();
            TorusCreate_Panel = new Panel();
            torusGraph_Label = new Label();
            torusN_TextBox = new TextBox();
            torusM_TextBox = new TextBox();
            createTorusGraph_Button = new Button();
            x_Label = new Label();
            chooseGraphTyp_ComboBox = new ComboBox();
            createUnCircleGraph_Panel = new Panel();
            unCircleSize_Label = new Label();
            createUndirectedCircleGraph_Button = new Button();
            unCircleSizeInput_TextBox = new TextBox();
            createDiCircleGraph_Panel = new Panel();
            diCircleLabel_Label = new Label();
            createDirectedCircleGraph_Button = new Button();
            diCircleSizeInput_TextBox = new TextBox();
            openFileDialog1 = new OpenFileDialog();
            createFullyConGraph_Panel = new Panel();
            fullyConCreate_Button = new Button();
            fullyConSizeInput_TextBox = new TextBox();
            fullyConSize_Label = new Label();
            createGirdGraph_Panel = new Panel();
            GridGraphCreate_Button = new Button();
            GirdGraphTimes_Label = new Label();
            GridM_TextBox = new TextBox();
            GirdN_TextBox = new TextBox();
            GirdGraphSize_Label = new Label();
            ((System.ComponentModel.ISupportInitialize)graph_PictureBox).BeginInit();
            graphCreate_Panel.SuspendLayout();
            computeEnt_Panel.SuspendLayout();
            GameSettings_Panel.SuspendLayout();
            detMovement_Panel.SuspendLayout();
            thiefMovement_Panel.SuspendLayout();
            TorusCreate_Panel.SuspendLayout();
            createUnCircleGraph_Panel.SuspendLayout();
            createDiCircleGraph_Panel.SuspendLayout();
            createFullyConGraph_Panel.SuspendLayout();
            createGirdGraph_Panel.SuspendLayout();
            SuspendLayout();
            // 
            // graph_PictureBox
            // 
            resources.ApplyResources(graph_PictureBox, "graph_PictureBox");
            graph_PictureBox.BorderStyle = BorderStyle.FixedSingle;
            graph_PictureBox.Name = "graph_PictureBox";
            graph_PictureBox.TabStop = false;
            graph_PictureBox.Paint += GraphPicture_Paint;
            // 
            // vertexInput_TextBox
            // 
            resources.ApplyResources(vertexInput_TextBox, "vertexInput_TextBox");
            vertexInput_TextBox.BackColor = SystemColors.Window;
            vertexInput_TextBox.ForeColor = SystemColors.WindowText;
            vertexInput_TextBox.Name = "vertexInput_TextBox";
            // 
            // headline_Label
            // 
            resources.ApplyResources(headline_Label, "headline_Label");
            headline_Label.ForeColor = SystemColors.WindowText;
            headline_Label.Name = "headline_Label";
            // 
            // vertexInput_Label
            // 
            resources.ApplyResources(vertexInput_Label, "vertexInput_Label");
            vertexInput_Label.ForeColor = SystemColors.WindowText;
            vertexInput_Label.Name = "vertexInput_Label";
            // 
            // newVertex_Button
            // 
            resources.ApplyResources(newVertex_Button, "newVertex_Button");
            newVertex_Button.BackColor = SystemColors.ButtonHighlight;
            newVertex_Button.ForeColor = SystemColors.ControlText;
            newVertex_Button.Name = "newVertex_Button";
            newVertex_Button.UseVisualStyleBackColor = false;
            newVertex_Button.Click += newVertex_Click;
            // 
            // entanglement_Button
            // 
            resources.ApplyResources(entanglement_Button, "entanglement_Button");
            entanglement_Button.BackColor = SystemColors.ButtonHighlight;
            entanglement_Button.ForeColor = SystemColors.ControlText;
            entanglement_Button.Name = "entanglement_Button";
            entanglement_Button.UseVisualStyleBackColor = false;
            entanglement_Button.Click += entanglement_Click;
            // 
            // TextOutput_TextBox
            // 
            resources.ApplyResources(TextOutput_TextBox, "TextOutput_TextBox");
            TextOutput_TextBox.BackColor = SystemColors.Window;
            TextOutput_TextBox.ForeColor = SystemColors.WindowText;
            TextOutput_TextBox.Name = "TextOutput_TextBox";
            TextOutput_TextBox.ReadOnly = true;
            // 
            // vertex_Label
            // 
            resources.ApplyResources(vertex_Label, "vertex_Label");
            vertex_Label.ForeColor = SystemColors.WindowText;
            vertex_Label.Name = "vertex_Label";
            // 
            // edgeFrom_Label
            // 
            resources.ApplyResources(edgeFrom_Label, "edgeFrom_Label");
            edgeFrom_Label.ForeColor = SystemColors.WindowText;
            edgeFrom_Label.Name = "edgeFrom_Label";
            // 
            // edgeTo_Label
            // 
            resources.ApplyResources(edgeTo_Label, "edgeTo_Label");
            edgeTo_Label.ForeColor = SystemColors.WindowText;
            edgeTo_Label.Name = "edgeTo_Label";
            // 
            // edgeSource_TextBox
            // 
            resources.ApplyResources(edgeSource_TextBox, "edgeSource_TextBox");
            edgeSource_TextBox.BackColor = SystemColors.Window;
            edgeSource_TextBox.ForeColor = SystemColors.WindowText;
            edgeSource_TextBox.Name = "edgeSource_TextBox";
            // 
            // edgeTarget_TextBox
            // 
            resources.ApplyResources(edgeTarget_TextBox, "edgeTarget_TextBox");
            edgeTarget_TextBox.BackColor = SystemColors.Window;
            edgeTarget_TextBox.ForeColor = SystemColors.WindowText;
            edgeTarget_TextBox.Name = "edgeTarget_TextBox";
            // 
            // addEdge_Button
            // 
            resources.ApplyResources(addEdge_Button, "addEdge_Button");
            addEdge_Button.BackColor = SystemColors.ButtonHighlight;
            addEdge_Button.ForeColor = SystemColors.ControlText;
            addEdge_Button.Name = "addEdge_Button";
            addEdge_Button.UseVisualStyleBackColor = false;
            addEdge_Button.Click += addEdge_Click;
            // 
            // deleteGraph_Button
            // 
            resources.ApplyResources(deleteGraph_Button, "deleteGraph_Button");
            deleteGraph_Button.BackColor = SystemColors.ButtonHighlight;
            deleteGraph_Button.ForeColor = SystemColors.ControlText;
            deleteGraph_Button.Name = "deleteGraph_Button";
            deleteGraph_Button.UseVisualStyleBackColor = false;
            deleteGraph_Button.Click += deleteGraph_Click;
            // 
            // startPosEnt_Label
            // 
            resources.ApplyResources(startPosEnt_Label, "startPosEnt_Label");
            startPosEnt_Label.ForeColor = SystemColors.WindowText;
            startPosEnt_Label.Name = "startPosEnt_Label";
            // 
            // playGraph_Button
            // 
            resources.ApplyResources(playGraph_Button, "playGraph_Button");
            playGraph_Button.BackColor = SystemColors.ButtonHighlight;
            playGraph_Button.ForeColor = SystemColors.ControlText;
            playGraph_Button.Name = "playGraph_Button";
            playGraph_Button.UseVisualStyleBackColor = false;
            playGraph_Button.Click += playGraph_Click;
            // 
            // graphCreate_Panel
            // 
            resources.ApplyResources(graphCreate_Panel, "graphCreate_Panel");
            graphCreate_Panel.Controls.Add(deleteEdge_Button);
            graphCreate_Panel.Controls.Add(deleteVertex_Button);
            graphCreate_Panel.Controls.Add(vertexInput_Label);
            graphCreate_Panel.Controls.Add(vertexInput_TextBox);
            graphCreate_Panel.Controls.Add(newVertex_Button);
            graphCreate_Panel.Controls.Add(vertex_Label);
            graphCreate_Panel.Controls.Add(edgeFrom_Label);
            graphCreate_Panel.Controls.Add(edgeTo_Label);
            graphCreate_Panel.Controls.Add(edgeSource_TextBox);
            graphCreate_Panel.Controls.Add(edgeTarget_TextBox);
            graphCreate_Panel.Controls.Add(addEdge_Button);
            graphCreate_Panel.Controls.Add(deleteGraph_Button);
            graphCreate_Panel.Name = "graphCreate_Panel";
            // 
            // deleteEdge_Button
            // 
            resources.ApplyResources(deleteEdge_Button, "deleteEdge_Button");
            deleteEdge_Button.BackColor = SystemColors.ButtonHighlight;
            deleteEdge_Button.ForeColor = SystemColors.ControlText;
            deleteEdge_Button.Name = "deleteEdge_Button";
            deleteEdge_Button.UseVisualStyleBackColor = false;
            deleteEdge_Button.Click += deleteEdge_Button_Click;
            // 
            // deleteVertex_Button
            // 
            resources.ApplyResources(deleteVertex_Button, "deleteVertex_Button");
            deleteVertex_Button.BackColor = SystemColors.ButtonHighlight;
            deleteVertex_Button.ForeColor = SystemColors.ControlText;
            deleteVertex_Button.Name = "deleteVertex_Button";
            deleteVertex_Button.UseVisualStyleBackColor = false;
            deleteVertex_Button.Click += deleteVertex_Button_Click;
            // 
            // doNothingDet_Button
            // 
            resources.ApplyResources(doNothingDet_Button, "doNothingDet_Button");
            doNothingDet_Button.BackColor = SystemColors.ButtonHighlight;
            doNothingDet_Button.ForeColor = SystemColors.ControlText;
            doNothingDet_Button.Name = "doNothingDet_Button";
            doNothingDet_Button.UseVisualStyleBackColor = false;
            doNothingDet_Button.Click += doNothingDet_Click;
            // 
            // moveDetective_Button
            // 
            resources.ApplyResources(moveDetective_Button, "moveDetective_Button");
            moveDetective_Button.BackColor = SystemColors.ButtonHighlight;
            moveDetective_Button.ForeColor = SystemColors.ControlText;
            moveDetective_Button.Name = "moveDetective_Button";
            moveDetective_Button.UseVisualStyleBackColor = false;
            moveDetective_Button.Click += moveDetective_Click;
            // 
            // movedDet
            // 
            resources.ApplyResources(movedDet, "movedDet");
            movedDet.BackColor = SystemColors.Window;
            movedDet.ForeColor = SystemColors.WindowText;
            movedDet.Name = "movedDet";
            // 
            // detective_Label
            // 
            resources.ApplyResources(detective_Label, "detective_Label");
            detective_Label.ForeColor = SystemColors.WindowText;
            detective_Label.Name = "detective_Label";
            // 
            // chooseDetPlay_Label
            // 
            resources.ApplyResources(chooseDetPlay_Label, "chooseDetPlay_Label");
            chooseDetPlay_Label.ForeColor = SystemColors.WindowText;
            chooseDetPlay_Label.Name = "chooseDetPlay_Label";
            // 
            // computeEnt_Panel
            // 
            resources.ApplyResources(computeEnt_Panel, "computeEnt_Panel");
            computeEnt_Panel.Controls.Add(chooseDFSOrBFS_ComboBox);
            computeEnt_Panel.Controls.Add(startPosEnt_Label);
            computeEnt_Panel.Controls.Add(entanglement_Button);
            computeEnt_Panel.Name = "computeEnt_Panel";
            // 
            // chooseDFSOrBFS_ComboBox
            // 
            resources.ApplyResources(chooseDFSOrBFS_ComboBox, "chooseFixOrBack_ComboBox");
            chooseDFSOrBFS_ComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            chooseDFSOrBFS_ComboBox.FormattingEnabled = true;
            chooseDFSOrBFS_ComboBox.Items.AddRange(new object[] { resources.GetString("chooseFixOrBack_ComboBox.Items"), resources.GetString("chooseFixOrBack_ComboBox.Items1"), resources.GetString("chooseFixOrBack_ComboBox.Items2") });
            chooseDFSOrBFS_ComboBox.Name = "chooseFixOrBack_ComboBox";
            chooseDFSOrBFS_ComboBox.Tag = "";
            // 
            // startPosPlay_Label
            // 
            resources.ApplyResources(startPosPlay_Label, "startPosPlay_Label");
            startPosPlay_Label.ForeColor = SystemColors.WindowText;
            startPosPlay_Label.Name = "startPosPlay_Label";
            // 
            // GameSettings_Panel
            // 
            resources.ApplyResources(GameSettings_Panel, "GameSettings_Panel");
            GameSettings_Panel.Controls.Add(playDetective_Button);
            GameSettings_Panel.Controls.Add(playThief_Button);
            GameSettings_Panel.Controls.Add(detectiveAmountInput_TextBox);
            GameSettings_Panel.Controls.Add(detectiveAmount_Label);
            GameSettings_Panel.Controls.Add(startPosInputPlay_TextBox);
            GameSettings_Panel.Controls.Add(startPosPlay_Label);
            GameSettings_Panel.Name = "GameSettings_Panel";
            // 
            // playDetective_Button
            // 
            resources.ApplyResources(playDetective_Button, "playDetective_Button");
            playDetective_Button.BackColor = SystemColors.ButtonHighlight;
            playDetective_Button.ForeColor = SystemColors.ControlText;
            playDetective_Button.Name = "playDetective_Button";
            playDetective_Button.UseVisualStyleBackColor = false;
            playDetective_Button.Click += playDetective_Click;
            // 
            // playThief_Button
            // 
            resources.ApplyResources(playThief_Button, "playThief_Button");
            playThief_Button.BackColor = SystemColors.ButtonHighlight;
            playThief_Button.ForeColor = SystemColors.ControlText;
            playThief_Button.Name = "playThief_Button";
            playThief_Button.UseVisualStyleBackColor = false;
            playThief_Button.Click += playThief_Click;
            // 
            // detectiveAmountInput_TextBox
            // 
            resources.ApplyResources(detectiveAmountInput_TextBox, "detectiveAmountInput_TextBox");
            detectiveAmountInput_TextBox.BackColor = SystemColors.Window;
            detectiveAmountInput_TextBox.ForeColor = SystemColors.WindowText;
            detectiveAmountInput_TextBox.Name = "detectiveAmountInput_TextBox";
            // 
            // detectiveAmount_Label
            // 
            resources.ApplyResources(detectiveAmount_Label, "detectiveAmount_Label");
            detectiveAmount_Label.ForeColor = SystemColors.WindowText;
            detectiveAmount_Label.Name = "detectiveAmount_Label";
            // 
            // startPosInputPlay_TextBox
            // 
            resources.ApplyResources(startPosInputPlay_TextBox, "startPosInputPlay_TextBox");
            startPosInputPlay_TextBox.BackColor = SystemColors.Window;
            startPosInputPlay_TextBox.ForeColor = SystemColors.WindowText;
            startPosInputPlay_TextBox.Name = "startPosInputPlay_TextBox";
            // 
            // restartGame_Button
            // 
            resources.ApplyResources(restartGame_Button, "restartGame_Button");
            restartGame_Button.Name = "restartGame_Button";
            restartGame_Button.UseVisualStyleBackColor = true;
            restartGame_Button.Click += restartGame_Click_1;
            // 
            // editGraph_Button
            // 
            resources.ApplyResources(editGraph_Button, "editGraph_Button");
            editGraph_Button.BackColor = SystemColors.ButtonHighlight;
            editGraph_Button.ForeColor = SystemColors.ControlText;
            editGraph_Button.Name = "editGraph_Button";
            editGraph_Button.UseVisualStyleBackColor = false;
            editGraph_Button.Click += editGraph_Click;
            // 
            // detMovement_Panel
            // 
            resources.ApplyResources(detMovement_Panel, "detMovement_Panel");
            detMovement_Panel.Controls.Add(label1_Label);
            detMovement_Panel.Controls.Add(doNothingDet_Button);
            detMovement_Panel.Controls.Add(chooseDetPlay_Label);
            detMovement_Panel.Controls.Add(moveDetective_Button);
            detMovement_Panel.Controls.Add(movedDet);
            detMovement_Panel.Controls.Add(detective_Label);
            detMovement_Panel.Name = "detMovement_Panel";
            // 
            // label1_Label
            // 
            resources.ApplyResources(label1_Label, "label1_Label");
            label1_Label.Name = "label1_Label";
            // 
            // thiefMovement_Panel
            // 
            resources.ApplyResources(thiefMovement_Panel, "thiefMovement_Panel");
            thiefMovement_Panel.Controls.Add(label2_Label);
            thiefMovement_Panel.Controls.Add(moveThiefToTarget_Button);
            thiefMovement_Panel.Controls.Add(targetThiefInput_TextBox);
            thiefMovement_Panel.Controls.Add(targetVertexofThief_Label);
            thiefMovement_Panel.Controls.Add(chooseThiefPlay_Label);
            thiefMovement_Panel.Name = "thiefMovement_Panel";
            // 
            // label2_Label
            // 
            resources.ApplyResources(label2_Label, "label2_Label");
            label2_Label.Name = "label2_Label";
            // 
            // moveThiefToTarget_Button
            // 
            resources.ApplyResources(moveThiefToTarget_Button, "moveThiefToTarget_Button");
            moveThiefToTarget_Button.Name = "moveThiefToTarget_Button";
            moveThiefToTarget_Button.UseVisualStyleBackColor = true;
            moveThiefToTarget_Button.Click += moveThiefToTarget_Click;
            // 
            // targetThiefInput_TextBox
            // 
            resources.ApplyResources(targetThiefInput_TextBox, "targetThiefInput_TextBox");
            targetThiefInput_TextBox.Name = "targetThiefInput_TextBox";
            // 
            // targetVertexofThief_Label
            // 
            resources.ApplyResources(targetVertexofThief_Label, "targetVertexofThief_Label");
            targetVertexofThief_Label.Name = "targetVertexofThief_Label";
            // 
            // chooseThiefPlay_Label
            // 
            resources.ApplyResources(chooseThiefPlay_Label, "chooseThiefPlay_Label");
            chooseThiefPlay_Label.Name = "chooseThiefPlay_Label";
            // 
            // TorusCreate_Panel
            // 
            resources.ApplyResources(TorusCreate_Panel, "TorusCreate_Panel");
            TorusCreate_Panel.Controls.Add(torusGraph_Label);
            TorusCreate_Panel.Controls.Add(torusN_TextBox);
            TorusCreate_Panel.Controls.Add(torusM_TextBox);
            TorusCreate_Panel.Controls.Add(createTorusGraph_Button);
            TorusCreate_Panel.Controls.Add(x_Label);
            TorusCreate_Panel.Name = "TorusCreate_Panel";
            // 
            // torusGraph_Label
            // 
            resources.ApplyResources(torusGraph_Label, "torusGraph_Label");
            torusGraph_Label.ForeColor = SystemColors.WindowText;
            torusGraph_Label.Name = "torusGraph_Label";
            // 
            // torusN_TextBox
            // 
            resources.ApplyResources(torusN_TextBox, "torusN_TextBox");
            torusN_TextBox.BackColor = SystemColors.Window;
            torusN_TextBox.ForeColor = SystemColors.WindowText;
            torusN_TextBox.Name = "torusN_TextBox";
            // 
            // torusM_TextBox
            // 
            resources.ApplyResources(torusM_TextBox, "torusM_TextBox");
            torusM_TextBox.BackColor = SystemColors.Window;
            torusM_TextBox.ForeColor = SystemColors.WindowText;
            torusM_TextBox.Name = "torusM_TextBox";
            // 
            // createTorusGraph_Button
            // 
            resources.ApplyResources(createTorusGraph_Button, "createTorusGraph");
            createTorusGraph_Button.BackColor = SystemColors.ButtonHighlight;
            createTorusGraph_Button.ForeColor = SystemColors.ControlText;
            createTorusGraph_Button.Name = "createTorusGraph";
            createTorusGraph_Button.UseVisualStyleBackColor = false;
            createTorusGraph_Button.Click += createTorusGraph_Click;
            // 
            // x_Label
            // 
            resources.ApplyResources(x_Label, "x_Label");
            x_Label.ForeColor = SystemColors.WindowText;
            x_Label.Name = "x_Label";
            // 
            // chooseGraphTyp_ComboBox
            // 
            resources.ApplyResources(chooseGraphTyp_ComboBox, "chooseGraphTyp_ComboBox");
            chooseGraphTyp_ComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            chooseGraphTyp_ComboBox.FormattingEnabled = true;
            chooseGraphTyp_ComboBox.Items.AddRange(new object[] { resources.GetString("chooseGraphTyp_ComboBox.Items"), resources.GetString("chooseGraphTyp_ComboBox.Items1"), resources.GetString("chooseGraphTyp_ComboBox.Items2"), resources.GetString("chooseGraphTyp_ComboBox.Items3"), resources.GetString("chooseGraphTyp_ComboBox.Items4"), resources.GetString("chooseGraphTyp_ComboBox.Items5") });
            chooseGraphTyp_ComboBox.Name = "chooseGraphTyp_ComboBox";
            chooseGraphTyp_ComboBox.SelectedIndexChanged += chooseGraphTyp_SelectedIndexChanged;
            // 
            // createUnCircleGraph_Panel
            // 
            resources.ApplyResources(createUnCircleGraph_Panel, "createUnCircleGraph_Panel");
            createUnCircleGraph_Panel.Controls.Add(unCircleSize_Label);
            createUnCircleGraph_Panel.Controls.Add(createUndirectedCircleGraph_Button);
            createUnCircleGraph_Panel.Controls.Add(unCircleSizeInput_TextBox);
            createUnCircleGraph_Panel.Name = "createUnCircleGraph_Panel";
            // 
            // unCircleSize_Label
            // 
            resources.ApplyResources(unCircleSize_Label, "unCircleSize_Label");
            unCircleSize_Label.Name = "unCircleSize_Label";
            // 
            // createUndirectedCircleGraph_Button
            // 
            resources.ApplyResources(createUndirectedCircleGraph_Button, "createUndirectedCircleGraph_Button");
            createUndirectedCircleGraph_Button.Name = "createUndirectedCircleGraph_Button";
            createUndirectedCircleGraph_Button.UseVisualStyleBackColor = true;
            createUndirectedCircleGraph_Button.Click += createUndirectedCircleGraph_Click;
            // 
            // unCircleSizeInput_TextBox
            // 
            resources.ApplyResources(unCircleSizeInput_TextBox, "unCircleSizeInput_TextBox");
            unCircleSizeInput_TextBox.Name = "unCircleSizeInput_TextBox";
            // 
            // createDiCircleGraph_Panel
            // 
            resources.ApplyResources(createDiCircleGraph_Panel, "createDiCircleGraph_Panel");
            createDiCircleGraph_Panel.Controls.Add(diCircleLabel_Label);
            createDiCircleGraph_Panel.Controls.Add(createDirectedCircleGraph_Button);
            createDiCircleGraph_Panel.Controls.Add(diCircleSizeInput_TextBox);
            createDiCircleGraph_Panel.Name = "createDiCircleGraph_Panel";
            // 
            // diCircleLabel_Label
            // 
            resources.ApplyResources(diCircleLabel_Label, "diCircleLabel_Label");
            diCircleLabel_Label.Name = "diCircleLabel_Label";
            // 
            // createDirectedCircleGraph_Button
            // 
            resources.ApplyResources(createDirectedCircleGraph_Button, "createDirectedCircleGraph_Button");
            createDirectedCircleGraph_Button.Name = "createDirectedCircleGraph_Button";
            createDirectedCircleGraph_Button.UseVisualStyleBackColor = true;
            createDirectedCircleGraph_Button.Click += createDirectedCircleGraph_Click;
            // 
            // diCircleSizeInput_TextBox
            // 
            resources.ApplyResources(diCircleSizeInput_TextBox, "diCircleSizeInput_TextBox");
            diCircleSizeInput_TextBox.Name = "diCircleSizeInput_TextBox";
            // 
            // openFileDialog1
            // 
            openFileDialog1.FileName = "openFileDialog1";
            resources.ApplyResources(openFileDialog1, "openFileDialog1");
            // 
            // createFullyConGraph_Panel
            // 
            resources.ApplyResources(createFullyConGraph_Panel, "createFullyConGraph_Panel");
            createFullyConGraph_Panel.Controls.Add(fullyConCreate_Button);
            createFullyConGraph_Panel.Controls.Add(fullyConSizeInput_TextBox);
            createFullyConGraph_Panel.Controls.Add(fullyConSize_Label);
            createFullyConGraph_Panel.Name = "createFullyConGraph_Panel";
            // 
            // fullyConCreate_Button
            // 
            resources.ApplyResources(fullyConCreate_Button, "fullyConCreate_Button");
            fullyConCreate_Button.Name = "fullyConCreate_Button";
            fullyConCreate_Button.UseVisualStyleBackColor = true;
            fullyConCreate_Button.Click += fullyConCreate_Click;
            // 
            // fullyConSizeInput_TextBox
            // 
            resources.ApplyResources(fullyConSizeInput_TextBox, "fullyConSizeInput_TextBox");
            fullyConSizeInput_TextBox.Name = "fullyConSizeInput_TextBox";
            // 
            // fullyConSize_Label
            // 
            resources.ApplyResources(fullyConSize_Label, "fullyConSize_Label");
            fullyConSize_Label.Name = "fullyConSize_Label";
            // 
            // createGirdGraph_Panel
            // 
            resources.ApplyResources(createGirdGraph_Panel, "createGirdGraph_Panel");
            createGirdGraph_Panel.Controls.Add(GridGraphCreate_Button);
            createGirdGraph_Panel.Controls.Add(GirdGraphTimes_Label);
            createGirdGraph_Panel.Controls.Add(GridM_TextBox);
            createGirdGraph_Panel.Controls.Add(GirdN_TextBox);
            createGirdGraph_Panel.Controls.Add(GirdGraphSize_Label);
            createGirdGraph_Panel.Name = "createGirdGraph_Panel";
            // 
            // GridGraphCreate_Button
            // 
            resources.ApplyResources(GridGraphCreate_Button, "GridGraphCreate_Button");
            GridGraphCreate_Button.Name = "GridGraphCreate_Button";
            GridGraphCreate_Button.UseVisualStyleBackColor = true;
            GridGraphCreate_Button.Click += GridGraphCreate_Button_Click;
            // 
            // GirdGraphTimes_Label
            // 
            resources.ApplyResources(GirdGraphTimes_Label, "GirdGraphTimes_Label");
            GirdGraphTimes_Label.Name = "GirdGraphTimes_Label";
            // 
            // GridM_TextBox
            // 
            resources.ApplyResources(GridM_TextBox, "GridM_TextBox");
            GridM_TextBox.Name = "GridM_TextBox";
            // 
            // GirdN_TextBox
            // 
            resources.ApplyResources(GirdN_TextBox, "GirdN_TextBox");
            GirdN_TextBox.Name = "GirdN_TextBox";
            // 
            // GirdGraphSize_Label
            // 
            resources.ApplyResources(GirdGraphSize_Label, "GirdGraphSize_Label");
            GirdGraphSize_Label.Name = "GirdGraphSize_Label";
            // 
            // MainScreen
            // 
            resources.ApplyResources(this, "$this");
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.Window;
            Controls.Add(createGirdGraph_Panel);
            Controls.Add(graphCreate_Panel);
            Controls.Add(createFullyConGraph_Panel);
            Controls.Add(createUnCircleGraph_Panel);
            Controls.Add(createDiCircleGraph_Panel);
            Controls.Add(restartGame_Button);
            Controls.Add(detMovement_Panel);
            Controls.Add(graph_PictureBox);
            Controls.Add(editGraph_Button);
            Controls.Add(computeEnt_Panel);
            Controls.Add(playGraph_Button);
            Controls.Add(TextOutput_TextBox);
            Controls.Add(headline_Label);
            Controls.Add(GameSettings_Panel);
            Controls.Add(chooseGraphTyp_ComboBox);
            Controls.Add(TorusCreate_Panel);
            Controls.Add(thiefMovement_Panel);
            Name = "MainScreen";
            ((System.ComponentModel.ISupportInitialize)graph_PictureBox).EndInit();
            graphCreate_Panel.ResumeLayout(false);
            graphCreate_Panel.PerformLayout();
            computeEnt_Panel.ResumeLayout(false);
            GameSettings_Panel.ResumeLayout(false);
            GameSettings_Panel.PerformLayout();
            detMovement_Panel.ResumeLayout(false);
            detMovement_Panel.PerformLayout();
            thiefMovement_Panel.ResumeLayout(false);
            thiefMovement_Panel.PerformLayout();
            TorusCreate_Panel.ResumeLayout(false);
            TorusCreate_Panel.PerformLayout();
            createUnCircleGraph_Panel.ResumeLayout(false);
            createUnCircleGraph_Panel.PerformLayout();
            createDiCircleGraph_Panel.ResumeLayout(false);
            createDiCircleGraph_Panel.PerformLayout();
            createFullyConGraph_Panel.ResumeLayout(false);
            createFullyConGraph_Panel.PerformLayout();
            createGirdGraph_Panel.ResumeLayout(false);
            createGirdGraph_Panel.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private PictureBox graph_PictureBox;
        private TextBox vertexInput_TextBox;
        private Label headline_Label;
        private Label vertexInput_Label;
        private Button newVertex_Button;
        private Button entanglement_Button;
        private TextBox TextOutput_TextBox;
        private Label vertex_Label;
        private Label edgeFrom_Label;
        private Label edgeTo_Label;
        private TextBox edgeSource_TextBox;
        private TextBox edgeTarget_TextBox;
        private Button addEdge_Button;
        private Button deleteGraph_Button;
        private Label startPosEnt_Label;
        private Button playGraph_Button;
        private Panel graphCreate_Panel;
        private Panel computeEnt_Panel;
        private Label startPosPlay_Label;
        private Panel GameSettings_Panel;
        private Label detectiveAmount_Label;
        private TextBox startPosInputPlay_TextBox;
        private Button playDetective_Button;
        private Button playThief_Button;
        private TextBox detectiveAmountInput_TextBox;
        private Button editGraph_Button;
        private Label chooseDetPlay_Label;
        private TextBox movedDet;
        private Label detective_Label;
        private Button moveDetective_Button;
        private Button doNothingDet_Button;
        private Panel detMovement_Panel;
        private Button restartGame_Button;
        private Label label1_Label;
        private Panel thiefMovement_Panel;
        private Label targetVertexofThief_Label;
        private Label chooseThiefPlay_Label;
        private Label label2_Label;
        private Button moveThiefToTarget_Button;
        private TextBox targetThiefInput_TextBox;
        private Panel TorusCreate_Panel;
        private Label torusGraph_Label;
        private TextBox torusN_TextBox;
        private TextBox torusM_TextBox;
        private Button createTorusGraph_Button;
        private Label x_Label;
        private ComboBox chooseGraphTyp_ComboBox;
        private Panel createUnCircleGraph_Panel;
        private Button createUndirectedCircleGraph_Button;
        private TextBox unCircleSizeInput_TextBox;
        private Label unCircleSize_Label;
        private Panel createDiCircleGraph_Panel;
        private Button createDirectedCircleGraph_Button;
        private TextBox diCircleSizeInput_TextBox;
        private Label diCircleLabel_Label;
        private OpenFileDialog openFileDialog1;
        private Panel createFullyConGraph_Panel;
        private Button fullyConCreate_Button;
        private TextBox fullyConSizeInput_TextBox;
        private Label fullyConSize_Label;
        private ComboBox chooseDFSOrBFS_ComboBox;
        private Button deleteVertex_Button;
        private Button deleteEdge_Button;
        private Panel createGirdGraph_Panel;
        private Button GridGraphCreate_Button;
        private Label GirdGraphTimes_Label;
        private TextBox GridM_TextBox;
        private TextBox GirdN_TextBox;
        private Label GirdGraphSize_Label;
    }
}
