/*-------------------------------------------------------------------------------
Created By: Rakesh Patidar
Date: 10 Feb 2015
Description: Initial Creation
----------------------------------------------------------------------------------
  * Revision History
 
 Modify By          Date            Desc                                Line No 
 Rakesh Patidar     11 Feb 2015     Add FillApplicationNameDropDown     33
 Rakesh paridar     12 Feb 2015     Add GetAllApplicationNameList       41
 ----------------------------------------------------------------------------------*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RepidShare.Entities;
using RepidShare.Data;
using System.Data;


namespace RepidShare.Business
{
    public class BLCommon : BLBase
    {




        #region enum
        public enum QuestionType { SingleLine, MultiLine, SingleSelect, MultiSelect, DateAndTime, Number }
        #endregion
    }
}
