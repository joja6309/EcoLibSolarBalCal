using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Dimensions;
using System.Linq;
//using Autodesk.AutoCAD.


namespace DXFInterface
{
    public class DxfIO
    {   
        private int block_start = 0;
        private int block_end = 0;
        private int entity_start = 0;
        private int entity_end = 0;
        private int tables_start = 0;
        private int tables_end = 0;
        private string BlockTitle;
        private readonly string _outputFilePath;
        private readonly string _inputFilePath;
        private readonly string[] _inputFile;
        private BasicDimensions BlocksSectionValues = new BasicDimensions();
        private IFIPerimeter EntitiesIFI = new IFIPerimeter();
        private List<EcoPanel> EntitiesPanelList = new List<EcoPanel>();
        private readonly bool isLandScape;
        public DxfIO(string inputfilePath, string outputfilename, bool land)
        { 
            _outputFilePath = outputfilename;
            _inputFilePath = inputfilePath;
            _inputFile = File.ReadAllLines(_inputFilePath);
            isLandScape = land;


        }
        public void SetBlockTitle(string panelName)
        {
            BlockTitle = panelName; 
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
       private string TableTemplatingFunction(string uniqueId, int block_case)
        {
            string template = @"{1}"; 
            string formated = String.Format(template, block_case, uniqueId);
            return formated;
        }
        private string EntitiesTemplatingFunction(string uniqueId, int block_case, Tuple<double, double> centerpoint)
        {
            var formated_X = Math.Round(centerpoint.Item1, 13);
            var formated_Y = Math.Round(centerpoint.Item2, 13);
            string template = @"  0" + Environment.NewLine + "INSERT" + Environment.NewLine +
                                "  5" + Environment.NewLine + "{1}" + Environment.NewLine +
                                "330" + Environment.NewLine + "2" + Environment.NewLine +
                                "100" + Environment.NewLine + "AcDbEntity" + Environment.NewLine +
                                "  8" + Environment.NewLine + "HATCH {0}" + Environment.NewLine +
                                "100" + Environment.NewLine + "AcDbBlockReference" + Environment.NewLine +
                                "  2" + Environment.NewLine + "EF3_HATCH_{0}" + Environment.NewLine +
                                " 10" + Environment.NewLine + "{2}" + Environment.NewLine +
                                " 20" + Environment.NewLine + "{3}" + Environment.NewLine +
                                " 30" + Environment.NewLine + "0.0";
            string formated_template = string.Format(template, block_case, uniqueId, formated_X, formated_Y);
            return formated_template;
        }
        private string EntitiesTextTemplatingFunction(string uniqueId, Tuple<double, double> centerpoint, double input)
        {
            var formated_X = Math.Round(centerpoint.Item1, 13) + 8;
            var formated_Y = Math.Round(centerpoint.Item2, 13);
            string template = @"  0" + Environment.NewLine + "TEXT" + Environment.NewLine +
                                "  5" + Environment.NewLine + "{0}" + Environment.NewLine +
                                "330" + Environment.NewLine + "2" + Environment.NewLine +
                                "100" + Environment.NewLine + "AcDbEntity" + Environment.NewLine +
                                "  8" + Environment.NewLine + "EF3 TEXT" + Environment.NewLine +
                                "100" + Environment.NewLine + "AcDbText" + Environment.NewLine +
                                " 10" + Environment.NewLine + "{1}" + Environment.NewLine +
                                " 20" + Environment.NewLine + "{2}" + Environment.NewLine +
                                " 30" + Environment.NewLine + "0.0" + Environment.NewLine +
                                " 40" + Environment.NewLine + "4.0" + Environment.NewLine +
                                "  1" + Environment.NewLine + "{3}" + Environment.NewLine +
                                "  7" + Environment.NewLine + "mytext" + Environment.NewLine +
                                "100" + Environment.NewLine + "AcDbText";
            string formated_template = string.Format(template, uniqueId, formated_X, formated_Y, input);
            return formated_template;
        }
        public void GenerateFileOut(List<Base> final_list, List<EcoPanel> PanelList)
        {
            Random rand = new Random();
            int rand_num = rand.Next(0, 20);
            string uniqueId = rand.Next(0,20).ToString() + rand.Next(0, 20).ToString() + rand.Next(0, 20).ToString();
            
            Dictionary<int, List<string>> tables_dic = new Dictionary<int, List<string>>();
            List<string> empty_list = new List<string>();
            string ent_string = ""; 
//          List<string> list_1_template = new List<string>();
//          List<string> list_2_template = new List<string>();
//          List<string> list_3_template = new List<string>();
//          List<string> list_4_template = new List<string>();
//          List<string> list_5_template = new List<string>();
//          List<string> list_6_template = new List<string>();
//          List<string> list_7_template = new List<string>();
            
            foreach (Base pb in final_list)
            {
                rand_num = rand_num + 1;
                string formated_number = String.Format("{0:00000}", rand_num);
                string newId = uniqueId + formated_number;
                
                if (pb.BallastBlockValue == 1)
                {
                    ent_string = ent_string + EntitiesTemplatingFunction(newId, 1, pb.Center);
                    //list_1_template.Add(TableTemplatingFunction(newId, 1));

                }
                    
                else if (pb.BallastBlockValue == 2)
                {
                    ent_string = ent_string + EntitiesTemplatingFunction(newId, 2, pb.Center);
                    //list_2_template.Add(TableTemplatingFunction(newId, 2));

                }
                   

                else if (pb.BallastBlockValue == 3)
                {
                    ent_string = ent_string + EntitiesTemplatingFunction(newId, 3, pb.Center);
                    //list_3_template.Add(TableTemplatingFunction(newId, 3));

                }
                   
                else if (pb.BallastBlockValue == 4)
                {
                    ent_string = ent_string + EntitiesTemplatingFunction(newId, 4, pb.Center);
                    //list_4_template.Add(TableTemplatingFunction(newId, 4));

                }
                else if (pb.BallastBlockValue == 5)
                {
                    ent_string = ent_string + EntitiesTemplatingFunction(newId, 5, pb.Center);
                    //list_5_template.Add(TableTemplatingFunction(newId, 5));

                }
                else if (pb.BallastBlockValue == 6)
                {
                    ent_string = ent_string + EntitiesTemplatingFunction(newId, 6, pb.Center);
                    //list_6_template.Add(TableTemplatingFunction(newId, 6));

                }
                else
                {
                    ent_string = ent_string + EntitiesTemplatingFunction(newId, 7, pb.Center);
                    //list_7_template.Add(TableTemplatingFunction(newId, 7));

                }
                if (pb != final_list[final_list.Count - 1 ])
                {
                    ent_string = ent_string + Environment.NewLine; 

                }


            }

            ent_string = ent_string + Environment.NewLine;
          foreach (Base pb in final_list)
          {
              rand_num = rand_num + 1;
              string formated_number = String.Format("{0:00000}", rand_num);
              string newId = uniqueId + formated_number;
              ent_string = ent_string + EntitiesTextTemplatingFunction(newId, pb.Center, Math.Round(pb.UnroundedBallastBlockValue, 3));
              if (pb != final_list[final_list.Count - 1])
              {
                  ent_string = ent_string + Environment.NewLine;
              }
          }
            ent_string = ent_string + Environment.NewLine;

            foreach (EcoPanel x in PanelList)
          {
              rand_num = rand_num + 1;
              string formated_number = String.Format("{0:00000}", rand_num);
              string newId = uniqueId + formated_number;
              ent_string = ent_string + EntitiesTextTemplatingFunction(newId, x.Center, Math.Round(x.ValueFromExcel, 3));
              if (x != PanelList[PanelList.Count - 1])
              {
                  ent_string = ent_string + Environment.NewLine;
              }
          }
            //          tables_dic.Add(1, list_1_template);
            //          tables_dic.Add(2, list_2_template);
            //          tables_dic.Add(3, list_3_template);
            //          tables_dic.Add(4, list_4_template);
            //          tables_dic.Add(5, list_5_template);
            //          tables_dic.Add(6, list_6_template);
            //          tables_dic.Add(7, list_7_template);
            TexttoFile(ent_string); 
        }
        private void TexttoFile(string entities_string)
        {
           
            int count = 0;
            List<string> new_file = new List<string>();
            foreach (var x in _inputFile)
            {
                new_file.Add(x);
                
                if (count.Equals(entity_start))
                {
                    new_file.Add(entities_string);
                    
                }
                count += 1;
            }


            using (StreamWriter outFile = new StreamWriter(_outputFilePath))
            {
                foreach (var line in new_file)
                {
                    outFile.WriteLine(line);

                }

                outFile.Flush();
                outFile.Close();
            }
        }
        public void ParseFile()
        {

            int index = 0;
            bool entities_hit = false;
            bool tables_hit = false;
            bool blocks_hit = false;

            foreach (string x in _inputFile)
            {

                if (x.Contains("SECTION") && (_inputFile[index + 2].Contains("BLOCKS")))
                {
                    block_start = index + 2;
                    blocks_hit = true;
                    Console.WriteLine(block_start);
                }
                if (blocks_hit)
                {
                    if (x.Contains("ENDSEC"))
                    {
                        block_end = index;
                        blocks_hit = false;
                        Console.WriteLine(block_end);
                    }
                }

                if (x.Contains("SECTION") && (_inputFile[index + 2].Contains("ENTITIES")))
                {
                    entity_start = index + 2;
                    entities_hit = true;
                }
                if (entities_hit)
                {
                    if (x.Contains("ENDSEC"))
                    {
                        entity_end = index;
                        entities_hit = false;
                    }
                }
                if (x.Contains("SECTION") && (_inputFile[index + 2].Contains("TABLES")))
                {
                    tables_start = index + 2;
                    tables_hit = true;
                }
                if (tables_hit != false)
                {
                    if (x.Contains("ENDSEC"))
                    {
                        tables_end = index;

                        tables_hit = false;
                    }
                }
                index += 1;
            }

            ParseBlocks(block_start, block_end);
            ParseEntities(entity_start, entity_end);
          // List<string> scan = ScanForPanels(); 
          // foreach(var x in scan)
          // {
          //     Console.WriteLine(x.ToString());
          // }
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

            Array.Copy(_inputFile, start, perims_section, 0, end - start);

            bool perm_name_condition = false;

            int index = 0;

            foreach (string x in perims_section)
            {
                if (x.Contains(BlockTitle))
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
            Array.Copy(_inputFile, start, entites_section, 0, end - start);
            int index = 0;
            int panel_count = 1;

            foreach (string x in entites_section)
            {


                if (x.Contains("AcDbBlockReference") && (entites_section[index + 2].Contains(BlockTitle)))
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
        public List<string> ScanForPanels()
        {
            int flag = 0;
            IEnumerable<string> listOflines  = _inputFile.Where(x => ((x.Contains("10") | x.Contains("5")) && (x.Contains("deg") | x.Contains("Deg") | x.Contains("DEG")))).Distinct();
            List<string> panelNameList = listOflines.ToList();
            foreach(var x in panelNameList)
            {
                Console.WriteLine(x);
            }
            Console.ReadKey();

            return panelNameList;
        }
    }
}

