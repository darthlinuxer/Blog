# Blog Rest API

This is a **.NET Core 8** Blog WebApi with an InMemory Db

## How To run:
```
docker-compose up -d
```
## Database Seed Data
| Login  | Password|  Role  |
|----- |------|------|
|admin | ChangeMe1$ | Editor |
|darthlinuxer | ChangeMe1$ | Writer |
|luke | ChangeMe1$ | Public |

After sucessfull login a JWT Token will be returned. Add it to the Swagger Authorization Section to use the endpoints.
http://localhost:8080/swagger

Some endpoints are only accessible by specific Roles. 

## Rules

There are only 3 possible user roles:
* Public: It is not a writer, cannot add posts, only read and comment them.
* Writer: Can add posts 
* Editor: Acts like a moderator

### Post Lifecycle

* All posts starts as Drafts .  
* Once a Writer finishes it, is moves the Post status to pending!
* Editors can see pending posts, and either approve or reject them. 
* Once a Post is approved by the Editor, it is then eligible to be published by the Writer
* If a Post is rejected by the Editor, the Writer can then move the Post to Draft and Edit it based on the Editors comments

PostStatus is an enum:
| Eligible Status | Value 
| -------------| --------
| draft | 0 |
| pending | 1 |
| approved | 2 |
| rejected | 3 |
| published | 4 |

# SQLite

Optionally, this Blog can be configured in the src/WebApi/Extensions/DbContextExtensions.cs to run with a local SQLite database. Just uncomment the code and run the migrations with 
```
dotnet ef database update --project src/webapi
```