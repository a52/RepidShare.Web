using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.Collections;
using System.Globalization;
using System.Web.Mvc;
using System.Linq.Expressions;
using RepidShare.Entities;
using System.Web.Routing;
using System.Data;
using System.Xml.Linq;
using System.IO;

namespace RepidShare.Utility
{
    public static class HTMLHelper
    {
        #region TextBox
        /// <summary>
        /// TextBox
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="htmlHelper"></param>
        /// <param name="expression"></param>
        /// <param name="objQuestionAnswerModel"></param>
        /// <returns></returns>
        public static MvcHtmlString SingleLineTextBoxFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, ViewQuestionAnswerModel objQuestionAnswerModel)
        {
            MvcHtmlString html = default(MvcHtmlString);
            string strHtml = "";
            if (objQuestionAnswerModel != null)
            {
                html = System.Web.Mvc.Html.InputExtensions.TextBoxFor(htmlHelper, expression, GetAttributes(objQuestionAnswerModel));
                strHtml = html.ToHtmlString();
                string htmlFieldName = ExpressionHelper.GetExpressionText(expression);
                var properties = typeof(SingleLineModel).GetProperties();
                strHtml += GetStringFromModel(objQuestionAnswerModel.QuestionTypeDetail.SingleLineTextType, properties, htmlFieldName);
            }
            else
            {
                html = System.Web.Mvc.Html.InputExtensions.TextBoxFor(htmlHelper, expression, new { @class = "form-control-input" });
                strHtml = html.ToHtmlString();
            }
            return new MvcHtmlString(strHtml);
        }

        #endregion

        #region TextArea

