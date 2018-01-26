using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RepidShare.Entities;
using System.Data;
using RepidShare.Data;


namespace RepidShare.Business
{
    public class BLQuestionType: BLBase
    {
        /// <summary>
        /// Get Question Type List
        /// </summary>
        /// <returns></returns>
        public List<QuestionTypeModel> GetQuesetionTypeList()
        {
            List<QuestionTypeModel> lstQuestionTypeModel = new List<QuestionTypeModel>();

            DataTable dtQuestionType = DLQuestionType.GetQuesetionTypeList();

            foreach (DataRow dr in dtQuestionType.Rows)
            {

                QuestionTypeModel objQuestionTypeModel = GetDataRowToEntity<QuestionTypeModel>(dr);
                lstQuestionTypeModel.Add(objQuestionTypeModel);
            }

            return lstQuestionTypeModel;
        }

        /// <summary>
        /// Get All Display Choice
        /// </summary>
        /// <returns></returns>
        public List<DisplayChoiceModel> GetAllDisplayChoice()
        {
            try
            {
                List<DisplayChoiceModel> lstDisplayChoice = new List<DisplayChoiceModel>();
                //Get All application name list
                DataTable dtDisplayChoice = DLQuestionType.GetAllDisplayChoice();
                //convert rows into DropdownModel Item
                foreach (DataRow dr in dtDisplayChoice.Rows)
                {
                    DisplayChoiceModel objDisplayChoiceModel = new DisplayChoiceModel();
                    objDisplayChoiceModel = GetDataRowToEntity<DisplayChoiceModel>(dr);
                    lstDisplayChoice.Add(objDisplayChoiceModel);
                }
                return lstDisplayChoice;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
