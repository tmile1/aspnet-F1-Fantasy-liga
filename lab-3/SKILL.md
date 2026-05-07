# Entity Framework Skill

## When to use

Use this skill when:

- Adding or modifying EF model classes.
- Configuring or updating DbContext.
- Generating or applying migrations.
- Troubleshooting EF Core migration or runtime errors.

## Step-by-step: add a new model class with EF annotations

1. Create a new model class in Models.
2. Add a primary key with [Key].
3. Add required fields, types, and defaults.
4. Add foreign key properties with [ForeignKey("NavigationName")].
5. Add navigation properties and collections:
   - 1-N: use virtual ICollection<T> on the principal.
   - N-1: use virtual T on the dependent.
   - M-N: add ICollection<T> on both sides.

Example:

```csharp
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class ExampleChild
{
    [Key]
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    [ForeignKey("Parent")]
    public int ParentId { get; set; }

    public virtual ExampleParent Parent { get; set; }
}
```

## Configure relationships in DbContext.OnModelCreating

1. Open Data/F1DbContext.cs.
2. Add Fluent API configuration when needed (especially for M-N or custom tables).
3. Keep conventions for simple 1-N if no customization is required.

Example for M-N join table:

```csharp
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    base.OnModelCreating(modelBuilder);

    modelBuilder.Entity<FantasyTeam>()
        .HasMany(ft => ft.Drivers)
        .WithMany(d => d.FantasyTeams)
        .UsingEntity(j => j.ToTable("FantasyTeamDrivers"));
}
```

## Generate and apply migrations

1. Build the project.
2. Add migration:
   - `dotnet ef migrations add <MigrationName>`
3. Update database:
   - `dotnet ef database update`

If the solution has multiple projects, run with explicit parameters:

- `dotnet ef migrations add <MigrationName> --startup-project <WebProject> --context F1DbContext`
- `dotnet ef database update --startup-project <WebProject> --context F1DbContext`

## Common errors and fixes

### Cascade delete cycle detected

Error: "The DELETE statement conflicted with the REFERENCE constraint" or a message about cascade cycles.

Fix:

- Set delete behavior to Restrict or NoAction in OnModelCreating.

```csharp
modelBuilder.Entity<ExampleChild>()
    .HasOne(c => c.Parent)
    .WithMany(p => p.Children)
    .HasForeignKey(c => c.ParentId)
    .OnDelete(DeleteBehavior.Restrict);
```

### Decimal precision warnings

Warning: "The property 'X' on entity type 'Y' is of type 'decimal' with no precision configured".

Fix:

- Configure precision in OnModelCreating.

```csharp
modelBuilder.Entity<Driver>()
    .Property(d => d.Price)
    .HasPrecision(10, 2);
```

### Missing Microsoft.EntityFrameworkCore.Design

Error: "Your startup project doesn't reference Microsoft.EntityFrameworkCore.Design".

Fix:

- Add the package to the startup project.
- Rebuild the solution.

### No DbContext found

Error: "No DbContext was found in assembly".

Fix:

- Ensure F1DbContext is public.
- Use `--context F1DbContext` in the command.

### Connection string not found

Error: "A connection string named 'F1FantasyDb' could not be found".

Fix:

- Add it to appsettings.json and verify Program.cs uses the same name.