        /// <summary>
        /// TextArea
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="htmlHelper"></param>
        /// <param name="expression"></param>
        /// <param name="objTextAreaModel"></param>
        /// <returns></returns>
        public static MvcHtmlString MultiLineTextBoxFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, ViewQuestionAnswerModel objQuestionAnswerModel)
        {
            MvcHtmlString html = default(MvcHtmlString);
            string strHtml = "";
            if (objQuestionAnswerModel != null)
            {
                html = System.Web.Mvc.Html.TextAreaExtensions.TextAreaFor(htmlHelper, expression, GetAttributes(objQuestionAnswerModel));
                strHtml = html.ToHtmlString();
                string htmlFieldName = ExpressionHelper.GetExpressionText(expression);
                var properties = typeof(MultiLineModel).GetProperties();
                strHtml += GetStringFromModel(objQuestionAnswerModel.QuestionTypeDetail.MultiLineTextType, properties, htmlFieldName);
            }
            else
            {
                html = System.Web.Mvc.Html.InputExtensions.TextBoxFor(htmlHelper, expression);
                strHtml = html.ToHtmlString();
            }
            return new MvcHtmlString(strHtml);
        }



        #endregion

        #region Number
        /// <summary>
        /// TextBox
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="htmlHelper"></param>
        /// <param name="expression"></param>
        /// <param name="objQuestionAnswerModel"></param>
        /// <returns></returns>
        public static MvcHtmlString NumberTextBoxFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, ViewQuestionAnswerModel objQuestionAnswerModel)
        {
            MvcHtmlString html = default(MvcHtmlString);
            string strHtml = "";
            if (objQuestionAnswerModel != null)
            {
                html = System.Web.Mvc.Html.InputExtensions.TextBoxFor(htmlHelper, expression, GetAttributes(objQuestionAnswerModel));
                strHtml = html.ToHtmlString();
                string htmlFieldName = ExpressionHelper.GetExpressionText(expression);
                var properties = typeof(NumberModel).GetProperties();
                strHtml += GetStringFromModel(objQuestionAnswerModel.QuestionTypeDetail.NumberType, properties, htmlFieldName);
            }
            else
            {
                html = System.Web.Mvc.Html.InputExtensions.TextBoxFor(htmlHelper, expression);
                strHtml = html.ToHtmlString();
            }
            return new MvcHtmlString(strHtml);
        }

        #endregion

        #region DateTime
        /// <summary>
        /// TextBox
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="htmlHelper"></param>
        /// <param name="expression"></param>
        /// <param name="objQuestionAnswerModel"></param>
        /// <returns></returns>
        public static MvcHtmlString DateTimeTextBoxFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, ViewQuestionAnswerModel objQuestionAnswerModel)
        {
            MvcHtmlString html = default(MvcHtmlString);
            string strHtml = "";
            if (objQuestionAnswerModel != null)
            {
                var attrDate = GetAttributes(objQuestionAnswerModel);
                string htmlFieldName = ExpressionHelper.GetExpressionText(expression);
                object model = htmlHelper.ViewData.Model;

                var index = ((DocumentResponseDetailModel)model).Index;


                /*---------------*/
                string DefaultDateTimeValue = null;
                //check value type is date or datet time
                if (objQuestionAnswerModel.QuestionTypeDetail.DateAndTimeType.IsDateOnly == true)
                {
                    //check date is none,today or specific date
                    if (objQuestionAnswerModel.QuestionTypeDetail.DateAndTimeType.DefaultValueType == CommonUtils.DateDefaultValue.TodaysDate.ToString())
                    {
                        DefaultDateTimeValue = Convert.ToDateTime(System.DateTime.Now).ToString(CommonUtils.ShortDateFormat);
                    }
                    else if (objQuestionAnswerModel.QuestionTypeDetail.DateAndTimeType.DefaultValueType == CommonUtils.DateDefaultValue.SpecificDate.ToString())
                    {
                        DefaultDateTimeValue = Convert.ToDateTime(objQuestionAnswerModel.QuestionTypeDetail.DateAndTimeType.DateDefaultValue).ToString(CommonUtils.ShortDateFormat);
                    }
                }
                else if (objQuestionAnswerModel.QuestionTypeDetail.DateAndTimeType.IsDateOnly == false)
                {
                    //check date is none,today or specific date
                    if (objQuestionAnswerModel.QuestionTypeDetail.DateAndTimeType.DefaultValueType == CommonUtils.DateDefaultValue.TodaysDate.ToString())
                    {
                        DefaultDateTimeValue = Convert.ToDateTime(System.DateTime.Now).ToString(CommonUtils.LongDateFormat);
                    }
                    else if (objQuestionAnswerModel.QuestionTypeDetail.DateAndTimeType.DefaultValueType == CommonUtils.DateDefaultValue.SpecificDate.ToString())
                    {
                        DefaultDateTimeValue = Convert.ToDateTime(objQuestionAnswerModel.QuestionTypeDetail.DateAndTimeType.DateDefaultValue).ToString(CommonUtils.LongDateFormat);
                    }
                }

                attrDate.Add("Value", string.IsNullOrWhiteSpace(((DocumentResponseDetailModel)model).Questions[index].AnswerDetail) ? DefaultDateTimeValue : ((DocumentResponseDetailModel)model).Questions[index].AnswerDetail);
                //attrDate.Add("Value", (
                //    objQuestionAnswerModel.QuestionTypeDetail.DateAndTimeType.DefaultValueType == CommonUtils.DateDefaultValue.SpecificDate.ToString() ?
                //    objQuestionAnswerModel.QuestionTypeDetail.DateAndTimeType.IsDateOnly == true ?
                //    Convert.ToDateTime(objQuestionAnswerModel.QuestionTypeDetail.DateAndTimeType.DateDefaultValue).ToString(CommonUtils.ShortDateFormat)
                //    : Convert.ToDateTime(objQuestionAnswerModel.QuestionTypeDetail.DateAndTimeType.DateDefaultValue).ToString(CommonUtils.LongDateFormat) 
                //    : ""));
                attrDate.Add("Id", "DatetimePicker_" + index);
                //// get the metdata
                //ModelMetadata fieldmetadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);

                ////// get the field name
                //var fieldName = ExpressionHelper.GetExpressionText(expression);

                ////// get full field (template may have prefix)
                //var fullName = htmlHelper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(fieldName);

                html = System.Web.Mvc.Html.InputExtensions.TextBoxFor(htmlHelper, expression, attrDate);

                strHtml = html.ToHtmlString();

                var properties = typeof(DateAndTimeModel).GetProperties();
                strHtml += GetStringFromModel(objQuestionAnswerModel.QuestionTypeDetail.DateAndTimeType, properties, htmlFieldName);
            }
            else
            {
                html = System.Web.Mvc.Html.InputExtensions.TextBoxFor(htmlHelper, expression);
                strHtml = html.ToHtmlString();
            }
            return new MvcHtmlString(strHtml);
        }

        #endregion


        #region Radio
        public static MvcHtmlString AnswerRadioButtonFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, ViewQuestionAnswerModel objQuestionAnswerModel, QuestionOptionsModel objQuestionOption, int optionIndex)
        {
            MvcHtmlString html = default(MvcHtmlString);
            string strHtml = "";
            string htmlFieldName = ExpressionHelper.GetExpressionText(expression);

            var properties = typeof(SingleSelectModel).GetProperties();
            if (objQuestionAnswerModel != null && objQuestionAnswerModel.QuestionOptionsList != null)
            {

                object model = htmlHelper.ViewData.Model;
                DocumentResponseDetailModel objModel = (DocumentResponseDetailModel)model;
                //for (int i = 0; i < objQuestionTypeModel.lstSingleSelection.Count(); i++)
                //{
                var Attributes = GetAttributes(objQuestionAnswerModel);

                Attributes["class"] += "radioanswer";
                Attributes.Add("id", "rdb_" + objModel.Questions[objModel.Index].QuestionOptionsList[optionIndex].QuestionOptionsID + "_" + objModel.Questions[objModel.Index].QuestionID);
                Attributes.Add("Value", objModel.Questions[objModel.Index].QuestionOptionsList[optionIndex].QuestionOptionsID);

                html = System.Web.Mvc.Html.InputExtensions.RadioButton(htmlHelper, "questionAnswer_" + objModel.Questions[objModel.Index].QuestionID,
                  objModel.Questions[objModel.Index].QuestionOptionsList[optionIndex].QuestionOptionsID, Attributes);
                //html = System.Web.Mvc.Html.InputExtensions.RadioButton(htmlHelper, "questionAnswer_" + objModel.Questions[objModel.Index].QuestionID,
                //    objModel.Questions[objModel.Index].QuestionOptionsList[optionIndex].QuestionOptionsID,
                //    new
                //    {
                //        @class = "radioanswer",
                //        id = "rdb_" + objModel.Questions[objModel.Index].QuestionOptionsList[optionIndex].QuestionOptionsID + "_" + objModel.Questions[objModel.Index].QuestionID,
                //        Value = objModel.Questions[objModel.Index].QuestionOptionsList[optionIndex].QuestionOptionsID
                //    });
                //html = System.Web.Mvc.Html.InputExtensions.RadioButton(htmlHelper, "rbSingle", objQuestionOption.OptionText, false, Attributes);
                strHtml += html.ToHtmlString();
                html = System.Web.Mvc.Html.InputExtensions.HiddenFor(htmlHelper, expression);
                strHtml += html.ToHtmlString();
                strHtml += "<label>" + objQuestionOption.OptionText + "</label>";

                strHtml += GetStringFromModel(objQuestionAnswerModel.QuestionTypeDetail.SingleSelect, properties, htmlFieldName);
                // }
            }
            else
            {
                html = System.Web.Mvc.Html.InputExtensions.RadioButtonFor(htmlHelper, expression, "");
                strHtml = html.ToHtmlString();
            }
            return new MvcHtmlString(strHtml);
        }

        #endregion

        #region DropDownn
        public static MvcHtmlString DropDownFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, ViewQuestionAnswerModel objQuestionAnswerModel)
        {
            MvcHtmlString html = default(MvcHtmlString);
            string strHtml = "";
            List<SelectListItem> objSelectListItem = new List<SelectListItem>();

            StringReader theReader = new StringReader("<XmlDS>" + objQuestionAnswerModel.DropDownXML + "</XmlDS>");
            DataSet theDataSet = new DataSet();
            theDataSet.ReadXml(theReader);

            if (theDataSet != null && theDataSet.Tables.Count > 0)
            {
                foreach (DataRow item in theDataSet.Tables[0].Rows)
                {
                    objSelectListItem.Add(new SelectListItem
                    {
                        Text = item["DropDownText"].ToString(),
                        Value = item["QuestionDropDownID"].ToString(),
                        Selected = item["QuestionDropDownID"].ToString() == objQuestionAnswerModel.AnswerDetail ? true : false
                    });
                }
            }

            if (objQuestionAnswerModel != null)
            {

                html = System.Web.Mvc.Html.SelectExtensions.DropDownListFor(htmlHelper, expression, objSelectListItem);
                strHtml = html.ToHtmlString();
                string htmlFieldName = ExpressionHelper.GetExpressionText(expression);
                var properties = typeof(SingleLineModel).GetProperties();
                strHtml += GetStringFromModel(objQuestionAnswerModel.QuestionTypeDetail.DropDownType, properties, htmlFieldName);
            }
            else
            {
                html = System.Web.Mvc.Html.SelectExtensions.DropDownListFor(htmlHelper, expression, objSelectListItem,new { @class="form-control"});
                strHtml = html.ToHtmlString();
            }
            return new MvcHtmlString(strHtml);
        }

        #endregion

        #region Method
        public static Boolean CheckProp(object PropVal, ViewQuestionAnswerModel objQuestionAnswerModel)
        {

            //    QuestionTypeModel objQuestionType = new QuestionTypeModel();

            if ((PropVal != null) && ((objQuestionAnswerModel.QuestionTypeDetail.SingleLineTextType != null && objQuestionAnswerModel.QuestionTypeDetail.SingleLineTextType.GetType() == PropVal.GetType())
                || (objQuestionAnswerModel.QuestionTypeDetail.MultiLineTextType != null && objQuestionAnswerModel.QuestionTypeDetail.MultiLineTextType.GetType() == PropVal.GetType())
                || (objQuestionAnswerModel.QuestionTypeDetail.NumberType != null && objQuestionAnswerModel.QuestionTypeDetail.NumberType.GetType() == PropVal.GetType())
                || (objQuestionAnswerModel.QuestionTypeDetail.SingleSelect != null && objQuestionAnswerModel.QuestionTypeDetail.SingleSelect.GetType() == PropVal.GetType())
                || (objQuestionAnswerModel.QuestionTypeDetail.MultiSelect != null && objQuestionAnswerModel.QuestionTypeDetail.MultiSelect.GetType() == PropVal.GetType())))
            {
                return false;
            }
            return true;
        }


        private static RouteValueDictionary GetAttributes(ViewQuestionAnswerModel objQuestionAnswerModel)
        {
            var attributes = new RouteValueDictionary();
            //add form control class
            string cssClass = "form-control remove-bottom";
            //attributes["class"] = "form-control";


            if (Convert.ToBoolean(objQuestionAnswerModel.IsRequireResponse))
            {
                attributes["required"] = "required";
                //attributes["data-val-required"] = RepidShare.Entities.Resource.Response.valAnswer;
                attributes["data-val-required"] = "Answer is required.";
                
                attributes["data-val"] = "true";
            }

            if (objQuestionAnswerModel.QuestionPropertyList != null && objQuestionAnswerModel.QuestionPropertyList.Count > 0)
            {
                foreach (var queProperty in objQuestionAnswerModel.QuestionPropertyList)
                {
                    if (queProperty.PropertyText != null && queProperty.PropertyText.ToString().ToLower() == CommonUtils.QuestionProperty.MAX_CHAR.ToString().ToLower())
                    {
                        attributes["maxlength"] = queProperty.PropertyValue;
                    }

                    if (string.Compare(queProperty.PropertyText, CommonUtils.QuestionProperty.MIN_VALUE.ToString(), true) == 0)
                    {
                        attributes["data-val-length-min"] = queProperty.PropertyValue;
                        attributes["data-val"] = "true";
                        attributes["questionid"] = objQuestionAnswerModel.QuestionID;
                    }
                    if (string.Compare(queProperty.PropertyText, CommonUtils.QuestionProperty.MAX_VALUE.ToString(), true) == 0)
                    {
                        attributes["data-val-length-max"] = queProperty.PropertyValue;
                        attributes["data-val"] = "true";
                    }
                    if (string.Compare(queProperty.PropertyText, CommonUtils.QuestionProperty.NO_OF_DECIMAL.ToString(), true) == 0)
                    {

                        if (Convert.ToInt32(queProperty.PropertyValue) > 0)
                        {
                            attributes["data-val-regex"] = string.Format("Only {0} decimal place value accepted.", queProperty.PropertyValue);// "Price must can't have more than " + queProperty.PropertyValue + " decimal places";
                            attributes["data-val-regex-pattern"] = @"^(0|\d{0,30}(\.\d{0," + queProperty.PropertyValue + "})?)$";
                            cssClass += "numericDec";
                        }
                        else
                            cssClass += "numericInt";
                        attributes["id"] = "num_" + objQuestionAnswerModel.QuestionID; ;
                    }

                    if (string.Compare(queProperty.PropertyText, CommonUtils.QuestionProperty.ISDATEONLY.ToString(), true) == 0)
                    {
                        cssClass += "pickDate form-control cal_icon dateTimePicker";
                        attributes["readonly"] = "readonly";
                    }
                    if (queProperty.PropertyText != null && queProperty.PropertyText.ToString().ToLower() == CommonUtils.QuestionProperty.NO_OF_LINES.ToString().ToLower())
                    {
                        attributes["rows"] = queProperty.PropertyValue;
                    }



                }
            }
                attributes["class"] = cssClass;
                if (objQuestionAnswerModel.QuestionType == CommonUtils.QuestionType.MultiLine.ToString())
                {
                    //  attributes["onkeydown"] = "limitTextareaLine(this)";
                    attributes["class"] += "yourtextarea";
                    attributes["wrap"] = "off";
                    //  attributes["onkeyup"] = "foo(this,6)";

                }
            
            return attributes;
        }


        private static string GetStringFromModel<TModel>(TModel QuestionTypeModel, System.Reflection.PropertyInfo[] properties, string htmlFieldName)
        {

            string strHtml = string.Empty;
            string propertyName = htmlFieldName.Contains(".") ? htmlFieldName.Substring(0, htmlFieldName.LastIndexOf('.')) : htmlFieldName;

            if (QuestionTypeModel != null)
            {
                //  string propertyName = htmlFieldName.Substring(0, htmlFieldName.);
                foreach (var property in properties)
                {
                    object PropVal = property.GetValue(QuestionTypeModel, null);
                    if (PropVal != null)
                        strHtml += "<input name=\"" + propertyName + "." + property.Name + "\" id=\"" + propertyName.Replace(".", "_") + "_" + property.Name + "\" type=\"hidden\"  value=\"" + Convert.ToString(PropVal) + "\"/>";
                }
            }
            return strHtml;
        }


        #endregion
    }
}
