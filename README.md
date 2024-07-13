# Description
`ASP.NET Core` has constraints for `Route` attribute

**For example:** action in controller
```csharp
[HttpGet]
[Route("get/{number:int}")]
public IActionResult GetEven(int number)
{
    return Ok(number);
}
```

To reach this endpoint request must have `number` parameter as `int` <br>

<br>

But what if we want to do a more complex check? <br>
Then we need **custom route constraints**

## Custom route constraints
**Custom route constraints** - `class` that **implements `IRouteConstraint` interface**

Let's make an `EvenNumberRouteConstraint` class for instance

### 1 Creating a class

<details>
  <summary><code>C#</code> code implementation <code>EvenNumberRouteConstraint</code></summary>

  ```csharp
  public class EvenNumberRouteConstraint : IRouteConstraint
  {
      public bool Match(
          HttpContext? httpContext,
          IRouter? route,
          string routeKey,
          RouteValueDictionary values,
          RouteDirection routeDirection)
      {
          if (!values.TryGetValue(routeKey, out var routeValue))
          {
              return false;
          }
  
          var routeValueString = Convert.ToString(routeValue, CultureInfo.InvariantCulture);
  
          if (routeValueString is null)
          {
              return false;
          }
  
          if (!int.TryParse(routeValueString, out var routeNumber))
          {
              return false;
          }
  
          return routeNumber % 2 == 0;
      }
  }
  ```

  Method `Match` check input `values` by searching needed segment by `routeKey` (get `routeValue`) <br>
  Then converts: 
  - `routeValue` to a `string` (get `routeValueString`)
  - `routeValueString` to an `int` (get `routeNumber`)
  
  And make constraint logic: `routeNumber` must be **even**
  
</details>

### 2 Register
When register we need to set a `name` constraint. This `name` will be used for constrain <br>

<br>

`.NET` versions:
- `> 5`: `Program.cs`
  ```csharp
  builder.Services.AddRouting(options =>
  {
      options.ConstraintMap.Add("even", typeof(EvenNumberRouteConstraint));
  });
  ```
- `<= 5`: `Startup.cs` `ConfigureServices`
  ```csharp
  services.AddRouting(options =>
  {
      options.ConstraintMap.Add("even", typeof(EvenNumberRouteConstraint));
  });
  ```

### 3 Usage
Use custom constraint like others: `:constraintName`
```csharp
[HttpGet]
[Route("get/{number:int:even}")]
public IActionResult GetEven(int number)
{
    return Ok($"Number {number} is even");
}
```
## Addition
### 1 Order matters

- `[Route("get/{number:int:even}")]`:	first check `number` is `int` then that `number` is `even`
- `[Route("get/{number:even:int}")]`: first check `number` is `even` then that `number` is `int`

In case 2, if there is no check for `int` in constraint, there may be an `exception`

### 2 Constraint params != method params

`Constraint params` **can** conside with `method params` but not necessarily! <br>

<br>

When they are different `constraint params` can have **any** name
```csharp
[Route("get/{inputNumber:int}")]
public IActionResult Get(int number)
{
	return Ok();
}
```
In this case, `inputNumber` can just name the `Route` segment <br>
 `inputNumber` can't be used in method body

### 3 Current realisation
URL: `api/number/get/[number]` <br>

This project realises 2 custom constraints:
1. `even`: Possible `[number]` values:	`... -4 -2 0 2 4 6 ...`
2. `positiveOddDouble` Possible `[number]` values: `11 33 55 77 99`
