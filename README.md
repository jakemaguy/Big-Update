# Big Update 
The motivation for this application was to reduce the annoyance factor in regard to sending daily status update emails to project managers.  You can quickly enter various tasks and details that you worked on throughout the day. 

<b>This application is focused around sending Outlook emails, since It's the email platform that the company I work for uses.</b>

## Built With
* [Dapper](https://www.nuget.org/packages/Dapper/) - Simplified database querying
* [Microsoft Office Interop Outlook](https://www.nuget.org/packages/Microsoft.Office.Interop.Outlook/) - Microsoft's Outlook Assembly
* [SQLite Core](https://www.nuget.org/packages/System.Data.SQLite.Core/) - Database engine for .NET

### Main Menu Navigation 
Use the ↑ and ↓ arrow keys to navigate all the menus.

Press <i><b>enter</b></i> to go into a menu item

![navigation](https://github.com/jakemaguy/Big-Update/blob/master/assets/navigation.gif)

### Database Operations
You can simply enter a contacts name and corresponding email address, for ease of use in the future

To add a contact: Enter the recipients name, followed by their outlook email address.

![addcontact](https://github.com/jakemaguy/Big-Update/blob/master/assets/addcontact.gif)

To remove a contact: Enter the ID of the database contact you wish to remove

![removecontact](https://github.com/jakemaguy/Big-Update/blob/master/assets/removecontact.gif)

### Task Entry UI
To enter tasks:  Enter a detailed description of the task you performed.  Press <b><i>tab</i></b> to change the menu selection.  You can then add details to go along with the current task to give the reader more insight to what you accomplished and any setbacks.

![task-entry](https://github.com/jakemaguy/Big-Update/blob/master/assets/taskentry.gif)

### Email Composition

Composing an email is as easy as selecting the ID's from the current database table.  All you have to do is enter a CSV list of ID's and the program will send out emails to all of those contacts.

![send-email](https://github.com/jakemaguy/Big-Update/blob/master/assets/sendemail.gif)

In the above example, the user entered: <b><i>1,2</b></i>  -  This will send the email to those two contacts stored in database. 

## Sample Output - What The Recipient Will See
![sample-output](https://github.com/jakemaguy/Big-Update/blob/master/assets/sampleoutput.JPG)
