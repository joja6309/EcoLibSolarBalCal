using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Dimensions; 

namespace DXFInterface
{
    public class DxfIO : IDxfIO
    {
        public InputParser InParse;
        public OutputGenerator OutGen; 
        public DxfIO(string inputfilePath, string outputfilename)
        {
            InParse = new InputParser(inputfilePath);
            OutGen = new OutputGenerator(outputfilename);

        }
        public class OutputGenerator
        {
            private readonly string _outputFilePath;
            public OutputGenerator(string outname)
            {
                string desktopPath = Environment.GetFolderPath(System.Environment.SpecialFolder.DesktopDirectory);
                string outPath = desktopPath + "/" + outname + ".dxf"; 
                _outputFilePath = outPath; 

             }
             private FileStream CreateFile()
             {
                    FileStream fs = new FileStream(_outputFilePath, FileMode.OpenOrCreate, FileAccess.Write);
                    return fs; 
             }
            static class RandomLetter
            {
                static Random _random = new Random();
                public static string GetLetter()
                {
                    // This method returns a random lowercase letter.
                    // ... Between 'a' and 'z' inclusize.
                    int num = _random.Next(0, 26); // Zero to 25
                    char let = (char)('a' + num);
                    return Convert.ToString(let);
                }
            }
//            private string EntitiesTemplatingFunction(string uniqueId, int block_case, Tuple<double,double> centerpoint )
//            {
//                string template = String.Format(
//@" 
//0
//INSERT
//  5
//{1} 
//2
//100
//AcDbEntity
//  8
//HATCH 1
//100
//AcDbBlockReference
//  2
//EF3_HATCH_{0}
// 10
//{2}
// 20
//{3}
// 30
//0.0", , , ); }
        


            
            //public void GenerateFileOut(List<PanelBase> final_list)
            // {
            //    Random rand = new Random();
            //    int rand_num = rand.Next(0, 100); 
            //    string uniqueId = RandomLetter.GetLetter() + RandomLetter.GetLetter() + RandomLetter.GetLetter();
            //    Dictionary<string, string> InputDictionary = new Dictionary<string, string>(); 
                
            //    foreach (PanelBase pb in final_list)
            //    {


            //        switch (pb.BlockTotal)
            //        {
            //            case 1:
            //                rand_num += 1;
            //                uniqueId = uniqueId + rand_num.ToString();


            //            case 2:

            //            case 3:

            //            case 4:

            //            case 5:

            //            case 6:

            //            case 7: 

            //            default:
            //                break;
                        
                      
            //        }

                        
                            


            //    }

            // }
                
                 
                
             private void  TexttoFile(Dictionary<string, List<string>> outPutDic)
            {
                FileStream fs = CreateFile();
                StreamWriter sw = new StreamWriter(fs);
                {
                    foreach( string key in outPutDic.Keys)
                    {
                        sw.WriteLine(key);
                        foreach(string value in outPutDic[key])
                        {
                            sw.WriteLine(value); 
                        }
                    }

                }
                sw.Flush();
                sw.Close(); 
            }

        }
    
        public class InputParser
        {
            private readonly string _inputfilePath;
            private readonly string[] _inputfile;


