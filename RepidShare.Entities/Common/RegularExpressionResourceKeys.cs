

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RepidShare.Entities
{
    public class RegularExpressionResourceKeys
    { 
        public const string SpecialCharacterPattern = "^[^;>;&;<;%?*!~'`;:\";+=|]*$";

        public const string NumericPattern = "^[0-9]{1,18}$";

        public const string NumericPatternWithHyphen = "^[0-9-]{1,13}$";

        public const string DecimalPattern = "^[0-9]+(\\.[0-9]{1,2})?$";

        public const string DecimalPattern18_4 = "^[0-9]{0,18}(\\.[0-9]{1,4})?$";

        public const string AlphabetPattern = "^[a-zA-Z]{1,100}$";

        public const string AlphaNumeric = "^[^;<;&;>;%?*!~'`;:,.\";+=|;#$@#(){}-]*$";

        public const string UsernamePattern = "^[a-zA-Z0-9._]{1,100}$";

        public const string PasswordPattern = @"^[A-Za-z0-9]+[\W_]+.*$";
        
        public const string FinancialYearPattern = "^[0-9-]*$";
        
        public const string AlphabetOnly = "^[a-zA-Z' ']+$";
        
        public const string EmailPattern = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";

        public const string NumericPatternPincode = "^[6]{1}[0-9]{5}$";
        
        public const string DatePattern = @"^(((0[1-9]|[12]\d|3[01])\/(0[13578]|1[02])\/((19|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\/(0[13456789]|1[012])\/((19|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\/02\/((19|[2-9]\d)\d{2}))|(29\/02\/((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$";

        public const string PanCardPattern = @"^([a-zA-Z]){5}([0-9]){4}([a-zA-Z]){1}?$";
        
        public const string NumericPatternTanNumber = "^([a-zA-Z]){4}([0-9]){5}([a-zA-Z]){1}$";

        public const string QuestionAnswerSpecialCharacterPattern = "^[^;>;&;<;%*!~'`;:\";+=|]*$";
    }
}
