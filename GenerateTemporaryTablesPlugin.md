## Generate temporary tables based on selected queries ##
This plugin will generate temporary tables based on all the queries you have selected.
It will also generate multiple temporary tables in cases where your stored procedures return multiple result sets.

This is useful when you have a complicated query that you wish to store in a temporary table. For regular SELECT statements you can always `SELECT * INTO #temp` but in many cases you may want to insert from multiple sources. In those other cases this plugin will simply produce temporary table definitions with drop statement and insert the script in your current document.
### How to use: ###
  1. Select the query or queries that you wish to generate temporary tables for
  1. Select the plugin from the Peter Henell menu (or use keyboard shortcut default: CRTL+ALT+D).
  1. The plugin will EXECUTE the query and collect the first result and parse the meta data from it. **Do note that if the query is heavy then the result will require some time before appearing.**