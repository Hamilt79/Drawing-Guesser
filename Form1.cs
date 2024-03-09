using System.Diagnostics.Contracts;
using System.Drawing.Drawing2D;

namespace Drawing_Guesser
{
    public partial class Form1 : Form
    {
        private List<RichTextBox>? wordTextBoxes;
        private List<RichTextBox>? boxesWithBorders;
        private Rectangle originalFormSize;
        private int firstLen = 0;
        private int secondLen = 0;
        private int thirdLen = 0;
        private char[]? fullWord;
        private bool blockChangedTextEvent = false;

        public Form1()
        {
            InitializeComponent();
        }

        private void wordInput_TextChanged(object sender, EventArgs e)
        {
            if (!blockChangedTextEvent)
            {
                for (int i = 0; i < wordTextBoxes.Count && i < fullWord.Length; i++)
                {
                    if (wordTextBoxes[i].Visible)
                    {
                        if (wordTextBoxes[i].Text == "")
                        {
                            fullWord[i] = '?';
                        }
                        else
                        {
                            fullWord[i] = wordTextBoxes[i].Text[0];
                        }
                    }
                    else
                    {
                        fullWord[i] = ' ';
                    }
                }
                ListPossibleWords();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            boxesWithBorders = new List<RichTextBox>();
            wordTextBoxes = new List<RichTextBox>();
            wordTextBoxes.Add(wordInput0);
            wordTextBoxes.Add(wordInput1);
            wordTextBoxes.Add(wordInput2);
            wordTextBoxes.Add(wordInput3);
            wordTextBoxes.Add(wordInput4);
            wordTextBoxes.Add(wordInput5);
            wordTextBoxes.Add(wordInput6);
            wordTextBoxes.Add(wordInput7);
            wordTextBoxes.Add(wordInput8);
            wordTextBoxes.Add(wordInput9);
            wordTextBoxes.Add(wordInput10);
            wordTextBoxes.Add(wordInput11);
            wordTextBoxes.Add(wordInput12);
            wordTextBoxes.Add(wordInput13);
            wordTextBoxes.Add(wordInput14);
            wordTextBoxes.Add(wordInput15);
            wordTextBoxes.Add(wordInput16);
            wordTextBoxes.Add(wordInput17);
            wordTextBoxes.Add(wordInput18);
            wordTextBoxes.Add(wordInput19);
            wordTextBoxes.Add(wordInput20);
            wordTextBoxes.Add(wordInput21);
            foreach(RichTextBox textBox in wordTextBoxes)
            {
                textBox.Hide();
                textBox.Enter += new EventHandler(_OnEnterHighlight);
                boxesWithBorders.Add(textBox);
                //allControlsForResize.Add(GetControlAsKeyValuePair(textBox));
            }
            boxesWithBorders.Add(firstWordLength);
            boxesWithBorders.Add(secondWordLength);
            boxesWithBorders.Add(thirdWordLength);
        }
        private KeyValuePair<Control, Rectangle> GetControlAsKeyValuePair(Control control)
        {
            return new KeyValuePair<Control, Rectangle>(control, new Rectangle(control.Location, control.Size));
        }

        private void SupressDingAll_KeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true;
            e.SuppressKeyPress = true;
        }
        
        private void SupressDingNums_KeyDown(object sender, KeyEventArgs e)
        {
            if (!int.TryParse(new KeysConverter().ConvertToString(e.KeyCode), out _) && e.KeyCode != Keys.Back && e.KeyCode != Keys.Left && e.KeyCode != Keys.Right)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }

