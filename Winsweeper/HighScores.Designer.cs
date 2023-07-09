namespace Winsweeper
{
    partial class HighScores
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
            groupBox1 = new GroupBox();
            scoresFlp = new FlowLayoutPanel();
            label1 = new Label();
            hPlayerLbl = new AuraLabel();
            clearBtn = new Button();
            groupBox1.SuspendLayout();
            SuspendLayout();
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(scoresFlp);
            groupBox1.Location = new Point(12, 129);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(558, 252);
            groupBox1.TabIndex = 0;
            groupBox1.TabStop = false;
            groupBox1.Text = "Scores";
            // 
            // scoresFlp
            // 
            scoresFlp.Dock = DockStyle.Fill;
            scoresFlp.Location = new Point(3, 19);
            scoresFlp.Name = "scoresFlp";
            scoresFlp.Size = new Size(552, 230);
            scoresFlp.TabIndex = 0;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(15, 57);
            label1.Name = "label1";
            label1.Size = new Size(65, 15);
            label1.TabIndex = 1;
            label1.Text = "High Score";
            // 
            // hPlayerLbl
            // 
            hPlayerLbl.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            hPlayerLbl.BackColor = Color.FromArgb(190, 240, 240, 240);
            hPlayerLbl.Location = new Point(83, 38);
            hPlayerLbl.Name = "hPlayerLbl";
            hPlayerLbl.Size = new Size(487, 51);
            hPlayerLbl.TabIndex = 2;
            hPlayerLbl.Text = "auraLabel1";
            // 
            // clearBtn
            // 
            clearBtn.Location = new Point(15, 387);
            clearBtn.Name = "clearBtn";
            clearBtn.Size = new Size(100, 23);
            clearBtn.TabIndex = 3;
            clearBtn.Text = "Clear Scores";
            clearBtn.UseVisualStyleBackColor = true;
            clearBtn.Click += clearBtn_Click;
            // 
            // HighScores
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(582, 422);
            Controls.Add(clearBtn);
            Controls.Add(hPlayerLbl);
            Controls.Add(label1);
            Controls.Add(groupBox1);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "HighScores";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Minesweeper Scores";
            Load += HighScores_Load;
            groupBox1.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private GroupBox groupBox1;
        private Label label1;
        private FlowLayoutPanel scoresFlp;
        private AuraLabel hPlayerLbl;
        private Button clearBtn;
    }
}