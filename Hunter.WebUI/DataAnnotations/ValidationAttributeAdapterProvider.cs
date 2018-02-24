// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.ComponentModel.DataAnnotations;
using Hunter.WebUI.DataAnnotations.Internal;
using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.Extensions.Localization;

namespace Hunter.WebUI.DataAnnotations
{
    /// <summary>
    /// Creates an <see cref="IAttributeAdapter"/> for the given attribute.
    /// </summary>
    public class ValidationAttributeAdapterProvider : IValidationAttributeAdapterProvider
    {
        public IAttributeAdapter GetAttributeAdapter(ValidationAttribute attribute, IStringLocalizer stringLocalizer)
        {
            if (attribute == null)
            {
                throw new ArgumentNullException(nameof(attribute));
            }

            switch (attribute)
            {
                case RegularExpressionAttribute item:
                    return new RegularExpressionAttributeAdapter(item, stringLocalizer);
                case MaxLengthAttribute item:
                    return new MaxLengthAttributeAdapter(item, stringLocalizer);
                case RequiredAttribute item:
                    return new RequiredAttributeAdapter(item, stringLocalizer);
                case CompareAttribute item:
                    return new CompareAttributeAdapter(item, stringLocalizer);
                case MinLengthAttribute item:
                    return new MinLengthAttributeAdapter(item, stringLocalizer);
                case StringLengthAttribute item:
                    return new StringLengthAttributeAdapter(item, stringLocalizer);
                case RangeAttribute item:
                    return new RangeAttributeAdapter(item, stringLocalizer);
                case CreditCardAttribute item:
                    return new DataTypeAttributeAdapter(item, "creditcard", stringLocalizer);
                case EmailAddressAttribute item:
                    return new DataTypeAttributeAdapter(item, "email", stringLocalizer);
                case PhoneAttribute item:
                    return new DataTypeAttributeAdapter(item, "phone", stringLocalizer);
                case UrlAttribute item:
                    return new DataTypeAttributeAdapter(item, "url", stringLocalizer);
                case FileExtensionsAttribute item:
                    return new FileExtensionsAttributeAdapter((FileExtensionsAttribute)attribute, stringLocalizer);
                default:
                    return null;
            }
            
        }
    };
}
