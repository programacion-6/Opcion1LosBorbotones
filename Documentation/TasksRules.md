# Tasks Rules
## Branch Naming Convention
**Category**
- feature: for adding, refactoring or removing a feature
- bugfix: is for fixing a bug
- hotfix:  is for changing code with a temporary solution and/or without following the usual process 
(usually because of an emergency)

**Naming**

After the category, there should be a "/" followed by the ID of the ticket you are working on.
After the reference, there should be another "/" followed by a title of the ticket. This part should be in "kebab-cased".

E.g.

`
git branch <category/ticketID/title-in-kebab-case>
`

## Commit Naming Convention

**Category**

- feat: for adding a new feature
- fix: for fixing a bug
- refactor: for changing code for performance or convenience purpose (e.g. readibility)
- chore: for everything else (writing documentation, formatting, adding tests, cleaning useless code etc.)

**Naming**

A commit message should start with a category of change, after the category, there should be a ":" announcing 
the commit description.

`
git commit -m '<category: do something
`