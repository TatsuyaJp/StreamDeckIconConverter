using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Drawing.Imaging;
using System.Reflection;

namespace StreamDeckIconConverter
{
    public partial class MainForm : Form
    {
        // FFmpegの実行ファイル名
        const string FFMPEG_EXE_NAME = "ffmpeg.exe";
        // FFmpegのファイルパス
        string m_sFFmpegExePath = FFMPEG_EXE_NAME;
        // 入力動画のサイズ
        int m_iInputSizeWidth = 0;
        int m_iInputSizeHeight = 0;
        // 入力動画の長さ
        TimeSpan m_tsDuration = new TimeSpan();
        // アイコンの幅
        const int ICON_WIDTH = 72;
        // 枠線の幅
        int m_iBorderWidth = 36;
        // アプリケーションのディレクトリ
        string m_sAppDir = "";
        // 枠線画像ファイル名
        const string BORDER_IMAGE_NAME = "border.png";
        // プレビュー作成待機時間
        int m_iPreviewWait = 0;
        // ボーダー画像作成済みフラグ
        bool m_bBorderBitmapSaved = false;

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            m_sAppDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            this.Text += " Ver." + Application.ProductVersion;
#if DEBUG
            this.Text += " *DEBUG*";
#endif

            PrintOutputSize();
            comboBoxVideoFrame.SelectedIndex = 0;
        }

