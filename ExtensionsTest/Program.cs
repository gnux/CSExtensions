using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using gnux.Extensions.extEnums;
using System.ComponentModel;

namespace gnux.ExtensionsTest
{
    
    enum TEST
    {
        none=0,
        [Description("first Value")]
        first=1,
        [Description("second Value")]
        second=2,
        [Description("third Value")]
        third=4,
        [Description("fourth Value")]
        fourth=8,
        [Description("fifth Value")]
        fifth=16,
        [Description("sixth Value")]
        sixth=32
    }


    class Program
    {
        static void Main(string[] args)
        {
            TEST testen= TEST.fifth | TEST.first | TEST.third;
            testen = (TEST) testen.FromDescriptionString("sixth Value", false);
            Console.WriteLine(testen.ToDescriptionString() + "\n");
            testen= TEST.fifth | TEST.second ;//| TEST.third;
            testen = TEST.third|TEST.fifth | TEST.second;// | TEST.none;
            Console.WriteLine(testen.ToString()+ "\n");
            foreach (string str in testen.GetSelectedValuesAsDescriptions()) 
              Console.WriteLine(str);
            Console.ReadKey();
        }
    }
}
