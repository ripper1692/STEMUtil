namespace STEMUtil
{
    partial class Form1
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
            this.ExtractBox = new System.Windows.Forms.GroupBox();
            this.ExtractOutputTextBox = new System.Windows.Forms.TextBox();
            this.ExtractOutputLabel = new System.Windows.Forms.Label();
            this.ExtractBtn = new System.Windows.Forms.Button();
            this.ExtractOutputChooser = new System.Windows.Forms.Button();
            this.ExtractInputChooser = new System.Windows.Forms.Button();
            this.ExtractInputTextBox = new System.Windows.Forms.TextBox();
            this.ExtractInputLabel = new System.Windows.Forms.Label();
            this.ImportBox = new System.Windows.Forms.GroupBox();
            this.ImportOutputTextBox = new System.Windows.Forms.TextBox();
            this.ImportInputTextBox = new System.Windows.Forms.TextBox();
            this.ImportOutputChooser = new System.Windows.Forms.Button();
            this.ImportBtn = new System.Windows.Forms.Button();
            this.ImportInputChooser = new System.Windows.Forms.Button();
            this.ImportOutputLabel = new System.Windows.Forms.Label();
            this.ImpotInputLabel = new System.Windows.Forms.Label();
            this.progressBar1 = new STEMUtil.MyComponents.FixedProgressBar();
            this.label1 = new System.Windows.Forms.Label();
            this.ExtractBox.SuspendLayout();
            this.ImportBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // ExtractBox
            // 
            this.ExtractBox.Controls.Add(this.ExtractOutputTextBox);
            this.ExtractBox.Controls.Add(this.ExtractOutputLabel);
            this.ExtractBox.Controls.Add(this.ExtractBtn);
            this.ExtractBox.Controls.Add(this.ExtractOutputChooser);
            this.ExtractBox.Controls.Add(this.ExtractInputChooser);
            this.ExtractBox.Controls.Add(this.ExtractInputTextBox);
            this.ExtractBox.Controls.Add(this.ExtractInputLabel);
            this.ExtractBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.ExtractBox.Location = new System.Drawing.Point(10, 10);
            this.ExtractBox.Name = "ExtractBox";
            this.ExtractBox.Size = new System.Drawing.Size(560, 200);
            this.ExtractBox.TabIndex = 4;
            this.ExtractBox.TabStop = false;
            this.ExtractBox.Text = "Extract";
            // 
            // ExtractOutputTextBox
            // 
            this.ExtractOutputTextBox.Location = new System.Drawing.Point(18, 106);
            this.ExtractOutputTextBox.Name = "ExtractOutputTextBox";
            this.ExtractOutputTextBox.Size = new System.Drawing.Size(486, 22);
            this.ExtractOutputTextBox.TabIndex = 6;
            // 
            // ExtractOutputLabel
            // 
            this.ExtractOutputLabel.AutoSize = true;
            this.ExtractOutputLabel.Location = new System.Drawing.Point(15, 87);
            this.ExtractOutputLabel.Name = "ExtractOutputLabel";
            this.ExtractOutputLabel.Size = new System.Drawing.Size(103, 16);
            this.ExtractOutputLabel.TabIndex = 5;
            this.ExtractOutputLabel.Text = "Extract directory";
            // 
            // ExtractBtn
            // 
            this.ExtractBtn.Location = new System.Drawing.Point(466, 164);
            this.ExtractBtn.Name = "ExtractBtn";
            this.ExtractBtn.Size = new System.Drawing.Size(88, 30);
            this.ExtractBtn.TabIndex = 4;
            this.ExtractBtn.Text = "Extract";
            this.ExtractBtn.UseVisualStyleBackColor = true;
            this.ExtractBtn.Click += new System.EventHandler(this.ExtractBtn_Click);
            // 
            // ExtractOutputChooser
            // 
            this.ExtractOutputChooser.Location = new System.Drawing.Point(514, 106);
            this.ExtractOutputChooser.Name = "ExtractOutputChooser";
            this.ExtractOutputChooser.Size = new System.Drawing.Size(40, 22);
            this.ExtractOutputChooser.TabIndex = 3;
            this.ExtractOutputChooser.Text = "...";
            this.ExtractOutputChooser.UseVisualStyleBackColor = true;
            this.ExtractOutputChooser.Click += new System.EventHandler(this.ExtractOutputChooser_Click);
            // 
            // ExtractInputChooser
            // 
            this.ExtractInputChooser.Location = new System.Drawing.Point(514, 49);
            this.ExtractInputChooser.Name = "ExtractInputChooser";
            this.ExtractInputChooser.Size = new System.Drawing.Size(40, 22);
            this.ExtractInputChooser.TabIndex = 2;
            this.ExtractInputChooser.Text = "...";
            this.ExtractInputChooser.UseVisualStyleBackColor = true;
            this.ExtractInputChooser.Click += new System.EventHandler(this.ExtractInputChooser_Click);
            // 
            // ExtractInputTextBox
            // 
            this.ExtractInputTextBox.Location = new System.Drawing.Point(18, 49);
            this.ExtractInputTextBox.Name = "ExtractInputTextBox";
            this.ExtractInputTextBox.Size = new System.Drawing.Size(486, 22);
            this.ExtractInputTextBox.TabIndex = 1;
            // 
            // ExtractInputLabel
            // 
            this.ExtractInputLabel.AutoSize = true;
            this.ExtractInputLabel.Location = new System.Drawing.Point(15, 30);
            this.ExtractInputLabel.Name = "ExtractInputLabel";
            this.ExtractInputLabel.Size = new System.Drawing.Size(61, 16);
            this.ExtractInputLabel.TabIndex = 0;
            this.ExtractInputLabel.Text = "PTR File";
            // 
            // ImportBox
            // 
            this.ImportBox.Controls.Add(this.ImportOutputTextBox);
            this.ImportBox.Controls.Add(this.ImportInputTextBox);
            this.ImportBox.Controls.Add(this.ImportOutputChooser);
            this.ImportBox.Controls.Add(this.ImportBtn);
            this.ImportBox.Controls.Add(this.ImportInputChooser);
            this.ImportBox.Controls.Add(this.ImportOutputLabel);
            this.ImportBox.Controls.Add(this.ImpotInputLabel);
            this.ImportBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.ImportBox.Location = new System.Drawing.Point(10, 218);
            this.ImportBox.Margin = new System.Windows.Forms.Padding(5);
            this.ImportBox.Name = "ImportBox";
            this.ImportBox.Size = new System.Drawing.Size(560, 200);
            this.ImportBox.TabIndex = 5;
            this.ImportBox.TabStop = false;
            this.ImportBox.Text = "Import";
            // 
            // ImportOutputTextBox
            // 
            this.ImportOutputTextBox.Location = new System.Drawing.Point(18, 106);
            this.ImportOutputTextBox.Name = "ImportOutputTextBox";
            this.ImportOutputTextBox.Size = new System.Drawing.Size(486, 22);
            this.ImportOutputTextBox.TabIndex = 6;
            // 
            // ImportInputTextBox
            // 
            this.ImportInputTextBox.Location = new System.Drawing.Point(18, 49);
            this.ImportInputTextBox.Name = "ImportInputTextBox";
            this.ImportInputTextBox.Size = new System.Drawing.Size(486, 22);
            this.ImportInputTextBox.TabIndex = 5;
            // 
            // ImportOutputChooser
            // 
            this.ImportOutputChooser.Location = new System.Drawing.Point(514, 106);
            this.ImportOutputChooser.Name = "ImportOutputChooser";
            this.ImportOutputChooser.Size = new System.Drawing.Size(40, 22);
            this.ImportOutputChooser.TabIndex = 4;
            this.ImportOutputChooser.Text = "...";
            this.ImportOutputChooser.UseVisualStyleBackColor = true;
            this.ImportOutputChooser.Click += new System.EventHandler(this.ImportOutputChooser_Click);
            // 
            // ImportBtn
            // 
            this.ImportBtn.Location = new System.Drawing.Point(466, 164);
            this.ImportBtn.Name = "ImportBtn";
            this.ImportBtn.Size = new System.Drawing.Size(88, 30);
            this.ImportBtn.TabIndex = 3;
            this.ImportBtn.Text = "Import";
            this.ImportBtn.UseVisualStyleBackColor = true;
            this.ImportBtn.Click += new System.EventHandler(this.ImportBtn_Click);
            // 
            // ImportInputChooser
            // 
            this.ImportInputChooser.Location = new System.Drawing.Point(514, 49);
            this.ImportInputChooser.Name = "ImportInputChooser";
            this.ImportInputChooser.Size = new System.Drawing.Size(40, 22);
            this.ImportInputChooser.TabIndex = 2;
            this.ImportInputChooser.Text = "...";
            this.ImportInputChooser.UseVisualStyleBackColor = true;
            this.ImportInputChooser.Click += new System.EventHandler(this.ImportInputChooser_Click);
            // 
            // ImportOutputLabel
            // 
            this.ImportOutputLabel.AutoSize = true;
            this.ImportOutputLabel.Location = new System.Drawing.Point(15, 87);
            this.ImportOutputLabel.Name = "ImportOutputLabel";
            this.ImportOutputLabel.Size = new System.Drawing.Size(92, 16);
            this.ImportOutputLabel.TabIndex = 1;
            this.ImportOutputLabel.Text = "PTR/PKR File";
            // 
            // ImpotInputLabel
            // 
            this.ImpotInputLabel.AutoSize = true;
            this.ImpotInputLabel.Location = new System.Drawing.Point(15, 30);
            this.ImpotInputLabel.Name = "ImpotInputLabel";
            this.ImpotInputLabel.Size = new System.Drawing.Size(92, 16);
            this.ImpotInputLabel.TabIndex = 0;
            this.ImpotInputLabel.Text = "Data directory";
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(10, 474);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(560, 20);
            this.progressBar1.TabIndex = 6;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 439);
            this.label1.MaximumSize = new System.Drawing.Size(560, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(0, 13);
            this.label1.TabIndex = 7;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 506);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.ImportBox);
            this.Controls.Add(this.ExtractBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "Form1";
            this.Text = "STEM Util";
            this.ExtractBox.ResumeLayout(false);
            this.ExtractBox.PerformLayout();
            this.ImportBox.ResumeLayout(false);
            this.ImportBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.GroupBox ExtractBox;
        private System.Windows.Forms.GroupBox ImportBox;
        private System.Windows.Forms.TextBox ExtractOutputTextBox;
        private System.Windows.Forms.Label ExtractOutputLabel;
        private System.Windows.Forms.Button ExtractBtn;
        private System.Windows.Forms.Button ExtractOutputChooser;
        private System.Windows.Forms.Button ExtractInputChooser;
        private System.Windows.Forms.TextBox ExtractInputTextBox;
        private System.Windows.Forms.Label ExtractInputLabel;
        private System.Windows.Forms.TextBox ImportOutputTextBox;
        private System.Windows.Forms.TextBox ImportInputTextBox;
        private System.Windows.Forms.Button ImportOutputChooser;
        private System.Windows.Forms.Button ImportBtn;
        private System.Windows.Forms.Button ImportInputChooser;
        private System.Windows.Forms.Label ImportOutputLabel;
        private System.Windows.Forms.Label ImpotInputLabel;
        private STEMUtil.MyComponents.FixedProgressBar progressBar1;
        private System.Windows.Forms.Label label1;
    }
}

