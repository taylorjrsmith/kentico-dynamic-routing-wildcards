# Dynamic Routing Wildcards
Dynamic Routing Wildcards for Kentico is an extension of <a href="github.com/KenticoDevTrev/DynamicRouting">KenticoDevTrev's Dynamic Routing Package</a>. In KenticoDevTrev's package you are able to link Views, Controllers and Pages to customisable routes in Kentico MVC. With this package you can now introduce parameters to your Urls. For example if I have a url */blog* but I want to filter my blog listing page to only show certain categories, I can add the url */blog/{category}*. This will route to the original page and add parameters to the route data to parse into your controller actions.

## Installation
### Installing on the Admin
1. Install the NuGet Package DynamicRouting.Kentico.Wildcards on your Kentico Admin. 
2. Go to modules and ensure that DynamicRouting.Kentico.Wildcards exist
3. That's it for the admin 🎉

### installing on the Presentation Layer
1. Install the same NuGet Package DynamicRouting.Kentico.Wildcards on your main MVC site.
2. In your Global.asax.cs add the following using statement

using DynamicRouting.Kentico.Wildcards.Helpers;

3. In your Application_Start() method add the following code

WildcardRoutingInitialiser.AddWildcards();

#### All Done!  🎉
You should now be able to just add url wildcards in your Kentico admin. 
**NOTE:** Remember to always start a new wildcard with /{urlslug}/ 