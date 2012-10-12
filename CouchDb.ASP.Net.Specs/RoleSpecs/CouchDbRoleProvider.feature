Feature: CouchDb role provider for ASP.NET
	Role Provider to use with ASP.NET Membership
	Having the role data kept in Couch Db
	
@Role_management
Scenario: Add a new role to the database
	Given I have defined a new role
	When I call the CreateRole API
	Then the role is added to the database
	And I can retrieve it using the GetAllRoles API

@Role_management
Scenario: Retrieve all the roles defined
	Given there are roles defined in the database
	When I call the GetAllRoles API
	Then I receive all the roles defined

@Role_management
Scenario: Delete a role from the database
	Given I have a valid role
	When I call the DeleteRole API
	Then the role is deleted
	And I can not retrieve it using the GetAllRoles API

@Role_management
Scenario: Can not delete a role assigned to a user
	Given I have a valid role assigned to a user
	When I call the Delete Role API for that role asking for an exception if the role is in use
	Then a ProviderException is thrown
	And I can still retrieve the role using the GetAllRoles API

@Role_management
Scenario: The user can check if a give role exists using the Role API
	Given I have a valid role name
	When I call the RoleExists API
	Then the API response is true

@Role_management
Scenario: The user can verify that an invalid row does'nt exist
	Given I have an invalid role name
	When I call the RoleExists API on that name
	Then the API response is false

@User_roles
Scenario: A user can be assigned to one or more roles
	Given I have a valid CouchDbMembershipUser
	When I assign a role to a user
	Then I can check that the user has the role

@User_roles
Scenario: A user has been assigned to one or more roles and I need to know which are those roles
	Given I have a valid CouchDbMembershipUser with roles assigned
	When I call the GetRolesForUser API
	Then I get a list of all the roles assigned to the user

@User_roles
Scenario: A user can be removed from a role
	Given I have a valid user with roles assigned
	When I call the RemoveFromRole API
	Then the user is not assigned to that role anymore

@User_roles
Scenario: Can retrieve all the users assigned to a role
	Given I have valid users with roles assigned
	When I call the GetUsersInRole API
	Then I get a list of all the users in that role