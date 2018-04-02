[![GitHub license](https://img.shields.io/github/license/codedek/CodeDek.Ini.svg)](https://github.com/codedek/CodeDek.Ini/blob/master/LICENSE)

# CodeDek.Ini

>A fully managed api written in C# for working with .ini configuration files.

## Usage

### __Write__ - Create an ini config file and save to local storage

  > One way

  ```CSharp
  var iniString = @"[Section1]
  key=value
  foo=bar
  fizz=bazz

  [Section2]
  name=Jon Doe
  age=0
  color=green
  ";

  var ini = IniDocument.Parse(iniString);

  IniDocument.Write("C:\\file.ini", ini);
  ```

  > Another

  ```csharp
  var ini = new Ini();

  ini.Add(new Section("Section1"));
  ini.Section("Section1").Add(new Property("key","value"));
  ini.Section("Section1").Add(new Property("foo","bar"));
  ini.Section("Section1").Add(new Property("fizz","bazz"));

  ini.Add(new Section("Section2"));
  ini.Section("Section2").Add(new Property("name","Jon Doe"));
  ini.Section("Section2").Add(new Property("age","0"));
  ini.Section("Section2").Add(new Property("color","green"));

  IniDocument.Write("C:\\file.ini", ini);
  ```

  > And yet another

  ```csharp
  var file = "C:\\file.ini";

  // If the file and property exists With this write option enabled it will update the value
  // if value is the same, nothing will be done
  // if the file does not exist, it will be created, then the section and property will be added
  IniDocument.Write(file, "Section1", "key", "value", WriteOption.UpdateExistingPropertyValue);

  IniDocument.Write(file, "Section1", "foo", "bar");

  IniDocument.Write(file, "Section1", "fizz", "bazz");
  ```

### __Read__ - Load a property's value or an ini config file into memory

  ```CSharp
  var result = IniDocument.Read("C:\\file.ini", "Section1", "key");
  // value

  var result1 = IniDocument.Read("C:\\file.ini", "Section1", "fizz");
  // bazz

  var ini = IniDocument.Load("C:\\file.ini");
  /*
    [Section1]
    key=value
    foo=bar
    fizz=bazz

    [Section2]
    name=Jon Doe
    age=0
    color=green
  */
  ```
