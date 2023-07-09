namespace Winsweeper
{
    partial class GameWindow
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
            scoresBtn = new Button();
            SuspendLayout();
            // 
            // scoresBtn
            // 
            scoresBtn.BackgroundImage = Properties.Resources.Cool;
            scoresBtn.BackgroundImageLayout = ImageLayout.Zoom;
            scoresBtn.Location = new Point(65, 12);
            scoresBtn.Name = "scoresBtn";
            scoresBtn.Size = new Size(32, 32);
            scoresBtn.TabIndex = 0;
            scoresBtn.UseVisualStyleBackColor = true;
            scoresBtn.Click += scoresBtn_Click;
            // 
            // GameWindow
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(683, 609);
            Controls.Add(scoresBtn);
            FormBorderStyle = FormBorderStyle.Fixed3D;
            MaximizeBox = false;
            Name = "GameWindow";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Minesweeper";
            ResumeLayout(false);
        }

        #endregion

        private Button scoresBtn;
    }
}