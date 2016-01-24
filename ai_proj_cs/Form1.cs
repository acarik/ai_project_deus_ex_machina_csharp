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
            gridTextBoxes = new System.Windows.Forms.PictureBox[GameState.puzzleSize, GameState.puzzleSize];
        }
        public int nodeCounter = 0;
        public System.Windows.Forms.PictureBox[,] gridTextBoxes;
//        public static int empty = 0;
//        public static int possible = 1;
//        public static int black = 2;
//        public static int white = 3;
//        public static int puzzleSize = 8;

        public void Form1_Load(object sender, EventArgs e)
        {
            if (GameState.logger)
                if (System.IO.File.Exists(@"D:\data.txt"))
                    System.IO.File.Delete(@"D:\data.txt");
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
            for (int i = 0; i < GameState.puzzleSize; i++)
            {
                for (int j = 0; j < GameState.puzzleSize; j++)
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
            for (int i = 0; i < GameState.puzzleSize; i++)
                for (int j = 0; j < GameState.puzzleSize; j++)
                    gridTextBoxes[i, j].BackColor = currentState.data[i, j].getColor();
        }

        private void button_Click(object sender, EventArgs e)
        {
            Random rnd = new Random();
            int ci = -1;
            int cj = -1;
            int[] c = {-1, -1};
            int temp;
            //int[,] retdata;
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
                if (GameState.randomStrategy)
                    //make random move
                    while (true)
                    {
                        ci = rnd.Next(0, GameState.puzzleSize - 1);
                        cj = rnd.Next(0, GameState.puzzleSize - 1);
                        if (currentState.data[ci, cj].isPossible())
                            break;
                    }
                else
                {
                    // contruct game tree
                    currentState.dumpData(0);
                    currentState.ConstructGameTree();
                    c = currentState.findBestMove();
                    ci = c[0];
                    cj = c[1];
                    currentState.numChildren = 0;
                }

            }
            if (currentState.makeMove(ci, cj) == -1)
                return;
            // update turn number
            currentState.nextMove();
            this.updateBoard();
            retstring2 = currentState.getDataStr();
            temp = 0;
        }
    }

    public class GameState
    {
        public void dumpData(int currDepth)
        {
            if (GameState.logger)
            {
                string[] lines = new string[puzzleSize];
                string s;
                if (isComputersTurn())
                    s = "comp/white/" + Convert.ToString(Piece.white) + " moves";
                else
                    s = "user/black/" + Convert.ToString(Piece.black) + " moves";
                for (int i = 0; i < puzzleSize; i++)
                {
                    for (int j = 0; j < puzzleSize; j++)
                        if (data[i, j].isOccupied())
                            lines[i] += Convert.ToString(data[i, j].val);
                        else
                            lines[i] += "0";
                }
                using (System.IO.StreamWriter file =
                new System.IO.StreamWriter(@"D:\data.txt", true))
                {
                    if (this.parent == null)
                        file.WriteLine("curr:" + this.name + " at depth " + Convert.ToString(currDepth) + ", " + s + ", parent:" + "null" + ", util:" + Convert.ToString(this.getUtilVal()));
                    else
                        file.WriteLine("curr:" + this.name + " at depth " + Convert.ToString(currDepth) + ", " + s + ", parent:" + this.parent.name + ", util:" + Convert.ToString(this.getUtilVal()));
                    for (int i = 0; i < puzzleSize; i++)
                    {
                        file.WriteLine(lines[i]);
                    }
                }
            }
        }
        public void dumpTree()
        {
            if (GameState.logger)
            {
                this.dumpTreeRecursive();
            }
        }
        public void dumpTreeRecursive()
        {
            GameState curr = this;
            GameState r;
            string s;
            string s1;
            for (int i = 0; i < 250; i++)
            {
                s = "";
                if (curr == null)
                    break;
                for (int j = 0; j < curr.numChildren; j++)
                {
                    r = curr.children[j];
                    if (r.isComputersTurn())
                        s1 = "M";
                    else
                        s1 = "m";
                    s += "(" + r + r.name + ",";
                }
            }
        }
        public int[] findBestMove()
        {
            int[] s = new int[]{0,0};
            int ind = -1;
            int maxVal = -5000;
            for (int i = 0; i<this.numChildren; i++)
                if (this.children[i].getUtilVal() > maxVal)
                {
                    ind = i;
                    maxVal = this.children[i].getUtilVal();
                }
            // ind sirali cocuga gelmek icin ypailmasi gereken hamleyi bul
            for(int i=0;i<puzzleSize;i++)
                for(int j=0;j<puzzleSize;j++)
                    if (this.children[ind].data[i,j].isOccupied())
                        if (!this.data[i, j].isOccupied())
                        {
                            s[0] = i;
                            s[1] = j;
                        }
            return s;
        }
        public int getVal()
        {
            int retVal;
            if (this.numChildren == 0)
                retVal = this.getUtilVal();
            else
            {
                retVal = this.children[0].getUtilVal();
                for (int i = 1; i < this.numChildren; i++)
                    if (this.isComputersTurn())
                    {
                        // max
                        if (this.children[i].getVal() > retVal)
                            retVal = this.children[i].getVal();
                    }
                    else
                    {
                        // min
                        if (this.children[i].getVal() < retVal)
                            retVal = this.children[i].getVal();
                    }
            }
            return retVal;
        }
        
        public static int[,] squareWeight = {{100,-25,10,5,5,10,-25,100},
											 {25 ,25 ,2 ,2,2,2 ,25 ,25},
											 {10 ,2  ,5 ,1,1,5 ,2  ,10},
											 {5  ,2  ,1 ,2,2,1 ,2  ,5},
											 {5  ,2  ,1 ,2,2,1 ,2  ,5},
											 {10 ,2  ,5 ,1,1,5 ,2  ,10},
											 {25 ,25 ,2 ,2,2,2 ,25 ,25},
											 {100,-25,10,5,5,10,-25,100},};
        /*
        public static int[,] squareWeight = {{100,25,10,5,5,10, 25,100},
											 {25 ,25 ,2 ,2,2,2 ,25 ,25},
											 {10 ,2  ,5 ,1,1,5 ,2  ,10},
											 {5  ,2  ,1 ,2,2,1 ,2  ,5},
											 {5  ,2  ,1 ,2,2,1 ,2  ,5},
											 {10 ,2  ,5 ,1,1,5 ,2  ,10},
											 {25 ,25 ,2 ,2,2,2 ,25 ,25},
											 {100,25,10,5,5,10 ,25 ,100},};
        */
        public bool utilValValid = false;
        public int utilVal = 0;
        public int getUtilVal()
        {
            if (!utilValValid)
            {
                int sWeight = 0;
                int tileDiff = 0, tileDiffW = 0, tileDiffB = 0;
                int mobilityDiff = 0, mobilityW = 0, mobilityB = 0;
                int coeffSquareWeight = 100;
                int coeffMaxTile, coeffMaxMobility;
                //evaluate square weight component
                if (isComputersTurn())
                {
                    for (int i = 0; i < puzzleSize; i++)
                        for (int j = 0; j < puzzleSize; j++)
                            if (data[i, j].isWhite())
                                sWeight = sWeight + squareWeight[i, j];
                }
                else
                {
                    for (int i = 0; i < puzzleSize; i++)
                        for (int j = 0; j < puzzleSize; j++)
                            if (data[i, j].isBlack())
                                sWeight = sWeight + squareWeight[i, j];
                }
                //evaluate maximize tiles component
                for (int i = 0; i < puzzleSize; i++)
                    for (int j = 0; j < puzzleSize; j++)
                        if (data[i, j].isWhite())
                            tileDiffW++;
                        else
                            tileDiffB++;
                if (isComputersTurn())
                    tileDiff = tileDiffB - tileDiffW;
                else
                    tileDiff = tileDiffW - tileDiffB;

                //evaluate maximize mobility component
                turn++;
                mobilityB = calculatePossibleMoves();
                turn--;
                mobilityW = calculatePossibleMoves();
                mobilityDiff = mobilityW - mobilityB;

                //calculate coefficients for tileDiff and mobilityDiff
                if (turn < 50)
                    coeffMaxTile = 1;
                else
                    coeffMaxTile = 10;
                if (turn < 50)
                    coeffMaxMobility = 1 * turn;
                else
                    coeffMaxMobility = 50;
                utilVal = coeffSquareWeight * sWeight + coeffMaxTile * tileDiff + coeffMaxMobility * mobilityDiff;
                //utilVal = sWeight;//coeffSquareWeight * sWeight + coeffMaxTile * tileDiff + coeffMaxMobility * mobilityDiff;
                utilValValid = true;
            }
            return utilVal;
        }
        public static int nodeCounter = 0;
        public static int maxNumChildren = 64;
        public void ConstructGameTree()
        {
            ConstructGameTreeRecursive(this, 0);
        }
        public static void ConstructGameTreeRecursive(GameState node, int currDepth)
        {
            int temp = 0;
            //temp = node.getUtilVal();
            if (currDepth >= maxDepth)
                return;
            int[] c;
            int[] possibleMoveInd;
            //GameState next = new GameState();
            //GameState next = (GameState)node.MemberwiseClone();
            possibleMoveInd = node.getPossibleMoves();
            GameState[] next = new GameState[possibleMoveInd.Length];
            for (int i = 0; i < possibleMoveInd.Length; i++)
            {
                next[i] = node.cloneTurnAndData();
                c = ind2sub(possibleMoveInd[i]);
                if (next[i].makeMove(c[0], c[1]) == -1)
                    temp = 1;
                next[i].nextMove();
                node.addChild(next[i]);
                next[i].dumpData(currDepth + 1);
            }
            for (int i = 0; i < possibleMoveInd.Length; i++)
                ConstructGameTreeRecursive(next[i], currDepth + 1);
        }
        
        public GameState cloneTurnAndData()
        {
            GameState y = new GameState();
            // set data
            for (int i = 0; i < puzzleSize; i++)
                for (int j = 0; j < puzzleSize; j++)
                    y.data[i, j].val = this.data[i, j].val;
            y.turn = this.turn;
            return y;

        }
        public void setData(Piece[,] data_)
        {
            for (int i = 0; i < puzzleSize; i++)
                for (int j = 0; j < puzzleSize; j++)
                    data[i, j].val = data_[i, j].val;
        }
        public static bool randomStrategy = false;
        public static int maxDepth = 5;
        public void addChild(GameState node_)
        {
            node_.parent = this;
            children[numChildren] = node_;
            numChildren++;
        }
        public static bool logger = true;
        public static int[] ind2sub(int m)
        {
            int[] i = new int[2];
            i[0] = m / puzzleSize;
            i[1] = m % puzzleSize;
            return i;
        }
        public static int sub2ind(int i, int j)
        {
            return i * puzzleSize + j;
        }
        public int[] getPossibleMoves()
        {
            int[] possInd;
            this.calculatePossibleMoves();
            List<int> possIndList = new List<int>();
            for (int i = 0; i < puzzleSize; i++)
                for (int j = 0; j < puzzleSize; j++)
                    if (data[i, j].isPossible())
                        possIndList.Add(sub2ind(i, j));
            possInd = possIndList.ToArray();
            return possInd;
        }
        public int numChildren = 0;
        public string name;
        public static int puzzleSize = 8;
        public int[,] getData()
        {
            int puzzleSize = GameState.puzzleSize;
            int[,] retdata = new int[puzzleSize, puzzleSize];
            for (int i = 0; i < puzzleSize; i++)
                for (int j = 0; j < puzzleSize; j++)
                    retdata[i, j] = data[i, j].val;
            return retdata;
        }
        public string[] getDataStr()
        {
            int puzzleSize = GameState.puzzleSize;
            string[] retstring = new string[puzzleSize];
            for (int i = 0; i < puzzleSize; i++)
                retstring[i] = "";
            for (int i = 0; i < puzzleSize; i++)
                for (int j = 0; j < puzzleSize; j++)
                    retstring[i] = retstring[i] + Convert.ToString(data[i, j].val);
            return retstring;
        }
        public GameState parent;
        public GameState[] children;
        public GameState(GameState currGs, GameState parent_)
        {
            Piece[,] data = new Piece[puzzleSize, puzzleSize];
            for (int i = 0; i < puzzleSize; i++)
                for (int j = 0; j < puzzleSize; j++)
                    data[i, j] = new Piece(currGs.data[i, j].val);
            this.parent = parent_;
            this.children = new GameState[maxNumChildren];
        }
        public GameState(Piece[,] data_)
        {
            data = new Piece[puzzleSize, puzzleSize];
            for (int i = 0; i < puzzleSize; i++)
                for (int j = 0; j < puzzleSize; j++)
                    data[i, j] = new Piece(data_[i, j].val);
            this.children = new GameState[maxNumChildren];
        }
        public GameState()
        {
            int puzzleSize = GameState.puzzleSize;
            data = new Piece[puzzleSize, puzzleSize];
            for (int i = 0; i < puzzleSize; i++)
                for (int j = 0; j < puzzleSize; j++)
                    data[i, j] = new Piece();
            data[3, 3].setWhite();
            data[4, 4].setWhite();
            data[3, 4].setBlack();
            data[4, 3].setBlack();
            this.children = new GameState[maxNumChildren];
            this.name = Convert.ToString(GameState.nodeCounter++);
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
            int puzzleSize = GameState.puzzleSize;
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
            int puzzleSize = GameState.puzzleSize;
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
        public int calculatePossibleMoves()
        {
            int ci = -1;
            int cj = -1;
            int n = 0;
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
                                    {
                                        data[i, j].setPossible();
                                        n++;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return n;
        }
        public int makeMove(int x, int y)
        {
            int count = -1;
            int ci;
            int cj;
            int c;
            this.calculatePossibleMoves();
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
        public Piece(int val_)
        {
            val = val_;
        }
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
        public static int white = 3;
        public static int black = 2;
        public static int empty = 0;
        public static int possible = 1;
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
