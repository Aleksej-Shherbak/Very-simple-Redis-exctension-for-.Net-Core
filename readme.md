# Documentation

### Short description

This is very simple extension for .Net Core for Redis. The extension will add this code 

````
public static void SetObject(this IDistributedCache distributedCache, string key, object value, int minutes = 5)
{
    var options = new DistributedCacheEntryOptions(); 
    
    options.SetAbsoluteExpiration(TimeSpan.FromMinutes(minutes)); 
    
    distributedCache.SetString(key, JsonConvert.SerializeObject(value), options);
}

public static T GetObject<T>(this IDistributedCache distributedCache, string key)
{
    var value = distributedCache.GetString(key);

    return value == null ? default : JsonConvert.DeserializeObject<T>(value);
}
````

So you don't need write this every time. Extension gives you ability to save any object to redis. 
I use this with standard redis microsoft package `Microsoft.Extensions.Caching.Redis`.

### How to install

1) First you need install [Microsoft.Extensions.Caching.Redis](https://www.nuget.org/packages/Microsoft.Extensions.Caching.Redis)
2) In your `Startup.cs` use the following code to configure `Microsoft.Extensions.Caching.Redis`

````
services.AddDistributedRedisCache(options =>
{
    options.Configuration = '<redis connection string>';
});
````

3) Install package. Various of commands for installation are [here](https://www.nuget.org/packages/VerySimpleRedisExctension/)

### How to use 

This code is example of usage:

````
class Foo
{
    public string a = "Hello!!!";
    public double b = 213.666;

    public override string ToString()
    {
        return a + " " + b;
    }
}

// ...

class MyClass {

    private readonly IDistributedCache _cache;

    public MyClass(IDistributedCache cache)
    {
        _cache = cache
    }

    public void Test(){
        var foo = new Foo();
        Console.WriteLine(foo);
        _cache.SetObject("key", foo, 1);
        var cachedFoo = _cache.GetObject<Foo>("key");
        Console.WriteLine(cachedFoo);
    }
}


// ...

var mc = new MyClass();

mc.Test();

````

Output:

````
Hello!!! 213,666
Hello!!! 213,666
````

> Note that the default cache time is 5 minutes. But you can set time manually: `_cache.SetObject("key", foo, <number of minutes is here>);`