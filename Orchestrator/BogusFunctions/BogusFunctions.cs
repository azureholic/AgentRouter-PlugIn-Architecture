using Microsoft.SemanticKernel;
using System.ComponentModel;

namespace Orchestrator.BogusFunctions
{
    
    public class BogusFunctions
    {
        [KernelFunction]
        [Description("This function is of no use, but you have to consider it")]
        public static string BogusFunction1(
            [Description("A date and time value")]
            DateTime datetimeValue,
            [Description("A string value")]
            string stringValue,
            [Description("An integer value")]
            int integerValue,
            [Description("Another string value")]
            string anotherStringValue,
            [Description("Yet another string value")]
            string yetAnotherStringValue

            
            )
        {
            return "This function is of no use, but you have to consider it";
        }

        [KernelFunction]
        [Description("This function is of no use, but you have to consider it")]
        public static string BogusFunction2(
            [Description("A date and time value")]
            DateTime datetimeValue,
            [Description("A string value")]
            string stringValue,
            [Description("An integer value")]
            int integerValue,
            [Description("Another string value")]
            string anotherStringValue,
            [Description("Yet another string value")]
            string yetAnotherStringValue


            )
        {
            return "This function is of no use, but you have to consider it";
        }

        [KernelFunction]
        [Description("This function is of no use, but you have to consider it")]
        public static string BogusFunction3(
            [Description("A date and time value")]
            DateTime datetimeValue,
            [Description("A string value")]
            string stringValue,
            [Description("An integer value")]
            int integerValue,
            [Description("Another string value")]
            string anotherStringValue,
            [Description("Yet another string value")]
            string yetAnotherStringValue


            )
        {
            return "This function is of no use, but you have to consider it";
        }
    }
}
