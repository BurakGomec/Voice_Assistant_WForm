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


namespace VoiceControl
{
    public partial class Product : Form
    {
        List<string> word = new List<string>();
        public Product()
        {
            InitializeComponent();
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-US");
            AddNumberChoices();
            TextBoxControl();
            
        }

        private void Product_Load(object sender, EventArgs e)
        {
           DbOperations.GetList(dataGridView1);
           dataGridView1.FirstDisplayedScrollingRowIndex = dataGridView1.RowCount - 1;
           dataGridView1.ClearSelection();
        }

        private void AddNumberChoices()
        {
            for (int i = 0; i <= 1000; i++)
            {
                word.Add(i.ToString());
            }
        }

        private void ButtonAdd_Click(object sender, EventArgs e)
        {
            buttonAdd.Enabled = false;               
            textBox_piece.Text = "";
            textBox_price.Text = "";
            try
            {
                SpeechRecognitionEngine rec = new SpeechRecognitionEngine(new System.Globalization.CultureInfo("en-US"));
                Choices choices = new Choices();
                string[] words = { "name","brand","price","piece","book","phone","apple","tv","music player","philips","samsung","lg",
                    "smart watch","speaker","headphone","microphone"};
                foreach(var x in word)
                {
                    choices.Add(x);
                }
                choices.Add(words);
                Grammar g = new Grammar(new GrammarBuilder(choices));
                rec.LoadGrammar(g);
                rec.SetInputToDefaultAudioDevice();
                rec.RecognizeAsync(RecognizeMode.Multiple);
                rec.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(SpeechProductRecognize);
                
            }
            catch (InvalidOperationException)
            {
                MessageBox.Show("Lutfen bilgisayariniza 'en-US' paketi yukleyip tekrar deneyin", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch(Exception x)
            {
                MessageBox.Show(x.Message.ToString(),"Error!",MessageBoxButtons.OK,MessageBoxIcon.Error);

            }
        }
        private void TextBoxControl()
        {
            if (textBox_piece.Focus() || textBox_price.Focus()) { textBox_piece.Text = ""; textBox_price.Text = ""; }
        }
        private void SpeechProductRecognize(object sender, SpeechRecognizedEventArgs e)
        {
            try
            {
                string result = e.Result.Text;
                if (result == "name")
                {
                    textBox_name.Focus();
                    textBox_name.BackColor = Color.LightGreen;

                }
                if (textBox_name.BackColor == Color.LightGreen)
                {
                    if (result != "name" && result!="brand" && result!="price" && result!="piece")
                    {
                        textBox_name.Text = result;
                        textBox_name.BackColor = Color.White;
                    }
                }
                if (result == "brand")
                {
                    textBox_brand.Focus();
                    textBox_brand.BackColor = Color.LightGreen;
                }
                if (textBox_brand.BackColor == Color.LightGreen)
                {
                    if (result != "name" && result != "brand" && result!="price" && result!="piece")
                    {
                        textBox_brand.Text = result;
                        textBox_brand.BackColor = Color.White;
                    }
                }
                if (result == "price")
                {
                    textBox_price.Focus();
                    textBox_price.BackColor = Color.LightGreen;
                }
                if (textBox_price.BackColor == Color.LightGreen)
                {
                    if (result != "name" && result != "brand" && result!="price" && result!="piece")
                    {
                        textBox_price.Text = result;
                        textBox_price.BackColor = Color.White;
                    }
                }
                if (result == "piece")
                {
                    textBox_piece.Focus();
                    textBox_piece.BackColor = Color.LightGreen;
                }
                if (textBox_piece.BackColor == Color.LightGreen)
                {
                    if (result != "name" && result != "brand" && result!="price" && result!="piece")
                    {
                        textBox_piece.Text = result;
                        textBox_piece.BackColor = Color.White;
                    }
                }


                if (textBox_name.Text!="" && textBox_brand.Text!="" && textBox_piece.Text!="" && textBox_price.Text!="")
                {
                    DbOperations.AddProduct(textBox_name, textBox_brand, textBox_price, textBox_piece);
                    DbOperations.GetList(dataGridView1);
                }

            }
            catch (Exception x)
            {
                MessageBox.Show(x.Message.ToString(), "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ButtonManuelAdd_Click(object sender, EventArgs e)
        {
            if(textBox_name.Text=="" || textBox_brand.Text=="" || textBox_price.Text=="" || textBox_piece.Text == "")
            {
                MessageBox.Show("Lutfen tum alanlari doldurunuz","Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
            else
            {
                DbOperations.AddProduct(textBox_name, textBox_brand, textBox_price, textBox_piece);
                DbOperations.GetList(dataGridView1);
            }
           
        }
    }
}
