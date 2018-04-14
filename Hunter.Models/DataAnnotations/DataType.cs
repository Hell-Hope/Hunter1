using System;
using System.Collections.Generic;
using System.Text;

namespace Hunter.Models.DataAnnotations
{

    public enum DataType : int
    {

        //
        // 摘要:
        //     Represents a custom data type.
        Custom = 0,
        //
        // 摘要:
        //     Represents an instant in time, expressed as a date and time of day.
        DateTime = 1,
        //
        // 摘要:
        //     Represents a date value.
        Date = 2,
        //
        // 摘要:
        //     Represents a time value.
        Time = 3,
        //
        // 摘要:
        //     Represents a continuous time during which an object exists.
        Duration = 4,
        //
        // 摘要:
        //     Represents a phone number value.
        PhoneNumber = 5,
        //
        // 摘要:
        //     Represents a currency value.
        Currency = 6,
        //
        // 摘要:
        //     Represents text that is displayed.
        Text = 7,
        //
        // 摘要:
        //     Represents an HTML file.
        Html = 8,
        //
        // 摘要:
        //     Represents multi-line text.
        MultilineText = 9,
        //
        // 摘要:
        //     Represents an e-mail address.
        EmailAddress = 10,
        //
        // 摘要:
        //     Represent a password value.
        Password = 11,
        //
        // 摘要:
        //     Represents a URL value.
        Url = 12,
        //
        // 摘要:
        //     Represents a URL to an image.
        ImageUrl = 13,
        //
        // 摘要:
        //     Represents a credit card number.
        CreditCard = 14,
        //
        // 摘要:
        //     Represents a postal code.
        PostalCode = 15,
        //
        // 摘要:
        //     Represents file upload data type.
        Upload = 16

    }
}
