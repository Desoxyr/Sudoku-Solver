using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;

namespace Sudoku_Solver
{
    public partial class Form1 : Form
    {
        private UI Ui { get; set; }
        private Board Board { get; set; }

        public Form1()
        {
            InitializeComponent();
            Board = new Board();
            Ui = new UI(Board, this);
        }
    }
}
