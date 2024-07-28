# Heinekamp Code Challenge

## About

The app is coded with C# and .NET tools for the backend, Entity Framework Core with PostgreSQL for data storage, and JavaScript + React for the frontend. All third-party libraries are free to use and don’t require any licenses. They are also cross-platform, so the solution can be launched on Windows, Linux, and macOS.

## How to Launch the Solution

1. **Clone the Repository**

   Clone the repo from [https://github.com/nikolas-mo4-11/heinekamp.git](https://github.com/nikolas-mo4-11/heinekamp.git)

2. **Install PostgreSQL Server and Connected Tools**

   a) Go to [PostgreSQL Downloads](https://www.postgresql.org/download/)  
   b) Choose your OS and version (I used v. 12.19 for Windows)  
   c) During the installation, you’ll be prompted to install additional tools, such as pgAdmin. Confirm all options just in case.  
   d) You will also be asked to set a password. Remember it, as you’ll need it later!

3. **Install Node.js**

   a) Go to [Node.js Downloads](https://nodejs.org/en/download/package-manager)  
   b) Follow the instructions; I used version 14.16.0.

4. **Set the Configurations**

   a) Open the solution (you should have MS Visual Studio, JetBrains Rider, or any other tool capable of running .NET solutions).  
   b) Open `Heinekamp/appsettings.json`.  
   c) In the following line set your pg password from the step 1d instead of ‘qwerty’
   ```json
   "PgDbConnectionString": "Server=localhost;Database=heinekamp;User Id=postgres;Password=qwerty"

5. **Install npm Packages and Build the Frontend**

   (Note: This part might differ for non-Windows users)  
   a) Open the terminal and run the following commands:  
   b) `cd Heinekamp`  
   c) `npm install`  
   d) `npm run build`  
   e) `npm start`

6. **Build and Run the Solution**

   After completing the above steps, build and run the solution using your development environment.

## Main architecture and design decisions

The best documentation for the code is the code, but I'll write about some key points:

1. **Frontend**

index.js is the root file of the frontend part. It contains the App component that includes all the frontend logic. I tried to use maximum of react features to make the page react dynamically on user's actions without reloading the page. Also, I used Ant library of React components to make the UI look better (it's license free).

2. **Static resources**

Static resources are stored at wwwroot folder.

3. **Startup**

The Startup.cs file is the root file of backend (except Program.cs where the app starts running), where all the necessary modules, migrations and dependencies are described. I used internal System DI tools for Dependency Injection.

4. **Entity framework core**

I used EF core with Postgree SQL for data storage. Heinekzmp.PgDb assembly contains all the stuff connected with DB, such as DbContext and its Factory, repositories classes, and the folder Migrations with all the information about migrations (that are being applied during the startup)

5. **API**

Frontend and backend are connected by API. The endpoints are in the files DocumentApi.js and DocumentController.cs

6. **Backend structure**
   
Basically, the backend has 3 layers: Controllers layer (API description), Service layer (all the business logic), and Repository layer containing the operations with Database

7. **Asynchronously**

Almost all the operations need to read/write data from/to DB, some of them need a lot of resources to be finished as well (previews creation, for example). This brings me to the decision to make all the code async, and also run number of tasks at the same time sometimes (for document creation and downloading). Most of the asynchronous logic on the backend you can see in DocumentService.cs. On the frontend I used Promises to support asynchronous methods (with .then.catch construction)

8. **Preview creation**

This is the most difficult operation on the backend, it needs a lot of resources, and thanks to a huge number of extensions to support, it also needed a good architecture decision. I have a Generator for each format to create the preview picture for the file and to save it to the server memory. PreviewGeneratorResolver class resolves which Generator we need for each concrete file, then method CreatePreview of the Generator is called. All the Generators implement the IPreviewGenerator interface.

## Interface description

**Main page**
The interface is simple: on the main page there is the table with Documents' names, creation dates, download counts and actions. The table has a pagination.

Action buttons:
1. Download button - download the document
2. Preview button - open the preview popup
3. Create document link button - open the link creatin popup

There are also 2 buttons in the header
1. Upload - open the upload documents popup
2. Download selected [visible only when there are selected documents] - downloads an archive with selected documents

**Preview popup**
Contains information about the document - name, creation date, downloads count and preview image
On the bottom there are 3 buttons: close (to close popup), delete (to delete the doc), edit. Edit button activate 'Edit mode' where instead of the name of the doc we see input window, so we can change the name. After that we can push the Save button to save changes.

**Create download link popup**
We can create a temporary link here. Enter the amount and select the unit to set the availability period (you'll see the expiration time below). Then push the Create link button, the link will be shown below. Copy it and use! If the link is expired - you'll see 404 error while trying to download the document.

**Upload files popup**
Push Upload files, select needed docs. Then you'll see a table with docs' information. Push the Upload button to upload documents to the system.




## Never Perfect: What to Do Next

1. **Microservices Architecture**: Transition to a microservices architecture and containerize the application. Even if it’s just a single container for now, it would enhance mobility.

2. **Tests**: Implement tests. Due to time constraints, I did not write tests, but they are necessary for ensuring code quality and reliability.

3. **Transactions**: Ensure that all actions are wrapped in transactions to prevent data inconsistency. For example, the file uploading process includes creating a database entity, saving the file on the server, and creating a preview image—all should be done within a single transaction.

4. **Localization**: Currently, all interface strings are hard-coded. Implement a localization dictionary to make the code cleaner and more extensible. This will facilitate the addition of new languages and translations without having to screen all `.js` files.

5. **Optimaze DB**: Short up the number of operations by creating more specific repository methods or SQL-functions
