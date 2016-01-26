using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;

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
            if (GameState.logger || GameState.logger2)
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
            label1.Text = "";
            label2.Text = "";
            label3.Text = "";
            label4.Text = "";
            this.updateBoard(0);
        }

        public int[] updateBoard(int childrenCount)
        {
            int[] bwCount = new int[2];
            int whiteCount = 0;
            int blackCount = 0;
            currentState.calculatePossibleMoves();
            int[,] retdata = currentState.getData();
            for (int i = 0; i < GameState.puzzleSize; i++)
                for (int j = 0; j < GameState.puzzleSize; j++)
                {
                    gridTextBoxes[i, j].BackColor = currentState.data[i, j].getColor();
                    if (currentState.data[i, j].isWhite())
                        whiteCount++;
                    if (currentState.data[i, j].isBlack())
                        blackCount++;
                }
            // average elapsed time per turn
            int count = 0;
            int sum = 0;
            for (int i = 1; i < GameState.puzzleSize * GameState.puzzleSize; i = i + 2)
            {
                if (GameState.elapsedTime[i] == 0)
                    break;
                sum += GameState.elapsedTime[i];
                count++;
            }
            int averageElapsedTime;
            if (count == 0)
                averageElapsedTime = 0;
            else
                averageElapsedTime = sum / count;
            if (currentState.isComputersTurn())
            {
                label3.Text = "";
                label4.Text = "";
            }
            else
            {
                label3.Text = "Avg. comp. calculation time per turn: " + Convert.ToString(averageElapsedTime) + " ms.";
                label4.Text = "Nodes expanded in last turn: " + Convert.ToString(childrenCount);
                if (GameState.logger2)
                    if (currentState.turn > 1)
                    {
                        {
                            using (System.IO.StreamWriter file =
                                new System.IO.StreamWriter(@"D:\data.txt", true))
                            {
                                file.WriteLine(Convert.ToString(GameState.elapsedTime[currentState.turn - 1]) + " " + Convert.ToString(childrenCount));
                            }
                        }
                    }
                /*
                 * if (GameState.logger)
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
                 */
            }

            if (currentState.isComputersTurn())
                label1.Text = "Computer/White's turn";
            else
                label1.Text = "User/Black's turn";

            label2.Text = "Black: " + Convert.ToString(blackCount) + ", White: " + Convert.ToString(whiteCount);
            bwCount[0] = blackCount;
            bwCount[1] = whiteCount;
            return bwCount;
        }

        private void button_Click(object sender, EventArgs e)
        {
            if (currentState.gameEnded)
                return;
            int totalChildrenNum = 0;
            Random rnd = new Random();
            int ci = -1;
            int cj = -1;
            int[] c = {-1, -1};
            //int[,] retdata;
            string[] retstring1;
            //string[] retstring2;
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
                // check to see if no valid moves
                int n = currentState.calculatePossibleMoves();
                if (n == 0)
                {
                    ci = -2;
                    cj = -2;
                }
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
                    var watch = System.Diagnostics.Stopwatch.StartNew(); 
                    // contruct game tree
                    currentState.dumpData(0);
                    c = currentState.ConstructGameTree();
                    watch.Stop();
                    var elapsedMs = watch.ElapsedMilliseconds;
                    GameState.elapsedTime[currentState.turn] = Convert.ToInt32(elapsedMs);
                    //c = currentState.findBestMove();
                    ci = c[0];
                    cj = c[1];
                    totalChildrenNum = currentState.getChildNumRecursive();
                    currentState.numChildren = 0;
                }

            }
            // check if all cells are occupied
            bool boardFullFlag = true;
            for (int i = 0; i < GameState.puzzleSize; i++)
                for (int j = 0; j < GameState.puzzleSize; j++)
                    if (!((ci == i) && (cj == j)))
                        if (!currentState.data[i, j].isOccupied())
                            boardFullFlag = false;
            if ((boardFullFlag) || ((ci == -2) && (cj == -2)))
            {
                currentState.makeMove(ci, cj);
                currentState.noValidMoveCount++;
                if (currentState.noValidMoveCount == 2)
                {
                    // finish the game
                    currentState.gameEnded = true;
                    int[] bwCount = this.updateBoard(0);
                    if (bwCount[0] == bwCount[1])
                        label1.Text = "Draw!";
                    else
                    {
                        if (bwCount[0] > bwCount[1])
                            label1.Text = "User/Black wins!";
                        else
                            label1.Text = "Computer/White wins!";
                    }
                    return;
                }
            }
            else
            {
                if (currentState.makeMove(ci, cj) == -1)
                    return;
            };
            // update turn number
            currentState.nextMove();
            int[] dummy = this.updateBoard(totalChildrenNum);
