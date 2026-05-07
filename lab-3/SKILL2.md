# MVC CRUD Pages Skill

## When to use

Use this skill when:

- Creating Index (list) pages for any entity.
- Creating Details pages.
- Creating Create/Edit form pages.
- Wiring controller actions and views for CRUD in this ASP.NET MVC project.

## Controller actions

Add actions in the relevant controller. Use F1DbContext and EF Core queries.

### Index

- Query the entity list.
- Include navigation properties needed by the view.

Example:

```csharp
[HttpGet("")]
public IActionResult Index()
{
    var items = _db.Drivers
        .Include(d => d.Constructor)
        .ToList();

    return View(items);
}
```

### Details

- Load a single entity by id.
- Include navigation properties needed by the view.
- Return NotFound if missing.

```csharp
[HttpGet("{id:int}")]
public IActionResult Details(int id)
{
    var item = _db.Drivers
        .Include(d => d.Constructor)
        .FirstOrDefault(d => d.Id == id);

    if (item is null)
    {
        return NotFound();
    }

    return View(item);
}
```

### Create (GET)

- Prepare any dropdown lists for foreign keys.
- Return the empty view.

```csharp
[HttpGet("create")]
public IActionResult Create()
{
    ViewData["ConstructorId"] = new SelectList(_db.Constructors, "Id", "Name");
    return View();
}
```

### Create (POST)

- Validate ModelState.
- Add entity and save.
- Redirect to Index on success.

```csharp
[HttpPost("create")]
[ValidateAntiForgeryToken]
public IActionResult Create(Driver driver)
{
    if (!ModelState.IsValid)
    {
        ViewData["ConstructorId"] = new SelectList(_db.Constructors, "Id", "Name", driver.ConstructorId);
        return View(driver);
    }

    _db.Drivers.Add(driver);
    _db.SaveChanges();
    return RedirectToAction(nameof(Index));
}
```

### Edit (GET)

- Load the entity by id.
- Prepare dropdowns.
- Return NotFound if missing.

```csharp
[HttpGet("edit/{id:int}")]
public IActionResult Edit(int id)
{
    var driver = _db.Drivers.Find(id);
    if (driver is null)
    {
        return NotFound();
    }

    ViewData["ConstructorId"] = new SelectList(_db.Constructors, "Id", "Name", driver.ConstructorId);
    return View(driver);
}
```

### Edit (POST)

- Validate ModelState.
- Update entity and save.
- Redirect to Index on success.

```csharp
[HttpPost("edit/{id:int}")]
[ValidateAntiForgeryToken]
public IActionResult Edit(int id, Driver driver)
{
    if (id != driver.Id)
    {
        return BadRequest();
    }

    if (!ModelState.IsValid)
    {
        ViewData["ConstructorId"] = new SelectList(_db.Constructors, "Id", "Name", driver.ConstructorId);
        return View(driver);
    }

    _db.Drivers.Update(driver);
    _db.SaveChanges();
    return RedirectToAction(nameof(Index));
}
```

## Views and tag helpers

Follow patterns used in existing list/detail views:

- Index pages use a list or table with links to Details.
- Details pages show primary fields and related info.

### Index view

- Model: IEnumerable<Entity>
- Use `asp-action` and `asp-route-id` for links.

```cshtml
@model IEnumerable<F1_Fantasy_liga.Models.Driver>

<h1>Drivers</h1>

<table>
    <thead>
        <tr>
            <th>Name</th>
            <th>Constructor</th>
            <th></th>
        </tr>
    </thead>
    <tbody>
    @foreach (var item in Model)
    {
        <tr>
            <td>@item.Name @item.Surname</td>
            <td>@item.Constructor?.Name</td>
            <td>
                <a asp-action="Details" asp-route-id="@item.Id">Details</a>
                <a asp-action="Edit" asp-route-id="@item.Id">Edit</a>
            </td>
        </tr>
    }
    </tbody>
</table>

<a asp-action="Create">Create new</a>
```

### Create/Edit form view

- Model: Entity
- Use tag helpers: `asp-for`, `asp-action`.
- Add validation summary and field-level validation.
- Add a Back to list link.

```cshtml
@model F1_Fantasy_liga.Models.Driver

<h1>Edit Driver</h1>

<form asp-action="Edit" method="post">
    <div asp-validation-summary="ModelOnly"></div>

    <input type="hidden" asp-for="Id" />

    <div>
        <label asp-for="Name"></label>
        <input asp-for="Name" />
        <span asp-validation-for="Name"></span>
    </div>

    <div>
        <label asp-for="ConstructorId"></label>
        <select asp-for="ConstructorId" asp-items="ViewBag.ConstructorId"></select>
        <span asp-validation-for="ConstructorId"></span>
    </div>

    <button type="submit">Save</button>
</form>

<a asp-action="Index">Back to list</a>

@section Scripts {
    <partial name="_ValidationScriptsPartial" />
}
```

## Validation

- Use data annotations on model classes.
- In POST actions, check `ModelState.IsValid`.
- Include validation summary and validation spans in the view.
- Add `_ValidationScriptsPartial` for client-side validation.

## Navigation links

- Use `asp-action` and `asp-route-id`.
- Use the existing list and details views as layout examples.

## Dropdowns for foreign keys

- Use `SelectList` for FK values.
- Provide selected value on validation error or edit.

Example:

```csharp
ViewData["ConstructorId"] = new SelectList(_db.Constructors, "Id", "Name", model.ConstructorId);
```

```cshtml
<select asp-for="ConstructorId" asp-items="ViewBag.ConstructorId"></select>
```
