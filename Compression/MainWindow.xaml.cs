using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;

namespace Compression
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

        }
        Dictionary<int, string> MapDic = new Dictionary<int, string>(); // dictionary
        Dictionary<int, string> MapComp = new Dictionary<int, string>();// map compression
        Dictionary<int, string> MapDecomp = new Dictionary<int, string>();// map decompression
        string text = "";
        List<int> ArrComp = new List<int>(); // for copmressed data 

        // compression
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            text = screen.Text;


            screen.Text = "";


            string curChar = ""; // current char
            int sizeDic = 128, check = 0, newDicIndex = 0;
            int checkBreak = 0;

            for (int indxTxt = 0; indxTxt < text.Length; indxTxt++)
            {

                curChar += text[indxTxt];

                while (curChar.Length > 1 /* at least 2 char */|| indxTxt + 1 == text.Length /*last char */)
                {

                    for (int indexDic = 128; indexDic <= sizeDic; indexDic++)
                    {
                        string compChar = "";
                        if (MapDic.Count != 0 && indexDic != sizeDic)

                            compChar = MapDic[indexDic]; // get data from dictionary to compare 

                        if (MapDic.Count == 0) // dictionary is empty


                            check = 1;

                        else

                            if (compChar == curChar) 
                            {
                                indxTxt++;

                                if (indxTxt + 1 < text.Length) 

                                    curChar += text[indxTxt]; // take the nxt char to save it in dictionary
                                else
                                    curChar += " "; // the end of text

                                check = 0;
                                newDicIndex = indexDic;

                            }
                            else if (compChar != curChar) // current char is not in dictionary
                                check = 1;


                    }
                    if (check == 1)
                    {
                       
                            indxTxt--; // back one char
                            if (curChar.Length > 1)
                            {
                                if (curChar[curChar.Length - 1] != ' ')
                                {
                                    MapDic.Add(sizeDic, curChar); // add new data
                                    sizeDic++;
                                }
                            }

                            else
                            {
                                ArrComp.Add((int)curChar[0]); // take the asscii for one char 
                                checkBreak = 1;
                                break;

                            }
                        if (curChar.Length > 1)
                        {
                            if (curChar.Length == 2)
                                ArrComp.Add((int)text[indxTxt]); // get th value of data 
                            else
                                ArrComp.Add(newDicIndex); 
                        }
                        curChar = "";
                        check = 0;
                        
                    }
                }
                if (checkBreak == 1)
                    break; 
            }
                // compressing
            for (int indexDic = 0; indexDic < ArrComp.Count; indexDic++)
            
                screen.Text += ArrComp[indexDic] + " ";


            MapDic.Clear();
            ArrComp.Clear();


        }
        // decompression
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {

            string[] CompRslt = screen.Text.Split();
            for (int i = 0; i < CompRslt.Length-1; i++) // convert from string to int
                ArrComp.Add(int.Parse(CompRslt[i]));
            screen.Text = "";
            text = "";

            MapDecomp.Add(ArrComp[0], (char)ArrComp[0] + ""); // add first element in dictionary
            // ArrComp is comperessed data
            screen.Text += (char)ArrComp[0];
       
            int sizeDic = 128;
            string curChar = "";
            curChar += (char)ArrComp[0];
            // fill the dictionary
            for (int indexComp = 1; indexComp < ArrComp.Count; indexComp++)
            {
                if (ArrComp[indexComp] < 128)
                {
                    try
                    {
                        // if one char then add it direct
               
                        MapDecomp.Add(ArrComp[indexComp], (char)ArrComp[indexComp] + ""); 
                    }
                    catch (Exception) { }

                    curChar += (char)ArrComp[indexComp];
                }
                else 
                {

                    if (sizeDic <= ArrComp[indexComp])
                    {
                        string ch = MapDecomp [ ArrComp[indexComp - 1]];

                        MapDecomp.Add(ArrComp[indexComp], ch[0] + MapDecomp[ArrComp[indexComp - 1]]);
                  
                    }
                }
               // decompress 
                for (int indexDic = 0; indexDic < MapDecomp.Count; indexDic++)
                {
                    text = MapDecomp[ArrComp[indexComp]] ;
                    if (MapDecomp[ArrComp[indexDic]] ==  text)
                    {

                        screen.Text += text;
                        if ( text.Length > 1 )
                        curChar += text.Substring(0, text.Length - 1 );
                        else
                        curChar += text ;
                      
                        try
                        {

                            MapDecomp.Add(sizeDic, curChar.Substring(0, curChar.Length - 1));
                           
                        }
                        catch (Exception)
                        {
                            try
                            {
                                // if data duplicated

                                MapDecomp.Add(sizeDic, curChar);
                            }
                            catch (Exception)
                            { }
                        }

                        if ( indexComp + 1 < ArrComp.Count && ArrComp[indexComp + 1] <= sizeDic)
                        
                            curChar = MapDecomp[ArrComp[indexComp]]; // get current compressed data
                        
                        sizeDic++;
                           
                        break;
                    }
                }
                   
            }
            // default variable
            ArrComp.Clear();
            MapDecomp.Clear();

        }

        
    }
}
