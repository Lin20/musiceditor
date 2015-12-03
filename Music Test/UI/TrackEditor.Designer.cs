namespace Music_Test.UI
{
	partial class TrackEditor
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

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.SuspendLayout();
			// 
			// TrackEditor
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(48)))), ((int)(((byte)(64)))));
			this.DoubleBuffered = true;
			this.Name = "TrackEditor";
			this.Paint += new System.Windows.Forms.PaintEventHandler(this.TrackEditor_Paint);
			this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TrackEditor_KeyDown);
			this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.TrackEditor_KeyUp);
			this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.TrackEditor_MouseDown);
			this.MouseEnter += new System.EventHandler(this.TrackEditor_MouseEnter);
			this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.TrackEditor_MouseMove);
			this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.TrackEditor_MouseUp);
			this.ResumeLayout(false);

		}

		#endregion
	}
}
