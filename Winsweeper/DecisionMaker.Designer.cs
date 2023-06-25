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
            this.decisionFlp = new System.Windows.Forms.FlowLayoutPanel();
            this.goBtn = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.levelChooserNud = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.levelChooserNud)).BeginInit();
            this.SuspendLayout();
            // 
            // decisionFlp
            // 
            this.decisionFlp.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.decisionFlp.Location = new System.Drawing.Point(12, 12);
            this.decisionFlp.Name = "decisionFlp";
            this.decisionFlp.Size = new System.Drawing.Size(474, 292);
            this.decisionFlp.TabIndex = 0;
            // 
            // goBtn
            // 
            this.goBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.goBtn.Location = new System.Drawing.Point(332, 310);
            this.goBtn.Name = "goBtn";
            this.goBtn.Size = new System.Drawing.Size(154, 49);
            this.goBtn.TabIndex = 1;
            this.goBtn.Text = "Let\'s Play";
            this.goBtn.UseVisualStyleBackColor = true;
            this.goBtn.Click += new System.EventHandler(this.goBtn_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 327);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(175, 15);
            this.label1.TabIndex = 2;
            this.label1.Text = "Or enter a level between 1 and 9";
            // 
            // levelChooserNud
            // 
            this.levelChooserNud.Location = new System.Drawing.Point(193, 324);
            this.levelChooserNud.Maximum = new decimal(new int[] {
            9,
            0,
            0,
            0});
            this.levelChooserNud.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.levelChooserNud.Name = "levelChooserNud";
            this.levelChooserNud.Size = new System.Drawing.Size(73, 23);
            this.levelChooserNud.TabIndex = 3;
            this.levelChooserNud.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // DecisionMaker
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(498, 371);
            this.Controls.Add(this.levelChooserNud);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.goBtn);
            this.Controls.Add(this.decisionFlp);
            this.Name = "DecisionMaker";
            this.Text = "Minesweeper - Select Level";
            ((System.ComponentModel.ISupportInitialize)(this.levelChooserNud)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private FlowLayoutPanel decisionFlp;
        private Button goBtn;
        private Label label1;
        private NumericUpDown levelChooserNud;
    }
}