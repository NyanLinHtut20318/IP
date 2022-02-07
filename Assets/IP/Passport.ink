VAR validName = "true"
VAR NAME = ""
VAR validcountry = "false"
VAR Country = ""
VAR validDOB = "true"
VAR DOB =""
VAR validSex = "true"
VAR validPassNum = "true"
VAR validIssueDate = "true"
VAR validExpiryDate = "true"


*[NameVerfiy]
->checkName
*[CountryVerify]
->check_Country
*[DOBVerify]
->check_DOB

==checkName

YOU Sorry, may I ask your name again?
{ validName:
  {validName=="true": My name is {NAME}}
  {validName=="false": 
  My name is{NAME}
  ->response_Name
  }
  ->DONE
  
}
==response_Name
YOU Hi your name don't seem to match your passport 
Tra How can that be!
->DONE

==check_Country
YOU Sorry, may I ask where're you from?
{ validcountry:
  {validcountry=="true":I'm from {Country}}
  {validcountry=="false": 
  I'm from {Country}
  ->response_Coutnry
  }
  ->DONE
}
==response_Coutnry
YOU Hi your country don't seem to match your passport 
Tra Are you doubting my nationality!
->DONE

==check_DOB==
YOU Sorry, may I ask your Date of birth?
{ validDOB:
  {validDOB=="true":I'm from {DOB}}
  {validDOB=="false": 
  My date of birth is {DOB}
  ->response_DOB
  }
  ->DONE
}
==response_DOB
YOU Hi your country don't seem to match your passport 
Tra Are you doubting my birth!





->END
