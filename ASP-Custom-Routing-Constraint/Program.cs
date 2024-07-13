using ASP_Custom_Routing_Constraint.RouteConstraints;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddRouting(options =>
{
    options.ConstraintMap.Add("even", typeof(EvenNumberRouteConstraint));
    options.ConstraintMap.Add("positiveOddDouble", typeof(PositiveOddDoubleNumberRouteConstraint));
});

var app = builder.Build();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