            public InputParser(string inputPath)
            {
                _inputfilePath = inputPath;
                _inputfile = File.ReadAllLines(inputPath);
                ParseFile();


            }
            private BasicDimensions BlocksSectionValues = new BasicDimensions();
            private IFIPerimeter EntitiesIFI = new IFIPerimeter();
            private List<EcoPanel> EntitiesPanelList = new List<EcoPanel>();
            private string GetInputFilePath()
            {
                return _inputfilePath;
            }
            private void ParseFile()
            {

                int index = 0;
                int perm_start = 0;
                int perm_end = 0;
                int entity_start = 0;
                int entity_end = 0;
                foreach (string x in _inputfile)
                {


                    if (x.Contains("BLOCKS"))
                    {
                        perm_start = index;

                    }
                    if (perm_start != 0)
                    {
                        if (x.Contains("ENDSEC"))
                        {
                            perm_end = index;
                        }
                    }

                    if (x.Contains("ENTITIES"))
                    {
                        entity_start = index;

                    }
                    if (entity_start != 0)
                    {
                        if (x.Contains("ENDSEC"))
                        {
                            entity_end = index;
                        }
                    }

                    index += 1;
                }

                ParseBlocks(perm_start, perm_end);
                ParseEntities(entity_start, entity_end);
                return;

            }
            public BasicDimensions GetValuesFromBlockSection()
            {
                return BlocksSectionValues;
            }
            public List<EcoPanel> GetEntitiesPanels()
            {
                return EntitiesPanelList;
            }
            public IFIPerimeter GetIFIValues()
            {
                return EntitiesIFI;
            }
            private void ParseBlocks(int _blockstart, int _blockend)
            {


                var start = _blockstart;

                var end = _blockend;

                string[] perims_section = new string[end - start];

                Array.Copy(_inputfile, start, perims_section, 0, end - start);

                bool perm_name_condition = false;

                int index = 0;

                foreach (string x in perims_section)
                {
                    if (x.Contains("JA 320W 10DEG"))
                    {
                        perm_name_condition = true;
                    }
                    if (perm_name_condition)
                    {
                        if (x.Contains("AcDbPolyline"))
                        {

                            List<Double> temp_list = new List<Double>();

                            for (int i = 8; i <= 22; i += 2)
                            {
                                temp_list.Add(Convert.ToDouble(perims_section[index + i].Trim()));
                            }

                            BlocksSectionValues.Corner1 = new Tuple<double, double>(temp_list[0], temp_list[1]);
                            BlocksSectionValues.Corner2 = new Tuple<double, double>(temp_list[2], temp_list[3]);
                            BlocksSectionValues.Corner3 = new Tuple<double, double>(temp_list[4], temp_list[5]);
                            BlocksSectionValues.Corner4 = new Tuple<double, double>(temp_list[6], temp_list[7]);
                            break;

                        }

                    }
                    index += 1;
                }

            }
            private void ParseEntities(int _start, int _end)
            {
                var start = _start;
                var end = _end;
                string[] entites_section = new string[end - start];
                Array.Copy(_inputfile, start, entites_section, 0, end - start);
                int index = 0;
                int panel_count = 1;

                foreach (string x in entites_section)
                {


                    if (x.Contains("AcDbBlockReference") && (entites_section[index + 2].Contains("JA 320W 10DEG")))
                    {

                        List<double> temp_list = new List<double>();
                        for (int i = 4; i <= 8; i += 2)
                        {
                            temp_list.Add(Convert.ToDouble(entites_section[index + i].Trim()));

                        }
                        EcoPanel temp_base = new EcoPanel();
                        temp_base.Xvalue = temp_list[0];
                        temp_base.Yvalue = temp_list[1];
                        temp_base.PanelID = panel_count;
                        EntitiesPanelList.Add(temp_base);
                        panel_count = panel_count + 1;


                    }

                    if (x.Contains("IFI"))
                    {
                        List<Double> temp_list = new List<Double>();
                        for (int i = 0; i < 16; i += 2)
                        {
                            temp_list.Add(Convert.ToDouble(entites_section[index + 10 + i].Trim()));

                        }
                        EntitiesIFI.Corner1 = new Tuple<double, double>(temp_list[0], temp_list[1]);
                        EntitiesIFI.Corner2 = new Tuple<double, double>(temp_list[2], temp_list[3]);
                        EntitiesIFI.Corner3 = new Tuple<double, double>(temp_list[4], temp_list[5]);
                        EntitiesIFI.Corner4 = new Tuple<double, double>(temp_list[6], temp_list[7]);

                    }
                    index += 1;

                }

            }

        }
    
    }
}