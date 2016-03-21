using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;

namespace ConsoleApplication1
{
    class Array
    {
        public double sum;//油井砂岩，渗透率计算
        public Dictionary<string, double> dic = new Dictionary<string, double>();//添加每口井对应的水or油总产量
    }

    class Well
    {
        public string isWater;//判断是否水井
        public string isTwo;//判断是否二类油层
        public string isThree;//判断是否三类油层
        public Well(string water, string two, string three)
        {
            this.isWater = water;
            this.isTwo = two;
            this.isThree = three;
        }
    }

    
    class Program
    {
        const int NUMBER = 4000;//可能更改
        
        static void Main(string[] args)
        {

            string r = "150";
            string rpath1 = @"C:\Users\Sun\Desktop\data\0307data\excel" + r + ".txt";
            string rpath2 = @"C:\Users\Sun\Desktop\data\0307data\excel0" + r + ".txt";
            string wpath;
            Well[] wells = new Well[4];
            wells[0] = new Well("O", "A", "X");
            wells[1] = new Well("O", "X", "B");
            wells[2] = new Well("W", "A", "X");
            wells[3] = new Well("W", "X", "B");

            //for (int i = 0; i < 4; i++)
            //{
            //    if (wells[i].isTwo.Equals("I"))
            //    {
            //        wpath = @"C:\Users\Sun\Desktop\data\0307data\ysjs\" + r + "-" +"2"+ "-" + wells[i].isWater + ".txt";
            //    }
            //    else 
            //        wpath = @"C:\Users\Sun\Desktop\data\0307data\ysjs\" + r + "-" + "3" + "-" + wells[i].isWater + ".txt";
            //    a(wells[i], rpath1, rpath2, wpath);
            //}
            wpath = @"C:\Users\Sun\Desktop\data\0307data\ysjs\150-2&3-W.txt";
            a(wells[3], rpath1, rpath2, wpath);

        }


        public static void a(Well well, string rpath1, string rpath2, string wpath)
        {
            string strLine;//strLine读excel.txt
            string jh = "";
            Array array = new Array();
            try
            {
                FileStream aFile = new FileStream(rpath1, FileMode.Open);
                StreamReader sr = new StreamReader(aFile);


                strLine = sr.ReadLine();//跳过第一行
                strLine = sr.ReadLine();
                array = fileReader(rpath2, well);
                while (strLine != null)
                {
                    string[] nums = Regex.Split(strLine, @"\s+");
                    if (nums[2].Equals(well.isWater) && nums[17].Equals(well.isTwo) && (nums[15].Equals(well.isThree)))
                    //if (nums[2].Equals(well.isWater))//该语句只对2&3有效
                    {
                        if (!jh.Equals(nums[0].Substring(3, nums[0].Length - 3)))
                        {
                            jh = nums[0].Substring(3, nums[0].Length - 3);
                            fileWriter(jh, well, wpath, array);
                        }
                    }
                    strLine = sr.ReadLine();

                }
                sr.Close();
            }
            catch (IOException ex)
            {
                Console.WriteLine("An IOException has been thrown!");
                Console.WriteLine(ex.ToString());
                Console.ReadLine();
                return;
            }
        }


        private static Array fileReader(string path, Well well)
        {
            Array array = new Array();
            string strLine;
            string jh = "";
            //bool isWater = false;//判断是否水井
            //bool isTwo = false;//判断是否二类油层
            //bool isThree = false;//判断是否三类油层
            bool isNull = false;//判断渗透率是否为空
            double count = 0;//单口油or水井相加
            double total = 0;//所有油or水井相加
            double result;//一次计算结果
            try
            {
                FileStream aFile = new FileStream(path, FileMode.Open);
                StreamReader sr = new StreamReader(aFile);
                strLine = sr.ReadLine();//跳过第一行
                strLine = sr.ReadLine();
                while (strLine != null)
                {
                    string[] nums = Regex.Split(strLine, @"\s+");
                    //if (nums[2].Equals("W"))//判断油水井类别
                    //{
                    //    isWater = true;
                    //}
                    //else
                    //    isWater = false;
                    //if (nums[15].Equals("I"))//判断是否三类油层
                    //{
                    //    isThree = true;
                    //}
                    //else
                    //    isThree = false;
                    //if (nums[17].Equals("I"))//判断是否二类油层
                    //{
                    //    isTwo = true;
                    //}
                    //else
                    //    isTwo = false;
                    if (nums[9].Equals("-1"))//判断渗透率是否为空
                    {
                        isNull = true;
                    }
                    else
                        isNull = false;
                    //if (!isWater && isTwo && (!isThree))
                    // if (nums[2].Equals(well.isWater) && nums[17].Equals(well.isTwo) && (nums[15].Equals(well.isThree)))
                    if (nums[2].Equals(well.isWater))//该语句只对做2&3有效
                    {

                        if (!jh.Equals(nums[0].Substring(3, nums[0].Length - 3)))
                        {
                            if (jh.Length != 0)
                            {
                                array.dic.Add(jh, count);
                            }
                            count = 0;
                            jh = nums[0].Substring(3, nums[0].Length - 3);
                        }
                        if (isNull)
                        {
                            result = (Convert.ToDouble(nums[7]) / 3 * 0.01);
                        }
                        else
                        {
                            result = (Convert.ToDouble(nums[7]) * Convert.ToDouble(nums[9]));
                        }
                        if (nums[17].Equals(well.isTwo) && (nums[15].Equals(well.isThree)))//if语句只对做2&3有效
                        {
                            count += result;
                        }
                        //count += result;//其余使用
                        total += result;
                        array.sum = total;
                        
                    }
                    

                    strLine = sr.ReadLine();
                }

                sr.Close();
                return array;
            }
            catch (IOException ex)
            {
                Console.WriteLine("An IOException has been thrown!");
                Console.WriteLine(ex.ToString());
                Console.ReadLine();
            }
            return array;
        }


        private static void fileWriter(string jh, Well well, string path, Array array)
        {
            FileStream wFile = new FileStream(path, FileMode.Append);
            StreamWriter sw = new StreamWriter(wFile);
            double result;
            foreach (KeyValuePair<string, double> a in array.dic)
            {
                if (jh == a.Key)
                {
                    result = a.Value / array.sum * NUMBER;
                    if (well.isWater.Equals("W"))
                    {
                        if (well.isTwo.Equals("A"))
                        {
                            sw.WriteLine("INJECTOR MOBWEIGHT EXPLICIT '" + jh + "A_ij'");
                        }
                        else if (well.isThree.Equals("B"))
                        {
                            sw.WriteLine("INJECTOR MOBWEIGHT EXPLICIT '" + jh + "B_ij'");
                        }
                        sw.WriteLine("OPERATE  MAX  BHW  " + result.ToString("f2") + "  CONT");
                        sw.WriteLine("OPERATE  MAX  BHP  22000.  CONT");
                    }
                    else
                    {
                        sw.WriteLine("PRODUCER	'" + jh + "'");
                        sw.WriteLine("OPERATE	MAX	STL	" + result.ToString("f2"));
                    }
                }
            }
            sw.Close();
        }

       
    }

    

    
}