        private void firstWordLength_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                int iout;
                firstLen = int.TryParse(firstWordLength.Text, out iout) ? iout : 0;
                ClearBoxes();
                DisplayBoxes(firstLen, secondLen, thirdLen);
            }
            catch { }
        }

        private void secondWordLength_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                int iout;
                secondLen = int.TryParse(secondWordLength.Text, out iout) ? iout : 0;
                ClearBoxes();
                DisplayBoxes(firstLen, secondLen, thirdLen);
            }
            catch { }
        }

        private void thirdWordLength_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                int iout;
                thirdLen = int.TryParse(thirdWordLength.Text, out iout) ? iout : 0;
                ClearBoxes();
                DisplayBoxes(firstLen, secondLen, thirdLen);
            }
            catch { }
        }

        private void Form_OnPaint(object? sender, PaintEventArgs? e)
        {
            foreach(RichTextBox box in boxesWithBorders)
            {
                
                if (box.Visible)
                {
                    Rectangle r = new Rectangle(box.Location.X - 1, box.Location.Y - 1, box.Width + 1, box.Height + 1);
                    Pen p = new Pen(Color.Black, 1);
                    this.CreateGraphics().DrawRectangle(p, r);
                }
                else
                {
                    Rectangle r = new Rectangle(box.Location.X - 1, box.Location.Y - 1, box.Width + 1, box.Height + 1);
                    Pen p = new Pen(this.BackColor, 3);
                    this.CreateGraphics().DrawRectangle(p, r);
                }
            }

        }
        
        private void _OnEnterHighlight(object? sender, EventArgs e)
        {
            ((RichTextBox)sender).SelectAll();
        }
        
        private void ClearBoxes()
        {
            blockChangedTextEvent = true;
            foreach (RichTextBox box in wordTextBoxes)
            {
                box.Text = "";
            }
            blockChangedTextEvent = false;
        }

        private void DisplayBoxes(int first, int second, int third)
        {
            fullWord = new char[first];
            if(second > 0)
            {
                fullWord = new char[first + second + 1];
            }
            if (third > 0)
            {
                fullWord = new char[first + second + third + 2];
            }

            foreach(RichTextBox box in wordTextBoxes)
            {
                box.Hide();
            }
            for(int i = 0; i < first && i < wordTextBoxes.Count; i++)
            {
                wordTextBoxes[i].Show();
            }
            if (second > 0)
            {
                for (int i = first + 1; i < first + second + 1 && i < wordTextBoxes.Count; i++)
                {
                    wordTextBoxes[i].Show();
                }
                if (third > 0)
                {
                    for (int i = first + second + 2; i < first + second + third + 2 && i < wordTextBoxes.Count; i++)
                    {
                        wordTextBoxes[i].Show();
                    }
                }
            }
            Form_OnPaint(null, null);
        }

        private void ListPossibleWords()
        {
            string fullWordString = "";
            foreach(char c in fullWord)
            {
                fullWordString += c;
            }
            List<string> possibleWords = new List<string>();
            
            foreach(string word in Words.WordsArr)
            {
                if(word.Length == fullWordString.Length)
                {
                    bool fail = false;
                    for(int i = 0; i < fullWordString.Length; ++i)
                    {
                        if (fullWordString[i] != '?' || word[i] == ' ')
                        {
                            if(fullWordString[i] != word[i])
                            {
                                fail = true;
                                break;
                            }
                        }
                    }
                    if (!fail)
                    {
                        possibleWords.Add(word);
                    }
                }
            }

            possibleWordsTextBox.Text = "";
            for(int i = 0; i < possibleWords.Count; ++i)
            {
                if(i == possibleWords.Count - 1)
                {
                    possibleWordsTextBox.Text += possibleWords[i];
                }
                else
                {
                    possibleWordsTextBox.Text += possibleWords[i] + "----";
                }
            }
        
        }

        private void resetButton_Click(object sender, EventArgs e)
        {
            blockChangedTextEvent = true;
            ClearBoxes();
            possibleWordsTextBox.Text = "";
            firstWordLength.Text = "";
            secondWordLength.Text = "";
            thirdWordLength.Text = "";
            blockChangedTextEvent = false;
        }

        private static GraphicsPath RoundedRect(Rectangle bounds, int radius)
        {
            int diameter = radius * 2;
            Size size = new Size(diameter, diameter);
            Rectangle arc = new Rectangle(bounds.Location, size);
            GraphicsPath path = new GraphicsPath();

            if (radius == 0)
            {
                path.AddRectangle(bounds);
                return path;
            }

            // top left arc  
            path.AddArc(arc, 180, 90);

            // top right arc  
            arc.X = bounds.Right - diameter;
            path.AddArc(arc, 270, 90);

            // bottom right arc  
            arc.Y = bounds.Bottom - diameter;
            path.AddArc(arc, 0, 90);

            // bottom left arc 
            arc.X = bounds.Left;
            path.AddArc(arc, 90, 90);

            path.CloseFigure();
            return path;
        }

    }
}