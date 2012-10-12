CouchDB.NET
-----------
Libraries and providers to use CouchDB features from .NET

This distribution includes the following projects:
- MachineKeyGenerator: Command line tool to generate a machine key string for use in App.Config and Web.Config files.
- CouchDB.NET: Library to facilitate the use of CouchDB features. It uses Hadi Hariri's EasyHttp library to communicate with the CouchDB server. More info at: https://github.com/hhariri/EasyHttp
- CouchDb.ASP.NET: ASP.NET Membership Provider and ASP.NET Roles Provider for Couch DB. It allows you to use CouchDB as a backend for these Membership features of ASP.NET
- CouchDb.ASP.Net.Specs: Unit test project for the library. Based on SpecFlow and NUnit. You'll need both to run the tests. More info at: http://specflow.org/ and http://www.nunit.org/. I also recommend using TestDriven.Net to run the tests, available at: http://testdriven.net/.
- TestWebSite: ASP.NET MVC test web site to show the usage and configuration on a sample site.
- TestWebSitePopulation: Command line tool to create the objects needed for the TestWebSite.

Usage:
The first thing to try out should be the Specs project. For that you need SpecFlow and NUnit.
- Create a database on CouchDB. The config files are already configured to use a database named "couchdbnet" without the quotes running on localhost:5984.
- Run the MachineKeyGenerator tool and copy the key.
- Paste the key in CouchDb.ASP.Net.Specs/App.Config and CouchDb.ASP.Net.Specs/Web.Config. The key is needed because the Membership provider only accepts hashed passwords.
- Run the Specs with NUnit. Configure NUnit to use the Web.Config file as its configuration file. This way you make sure you are using an actual ASP.NET config file. If you run the tests with the debugger, you'll be using App.Config, that's why you may need both files present and configured.

Running the test web application:
- Configuring the application. You may use the TestWebSitePopulation project to create and configure a database for the test site.
  You first need to generate a key to paste on the App.config file (using MachineKeyGenerator) just as you did before, so the user password hashes can be created.
  This application will create a database named "aspnetsitetest" on the localhost server running on port 5984 (the defaults).
  Once created it will add the views needed by the ASP.NET providers.
  It will also create two roles, "Admin" and "RegularUser", and two users, "admin@couchdb.test" assigned to the "Admin" role and "user@couchdb.test" assigned to the "RegularUser" role.
  Both users will have the password "couchdb" without the quotes.

- Once created you should configure the ASP.NET site.
  Copy the key generated before and paste it in the Web.Config file for the TestWebSiteProject so that the passwords can be validated.
  Verify that the membership section and roles section of the Web.Config file match the database server name, port and database name configured before.
  Run the application and login with a regular user (user@couchdb.test). Then logout and login again with the admin user (admin@couchdb.test).
  Verify that you can access the admin menu/url only when you are logged in as admin.

