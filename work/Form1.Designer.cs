namespace work
{
    partial class Form1
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
            menuStrip1 = new MenuStrip();
            dictionaryMenu = new ToolStripMenuItem();
            созданиеСловаряToolStripMenuItem = new ToolStripMenuItem();
            обновлениеСловаряToolStripMenuItem = new ToolStripMenuItem();
            очисткаСловаряToolStripMenuItem = new ToolStripMenuItem();
            fbd = new FolderBrowserDialog();
            menuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // menuStrip1
            // 
            menuStrip1.ImageScalingSize = new Size(24, 24);
            menuStrip1.Items.AddRange(new ToolStripItem[] { dictionaryMenu });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(800, 33);
            menuStrip1.TabIndex = 0;
            menuStrip1.Text = "menuStrip1";
            // 
            // dictionaryMenu
            // 
            dictionaryMenu.DropDownItems.AddRange(new ToolStripItem[] { созданиеСловаряToolStripMenuItem, обновлениеСловаряToolStripMenuItem, очисткаСловаряToolStripMenuItem });
            dictionaryMenu.Name = "dictionaryMenu";
            dictionaryMenu.Size = new Size(98, 29);
            dictionaryMenu.Text = "Словарь";
            // 
            // созданиеСловаряToolStripMenuItem
            // 
            созданиеСловаряToolStripMenuItem.Name = "созданиеСловаряToolStripMenuItem";
            созданиеСловаряToolStripMenuItem.Size = new Size(288, 34);
            созданиеСловаряToolStripMenuItem.Text = "Создание словаря";
            // 
            // обновлениеСловаряToolStripMenuItem
            // 
            обновлениеСловаряToolStripMenuItem.Name = "обновлениеСловаряToolStripMenuItem";
            обновлениеСловаряToolStripMenuItem.Size = new Size(288, 34);
            обновлениеСловаряToolStripMenuItem.Text = "Обновление словаря";
            // 
            // очисткаСловаряToolStripMenuItem
            // 
            очисткаСловаряToolStripMenuItem.Name = "очисткаСловаряToolStripMenuItem";
            очисткаСловаряToolStripMenuItem.Size = new Size(288, 34);
            очисткаСловаряToolStripMenuItem.Text = "Очистка словаря";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(menuStrip1);
            MainMenuStrip = menuStrip1;
            Name = "Form1";
            Text = "Текстовый процессор";
            Load += Form1_Load;
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private MenuStrip menuStrip1;
        private ToolStripMenuItem dictionaryMenu;
        private ToolStripMenuItem созданиеСловаряToolStripMenuItem;
        private ToolStripMenuItem обновлениеСловаряToolStripMenuItem;
        private ToolStripMenuItem очисткаСловаряToolStripMenuItem;
        private FolderBrowserDialog fbd;
    }
}