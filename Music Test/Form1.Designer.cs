namespace Music_Test
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
			this.components = new System.ComponentModel.Container();
			this.tmrLine = new System.Windows.Forms.Timer(this.components);
			this.trackEditor1 = new Music_Test.UI.TrackEditor();
			this.keyDisplay1 = new Music_Test.UI.KeyDisplay();
			this.SuspendLayout();
			// 
			// tmrLine
			// 
			this.tmrLine.Tick += new System.EventHandler(this.tmrLine_Tick);
			// 
			// trackEditor1
			// 
			this.trackEditor1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(48)))), ((int)(((byte)(64)))));
			this.trackEditor1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.trackEditor1.Location = new System.Drawing.Point(14, 116);
			this.trackEditor1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.trackEditor1.MouseAction = Music_Test.UI.MouseActions.None;
			this.trackEditor1.Name = "trackEditor1";
			this.trackEditor1.NoteBackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(48)))), ((int)(((byte)(64)))));
			this.trackEditor1.NoteBackgroundHighlightColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(48)))), ((int)(((byte)(96)))));
			this.trackEditor1.Playing = false;
			this.trackEditor1.Size = new System.Drawing.Size(1236, 552);
			this.trackEditor1.Step = 0;
			this.trackEditor1.TabIndex = 1;
			this.trackEditor1.Track = null;
			this.trackEditor1.VerticalScroll = 896;
			this.trackEditor1.Zoom = 1D;
			this.trackEditor1.Click += new System.EventHandler(this.trackEditor1_Click);
			// 
			// keyDisplay1
			// 
			this.keyDisplay1.BackColor = System.Drawing.Color.WhiteSmoke;
			this.keyDisplay1.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.keyDisplay1.Key = null;
			this.keyDisplay1.Location = new System.Drawing.Point(14, 13);
			this.keyDisplay1.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
			this.keyDisplay1.Name = "keyDisplay1";
			this.keyDisplay1.Size = new System.Drawing.Size(1236, 98);
			this.keyDisplay1.TabIndex = 0;
			this.keyDisplay1.Click += new System.EventHandler(this.keyDisplay1_Click);
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1264, 682);
			this.Controls.Add(this.trackEditor1);
			this.Controls.Add(this.keyDisplay1);
			this.KeyPreview = true;
			this.Name = "Form1";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Music Magic Tool";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
			this.Load += new System.EventHandler(this.Form1_Load);
			this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
			this.ResumeLayout(false);

		}

		#endregion

		private UI.KeyDisplay keyDisplay1;
		private UI.TrackEditor trackEditor1;
		private System.Windows.Forms.Timer tmrLine;
	}
}

