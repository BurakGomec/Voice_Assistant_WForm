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
using System.Speech.Recognition;
using System.Speech.Synthesis;
using System.Media;
using System.Diagnostics;
using System.Globalization; //totitlecase
using System.Threading;


namespace VoiceControl
{
    public partial class FormVoice : Form
    {
        SpeechRecognitionEngine rec = new SpeechRecognitionEngine(new System.Globalization.CultureInfo("en-US"));
        SpeechSynthesizer speech = new SpeechSynthesizer();
        SoundPlayer player = new SoundPlayer();
        readonly string on =Application.StartupPath+"\\SiriOn.wav";
        readonly string understood =Application.StartupPath+"\\SiriUnderstood.wav";
        
        private void StartingVoice()
        {
            try
            {
                System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-US");
                Choices choices = new Choices();
                string[] words = { "hello", "open paint", "open google", "open youtube", "what time is it","how are you"
                ,"hey assistant","exit the application","stop listen","open other form","show todays exchange rate"};
                choices.Add(words);
                Grammar grammar = new Grammar(new GrammarBuilder(choices));
                rec.LoadGrammar(grammar);
                rec.SetInputToDefaultAudioDevice();
                rec.RecognizeAsync(RecognizeMode.Multiple);
                rec.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(SpeechRecognized);
            }
            catch (InvalidOperationException)
            {
                MessageBox.Show("Lutfen bilgisayariniza 'en-US' paketi yukleyip tekrar deneyin", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message.ToString(), "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


       
        }
        public FormVoice()
        {
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-US");
            InitializeComponent();
        }

        private void ExchangeRate()
        {
            DataSet dsDovizKur = new DataSet();
            dsDovizKur.ReadXml(@"http://www.tcmb.gov.tr/kurlar/today.xml");
            richTextBox1.AppendText("Euro= " + dsDovizKur.Tables[1].Rows[3].ItemArray[4].ToString().Replace('.', ',')+Environment.NewLine);
            richTextBox1.AppendText("Dolar= " + dsDovizKur.Tables[1].Rows[0].ItemArray[4].ToString().Replace('.',',')+Environment.NewLine);
        }

        private void SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            
            string result = e.Result.Text;
            string newy = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(result);
            richTextBox1.AppendText("You: " + newy + Environment.NewLine);
            //timer1.Enabled = true;
            bool control = false;
            Random rd = new Random();
            if (result == "hey assistant")
            {
                int x = rd.Next(1,3);
                if (x == 1)
                {
                    control = true;
                    result = "I'm listening to you,you can use these words:";
                    richTextBox1.AppendText("Assistant: " + result + Environment.NewLine);
                    player.SoundLocation = understood;
                    player.Play();
                    speech.SpeakAsync(result);
                    richTextBox1.AppendText("\nHello, Open paint, Open google, Open youtube, What time is it, How are you," +
                    "Hey assistant, Exit the application, Stop listen, Open other form,Show Todays Exchange Rate..."+Environment.NewLine);
                }
                else
                {
                    result = "Hi,how can I help u";
                }
                
             
            }
            else if (result.Contains("are you"))
            {
                //better than say how are you 
                result = "I'm better now that I'm talking to you";
            }
            else if (result == "how are you")
            {
                result = "I'm better now that I'm talking to you";
            }
            else if (result == "hello")
            {
                
                int i=rd.Next(1, 3);
                if (i == 1)
                {
                    result = "Hello,How are you";
                }
                else
                {
                    result = "What can I do for you?";
                }

            }
            
            else if(result == "open youtube" )
            {
                System.Diagnostics.Process.Start("https://www.youtube.com/?gl=TR");
                result = "Opening youtube";
            }
           
            else if(result == "open google" )
            {
                result = "Opening google";
                System.Diagnostics.Process.Start("https://www.google.com.tr");
            }
            else if(result == "open paint")
            {
                result = "Opening paint";
                System.Diagnostics.Process.Start("MSpaint.Exe");
                
            }
            else if(result == "exit the application")
            {
                result = "Okey closing it";
                Application.Exit();
                
            }
            else if(result == "what time is it")
            {
                result = "It is " + DateTime.Now.ToLongTimeString();
            }
            else if(result == "show todays exchange rate")
            {
                result = "I listing todays exchange rate";
                player.SoundLocation = understood;
                player.Play();
                speech.SpeakAsync(result);
                ExchangeRate();
                control = true;
            }
            else if(result == "stop listen")
            {
                result = "I stopped listening to you";
                rec.RecognizeAsyncCancel();
                pictureBox1.Enabled = true;
                labelActivate.Visible = true;

            }
            else if(result=="open other form")
            {
                result = "Opening now";
                control = true;
                OpenAnotherForm();
                rec.Dispose();
                speech.Dispose();
            }
            else
            {
                result = "Please repeat";

            }
            if (!control)
            {
                richTextBox1.AppendText("Assistant: " + result + Environment.NewLine);
                player.SoundLocation = understood;
                player.Play();
                speech.SpeakAsync(result);
                Thread.Sleep(1200);

            }
            
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
            labelActivate.Visible = false;
           
        }
        private void OpenAnotherForm()
        {
            rec.Dispose();
            speech.Dispose();
            this.Hide();
            Product f2 = new Product();
            f2.FormClosing += f2_FormClosing;
            f2.ShowDialog();
        
        }
        private void Button1_Click(object sender, EventArgs e)
        {
            rec.Dispose();
            speech.Dispose();
            this.Hide();
            Product f2 = new Product();
            f2.FormClosing += f2_FormClosing;
            f2.ShowDialog();
        }
        private void f2_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Close();
            this.Dispose();
            
        }

    }
}
