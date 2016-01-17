using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ai_proj_cs
{
    public partial class Form1 : Form
    {
        public GameState currentState;
        public Form1()
        {
            InitializeComponent();
            currentState = new GameState();
            gridTextBoxes = new System.Windows.Forms.PictureBox[puzzleSize, puzzleSize];
        }
        public System.Windows.Forms.PictureBox[,] gridTextBoxes;
        public int empty = 0;
        public int possible = 1;
        public int black = 2;
        public int white = 3;
        static public int puzzleSize = 8;

        public void Form1_Load(object sender, EventArgs e)
        {
            // and construct the grid
            int x0 = 13;
            int y0 = 50;
            int x = x0;
            int y = y0;
            // int s = 6;
            int sx = 24;
            int dsx = 2;
            int dsy = 2;
            int sy = 24;
            int tabInd = 0;
            //System.Windows.Forms.PictureBox[,] gridTextBoxes = new System.Windows.Forms.PictureBox[puzzleSize, puzzleSize];
            for (int i = 0; i < puzzleSize; i++)
            {
                for (int j = 0; j < puzzleSize; j++)
                {
                    gridTextBoxes[i, j] = new System.Windows.Forms.PictureBox();
                    gridTextBoxes[i, j].Name = Convert.ToString(i) + "_" + Convert.ToString(j);
                    gridTextBoxes[i, j].Location = new System.Drawing.Point(x, y);
                    gridTextBoxes[i, j].Size = new System.Drawing.Size(sx, sy);
                    gridTextBoxes[i, j].TabIndex = ++tabInd;
                    gridTextBoxes[i, j].Text = "";
                    //gridTextBoxes[i, j].TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
                    gridTextBoxes[i, j].BackColor = Color.Green;
                    gridTextBoxes[i, j].Click += new EventHandler(button_Click);
                    this.Controls.Add(gridTextBoxes[i, j]);

                    x += sx;
                    x += dsx;
                };

                y += sy;
                y += dsy;
                x = x0;
            };

            this.updateBoard();
        }

        public void updateBoard()
        {
            currentState.calculatePossibleMoves();
            int[,] retdata = currentState.getData();
            for (int i = 0; i < puzzleSize; i++)
                for (int j = 0; j < puzzleSize; j++)
                    gridTextBoxes[i, j].BackColor = currentState.data[i, j].getColor();
        }

        private void button_Click(object sender, EventArgs e)
        {
            Random rnd = new Random();
            int month = rnd.Next(1, 13); // creates a number between 1 and 12
            int dice = rnd.Next(1, 7);   // creates a number between 1 and 6
            int card = rnd.Next(52);     // creates a number between 0 and 51

            int ci = -1;
            int cj = -1;
            int temp;
            int[,] retdata;
            string[] retstring1;
            string[] retstring2;
            retstring1 = currentState.getDataStr();
            if (currentState.isUsersTurn())
            {
                // find which image is clicked.
                PictureBox clicked = (PictureBox)sender;
                string clickedName = clicked.Name;
                char[] delimiterChars = { '_' };
                string[] words = clickedName.Split(delimiterChars);
                ci = Convert.ToInt16(words[0]);
                cj = Convert.ToInt16(words[1]);
            }
            else
            {
                //make random move
                while (true)
                {
                    ci = rnd.Next(0, puzzleSize - 1);
                    cj = rnd.Next(0, puzzleSize - 1);
                    if (currentState.data[ci, cj].isPossible())
                        break;
                }

            }
            if (currentState.makeMove(ci, cj) == -1)
                return;
            currentState.nextMove();
            this.updateBoard();
            retstring2 = currentState.getDataStr();
            temp = 0;
        }
    }

    public class GameState
    {
        public int[,] getData()
        {
            int puzzleSize = ai_proj_cs.Form1.puzzleSize;
            int[,] retdata = new int[puzzleSize, puzzleSize];
            for (int i = 0; i < puzzleSize; i++)
                for (int j = 0; j < puzzleSize; j++)
                    retdata[i, j] = data[i, j].val;
            return retdata;
        }
        public string[] getDataStr()
        {
            int puzzleSize = ai_proj_cs.Form1.puzzleSize;
            string[] retstring = new string[puzzleSize];
            for (int i = 0; i < puzzleSize; i++)
                retstring[i] = "";
            for (int i = 0; i < puzzleSize; i++)
                for (int j = 0; j < puzzleSize; j++)
                    retstring[i] = retstring[i] + Convert.ToString(data[i, j].val);
            return retstring;
        }
        public GameState(Piece[,] data)
        {
            int puzzleSize = ai_proj_cs.Form1.puzzleSize;
            for (int i = 0; i < puzzleSize; i++)
                for (int j = 0; j < puzzleSize; j++)
                    data[i, j].setEmpty();
        }
        public GameState()
        {
            int puzzleSize = ai_proj_cs.Form1.puzzleSize;
            data = new Piece[puzzleSize, puzzleSize];
            for (int i = 0; i < puzzleSize; i++)
                for (int j = 0; j < puzzleSize; j++)
                    data[i, j] = new Piece();
            data[3, 3].setWhite();
            data[4, 4].setWhite();
            data[3, 4].setBlack();
            data[4, 3].setBlack();
        }
        public Piece[,] data;
        public int turn = 0;
        public bool isUsersTurn()
        {
            return ((turn % 2) == 0);
        }
        public bool isComputersTurn()
        {
            return ((turn % 2) == 1);
        }
        public void nextMove()
        {
            turn++;
        }
        public bool isPotentialCell(int x, int y)
        {
            int puzzleSize = ai_proj_cs.Form1.puzzleSize;
            // isolated_cell: kendisi bos olan ve en az bir komsusu dolu olan hucre.
            if (data[x, y].isOccupied())
                return false;
            for (int i = x - 1; i <= x + 1; i++)
            {
                for (int j = y - 1; j <= y + 1; j++)
                {
                    if (!checkInd(i, j))
                        continue;
                    if (i == 0 && j == 0)
                        continue;
                    if (data[i, j].isOccupied())
                        return true;
                }
            }
            return false;
        }
        public bool checkInd(int i, int j)
        {
            int puzzleSize = ai_proj_cs.Form1.puzzleSize;
            if ((i < 0) || (i >= puzzleSize) || (j < 0) || (j >= puzzleSize))
                return false;
            else
                return true;
        }
        public bool checkCellCurr(int i, int j)
        {
            if (!checkInd(i, j))
                return false;
            if (isComputersTurn())
                return data[i, j].isWhite();
            else
                return data[i, j].isBlack();
        }
        public bool checkCellOppn(int i, int j)
        {
            if (!checkInd(i, j))
                return false;
            if (isComputersTurn())
                return data[i, j].isBlack();
            else
                return data[i, j].isWhite();
        }
        public void setCellCurr(int i, int j)
        {
            if (!checkInd(i, j))
                return;
            if (isComputersTurn())
                data[i, j].setWhite();
            else
                data[i, j].setBlack();
        }
        public void calculatePossibleMoves()
        {
            int ci = -1;
            int cj = -1;
            int puzzleSize = ai_proj_cs.Form1.puzzleSize;
            for (int i = 0; i < puzzleSize; i++)
            {
                for (int j = 0; j < puzzleSize; j++)
                {
                    if (!data[i, j].isOccupied())
                        data[i, j].setEmpty();
                    if (isPotentialCell(i, j))
                    {
                        // sirayla her yonu kontrol et.
                        for (int di = -1; di <= 1; di++)
                        {
                            for (int dj = -1; dj <= 1; dj++)
                            {
                                if (di == 0 && dj == 0)
                                    continue; // cunku bu bir yon degil.
                                // bakalim bu hucrede rakibin tasi var miymis.
                                //					flag1 = Check_Cell(*State_Data,i+di,j+dj,oppn_color);
                                //					if (flag1) {
                                // simdi bu dogrultuda ilerleyebiliriz.
                                ci = i + di;
                                cj = j + dj;
                                while (true)
                                {
                                    if (!checkCellOppn(ci, cj))
                                        break;
                                    ci = ci + di;
                                    cj = cj + dj;
                                }
                                // eger ilerlemissek,
                                // simdi geldigimiz hucre kendi rengimizse [i,j] noktasi bir possible move'dur.
                                if ((ci == i + di) && (cj == j + dj))
                                {
                                    // demek ki hic ilerlememisiz.
                                }
                                else
                                {
                                    if (checkCellCurr(ci, cj))
                                        data[i, j].setPossible();
                                }
                            }
                        }
                    }
                }
            }
        }
        public int makeMove(int x, int y)
        {
            int count = -1;
            int ci;
            int cj;
            int c;
            int puzzleSize = ai_proj_cs.Form1.puzzleSize;
            if (!checkInd(x, y))
                return -1;
            if (!data[x, y].isPossible())
                return -1;
            else
            {
                // tas konan yerin komsuluklarini ara
                for (int di = -1; di <= 1; di++)
                {
                    for (int dj = -1; dj <= 1; dj++)
                    {
                        if (di == 0 && dj == 0)
                            // bu bir yon degil.
                            continue;
                        ci = x + di;
                        cj = y + dj;
                        // dogrultu boyunca takip edelim.
                        count = 0;
                        while (true)
                        {
                            if (checkCellOppn(ci, cj))
                            {
                                ci = ci + di;
                                cj = cj + dj;
                                count++;
                            }
                            else
                                break;
                        }
                        // durdugumuz noktada bizim tas varsa super.
                        if (checkCellCurr(ci, cj))
                        {
                            // aradaki taslari kendi rengimize cevirelim.
                            for (c = 0; c <= count; c++)
                            {
                                ci = ci - di;
                                cj = cj - dj;
                                setCellCurr(ci, cj);
                            }
                        }
                    }
                }
            }
            for (int i = 0; i < puzzleSize; i++)
                for (int j = 0; j < puzzleSize; j++)
                    if (data[i, j].isPossible())
                        data[i, j].setEmpty();
            return 0;
        }
    }

    public class Piece
    {
        public Piece()
        {
            val = empty;
        }
        public void setBlack()
        {
            val = black;
        }
        public void setWhite()
        {
            val = white;
        }
        public void setPossible()
        {
            val = possible;
        }
        public void setEmpty()
        {
            val = empty;
        }
        public bool isBlack()
        {
            return (val == black);
        }
        public bool isWhite()
        {
            return (val == white);
        }
        public bool isEmpty()
        {
            return (val == empty);
        }
        public bool isPossible()
        {
            return (val == possible);
        }
        public bool isOccupied()
        {
            if (isBlack())
                return true;
            if (isWhite())
                return true;
            return false;
        }
        public int val;
        private int white = 3;
        private int black = 2;
        private int empty = 0;
        private int possible = 1;
        public Color getColor()
        {
            if (isBlack())
                return Color.Black;
            if (isWhite())
                return Color.White;
            if (isPossible())
                return Color.Blue;
            if (isEmpty())
                return Color.Green;
            return Color.Gold;
        }
    }
}
