namespace Winsweeper
{
    partial class DecisionMaker
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            decisionFlp = new FlowLayoutPanel();
            goBtn = new Button();
            label1 = new Label();
            levelChooserNud = new NumericUpDown();
            boardSizeFlp = new FlowLayoutPanel();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            playerNameTxt = new TextBox();
            hScoreBtn = new Button();
            ((System.ComponentModel.ISupportInitialize)levelChooserNud).BeginInit();
            SuspendLayout();
            // 
            // decisionFlp
            // 
            decisionFlp.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            decisionFlp.BorderStyle = BorderStyle.Fixed3D;
            decisionFlp.Location = new Point(12, 78);
            decisionFlp.Name = "decisionFlp";
            decisionFlp.Size = new Size(236, 279);
            decisionFlp.TabIndex = 0;
            // 
            // goBtn
            // 
            goBtn.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            goBtn.Location = new Point(332, 363);
            goBtn.Name = "goBtn";
            goBtn.Size = new Size(154, 49);
            goBtn.TabIndex = 1;
            goBtn.Text = "Let's Play";
            goBtn.UseVisualStyleBackColor = true;
            goBtn.Click += goBtn_Click;
            // 
            // label1
            // 
            label1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            label1.AutoSize = true;
            label1.Location = new Point(12, 380);
            label1.Name = "label1";
            label1.Size = new Size(175, 15);
            label1.TabIndex = 2;
            label1.Text = "Or enter a level between 1 and 9";
            // 
            // levelChooserNud
            // 
            levelChooserNud.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            levelChooserNud.Location = new Point(193, 377);
            levelChooserNud.Maximum = new decimal(new int[] { 9, 0, 0, 0 });
            levelChooserNud.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            levelChooserNud.Name = "levelChooserNud";
            levelChooserNud.Size = new Size(73, 23);
            levelChooserNud.TabIndex = 3;
            levelChooserNud.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // boardSizeFlp
            // 
            boardSizeFlp.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            boardSizeFlp.BorderStyle = BorderStyle.Fixed3D;
            boardSizeFlp.Location = new Point(254, 78);
            boardSizeFlp.Name = "boardSizeFlp";
            boardSizeFlp.Size = new Size(232, 279);
            boardSizeFlp.TabIndex = 0;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(12, 60);
            label2.Name = "label2";
            label2.Size = new Size(88, 15);
            label2.TabIndex = 4;
            label2.Text = "Level Difficulty:";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(254, 60);
            label3.Name = "label3";
            label3.Size = new Size(64, 15);
            label3.TabIndex = 4;
            label3.Text = "Board Size:";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(12, 15);
            label4.Name = "label4";
            label4.Size = new Size(69, 15);
            label4.TabIndex = 5;
            label4.Text = "Your Name:";
            // 
            // playerNameTxt
            // 
            playerNameTxt.Location = new Point(87, 12);
            playerNameTxt.Name = "playerNameTxt";
            playerNameTxt.Size = new Size(231, 23);
            playerNameTxt.TabIndex = 6;
            // 
            // hScoreBtn
            // 
            hScoreBtn.Location = new Point(357, 4);
            hScoreBtn.Name = "hScoreBtn";
            hScoreBtn.Size = new Size(129, 37);
            hScoreBtn.TabIndex = 7;
            hScoreBtn.Text = "High Scores";
            hScoreBtn.UseVisualStyleBackColor = true;
            hScoreBtn.Click += hScoreBtn_Click;
            // 
            // DecisionMaker
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(498, 424);
            Controls.Add(hScoreBtn);
            Controls.Add(playerNameTxt);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(levelChooserNud);
            Controls.Add(label1);
            Controls.Add(goBtn);
            Controls.Add(boardSizeFlp);
            Controls.Add(decisionFlp);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "DecisionMaker";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Minesweeper - Select Level";
            ((System.ComponentModel.ISupportInitialize)levelChooserNud).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private FlowLayoutPanel decisionFlp;
        private Button goBtn;
        private Label label1;
        private NumericUpDown levelChooserNud;
        private FlowLayoutPanel boardSizeFlp;
        private Label label2;
        private Label label3;
        private Label label4;
        private TextBox playerNameTxt;
        private Button hScoreBtn;
    }
}