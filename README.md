# Website parser

Website was created to make extracting data from websites easier. \
Instead of creating methods / classes with parsing logic, you can describe way to parse website using attributes.\
\
Important: Order of attributes matters.

## Usage

Parsing:
```csharp
string html;
// ...
UserInfo johnInfo = WebContentParser.Parse<UserInfo>(html);
```

Model class:
```csharp
class UserInfo
{
    [Selector("#personName")]
    public string Name { get; set; }

    [Selector("#personBirdthDate")]
    [Converter(typeof(DateTimeConverter))]
    public DateTime BirdthDate { get; set; }

    [Selector("#personAbout")]
    [Regex("about me: (.*)")]
    public string Description { get; set; }
}
```

Input data:
```html
<html>
    <body>
        <div id="personName">John</div>
        <div id="personBirdthDate">23-10-2000</div>
        <div id="personAbout">about me: lorem ipsum dolor sit amet</div>
    </body>
</html>
```

## Lists
For parsing lists use `ListSelectorAttribute` which should be used on model class. \
Constructor's argument "selector" is parent of children which should be parsed. \
`ChildSelector` property is used to choose which try to parse.

### Example:

Parsing:
```csharp
string html;
// ...
IEnumerable<UserInfo> people = WebContentParser.ParseList<UserInfo>(html);
```

Model class:
```csharp
[ListSelector("people", ChildSelector = ".person")]
class UserInfo
{
    [Selector("#personName")]
    public string Name { get; set; }

    [Selector("#personBirdthDate")]
    [Converter(typeof(DateTimeConverter))]
    public DateTime BirdthDate { get; set; }

    [Selector("#personAbout")]
    [Regex("about me: (.*)")]
    public string Description { get; set; }
}
```

Input data:
```html
<html>
    <body>
        <div id="people">
            <div class="person">
                <div id="personName">John</div>
                <div id="personBirdthDate">19-01-2038</div>
                <div id="personAbout">about me: lorem ipsum dolor sit amet</div>
            </div>
            <div class="person">
                <div id="personName">Joe</div>
                <div id="personBirdthDate">01-01-1970</div>
                <div id="personAbout">about me: hello world</div>
            </div>
        </div>
    </body>
</html>
```

# Attributes

There are a few attributes:

## Selector
Properties without this attribute won't take part of parse process. \
Returns string \
\
Known restrictions:
- Don't use `:nth-of-type` pseudo class

## Regex
It uses System.Text.RegularExpressions.Regex to extract first matched group. \
The attribute above RegexAttribute have to returns string.\
\
I'll try to make it more flexible in the furure.

## Converter
Converts value using class which implements IConverter interface.\
You can create custon IConverter and provide it to ConverterAttribute.

## Debug
It displays actual value in output window.

## License
[MIT](https://choosealicense.com/licenses/mit/)