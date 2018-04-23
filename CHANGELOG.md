# Road map

- [x] A feature that has been completed
- [ ] A feature that has NOT yet been completed

Features that have a checkmark are complete and available for
download in the
[CI build].

# Change log

These are the changes to each version that has been officially released.

## 0.2

- new: Can now construct your ini in a Functional linq-like way. e.g.
```csharp
var sec = new Ini(new Section("Root",
                                  new Property("ID", "123"),
                                  new Property("Name", "Jon"),
                                  new Property("Gender", "true"),
                                  new Property("DateOfBirth", "1995-08-29T00:00:00")),
                  new Section("Root2",
                                  new Property("ID", "456"),
                                  new Property("Name", "Jane"),
                                  new Property("Gender", "false"),
                                  new Property("DateOfBirth", "1997-10-02T00:00:00")));
```
- new: Can now call IsNullOrEmpty on Ini and Section class to check your objects. e.g.
```csharp
var sec = null;
if(!Section.IsNullOrEmpty(sec))
   doSomething();
```

## 0.1

- [x] Initial release

# Development
## 0.0

- [x] Initial commit