//            retstring2 = currentState.getDataStr();
        }

    }

    public class GameState
    {
        public int getChildNumRecursive()
        {
            int c = 0;
            if (this.numChildren == 0)
                return 1;
            for (int i = 0; i < this.numChildren; i++)
                c += this.children[i].getChildNumRecursive();
            return c;
        }
        public bool gameEnded = false;
        public int noValidMoveCount = 0;
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
        /*public int[] findBestMove()
        {
            int[] s = new int[]{-1,-1};
            if (this.numChildren == 0)
            {
                // then no possible move.
                s[0] = -2;
                s[1] = -2;
                return s;
            }
            int ind = 0;
            int maxVal = this.children[0].getVal();
            for (int i = 1; i<this.numChildren; i++)
                if (this.children[i].getVal() > maxVal)
                {
                    ind = i;
                    maxVal = this.children[i].getVal();
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
         * */
        public int getVal()
        {
            if (this.numChildren == 0)
                this.val = this.getUtilVal();
            return val;
            /*
            int retVal;
            if (this.numChildren == 0)
                retVal = this.getUtilVal();
            else
            {
                retVal = this.children[0].getVal();
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
             */
        }
        public int val = Inf;
        /*
        public static int[,] squareWeight = {{100,-25,10,5,5,10,-25,100},
											 {25,25,2,2,2,2,25,25},
											 {10,2,5,1,1,5,2,10},
											 {5,2,1,2,2,1,2,5},
											 {5,2,1,2,2,1,2,5},
											 {10,2,5,1,1,5,2,10},
											 {25,25,2,2,2,2,25,25},
											 {100,-25,10,5,5,10,-25,100},};
         */
        
        public static int[,] squareWeight = {{100,-25,10,5,5,10,-25,100},
											 {-25 ,-25 ,2 ,2,2,2 ,-25 ,-25},
											 {10 ,2  ,5 ,1,1,5 ,2  ,10},
											 {5  ,2  ,1 ,2,2,1 ,2  ,5},
											 {5  ,2  ,1 ,2,2,1 ,2  ,5},
											 {10 ,2  ,5 ,1,1,5 ,2  ,10},
											 {-25 ,-25 ,2 ,2,2,2 ,-25 ,-25},
											 {100,-25,10,5,5,10,-25,100},};
        public bool utilValValid = false;
        public int utilVal = 0;
        public string getStateString()
        {
            string stateString = null;
            for (int i = 0; i < puzzleSize; i++)
                for (int j = 0; j < puzzleSize; j++)
                    if (this.data[i, j].isOccupied())
                        stateString += Convert.ToChar(data[i, j].val);
                    else
                        stateString += Convert.ToChar(Piece.empty);
            return stateString;
        }
        public static Hashtable calculatedStates = new Hashtable();
        public static int[] elapsedTime = new int[64];
        public int getUtilVal()
        {
            string stateString = "";
            if (!utilValValid)
            {
                if (GameState.useHashTable)
                {
                    // first check if hashtable contains this state or not
                    // first create 'key' string for given state
                    stateString = this.getStateString();
                    //if given state is in the hashtable return its value 
                    if (GameState.calculatedStates.ContainsKey(stateString))
                    {
                        utilVal = Int32.Parse(calculatedStates[stateString].ToString());
                        utilValValid = true;
                        return utilVal;
                    }
                }
                // if it is not in the hashtable calculate utility value


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
                        if (data[i, j].isOccupied())
                        {
                            if (data[i, j].isWhite())
                                tileDiffW++;
                            else
                                tileDiffB++;
                        };
                if (isComputersTurn())
                    tileDiff = tileDiffW - tileDiffB;
                else
                    tileDiff = tileDiffB - tileDiffW;

                //evaluate maximize mobility component
                this.turn++;
                mobilityB = this.calculatePossibleMoves();
                this.turn--;
                mobilityW = this.calculatePossibleMoves();
                if (isComputersTurn())
                    mobilityDiff = mobilityW - mobilityB;
                else
                    mobilityDiff = mobilityB - mobilityW;

                //calculate coefficients for tileDiff and mobilityDiff
                if (turn < 10)
                    coeffMaxTile = 1;
                else
                    coeffMaxTile = 40;
                if (turn < 20)
                    coeffMaxMobility = 1 * turn;
                else
                    coeffMaxMobility = 50;
                utilVal = coeffSquareWeight * sWeight + coeffMaxTile * tileDiff + coeffMaxMobility * mobilityDiff;

                utilValValid = true;

                if (GameState.useHashTable)
                    GameState.calculatedStates.Add(stateString, utilVal);
            }
            return utilVal;
        }
        public static int nodeCounter = 0;
        public static int maxNumChildren = 64;
        public int[] ConstructGameTree()
        {
            int[] s = new int[2];
            ConstructGameTreeRecursive(this, 0);
            if (this.numChildren == 0)
            {
                s[0] = -2;
                s[1] = -2;
                return s;
            }
            int max = this.children[0].getVal();
            int ind = 0;
            for (int i = 1; i < this.numChildren; i++)
                if (this.children[i].getVal() > max)
                {
                    max = this.children[i].getVal();
                    ind = i;
                }
            for (int i = 0; i<puzzleSize;i++)
                for (int j=0;j<puzzleSize;j++)
                    if (this.children[ind].data[i,j].isOccupied())
                        if (!this.data[i, j].isOccupied())
                        {
                            s[0] = i;
                            s[1] = j;
                        }
            return s;
        }
        public static int Inf = 10000000;
        public static void ConstructGameTreeRecursive(GameState node, int currDepth)
        {
            int j;
            int temp;
            bool breakFlag = false;
            int[] s = new int[] { -1, -1 };
            //temp = node.getUtilVal();
            if (currDepth >= maxDepth)
            {
                node.getVal();
                return;
            }
            int[] possibleMoveInd;
            //GameState next = new GameState();
            //GameState next = (GameState)node.MemberwiseClone();
            possibleMoveInd = node.getPossibleMoves();
            GameState[] next = new GameState[possibleMoveInd.Length];
            GameState grandParent = node.parent;
            for (int i = 0; i < possibleMoveInd.Length; i++)
            {
                next[i] = node.cloneTurnAndData();
                s = ind2sub(possibleMoveInd[i]);
                if (next[i].makeMove(s[0], s[1]) == -1)
                    return;
                next[i].nextMove();
                node.addChild(next[i]);
                ConstructGameTreeRecursive(next[i], currDepth + 1);
                next[i].dumpData(currDepth + 1);
                if (GameState.useAlphaBetaPr)
                {
                    // alpha-beta control
                    if (node.isComputersTurn())
                    {
                        
                        breakFlag = false;
                        if (grandParent != null)
                        {
                            for (j = 0; j < grandParent.numChildren; j++)
                                if (grandParent.children[j] == node)
                                    break;
                            for (int k = 0; k < j; k++)
                            {
                                if ((grandParent.children[k].val == Inf) || (grandParent.children[k].val == -Inf) || (next[i].val == Inf) || (next[i].val == -Inf))
                                    break;
                                if (grandParent.children[k].val < next[i].val)
                                {
                                    node.val = next[i].val;
                                    // prune!
                                    breakFlag = true;
                                    break;
                                }
                            }
                        }
                        if (breakFlag)
                            // daha fazla cocuk ekleme, cik
                            break;
                    }
                    else
                    {
                        breakFlag = false;
                        if (grandParent != null)
                        {
                            for (j = 0; j < grandParent.numChildren; j++)
                                if (grandParent.children[j] == node)
                                    break;
                            for (int k = 0; k < j; k++)
                            {
                                if ((grandParent.children[k].val == Inf) || (grandParent.children[k].val == -Inf) || (next[i].val == Inf) || (next[i].val == -Inf))
                                    break;
                                if (grandParent.children[k].val > next[i].val)
                                {
                                    node.val = next[i].val;
                                    // prune!
                                    breakFlag = true;
                                    break;
                                }
                            }
                        }
                        if (breakFlag)
                            // daha fazla cocuk ekleme, cik
                            break;
                    }
                }
            }
            // tum cocuklar eklendigine gore intervali guncelle
            int max = -Inf;
            int min = Inf;
            int currVal;
            GameState temp1 = node.parent;
            if (temp1 == null)
                temp = 1;
            for (int i = 0; i < node.numChildren; i++)
            {
                currVal = node.children[i].getVal();
                if (currVal > max)
                    max = currVal;
                if (currVal < min)
                    min = currVal;
            }
            if (node.isComputersTurn())
                // max node
                node.val = max;
            else
                node.val = min;
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
        public static int maxDepth = 4;
        public void addChild(GameState node_)
        {
            node_.parent = this;
            children[numChildren] = node_;
            numChildren++;
        }
        public static bool useHashTable = false;
        public static bool useAlphaBetaPr = true;
        public static bool logger = false;
        public static bool logger2 = false; // nodes ve sure bilgileri
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
                {
                    data[i, j] = new Piece();
                    //data[i, j].setWhite();
                };
            data[3, 3].setWhite();
            data[4, 4].setWhite();
            data[3, 4].setBlack();
            data[4, 3].setBlack();

            /*
            data[6, 6].setEmpty();
            data[6, 7].setEmpty();
            data[7, 7].setEmpty();
             * */

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
            this.noValidMoveCount = 0;
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
