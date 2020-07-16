using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Speech;
using System.Speech.Recognition;//Sesi Tanıma
using System.Speech.Synthesis;
using System.Media;
using System.Diagnostics;
using System.Globalization;//totitlecase

namespace VoiceControl
{
    public partial class FormVoice : Form
    {
        SpeechRecognitionEngine rec = new SpeechRecognitionEngine();
        SpeechSynthesizer speech = new SpeechSynthesizer();
        SoundPlayer player = new SoundPlayer();
        string on =Application.StartupPath+"\\SiriOn.wav";
        string off =Application.StartupPath+ "\\SiriOff.wav";
        string understood =Application.StartupPath+"\\SiriUnderstood.wav";
        
        private void StartingVoice()
        {
            try
            {
                System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-US");
                Choices choices = new Choices();
                string[] words = { "hello", "open paint", "open word", "open google", "open youtube", "what time is it","how are you"
                ,"hey assistant","exit the application","stop listen"};
                choices.Add(words);
                Grammar grammar = new Grammar(new GrammarBuilder(choices));
                //Grammar grammar = new DictationGrammar();
                rec.LoadGrammar(grammar);
                rec.SetInputToDefaultAudioDevice();
                rec.RecognizeAsync(RecognizeMode.Multiple);
                rec.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(Speechrecognized);
            }
            catch (InvalidOperationException)
            {
                MessageBox.Show("Lutfen bilgisayarinizin dilini ingilizceye ceviriniz", "Error!");
                Application.Exit();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message.ToString(),"Error!");
            }


       
        }


        public FormVoice()
        {
            InitializeComponent();
        }

        private void Speechrecognized(object sender, SpeechRecognizedEventArgs e)
        {
            //timer1.Enabled = true;
            string result = e.Result.Text;
            string newy = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(result);
            richTextBox1.AppendText("You: " + newy + Environment.NewLine);
            if (result == "hey assistant")
            {
                result = "Hi,how can i help u ";
            }
            if (result == "hello")
            {
                result = "Hello,How are you";

            }
            if(result== "what time is it" )
            {
                result = "It is" + DateTime.Now.ToLongTimeString();
            }
            if(result.Contains("youtube"))
            {
                System.Diagnostics.Process.Start("https://www.youtube.com/?gl=TR");
                result = "Opening youtube";
            }
           
            if(result.Contains("google"))
            {
                result = "Opening google";
                System.Diagnostics.Process.Start("https://www.google.com.tr");
            }
            if(result.Contains("paint"))
            {
                result = "Opening paint";
                System.Diagnostics.Process.Start("MSpaint.Exe");
                
            }
            if(result == "open word")
            {
                result = "Opening word";
                
            }
            if(result == "exit the application")
            {
                result = "Okey closing it";
                //player.SoundLocation = off;
                //player.Play();
                Application.Exit();
                
            }
            if(result == "what time is it")
            {
                result = "It is" + DateTime.Now.ToLongTimeString();
            }
            if(result == "what time is it")
            {
                result = "It is" + DateTime.Now.ToLongTimeString();
            }
            //if (result.Contains("stop listen"))
            //{
                
            //    result = "Okey,i stopped listen";
            //    player.SoundLocation = understood;
            //    player.Play();
            //    speech.SpeakAsync(result);
            //    richTextBox1.AppendText("Assistant: " + result + Environment.NewLine);
            //    if(result.Contains("keep listening"))
            //    {
            //        result = "Okey,i listening you";

            //    }

            //}
       
            //else (result == "" || result==null)
            //{
            //    result = "Would you repeat?";
                
            //}

            player.SoundLocation = understood;
            player.Play();
            speech.SpeakAsync(result);
            richTextBox1.AppendText("Assistant: " + result + Environment.NewLine);
           
            

        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            player.SoundLocation = on;
            player.Play();
            StartingVoice();
            pictureBox1.Enabled = false;
        }

    }
}
