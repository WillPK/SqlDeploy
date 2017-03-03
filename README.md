# SqlDeploy
SqlDeploy is a console app used to automated running sql scripts in a database. It keeps the versions deployed to the database and loads from a folder all the new scripts it needs to run.

file pattern = @"^(\d+)\.\s(\w|\s|\.)+.sql$";
