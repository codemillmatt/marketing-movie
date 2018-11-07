# Movie Reviews with Free Azure Credits

Use your free Azure credits to create a movie review app!

So what are you waiting for - [get some free Azure now](https://msou.co/boi)!

## Have Azure Credits? Let's Increase Our Free Time!

When you're handed a bunch of [free credits for various services on Azure](https://msou.co/boi), you can do one of three things.

1. Create a world changing app.
1. Create an app that will make you billions of dollars.
1. Create something that will increase your free time.

And let's face it - there's no time like free time ... so let's create a movie review app that will make sure we never have to see a bad movie again! (Because who has time to have a bad time at the old cineplex?)

Ok ... so this is how we're going to set it all up.

* We're going to have an [Azure Cosmos DB](https://msou.co/boj) backend that stores movie reviews for us.
* Going to display those movie reviews with a Xamarin.Forms app - accessing the Azure Cosmos DB through a .NET SDK.
* But... we're only going to display some of those reviews to the general public - other, more premium reviews, are going to be witheld for only those people logged in via Azure AD B2C!
* Then we'll have an Azure Function that will get invoked via a web call. This function checks to see whether or not a user has been authenticated via Azure AD B2C, and then returns the appropriate permissions.

## Azure Cosmos DB

The first thing that you'll need to do is [create an Azure Cosmos DB](https://msou.co/bok). Follow the directions here in order to create a SQL API version.

Create a database named: `movies-and-reviews`

Then create a collection named: `movies` 

And another named `reviews`. When creating the `reviews` collection, set a [partition key](https://msou.co/bol) to be `\isPremium`.

Then go ahead and populate some data into each.

The movie records should look like:

```language-json
{
    "id": "7e457301-e889-43c7-b035-ee738905e400",
    "movieName": "Venom",
    "releaseDate": "5-OCT-2018",
    "_rid": "vcYgAM45eqYBAAAAAAAAAA==",
    "_self": "dbs/vcYgAA==/colls/vcYgAM45eqY=/docs/vcYgAM45eqYBAAAAAAAAAA==/",
    "_etag": "\"04002f7a-0000-0800-0000-5bc110030000\"",
    "_attachments": "attachments/",
    "_ts": 1539379203
}
```

And the review records should look like:

```language-json
{
    "id": "bd84ed9e-b0ec-49d9-bffd-5f208967df07",
    "movieId": "b404bb8f-f9c9-4389-9fa6-26aca20104fe",
    "isPremium": true,
    "reviewText": "Good stuff",
    "reviewDate": "12-OCT-2018",
    "_rid": "vcYgAK6T9FoBAAAAAAAACA==",
    "_self": "dbs/vcYgAA==/colls/vcYgAK6T9Fo=/docs/vcYgAK6T9FoBAAAAAAAACA==/",
    "_etag": "\"220002ec-0000-0800-0000-5bc117d80000\"",
    "_attachments": "attachments/",
    "_ts": 1539381208
}
```

## Xamarin.Forms

The next thing you want to do is create a [Xamarin.Forms app](https://msou.co/bom). Lucky you I've done all the hard work and put it into the `/src` directory of this repo.

One thing that I do want to call out is that the app is accessing Azure Cosmos DB directly [via an SDK](https://msou.co/bon). Which is awesome! It doesn't need the app to go through another webservice of any type of talk to the database.

## Azure AD B2C

Azure Active Directory Business 2 Consumer. Whew! This is what is going to let you have users create accounts and log in. I'm not going to kid you - there's a LOT to this. 

[Check out the docs here](https://msou.co/boo).

Check out an [ELEVEN PART BLOG SERIES](https://msou.co/bop) I wrote on B2C and mobile apps here.

## Azure Functions

Then there's the [Azure Functions](https://msou.co/boq).

There's super coolness going on here.

First off the function is invoked via an [HTTP trigger](https://msou.co/bor). In other words - it runs in response to a web call.

Secondly - [it's BOUND to Azure Cosmos DB](https://msou.co/bos) ... wait wha?? What does that mean? 

It means the Function news up it's own connection to Azure Cosmos DB and takes care of maintaining the connection. We - the developers - don't have to worry about it at all.

Crazy town ... I know.

## Summing It Up

So there you have it - a quick explanation of mashing 3 different Azure services together to make a movie review app with premium reviews ... and wrapping it up in a pretty bow with Xamarin.Forms. Any questions or comments - please reach out to me on Twitter at [@codemillmatt](https://twitter.com/codemillmatt)

