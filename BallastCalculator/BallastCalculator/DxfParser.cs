using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Globalization;
using System.Threading;


namespace BallastCalculator
{
    
        class DxfParser
        {
            private int BlocksStart;
            private int BlocksEnd;

            private int EntitiesStart;
            private int EntitiesEnd;

            private void SetSectionIndices(int b_start, int b_end, int e_start, int e_end)
            {
                BlocksStart = b_start;
                BlocksEnd = b_end;

                EntitiesStart = e_start;
                EntitiesEnd = e_end;
            }

            public IFIPerimeter IFIBoarder = new IFIPerimeter();

            public List<Panel> PanelList = new List<Panel>();

            //perims Section (Template) -> Solar Pannel Dimensions -> LxW 
            // - points of each solar panel 
            // - 4 corners 
            // perm 
            // 3 
            // permNAME 
            //7602 
            //10500,10510
            //10568
            //sw315, AcDbPolyline
            // Find all lines containing 10deg or 5deg 
            // From those, show all unique titles 
            // Let the user choose relavent solar panel 
            // if 10deg or 5deg does not return anything 
            // prompt user to input name 
            // if does not return an error 
            // 
            //IFI NUMBER 
            // - 4 coordinates 

            //Entities Section 
            // - XY solar panel location (top left + top left corner ) 
            // - Entity Number 
            // 

            // Min X and Max Y of IFI 
            // Min X and Min Y 

            // X1 should be greater than X2 
            // Y1 should be greater than Y2 
            // X1 Y1 North west corner 
            // X2 Y2 Largest X, Y 

            // Center point of each module 
            // Entity point + calculated center point 
            // Use calculated center point and NE_Zone pattern to place each module in correct NE_Zone 

            // Excel calculation 

            private string[] lines = File.ReadAllLines(@"C:\Users\Owner\Desktop\EcoLibriumSolar\Greenskies_Griswold Elem EF3_Ecolibrium Layout (WITH IFI PERIMETER) stripped down.dxf");


            private string FileName;

            public DxfParser(string filename)
            {
                FileName = filename;
            }
            // Blocks Section: 
            // Contains information on Dimensions of Each Panel
            public BasicDimensions ParseBlocks()
            {
                BasicDimensions BlockPerimeter = new BasicDimensions();

                var start = BlocksStart;

                var end = BlocksEnd;

                string[] perims_section = new string[end - start];

                Array.Copy(lines, start, perims_section, 0, end - start);

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

                            BlockPerimeter.Corner1 = new Tuple<double, double>(temp_list[0], temp_list[1]);
                            BlockPerimeter.Corner2 = new Tuple<double, double>(temp_list[2], temp_list[3]);
                            BlockPerimeter.Corner3 = new Tuple<double, double>(temp_list[4], temp_list[5]);
                            BlockPerimeter.Corner4 = new Tuple<double, double>(temp_list[6], temp_list[7]);

                            break;

                        }

                    }
                    index += 1;
                }


                return BlockPerimeter;
            }
            //Entities Section
            // Contains information on the IFI Perimeter and the location of each Panel
            public void ParseEntities()
            {
                var start = EntitiesStart;
                var end = EntitiesEnd;

                string[] entites_section = new string[end - start];
                Array.Copy(lines, start, entites_section, 0, end - start);
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
                        Panel temp_base = new Panel();
                        temp_base.Xvalue = temp_list[0];
                        temp_base.Yvalue = temp_list[1];
                        temp_base.PanelID = panel_count;
                        PanelList.Add(temp_base);
                        panel_count = panel_count + 1;


                    }

                    if (x.Contains("IFI"))
                    {
                        List<Double> temp_list = new List<Double>();
                        for (int i = 0; i < 16; i += 2)
                        {
                            temp_list.Add(Convert.ToDouble(entites_section[index + 10 + i].Trim()));

                        }
                        IFIBoarder.Corner1 = new Tuple<double, double>(temp_list[0], temp_list[1]);
                        IFIBoarder.Corner2 = new Tuple<double, double>(temp_list[2], temp_list[3]);
                        IFIBoarder.Corner3 = new Tuple<double, double>(temp_list[4], temp_list[5]);
                        IFIBoarder.Corner4 = new Tuple<double, double>(temp_list[6], temp_list[7]);

                    }
                    index += 1;

                }



            }

            public void ParseFile()
            {

                int index = 0;
                int perm_start = 0;
                int perm_end = 0;
                int entity_start = 0;
                int entity_end = 0;
                foreach (string x in lines)
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
                SetSectionIndices(perm_start, perm_end, entity_start, entity_end);


            }

        
    }
   
}
    
