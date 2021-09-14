using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualizeSensors
{
    public class SensorConfig
    {
        public Dictionary<string, List<List<int>>> regions { get; set; }

        public RefScreen ref_screen;

    }

    public class RefScreen
    {
        public int width { get; set; }
        public int height { get; set; }
    }

    public enum SensorRegions
    {
        C_REGION,
        A1_REGION,
        A2_REGION,
        A3_REGION,
        A4_REGION,
        A5_REGION,
        A6_REGION,
        A7_REGION,
        A8_REGION,
        B1_REGION,
        B2_REGION,
        B3_REGION,
        B4_REGION,
        B5_REGION,
        B6_REGION,
        B7_REGION,
        B8_REGION
    }

}
