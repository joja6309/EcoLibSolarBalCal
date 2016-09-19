using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Dimensions;


namespace DXFInterface
{
    public class DxfIO
    {   //5852
        //7649
        //public InputParser InParse;
        //public OutputGenerator OutGen;
        private int block_start = 0;
        private int block_end = 0;
        private int entity_start = 0;
        private int entity_end = 0;
        private int tables_start = 0;
        private int tables_end = 0;
        private int first_Acdb = 0;
        private string BlockTitle;
        private readonly string _outputFilePath;
        private readonly string _inputFilePath;
        private readonly string[] _inputFile;
        private BasicDimensions BlocksSectionValues = new BasicDimensions();
        private IFIPerimeter EntitiesIFI = new IFIPerimeter();
        private List<EcoPanel> EntitiesPanelList = new List<EcoPanel>();
        private readonly bool isLandScape;
        public DxfIO(string inputfilePath, string outputfilename, string panelName, bool land)
        { 
            _outputFilePath = outputfilename;
            _inputFilePath = inputfilePath;
            _inputFile = File.ReadAllLines(_inputFilePath);
            BlockTitle = panelName;
            isLandScape = land;

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

            string template = @"AcDbSymbolTableRecord" + Environment.NewLine + "100" + Environment.NewLine + "AcDbBlockTableRecord" +
                                         Environment.NewLine + " 2" + Environment.NewLine +
                                         "EF3_HATCH_{0}" + Environment.NewLine + "340" + Environment.NewLine +
                                         "0" + Environment.NewLine + "310" + Environment.NewLine +
                                         "{1}" + Environment.NewLine
                                         ;
            string formated = String.Format(template, block_case, uniqueId);
            return formated;
        }
        private string EntitiesTemplatingFunction(string uniqueId, int block_case, Tuple<double, double> centerpoint)
        {
            var formated_X = Math.Round(centerpoint.Item1, 13);
            var formated_Y = Math.Round(centerpoint.Item2, 13);
            string template = @" 0" + Environment.NewLine + "INSERT" + Environment.NewLine + " 5" + Environment.NewLine +
                                           "{1}" + Environment.NewLine + "330" + Environment.NewLine + "2" + Environment.NewLine + "100" + Environment.NewLine +
                                           "AcDbEntity" + Environment.NewLine + "  8" + Environment.NewLine +
                                           "HATCH 1" + Environment.NewLine + "100" + Environment.NewLine + "AcDbBlockReference" +
                                           Environment.NewLine + "  2" + Environment.NewLine
                                           + "EF3_HATCH_{0}" + Environment.NewLine +
                                           " 10" + Environment.NewLine + "{2}" + Environment.NewLine
                                           + " 20" + Environment.NewLine + "{3}" + Environment.NewLine +
                                           " 30" + Environment.NewLine + "0.0";
            string formated_template = String.Format(template, block_case, uniqueId, centerpoint.Item1, centerpoint.Item2);
            return formated_template;
        }

        public void GenerateFileOut(List<PanelBase> final_list)
        {
            Random rand = new Random();
            int rand_num = rand.Next(0, 20);
            string uniqueId = RandomLetter.GetLetter() + RandomLetter.GetLetter() + RandomLetter.GetLetter();
            Dictionary<string, string> InputDictionary = new Dictionary<string, string>();

            foreach (PanelBase pb in final_list)
            {
                rand_num = rand_num + 1;
                string newId = uniqueId + rand_num.ToString();
                if (pb.BlockTotal == 1)
                    InputDictionary[TableTemplatingFunction(newId, 1)] = EntitiesTemplatingFunction(newId, 1, pb.Center);
                else if (pb.BlockTotal == 2)
                    InputDictionary[TableTemplatingFunction(newId, 2)] = EntitiesTemplatingFunction(newId, 2, pb.Center);
                else if (pb.BlockTotal == 3)
                    InputDictionary[TableTemplatingFunction(newId, 3)] = EntitiesTemplatingFunction(newId, 3, pb.Center);
                else if (pb.BlockTotal == 4)
                    InputDictionary[TableTemplatingFunction(newId, 4)] = EntitiesTemplatingFunction(newId, 4, pb.Center);
                else if (pb.BlockTotal == 5)
                    InputDictionary[TableTemplatingFunction(newId, 5)] = EntitiesTemplatingFunction(newId, 5, pb.Center);
                else if (pb.BlockTotal == 6)
                    InputDictionary[TableTemplatingFunction(newId, 6)] = EntitiesTemplatingFunction(newId, 6, pb.Center);

            }
            TexttoFile(InputDictionary);
        }




        private void TexttoFile(Dictionary<string, string> outPutDic)
        {
            int count = 0;
            string long_string = "";
            string long_string_tables = "";
            //Console.WriteLine(entity_end);
            //Console.WriteLine(tables_end);
            //Console.WriteLine("============");
            //Console.ReadKey();
            foreach (var key in outPutDic.Keys)
            {
                long_string = long_string + outPutDic[key];
                long_string_tables = long_string_tables + key;
            }
            //Console.WriteLine(long_string_tables);
            //Console.ReadKey(); 
            var text = new StringBuilder();


            List<string> new_file = new List<string>();
            foreach (var x in _inputFile)
            {
                new_file.Add(x);
                if (count == (tables_end - 1))
                {
                    //outFile.Write(long_string_tables + Environment.NewLine);
                    foreach (var key in outPutDic.Keys)
                    {
                        new_file.Add(key);
                        Console.WriteLine(key);
                    }


                }

                if (count == (entity_end - 2))
                {
                    foreach (var key in outPutDic.Keys)
                    {
                        new_file.Add(outPutDic[key]);
                    }

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

                if (x.Contains("BLOCKS"))
                {
                    block_start = index;
                    blocks_hit = true;

                }
                if (blocks_hit)
                {
                    if (x.Contains("ENDSEC"))
                    {
                        block_end = index;
                        blocks_hit = false;
                    }
                }

                if (x.Contains("ENTITIES"))
                {
                    entity_start = index;
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
                if (x.Contains("TABLES") && (_inputFile[index + 2].Contains("TABLE")))
                {
                    tables_start = index;
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

        private List<int> ScanForMarker(string marker, string[] section)
        {
            List<int> listOfIndicies = new List<int>();
            int line_count = 0;
            foreach (var line in section)
            {
                if (line.Contains(marker))
                {
                    listOfIndicies.Add(line_count);
                }
                line_count = line_count + 1;
            }
            return listOfIndicies;
        }
    }
}




//
//
//
//
//
//
//
//
