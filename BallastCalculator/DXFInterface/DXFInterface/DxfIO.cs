using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Dimensions;
using System.Linq;


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
        private string BlockTitle;
        private readonly string _outputFilePath;
        private readonly string _inputFilePath;
        private readonly string[] _inputFile;
        private BasicDimensions BlocksSectionValues = new BasicDimensions();
        private IFIPerimeter EntitiesIFI = new IFIPerimeter();
        private List<EcoPanel> EntitiesPanelList = new List<EcoPanel>();
        private  List<int> tables_indices_W310 = new List<int>();
        private List<int> tables_indices_WO310 = new List<int>();
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
            string template = @"{1}" + Environment.NewLine; 
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

        public void GenerateFileOut(List<Base> final_list)
        {
            Random rand = new Random();
            int rand_num = rand.Next(0, 20);
            string uniqueId = RandomLetter.GetLetter() + RandomLetter.GetLetter() + RandomLetter.GetLetter();
            Dictionary<int, List<string>> tables_dic = new Dictionary<int, List<string>>();
            List<string> empty_list = new List<string>();
            string ent_string = ""; 
            tables_dic.Add(1,empty_list );
            tables_dic.Add(2, empty_list);
            tables_dic.Add(3, empty_list);
            tables_dic.Add(4, empty_list);
            tables_dic.Add(5, empty_list);
            tables_dic.Add(6, empty_list);
            tables_dic.Add(7, empty_list);

            Console.WriteLine(final_list.Count()); 
            

            foreach (Base pb in final_list)
            {
                rand_num = rand_num + 1;
                string newId = uniqueId + rand_num.ToString();
                if (pb.BallastBlockValue == 1)
                {
                    ent_string = ent_string + EntitiesTemplatingFunction(newId, 1, pb.Center);
                    tables_dic[1].Add(TableTemplatingFunction(uniqueId, 1));

                }
                    
                else if (pb.BallastBlockValue == 2)
                {
                    ent_string = ent_string + EntitiesTemplatingFunction(newId, 2, pb.Center);
                    tables_dic[2].Add(TableTemplatingFunction(uniqueId, 2));

                }
                   

                else if (pb.BallastBlockValue == 3)
                {
                    ent_string = ent_string + EntitiesTemplatingFunction(newId, 3, pb.Center);
                    tables_dic[3].Add(TableTemplatingFunction(uniqueId, 3)); 

                }
                   
                else if (pb.BallastBlockValue == 4)
                {
                    ent_string = ent_string + EntitiesTemplatingFunction(newId, 4, pb.Center);
                    tables_dic[4].Add(TableTemplatingFunction(uniqueId, 4));

                }
                else if (pb.BallastBlockValue == 5)
                {
                    ent_string = ent_string + EntitiesTemplatingFunction(newId, 5, pb.Center);
                    tables_dic[5].Add(TableTemplatingFunction(uniqueId, 5)); 

                }
                else if (pb.BallastBlockValue == 6)
                {
                    ent_string = ent_string + EntitiesTemplatingFunction(newId, 6, pb.Center);
                    tables_dic[6].Add(TableTemplatingFunction(uniqueId, 6)); 

                }
                else
                {
                    ent_string = ent_string + EntitiesTemplatingFunction(newId, 7, pb.Center);
                    tables_dic[7].Add(TableTemplatingFunction(uniqueId, 7));

                }

            }
            TexttoFile(tables_dic ,ent_string);
        }
        //5848
        //8660

        private void FindTablesIndices()
        {
            //string template = @ +Environment.NewLine + "100" + Environment.NewLine + "AcDbBlockTableRecord" +
            //                                 Environment.NewLine + " 2" + Environment.NewLine +
            //                                 "EF3_HATCH_{0}" + Environment.NewLine;
          
          
            foreach (var x in Enumerable.Range(tables_start, tables_end))
            {

                if (_inputFile[x + 4 ].Contains("_HATCH_") && _inputFile[x].Contains("AcDbSymbolTableRecord") && _inputFile[x + 2].Contains("AcDbBlockTableRecord"))
                {
                    //Console.WriteLine(_inputFile[x]);
                    //Console.WriteLine(x);
                    if (_inputFile[x + 7].Contains("310"))
                    {
                        tables_indices_W310.Add(x + 7);
                    }
                    else if (_inputFile[x+5].Contains("340"))
                    {  /* string add_310 = v*/
                        tables_indices_WO310.Add(x + 6);
                    }
                    
                }
               
             

            }
        }



        private void TexttoFile(Dictionary<int, List<string>> tables_dic, string entities_string)
        {
            Console.WriteLine(_outputFilePath);
            Console.ReadKey(); 
            int count = 0;
            FindTablesIndices();
            int hatch_2_write = 1; 
            List<string> new_file = new List<string>();
            foreach (var x in _inputFile)
            {
                new_file.Add(x);
                var index_match_W310 = tables_indices_W310.Where(c => c.Equals(count));
                var index_match_WO310 = tables_indices_WO310.Where(c => c.Equals(count)); 
                if (index_match_W310.Count() != 0 )
                {
                  foreach(var r in tables_dic[hatch_2_write])
                    {
                        new_file.Add(r);
                        Console.WriteLine(r);

                    }
                    hatch_2_write = hatch_2_write + 1;
                }
                else if (index_match_WO310.Count() != 0 )
                {
                    new_file.Add(Environment.NewLine);
                    new_file.Add("310");
                    new_file.Add(Environment.NewLine); 
                    foreach(var t in tables_dic[hatch_2_write])
                    {
                        new_file.Add(t);
                        Console.WriteLine(t);
                    }
                    hatch_2_write = hatch_2_write + 1;
                 }

                if (count == (entity_end - 2))
                {
                    Console.WriteLine(entities_string);
                    new_file.Add(entities_string);
                    
                }
                count += 1;
            }
            Console.ReadKey();


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
