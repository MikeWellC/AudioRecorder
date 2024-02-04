using Microsoft.VisualBasic.Devices;
using NAudio.Lame;
using NAudio.Wave;

namespace WinFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        public static void RecordSystemAudio(string outFile, int msToRecord = 10000)
        {
            // Redefine the capturer instance with a new instance of the LoopbackCapture class
            WasapiLoopbackCapture CaptureInstance = new WasapiLoopbackCapture();

            // Redefine the audio writer instance with the given configuration
            LameMP3FileWriter RecordedAudioWriter = new LameMP3FileWriter(outFile, CaptureInstance.WaveFormat, LAMEPreset.ABR_128);
            // When the capturer receives audio, start writing the buffer into the mentioned file
            CaptureInstance.DataAvailable += (s, a) =>
            {
                // Write buffer into the file of the writer instance
                RecordedAudioWriter.Write(a.Buffer, 0, a.BytesRecorded);
            };

            // When the Capturer Stops, dispose instances of the capturer and writer
            CaptureInstance.RecordingStopped += (s, a) =>
            {
                RecordedAudioWriter.Dispose();
                RecordedAudioWriter = null;
                CaptureInstance.Dispose();
            };

            // Start audio recording !
            CaptureInstance.StartRecording();
            Thread.Sleep(msToRecord);
            CaptureInstance.StopRecording();
            MessageBox.Show("Â¼ÖÆÍê³É");
        }
        public static void RecordAudio(int msToRecord = 10000)
        {
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string outputPath = desktopPath + "\\" + Guid.NewGuid().ToString() + ".mp3";
            Thread sysThread = new Thread(() => RecordSystemAudio(outputPath, msToRecord));
            sysThread.Start();

        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != null)
            {
                Int32 sToRecord = Convert.ToInt32(textBox1.Text) * 1000;
                if (sToRecord > 0)
                    RecordAudio(sToRecord);
            }

        }
    }
}