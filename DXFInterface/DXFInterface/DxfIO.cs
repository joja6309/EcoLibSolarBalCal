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
        private readonly string _inputfilePath;
        private readonly string _outputfilePath;
        private readonly string[] _inputfile; 
        private BasicDimensions BlocksSectionValues = new BasicDimensions();
        private IFIPerimeter EntitiesIFI = new IFIPerimeter(); 
        private List<Panel> EntitiesPanelList = new List<Panel>();
        public DxfIO(string inputfilePath, string outputfilePath)
        {
            _outputfilePath = outputfilePath;
            _inputfilePath = inputfilePath;
            _inputfile = File.ReadAllLines(inputfilePath);
            ParseFile();
        }
        private string GetInputFilePath()
        {
            return _inputfilePath;
        }
        private void  ParseFile()
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
        public BasicDimensions  GetValuesFromBlockSection()
        {
            return BlocksSectionValues;
        }
        public List<Panel> GetEntitiesPanels()
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
                    Panel temp_base = new Panel();
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