# NewsPostsController Documentation

The `NewsPostsController` is a controller that handles API requests related to news posts in the Anchor News application. It interacts with the `NewsPostService` and `NewsApiService` to perform various CRUD operations and fetch news data from an external API.

## Class: NewsPostsController

### Inherits: Controller

The `NewsPostsController` class is a controller that handles API requests for news posts.

### Methods:

#### 1. GetAllNewsPosts

- HTTP Method: GET
- Route: `/api/NewsPosts/getAllNewsPost`
- Description: Retrieves all news posts from the database.

#### 2. GetNewsPost

- HTTP Method: GET
- Route: `/api/NewsPosts/getNewsPost`
- Description: Retrieves a specific news post by its ID.

#### 3. EditNewsPost

- HTTP Method: PUT
- Route: `/api/NewsPosts/editNewsPost`
- Description: Edits a news post with the specified ID.

#### 4. CreateNewsPost

- HTTP Method: POST
- Route: `/api/NewsPosts/createNewsPost`
- Description: Creates a new news post.

#### 5. DeleteNewsPost

- HTTP Method: DELETE
- Route: `/api/NewsPosts/deleteNewsPost`
- Description: Deletes a news post with the specified ID.

#### 6. GetNewsFromApi

- HTTP Method: GET
- Route: `/api/NewsPosts/getNewsFromApi`
- Description: Fetches news data from an external API.

#### 7. GetFetchedNews

- HTTP Method: GET
- Route: `/api/NewsPosts/getFetchedNews`
- Description: Retrieves fetched news data.


# CommentController Documentation

The `CommentController` is a controller that handles API requests related to comments in the Anchor News application. It interacts with the `CommentService` to perform operations such as retrieving comments, adding comments, and deleting comments.

## Class: CommentController

### Inherits: ControllerBase

The `CommentController` class is a controller that handles API requests for comments.

### Methods:

#### 1. GetComments

- HTTP Method: GET
- Route: `/api/comments`
- Description: Retrieves comments for a specific post by its ID.

#### 2. AddComment

- HTTP Method: POST
- Route: `/api/comments`
- Description: Adds a new comment to a post. This action requires authorization.

#### 3. DeleteComment

- HTTP Method: DELETE
- Route: `/api/comments/{id}`
- Description: Deletes a comment with the specified ID. This action requires authorization and is only accessible to users with the role "Admin".

# AuthController Documentation

The `AuthController` is a controller that handles API requests related to user authentication and authorization in the Anchor News application. It interacts with the `UserManager`, `RoleManager`, `UsersDbContext`, and `TokenService` to perform operations such as user registration, user login, role creation, and role assignment.

## Class: AuthController

### Inherits: ControllerBase

The `AuthController` class is a controller that handles API requests for user authentication and authorization.

### Methods:

#### 1. Register

- HTTP Method: POST
- Route: `/auth/register`
- Description: Registers a new user with the provided registration information. This action does not require authentication.

#### 2. Authenticate

- HTTP Method: POST
- Route: `/auth/login`
- Description: Authenticates a user and generates an access token for further authorization. This action does not require authentication.

#### 3. CreateRole

- HTTP Method: POST
- Route: `/auth/createRole`
- Description: Creates a new role with the specified name. This action does not require authentication. (FOR TESTING ONLY)

#### 4. AssignRole

- HTTP Method: POST
- Route: `/auth/assignRole`
- Description: Assigns a role to a user based on the user ID and role name. This action does not require authentication. (FOR TESTING ONLY)
