namespace Stawis {
  partial class Form1 {
    /// <summary>
    /// Erforderliche Designervariable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Verwendete Ressourcen bereinigen.
    /// </summary>
    /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
    protected override void Dispose(bool disposing) {
      if (disposing && (components != null)) {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Vom Windows Form-Designer generierter Code

    /// <summary>
    /// Erforderliche Methode für die Designerunterstützung.
    /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
    /// </summary>
    private void InitializeComponent() {
      this.pnlD1 = new System.Windows.Forms.Panel();
      this.pnlD2 = new System.Windows.Forms.Panel();
      this.pnlC1 = new System.Windows.Forms.Panel();
      this.pnlC3 = new System.Windows.Forms.Panel();
      this.pnlC2 = new System.Windows.Forms.Panel();
      this.pnlChgl = new System.Windows.Forms.Panel();
      this.lstOrders = new System.Windows.Forms.ListView();
      this.colCharge = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.colWeight = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.cbxAlarm = new System.Windows.Forms.ComboBox();
      this.cmdClose = new System.Windows.Forms.Button();
      this.cmdReset = new System.Windows.Forms.Button();
      this.SuspendLayout();
      // 
      // pnlD1
      // 
      this.pnlD1.BackColor = System.Drawing.Color.Transparent;
      this.pnlD1.BackgroundImage = global::Stawis.Properties.Resources.DesEmpty;
      this.pnlD1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
      this.pnlD1.Location = new System.Drawing.Point(34, 194);
      this.pnlD1.Name = "pnlD1";
      this.pnlD1.Size = new System.Drawing.Size(73, 82);
      this.pnlD1.TabIndex = 1;
      // 
      // pnlD2
      // 
      this.pnlD2.BackColor = System.Drawing.Color.Transparent;
      this.pnlD2.BackgroundImage = global::Stawis.Properties.Resources.DesEmpty;
      this.pnlD2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
      this.pnlD2.Location = new System.Drawing.Point(34, 305);
      this.pnlD2.Name = "pnlD2";
      this.pnlD2.Size = new System.Drawing.Size(73, 82);
      this.pnlD2.TabIndex = 2;
      // 
      // pnlC1
      // 
      this.pnlC1.BackColor = System.Drawing.Color.Transparent;
      this.pnlC1.BackgroundImage = global::Stawis.Properties.Resources.ConvEmpty;
      this.pnlC1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
      this.pnlC1.Location = new System.Drawing.Point(341, 54);
      this.pnlC1.Name = "pnlC1";
      this.pnlC1.Size = new System.Drawing.Size(73, 82);
      this.pnlC1.TabIndex = 3;
      // 
      // pnlC3
      // 
      this.pnlC3.BackColor = System.Drawing.Color.Transparent;
      this.pnlC3.BackgroundImage = global::Stawis.Properties.Resources.ConvEmpty;
      this.pnlC3.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
      this.pnlC3.Location = new System.Drawing.Point(624, 54);
      this.pnlC3.Name = "pnlC3";
      this.pnlC3.Size = new System.Drawing.Size(73, 82);
      this.pnlC3.TabIndex = 5;
      // 
      // pnlC2
      // 
      this.pnlC2.BackColor = System.Drawing.Color.Transparent;
      this.pnlC2.BackgroundImage = global::Stawis.Properties.Resources.ConvEmpty;
      this.pnlC2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
      this.pnlC2.Location = new System.Drawing.Point(481, 54);
      this.pnlC2.Name = "pnlC2";
      this.pnlC2.Size = new System.Drawing.Size(73, 82);
      this.pnlC2.TabIndex = 4;
      // 
      // pnlChgl
      // 
      this.pnlChgl.BackColor = System.Drawing.Color.Transparent;
      this.pnlChgl.BackgroundImage = global::Stawis.Properties.Resources.LadleEmpty;
      this.pnlChgl.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
      this.pnlChgl.Location = new System.Drawing.Point(214, 54);
      this.pnlChgl.Name = "pnlChgl";
      this.pnlChgl.Size = new System.Drawing.Size(73, 82);
      this.pnlChgl.TabIndex = 0;
      // 
      // lstOrders
      // 
      this.lstOrders.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colCharge,
            this.colWeight});
      this.lstOrders.GridLines = true;
      this.lstOrders.Location = new System.Drawing.Point(34, 19);
      this.lstOrders.Name = "lstOrders";
      this.lstOrders.Size = new System.Drawing.Size(158, 144);
      this.lstOrders.TabIndex = 6;
      this.lstOrders.UseCompatibleStateImageBehavior = false;
      this.lstOrders.View = System.Windows.Forms.View.Details;
      // 
      // colCharge
      // 
      this.colCharge.Text = "Charge";
      this.colCharge.Width = 68;
      // 
      // colWeight
      // 
      this.colWeight.Text = "Gewicht";
      this.colWeight.Width = 67;
      // 
      // cbxAlarm
      // 
      this.cbxAlarm.FormattingEnabled = true;
      this.cbxAlarm.Location = new System.Drawing.Point(32, 401);
      this.cbxAlarm.Name = "cbxAlarm";
      this.cbxAlarm.Size = new System.Drawing.Size(665, 21);
      this.cbxAlarm.TabIndex = 7;
      this.cbxAlarm.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.cbxAlarm_KeyPress);
      // 
      // cmdClose
      // 
      this.cmdClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.cmdClose.Location = new System.Drawing.Point(624, 357);
      this.cmdClose.Name = "cmdClose";
      this.cmdClose.Size = new System.Drawing.Size(73, 29);
      this.cmdClose.TabIndex = 8;
      this.cmdClose.Text = "Beenden";
      this.cmdClose.UseVisualStyleBackColor = true;
      this.cmdClose.Click += new System.EventHandler(this.cmdClose_Click);
      // 
      // cmdReset
      // 
      this.cmdReset.Location = new System.Drawing.Point(524, 358);
      this.cmdReset.Name = "cmdReset";
      this.cmdReset.Size = new System.Drawing.Size(80, 27);
      this.cmdReset.TabIndex = 9;
      this.cmdReset.Text = "Reset";
      this.cmdReset.UseVisualStyleBackColor = true;
      this.cmdReset.Click += new System.EventHandler(this.cmdReset_Click);
      // 
      // Form1
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(737, 461);
      this.Controls.Add(this.cmdReset);
      this.Controls.Add(this.cmdClose);
      this.Controls.Add(this.cbxAlarm);
      this.Controls.Add(this.lstOrders);
      this.Controls.Add(this.pnlChgl);
      this.Controls.Add(this.pnlC2);
      this.Controls.Add(this.pnlC3);
      this.Controls.Add(this.pnlC1);
      this.Controls.Add(this.pnlD2);
      this.Controls.Add(this.pnlD1);
      this.Name = "Form1";
      this.Text = "Stahlwerksinformationssystem";
      this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
      this.Load += new System.EventHandler(this.Form1_Load);
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.ColumnHeader colCharge;
    private System.Windows.Forms.ColumnHeader colWeight;
    public System.Windows.Forms.Panel pnlD1;
    public System.Windows.Forms.Panel pnlD2;
    public System.Windows.Forms.Panel pnlC1;
    public System.Windows.Forms.Panel pnlC3;
    public System.Windows.Forms.Panel pnlC2;
    public System.Windows.Forms.Panel pnlChgl;
    public System.Windows.Forms.ListView lstOrders;
    public System.Windows.Forms.ComboBox cbxAlarm;
    private System.Windows.Forms.Button cmdClose;
    private System.Windows.Forms.Button cmdReset;
  }
}

