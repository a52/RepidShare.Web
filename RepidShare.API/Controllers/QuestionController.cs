using RepidShare.Business;
using RepidShare.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace RepidShare.API.Controllers
{
    public class QuestionController : ApiController
    {
        private BLQuestion objBLQuestion = new BLQuestion();
        private BLQuestionType objBLQuestionType = new BLQuestionType();

        [HttpPost]
        public ViewQuestionModel GetQuestions(ViewQuestionModel objViewQuestionModel)
        {
            return objBLQuestion.GetQuestions(objViewQuestionModel);
        }

        [HttpPost]
        public ViewQuestionModel GetQuestionsList(ViewQuestionModel objViewQuestionModel)
        {
            return objBLQuestion.GetQuestionsList(objViewQuestionModel);
        }

        [HttpPost]
        public ViewQuestionModel InsertUpdateQuestion(ViewQuestionModel objViewQuestionModel)
        {
            return objBLQuestion.InsertUpdateQuestion(objViewQuestionModel);
        }

        [HttpPost]
        public ViewQuestionModel DeleteQuestion(ViewQuestionModel objViewQuestionModel)
        {
            //Delete  Category by DeletedCategoryID 
            return objBLQuestion.DeleteQuestion(objViewQuestionModel);
        }

        [HttpGet]
        public List<QuestionTypeModel> GetQuesetionTypeList()
        {
            return objBLQuestionType.GetQuesetionTypeList();
        }

        [HttpGet]
        public List<DisplayChoiceModel> GetAllDisplayChoice()
        {
            return objBLQuestionType.GetAllDisplayChoice();
        }
    }
}
