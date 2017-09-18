namespace M4.M4v2.Chart
{
    partial class TelerickCtlChart
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        ///

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ctlChart));
            this.StockChartX1 = new AxSTOCKCHARTXLib.AxStockChartX();
            this.tmrEdit = new System.Windows.Forms.Timer(this.components);
            this.StockChartX2 = new AxSTOCKCHARTXLib.AxStockChartX();
            this.Label2 = new System.Windows.Forms.Label();
            this.Label1 = new System.Windows.Forms.Label();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.pnlConsensus = new System.Windows.Forms.Panel();
            this.guage1 = new AGaugeApp.AGauge();
            this.pnlTwitter = new System.Windows.Forms.Panel();
            this.TwitterTimelineControl = new System.Windows.Forms.FlowLayoutPanel();
            this.UserNameLabel = new System.Windows.Forms.Label();
            this.pnlTwitterControls = new System.Windows.Forms.Panel();
            this.lblTweetSize = new System.Windows.Forms.Label();
            this.txtTweet = new System.Windows.Forms.TextBox();
            this.chkTwitter = new System.Windows.Forms.CheckBox();
            this.tmrAttention = new System.Windows.Forms.Timer(this.components);
            this.pnlTwitterAuthorize = new System.Windows.Forms.Panel();
            this.lblPin = new System.Windows.Forms.Label();
            this.cmdSave = new System.Windows.Forms.Button();
            this.txtPin = new System.Windows.Forms.TextBox();
            this.webBrowser1 = new System.Windows.Forms.WebBrowser();
            this.picHighlight = new System.Windows.Forms.PictureBox();
            this.userPictureBox = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.cmdEAs = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.StockChartX1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.StockChartX2)).BeginInit();
            this.pnlConsensus.SuspendLayout();
            this.pnlTwitter.SuspendLayout();
            this.pnlTwitterControls.SuspendLayout();
            this.pnlTwitterAuthorize.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picHighlight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.userPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();





            // 
            // StockChartX1
            // 
            this.StockChartX1.Enabled = true;
            this.StockChartX1.Location = new System.Drawing.Point(0, 0);
            this.StockChartX1.Name = "StockChartX1";
            this.StockChartX1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("StockChartX1.OcxState")));
            this.StockChartX1.Size = new System.Drawing.Size(495, 519);
            this.StockChartX1.TabIndex = 1;
            this.StockChartX1.MouseMoveEvent += new AxSTOCKCHARTXLib._DStockChartXEvents_MouseMoveEventHandler(this.StockChartX1_MouseMoveEvent);
            this.StockChartX1.PaintEvent += new AxSTOCKCHARTXLib._DStockChartXEvents_PaintEventHandler(this.StockChartX1PaintEvent);
            this.StockChartX1.ItemRightClick += new AxSTOCKCHARTXLib._DStockChartXEvents_ItemRightClickEventHandler(this.StockChartX1_ItemRightClick);
            this.StockChartX1.OnLButtonDown += new System.EventHandler(this.StockChartX1_OnLButtonDown);
            this.StockChartX1.OnLButtonUp += new System.EventHandler(this.StockChartX1_OnLButtonUp);
            this.StockChartX1.ShowDialog += new System.EventHandler(this.StockChartX1ShowDialog);
            this.StockChartX1.HideDialog += new System.EventHandler(this.StockChartX1HideDialog);
            this.StockChartX1.EnumIndicator += new AxSTOCKCHARTXLib._DStockChartXEvents_EnumIndicatorEventHandler(this.StockChartX1_EnumIndicator);
            this.StockChartX1.ClickEvent += new System.EventHandler(this.StockChartX1_ClickEvent);
            this.StockChartX1.OnRButtonDown += new System.EventHandler(this.StockChartX1_OnRButtonDown);
            this.StockChartX1.OnRButtonUp += new System.EventHandler(this.StockChartX1_OnRButtonUp);
            this.StockChartX1.EnumSeriesEvent += new AxSTOCKCHARTXLib._DStockChartXEvents_EnumSeriesEventHandler(this.StockChartX1_EnumSeriesEvent);
            this.StockChartX1.TrendLinePenetration += new AxSTOCKCHARTXLib._DStockChartXEvents_TrendLinePenetrationEventHandler(this.StockChartX1_TrendLinePenetration);
            // 
            // tmrEdit
            // 
            this.tmrEdit.Tick += new System.EventHandler(this.TmrEditTick);
            // 
            // StockChartX2
            // 
            this.StockChartX2.Enabled = true;
            this.StockChartX2.Location = new System.Drawing.Point(143, 78);
            this.StockChartX2.Name = "StockChartX2";
            this.StockChartX2.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("StockChartX2.OcxState")));
            this.StockChartX2.Size = new System.Drawing.Size(131, 114);
            this.StockChartX2.TabIndex = 2;
            this.StockChartX2.Visible = false;
            // 
            // Label2
            // 
            this.Label2.AutoSize = true;
            this.Label2.BackColor = System.Drawing.Color.Transparent;
            this.Label2.Location = new System.Drawing.Point(16, 40);
            this.Label2.Name = "Label2";
            this.Label2.Size = new System.Drawing.Size(45, 13);
            this.Label2.TabIndex = 126;
            this.Label2.Text = "Portfolio";
            // 
            // Label1
            // 
            this.Label1.AutoSize = true;
            this.Label1.BackColor = System.Drawing.Color.Transparent;
            this.Label1.Location = new System.Drawing.Point(16, 68);
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size(46, 13);
            this.Label1.TabIndex = 47;
            this.Label1.Text = "Quantity";
            // 
            // imageList1
            // 
            this.imageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.imageList1.ImageSize = new System.Drawing.Size(16, 16);
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // pnlConsensus
            // 
            this.pnlConsensus.Controls.Add(this.guage1);
            this.pnlConsensus.Location = new System.Drawing.Point(417, 0);
            this.pnlConsensus.Name = "pnlConsensus";
            this.pnlConsensus.Size = new System.Drawing.Size(376, 516);
            this.pnlConsensus.TabIndex = 63;
            this.pnlConsensus.Visible = false;
            // 
            // guage1
            // 
            this.guage1.BaseArcColor = System.Drawing.Color.Gray;
            this.guage1.BaseArcRadius = 130;
            this.guage1.BaseArcStart = 135;
            this.guage1.BaseArcSweep = 270;
            this.guage1.BaseArcWidth = 2;
            this.guage1.Cap_Idx = ((byte)(0));
            this.guage1.CapColors = new System.Drawing.Color[] {
        System.Drawing.Color.Black,
        System.Drawing.Color.Black,
        System.Drawing.Color.Black,
        System.Drawing.Color.Black,
        System.Drawing.Color.Black};
            this.guage1.CapPosition = new System.Drawing.Point(115, 90);
            this.guage1.CapsPosition = new System.Drawing.Point[] {
        new System.Drawing.Point(115, 90),
        new System.Drawing.Point(100, 200),
        new System.Drawing.Point(10, 10),
        new System.Drawing.Point(10, 10),
        new System.Drawing.Point(10, 10)};
            this.guage1.CapsText = new string[] {
        "Bearish         Bullish",
        "Expert Advisor Consensus",
        "",
        "",
        ""};
            this.guage1.CapText = "Bearish         Bullish";
            this.guage1.Center = new System.Drawing.Point(170, 170);
            this.guage1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.guage1.Location = new System.Drawing.Point(15, -3);
            this.guage1.MaxValue = 100F;
            this.guage1.MinValue = -100F;
            this.guage1.Name = "guage1";
            this.guage1.NeedleColor1 = AGaugeApp.AGauge.NeedleColorEnum.Yellow;
            this.guage1.NeedleColor2 = System.Drawing.Color.Gold;
            this.guage1.NeedleRadius = 130;
            this.guage1.NeedleType = 0;
            this.guage1.NeedleWidth = 4;
            this.guage1.Range_Idx = ((byte)(1));
            this.guage1.RangeColor = System.Drawing.Color.LimeGreen;
            this.guage1.RangeEnabled = true;
            this.guage1.RangeEndValue = 100F;
            this.guage1.RangeInnerRadius = 100;
            this.guage1.RangeOuterRadius = 130;
            this.guage1.RangesColor = new System.Drawing.Color[] {
        System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(72)))), ((int)(((byte)(72))))),
        System.Drawing.Color.LimeGreen,
        System.Drawing.SystemColors.Control,
        System.Drawing.SystemColors.Control,
        System.Drawing.SystemColors.Control};
            this.guage1.RangesEnabled = new bool[] {
        true,
        true,
        false,
        false,
        false};
            this.guage1.RangesEndValue = new float[] {
        0F,
        100F,
        0F,
        0F,
        0F};
            this.guage1.RangesInnerRadius = new int[] {
        100,
        100,
        70,
        70,
        70};
            this.guage1.RangesOuterRadius = new int[] {
        130,
        130,
        80,
        80,
        80};
            this.guage1.RangesStartValue = new float[] {
        -100F,
        0F,
        0F,
        0F,
        0F};
            this.guage1.RangeStartValue = 0F;
            this.guage1.ScaleLinesInterColor = System.Drawing.Color.Black;
            this.guage1.ScaleLinesInterInnerRadius = 130;
            this.guage1.ScaleLinesInterOuterRadius = 140;
            this.guage1.ScaleLinesInterWidth = 2;
            this.guage1.ScaleLinesMajorColor = System.Drawing.Color.Black;
            this.guage1.ScaleLinesMajorInnerRadius = 130;
            this.guage1.ScaleLinesMajorOuterRadius = 142;
            this.guage1.ScaleLinesMajorStepValue = 50F;
            this.guage1.ScaleLinesMajorWidth = 3;
            this.guage1.ScaleLinesMinorColor = System.Drawing.Color.Gray;
            this.guage1.ScaleLinesMinorInnerRadius = 130;
            this.guage1.ScaleLinesMinorNumOf = 9;
            this.guage1.ScaleLinesMinorOuterRadius = 140;
            this.guage1.ScaleLinesMinorWidth = 1;
            this.guage1.ScaleNumbersColor = System.Drawing.Color.Black;
            this.guage1.ScaleNumbersFormat = null;
            this.guage1.ScaleNumbersRadius = 160;
            this.guage1.ScaleNumbersRotation = 0;
            this.guage1.ScaleNumbersStartScaleLine = 0;
            this.guage1.ScaleNumbersStepScaleLines = 1;
            this.guage1.Size = new System.Drawing.Size(347, 295);
            this.guage1.TabIndex = 22;
            this.guage1.Text = "aGauge13";
            this.guage1.Value = 0F;
            this.guage1.Visible = false;
            // 
            // pnlTwitter
            // 
            this.pnlTwitter.BackColor = System.Drawing.Color.White;
            this.pnlTwitter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlTwitter.Controls.Add(this.TwitterTimelineControl);
            this.pnlTwitter.Controls.Add(this.UserNameLabel);
            this.pnlTwitter.Controls.Add(this.userPictureBox);
            this.pnlTwitter.Controls.Add(this.pnlTwitterControls);
            this.pnlTwitter.Location = new System.Drawing.Point(835, 0);
            this.pnlTwitter.Name = "pnlTwitter";
            this.pnlTwitter.Size = new System.Drawing.Size(376, 516);
            this.pnlTwitter.TabIndex = 64;
            this.pnlTwitter.Visible = false;
            // 
            // TwitterTimelineControl
            // 
            this.TwitterTimelineControl.AutoScroll = true;
            this.TwitterTimelineControl.BackColor = System.Drawing.Color.White;
            this.TwitterTimelineControl.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TwitterTimelineControl.Location = new System.Drawing.Point(13, 77);
            this.TwitterTimelineControl.Name = "TwitterTimelineControl";
            this.TwitterTimelineControl.Size = new System.Drawing.Size(347, 195);
            this.TwitterTimelineControl.TabIndex = 19;
            // 
            // UserNameLabel
            // 
            this.UserNameLabel.AutoSize = true;
            this.UserNameLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.UserNameLabel.Location = new System.Drawing.Point(73, 30);
            this.UserNameLabel.Name = "UserNameLabel";
            this.UserNameLabel.Size = new System.Drawing.Size(242, 17);
            this.UserNameLabel.TabIndex = 18;
            this.UserNameLabel.Text = "Please authorize the application";
            // 
            // userPictureBox
            // 
            this.userPictureBox.Location = new System.Drawing.Point(16, 13);
            this.userPictureBox.Name = "userPictureBox";
            this.userPictureBox.Size = new System.Drawing.Size(51, 51);
            this.userPictureBox.TabIndex = 17;
            this.userPictureBox.TabStop = false;
            // 
            // pnlTwitterControls
            // 
            this.pnlTwitterControls.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.pnlTwitterControls.Controls.Add(this.lblTweetSize);
            this.pnlTwitterControls.Controls.Add(this.txtTweet);
            this.pnlTwitterControls.Controls.Add(this.chkTwitter);
            this.pnlTwitterControls.Controls.Add(this.pictureBox1);
            this.pnlTwitterControls.Location = new System.Drawing.Point(3, 290);
            this.pnlTwitterControls.Name = "pnlTwitterControls";
            this.pnlTwitterControls.Size = new System.Drawing.Size(368, 221);
            this.pnlTwitterControls.TabIndex = 4;
            // 
            // lblTweetSize
            // 
            this.lblTweetSize.AutoSize = true;
            this.lblTweetSize.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.lblTweetSize.Location = new System.Drawing.Point(13, 56);
            this.lblTweetSize.Name = "lblTweetSize";
            this.lblTweetSize.Size = new System.Drawing.Size(0, 13);
            this.lblTweetSize.TabIndex = 21;
            // 
            // txtTweet
            // 
            this.txtTweet.Location = new System.Drawing.Point(10, 3);
            this.txtTweet.MaxLength = 140;
            this.txtTweet.Multiline = true;
            this.txtTweet.Name = "txtTweet";
            this.txtTweet.Size = new System.Drawing.Size(285, 47);
            this.txtTweet.TabIndex = 8;
            this.txtTweet.TextChanged += new System.EventHandler(this.txtTweet_TextChanged);
            // 
            // chkTwitter
            // 
            this.chkTwitter.AutoSize = true;
            this.chkTwitter.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkTwitter.Location = new System.Drawing.Point(110, 56);
            this.chkTwitter.Name = "chkTwitter";
            this.chkTwitter.Size = new System.Drawing.Size(189, 17);
            this.chkTwitter.TabIndex = 12;
            this.chkTwitter.Text = "Automatically Tweet ALL Trades?";
            this.chkTwitter.UseVisualStyleBackColor = true;
            this.chkTwitter.CheckedChanged += new System.EventHandler(this.ChkTwitterCheckedChanged);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.pictureBox1.Image = global::M4.Properties.Resources.twitter;
            this.pictureBox1.Location = new System.Drawing.Point(9, 146);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(100, 72);
            this.pictureBox1.TabIndex = 3;
            this.pictureBox1.TabStop = false;
            // 
            // tmrAttention
            // 
            this.tmrAttention.Enabled = true;
            this.tmrAttention.Interval = 700;
            this.tmrAttention.Tick += new System.EventHandler(this.TmrAttentionTick);
            // 
            // pnlTwitterAuthorize
            // 
            this.pnlTwitterAuthorize.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(222)))), ((int)(((byte)(237)))));
            this.pnlTwitterAuthorize.Controls.Add(this.lblPin);
            this.pnlTwitterAuthorize.Controls.Add(this.cmdSave);
            this.pnlTwitterAuthorize.Controls.Add(this.txtPin);
            this.pnlTwitterAuthorize.Controls.Add(this.picHighlight);
            this.pnlTwitterAuthorize.Location = new System.Drawing.Point(291, 365);
            this.pnlTwitterAuthorize.Name = "pnlTwitterAuthorize";
            this.pnlTwitterAuthorize.Size = new System.Drawing.Size(306, 55);
            this.pnlTwitterAuthorize.TabIndex = 68;
            this.pnlTwitterAuthorize.Visible = false;
            // 
            // lblPin
            // 
            this.lblPin.AutoSize = true;
            this.lblPin.BackColor = System.Drawing.Color.Transparent;
            this.lblPin.ForeColor = System.Drawing.Color.Black;
            this.lblPin.Location = new System.Drawing.Point(45, 23);
            this.lblPin.Name = "lblPin";
            this.lblPin.Size = new System.Drawing.Size(24, 13);
            this.lblPin.TabIndex = 16;
            this.lblPin.Text = "PIN";
            // 
            // cmdSave
            // 
            this.cmdSave.Location = new System.Drawing.Point(186, 19);
            this.cmdSave.Name = "cmdSave";
            this.cmdSave.Size = new System.Drawing.Size(74, 20);
            this.cmdSave.TabIndex = 15;
            this.cmdSave.Text = "Authorize";
            this.cmdSave.UseVisualStyleBackColor = true;
            this.cmdSave.Click += new System.EventHandler(this.cmdSave_Click);
            // 
            // txtPin
            // 
            this.txtPin.Location = new System.Drawing.Point(75, 19);
            this.txtPin.Name = "txtPin";
            this.txtPin.Size = new System.Drawing.Size(103, 20);
            this.txtPin.TabIndex = 14;
            // 
            // picHighlight
            // 
            this.picHighlight.Image = global::M4.Properties.Resources.twitter_highlight;
            this.picHighlight.Location = new System.Drawing.Point(32, 6);
            this.picHighlight.Name = "picHighlight";
            this.picHighlight.Size = new System.Drawing.Size(243, 46);
            this.picHighlight.TabIndex = 17;
            this.picHighlight.TabStop = false;
            this.picHighlight.Visible = false;
            // 
            // webBrowser1
            // 
            this.webBrowser1.Location = new System.Drawing.Point(4, 2);
            this.webBrowser1.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser1.Name = "webBrowser1";
            this.webBrowser1.ScrollBarsEnabled = false;
            this.webBrowser1.Size = new System.Drawing.Size(77, 63);
            this.webBrowser1.TabIndex = 69;
            this.webBrowser1.Visible = false;
            this.webBrowser1.DocumentCompleted += new System.Windows.Forms.WebBrowserDocumentCompletedEventHandler(this.webBrowser1_DocumentCompleted);
            // 
            // cmdEAs
            // 
            this.cmdEAs.BackgroundImage = global::M4.Properties.Resources.ExpertAdvisorSmall;
            this.cmdEAs.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.cmdEAs.FlatAppearance.BorderSize = 2;
            this.cmdEAs.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.cmdEAs.Location = new System.Drawing.Point(331, 16);
            this.cmdEAs.Name = "cmdEAs";
            this.cmdEAs.Size = new System.Drawing.Size(57, 49);
            this.cmdEAs.TabIndex = 62;
            this.cmdEAs.UseVisualStyleBackColor = true;
            this.cmdEAs.Visible = false;
            this.cmdEAs.Click += new System.EventHandler(this.CmdEAsClick);
            // 
            // TelerickCtlChart
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnlTwitterAuthorize);
            this.Controls.Add(this.webBrowser1);
            this.Controls.Add(this.pnlTwitter);
            this.Controls.Add(this.pnlConsensus);
            this.Controls.Add(this.cmdEAs);
            this.Controls.Add(this.StockChartX2);
            this.Controls.Add(this.StockChartX1);
            this.Name = "TelerickCtlChart";
            this.Size = new System.Drawing.Size(1220, 524);
            this.Resize += new System.EventHandler(this.ctlChart_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.StockChartX1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.StockChartX2)).EndInit();
            this.pnlConsensus.ResumeLayout(false);
            this.pnlTwitter.ResumeLayout(false);
            this.pnlTwitter.PerformLayout();
            this.pnlTwitterControls.ResumeLayout(false);
            this.pnlTwitterControls.PerformLayout();
            this.pnlTwitterAuthorize.ResumeLayout(false);
            this.pnlTwitterAuthorize.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picHighlight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.userPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        internal AxSTOCKCHARTXLib.AxStockChartX StockChartX1;
        internal System.Windows.Forms.Timer tmrEdit;
        internal AxSTOCKCHARTXLib.AxStockChartX StockChartX2;
        internal System.Windows.Forms.Label Label2;
        internal System.Windows.Forms.Label Label1;
        private System.Windows.Forms.Button cmdEAs;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.Panel pnlConsensus;
        private AGaugeApp.AGauge guage1;
        private System.Windows.Forms.Panel pnlTwitter;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Panel pnlTwitterControls;
        private System.Windows.Forms.Label UserNameLabel;
        private System.Windows.Forms.PictureBox userPictureBox;
        private System.Windows.Forms.FlowLayoutPanel TwitterTimelineControl;
        private System.Windows.Forms.CheckBox chkTwitter;
        private System.Windows.Forms.TextBox txtTweet;
        private System.Windows.Forms.Label lblTweetSize;
        private System.Windows.Forms.Timer tmrAttention;
        private System.Windows.Forms.Panel pnlTwitterAuthorize;
        private System.Windows.Forms.Label lblPin;
        private System.Windows.Forms.Button cmdSave;
        private System.Windows.Forms.TextBox txtPin;
        private System.Windows.Forms.PictureBox picHighlight;
        private System.Windows.Forms.WebBrowser webBrowser1;

    }
}
