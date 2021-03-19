// https://www.meziantou.net/entity-framework-core-history-audit-table.htm
using System;
using System.Threading.Tasks;
using audittable;
using Microsoft.EntityFrameworkCore;

namespace ConsoleApp32
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            using (var context = new BloggingContext())
            {
                await context.Database.EnsureDeletedAsync();
                await context.Database.EnsureCreatedAsync();

                // Insert a row
                var blog = new Blog();
                blog.Url = "https://www.meziantou.net";
                context.Blogs.Add(blog);

                var post = new Post();
                post.Blog = blog;
                post.Title = "test";
                context.Posts.Add(post);

                var post2 = new Post();
                post2.Blog = blog;
                post2.Title= "TEST 23";
                context.Posts.Add(post2);
                await context.SaveChangesAsync();

                // Update the first customer
                post.Title = "a";
                await context.SaveChangesAsync();

                // Delete the customer
                context.Posts.Remove(post);
                await context.SaveChangesAsync();

                var audits = await context.Audits.ToListAsync();
                foreach (var audit in audits)
                {
                    Console.WriteLine("------------");
                    Console.WriteLine("Table: " + audit.TableName);
                    Console.WriteLine("KeyValues: " + audit.KeyValues);
                    Console.WriteLine("OldValues: " + audit.OldValues);
                    Console.WriteLine("NewValues: " + audit.NewValues);
                }
            }

            /*
------------
Table: Posts
KeyValues: {"PostId":1}
OldValues:
NewValues: {"Content":null,"Title":"test","BlogId":1}
------------
Table: Blogs
KeyValues: {"BlogId":1}
OldValues:
NewValues: {"Rating":0,"Url":"https://www.meziantou.net"}
------------
Table: Posts
KeyValues: {"PostId":1}
OldValues: {"Title":"test"}
NewValues: {"Title":"a"}
------------
Table: Posts
KeyValues: {"PostId":1}
OldValues: {"BlogId":1,"Content":null,"Title":"a"}
NewValues:
            */
        }
    }











}
