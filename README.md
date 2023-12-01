Project: CommissionManager

Overview

The Commission Manager is a small application for managing your commissions as an artist. 
Currently the application allows you to create commissions, edit them and delete them.
Each commission tracks its created date and its own deadline. They also feature a description box for short explanations of their contents.
In technical terms this is an ASP.NET CRUD API running locally with a WPF GUI application for users to interact.
Over all the project follows MVC architecture pattern as users interact through views that use controllers and models to update data within those views.
Currently there are a number of features planned for this project in future updates.

Requirements: .NET Version 8.0

Feature list Implementation

1. Implement a regex expression - Inside the UserProfile model of the API there’s an email field in which I use a regex expression to ensure the emails stays within your typical email format

2. Create a dictionary or list - Within the CommissionsController and the GetCommissionByEmail method, I use a list to store the commissions returned by the commission repository. Within the called repository I also use a list numerous times to store these objects and then use them within the program.

3. Add comments to your code explaining how you are using at least 2 of the solid principles - There are several places where I list each of my uses of the principles including the commissionRepository, The AuthToken Class and the UserProfileController.

4. Make your application an API - It is an API

5. Make your application a CRUD API - It's my understanding that since I you can create, retrieve, update, and delete objects from my database it is a CRUD API.

6. Make your application asynchronous - I have used async methods at every opportunity

7. Have 2 or more tables (entities) in your application that are related and have a function to return data from both entities.  In entity framework, this is equivalent to a join.

While I did not perform an explicit join I did query the UserProfiles table to return the users email and then later query the commissions table using that same retrieved email and felt it was worthy of noting. I will gladly accept it if this doesn’t meet the requirements. But I do feel it shows my understanding of the potential relationship between two objects from separate tables and how that can be leveraged.

8. Query your database using a raw SQL query, not EF - I didn’t use EF core for queries and have several raw SQL queries that should meet this requirement.

INSTRUCTIONS

1. Clone the repo to your computer
2. Double click StartComManager.bat to run the applications
3. This will open 3 windows, do not close them!
4. Log into the GUI using the credentials "example@email.com" and "SecurePassword"
5. Manage your commissions!

WARNING! The Client ID section is intended for future features. Please leave the field empty or the application WILL crash. This will be adressed in in a future update.



