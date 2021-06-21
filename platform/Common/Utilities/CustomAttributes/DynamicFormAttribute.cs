using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Common.Utilities.CustomAttributes
{
    public class DynamicFormAttribute : Attribute
    {

        /// <summary>
        ///The form element label to be displayed 
        /// </summary>
        public string Label { get; private set; }

        /// <summary>
        ///The input type that should be created for the field. Possible values include 'textbox', 'textarea', 'date', 'checkbox', 'dropdown', 'radio' 
        /// </summary>
        public string Value { get; private set; }

        /// <summary>
        ///The formControl for this property will be named according to Key, the resulting form submission will also contain this key as property name
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        ///The input type that should be created for the field. Possible values include 'textbox', 'textarea', 'date', 'checkbox', 'dropdown', 'radio'
        /// </summary>
        public string ControlType { get; set; }

        /// <summary>
        ///Whether or not the input for this field should be required 
        /// </summary>
        public bool Required { get; set; }

        /// <summary>
        ///Whether or not the input for this field should be visible on the form 
        /// </summary>
        public bool Visible { get; set; }

        /// <summary>
        /// Whether the input for this field should be disabled, but displayed
        /// </summary>
        public bool Disabled { get; set; }

        /// <summary>
        ///Defines a javascript regex pattern to be used to validate the input for this field
        /// </summary>
        public string Pattern { get; set; }

        /// <summary>
        ///Defines the error message to display on pattern validation failure
        /// </summary>
        public string PatternValidationMessage { get; set; }

        /// <summary>
        ///Specifies the minLength validation for textbox and textarea control types
        /// </summary>
        public int MinLength { get; set; }

        /// <summary>
        ///Specifies the maxLength validation for textbox and textarea control types
        /// </summary>
        public int MaxLength { get; set; }

        /// <summary>
        ///Specifies the type attribute of the input to be created for a textbox control type. Values include 'password', 'email', 'tel', 'number'
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Specifies options for dropdown or radio control types, this holds the keys for the options
        /// </summary>
        public string[] OptionsKeys { get; set; }

        /// <summary>
        /// Specifies options for dropdown or radio control types, this holds the values for the options
        /// </summary>
        public string[] OptionsValues { get; set; }

        /// <summary>
        /// If no options are specified for dropdown or radio control types, url postfix (eg. /api/admin/getoptions can be specified here from which the options should be loaded</param>
        /// </summary>
        public string OptionsFetchURLPostfix { get; set; }


        /// <summary>
        /// Defines the custom attribute for building dynamic forms 
        /// </summary>
        /// <param name="label">The form element label to be displayed</param>
        /// <param name="controlType">The input type that should be created for the field. Possible values include 'textbox', 'textarea', 'date', 'checkbox', 'dropdown', 'radio'</param>
        /// <param name="required">Whether or not the input for this field should be required</param>
        /// <param name="disabled">Whether the input for this field should be disabled, but displayed</param>
        /// <param name="visible">Whether the input for this field should be visible or not</param>
        /// <param name="pattern">Defines a javascript regex pattern to be used to validate the input for this field</param>
        /// <param name="patternValidationMessage">Defines the error message to display on pattern validation failure</param>
        /// <param name="type">Specifies the type attribute of the input to be created for a textbox control type. Values include 'password', 'email', 'tel', 'number'</param>
        /// <param name="optionsKeys">Specifies options for dropdown or radio control types, this holds the keys for the options</param>
        /// <param name="optionsValues">Specifies options for dropdown or radio control types, this holds the values for the options</param>
        /// <param name="optionsFetchURLPostfix"> If no options are specified for dropdown or radio control types, url postfix (eg. /api/admin/getoptions can be specified here from which the options should be loaded</param>
        /// <param name="minLength">Specifies the minLength validation for textbox and textarea control types</param>
        /// <param name="maxLength">Specifies the maxLength validation for textbox and textarea control types</param>
        public DynamicFormAttribute(
            string label,
            string controlType,
            bool required = false,
            bool disabled = false,
            bool visible = true,
            string pattern = null,
            string patternValidationMessage = null,
            string type = "text",
            string[] optionsKeys = null,
            string[] optionsValues = null,
            string optionsFetchURLPostfix = null,
            int minLength = 0,
            int maxLength = 1000)
        {
            this.Label = label;
            this.ControlType = controlType;
            this.Required = required;
            this.Disabled = disabled;
            this.Visible = visible;
            this.Pattern = pattern;
            this.Type = type;
            this.PatternValidationMessage = patternValidationMessage;
            this.MinLength = minLength;
            this.MaxLength = maxLength;
            this.OptionsKeys = optionsKeys;
            this.OptionsValues = optionsValues;
            this.OptionsFetchURLPostfix = optionsFetchURLPostfix;
        }
    }

    public static class DynamicFormAttributeHelper
    {



        public static DTODynamicFormsStructure BuildFromType(Type type)
        {

            var props = type.GetProperties(BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance);

            var res = new DTODynamicFormsStructure();

            props.ToList().ForEach(p =>
            {

                var attrs = p.GetCustomAttributes(false).ToList();

                if (attrs.Count > 0)
                {
                    attrs.ForEach(f =>
                    {

                        if (f is DynamicFormAttribute)
                        {
                            var el = new DTODynamicFormElement();

                            var dt = ((DynamicFormAttribute)f);

                            el.key = p.Name;
                            el.label = dt.Label;
                            el.required = dt.Required;
                            el.disabled = dt.Disabled;
                            el.visible = dt.Visible;
                            el.pattern = dt.Pattern;
                            el.patternValidationMsg = dt.PatternValidationMessage;
                            el.minLength = dt.MinLength;
                            el.maxLength = dt.MaxLength;
                            el.controlType = dt.ControlType;
                            el.type = dt.Type;

                            if (dt.OptionsKeys != null && dt.OptionsValues != null)
                            {
                                el.options = new DynamicFormElementOptions[dt.OptionsKeys.Length];
                                for (int i = 0; i < dt.OptionsKeys.Length; i++)
                                    el.options[i] = new DynamicFormElementOptions(dt.OptionsKeys.ElementAt(i), dt.OptionsValues.ElementAt(i));
                            }
                            else
                            {
                                el.optionsFetchAddress = dt.OptionsFetchURLPostfix;
                            }
                            res.elements.Add(el);
                        }

                    });


                }

            });


            return res;


        }

    }

    public class DTODynamicFormsStructure
    {
        public List<DTODynamicFormElement> elements { get; set; }

        public DTODynamicFormsStructure()
        {
            elements = new List<DTODynamicFormElement>();
        }
    }

    public class DTODynamicFormElement
    {
        public string label { get; set; }
        public string key { get; set; }
        public string controlType { get; set; }
        public string value { get; set; }
        public bool required { get; set; }
        public string pattern { get; set; }
        public string patternValidationMsg { get; set; }
        public int minLength { get; set; }
        public int maxLength { get; set; }
        public bool disabled { get; set; }
        public bool visible { get; set; }
        public string type { get; set; }
        public DynamicFormElementOptions[] options { get; set; }
        public string optionsFetchAddress { get; set; }

    }

    public class DynamicFormElementOptions
    {
        public string key { get; set; }
        public string value { get; set; }

        public DynamicFormElementOptions(string key, string value)
        {
            this.key = key;
            this.value = value;
        }
    }
}


