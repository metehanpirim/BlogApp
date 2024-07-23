using BlogApp.Data.Abstract;
using BlogApp.Data.Concrete;
using BlogApp.Data.Concrete.EfCore;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDbContextPool<BlogContext>( options => {
    var connectionString = builder.Configuration.GetConnectionString("sqlite");
    options.UseSqlite(connectionString);

    // var connectionString = builder.Configuration.GetConnectionString("mysql");
    // var version = new MySqlServerVersion(new Version(8,0,36));
    // options.UseMySql(connectionString, version);
});

builder.Services.AddScoped<IPostRepository, PostRepository>();
builder.Services.AddScoped<ITagRepository, TagRepository>();

var app = builder.Build();

//to use static files under wwwroot
app.UseStaticFiles();

SeedData.FillTestData(app);

app.MapControllerRoute(
    name: "post_details",
    pattern: "/posts/{slug}",
    defaults: new {controller = "Post", action = "Details"}
);

app.MapControllerRoute(
    name: "posts_by_tag",
    pattern: "/tags/{slug}",
    defaults: new {controller = "Post", action = "Index"}
);

app.MapControllerRoute(
    name: "Default",
    pattern: "{controller=Post}/{action=Index}/{id?}"
);

app.Run();
