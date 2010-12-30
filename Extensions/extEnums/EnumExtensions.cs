using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

namespace gnux.Extensions.extEnums
{
    /// <summary>
    /// Extend standard Enums, with some hopefully usefull functionality
    /// </summary>
    public static class EnumExtensions
    {
        /// <summary>
        /// Get the Description attribute, if any, for this Enum
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static string ToDescriptionString(this Enum val)
        {
            DescriptionAttribute[] attributes = (DescriptionAttribute[])val.GetType().GetField(val.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), false);
            return attributes.Length > 0 ? attributes[0].Description : string.Empty;
        }

        /// <summary>
        /// Get all Descriptions defiened in this Enum type as string[]
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static string[] GetDescriptionStrings(this Enum val)
        {
            List<string> list = val.GetDescriptionStringsList() as List<string>;
            string[] attributes = new string[list.Count];
            list.CopyTo(attributes);
            return attributes;
        }

        /// <summary>
        /// Get all Descriptions defiened in this Enum type as list
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static IList<string> GetDescriptionStringsList(this Enum val)
        {
            IList<string> list = new List<string>();
            foreach (FieldInfo field in val.GetType().GetFields())
            {
                DescriptionAttribute[] attributes = field.GetCustomAttributes(typeof(DescriptionAttribute), false) as DescriptionAttribute[];
                if (attributes.Length > 0)
                    list.Add(attributes[0].Description);
            }
            return list;
        }

        /// <summary>
        /// Get Enum Value from a given Description
        /// </summary>
        /// <param name="val"></param>
        /// <param name="description">The description</param>
        /// <param name="ignoreCase">Ignore case while conversion</param>
        /// <returns></returns>
        public static Enum FromDescriptionString(this Enum val, string description, bool ignoreCase)
        {
            foreach (FieldInfo field in val.GetType().GetFields())
            {
                DescriptionAttribute[] attributes = field.GetCustomAttributes(typeof(DescriptionAttribute), false) as DescriptionAttribute[];
                if (attributes.Length > 0)
                    if (String.Compare(attributes[0].Description, description, ignoreCase) == 0)
                        return field.GetValue(val) as Enum;
            }
            return null;
        }

        ///<summary>
        /// Get selected values in current Enumeration as their decscription string
        /// </summary>
        /// <remarks>Enum must look like follows
        /// /// <example>enum TEST
        ///{
        /// none=0,
        /// [Description("first Value")]
        /// first=1,
        /// [Description("second Value")]
        /// second=2,
        /// [Description("third Value")]
        /// third=4,
        /// [Description("fourth Value")]
        /// fourth=8,
        /// [Description("fifth Value")]
        /// fifth=16,
        /// [Description("sixth Value")]
        /// sixth=32
        /// }</example>
        /// </remarks>
        /// <param name="val"></param>
        /// <returns></returns>
        public static string[] GetSelectedValuesAsDescriptions(this Enum val)
        {
            string[] str = new string[0];
            if ((int)(object)val != 0)
            {
                foreach (string desc in val.GetDescriptionStrings())
                    if ((int)(object)val.FromDescriptionString(desc, true) != 0)
                        if (((int)(object)val & (int)(object)val.FromDescriptionString(desc, true)) == (int)(object)val.FromDescriptionString(desc, true))
                        {
                            Array.Resize<string>(ref str, str.Length + 1);
                            str[str.Length - 1] = desc;
                        }
            }
            return str;
        }
    }
}