        private void buttonInputFileBrowse_Click(object sender, EventArgs e)
        {
            if (IsExistFFmpeg(true))
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    if (GetVideoInfo(openFileDialog.FileName))
                    {
                        textBoxInputFilePath.Text = openFileDialog.FileName;
                        RequestPreview();
                    }
                }
            }
        }

        private void textBoxInputFilePath_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Move;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void textBoxInputFilePath_DragDrop(object sender, DragEventArgs e)
        {
            string[] sFilePathArray = (string[])e.Data.GetData(DataFormats.FileDrop, false);

            if (sFilePathArray.Length > 0)
            {
                if (File.Exists(sFilePathArray[0]))
                {
                    if (GetVideoInfo(sFilePathArray[0]))
                    {
                        textBoxInputFilePath.Text = sFilePathArray[0];
                        RequestPreview();
                    }
                }
            }
        }

        private bool IsExistFFmpeg(bool bNotice)
        {
            bool bExist = false;

            if (File.Exists(m_sFFmpegExePath))
            {
                bExist = true;
            }
            else if (File.Exists(m_sAppDir + Path.DirectorySeparatorChar + FFMPEG_EXE_NAME))
            {
                m_sFFmpegExePath = m_sAppDir + Path.DirectorySeparatorChar + FFMPEG_EXE_NAME;
                bExist = true;
            }
            else
            {
                if (bNotice)
                {
                    if (DialogResult.OK == MessageBox.Show(Properties.Resources.ffmpegNotFound + "\n" + Properties.Resources.ffmpegDownloadConfirm, Properties.Resources.warning, MessageBoxButtons.OKCancel, MessageBoxIcon.Warning))
                    {
                        Process.Start("https://www.gyan.dev/ffmpeg/builds/");
                        MessageBox.Show(Properties.Resources.ffmpegDownloadInformation, Properties.Resources.information, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }

            return bExist;
        }

        private bool GetVideoInfo(string sFilePath)
        {
            bool bResult = false;

            if (IsExistFFmpeg(true))
            {
                ProcessStartInfo processStartInfo = new ProcessStartInfo(m_sFFmpegExePath);
                processStartInfo.CreateNoWindow = true;
                processStartInfo.UseShellExecute = false;
                processStartInfo.RedirectStandardOutput = true;
                processStartInfo.RedirectStandardError = true;
                processStartInfo.Arguments = "-i \"" + sFilePath + "\"";

                Process process = Process.Start(processStartInfo);
                string sError = process.StandardError.ReadToEnd();

                if (process.ExitCode == 1)
                {
                    // 動画サイズの検出
                    Regex regexInputinfo = new Regex(@"Stream #\d+:\d+.* Video: .*, (?<width>\d+)x(?<height>\d+)( \[SAR (?<sarw>\d+):(?<sarh>\d+))?", RegexOptions.Multiline);
                    Match matchInputInfo = regexInputinfo.Match(sError);
                    // 再生時間の検出
                    Regex regexDuration = new Regex(@"Duration: (?<hour>\d+):(?<min>\d+):(?<sec>\d+)\.(?<msec>\d+)");
                    Match matchDuration = regexDuration.Match(sError);

                    if (matchInputInfo.Success && matchDuration.Success)
                    {
                        // 動画サイズの算出
                        int iWidth = Int32.Parse(matchInputInfo.Groups["width"].Value);
                        int iHeight = Int32.Parse(matchInputInfo.Groups["height"].Value);

                        double dSar = 1.0;

                        if (matchInputInfo.Groups["sarw"].Success && matchInputInfo.Groups["sarh"].Success)
                        {
                            dSar = (double)Int32.Parse(matchInputInfo.Groups["sarw"].Value) / Int32.Parse(matchInputInfo.Groups["sarh"].Value);
                        }

                        double dDar = ((double)iWidth / iHeight) * dSar;

                        iWidth = (int)Math.Floor(iHeight * dDar / 2.0) * 2;
                        iHeight = (int)Math.Floor(iHeight / 2.0) * 2;

                        int iDurationMs = Int32.Parse(matchDuration.Groups["msec"].Value) * 10;

                        // 再生時間の算出
                        m_tsDuration = new TimeSpan(0, Int32.Parse(matchDuration.Groups["hour"].Value), Int32.Parse(matchDuration.Groups["min"].Value), Int32.Parse(matchDuration.Groups["sec"].Value), iDurationMs);
                        hScrollBarStartTime.Maximum = (int)Math.Floor(m_tsDuration.TotalSeconds);
                        hScrollBarStartTimeMs.Maximum = 999;
                        hScrollBarEndTime.Maximum = (int)Math.Floor(m_tsDuration.TotalSeconds);
                        hScrollBarEndTimeMs.Maximum = 999;
                        hScrollBarStartTime.Value = 0;
                        hScrollBarStartTimeMs.Value = 0;
                        hScrollBarEndTime.Value = hScrollBarEndTime.Maximum;
                        hScrollBarEndTimeMs.Value = iDurationMs;
                        RefreshStartTime();
                        RefreshEndTime();

                        // 動画情報を表示
                        labelInputSize.Text = iWidth + "x" + iHeight;
                        labelInputDuration.Text = m_tsDuration.ToString(@"hh\:mm\:ss\.fff");

                        // 動画サイズを格納
                        m_iInputSizeWidth = iWidth;
                        m_iInputSizeHeight = iHeight;

                        SetCropMax();

                        bResult = true;
                    }
                    else
                    {
                        MessageBox.Show(Properties.Resources.failedGetVideoInfo, Properties.Resources.error, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show(Properties.Resources.ffmpegError, Properties.Resources.error, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            return bResult;
        }

        private void buttonIconLayoutMini_Click(object sender, EventArgs e)
        {
            numericUpDownIconLayoutCol.Value = 3;
            numericUpDownIconLayoutRow.Value = 2;
        }

        private void buttonIconLayoutNormal_Click(object sender, EventArgs e)
        {
            numericUpDownIconLayoutCol.Value = 5;
            numericUpDownIconLayoutRow.Value = 3;
        }

        private void buttonIconLayoutXL_Click(object sender, EventArgs e)
        {
            numericUpDownIconLayoutCol.Value = 8;
            numericUpDownIconLayoutRow.Value = 4;
        }

        private void numericUpDownIconLayoutCol_ValueChanged(object sender, EventArgs e)
        {
            PrintOutputSize();
            SetCropMax();
            m_bBorderBitmapSaved = false;
            RequestPreview();
        }

        private void numericUpDownIconLayoutRow_ValueChanged(object sender, EventArgs e)
        {
            PrintOutputSize();
            SetCropMax();
            m_bBorderBitmapSaved = false;
            RequestPreview();
        }

        private void PrintOutputSize()
        {
            Size sizeOutput = CalcOutputSize();
            labelOutputSize.Text = sizeOutput.Width + "x" + sizeOutput.Height;
        }

        private Size CalcOutputSize()
        {
            Size sizeOutput = new Size();

            sizeOutput.Width = ((int)numericUpDownIconLayoutCol.Value * ICON_WIDTH) + (((int)numericUpDownIconLayoutCol.Value - 1) * m_iBorderWidth);
            sizeOutput.Height = ((int)numericUpDownIconLayoutRow.Value * ICON_WIDTH) + (((int)numericUpDownIconLayoutRow.Value - 1) * m_iBorderWidth);

            return sizeOutput;
        }

        private void SetCropMax()
        {
            int iCropMax = 0;
            Size sizeOutput = CalcOutputSize();
            Size sizeCrop = CalcCropSize();

            if (comboBoxVideoFrame.SelectedIndex != 0)
            {
                iCropMax = 0;
            }
            else if (m_iInputSizeHeight == sizeCrop.Height)
            {
                iCropMax = m_iInputSizeWidth - sizeCrop.Width;
                labelCropPosDir1.Text = Properties.Resources.left;
                labelCropPosDir2.Text = Properties.Resources.right;
            }
            else
            {
                iCropMax = m_iInputSizeHeight - sizeCrop.Height;
                labelCropPosDir1.Text = Properties.Resources.top;
                labelCropPosDir2.Text = Properties.Resources.bottom;
            }

            hScrollBarCropPos.Maximum = iCropMax;
            hScrollBarCropPos.Value = iCropMax / 2;
            RefreshCropPosPixel();
        }

        private Size CalcCropSize()
        {
            Size sizeCrop = new Size();
            Size sizeOutput = CalcOutputSize();
            double dOutputRate = (double)sizeOutput.Width / sizeOutput.Height;

            if (((double)m_iInputSizeWidth / m_iInputSizeHeight) > dOutputRate)
            {
                // 横方向をクロップ
                sizeCrop.Width = (int)Math.Floor(m_iInputSizeHeight * dOutputRate / 2.0) * 2;
                sizeCrop.Height = m_iInputSizeHeight;
            }
            else
            {
                // 縦方向をクロップ
                sizeCrop.Width = m_iInputSizeWidth;
                sizeCrop.Height = (int)Math.Floor(m_iInputSizeWidth / dOutputRate / 2.0) * 2;
            }

            return sizeCrop;
        }

        private void hScrollBarCropPos_Scroll(object sender, ScrollEventArgs e)
        {
            RefreshCropPosPixel();
        }

        private void RefreshCropPosPixel()
        {
            labelCropStartPosPixel.Text = hScrollBarCropPos.Value + "[px]";
            RequestPreview();
        }

        private void comboBoxVideoFrame_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool bCropVisible = true;

            if (comboBoxVideoFrame.SelectedIndex != 0)
            {
                bCropVisible = false;
            }

            labelCropStartPos.Visible = bCropVisible;
            labelCropPosDir1.Visible = bCropVisible;
            labelCropPosDir2.Visible = bCropVisible;
            labelCropStartPosPixel.Visible = bCropVisible;
            hScrollBarCropPos.Visible = bCropVisible;

            RequestPreview();
        }

        private void hScrollBarStartTime_Scroll(object sender, ScrollEventArgs e)
        {
            RefreshStartTime();
        }

        private void RefreshStartTime()
        {
            TimeSpan timeSpan = new TimeSpan(0, 0, hScrollBarStartTime.Value);
            labelStartTimeSpan.Text = timeSpan.ToString(@"hh\:mm\:ss");

            if (hScrollBarStartTime.Value == hScrollBarStartTime.Maximum)
            {
                hScrollBarStartTimeMs.Maximum = m_tsDuration.Milliseconds;
            }
            else
            {
                hScrollBarStartTimeMs.Maximum = 999;
            }

            RefreshStartTimeMs();
        }

        private void hScrollBarStartTimeMs_Scroll(object sender, ScrollEventArgs e)
        {
            RefreshStartTimeMs();
        }

        private void RefreshStartTimeMs()
        {
            labelStartTimeMs.Text = hScrollBarStartTimeMs.Value + "[ms]";
            RefreshStartEndDuration();
        }

        private void hScrollBarEndTime_Scroll(object sender, ScrollEventArgs e)
        {
            RefreshEndTime();
        }

        private void RefreshEndTime()
        {
            TimeSpan timeSpan = new TimeSpan(0, 0, hScrollBarEndTime.Value);
            labelEndTimeSpan.Text = timeSpan.ToString(@"hh\:mm\:ss");

            if (hScrollBarEndTime.Value == hScrollBarEndTime.Maximum)
            {
                hScrollBarEndTimeMs.Maximum = m_tsDuration.Milliseconds;
            }
            else
            {
                hScrollBarEndTimeMs.Maximum = 999;
            }

            RefreshEndTimeMs();
        }

        private void hScrollBarEndTimeMs_Scroll(object sender, ScrollEventArgs e)
        {
            RefreshEndTimeMs();
        }

        private void RefreshEndTimeMs()
        {
            labelEndTimeMs.Text = hScrollBarEndTimeMs.Value + "[ms]";
            RefreshStartEndDuration();
        }

        private void RefreshStartEndDuration()
        {
            string sDuration = GetStartEndDuration();

            if (sDuration == "0.000")
            {
                labelGifDurationSec.Text = Properties.Resources.all;
            }
            else
            {
                labelGifDurationSec.Text = GetStartEndDuration() + "[s]";
            }

            RequestPreview();
        }

        private string GetStartEndDuration()
        {
            TimeSpan timeSpan1 = new TimeSpan(0, 0, 0, hScrollBarStartTime.Value, hScrollBarStartTimeMs.Value);
            TimeSpan timeSpan2 = new TimeSpan(0, 0, 0, hScrollBarEndTime.Value, hScrollBarEndTimeMs.Value);
            TimeSpan timeSpanDiff;

            if (timeSpan1 < timeSpan2)
            {
                timeSpanDiff = timeSpan2 - timeSpan1;
            }
            else
            {
                timeSpanDiff = timeSpan1 - timeSpan2;
            }

            return timeSpanDiff.TotalSeconds.ToString("0.000");
        }

        private void buttonPreviewMp4_Click(object sender, EventArgs e)
        {
            if (!IsExistFFmpeg(true))
            {
                return;
            }

            string sFilePath = textBoxInputFilePath.Text;

            if (!File.Exists(sFilePath))
            {
                MessageBox.Show(Properties.Resources.inputFileNotFound, Properties.Resources.warning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            SaveFileDialog saveFileDialog = new SaveFileDialog();

            saveFileDialog.Filter = "MP4 File (*.mp4)|*.mp4";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                if (sFilePath == saveFileDialog.FileName)
                {
                    MessageBox.Show(Properties.Resources.ioFilePathSame, Properties.Resources.error, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (checkBoxAddBorder.Checked)
                {
                    if (!MakeBorderBitmap())
                    {
                        return;
                    }
                }

                ProcessStartInfo processStartInfo = new ProcessStartInfo(m_sFFmpegExePath);
                processStartInfo.CreateNoWindow = false;
                processStartInfo.UseShellExecute = false;

                string sArguments = GetFFmpegArguments(sFilePath, ArgumentType.PREVIEW_MP4, "") + " \"" + saveFileDialog.FileName + "\"";

                processStartInfo.Arguments = sArguments;
                textBoxFFmpegArgument.Text = sArguments;

                Process process = Process.Start(processStartInfo);
                process.WaitForExit();

                if (process.ExitCode == 0)
                {
                    MessageBox.Show(Properties.Resources.processComplete, Properties.Resources.information, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show(Properties.Resources.ffmpegError, Properties.Resources.error, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        enum ArgumentType
        {
            PREVIEW_MP4,
            PREVIEW_GIF,
            ICON_GIF,
            PREVIEW_PIPE_START
        }

        private string GetFFmpegArguments(string sFilePath, ArgumentType eArgType, string sIconCropPos)
        {
            string sArguments = "";

            string sTrimTime = "";

            string sDuration = GetStartEndDuration();

            if (sDuration != "0.000")
            {
                TimeSpan timeSpanStart = new TimeSpan(0, 0, 0, hScrollBarStartTime.Value, hScrollBarStartTimeMs.Value);
                TimeSpan timeSpanEnd = new TimeSpan(0, 0, 0, hScrollBarEndTime.Value, hScrollBarEndTimeMs.Value);

                // ファイル開始端フラグ
                bool bSof = false;

                // ファイル終了端フラグ
                bool bEof = false;

                string sStartSec;

                if (timeSpanStart < timeSpanEnd)
                {
                    sStartSec = timeSpanStart.TotalSeconds.ToString("0.000");

                    // 開始時間が最小値か確認
                    if ((hScrollBarStartTime.Value == hScrollBarStartTime.Minimum) &&
                        (hScrollBarStartTimeMs.Value == hScrollBarStartTimeMs.Minimum))
                    {
                        bSof = true;
                    }

                    // 終了時間が最大値か確認
                    if ((hScrollBarEndTime.Value == hScrollBarEndTime.Maximum) &&
                        (hScrollBarEndTimeMs.Value == hScrollBarEndTimeMs.Maximum))
                    {
                        bEof = true;
                    }
                }
                else
                {
                    sStartSec = timeSpanEnd.TotalSeconds.ToString("0.000");

                    // 終了時間(=開始時間)が最大値か確認
                    if ((hScrollBarEndTime.Value == hScrollBarStartTime.Minimum) &&
                        (hScrollBarEndTimeMs.Value == hScrollBarStartTimeMs.Minimum))
                    {
                        bSof = true;
                    }

                    // 開始時間(=終了時間)が最大値か確認
                    if ((hScrollBarStartTime.Value == hScrollBarStartTime.Maximum) &&
                        (hScrollBarStartTimeMs.Value == hScrollBarStartTimeMs.Maximum))
                    {
                        bEof = true;
                    }
                }

                // 開始時間が最小値でないことを確認
                if (!bSof)
                {
                    sTrimTime = "-ss " + sStartSec + " ";
                }

                if (eArgType != ArgumentType.PREVIEW_PIPE_START)
                {
                    // 終了時間が最大値でないことを確認
                    if (!bEof)
                    {
                        sTrimTime += "-t " + sDuration + " ";
                    }
                }
            }

            string sPipePreview = "";

            if (eArgType == ArgumentType.PREVIEW_PIPE_START)
            {
                sPipePreview = "-vframes 1 -f image2pipe -vcodec png ";
            }

            string sPosition = "";

            if (comboBoxVideoFrame.SelectedIndex == 0)
            {
                // クロップ
                Size sizeOutput = CalcOutputSize();
                Size sizeCrop = CalcCropSize();

                string sCropStartPos = "";

                if (m_iInputSizeWidth == sizeCrop.Width)
                {
                    sCropStartPos = "0:" + hScrollBarCropPos.Value;
                }
                else
                {
                    sCropStartPos = hScrollBarCropPos.Value + ":0";
                }

                sPosition = "crop=" + sizeCrop.Width + ":" + sizeCrop.Height + ":" + sCropStartPos + "," +
                            "scale=" + sizeOutput.Width + ":" + sizeOutput.Height;
            }
            else
            {
                // レターボックス
                Size sizeOutput = CalcOutputSize();

                sPosition = "scale=w=" + sizeOutput.Width + ":h=" + sizeOutput.Height + ":force_original_aspect_ratio=decrease," +
                            "pad=w=" + sizeOutput.Width + ":h=" + sizeOutput.Height + ":x=(ow-iw)/2:y=(oh-ih)/2:color=#000000 ";
            }

            string sAddBorder1 = "";
            string sAddBorder2 = "";

            if (checkBoxAddBorder.Checked)
            {
                if ((eArgType == ArgumentType.PREVIEW_MP4) || (eArgType == ArgumentType.PREVIEW_GIF) || (eArgType == ArgumentType.PREVIEW_PIPE_START))
                {
                    sAddBorder1 = "-i \"" + m_sAppDir + Path.DirectorySeparatorChar + BORDER_IMAGE_NAME + "\" ";
                    sAddBorder2 = "[bg]; [bg][1:v]overlay=format=auto";
                }
            }

            string sIconCrop = "";

            if (sIconCropPos != "")
            {
                sIconCrop = "[iconbase]; [iconbase]crop=" + ICON_WIDTH + ":" + ICON_WIDTH + ":" + sIconCropPos;
            }

            string sPaletteGen = "";

            if ((eArgType == ArgumentType.PREVIEW_GIF) || (eArgType == ArgumentType.ICON_GIF))
            {
                sPaletteGen = "[gif]; [gif]split[a][b]; [a]palettegen[c]; [b][c]paletteuse";
            }

            string sFrameRate = "";

            if (eArgType != ArgumentType.PREVIEW_PIPE_START)
            {
                sFrameRate = "-r " + numericUpDownFrameRate.Value + " ";
            }

            sArguments = sTrimTime +
                         "-i \"" + sFilePath + "\" " +
                         sAddBorder1 +
                         sFrameRate +
                         "-filter_complex \"" +
                               "[0:v]scale=w=trunc(ih*dar/2)*2:h=trunc(ih/2)*2," +
                               "setsar=1/1," +
                               sPosition +
                               sAddBorder2 +
                               sIconCrop +
                               sPaletteGen +
                             "\" " +
                         sPipePreview +
                         "-an -y";

            return sArguments;
        }

        private void RequestPreview()
        {
            m_iPreviewWait = 500;
            timerMakePreview.Enabled = true;
        }

        private bool MakeBorderBitmap()
        {
            // ボーダー画像が保存済み かつ ボーダー画像が存在する か確認
            if (m_bBorderBitmapSaved && File.Exists(m_sAppDir + Path.DirectorySeparatorChar + BORDER_IMAGE_NAME))
            {
                return true;
            }

            Size sizeOutput = CalcOutputSize();
            using (Bitmap bitmapBorder = new Bitmap(sizeOutput.Width, sizeOutput.Height, PixelFormat.Format32bppArgb))
            {
                Graphics graphicsBorder = Graphics.FromImage(bitmapBorder);
                Bitmap bitmapIconBorder = new Bitmap(Properties.Resources.IconBorder);

                graphicsBorder.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceCopy;
                graphicsBorder.FillRectangle(Brushes.DimGray, graphicsBorder.VisibleClipBounds);

                for (int iRow = 0; iRow < numericUpDownIconLayoutRow.Value; iRow++)
                {
                    for (int iCol = 0; iCol < numericUpDownIconLayoutCol.Value; iCol++)
                    {
                        Point pointDraw = new Point(iCol * (m_iBorderWidth + ICON_WIDTH), iRow * (m_iBorderWidth + ICON_WIDTH));

                        graphicsBorder.DrawImage(bitmapIconBorder, pointDraw);
                    }
                }

                graphicsBorder.Dispose();
                bitmapIconBorder.Dispose();

                try
                {
                    bitmapBorder.Save(m_sAppDir + Path.DirectorySeparatorChar + BORDER_IMAGE_NAME, ImageFormat.Png);
                    m_bBorderBitmapSaved = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(Properties.Resources.failedSaveBordarImage + "\n\n" + ex.Message, Properties.Resources.error, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }

            return true;
        }

        private void buttonPreviewGif_Click(object sender, EventArgs e)
        {
            if (!IsExistFFmpeg(true))
            {
                return;
            }

            string sFilePath = textBoxInputFilePath.Text;

            if (!File.Exists(sFilePath))
            {
                MessageBox.Show(Properties.Resources.inputFileNotFound, Properties.Resources.warning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            SaveFileDialog saveFileDialog = new SaveFileDialog();

            saveFileDialog.Filter = "GIF File (*.gif)|*.gif";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                if (sFilePath == saveFileDialog.FileName)
                {
                    MessageBox.Show(Properties.Resources.ioFilePathSame, Properties.Resources.error, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (checkBoxAddBorder.Checked)
                {
                    if (!MakeBorderBitmap())
                    {
                        return;
                    }
                }

                ProcessStartInfo processStartInfo = new ProcessStartInfo(m_sFFmpegExePath);
                processStartInfo.CreateNoWindow = false;
                processStartInfo.UseShellExecute = false;

                string sArguments = GetFFmpegArguments(sFilePath, ArgumentType.PREVIEW_GIF, "") + " \"" + saveFileDialog.FileName + "\"";

                processStartInfo.Arguments = sArguments;
                textBoxFFmpegArgument.Text = sArguments;

                Process process = Process.Start(processStartInfo);
                process.WaitForExit();

                if (process.ExitCode == 0)
                {
                    MessageBox.Show(Properties.Resources.processComplete, Properties.Resources.information, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show(Properties.Resources.ffmpegError, Properties.Resources.error, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void buttonGenerateIcon_Click(object sender, EventArgs e)
        {
            if (!IsExistFFmpeg(true))
            {
                return;
            }

            string sFilePath = textBoxInputFilePath.Text;

            if (!File.Exists(sFilePath))
            {
                MessageBox.Show(Properties.Resources.inputFileNotFound, Properties.Resources.warning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            SaveFileDialog saveFileDialog = new SaveFileDialog();

            saveFileDialog.Filter = "GIF File (*.gif)|*.gif";
            saveFileDialog.OverwritePrompt = false;

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                ProcessStartInfo processStartInfo = new ProcessStartInfo(m_sFFmpegExePath);
                processStartInfo.CreateNoWindow = false;
                processStartInfo.UseShellExecute = false;

                // 上書き許可フラグ
                bool bAllowOverWrite = false;
                // 上書きファイル名
                string sOverWriteFileName = Path.GetFileNameWithoutExtension(saveFileDialog.FileName) + "_*" + Path.GetExtension(saveFileDialog.FileName);

                for (int iRow = 0; iRow < numericUpDownIconLayoutRow.Value; iRow++)
                {
                    for (int iCol = 0; iCol < numericUpDownIconLayoutCol.Value; iCol++)
                    {
                        int iNowNum = (iRow * (int)numericUpDownIconLayoutCol.Value) + iCol + 1;
                        string sOutputPath = Path.GetDirectoryName(saveFileDialog.FileName) + Path.DirectorySeparatorChar +
                                             Path.GetFileNameWithoutExtension(saveFileDialog.FileName) + "_" + iNowNum.ToString("00") + Path.GetExtension(saveFileDialog.FileName);

                        if (File.Exists(sOutputPath))
                        {
                            if (sFilePath == sOutputPath)
                            {
                                MessageBox.Show(Properties.Resources.ioFilePathSame, Properties.Resources.error, MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                            else if (!bAllowOverWrite)
                            {
                                if (DialogResult.OK == MessageBox.Show(Properties.Resources.confirmOverwrite + "\n\n" + sOverWriteFileName, Properties.Resources.warning, MessageBoxButtons.OKCancel, MessageBoxIcon.Warning))
                                {
                                    bAllowOverWrite = true;
                                }
                                else
                                {
                                    return;
                                }
                            }
                        }

                        string sIconCropPos = (iCol * (ICON_WIDTH + m_iBorderWidth)) + ":" + (iRow * (ICON_WIDTH + m_iBorderWidth));
                        string sArguments = GetFFmpegArguments(sFilePath, ArgumentType.ICON_GIF, sIconCropPos) + " \"" + sOutputPath + "\"";

                        processStartInfo.Arguments = sArguments;
                        textBoxFFmpegArgument.Text = sArguments;

                        Process process = Process.Start(processStartInfo);
                        process.WaitForExit();

                        if (process.ExitCode != 0)
                        {
                            MessageBox.Show(Properties.Resources.ffmpegError, Properties.Resources.error, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                }

                MessageBox.Show(Properties.Resources.processComplete, Properties.Resources.information, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void linkLabelTwitter_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://twitter.com/TatsuyaJp");
        }

        private void linkLabelGitHub_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://github.com/TatsuyaJp/StreamDeckIconConverter");
        }

        private void timerMakePreview_Tick(object sender, EventArgs e)
        {
            if (!backgroundWorkerMakePreview.IsBusy)
            {
                m_iPreviewWait -= timerMakePreview.Interval;

                if (m_iPreviewWait <= 0)
                {
                    m_iPreviewWait = 0;
                    timerMakePreview.Enabled = false;

                    if (!IsExistFFmpeg(false))
                    {
                        return;
                    }

                    string sFilePath = textBoxInputFilePath.Text;

                    if ((sFilePath == "") || !File.Exists(sFilePath))
                    {
                        return;
                    }

                    string sArguments = GetFFmpegArguments(sFilePath, ArgumentType.PREVIEW_PIPE_START, "") + " pipe:1";

                    backgroundWorkerMakePreview.RunWorkerAsync(sArguments);
                    textBoxFFmpegArgument.Text = sArguments;
                }
            }
        }

        private void backgroundWorkerMakePreview_DoWork(object sender, DoWorkEventArgs e)
        {
            if (checkBoxAddBorder.Checked)
            {
                if (!MakeBorderBitmap())
                {
                    return;
                }
            }

            ProcessStartInfo processStartInfo = new ProcessStartInfo(m_sFFmpegExePath);
            processStartInfo.CreateNoWindow = true;
            processStartInfo.UseShellExecute = false;
            processStartInfo.RedirectStandardOutput = true;
            processStartInfo.RedirectStandardError = false;
            processStartInfo.Arguments = (string)e.Argument;

            Process process = Process.Start(processStartInfo);

            MemoryStream msStdOut = new MemoryStream();

            // 標準出力を取得するタスクを作成
            Task taskGetStdOut = Task.Run(() =>
            {
                process.StandardOutput.BaseStream.CopyTo(msStdOut);
            });

            // 標準出力の取得完了を待つ
            Task.WaitAll(taskGetStdOut);

            // プロセスの終了を待つ
            process.WaitForExit();

            Image imagePreview = null;
            bool bDetectException = false;

            try
            {
                imagePreview = Image.FromStream(msStdOut);
            }
            catch (ArgumentException)
            {
                bDetectException = true;
            }

            if (!bDetectException && (process.ExitCode == 0))
            {
                e.Result = imagePreview;
            }
            else
            {
                e.Result = null;
            }
        }

        private void backgroundWorkerMakePreview_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Result == null)
            {
                pictureBoxPreview.Image = null;
            }
            else
            {
                pictureBoxPreview.Image = (Image)e.Result;

                // 画像サイズが表示領域を超えるか確認
                if ((pictureBoxPreview.Image.Width > pictureBoxPreview.Width) ||
                    (pictureBoxPreview.Image.Height > pictureBoxPreview.Height))
                {
                    // 縮小して表示
                    pictureBoxPreview.SizeMode = PictureBoxSizeMode.Zoom;
                }
                else
                {
                    // 等倍で表示
                    pictureBoxPreview.SizeMode = PictureBoxSizeMode.CenterImage;
                }
            }
        }

        private void checkBoxAddBorder_CheckedChanged(object sender, EventArgs e)
        {
            RequestPreview();
        }

        private void numericUpDownBorderWidth_ValueChanged(object sender, EventArgs e)
        {
            m_iBorderWidth = (int)numericUpDownBorderWidth.Value;
            PrintOutputSize();
            SetCropMax();
            m_bBorderBitmapSaved = false;
            RequestPreview();
        }
    }
}
