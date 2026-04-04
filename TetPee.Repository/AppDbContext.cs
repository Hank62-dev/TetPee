using Microsoft.EntityFrameworkCore;
using TetPee.Repository.Entity;

namespace TetPee.Repository;

public class AppDbContext : DbContext
{
    public static Guid userId = Guid.NewGuid();
    public static Guid userId2 = Guid.NewGuid();
    public static Guid Cate1 = Guid.NewGuid();
    public static Guid Cate2 = Guid.NewGuid();
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<Seller> Sellers { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<ProductStorage> ProductStorages { get; set; }
    public DbSet<Storage> Storages { get; set; }
    public DbSet<Cart> Carts { get; set; }
    public DbSet<Inventory> Inventories { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderDetail> OrderDetails { get; set; } 
    public DbSet<ProductCategory> ProductCategories { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<CartDetail> CartDetails { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //ham nay giup dat cac conflict của entity trong db
        modelBuilder.Entity<User>(builder =>
        {
            builder.Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(255);
            
            builder.HasIndex(u => u.Email)
                .IsUnique(); 
            
            builder.Property(u => u.FirstName)
                .IsRequired()
                .HasMaxLength(100);
            
            // LastName - required, max 100 characters
            builder.Property(u => u.LastName)
                .IsRequired()
                .HasMaxLength(100);
            
            // ImageUrl - nullable, max 500 characters (URL)
            builder.Property(u => u.ImageUrl)
                .HasMaxLength(500);
            
            // PhoneNumber - nullable, max 20 characters
            builder.Property(u => u.PhoneNumber)
                .HasMaxLength(20);
            
            // HashedPassword - required, max 500 characters
            builder.Property(u => u.HashedPassword)
                .IsRequired()
                .HasMaxLength(500);
            
            builder.Property(u => u.Role)
                .IsRequired()
                .HasMaxLength(20)
                .HasDefaultValue("User");
            
            // Relationship: User has one Seller (one-to-one)
            builder.HasOne(u => u.Seller)
                .WithOne(s => s.User)
                .HasForeignKey<Seller>(s => s.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            
            builder.HasOne(u => u.Cart)
                .WithOne(s => s.User)
                .HasForeignKey<Cart>(s => s.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            
            // DeleteBehavior.Cascade: Khi một User bị xóa, thì Seller liên quan cũng sẽ bị xóa theo.
            // DeleteBehavior.Restrict: Ngăn chặn việc xóa một User nếu có Seller liên quan tồn tại.
                //(Tham chiếu tới PK tồn tại)
                // 1 Project còn Task thì không xoá được
            // DeleteBehavior.NoAction: Không thực hiện hành động gì đặc biệt khi User bị xóa. ( Gàn giống Restrict, xử lí ở DB)
            // DeleteBehavior.SetNull: Khi một User bị xóa, thì trường UserId trong bảng Seller sẽ được đặt thành NULL.
                //(Áp dụng khi trường FK cho phép NULL)

                List<User> users = new List<User>()
                {
                    new User()
                    {
                        Id = userId,
                        Email = "huyvog6226@gmail.com",
                        FirstName = "Huy",
                        LastName = "Vo",
                        HashedPassword = "hashed_password_1"
                    },
                    new User()
                    {
                        Id = userId2,
                        Email = "huyvog626@gmail.com",
                        FirstName = "Huy",
                        LastName = "Vo",
                        HashedPassword = "hashed_password_1"
                    }
                };
                for (int i = 0; i <= 1000; i++)
                {
                    var newUser = new User()
                    {
                        Id = Guid.NewGuid(),   
                        Email = "huyvog6226" + i+"@gmail.com",
                        FirstName = "Huy" + i,
                        LastName = "Vo" + i,
                        HashedPassword = "hashed_password_" + i
                    };
                    users.Add(newUser);
                }

                builder.HasData(users);

        });

        modelBuilder.Entity<Seller>(builder =>
        {
            builder.Property(s => s.TaxCode)
                .IsRequired()
                .HasMaxLength(20);
            
            builder.Property(s => s.CompanyName)
                .IsRequired()
                .HasMaxLength(100);
            
            builder.Property(s => s.CompanyAddress)
                .IsRequired()
                .HasMaxLength(100);

            var seller = new List<Seller>()
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    TaxCode = "TAXCODE123",
                    CompanyName = "TAXCODE123",
                    CompanyAddress = "TAXCODE123",
                    UserId = userId
                }
            };
            builder.HasData(seller);
        });

        modelBuilder.Entity<Category>(builder =>
        {
            builder.Property(c => c.Name)
                .IsRequired();
            
            var categories = new List<Category>()
            {
                new Category()
                {
                    Id = Cate1,
                    Name = "Áo"
                },
            
                new Category()
                {
                    Id = Cate2,
                    Name = "Áo"
                },
                new Category()
                {
                    Id = Guid.NewGuid(),
                    Name = "áo thun",
                    ParentId = Cate1,
                },
                new Category()
                {
                    Id = Guid.NewGuid(),
                    Name = "áo thun",
                    ParentId = Cate2,
                }
            };

            for (int i = 0; i <= 1000; i++)
            {
                var newCate = new Category()
                {
                    Id = Guid.NewGuid(),
                    Name = i.ToString(),
                    ParentId = Cate1
                };
                categories.Add(newCate);
            }
            
            builder.HasData(categories);
        });

        
        
        
        
    }
}