using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Mylibrary
{
    class ReadingandsaveFromFile
    {
        public static string[] ReadingLiness(string path, int first = 1, int last = -1)
        {
            string[] readText = File.ReadAllLines(path);
            int count = 0;
            if (first < 1)
                first = 1;
            if (last > 0)
            {
                if (last > readText.Length)
                    last = readText.Length;
                string[] forreturn = new string[last - (first - 1)];

                for (int i = first - 1; i < last; i++)
                {
                    forreturn[count] = readText[i];
                    count++;
                }
                return forreturn;
            }
            else
            {
                if (first == 1)
                {
                    string[] forreturn = new string[readText.Length];
                    for (int i = first - 1; i < readText.Length; i++)
                    {
                        forreturn[count] = readText[i];
                        count++;
                    }
                    return forreturn;
                }
                else
                {
                    string[] forreturn = new string[readText.Length - (first - 1)];
                    for (int i = first - 1; i < readText.Length; i++)
                    {
                        forreturn[count] = readText[i];
                        count++;
                    }
                    return forreturn;
                }

            }

        } // Рабочие возвращаяет из фала либо выбранные строки либо весь текст 
        public static void SaveFromString(string path, string[] readText, int first = 1, int last = -1)
        {
            int count = 0;
            if (first < 1)
                first = 1;
            if (last > 0)
            {
                if (last > readText.Length)
                    last = readText.Length;
                string[] forreturn = new string[last - (first - 1)];

                for (int i = first - 1; i < last; i++)
                {
                    forreturn[count] = readText[i];
                    count++;
                }
                File.WriteAllLines(path, forreturn);
            }
            else
            {
                if (first == 1)
                {
                    string[] forreturn = new string[readText.Length];
                    for (int i = first - 1; i < readText.Length; i++)
                    {
                        forreturn[count] = readText[i];
                        count++;
                    }
                    File.WriteAllLines(path, forreturn);
                }
                else
                {
                    string[] forreturn = new string[readText.Length - (first - 1)];
                    for (int i = first - 1; i < readText.Length; i++)
                    {
                        forreturn[count] = readText[i];
                        count++;
                    }
                    File.WriteAllLines(path, forreturn);
                }
            }
        } // Рабочие записывает выбранное в файл
        public static void SaveFromString(string path, string readText)
        {
            File.WriteAllText(path, readText);

        } // Рабочие записывает выбранное в файл
        public static void ClenFile(string path)
        {
            string text = "";
            File.WriteAllText(path, text);
        }
        public static bool IsNumber(char a)
        {
            try
            {
                int c = Convert.ToInt32(a);
                return true;
            }
            catch
            {
                return false;
            }
        } // проверяет равно ли значение числу 
        public static bool IsNumber(string a)
        {
            try
            {
                int c = Convert.ToInt32(a);
                return true;
            }
            catch
            {
                return false;
            }
        }// проверяет равно ли значение числу 
        public static bool IsNumber(string[] a, int indexofmas) // проверяет конкретную строку в массива строк равна ли она числу 
        {
            try
            {
                int c = Convert.ToInt32(a[indexofmas]);
                return true;
            }
            catch
            {
                return false;
            }
        }

    }
}
