using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace Sudoku_Solver
{
    class UI
    {
        private Board _board;
        public TextBox[,] TextBoxArray { get; }

        private readonly int _tbSize = 50;
        private readonly int _sudokuSize = 9;

        private readonly ComboBox _comboBoxDifficulty;
        private readonly Label _lblCurrentDifficulty;
        private readonly Label _lblTimer;
        private readonly Font _font = new Font("Microsoft Sans Serif", 28, FontStyle.Bold);
        private bool _lockedTiles = false;


        public UI(Board board, Form form)
        {
            _board = board;

            //Add Array of textboxes to form
            TextBoxArray = new TextBox[_sudokuSize, _sudokuSize];
            for (int x = 0; x < _sudokuSize; x++)
            {
                for (int y = 0; y < _sudokuSize; y++)
                {
                    TextBox textBox = CreateTextBox(_tbSize + x * _tbSize, _tbSize + y * _tbSize);

                    if (x >= 3 && x <= 5 && y >= 3 && y <= 5 || x >= 6 && (y <= 2 || y >= 6) ||
                        x <= 2 && (y <= 2 || y >= 6))
                    {
                        textBox.BackColor = Color.White;
                    }
                    else textBox.BackColor = Color.LightGray;
                    TextBoxArray[x, y] = textBox;
                    var x1 = x;
                    var y1 = y;
                    textBox.TextChanged += (sender, e) => OnTextChangeEvent(sender, e, x: x1, y: y1);
                    form.Controls.Add(textBox);

                }
            }

            //Adds Combobox to form
            _comboBoxDifficulty = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Text = "Easy",
                Location = new Point(550, 65),
                Width = 130
            };
            _comboBoxDifficulty.Items.Add("Easy");
            _comboBoxDifficulty.Items.Add("Normal");
            _comboBoxDifficulty.Items.Add("Hard");
            _comboBoxDifficulty.SelectedItem = "Easy";
            form.Controls.Add(_comboBoxDifficulty);

            //Add labels to form
            Label lblDifficulty = new Label
            {
                Name = "lblDifficultyLevel",
                Text = "Difficulty Level",
                Font = new Font("Arial", 8),
                Location = new Point(548, 50)
            };
            form.Controls.Add(lblDifficulty);

            _lblCurrentDifficulty = new Label
            {
                Name = "lblCurrentDifficulty",
                Text = "Difficulty: ",
                Font = new Font("Arial", 8),
                Location = new Point(47, 505)
            };
            form.Controls.Add(_lblCurrentDifficulty);

            _lblTimer = new Label
            {
                Name = "lblTimer",
                Text = "Time: ",
                Font = new Font("Arial", 8),
                Location = new Point(450, 505)
            };
            form.Controls.Add(_lblTimer);

            //Add buttons to form
            Button btnFetchSudoku = new Button
            {
                Text = "Generate Sudoku",
                Location = new Point(549, 90),
                Name = "btnGenerateSudoku",
                Size = new Size(132, 40)
            };
            btnFetchSudoku.Click += BtnFetchSudoku;
            form.Controls.Add(btnFetchSudoku);

            Button btnClear = new Button
            {
                Text = "Clear",
                Location = new Point(549, 158),
                Name = "btnClear",
                Size = new Size(65, 40)
            };
            btnClear.Click += btnClear_Click;
            form.Controls.Add(btnClear);

            Button btnClearAll = new Button
            {
                Text = "Clear all",
                Location = new Point(614, 158),
                Name = "btnClearAll",
                Size = new Size(65, 40)
            };
            btnClearAll.Click += btnClearAll_Click;
            form.Controls.Add(btnClearAll);

            Button btnSolve = new Button
            {
                Text = "Solve Sudoku",
                Location = new Point(549, 265),
                Name = "btnSolveSudoku",
                Size = new Size(132, 40)
            };
            btnSolve.Click += btnSolve_Click;
            form.Controls.Add(btnSolve);

            Button btnDebug = new Button
            {
                Text = "Debug",
                Location = new Point(600, 350),
                Name = "btnDebug",
                Size = new Size(130, 40),
            };
            btnDebug.Click += btnDebug_Click;
            form.Controls.Add(btnDebug);

            Button btnLock = new Button();
            btnLock.Text = "Lock";
            btnLock.Location = new Point(549, 200);
            btnLock.Size = new Size(130, 40);
            btnLock.Click += btnLock_Click;
            form.Controls.Add(btnLock);

        }

        public void UpdateUi()
        {
            for (int x = 0; x < 9; x++)
            {
                for (int y = 0; y < 9; y++)
                {
                    TextBoxArray[x, y].Text = _board.Tiles[x, y].Value.ToString();
                }
            }
        }

        private TextBox CreateTextBox(int x, int y)
        {
            TextBox textBox = new TextBox
            {
                MaxLength = 1,
                Size = new System.Drawing.Size(_tbSize, _tbSize),
                AutoSize = false,
                Font = _font,
                TextAlign = HorizontalAlignment.Center,
                ForeColor = Color.CornflowerBlue,
                Location = new Point(x, y),
                BorderStyle = BorderStyle.FixedSingle
            };
            return textBox;
        }

        public void ClearTextBoxes()
        {
            foreach (TextBox tb in TextBoxArray)
            {
                tb.Text = "";
                tb.Enabled = true;
            }
        }

        //Event handlers
        private void btnDebug_Click(object sender, EventArgs e)
        {
            Console.WriteLine(_board.ToString());
        }

        //Allows certain user input
        protected void OnTextChangeEvent(object sender, EventArgs e, int x, int y)
        {
            TextBox tb = sender as TextBox;
            if (tb == null)
            {
                Console.WriteLine("Error: Textbox handler by something that is not a textbox");
            }

            if (int.TryParse(tb.Text, out int number) == false || number == 0)
            {
                tb.Text = "";
                return;
            }
            _board.Tiles[x, y].SetValue(number);
        }

        //Fetches sudoku from API
        private void BtnFetchSudoku(object sender, EventArgs e)
        {
            SudokuRequest request = new SudokuRequest();
            string difficulty = Convert.ToString(_comboBoxDifficulty.SelectedIndex + 1);
            SaveFile deserializeSaveFile = request.GetSaveFile(difficulty, _board);

            if (deserializeSaveFile == null)
            {
                MessageBox.Show("Error: Unable to fetch sudoku from API. Check your internet connection.");
                return;
            }

            if (deserializeSaveFile.Response == false)
            {
                MessageBox.Show("Error: The sudoku API may be down");
                return;
            }

            ClearTextBoxes();
            _board.Clear();

            foreach (Squares square in deserializeSaveFile.Squares)
            {
                TextBox tb = TextBoxArray[square.X, square.Y];
                tb.Text = Convert.ToString(square.Value);
                tb.Enabled = false;
                _board.Tiles[square.X, square.Y].Locked = true;
            }
            _lblCurrentDifficulty.Text = $"Difficulty: {_comboBoxDifficulty.Text}";


        }

        //Clears Tiles which are not locked
        private void btnClear_Click(object sender, EventArgs e)
        {
            foreach (Tile tile in _board.Tiles)
            {
                if (!tile.Locked)
                {
                    tile.Clear();
                    TextBoxArray[tile.Position.X, tile.Position.Y].Text = null;
                }
            }
        }

        //Clear all Tiles
        private void btnClearAll_Click(object sender, EventArgs e)
        {
            foreach (Tile tile in _board.Tiles)
            {
                tile.Clear();
                TextBox tb = TextBoxArray[tile.Position.X, tile.Position.Y];
                tb.Text = null;
                tb.Enabled = true;
            }
        }

        //Solves sudoku
        private void btnSolve_Click(object sender, EventArgs e)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            Solver solver = new Solver(_board);
            solver.Solve();
            _board = solver.Solve().Item2;
            UpdateUi();
            stopwatch.Stop();
            _lblTimer.Text = $"Time: {stopwatch.ElapsedMilliseconds}ms";
            _lblTimer.Update();
        }

        //Locks & Unlocks all tiles
        private void btnLock_Click(object sender, EventArgs e)
        {
            Button button = sender as Button;
            if (button == null)
            {
                Console.WriteLine("Error: Button handler by something that is not a button");
                return;
            }
            if (_lockedTiles)
            {
                foreach (TextBox textBox in TextBoxArray)
                {
                    textBox.Enabled = true;
                }
                button.Text = "Lock";
            }
            else
            {
                foreach (TextBox textBox in TextBoxArray)
                {
                    textBox.Enabled = false;
                }
                button.Text = "Unlock";
            }
            _lockedTiles = !_lockedTiles;
        }
    }
}
