# Book Reviews
As an avid reader, I like to track the books I've read throughout the year and gain insight into my habits.
I found that the existing tools for this tend to have a lot of functionality I never used, so
I wrote my own, pared-down version. The Book Reviews app allows users to:
- Search for books by their title, author, or ISBN
- Add, edit, and delete a book review
- Read and sort community reviews
- View statistics about the books read in a year
- List and sort a summary of books read in a year

## Technologies Used
This is written in C# using the .Net Framework. It has an MVC presentation layer and an MS SQL database.
Other tools used include:
- Google Books API for the book search
- Dapper ORM
- AutoMapper for DTOs
- Sass for styling
- Javascript/jQuery

I deliberately used vanilla CSS rather than a framework for the front end of this project, because I wanted to 
strengthen my design abilities. I improved on my ability to make a functional, responsive layout and organize the
styles sheet in a logical way. I also discovered that choosing a color palette and creating a logo is truly a skill
of its own.

## Usage
To use this project, simply clone the repository and set up a local MS SQL database named BookReviews. Then run the scripts
in the Entities folder of the BookReviews.Impl project to create the necessary tables.

## Examples
### Book search
<img src="https://github.com/justine-lorenc/BookReviews/assets/124648557/9e425187-0478-4a4d-97de-71bb5beb9355" alt="Screenshot of search" width="600"/>

### Book details and community reviews
<img src="https://github.com/justine-lorenc/BookReviews/assets/124648557/75b34f73-a267-4db5-bf14-91fd5c02d85a" alt="Screenshot of book details" width="600"/>

<img src="https://github.com/justine-lorenc/BookReviews/assets/124648557/b3367ab2-2a1e-4012-9e5d-002df6fa3c46" alt="Screenshot of community reviews" width="600"/>

### Review a book
<img src="https://github.com/justine-lorenc/BookReviews/assets/124648557/5e84990c-403c-4328-a8ca-5dc1a4977ad7" alt="Screenshot of review book form" width="200"/>

### Review statistics and summary
<img src="https://github.com/justine-lorenc/BookReviews/assets/124648557/793a2c98-46ef-4dae-b09e-ded0d2bcbf24" alt="Screenshot of review statistics" width="600"/>

<img src="https://github.com/justine-lorenc/BookReviews/assets/124648557/092d3976-b896-4177-a474-50696fde050b" alt="Screenshot of review summary" width="600"/>
