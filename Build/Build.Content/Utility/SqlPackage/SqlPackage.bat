ECHO OFF


ECHO SqlPackage.exe /a:Extract /ssn:<your server name> /sdn:<your db name> /tf:<file for new dacpac> /p:VerifyExtraction= false

.\SqlPackage.exe /a:Extract /ssn:<your server name> /sdn:<your db name> /tf:<file for new dacpac> /p:VerifyExtraction= false