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


## Never Perfect: What to Do Next

1. **Microservices Architecture**: Transition to a microservices architecture and containerize the application. Even if it’s just a single container for now, it would enhance mobility.

2. **Tests**: Implement tests. Due to time constraints, I did not write tests, but they are necessary for ensuring code quality and reliability.

3. **Transactions**: Ensure that all actions are wrapped in transactions to prevent data inconsistency. For example, the file uploading process includes creating a database entity, saving the file on the server, and creating a preview image—all should be done within a single transaction.

4. **Localization**: Currently, all interface strings are hard-coded. Implement a localization dictionary to make the code cleaner and more extensible. This will facilitate the addition of new languages and translations without having to screen all `.js` files.

