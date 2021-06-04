
namespace StreamDeckIconConverter
{
    partial class MainForm
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージド リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.labelInputFile = new System.Windows.Forms.Label();
            this.textBoxInputFilePath = new System.Windows.Forms.TextBox();
            this.buttonInputFileBrowse = new System.Windows.Forms.Button();
            this.labelVideoFrame = new System.Windows.Forms.Label();
            this.comboBoxVideoFrame = new System.Windows.Forms.ComboBox();
            this.labelInputSizeTitle = new System.Windows.Forms.Label();
            this.labelIconLayout = new System.Windows.Forms.Label();
            this.numericUpDownIconLayoutCol = new System.Windows.Forms.NumericUpDown();
            this.labelIconLayoutX = new System.Windows.Forms.Label();
            this.numericUpDownIconLayoutRow = new System.Windows.Forms.NumericUpDown();
            this.buttonIconLayoutMini = new System.Windows.Forms.Button();
            this.buttonIconLayoutNormal = new System.Windows.Forms.Button();
            this.buttonIconLayoutXL = new System.Windows.Forms.Button();
            this.labelOutputSize = new System.Windows.Forms.Label();
            this.labelCropStartPos = new System.Windows.Forms.Label();
            this.hScrollBarCropPos = new System.Windows.Forms.HScrollBar();
            this.labelCropPosDir1 = new System.Windows.Forms.Label();
            this.labelCropPosDir2 = new System.Windows.Forms.Label();
            this.labelCropStartPosPixel = new System.Windows.Forms.Label();
            this.labelStartTime = new System.Windows.Forms.Label();
            this.hScrollBarStartTime = new System.Windows.Forms.HScrollBar();
            this.labelStartTimeSpan = new System.Windows.Forms.Label();
            this.hScrollBarStartTimeMs = new System.Windows.Forms.HScrollBar();
            this.labelStartTimeMs = new System.Windows.Forms.Label();
            this.labelEndTime = new System.Windows.Forms.Label();
            this.hScrollBarEndTime = new System.Windows.Forms.HScrollBar();
            this.hScrollBarEndTimeMs = new System.Windows.Forms.HScrollBar();
            this.labelEndTimeSpan = new System.Windows.Forms.Label();
            this.labelEndTimeMs = new System.Windows.Forms.Label();
            this.labelGifDuration = new System.Windows.Forms.Label();
            this.labelGifDurationSec = new System.Windows.Forms.Label();
            this.numericUpDownFrameRate = new System.Windows.Forms.NumericUpDown();
            this.labelFrameRate = new System.Windows.Forms.Label();
            this.labelFrameRateFps = new System.Windows.Forms.Label();
            this.buttonPreviewMp4 = new System.Windows.Forms.Button();
            this.buttonPreviewGif = new System.Windows.Forms.Button();
            this.buttonGenerateIcon = new System.Windows.Forms.Button();
            this.labelFFmpegArgument = new System.Windows.Forms.Label();
            this.textBoxFFmpegArgument = new System.Windows.Forms.TextBox();
            this.checkBoxAddBorder = new System.Windows.Forms.CheckBox();
            this.labelInputSize = new System.Windows.Forms.Label();
            this.labelInputDurationTitle = new System.Windows.Forms.Label();
            this.labelInputDuration = new System.Windows.Forms.Label();
            this.labelOutputSizeTitle = new System.Windows.Forms.Label();
            this.linkLabelTwitter = new System.Windows.Forms.LinkLabel();
            this.linkLabelGitHub = new System.Windows.Forms.LinkLabel();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownIconLayoutCol)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownIconLayoutRow)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownFrameRate)).BeginInit();
            this.SuspendLayout();
            // 
            // labelInputFile
            // 
            resources.ApplyResources(this.labelInputFile, "labelInputFile");
            this.labelInputFile.Name = "labelInputFile";
            // 
            // textBoxInputFilePath
            // 
            this.textBoxInputFilePath.AllowDrop = true;
            resources.ApplyResources(this.textBoxInputFilePath, "textBoxInputFilePath");
            this.textBoxInputFilePath.Name = "textBoxInputFilePath";
            this.textBoxInputFilePath.ReadOnly = true;
            this.textBoxInputFilePath.DragDrop += new System.Windows.Forms.DragEventHandler(this.textBoxInputFilePath_DragDrop);
            this.textBoxInputFilePath.DragEnter += new System.Windows.Forms.DragEventHandler(this.textBoxInputFilePath_DragEnter);
            // 
            // buttonInputFileBrowse
            // 
            resources.ApplyResources(this.buttonInputFileBrowse, "buttonInputFileBrowse");
            this.buttonInputFileBrowse.Name = "buttonInputFileBrowse";
            this.buttonInputFileBrowse.UseVisualStyleBackColor = true;
            this.buttonInputFileBrowse.Click += new System.EventHandler(this.buttonInputFileBrowse_Click);
            // 
            // labelVideoFrame
            // 
            resources.ApplyResources(this.labelVideoFrame, "labelVideoFrame");
            this.labelVideoFrame.Name = "labelVideoFrame";
            // 
            // comboBoxVideoFrame
            // 
            this.comboBoxVideoFrame.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxVideoFrame.FormattingEnabled = true;
            this.comboBoxVideoFrame.Items.AddRange(new object[] {
            resources.GetString("comboBoxVideoFrame.Items"),
            resources.GetString("comboBoxVideoFrame.Items1")});
            resources.ApplyResources(this.comboBoxVideoFrame, "comboBoxVideoFrame");
            this.comboBoxVideoFrame.Name = "comboBoxVideoFrame";
            this.comboBoxVideoFrame.SelectedIndexChanged += new System.EventHandler(this.comboBoxVideoFrame_SelectedIndexChanged);
            // 
            // labelInputSizeTitle
            // 
            resources.ApplyResources(this.labelInputSizeTitle, "labelInputSizeTitle");
            this.labelInputSizeTitle.Name = "labelInputSizeTitle";
            // 
            // labelIconLayout
            // 
            resources.ApplyResources(this.labelIconLayout, "labelIconLayout");
            this.labelIconLayout.Name = "labelIconLayout";
            // 
            // numericUpDownIconLayoutCol
            // 
            resources.ApplyResources(this.numericUpDownIconLayoutCol, "numericUpDownIconLayoutCol");
            this.numericUpDownIconLayoutCol.Maximum = new decimal(new int[] {
            8,
            0,
            0,
            0});
            this.numericUpDownIconLayoutCol.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownIconLayoutCol.Name = "numericUpDownIconLayoutCol";
            this.numericUpDownIconLayoutCol.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.numericUpDownIconLayoutCol.ValueChanged += new System.EventHandler(this.numericUpDownIconLayoutCol_ValueChanged);
            // 
            // labelIconLayoutX
            // 
            resources.ApplyResources(this.labelIconLayoutX, "labelIconLayoutX");
            this.labelIconLayoutX.Name = "labelIconLayoutX";
            // 
            // numericUpDownIconLayoutRow
            // 
            resources.ApplyResources(this.numericUpDownIconLayoutRow, "numericUpDownIconLayoutRow");
            this.numericUpDownIconLayoutRow.Maximum = new decimal(new int[] {
            4,
            0,
            0,
            0});
            this.numericUpDownIconLayoutRow.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownIconLayoutRow.Name = "numericUpDownIconLayoutRow";
            this.numericUpDownIconLayoutRow.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.numericUpDownIconLayoutRow.ValueChanged += new System.EventHandler(this.numericUpDownIconLayoutRow_ValueChanged);
            // 
            // buttonIconLayoutMini
            // 
            resources.ApplyResources(this.buttonIconLayoutMini, "buttonIconLayoutMini");
            this.buttonIconLayoutMini.Name = "buttonIconLayoutMini";
            this.buttonIconLayoutMini.UseVisualStyleBackColor = true;
            this.buttonIconLayoutMini.Click += new System.EventHandler(this.buttonIconLayoutMini_Click);
            // 
            // buttonIconLayoutNormal
            // 
            resources.ApplyResources(this.buttonIconLayoutNormal, "buttonIconLayoutNormal");
            this.buttonIconLayoutNormal.Name = "buttonIconLayoutNormal";
            this.buttonIconLayoutNormal.UseVisualStyleBackColor = true;
            this.buttonIconLayoutNormal.Click += new System.EventHandler(this.buttonIconLayoutNormal_Click);
            // 
            // buttonIconLayoutXL
            // 
            resources.ApplyResources(this.buttonIconLayoutXL, "buttonIconLayoutXL");
            this.buttonIconLayoutXL.Name = "buttonIconLayoutXL";
            this.buttonIconLayoutXL.UseVisualStyleBackColor = true;
            this.buttonIconLayoutXL.Click += new System.EventHandler(this.buttonIconLayoutXL_Click);
            // 
            // labelOutputSize
            // 
            resources.ApplyResources(this.labelOutputSize, "labelOutputSize");
            this.labelOutputSize.Name = "labelOutputSize";
            // 
            // labelCropStartPos
            // 
            resources.ApplyResources(this.labelCropStartPos, "labelCropStartPos");
            this.labelCropStartPos.Name = "labelCropStartPos";
            // 
            // hScrollBarCropPos
            // 
            resources.ApplyResources(this.hScrollBarCropPos, "hScrollBarCropPos");
            this.hScrollBarCropPos.LargeChange = 1;
            this.hScrollBarCropPos.Maximum = 0;
            this.hScrollBarCropPos.Name = "hScrollBarCropPos";
            this.hScrollBarCropPos.Scroll += new System.Windows.Forms.ScrollEventHandler(this.hScrollBarCropPos_Scroll);
            // 
            // labelCropPosDir1
            // 
            resources.ApplyResources(this.labelCropPosDir1, "labelCropPosDir1");
            this.labelCropPosDir1.Name = "labelCropPosDir1";
            // 
            // labelCropPosDir2
            // 
            resources.ApplyResources(this.labelCropPosDir2, "labelCropPosDir2");
            this.labelCropPosDir2.Name = "labelCropPosDir2";
            // 
            // labelCropStartPosPixel
            // 
            resources.ApplyResources(this.labelCropStartPosPixel, "labelCropStartPosPixel");
            this.labelCropStartPosPixel.Name = "labelCropStartPosPixel";
            // 
            // labelStartTime
            // 
            resources.ApplyResources(this.labelStartTime, "labelStartTime");
            this.labelStartTime.Name = "labelStartTime";
            // 
            // hScrollBarStartTime
            // 
            resources.ApplyResources(this.hScrollBarStartTime, "hScrollBarStartTime");
            this.hScrollBarStartTime.LargeChange = 1;
            this.hScrollBarStartTime.Maximum = 0;
            this.hScrollBarStartTime.Name = "hScrollBarStartTime";
            this.hScrollBarStartTime.Scroll += new System.Windows.Forms.ScrollEventHandler(this.hScrollBarStartTime_Scroll);
            // 
            // labelStartTimeSpan
            // 
            resources.ApplyResources(this.labelStartTimeSpan, "labelStartTimeSpan");
            this.labelStartTimeSpan.Name = "labelStartTimeSpan";
            // 
            // hScrollBarStartTimeMs
            // 
            resources.ApplyResources(this.hScrollBarStartTimeMs, "hScrollBarStartTimeMs");
            this.hScrollBarStartTimeMs.LargeChange = 1;
            this.hScrollBarStartTimeMs.Maximum = 999;
            this.hScrollBarStartTimeMs.Name = "hScrollBarStartTimeMs";
            this.hScrollBarStartTimeMs.Scroll += new System.Windows.Forms.ScrollEventHandler(this.hScrollBarStartTimeMs_Scroll);
            // 
            // labelStartTimeMs
            // 
            resources.ApplyResources(this.labelStartTimeMs, "labelStartTimeMs");
            this.labelStartTimeMs.Name = "labelStartTimeMs";
            // 
            // labelEndTime
            // 
            resources.ApplyResources(this.labelEndTime, "labelEndTime");
            this.labelEndTime.Name = "labelEndTime";
            // 
            // hScrollBarEndTime
            // 
            resources.ApplyResources(this.hScrollBarEndTime, "hScrollBarEndTime");
            this.hScrollBarEndTime.LargeChange = 1;
            this.hScrollBarEndTime.Maximum = 0;
            this.hScrollBarEndTime.Name = "hScrollBarEndTime";
            this.hScrollBarEndTime.Scroll += new System.Windows.Forms.ScrollEventHandler(this.hScrollBarEndTime_Scroll);
            // 
            // hScrollBarEndTimeMs
            // 
            resources.ApplyResources(this.hScrollBarEndTimeMs, "hScrollBarEndTimeMs");
            this.hScrollBarEndTimeMs.LargeChange = 1;
            this.hScrollBarEndTimeMs.Maximum = 999;
            this.hScrollBarEndTimeMs.Name = "hScrollBarEndTimeMs";
            this.hScrollBarEndTimeMs.Scroll += new System.Windows.Forms.ScrollEventHandler(this.hScrollBarEndTimeMs_Scroll);
            // 
            // labelEndTimeSpan
            // 
            resources.ApplyResources(this.labelEndTimeSpan, "labelEndTimeSpan");
            this.labelEndTimeSpan.Name = "labelEndTimeSpan";
            // 
            // labelEndTimeMs
            // 
            resources.ApplyResources(this.labelEndTimeMs, "labelEndTimeMs");
            this.labelEndTimeMs.Name = "labelEndTimeMs";
            // 
            // labelGifDuration
            // 
            resources.ApplyResources(this.labelGifDuration, "labelGifDuration");
            this.labelGifDuration.Name = "labelGifDuration";
            // 
            // labelGifDurationSec
            // 
            resources.ApplyResources(this.labelGifDurationSec, "labelGifDurationSec");
            this.labelGifDurationSec.Name = "labelGifDurationSec";
            // 
            // numericUpDownFrameRate
            // 
            resources.ApplyResources(this.numericUpDownFrameRate, "numericUpDownFrameRate");
            this.numericUpDownFrameRate.Maximum = new decimal(new int[] {
            30,
            0,
            0,
            0});
            this.numericUpDownFrameRate.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownFrameRate.Name = "numericUpDownFrameRate";
            this.numericUpDownFrameRate.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numericUpDownFrameRate.ValueChanged += new System.EventHandler(this.numericUpDownFrameRate_ValueChanged);
            // 
            // labelFrameRate
            // 
            resources.ApplyResources(this.labelFrameRate, "labelFrameRate");
            this.labelFrameRate.Name = "labelFrameRate";
            // 
            // labelFrameRateFps
            // 
            resources.ApplyResources(this.labelFrameRateFps, "labelFrameRateFps");
            this.labelFrameRateFps.Name = "labelFrameRateFps";
            // 
            // buttonPreviewMp4
            // 
            resources.ApplyResources(this.buttonPreviewMp4, "buttonPreviewMp4");
            this.buttonPreviewMp4.Name = "buttonPreviewMp4";
            this.buttonPreviewMp4.UseVisualStyleBackColor = true;
            this.buttonPreviewMp4.Click += new System.EventHandler(this.buttonPreviewMp4_Click);
            // 
            // buttonPreviewGif
            // 
            resources.ApplyResources(this.buttonPreviewGif, "buttonPreviewGif");
            this.buttonPreviewGif.Name = "buttonPreviewGif";
            this.buttonPreviewGif.UseVisualStyleBackColor = true;
            this.buttonPreviewGif.Click += new System.EventHandler(this.buttonPreviewGif_Click);
            // 
            // buttonGenerateIcon
            // 
            resources.ApplyResources(this.buttonGenerateIcon, "buttonGenerateIcon");
            this.buttonGenerateIcon.Name = "buttonGenerateIcon";
            this.buttonGenerateIcon.UseVisualStyleBackColor = true;
            this.buttonGenerateIcon.Click += new System.EventHandler(this.buttonGenerateIcon_Click);
            // 
            // labelFFmpegArgument
            // 
            resources.ApplyResources(this.labelFFmpegArgument, "labelFFmpegArgument");
            this.labelFFmpegArgument.Name = "labelFFmpegArgument";
            // 
            // textBoxFFmpegArgument
            // 
            resources.ApplyResources(this.textBoxFFmpegArgument, "textBoxFFmpegArgument");
            this.textBoxFFmpegArgument.Name = "textBoxFFmpegArgument";
            this.textBoxFFmpegArgument.ReadOnly = true;
            // 
            // checkBoxAddBorder
            // 
            resources.ApplyResources(this.checkBoxAddBorder, "checkBoxAddBorder");
            this.checkBoxAddBorder.Checked = true;
            this.checkBoxAddBorder.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxAddBorder.Name = "checkBoxAddBorder";
            this.checkBoxAddBorder.UseVisualStyleBackColor = true;
            // 
            // labelInputSize
            // 
            resources.ApplyResources(this.labelInputSize, "labelInputSize");
            this.labelInputSize.Name = "labelInputSize";
            // 
            // labelInputDurationTitle
            // 
            resources.ApplyResources(this.labelInputDurationTitle, "labelInputDurationTitle");
            this.labelInputDurationTitle.Name = "labelInputDurationTitle";
            // 
            // labelInputDuration
            // 
            resources.ApplyResources(this.labelInputDuration, "labelInputDuration");
            this.labelInputDuration.Name = "labelInputDuration";
            // 
            // labelOutputSizeTitle
            // 
            resources.ApplyResources(this.labelOutputSizeTitle, "labelOutputSizeTitle");
            this.labelOutputSizeTitle.Name = "labelOutputSizeTitle";
            // 
            // linkLabelTwitter
            // 
            resources.ApplyResources(this.linkLabelTwitter, "linkLabelTwitter");
            this.linkLabelTwitter.Name = "linkLabelTwitter";
            this.linkLabelTwitter.TabStop = true;
            this.linkLabelTwitter.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelTwitter_LinkClicked);
            // 
            // linkLabelGitHub
            // 
            resources.ApplyResources(this.linkLabelGitHub, "linkLabelGitHub");
            this.linkLabelGitHub.Name = "linkLabelGitHub";
            this.linkLabelGitHub.TabStop = true;
            this.linkLabelGitHub.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelGitHub_LinkClicked);
            // 
            // MainForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.linkLabelGitHub);
            this.Controls.Add(this.linkLabelTwitter);
            this.Controls.Add(this.checkBoxAddBorder);
            this.Controls.Add(this.buttonGenerateIcon);
            this.Controls.Add(this.buttonPreviewGif);
            this.Controls.Add(this.buttonPreviewMp4);
            this.Controls.Add(this.labelEndTimeMs);
            this.Controls.Add(this.labelStartTimeMs);
            this.Controls.Add(this.labelEndTimeSpan);
            this.Controls.Add(this.labelStartTimeSpan);
            this.Controls.Add(this.labelCropStartPosPixel);
            this.Controls.Add(this.labelCropPosDir2);
            this.Controls.Add(this.labelCropPosDir1);
            this.Controls.Add(this.hScrollBarEndTimeMs);
            this.Controls.Add(this.hScrollBarStartTimeMs);
            this.Controls.Add(this.hScrollBarEndTime);
            this.Controls.Add(this.hScrollBarStartTime);
            this.Controls.Add(this.hScrollBarCropPos);
            this.Controls.Add(this.labelCropStartPos);
            this.Controls.Add(this.buttonIconLayoutXL);
            this.Controls.Add(this.buttonIconLayoutNormal);
            this.Controls.Add(this.buttonIconLayoutMini);
            this.Controls.Add(this.labelIconLayoutX);
            this.Controls.Add(this.numericUpDownIconLayoutRow);
            this.Controls.Add(this.numericUpDownFrameRate);
            this.Controls.Add(this.numericUpDownIconLayoutCol);
            this.Controls.Add(this.labelIconLayout);
            this.Controls.Add(this.labelOutputSizeTitle);
            this.Controls.Add(this.labelOutputSize);
            this.Controls.Add(this.labelInputSize);
            this.Controls.Add(this.labelInputDuration);
            this.Controls.Add(this.labelInputDurationTitle);
            this.Controls.Add(this.labelInputSizeTitle);
            this.Controls.Add(this.labelGifDurationSec);
            this.Controls.Add(this.labelFrameRateFps);
            this.Controls.Add(this.labelFFmpegArgument);
            this.Controls.Add(this.labelFrameRate);
            this.Controls.Add(this.labelGifDuration);
            this.Controls.Add(this.labelEndTime);
            this.Controls.Add(this.comboBoxVideoFrame);
            this.Controls.Add(this.labelStartTime);
            this.Controls.Add(this.labelVideoFrame);
            this.Controls.Add(this.buttonInputFileBrowse);
            this.Controls.Add(this.textBoxFFmpegArgument);
            this.Controls.Add(this.textBoxInputFilePath);
            this.Controls.Add(this.labelInputFile);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Load += new System.EventHandler(this.MainForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownIconLayoutCol)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownIconLayoutRow)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownFrameRate)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelInputFile;
        private System.Windows.Forms.TextBox textBoxInputFilePath;
        private System.Windows.Forms.Button buttonInputFileBrowse;
        private System.Windows.Forms.Label labelVideoFrame;
        private System.Windows.Forms.ComboBox comboBoxVideoFrame;
        private System.Windows.Forms.Label labelInputSizeTitle;
        private System.Windows.Forms.Label labelIconLayout;
        private System.Windows.Forms.NumericUpDown numericUpDownIconLayoutCol;
        private System.Windows.Forms.Label labelIconLayoutX;
        private System.Windows.Forms.NumericUpDown numericUpDownIconLayoutRow;
        private System.Windows.Forms.Button buttonIconLayoutMini;
        private System.Windows.Forms.Button buttonIconLayoutNormal;
        private System.Windows.Forms.Button buttonIconLayoutXL;
        private System.Windows.Forms.Label labelOutputSize;
        private System.Windows.Forms.Label labelCropStartPos;
        private System.Windows.Forms.HScrollBar hScrollBarCropPos;
        private System.Windows.Forms.Label labelCropPosDir1;
        private System.Windows.Forms.Label labelCropPosDir2;
        private System.Windows.Forms.Label labelCropStartPosPixel;
        private System.Windows.Forms.Label labelStartTime;
        private System.Windows.Forms.HScrollBar hScrollBarStartTime;
        private System.Windows.Forms.Label labelStartTimeSpan;
        private System.Windows.Forms.HScrollBar hScrollBarStartTimeMs;
        private System.Windows.Forms.Label labelStartTimeMs;
        private System.Windows.Forms.Label labelEndTime;
        private System.Windows.Forms.HScrollBar hScrollBarEndTime;
        private System.Windows.Forms.HScrollBar hScrollBarEndTimeMs;
        private System.Windows.Forms.Label labelEndTimeSpan;
        private System.Windows.Forms.Label labelEndTimeMs;
        private System.Windows.Forms.Label labelGifDuration;
        private System.Windows.Forms.Label labelGifDurationSec;
        private System.Windows.Forms.NumericUpDown numericUpDownFrameRate;
        private System.Windows.Forms.Label labelFrameRate;
        private System.Windows.Forms.Label labelFrameRateFps;
        private System.Windows.Forms.Button buttonPreviewMp4;
        private System.Windows.Forms.Button buttonPreviewGif;
        private System.Windows.Forms.Button buttonGenerateIcon;
        private System.Windows.Forms.Label labelFFmpegArgument;
        private System.Windows.Forms.TextBox textBoxFFmpegArgument;
        private System.Windows.Forms.CheckBox checkBoxAddBorder;
        private System.Windows.Forms.Label labelInputSize;
        private System.Windows.Forms.Label labelInputDurationTitle;
        private System.Windows.Forms.Label labelInputDuration;
        private System.Windows.Forms.Label labelOutputSizeTitle;
        private System.Windows.Forms.LinkLabel linkLabelTwitter;
        private System.Windows.Forms.LinkLabel linkLabelGitHub;
    }
}

