# Session 08 - Library Management System
## Part 1 - Abstract class

**Why is `LibraryItem` abstract?**
It describes what every library item has (Id, Title, Year) and must do (`DisplayInfo`), but "a generic item" is not a real thing
on a shelf. Only concrete kinds (Book, Magazine) make sense as objects. Abstract lets us share code and force every kind to
provide its own behavior.

**Why can't we write `new LibraryItem()`?**
An abstract class is incomplete (it contains members without a body), so the compiler forbids creating objects from it
(error CS0144). It exists only to be inherited.

**Abstract `DisplayInfo()` vs concrete `PrintBasicInfo()`?**
`DisplayInfo()` has no body: each derived class MUST override it with its own version (different behavior per type, resolved
at runtime = polymorphism). `PrintBasicInfo()` has a body: it is shared, identical for all items, and inherited as-is.

## Part 2 - IComparable<Book>

- **Negative** value: `this` comes **before** `other`.
- **Zero**: both have the same position (equal for ordering).
- **Positive** value: `this` comes **after** `other`.

`List<Book>.Sort()` uses the default comparer, `Comparer<Book>.Default`, which looks for `IComparable<Book>` on the type.
Because `Book` implements it, the list already knows how to compare two books, so no separate comparison function is needed.

## Part 3 - ICloneable

**Copying a reference vs cloning:** `Book b2 = b1;` copies only the *reference*: both variables point to the same object, so a
change through one is visible through the other. `Clone()` creates a *new object* with copied data, independent from the original.

**`Book clone = original;`** does not clone anything. There is still ONE object and two variable names for it, so changing the
title through `clone` also changes `original`.

**Why does `Clone()` return `object`?** `ICloneable` was designed in .NET 1.0, before generics, and it has to work for every
type, so the only common return type was `object`. The caller must cast: `(Book)original.Clone()`.

**Limitation / criticism:** the contract does not say whether the copy is *shallow* or *deep*, so callers cannot know if
nested objects are shared. Microsoft recommends not implementing `ICloneable` in public APIs. It also returns `object`
(no type safety).

## Part 4 - Static class

**Why can't it be instantiated?** A static class is compiled as `abstract sealed`: it cannot have instance members or an
instance constructor, and the compiler rejects `new` on it (CS0723).

**When is it appropriate?** For stateless helpers/utilities that only operate on their parameters (math helpers,
formatters, validators, extension methods).

**Static class vs normal class with static methods:** a normal class can still be instantiated and can contain instance
members/state; nothing stops someone from writing `new`. A static class *guarantees* at compile time that only static
members exist and that no object can be made. Extension methods must live in a static class.

## Part 5 - Sealed class

**What does sealed mean?** The class is final: no other class can inherit from it.

**Why prevent inheritance?** To protect invariants/security (a derived class could break rules), to state "this was not
designed for extension", and for possible performance gains (the compiler can call methods directly). `string` is sealed.

**Why is `PremiumLibraryCard : LibraryCard` invalid?** The compiler gives error CS0509 (cannot derive from sealed type).

**Can a sealed class inherit from another class?** Yes. (`sealed` limits what comes *after* it, not before.)

**Can a sealed class implement an interface?** Yes.

## Part 6 - Partial class

**What does `partial` do?** It lets one class definition be written across several files. The compiler merges all parts.

**Does it create two different `Book` classes?** No. It is ONE class, `Book`.

**Effect on runtime type?** None. The split exists only at compile time; `typeof(Book)` is a single type.

**Real-world use:** designer/generator code. For example Windows Forms / Entity Framework / source generators generate one
part (`Form1.Designer.cs`), and the developer writes the other part by hand, so regeneration never overwrites hand-written code.

## Part 7 - Operator overloading

**Why can operators be overloaded?** C# lets a type define static methods named `operator +`, `operator >`, ... The compiler
turns `a + b` into a call to that method.

**Why is it useful?** It makes code for mathematical/value-like types read naturally (`Money + Money`, `Vector * 2`).

**When does it hurt readability?** When the meaning is not obvious (what does `book + book` mean? a combined volume? a
list?), or when it has side effects/surprises.

**Why behave intuitively?** Readers assume `+` adds, `>` compares, and neither modifies its operands. A surprising operator
creates bugs.

**Decision for `+` (documented):** returns a NEW `Book` with `PageCount = left + right`, `Title = "A + B"`,
`Author = "X & Y"`, `Year = the later year`, `Id = 0` (not yet registered, Ids must be unique). Operands are not changed.

## Part 8 - Singleton (`LibraryConfiguration`)

**Why is the constructor private?** So no outside code can write `new LibraryConfiguration()`. Only the class itself decides
when an object is created.

**Why is the instance static?** A static member belongs to the type, so it can be reached without already having an object
(`LibraryConfiguration.Instance`) and there is exactly one per type.

**Why is the class sealed?** To stop anyone from deriving a subclass that could try to create another instance or change
the behavior; the single-instance guarantee stays airtight and the intent is explicit.

**How is a single instance guaranteed?** The only constructor is private, the only creation point is the `Lazy<T>` factory,
and `Lazy<T>` runs that factory once and caches the result. `Instance` always returns `Lazy.Value`.

**Multiple threads at the same time?** `LazyThreadSafetyMode.ExecutionAndPublication` makes `Lazy<T>` use a lock internally:
the first thread runs the factory, other threads wait, and all of them receive the same object. `Program.cs` proves it with
`Parallel.For` (50 parallel requests -> 1 distinct instance).
