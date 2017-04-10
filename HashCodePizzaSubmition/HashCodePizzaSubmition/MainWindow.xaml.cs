using System.Windows;
using Mylibrary;
using Microsoft.Win32;
using System.Text;
using System;
using System.IO;

// 2017 form Google HashCode
// Team BlackHorse
// Sorry for bad english and good luck :)

namespace HashCodePizzaSubmition
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        OpenFileDialog pizzaInputFile = null;
        int PizzaWidth = 0;
        int PizzaHeight = 0;
        int NeedIngredients = 0;
        int PieceSize = 0;

        int CutsCount = 0;
        int Score = 0;

        StringBuilder ResultFile = new StringBuilder();

        //Algorithm Start
        public void Start(object sender, RoutedEventArgs e)
        {
            // Open Pizza file
            openPizza();

            // i = 3: skip first line

            string[] fullPizza = ReadingandsaveFromFile.ReadingLiness(pizzaInputFile.FileName, 2, PizzaHeight + 1);

            for (int i = 0; i < PizzaHeight; i += 2)
            {
                string[] lines = new string[2];
                
                if (i + 1 >= PizzaHeight)
                {
                    CutLine(fullPizza[i], i);
                    break;             
                } 
                else
                {
                    Array.Copy(fullPizza, i, lines, 0, 2);
                }               
                
                CutDoubleLine(lines, i, i+1);
            }

            //create out.in
            writeResult();
        }

        public void CutLine(string line, int y)
        {
            int[] numericPizza = new int[PizzaWidth];

            for (int i = 0; i < PizzaWidth; i++)
            {
                if (line[i] == 'M')
                    numericPizza[i] = 1;
                else
                    numericPizza[i] = 0;
            }

            for (int i = 0; i < PizzaWidth; i += PieceSize)
            {
                int mashromCount = 0;
                int tomatoCount = 0;
                int pieceSize = 0;

                if (line[i] == 1)
                    mashromCount++;
                else
                    tomatoCount++;
                pieceSize++;

                if (mashromCount >= NeedIngredients && tomatoCount >= NeedIngredients)
                {
                    Score += pieceSize;
                    int width = (i + PieceSize - 1);
                    while (width >= PizzaWidth) width--;
                    ResultFile.AppendLine(y + " " + i + " " + y + " " + width);
                }                
            }
        }

        public void CutDoubleLine(string[] dLine, int y1, int y2)
        {
            int rectangleWidth = PieceSize / 2;

            int[,] numericPizza = new int[PizzaWidth, 2];

            // 0 - Tomato, 1 - Mushroom
            for (int line = 0; line < 2; line++)
            {
                for (int i = 0; i < PizzaWidth; i++)
                {
                    if (dLine[line][i] == 'M')
                        numericPizza[i, line] = 1;
                    else
                        numericPizza[i, line] = 0;
                }
            }

            // We begin to cut a piece of the size - (MaxPieceSize * 2)
            // Cut rectangles or lines

            int x1 = 0;

            while (x1 < PizzaWidth)
            {
                int countCurrentLines = 0;
                int countCurrentRectangles = 0;

                int pieceSize = 0;

                int printFirstLine = LineCut(numericPizza, x1, 0, ref pieceSize);
                int printSecondLine = LineCut(numericPizza, x1, 1, ref pieceSize);

                countCurrentLines = printFirstLine + printSecondLine;

                int printFirstRect = RectangleCut(numericPizza, x1, rectangleWidth);
                int printSecondRect = RectangleCut(numericPizza, x1 + rectangleWidth, rectangleWidth);

                countCurrentRectangles += printFirstRect + printSecondRect;

                // Do not give to go beyond pizza
                int doubleWidth = x1 + rectangleWidth * 2 - 1;
                while (doubleWidth >= PizzaWidth) doubleWidth--;

                //if all shapes are bad, insert ONE rectangle and go next
                if (countCurrentLines == 0 && countCurrentRectangles == 0)
                {
                    int width = (x1 + rectangleWidth - 1);
                    while (width >= PizzaWidth) width--;

                    //ResultFile.AppendLine(y1 + " " + x1 + " " + y2 + " " + width);
                    x1 += rectangleWidth;
                    continue;
                }

                if (countCurrentRectangles >= countCurrentLines) //choise best variant
                {
                    // Do not give to go beyond pizza
                    int width = (x1 + rectangleWidth - 1);
                    while (width >= PizzaWidth) width--;

                    if (printFirstRect == 1)
                    {
                        ResultFile.AppendLine(y1 + " " + x1 + " " + y2 + " " + width);
                        CutsCount++;
                    }

                    if (x1 + rectangleWidth < PizzaWidth && printSecondRect == 1)
                    {
                        ResultFile.AppendLine(y1 + " " + (x1 + rectangleWidth) + " " + y2 + " " + doubleWidth);
                        CutsCount++;
                    }
                    x1 += rectangleWidth * 2;
                }
                else
                {
                    // Do not give to go beyond pizza
                    int lineWidth = x1 + (PieceSize - 1);
                    while (lineWidth >= PizzaWidth) lineWidth--;

                    if (printFirstLine == 1)
                    {
                        ResultFile.AppendLine(y1 + " " + x1 + " " + y1 + " " + lineWidth);
                        CutsCount++;
                    }

                    if (printSecondLine == 1)
                    {
                        ResultFile.AppendLine(y2 + " " + x1 + " " + y2 + " " + lineWidth);
                        CutsCount++;
                    }

                    
                    x1 += PieceSize;
                }

                //Count score. 
                if (countCurrentLines == 2 || countCurrentRectangles == 2)
                {
                    // if two parts have the right amount of ingredients add piece size
                    Score += pieceSize;
                }
                else if (countCurrentLines == 1 || countCurrentRectangles == 1)
                {
                    // if only one part have the right amount of ingredients add half of piece size
                    Score += pieceSize / 2;
                }
            }
        }        

        public int RectangleCut(int[,] numericPizza, int x1, int rectangleWidth)
        {
            int mashromCount = 0;
            int tomatoCount = 0;

            for (int i = x1; i < x1 + rectangleWidth; i++)
            {
                try
                {
                    if (numericPizza[i, 0] == 1)
                        mashromCount++;
                    else
                        tomatoCount++;

                    if (numericPizza[i, 1] == 1)
                        mashromCount++;
                    else
                        tomatoCount++;
                }
                catch // if pizza line end
                {
                    break;
                }
            }

            if (mashromCount >= NeedIngredients && tomatoCount >= NeedIngredients)
            {
                return 1;
            }
            return 0;
        }

        public int LineCut(int[,] numericPizza, int x1, int line, ref int pieceSize)
        {
            int mashromCount = 0;
            int tomatoCount = 0;

            for (int i = x1; i < x1 + PieceSize; i++)
            {
                try
                {
                    if (numericPizza[i, line] == 1)
                        mashromCount++;
                    else
                        tomatoCount++;
                    pieceSize++;
                }
                catch // if pizza line end
                {
                    break;
                }
            }

            if (mashromCount >= NeedIngredients && tomatoCount >= NeedIngredients)
            {
                return 1;
            }
            return 0;
        }

        public void openPizza()
        {
            pizzaInputFile = new OpenFileDialog();
            pizzaInputFile.ShowDialog();
            string[] pizzaInfo = ReadingandsaveFromFile.ReadingLiness(pizzaInputFile.FileName, 1, 1)[0].Split(' ');

            PizzaHeight = int.Parse(pizzaInfo[0]);
            PizzaWidth = int.Parse(pizzaInfo[1]);
            NeedIngredients = int.Parse(pizzaInfo[2]);
            PieceSize = int.Parse(pizzaInfo[3]);

            //reset
            CutsCount = 0;
            Score = 0;
            ResultFile = new StringBuilder();

            Print("File Open: " + pizzaInputFile.FileName);
            Print("Size y: " + PizzaHeight);
            Print("Size x: " + PizzaWidth);
            Print("Min of elements: " + NeedIngredients);
            Print("Bit size: " + PieceSize);
        }

        public void writeResult()
        {
            StringBuilder finalResult = new StringBuilder();
            finalResult.AppendLine(CutsCount.ToString());
            finalResult.AppendLine(ResultFile.ToString());

            //finalResult.AppendLine("Score: " + Score); //if you want see score in file

            StreamWriter sw = new StreamWriter("-out.txt");
            sw.Write(finalResult.ToString().TrimEnd('\r', '\n'));
            sw.Close();
        }

        public void Print(string text)
        {
            Log.Text += text + "\n";
        }
    }
}