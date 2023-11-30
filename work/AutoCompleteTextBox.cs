using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace work
{
    public class AutoCompleteTextBox : TextBox
    {
        private ListBox _listBox;
        private bool _isAdded;
        private Dictionary<string, int> _values;
        private String _formerValue = String.Empty;
        private char[] delimiterChars = { ' ', ',', '.', ':', '\t', '\n' };
        private String lastChoise = String.Empty;

        public AutoCompleteTextBox()
        {
            InitializeComponent();
            ResetListBox();
        }

        private void InitializeComponent()
        {
            _listBox = new ListBox();
            KeyDown += this_KeyDown;
            KeyUp += this_KeyUp;
        }

        private void ShowListBox()
        {
            if (!_isAdded)
            {
                Parent.Controls.Add(_listBox);
                _listBox.Left = Left;
                _listBox.Top = Top;
                _isAdded = true;
            }
            _listBox.Visible = true;
            _listBox.BringToFront();
        }

        private void ResetListBox()
        {
            _listBox.Visible = false;
        }

        private void this_KeyUp(object sender, KeyEventArgs e)
        {
            UpdateListBox();
        }

        public void this_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                case Keys.Tab:
                    {
                        if (_listBox.Visible)
                        {
                            if (Text.LastIndexOf(" ") != -1)
                            {
                                Text = Text.Remove(Text.LastIndexOf(" "), SelectionStart - Text.LastIndexOf(" "));
                                Text += " ";
                                try {
                                    Text = Text.Insert(Text.LastIndexOf(" ") + lastChoise.Length + _listBox.SelectedItem.ToString().Length + 1, _listBox.SelectedItem.ToString() + " ");
                                    lastChoise = _listBox.SelectedItem.ToString();
                                }
                                catch {
                                    Text += _listBox.SelectedItem.ToString() + " ";
                                    lastChoise = _listBox.SelectedItem.ToString();
                                }
                            }
                            else
                                Text = _listBox.SelectedItem.ToString() + " ";
                            ResetListBox();
                            _formerValue = Text;
                            this.Select(Text.Length, 0);
                            e.Handled = true;
                        }
                        if (e.KeyCode == Keys.Enter && _isAdded || e.KeyCode == Keys.Tab && _isAdded)
                        {
                            e.SuppressKeyPress = true;
                        }
                        break;
                    }
                case Keys.Down:
                    {
                        if ((_listBox.Visible) && (_listBox.SelectedIndex < _listBox.Items.Count - 1))
                            _listBox.SelectedIndex++;
                        e.Handled = true;
                        break;
                    }
                case Keys.Up:
                    {
                        if ((_listBox.Visible) && (_listBox.SelectedIndex > 0))
                            _listBox.SelectedIndex--;
                        e.Handled = true;
                        break;
                    }
            }
        }

        protected override bool IsInputKey(Keys keyData)
        {
            switch (keyData)
            {
                case Keys.Tab:
                    if (_listBox.Visible)
                        return true;
                    else
                        return false;
                default:
                    return base.IsInputKey(keyData);
            }
        }

        private void UpdateListBox()
        {
            Point p = GetPositionFromCharIndex(SelectionStart > 0 ? SelectionStart - 1 : 0);
            if (Text == _formerValue)
                return;
            _formerValue = Text;
            string word = Text.Split(delimiterChars)[Text.Split(delimiterChars).Length - 1];
            if (_values != null && word.Length > 0)
            {
                var sort = _values.OrderByDescending(x => x.Value).ThenBy(x => x.Key).Select(x => x.Key).ToArray();
                string[] matches = Array.FindAll(sort,
                    x => (x.ToLower().StartsWith(word.ToLower())));;
                if (matches.Length > 0)
                {
                    ShowListBox();
                    _listBox.BeginUpdate();
                    _listBox.Items.Clear();
                    List<String> match = matches.Length < 5 ? matches.ToList() : matches.ToList().GetRange(0,5);
                    foreach (var matchItem in match)
                    {
                        _listBox.Items.Add(matchItem);
                    }
                    _listBox.SelectedIndex = 0;
                    _listBox.Height = 0;
                    _listBox.Width = 0;
                    _listBox.Left = Left + p.X + 15;
                    _listBox.Top = Top + p.Y + 25;
                    Focus();
                    using (Graphics graphics = _listBox.CreateGraphics())
                    {
                        for (int i = 0; i < _listBox.Items.Count; i++)
                        {
                            if (i < 20)
                                _listBox.Height += _listBox.GetItemHeight(i);
                            _listBox.Width = TextRenderer.MeasureText(matches.OrderByDescending(s => s.Length).First(), Font).Width;
                        }
                    }
                    _listBox.EndUpdate();
                }
                else
                {
                    ResetListBox();
                }
            }
            else
            {
                ResetListBox();
            }
        }

        public Dictionary<string, int> Values
        {
            get
            {
                return _values;
            }
            set
            {
                _values = value;
            }
        }

        public List<String> SelectedValues
        {
            get
            {
                String[] result = Text.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                return new List<String>(result);
            }
        }
    }
}
