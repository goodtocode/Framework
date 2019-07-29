
Install Test Tools Standalone (for build server): http://www.microsoft.com/en-us/download/details.aspx?id=40750 

Including files from the GAC: http://blog.anthonybaker.me/2013/05/running-mstest-without-visual-studio.html

* Gac files: C:\Windows\assembly\GAC

Had to remove from project file, was causing "Assembly not found" type errors when running via MSTest.exe
 <Choose>
    <When Condition="'$(VisualStudioVersion)' == '10.0' And '$(IsCodedUITest)' == 'True'">
