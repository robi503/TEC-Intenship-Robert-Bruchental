Changes made:

- made CRUD operations possible for all entities from the domain(except position)
- added dependency injection for:
	* db connection string in the ApiApp
	* HTTP client configuration named 'ApiWithJwt' in the WebApp
- made the controllers operations asynchronous
- added some form validation
- added data transfer object called 'PersonCreate' to use when creating and updating a person
- added middleware for handling JWT