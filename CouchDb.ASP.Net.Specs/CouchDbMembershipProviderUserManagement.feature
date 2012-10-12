Feature: CouchDBMembershipProvider User Managing
	In order be able to use Couch DB with ASP.NET
	As an ASP.NET programmer
	I want to be able to use CouchDbMembershipProvider to manage my site's users

@UserCreation
Scenario: Add a new user to the Membership Database with only the UserName and Password
	Given I have the data available to define a new application user, the userName and Password
	When I call the Membership API to create the user
	Then the Membership API must respond with a valid user with a new generated ProviderUserKey
	And the Membership API should create the user with a hashed password

@UserValidation
Scenario: The provider grants access to the user given his valid UserName and Password
	Given a user's UserName and Password
	When I call the ValidateUser Membership API
	Then the response is that the user is valid

@UserValidation
Scenario: The provider denies access to the user given his valid UserName but invalid Password
	Given a user's UserName and Invalid Password
	When I call the ValidateUser Membership API passing the invalid password
	Then the response is that the user is invalid


@UserRetrieval
Scenario: Retrieve the list of all the users in the database
	Given A configured Membership application
	When I call the GetAllUsers method in the Membership API
	Then I should get a collection with all the users defined in the database
	And Each user in the collection must be of type CouchDbMembershipUser
	And Each user in the collection musts have a unique ID and UserName

@UserRetrieval
Scenario: Retrieve a known user's data
	Given a known user name
	When I call the GetUser Membership API passing the user name
	Then the user's data is returned as a CouchDBMembershipUser object

@UserRetrieval
Scenario: Retrieve a known user's UserName by his email address
	Given a known email address
	When I call the GetUser Membership API passing the email
	Then the user's UserName is returned

@UserCreationFailure
Scenario: Can not create a new user with a duplicate UserName
	Given a known user
	When I try to create a new user with the same UserName
	Then the user creation operation fails

@UserCreationFailure
Scenario: Can not create a new user with a duplicate email address
	Given a known user's email address
	When I try to create a new user with the same email
	Then the user creation operation fails for that email

@UserPasswordManagement
Scenario: A user can change his password
	Given a valid username and password
	When the user requests to change his password
	Then the password change operation succeds

@UserDeletion
Scenario: A user can be deleted from the database
	Given a valid username
	When I call the DeleteUser method on the MemberchipProvider
	Then all the user's data is deleted