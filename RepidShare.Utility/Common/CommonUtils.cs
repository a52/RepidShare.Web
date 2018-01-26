using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.OleDb;
using System.Data;
using System.Text;
using System.ComponentModel;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Linq;
using System.Reflection;
using System.Configuration;
using System.IO;
using System.Globalization;
using System.Security.Cryptography;
using System;
using RepidShare.Entities;
using RepidShare.Business;
using System.Web.Mvc;


namespace RepidShare.Utility
{
    /// <summary>
    /// Common operation 
    /// </summary>
    public class CommonUtils
    {
        #region Methods

        public void UserLoginDetails(UserLogin objUserLogin)
        {
            if (objUserLogin != null)
                HttpContext.Current.Session["UserLogin"] = objUserLogin;
            else
                HttpContext.Current.Session["UserLogin"] = null;
        }

        public static UserLogin UserLogin
        {
            get
            {
                UserLogin _userLogin = null;
                if (HttpContext.Current.Session["UserLogin"] != null)
                    _userLogin = (UserLogin)HttpContext.Current.Session["UserLogin"];
                return _userLogin;
            }
        }

        /// <summary>
        /// Compare excel schema
        /// </summary>
        /// <param name="excelFilePath">File path of excel</param>
        /// <param name="excelTemplatePath">Path of excel template</param>
        /// <param name="sheetName">Name of sheet</param>
        /// <returns>string</returns>
        public string CompareExcelSchema(string excelFilePath, string excelTemplatePath, string sheetName)
        {
            StringBuilder result = new StringBuilder();
            try
            {
                OleDbConnection objOleDbConnection = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source='" + excelFilePath + "';Extended Properties= 'Excel 8.0;HDR=Yes;IMEX=1'");
                objOleDbConnection.Open();
                DataRow[] drFirstSheetColumns = objOleDbConnection.GetOleDbSchemaTable(OleDbSchemaGuid.Columns, null).Select("TABLE_NAME = '" + sheetName + "'", "ORDINAL_POSITION ASC");
                objOleDbConnection.Close();
                objOleDbConnection = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source='" + excelTemplatePath + "';Extended Properties= 'Excel 8.0;HDR=Yes;IMEX=1'");
                objOleDbConnection.Open();
                DataRow[] drTemplateFirstSheetColumns = objOleDbConnection.GetOleDbSchemaTable(OleDbSchemaGuid.Columns, null).Select("TABLE_NAME = '" + sheetName + "'", "ORDINAL_POSITION ASC");
                objOleDbConnection.Close();
                objOleDbConnection.Dispose();
                for (int index = 0; index < drTemplateFirstSheetColumns.Length; index++)
                {
                    if (!drTemplateFirstSheetColumns[index]["COLUMN_NAME"].Equals(drFirstSheetColumns[index]["COLUMN_NAME"]))
                    {
                        result.Append("Column " + drTemplateFirstSheetColumns[index]["COLUMN_NAME"] + " Not Found At Position " + drTemplateFirstSheetColumns[index]["ORDINAL_POSITION"] + ". ");
                    }
                }
            }
            catch (Exception exp)
            {
                result.Append(exp.StackTrace);
            }
            return result.ToString();
        }
        /// <summary>
        /// Convert List of object to datatable.
        /// </summary>
        /// <typeparam name="T">Generic Model</typeparam>
        /// <param name="data">List of Generic Model</param>
        /// <param name="columns">String array of column name </param>
        /// <returns>return datatable</returns>
        public DataTable ConvertToDataTable<T>(IList<T> data, string[] columns)
        {
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
            foreach (string column in columns)
            {
                foreach (PropertyDescriptor prop in properties)
                    if (column == prop.Name)
                        table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            }
            foreach (T item in data)
            {
                DataRow row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                    if (columns.Contains(prop.Name))
                        row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                table.Rows.Add(row);
            }
            return table;
        }


        /// <summary>
        /// Get XML of object model
        /// </summary>
        /// <typeparam name="T">Generic Model</typeparam>
        /// <param name="data">Object of Generic Model</param>
        /// <returns>string as XML</returns>
        public static string GetBulkXML<T>(T data)
        {
            try
            {
                XmlSerializer xs = new XmlSerializer(typeof(T));
                XDocument d = new XDocument();
                using (XmlWriter xw = d.CreateWriter())
                {
                    xs.Serialize(xw, data);
                }
                return Convert.ToString(d);
            }
            catch (Exception ex)
            {
                return HttpUtility.JavaScriptStringEncode(ex.Message);
            }
        }

