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
using System.IO;

namespace Cora
{
    public partial class Form1 : Form
    {
        SpeechRecognitionEngine recognitionEngine = new SpeechRecognitionEngine ();
        SpeechSynthesizer speechSynthesizer = new SpeechSynthesizer ();
        Random randomGen = new Random ();
        Boolean turnOn;
        string[] introductionsSpeech;
        string[] inputText;
        public Form1()
        {
            InitializeComponent();
            turnOn = false;
            introductionsSpeech = File.ReadAllLines("C:\\Documents\\Personal projects\\Speech asisstant\\Cora\\Cora_introductions.txt");

            inputText = new string[]{"Hi", "Hello", "How are you doing"};
            Grammar introductions = new Grammar(new GrammarBuilder(new Choices(inputText)));
            
            // same list from input text but can say "Cora" at the end such as "Hi Cora" instead of just "Hi"
            GrammarBuilder introBuild = new GrammarBuilder(new Choices(inputText));
            introBuild.Append("Cora");
            Grammar introWithName = new Grammar(introBuild);

            //Load the grammars in the recognition engine so the program knows which speech to recognize
            recognitionEngine.LoadGrammar(introductions);
            recognitionEngine.LoadGrammar(introWithName);

            recognitionEngine.SetInputToDefaultAudioDevice();
            
            // call this method after this event occurs
            recognitionEngine.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(speechRecognized);

            speechSynthesizer.SelectVoiceByHints(VoiceGender.Female);

        }

        private void speechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            string input = e.Result.Text;
            string output = "I could not understand the input";

            //iterate through the possible speech inputs
            for(int i = 0; i < inputText.Length; i++)
            {
                if (inputText[i] == input || inputText[i] + " Cora" == input)
                {
                    //output a random speech from the introductionsSpeech list
                    output = introductionsSpeech[randomGen.Next(introductionsSpeech.Length)];
                }
            }

            //output speech and update text box
            speechSynthesizer.SpeakAsync(output);
            richTextBox1.Text = input + "\n press button to speak to cora again";
            turnOn = false;
        }


        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(!turnOn)
            {
                recognitionEngine.RecognizeAsync(RecognizeMode.Single);
                turnOn = true;  
            } else
            {
                recognitionEngine.RecognizeAsyncCancel();
                turnOn = false; 
            }
        }
    }
}
