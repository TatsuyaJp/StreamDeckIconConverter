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
        const int BORDER_WIDTH = 36;
        // アプリケーションのディレクトリ
        string m_sAppDir = "";
        // 枠線画像ファイル名
        const string BORDER_IMAGE_NAME = "border.png";

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
            if (IsExistFFmpeg())
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    if (GetVideoInfo(openFileDialog.FileName))
                    {
                        textBoxInputFilePath.Text = openFileDialog.FileName;
                        RefreshFFmpegArguments();
                    }
                }
            }
        }

        private bool IsExistFFmpeg()
        {
            bool bExist = false;

            if (File.Exists(m_sFFmpegExePath))
            {
                bExist = true;
            }
            else if (File.Exists(m_sAppDir + Path.DirectorySeparatorChar + FFMPEG_EXE_NAME))
            {
                m_sFFmpegExePath = m_sAppDir + Path.DirectorySeparatorChar + FFMPEG_EXE_NAME;
            }
            else
            {
                if (DialogResult.OK == MessageBox.Show(Properties.Resources.ffmpegNotFound + "\n" + Properties.Resources.ffmpegDownloadConfirm, Properties.Resources.warning, MessageBoxButtons.OKCancel, MessageBoxIcon.Warning))
                {
                    Process.Start("https://www.gyan.dev/ffmpeg/builds/");
                    MessageBox.Show(Properties.Resources.ffmpegDownloadInformation, Properties.Resources.information, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }

            return bExist;
        }

        private bool GetVideoInfo(string sFilePath)
        {
            bool bResult = false;

            if (IsExistFFmpeg())
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
                    Regex regexInputinfo = new Regex(@"Stream #\d+:\d+.* Video: .*, (?<width>\d+)x(?<height>\d+) \[SAR (?<sarw>\d+):(?<sarh>\d+)", RegexOptions.Multiline);
                    Match matchInputInfo = regexInputinfo.Match(sError);
                    // 再生時間の検出
                    Regex regexDuration = new Regex(@"Duration: (?<hour>\d+):(?<min>\d+):(?<sec>\d+)\.(?<msec>\d+)");
                    Match matchDuration = regexDuration.Match(sError);

                    if (matchInputInfo.Success && matchDuration.Success)
                    {
                        // 動画サイズの算出
                        int iWidth = Int32.Parse(matchInputInfo.Groups["width"].Value);
                        int iHeight = Int32.Parse(matchInputInfo.Groups["height"].Value);
                        double dSar = (double)Int32.Parse(matchInputInfo.Groups["sarw"].Value) / Int32.Parse(matchInputInfo.Groups["sarh"].Value);
                        double dDar = ((double)iWidth / iHeight) * dSar;
                        iWidth = (int)Math.Floor(iHeight * dDar / 2.0) * 2;
                        iHeight = (int)Math.Floor(iHeight / 2.0) * 2;

                        // 再生時間の算出
                        m_tsDuration = new TimeSpan(Int32.Parse(matchDuration.Groups["hour"].Value), Int32.Parse(matchDuration.Groups["min"].Value), Int32.Parse(matchDuration.Groups["sec"].Value));
                        hScrollBarStartTime.Maximum = (int)Math.Floor(m_tsDuration.TotalSeconds);
                        hScrollBarEndTime.Maximum = (int)Math.Floor(m_tsDuration.TotalSeconds);
                        RefreshStartTime();
                        RefreshEndTime();

                        // 動画情報を表示
                        labelInputSize.Text = iWidth + "x" + iHeight;
                        labelInputDuration.Text = m_tsDuration.ToString("c");

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
        }

        private void numericUpDownIconLayoutRow_ValueChanged(object sender, EventArgs e)
        {
            PrintOutputSize();
            SetCropMax();
        }

        private void PrintOutputSize()
        {
            Size sizeOutput = CalcOutputSize();
            labelOutputSize.Text = sizeOutput.Width + "x" + sizeOutput.Height;
        }

        private Size CalcOutputSize()
        {
            Size sizeOutput = new Size();

            sizeOutput.Width = ((int)numericUpDownIconLayoutCol.Value * ICON_WIDTH) + (((int)numericUpDownIconLayoutCol.Value - 1) * BORDER_WIDTH);
            sizeOutput.Height = ((int)numericUpDownIconLayoutRow.Value * ICON_WIDTH) + (((int)numericUpDownIconLayoutRow.Value - 1) * BORDER_WIDTH);

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
            else if (m_iInputSizeWidth == sizeCrop.Width)
            {
                iCropMax = m_iInputSizeHeight - sizeCrop.Height;
                labelCropPosDir1.Text = Properties.Resources.top;
                labelCropPosDir2.Text = Properties.Resources.bottom;
            }
            else
            {
                iCropMax = m_iInputSizeWidth - sizeCrop.Width;
                labelCropPosDir1.Text = Properties.Resources.left;
                labelCropPosDir2.Text = Properties.Resources.right;
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
            RefreshFFmpegArguments();
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

            RefreshFFmpegArguments();
        }

        private void hScrollBarStartTime_Scroll(object sender, ScrollEventArgs e)
        {
            RefreshStartTime();
        }

        private void RefreshStartTime()
        {
            TimeSpan timeSpan = new TimeSpan(0, 0, hScrollBarStartTime.Value);
            labelStartTimeSpan.Text = timeSpan.ToString("c");
            RefreshStartEndDuration();
        }

        private void hScrollBarStartTimeMs_Scroll(object sender, ScrollEventArgs e)
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
            labelEndTimeSpan.Text = timeSpan.ToString("c");
            RefreshStartEndDuration();
        }

        private void hScrollBarEndTimeMs_Scroll(object sender, ScrollEventArgs e)
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

            RefreshFFmpegArguments();
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

        private void numericUpDownFrameRate_ValueChanged(object sender, EventArgs e)
        {
            RefreshFFmpegArguments();
        }

        private void buttonPreviewMp4_Click(object sender, EventArgs e)
        {
            if (!IsExistFFmpeg())
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

                processStartInfo.Arguments = GetFFmpegArguments(sFilePath, ArgumentType.PREVIEW_MP4, "") + " \"" + saveFileDialog.FileName + "\"";

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
            ICON_GIF
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

                string sStartSec;

                if (timeSpanStart < timeSpanEnd)
                {
                    sStartSec = timeSpanStart.TotalSeconds.ToString("0.000");
                }
                else
                {
                    sStartSec = timeSpanEnd.TotalSeconds.ToString("0.000");
                }

                sTrimTime = "-ss " + sStartSec + " " +
                            "-t " + sDuration + " ";
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

                sPosition = "scale=w=" + sizeOutput.Width + ":h=" + sizeOutput.Height + ":force_original_aspect_ratio=1," +
                            "pad=w=" + sizeOutput.Width + ":h=" + sizeOutput.Height + ":x=(ow-iw)/2:y=(oh-ih)/2:color=#000000 ";
            }

            string sAddBorder1 = "";
            string sAddBorder2 = "";

            if ((eArgType == ArgumentType.PREVIEW_MP4) || (eArgType == ArgumentType.PREVIEW_GIF))
            {
                sAddBorder1 = "-i \"" + m_sAppDir + Path.DirectorySeparatorChar + BORDER_IMAGE_NAME + "\" ";
                sAddBorder2 = "[bg]; [bg][1:v]overlay";
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

            sArguments = sTrimTime +
                         "-i \"" + sFilePath + "\" " +
                         sAddBorder1 +
                         "-r " + numericUpDownFrameRate.Value + " " +
                         "-filter_complex \"" +
                               "[0:v]scale=w=trunc(ih*dar/2)*2:h=trunc(ih/2)*2," +
                               "setsar=1/1," +
                               sPosition +
                               sAddBorder2 +
                               sIconCrop +
                               sPaletteGen +
                             "\" " +
                         "-an -y";

            return sArguments;
        }

        private void RefreshFFmpegArguments()
        {
            textBoxFFmpegArgument.Text = GetFFmpegArguments(textBoxInputFilePath.Text, ArgumentType.ICON_GIF, "0:0");
        }

        private bool MakeBorderBitmap()
        {
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
                        Point pointDraw = new Point(iCol * (BORDER_WIDTH + ICON_WIDTH), iRow * (BORDER_WIDTH + ICON_WIDTH));

                        graphicsBorder.DrawImage(bitmapIconBorder, pointDraw);
                    }
                }

                graphicsBorder.Dispose();
                bitmapIconBorder.Dispose();

                try
                {
                    bitmapBorder.Save(m_sAppDir + Path.DirectorySeparatorChar + BORDER_IMAGE_NAME, ImageFormat.Png);
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
            if (!IsExistFFmpeg())
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

                processStartInfo.Arguments = GetFFmpegArguments(sFilePath, ArgumentType.PREVIEW_GIF, "") + " \"" + saveFileDialog.FileName + "\"";

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
            if (!IsExistFFmpeg())
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
                            else if (DialogResult.OK != MessageBox.Show(Properties.Resources.confirmOverwrite + "\n\n" + Path.GetFileName(sOutputPath), Properties.Resources.warning, MessageBoxButtons.OKCancel, MessageBoxIcon.Warning))
                            {
                                return;
                            }
                        }

                        string sIconCropPos = (iCol * (ICON_WIDTH + BORDER_WIDTH)) + ":" + (iRow * (ICON_WIDTH + BORDER_WIDTH));
                        processStartInfo.Arguments = GetFFmpegArguments(sFilePath, ArgumentType.ICON_GIF, sIconCropPos) + " \"" + sOutputPath + "\"";

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
    }
}