        /// <summary>
        /// Get XML from list of generic model
        /// </summary>
        /// <typeparam name="T">Generic Model</typeparam>
        /// <param name="lstData">List of Generic Model</param>
        /// <returns>string as XML</returns>
        public static string GetBulkXML<T>(IList<T> lstData)
        {
            System.Text.StringBuilder strbuildXML = new System.Text.StringBuilder();
            DataTable dtDetail = ConvertToDataTable(lstData);
            DataSet dsinst = new DataSet();
            dsinst.Tables.Add(dtDetail);
            strbuildXML.Append("<Root>");
            foreach (DataRow dr in dtDetail.Rows)
            {
                strbuildXML.Append("<Table>");
                foreach (DataColumn colmname in dsinst.Tables[0].Columns)
                {
                    strbuildXML.Append("<" + colmname.ColumnName + ">");
                    strbuildXML.Append("<![CDATA[");
                    if (dr[colmname.ColumnName] != null && colmname.DataType == System.Type.GetType("System.Boolean") && Convert.ToBoolean(dr[colmname.ColumnName]) == true)
                    {
                        strbuildXML.Append("1");
                    }
                    else if (dr[colmname.ColumnName] != null && colmname.DataType == System.Type.GetType("System.Boolean") && Convert.ToBoolean(dr[colmname.ColumnName]) == false)
                    {
                        strbuildXML.Append("0");
                    }
                    else
                    {
                        if (dr[colmname.ColumnName] != null && colmname.DataType == System.Type.GetType("System.DateTime"))
                        {
                            strbuildXML.Append(Convert.ToDateTime(dr[colmname.ColumnName]).ToString("MM/dd/yyyy HH:mm:ss"));
                        }
                        else
                            strbuildXML.Append(dr[colmname.ColumnName]);
                    }
                    strbuildXML.Append("]]>");
                    strbuildXML.Append("</" + colmname.ColumnName + ">");
                }
                strbuildXML.Append("</Table>");
            }
            strbuildXML.Append("</Root>");
            return strbuildXML.ToString();
        }


        /// <summary>
        /// Convert List of Genrice Model into data table
        /// </summary>
        /// <typeparam name="T">Generic Model</typeparam>
        /// <param name="data">List of Generic Model</param>
        /// <returns>Return datatable from Generic List</returns>
        public static DataTable ConvertToDataTable<T>(IList<T> data)
        {
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
            foreach (PropertyDescriptor prop in properties)
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            foreach (T item in data)
            {
                DataRow row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                table.Rows.Add(row);
            }
            return table;
        }

        /// <summary>
        /// This method is set all properties of Complex result in to Model.
        /// </summary>
        /// <typeparam name="T">Generic Model</typeparam>
        /// <param name="objComplexType">Type of object</param>
        /// <returns>Set all properties of model and return model</returns>
        public static T GetComplexTypeToEntity<T>(Object objComplexType) where T : new()
        {
            try
            {
                var objT = new T();
                PropertyInfo[] objprop = objT.GetType().GetProperties();
                var objResultType = objComplexType.GetType();
                foreach (PropertyInfo pr in objprop)
                {
                    if (objResultType.GetProperty(pr.Name) != null)
                    {

                        var obj = objResultType.GetProperty(pr.Name).GetValue(objComplexType, null);
                        pr.SetValue(objT, obj, null);
                    }
                }
                return objT;
            }
            catch (Exception ex)
            {
                return default(T);
            }

        }

        /// <summary>
        /// Check file exist or not 
        /// </summary>
        /// <param name="DirectoryPath">Path of directory </param>
        /// <returns>Boolean</returns>
        public static Boolean CheckFileExist(string DirectoryPath)
        {
            bool isExists = System.IO.Directory.Exists(DirectoryPath);
            return isExists;
        }

        /// <summary>
        /// Create directory of given directory path
        /// </summary>
        /// <param name="DirectoryPath">Path of directory</param>
        public static void CreateDirectory(string DirectoryPath)
        {
            System.IO.Directory.CreateDirectory(DirectoryPath);
        }

