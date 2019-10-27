# Website parser

Website was created to make extracting data from websites easier. \
Instead of creating methods / classes with parsing logic, you can describe your properties using attributes.\
\
Important: Order of attributes matters.

## Nuget
`
PM> Install-Package WebsiteParser -Version 1.0.6
`\
\
[https://www.nuget.org/packages/WebsiteParser/1.0.6](https://www.nuget.org/packages/WebsiteParser/1.0.6)

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

Attributes are handled in same order they was added e.g.
```csharp
[Selector("#addDate")]
[Remove("Add date: ", RemoverValueType.Text)]
[Converter(typeof(DateTimeConverter))]
public DateTime AddDate { get; set; }
```
`Selector` will extract markup's text as string, `Remove` will change received string and pass it to converter, which will change string's type to DateTime.

## Creating custom attribute
You can create your own parser attribute simply implementing `WebsiteParser.Attributes.Abstract.IParserAttribute` interface. Keep in mind that input type depends on above attribute's output.

## Selector
Properties without this attribute won't take part of parse process. \
Returns string \
\
Optional properties:
- Attribute - when is set, Selector will extract attribute's value, not markup content
- NotParseWhenNotFound - if true, selector pointing a not existing element will skip to another property without throwing exception

Known restrictions:
- Don't use `:nth-of-type` pseudo class

## Regex
It uses System.Text.RegularExpressions.Regex to extract first matched group. \
Returns string\
Expects string
\
I'll try to make it more flexible in the furure.

## Remove
Depending on `RemoverValueType` enum it removes text or regex match from actual value. Provided value have to be string.
Returns string\
Expects string

## Format
Uses `System.String.Format` method to allow formating text (e.g. appending text).\
Returns string\
Expects string

## Converter
Converts value using class which implements IConverter interface.\
You can create custon IConverter and provide it to ConverterAttribute.

## Debug
It displays actual value in output window.

## License
[MIT](https://choosealicense.com/licenses/mit/)
