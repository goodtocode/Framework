<div style="margin: 0px auto; width: 100%; color: slategray;">
<table>
<tbody>
<tr>
<td>

<img alt="Your Genesys Source Framework" src="https://www.genesyssource.com/images/welcome/Genesys-Source-Logo-71x71-Transparent.png">

</td>
<td>

<h2> Genesys Source Framework </h2>

<h4>One Framework - Your Data - Any Platform</h4>

</td>
</tr>
<tr>
<td colspan="2">

### Features & Benefits

Genesys Source Framework helps you create your business-object framework with standard C# knowledge and minimal plumbing. Your objects can migrate full-stack, and be consumed in MVC, Web API, WPF, UWP and Xamarin iOS/Android. Works with your existing SQL tables, and allows you to incrementally build your reusable Framework one object, one page at a time.

</td>
</tr>
</table>
</tbody>
<table>
<tbody>
<tr>

<td style="vertical-align: top; border-left-color: rgb(213, 213, 213); border-left-width: 1px; border-left-style: solid;">

### Projects included in the Genesys Source Framework

<div>

<strong>Framework.WebApp</strong> - <span style="text-align:justify; line-height:18px">MVC Web App project. Small starter MVC web app that creates, reads, updates and deletes your reusable Framework objects in this solution. </span>

<strong>Framework.WebServices</strong> - <span style="text-align:justify; line-height:18px">Web API Web Services project. Small starter web service that creates, reads, updates and deletes your reusable Framework objects in this solution. </span>

<strong>Framework.UniversalApp</strong> - <span style="text-align:justify; line-height:18px">UWP Universal App project. Small starter cross-platform app that creates, reads, updates and deletes your reusable Framework objects in this solution. </span>

<strong>Framework.DesktopApp</strong> - <span style="text-align:justify; line-height:18px">WPF Desktop App project. Small starter desktop native app that creates, reads, updates and deletes your reusable Framework objects in this solution. </span>

<strong>Framework.Models</strong> - <span style="text-align:justify; line-height:18px">Cross-Platform View Models project. Contains the Framework-level view models for http transport and .Serialize() built-in.</span>

<strong>Framework.Interop</strong> - <span style="text-align:justify; line-height:18px">Cross-Platform Interface project. All projects reference this project so that interfaces are enforced in all tiers, and on mobile device as well.</span>

<strong>Framework.DataAccess</strong> - <span style="text-align:justify; line-height:18px">Entity Framework (EF)/Data Access Object (DAO) project. Contains EF (database first), Data Access Objects, CRUDEntity, ModelEntity, EntityReader and EntityWriter.</span>

<strong>Framework.Database</strong> - <span style="text-align:justify; line-height:18px">SQL Server Data Tools (SSDT) project. Contains view/SP layer for the Framework.DataAccess to consume.</span>

</div>
<br />

### More information

<div>
    <table>
        <tr>
            <td><a href="https://docs.genesyssource.com/products/genesys-framework/genesys-framework-ebook.pdf" target="_blank">[PDF]</a></td>
            <td><a href="https://docs.genesyssource.com/products/genesys-framework/genesys-framework-ebook.pdf" target="_blank">Genesys Framework eBook</a></td>
        </tr>
        <tr>
            <td><a href="https://docs.genesyssource.com/reference/genesys-framework" target="_blank">[API]</a></td>
            <td><a href="https://docs.genesyssource.com/reference/genesys-framework" target="_blank">Genesys Framework Reference</a></td>
        </tr>
        <tr>
            <td><a href="https://docs.genesyssource.com" target="_blank">[Docs]</a></td>
            <td><a href="https://docs.genesyssource.com" target="_blank">docs.genesyssource.com</a></td>
        </tr>
        <tr>
            <td><a href="https://github.com/GenesysSource/Framework/issues/new" target="_blank">[+/-]</a></td>
            <td><a href="https://github.com/GenesysSource/Framework/issues/new" target="_blank">Report an Issue</a></td>
        </tr>
        <tr>
            <td><a href="https://cloud.genesyssource.com/genesys-framework" target="_blank">[Zip]</a></td>
            <td><a href="https://cloud.genesyssource.com/genesys-framework" target="_blank">Download Genesys Framework</a></td>
        </tr>
        <tr>
            <td><a href="https://www.microsoft.com/net/download" target="_blank">[Azure]</a></td>
            <td><a href="https://www.microsoft.com/net/download" target="_blank">Cloud Web Environment</a></td>
        </tr>
    </table>
</div>

</td>

<td style="vertical-align: top; border-left-color: rgb(213, 213, 213); border-left-width: 1px; border-left-style: solid;">

<div style="text-align: left; color: red;">

### Critical Next Steps

<strong>❶ Install .NET Core SDK <a href="https://www.microsoft.com/net/download">[download]</a></strong>
<br />
<strong>❷ Update Visual Studio <a href="https://docs.microsoft.com/en-us/visualstudio/install/update-visual-studio?view=vs-2017">[download]</a></strong>
<br />
<strong>❸ Build solution to verify .NET Core</strong>

</div>

<div>

<br />

### Developer Next Steps

<div>❶ Right-click -> Set as Startup Project</div>
<br />
<div>❷ Press F5 to run debugger</div>
<br />
<div>❸ Click Search to lookup a customer</div>

</div>

<br />

### Frequently Asked Questions

**Where is the DB connection string?**

1.  Open \App_Data\ConnectionStrings.json
2.  Change _DefaultConnection_ to match your DB

**Where is the Web Service Url?**

1.  Open \App_Data\AppSettings.json
2.  Change _MyWebService_ to match your Url

**How to read from my database?**

1.  Open Framework.Database\ CustomerCode\ Views\ CustomerInfo.sql
2.  Change this view to join to your "Person" table

**How to write to my database?**

1.  Open Framework.Database\ CustomerCode\ Stored Procedures\ CustomerInsert.sql
2.  Change this SP to insert to your "Person" table

**How to publish the database?**

1.  Open Framework.Database\Publish\PublishToDev.publish.xml (Ensure database connection is correct)
2.  Click Generate Script and review
3.  Click Publish to push changes to SQL

**How to publish to a web server?**

1.  In Solution Explorer, right-click Framework.WebApp
2.  Click Publish, the Publish window will display
3.  Click the _Settings..._ link in the Publish window
4.  Change Target Location to the dev web site folder path, click Save
5.  Click Publish to publish the project to your development web server

</td>

</tr>

<tr>

<td style="border-top-color: rgb(213, 213, 213); border-top-width: 1px; border-top-style: solid; background-color: rgb(247, 247, 247);" colspan="2">

<div style="padding: 15px 40px 15px 15px; text-align: center; vertical-align: top;">

<div style="text-align:center;font-size: 1.6em; font-weight: bold;">
<strong>GENESYSSOURCE</strong>

22431 Antonio, Suite B160-843
<br />
Rancho Santa Margarita, CA 92688
<br />
+1 949.544.1900
<br />

[genesyssource.com](http://www.genesyssource.com) | [@genesyssource](http://www.twitter.com/genesyssource)


</div>
</div>

</td>

</tr>

</tbody>

</table>

</div>