        /// <summary>
        /// Delete directory of given directory path
        /// </summary>
        /// <param name="DirectoryPath">path of directory</param>
        public static void DeleteDirectory(string DirectoryPath)
        {
            try
            {
                System.IO.Directory.Delete(DirectoryPath, true);
            }
            catch { }
        }




        /// <summary>
        /// Encrypt a string using dual encryption method. Return a encrypted cipher Text
        /// </summary>
        /// <param name="toEncrypt">string to be encrypted</param>
        /// <param name="useHashing">use hashing? send to for extra secirity</param>
        /// <returns></returns>
        public static string Encrypt(string toEncrypt, bool useHashing = true)
        {
            try
            {
                byte[] keyArray;
                byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(toEncrypt);

                System.Configuration.AppSettingsReader settingsReader = new AppSettingsReader();
                // Get the key from config file
                string key = ConfigurationManager.AppSettings["secretKey"];
                //System.Windows.Forms.MessageBox.Show(key);
                if (useHashing)
                {
                    MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
                    keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
                    hashmd5.Clear();
                }
                else
                    keyArray = UTF8Encoding.UTF8.GetBytes(key);

                TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
                tdes.Key = keyArray;
                tdes.Mode = CipherMode.ECB;
                tdes.Padding = PaddingMode.PKCS7;

                ICryptoTransform cTransform = tdes.CreateEncryptor();
                byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
                tdes.Clear();
                return Convert.ToBase64String(resultArray, 0, resultArray.Length).Replace("/", ",,").Replace("+", "~");
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }
        /// <summary>
        /// DeCrypt a string using dual encryption method. Return a DeCrypted clear string
        /// </summary>
        /// <param name="cipherString">encrypted string</param>
        /// <param name="useHashing">Did you use hashing to encrypt this data? pass true is yes</param>
        /// <returns></returns>
        public static string Decrypt(string cipherString, bool useHashing = true)
        {
            try
            {
                byte[] keyArray;
                byte[] toEncryptArray = Convert.FromBase64String(cipherString.Replace(",,", "/").Replace("~", "+"));

                System.Configuration.AppSettingsReader settingsReader = new AppSettingsReader();
                //Get your key from config file to open the lock!
                string key = ConfigurationManager.AppSettings["secretKey"];

                if (useHashing)
                {
                    MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
                    keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
                    hashmd5.Clear();
                }
                else
                    keyArray = UTF8Encoding.UTF8.GetBytes(key);

                TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
                tdes.Key = keyArray;
                tdes.Mode = CipherMode.ECB;
                tdes.Padding = PaddingMode.PKCS7;

                ICryptoTransform cTransform = tdes.CreateDecryptor();
                byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

                tdes.Clear();
                return UTF8Encoding.UTF8.GetString(resultArray);
            }
            catch (Exception ex)
            {
                return string.Empty;
            }

        }

        /// <summary>
        /// Get Question Properties List  and  set in QuestionTypeDetailModel Model
        /// </summary>
        /// <param name="questionType"></param>
        /// <param name="lstQuestionProperty"></param>
        /// <param name="objQuestionTypeDetailModel"></param>
        /// <param name="lstQuestionOptions"></param>
        /// <returns></returns>
        public QuestionTypeDetailModel SetQuestionProperties(string questionType, List<QuestionPropertyModel> lstQuestionProperty, QuestionTypeDetailModel objQuestionTypeDetailModel, List<QuestionOptionsModel> lstQuestionOptions, string DropDownXML = null)
        {
            //If lstQuestionOptions is greater than 0 means question type is single select or multiselect
            if (lstQuestionOptions != null && lstQuestionOptions.Count > 0)
            {
                //if question type is single select than set QuestionOptions of singleSelect Model in QuestionTypeDetailModel Model and return QuestionTypeDetailModel
                if (questionType == CommonUtils.QuestionType.SingleSelect.ToString())
                {
                    objQuestionTypeDetailModel.SingleSelect = new SingleSelectModel();
                    objQuestionTypeDetailModel.SingleSelect.QuestionOptions = lstQuestionOptions;
                    return objQuestionTypeDetailModel;
                }
                else if (questionType == CommonUtils.QuestionType.MultiSelect.ToString())
                {
                    //if question type is Multi select than set QuestionOptions of MultiSelect Model in QuestionTypeDetailModel Model and return QuestionTypeDetailModel
                    objQuestionTypeDetailModel.MultiSelect = new MultiSelectModel();
                    objQuestionTypeDetailModel.MultiSelect.QuestionOptions = lstQuestionOptions;
                    return objQuestionTypeDetailModel;
                }
            }
            //if question type is single Line than set properties of SingleLine Model QuestionTypeDetailModel Model and return QuestionTypeDetailModel
            if (questionType == CommonUtils.QuestionType.SingleLine.ToString())
            {
                return SetQuestionTypeValidation(CommonUtils.QuestionType.SingleLine, lstQuestionProperty, objQuestionTypeDetailModel);
            }
            else if (questionType == CommonUtils.QuestionType.MultiLine.ToString())
            {
                //if question type is Multi Line than set properties of MultiLine Model QuestionTypeDetailModel Model and return QuestionTypeDetailModel
                return SetQuestionTypeValidation(CommonUtils.QuestionType.MultiLine, lstQuestionProperty, objQuestionTypeDetailModel);
            }
            else if (questionType == CommonUtils.QuestionType.Number.ToString())
            {
                //if question type is Number than set properties of Number Model QuestionTypeDetailModel Model and return QuestionTypeDetailModel
                return SetQuestionTypeValidation(CommonUtils.QuestionType.Number, lstQuestionProperty, objQuestionTypeDetailModel);
            }
            else if (questionType == CommonUtils.QuestionType.DropDown.ToString())
            {
                //if question type is Number than set properties of Number Model QuestionTypeDetailModel Model and return QuestionTypeDetailModel
                return SetQuestionTypeValidation(CommonUtils.QuestionType.DropDown, lstQuestionProperty, objQuestionTypeDetailModel, DropDownXML);
            }
            else
            {
                //if question type is DateTime than set properties of DateTime Model QuestionTypeDetailModel Model and return QuestionTypeDetailModel
                return SetQuestionTypeValidation(CommonUtils.QuestionType.DateAndTime, lstQuestionProperty, objQuestionTypeDetailModel);
            }
        }

        /// <summary>
        /// Set Question Type Validation like MaxChar,NoOfLine,MinValue , MaxValue etc.
        /// </summary>
        /// <param name="questionType"></param>
        /// <param name="lstQuestionProperty"></param>
        /// <param name="objQuestionTypeDetailModel"></param>
        /// <returns></returns>
        public QuestionTypeDetailModel SetQuestionTypeValidation(CommonUtils.QuestionType questionType, List<QuestionPropertyModel> lstQuestionProperty, QuestionTypeDetailModel objQuestionTypeDetailModel, string DropDownXML = null)
        {
            switch (questionType)
            {
                case CommonUtils.QuestionType.SingleLine:
                    //if question type is single line than set MaxChar Property of SingleLineModel
                    SingleLineModel objSingleLineModel = new SingleLineModel();
                    foreach (QuestionPropertyModel objQuestionPropertyModel in lstQuestionProperty)
                    {
                        if (objQuestionPropertyModel.PropertyText == CommonUtils.QuestionProperty.MAX_CHAR.ToString())
                        {
                            objSingleLineModel.MaxChar = Convert.ToInt32(objQuestionPropertyModel.PropertyValue);
                        }

                    }
                    objQuestionTypeDetailModel.SingleLineTextType = objSingleLineModel;
                    break;

                case CommonUtils.QuestionType.DropDown:

                    objQuestionTypeDetailModel.DropDownType = new SingleLineModel();
                    objQuestionTypeDetailModel.DropDownType.DropDownValue = DropDownXML;
                    break;

                case CommonUtils.QuestionType.MultiLine:
                    //if question type is MultiLine than set NoOfLines Property of MultiLineModel
                    MultiLineModel objMultiLineModel = new MultiLineModel();
                    foreach (QuestionPropertyModel objQuestionPropertyModel in lstQuestionProperty)
                    {
                        if (objQuestionPropertyModel.PropertyText == CommonUtils.QuestionProperty.NO_OF_LINES.ToString())
                        {
                            objMultiLineModel.NoOfLines = Convert.ToInt32(objQuestionPropertyModel.PropertyValue);
                        }

                    }
                    objQuestionTypeDetailModel.MultiLineTextType = objMultiLineModel;
                    break;

                case CommonUtils.QuestionType.Number:
                    //if question type is Number than set MinValue,MaxValue,NoOfDecimal Properties of Number
                    NumberModel objNumberModel = new NumberModel();
                    foreach (QuestionPropertyModel objQuestionPropertyModel in lstQuestionProperty)
                    {
                        if (objQuestionPropertyModel.PropertyText == CommonUtils.QuestionProperty.MIN_VALUE.ToString())
                        {
                            objNumberModel.MinValue = Convert.ToInt32(objQuestionPropertyModel.PropertyValue);
                        }
                        else if (objQuestionPropertyModel.PropertyText == CommonUtils.QuestionProperty.MAX_VALUE.ToString())
                        {
                            objNumberModel.MaxValue = Convert.ToInt32(objQuestionPropertyModel.PropertyValue);
                        }
                        else if (objQuestionPropertyModel.PropertyText == CommonUtils.QuestionProperty.NO_OF_DECIMAL.ToString())
                        {
                            objNumberModel.NoOfDecimal = Convert.ToInt32(objQuestionPropertyModel.PropertyValue);
                        }

                    }
                    objQuestionTypeDetailModel.NumberType = objNumberModel;
                    break;

                case CommonUtils.QuestionType.DateAndTime:
                    //if question type is DateAndTime than set IsDateOnly,DefaultValueType (like None,Todaysdate,specificDate), DateDefaultValue(in case of Specific date) Properties of DateAndTimeModel
                    DateAndTimeModel objDateAndTimeModel = new DateAndTimeModel();
                    foreach (QuestionPropertyModel objQuestionPropertyModel in lstQuestionProperty)
                    {
                        if (objQuestionPropertyModel.PropertyText == CommonUtils.QuestionProperty.ISDATEONLY.ToString())
                        {
                            objDateAndTimeModel.IsDateOnly = Convert.ToBoolean(objQuestionPropertyModel.PropertyValue);
                        }
                        else if (objQuestionPropertyModel.PropertyText == CommonUtils.QuestionProperty.DATETIME_DEFAULT_VALUE.ToString())
                        {
                            objDateAndTimeModel.DefaultValueType = objQuestionPropertyModel.PropertyValue;
                            objDateAndTimeModel.DateDefaultValue = objQuestionPropertyModel.DateDefaultValue;
                        }
                    }
                    objQuestionTypeDetailModel.DateAndTimeType = objDateAndTimeModel;
                    break;
            }
            return objQuestionTypeDetailModel;
        }

        /// <summary>
        /// Fill Question Properties based on QuestionTypeDetail Model
        /// </summary>
        /// <param name="objQuestionTypeDetailModel"></param>
        /// <returns></returns>
        public List<QuestionPropertyModel> FillQuestionProperties(QuestionTypeDetailModel objQuestionTypeDetailModel)
        {
            List<QuestionPropertyModel> lstQuestionPropertyModel = new List<QuestionPropertyModel>();
            QuestionPropertyModel objQuestionPropertyModel = new QuestionPropertyModel();
            try
            {
                if (objQuestionTypeDetailModel != null)
                {
                    //if SingleLineTextType Model is not null i.e. question is singleLineText than set Property MaxChar and Add in list lstQuestionPropertyModel
                    if (objQuestionTypeDetailModel.SingleLineTextType != null)
                    {
                        if (objQuestionTypeDetailModel.SingleLineTextType.MaxChar != null && objQuestionTypeDetailModel.SingleLineTextType.MaxChar > 0)
                        {
                            objQuestionPropertyModel.PropertyText = CommonUtils.QuestionProperty.MAX_CHAR.ToString();
                            objQuestionPropertyModel.PropertyValue = objQuestionTypeDetailModel.SingleLineTextType.MaxChar.ToString();
                            lstQuestionPropertyModel.Add(objQuestionPropertyModel);
                        }

                    }
                    else if (objQuestionTypeDetailModel.MultiLineTextType != null)
                    {
                        //if MultiLineTextType Model is not null i.e. question  is MultiLineText than set Property No Of Lines and Add in list lstQuestionPropertyModel
                        if (objQuestionTypeDetailModel.MultiLineTextType.NoOfLines != null && objQuestionTypeDetailModel.MultiLineTextType.NoOfLines > 0)
                        {
                            objQuestionPropertyModel.PropertyText = CommonUtils.QuestionProperty.NO_OF_LINES.ToString();
                            objQuestionPropertyModel.PropertyValue = objQuestionTypeDetailModel.MultiLineTextType.NoOfLines.ToString();
                            lstQuestionPropertyModel.Add(objQuestionPropertyModel);
                        }
                    }
                    else if (objQuestionTypeDetailModel.NumberType != null)
                    {
                        //if NumberType Model is not null i.e. question is Number type than set Property MinValue, MaxValue, NoOfDecimal and Add in list lstQuestionPropertyModel
                        if (objQuestionTypeDetailModel.NumberType.MinValue != null)
                        {
                            objQuestionPropertyModel.PropertyText = CommonUtils.QuestionProperty.MIN_VALUE.ToString();
                            objQuestionPropertyModel.PropertyValue = objQuestionTypeDetailModel.NumberType.MinValue.ToString();
                            lstQuestionPropertyModel.Add(objQuestionPropertyModel);
                        }
                        if (objQuestionTypeDetailModel.NumberType.MaxValue != null)
                        {
                            objQuestionPropertyModel = new QuestionPropertyModel();
                            objQuestionPropertyModel.PropertyText = CommonUtils.QuestionProperty.MAX_VALUE.ToString();
                            objQuestionPropertyModel.PropertyValue = objQuestionTypeDetailModel.NumberType.MaxValue.ToString();
                            lstQuestionPropertyModel.Add(objQuestionPropertyModel);
                        }
                        if (objQuestionTypeDetailModel.NumberType.NoOfDecimal != null)
                        {
                            objQuestionPropertyModel = new QuestionPropertyModel();
                            objQuestionPropertyModel.PropertyText = CommonUtils.QuestionProperty.NO_OF_DECIMAL.ToString();
                            objQuestionPropertyModel.PropertyValue = objQuestionTypeDetailModel.NumberType.NoOfDecimal.ToString();
                            lstQuestionPropertyModel.Add(objQuestionPropertyModel);
                        }
                    }
                    else if (objQuestionTypeDetailModel.DateAndTimeType != null)
                    {
                        //if DateAndTimeType Model is not null i.e. question is DateAndTime type than set Property IsDateOnly, DefaultValue(in case of Specific date), DefaultValueType(None,Todaysdate,SpecificDate) and Add in list lstQuestionPropertyModel
                        if (objQuestionTypeDetailModel.DateAndTimeType.IsDateOnly != null)
                        {
                            objQuestionPropertyModel.PropertyText = CommonUtils.QuestionProperty.ISDATEONLY.ToString();
                            objQuestionPropertyModel.PropertyValue = objQuestionTypeDetailModel.DateAndTimeType.IsDateOnly.ToString();
                            lstQuestionPropertyModel.Add(objQuestionPropertyModel);
                        }
                        if (objQuestionTypeDetailModel.DateAndTimeType.DefaultValueType != null)
                        {
                            objQuestionPropertyModel = new QuestionPropertyModel();
                            objQuestionPropertyModel.PropertyText = CommonUtils.QuestionProperty.DATETIME_DEFAULT_VALUE.ToString();
                            objQuestionPropertyModel.PropertyValue = objQuestionTypeDetailModel.DateAndTimeType.DefaultValueType.ToString();
                            objQuestionPropertyModel.DateDefaultValue = objQuestionTypeDetailModel.DateAndTimeType.DateDefaultValue;
                            lstQuestionPropertyModel.Add(objQuestionPropertyModel);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            //return list of question properties
            return lstQuestionPropertyModel;
        }


        #endregion

        #region Properties

        public static string preLoginPageController = "User";
        public static string preLoginPageActionMethod = "Login";

        public static Int16 PageSize
        {
            get
            {
                Int16 _pageSize = 10;
                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["PageSize"]))
                {
                    Int16.TryParse(ConfigurationManager.AppSettings["PageSize"], out _pageSize);
                }
                return _pageSize;
            }
        }
        public static string ShortDateFormat
        {
            get
            {
                string _dateFormat = "dd-MM-yyyy";
                _dateFormat = CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern;
                return _dateFormat;
            }
        }
        public static string LongDateFormat
        {
            get
            {
                string _dateFormat = "dd-MM-yyyy HH:mm";
                _dateFormat = String.Concat(CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern, " HH:mm");
                return _dateFormat;
            }
        }
        public static string DateSeprator
        {
            get
            {
                string _dateSeprator = "-";
                _dateSeprator = CultureInfo.CurrentCulture.DateTimeFormat.DateSeparator;
                return _dateSeprator;
            }
        }

        public static string WebAPIURL
        {
            get
            {
                string _WebAPIURL = string.Empty;
                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["WebAPIURL"]))
                    _WebAPIURL = ConfigurationManager.AppSettings["WebAPIURL"] + "api/";
                return _WebAPIURL;
            }
        }
        public static string AdminURL
        {
            get
            {
                string _AdmminURL = string.Empty;
                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["AdmminURL"]))
                    _AdmminURL = ConfigurationManager.AppSettings["AdmminURL"];
                return _AdmminURL;
            }
        }
        public static string FromEmail
        {
            get
            {
                string _fromEmail = string.Empty;
                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["FromEmail"]))
                    _fromEmail = ConfigurationManager.AppSettings["FromEmail"];
                return _fromEmail;
            }
        }

        public static string AllowedFileExtension
        {
            get
            {
                string _fileExtension = "";
                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["AllowedFileExtension"]))
                    _fileExtension = ConfigurationManager.AppSettings["AllowedFileExtension"];
                return _fileExtension;
            }
        }
        public static int AllowedMaxFileSize
        {
            get
            {
                int _maxFileSize = 52428800;
                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["AllowedMaxFileSize"]))
                    int.TryParse(ConfigurationManager.AppSettings["AllowedMaxFileSize"], out _maxFileSize);
                return _maxFileSize;
            }
        }
        public static string DefaultPassword
        {
            get
            {
                string _defaultPassword = "";
                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["DefaultPassword"]))
                    _defaultPassword = ConfigurationManager.AppSettings["DefaultPassword"];
                return _defaultPassword;
            }
        }
        public static int ExpirePasswordDays
        {
            get
            {

                int _expirePasswordDays = 30;
                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["ExpirePasswordDays"]))
                    int.TryParse(ConfigurationManager.AppSettings["ExpirePasswordDays"], out _expirePasswordDays);
                return _expirePasswordDays;

            }
        }
        public static int MaxLoginTry
        {
            get
            {
                int _MaxLoginTry = 3;
                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["MaxLoginTry"]))
                    int.TryParse(ConfigurationManager.AppSettings["MaxLoginTry"], out _MaxLoginTry);
                return _MaxLoginTry;

            }
        }

        #endregion

        #region Enum

        public enum MessageType { Success, Error, Notice }
        public enum QuestionType { SingleLine, MultiLine, SingleSelect, MultiSelect, DateAndTime, Number, DropDown, Price_Question,Price }
        public enum QuestionProperty { MAX_CHAR, NO_OF_LINES, MIN_VALUE, MAX_VALUE, NO_OF_DECIMAL, ISDATEONLY, DATETIME_DEFAULT_VALUE }
        public enum DisplayChoice { Radio, CheckBox, DropDown }
        public enum DateDefaultValue { None, TodaysDate, SpecificDate }

        public enum EmailType { ForgotPassword = 1, ForgotUsername = 2, }
        public enum NotificationType
        {


        }
        public enum ErrorType : int
        {
            INCORRECTUSERNAMEPASSWORD = 52,
            SUCCESS = 0
        }

        #endregion

        public static Boolean Send(System.Net.Mail.MailMessage message)
        {
            //string UserName = "pms.test2013@gmail.com";
            //string Password = "pmstest123456";
            //string smtp = "smtp.gmail.com";
            //string port = "587";

            // ConfigurationManager.AppSettings.Get("UserID"
            string UserName = ConfigurationManager.AppSettings["UserName"].ToString();
            string Password = ConfigurationManager.AppSettings["Password"].ToString();
            string smtp = ConfigurationManager.AppSettings["SMTPHost"].ToString();
            string port = ConfigurationManager.AppSettings["SMTPPort"].ToString();

            message.From = new System.Net.Mail.MailAddress(UserName, "Papeleslegales.com");

            //Send the message.
            System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient(smtp, Convert.ToInt32(port));
            client.EnableSsl = false;
            client.UseDefaultCredentials = false;
            System.Net.NetworkCredential myCreds = new System.Net.NetworkCredential(UserName, Password);
            client.Credentials = myCreds;


            try
            {
                client.Send(message);
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }
    }
}
