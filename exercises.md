# Northwind LINQ Exercises

These exercises help you practice writing Entity Framework Core LINQ queries against the Northwind database. For each exercise, add a new `ExecuteExerciseN` method to `Northwind.Application/Application.cs` (following the same style as the existing examples), wire it up from `Program.Main`, and run it with:

```bash
dotnet run --project Northwind.Application
```

Each exercise gets harder than the previous one. Try to solve it before peeking at the entity classes in `Northwind.Entities/`.

---

## Exercise 1 — List all categories

**Difficulty:** Beginner

Write a query that retrieves every row from the `Categories` table and prints the **category name** and **description** of each one to the terminal.

**Expected output (example):**

```
Beverages - Soft drinks, coffees, teas, beers, and ales
Condiments - Sweet and savory sauces, relishes, spreads, and seasonings
Confections - Desserts, candies, and sweet breads
...
```

**Hints:**
- Start from `context.Categories`.
- A simple `select` is enough — no filtering, no ordering.

---

## Exercise 2 — Find discontinued products

**Difficulty:** Easy

Some products in the catalog are no longer for sale. Write a query that retrieves only the products where `Discontinued` is `true`, and prints the **product id**, **product name**, and **unit price** for each one.

**Expected output (example):**

```
5  - Chef Anton's Gumbo Mix          - $21.35
9  - Mishi Kobe Niku                 - $97.00
17 - Alice Mutton                    - $39.00
...
```

**Hints:**
- Use a `where` clause on `context.Products`.
- Format the price as currency with `:c` in your interpolated string.

---

## Exercise 3 — German customers, sorted by company name

**Difficulty:** Medium

Write a query that retrieves all customers whose `Country` is `"Germany"`, sorted alphabetically by **company name**. For each customer, print the **customer id**, **company name**, **contact name**, and **city**.

**Expected output (example):**

```
ALFKI - Alfreds Futterkiste     - Maria Anders     - Berlin
BLAUS - Blauer See Delikatessen - Hanna Moos       - Mannheim
DRACD - Drachenblut Delikatessen - Sven Ottlieb    - Aachen
...
```

**Hints:**
- Combine `where` and `orderby` in the same query.
- Project into an anonymous type with only the four columns you need — don't pull the whole entity into memory.

---

## Exercise 4 — Products with their category name

**Difficulty:** Harder

A `Product` belongs to a `Category` through the `CategoryId` foreign key, and the `Product` entity has a navigation property to `Category`. Write a query that lists every product together with the **name of the category it belongs to**, sorted first by **category name** and then by **product name**.

For each row, print the **category name**, **product name**, and **units in stock**.

**Expected output (example):**

```
Beverages   - Chai                     - 39 in stock
Beverages   - Chang                    - 17 in stock
Beverages   - Chartreuse verte         - 69 in stock
...
Condiments  - Aniseed Syrup            - 13 in stock
...
```

**Hints:**
- You can either use `Include(p => p.Category)` and then access `p.Category.CategoryName`, or write an explicit `join` between `context.Products` and `context.Categories`.
- `orderby` can take more than one key, separated by commas.

---

## Exercise 5 — Top 5 customers by total order value

**Difficulty:** Challenging

Find the **5 customers who have spent the most money** across all of their orders, and show their **company name**, **number of orders**, and **total amount spent** (sorted from highest spender to lowest).

The total value of an order line in `OrderDetails` is calculated as:

```
UnitPrice * Quantity * (1 - Discount)
```

You will need to combine `Customers`, `Orders`, and `OrderDetails`.

**Expected output (example):**

```
1. QUICK-Stop                  - 28 orders - $117,483.39
2. Ernst Handel                - 30 orders - $104,874.98
3. Save-a-lot Markets          - 31 orders -  $96,978.74
4. Rattlesnake Canyon Grocery  - 18 orders -  $51,097.80
5. Hungry Owl All-Night Grocers - 19 orders - $49,979.91
```

**Hints:**
- Start from `context.Customers` and use the `Orders` navigation property, or join the three tables explicitly.
- Use `GroupBy` on the customer (or project per customer with a nested `Sum`).
- Inside the aggregation you'll need a nested `Sum` over `OrderDetails`.
- Finish with `OrderByDescending(...)` and `Take(5)`.
- Watch out: `Discount` is a `float`, so cast carefully when multiplying with `decimal` `UnitPrice`.

---

## Tips for all exercises

- Print the generated SQL for any query with `query.ToQueryString()` — it's a great way to confirm the database is doing the work, not your C# code.
- Prefer projecting into anonymous types over returning full entities; it keeps the SELECT list small.
- Remember to `using var context = northwindContextFactory.CreateDbContext();` at the top of each exercise method, just like the existing examples.
