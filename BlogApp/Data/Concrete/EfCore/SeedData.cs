using BlogApp.Entity;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Data.Concrete.EfCore
{
    public static class SeedData
    {
        public static void FillTestData(IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope()){
                var context = scope.ServiceProvider.GetRequiredService<BlogContext>();

                if(context != null){

                    if(context.Database.GetPendingMigrations().Any())
                        context.Database.Migrate();

                    if(!context.Tags.Any()){
                        context.Tags.Add( new Tag {Text = "Web Programming", Url="web-programming", Color = TagColors.warning});
                        context.Tags.Add( new Tag {Text = "Backend", Url="backend", Color = TagColors.success});
                        context.Tags.Add( new Tag {Text = "Frontend", Url="frontend", Color = TagColors.danger});
                        context.Tags.Add( new Tag {Text = "Fullstack", Url="fullstack", Color = TagColors.success});
                        context.Tags.Add( new Tag {Text = "php", Url="php", Color = TagColors.primary});
                        context.SaveChanges();
                    }

                    if(!context.Users.Any()){
                        context.Users.Add( new User {
                                UserName = "metehanpirim",
                                FullName="Metehan Pirim",
                                Email="metehanpirim@gmail.com",
                                Password="admin123", 
                                Image = "user1.jpg",        
                            });
                        context.Users.Add( new User { UserName = "aliveli",
                                FullName="Ali Veli",
                                Email="aliveli@gmail.com",
                                Password="admin123",
                                Image = "user2.jpg"
                            } );
                        context.SaveChanges();
                    }

                    if(!context.Posts.Any()){
                        context.Posts.Add(new Post { 
                            Title = "Asp.net Core",
                            Url="aspnet-core",
                            Content = "Asp.net core course content.",
                            Description = "Asp.net core course content.",
                            PublishedOn = DateTime.Now.AddDays(-5),
                            Image = "1.jpg",
                            IsActive = true,
                            Tags = context.Tags.Take(3).ToList(),
                            UserId = 1,
                            Comments = new List<Comment>() {
                                new Comment() { 
                                    Text = "This course is one of a kind!",
                                    PublishedOn = DateTime.Now.AddDays(-3),
                                    UserId = 1
                                },
                                new Comment() {
                                    Text = "Not good not bad.",
                                    PublishedOn = DateTime.Now.AddDays(-13),
                                    UserId = 2
                                }
                            }
                        });

                        context.Posts.Add(new Post { 
                            Title = "php",
                            Url="php",
                            Content = "php lessons.",
                            Description = "php lessons.",
                            PublishedOn = DateTime.Now.AddDays(-10),
                            Image = "2.jpg",
                            IsActive = true,
                            Tags = context.Tags.Take(2).ToList(),
                            UserId = 1
                        });

                        context.Posts.Add(new Post { 
                            Title = "Django",
                            Url="django",
                            Content = "Django lessons.",
                            Description = "Django lessons.",
                            PublishedOn = DateTime.Now.AddDays(-20),
                            Image = "3.jpg",
                            IsActive = true,
                            Tags = context.Tags.Take(4).ToList(),
                            UserId = 2
                        });

                        context.Posts.Add(new Post { 
                            Title = "Web Programming",
                            Url="web-programming",
                            Content = "Web programming content.",
                            PublishedOn = DateTime.Now.AddDays(-15),
                            Image = "4.jpg",
                            IsActive = true,
                            Tags = context.Tags.Take(4).ToList(),
                            UserId = 1
                        });

                        context.Posts.Add(new Post { 
                            Title = "Blockchain",
                            Url="blockchain",
                            Content = "All about blockchain.",
                            PublishedOn = DateTime.Now.AddDays(-40),
                            Image = "5.jpg",
                            IsActive = true,
                            Tags = context.Tags.Take(4).ToList(),
                            UserId = 2
                        });

                        context.SaveChanges();
                    }
                    
                }
            }
            GC.Collect();
            
        }
    }
}